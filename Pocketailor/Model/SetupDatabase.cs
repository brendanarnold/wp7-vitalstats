using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.IO;
using Pocketailor.Model.Conversions;

namespace Pocketailor.Model
{
    public class SetupDatabase
    {

        public static void InitialiseDB(AppDataContext db)
        {
            db.CreateDatabase();
            LoadConversions(db);
            db.SubmitChanges();
        }

        public static void EmptyDB(AppDataContext db)
        {
            // The order here is important so as to not leave hanging references
            db.Stats.DeleteAllOnSubmit(db.Stats);
            db.Profiles.DeleteAllOnSubmit(db.Profiles);
            db.DressSizes.DeleteAllOnSubmit(db.DressSizes);
            db.SubmitChanges();
        }

        public static void LoadConversions(AppDataContext db)
        {
            // The CsvReader classes are defined in Model.Conversions files to keep definitions for formats in same place
            ReadCsvFileIntoDb(db, new Model.Conversions.SuitCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.ShirtCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.TrousersCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.HatCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.DressCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.BraCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.HosieryCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.ShoesCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.SkiBootsCsvReader());
            ReadCsvFileIntoDb(db, new Model.Conversions.WetsuitCsvReader());
            
        }

        
 

        public class CsvLine
        {
            public CsvLine()
            {
                this.Measurements = new Dictionary<MeasurementId, double?>();
            }
            public Dictionary<MeasurementId, double?> Measurements;
            public RetailId Retailer;
            public RegionIds Region;
            public string SizeLetter;
            public string SizeNumber;
            public Gender Gender;
            public double? GetMeasurementOrNull(MeasurementId id)
            {
                double? d;
                if (this.Measurements.TryGetValue(id, out d))
                {
                    return d;
                }
                else
                {
                    return null;
                }
            }


        }


        public static void ReadCsvFileIntoDb(AppDataContext db, ICsvReader csvReader)
        {
            csvReader.Db = db;
            Uri uri = new Uri(AppConstants.CSV_DATA_DIRECTORY + csvReader.ConversionId.ToString() + ".txt", UriKind.Relative);
            var res = System.Windows.Application.GetResourceStream(uri);
            System.IO.StreamReader fh = new System.IO.StreamReader(res.Stream);

            int count = 0;
            List<string> headers = new List<string>();
            while (!fh.EndOfStream)
            {
                count++;
                string line = fh.ReadLine();
                // Read headers
                if (count == 1)
                {
                    headers = line.Split(AppConstants.CSV_DELIMITERS).ToList<string>();
                    continue;
                }
                // Skip commented lines
                if (line.StartsWith("#")) continue;
                var els = line.Split(AppConstants.CSV_DELIMITERS).ToList<string>();
                CsvLine csvLine = new CsvLine();
                for (int i = 0; i < els.Count; i++)
                {
                    switch (headers[i])
                    {
                        case "Retailer":
                            csvLine.Retailer = (RetailId)Enum.Parse(typeof(RetailId), els[i], true);
                            continue;
                        case "Region":
                            csvLine.Region = (RegionIds)Enum.Parse(typeof(RegionIds), els[i], true);
                            continue;
                        case "Gender":
                            csvLine.Gender = (Gender)Enum.Parse(typeof(Gender), els[i], true);
                            continue;
                        case "SizeLetter":
                            csvLine.SizeLetter = els[i];
                            continue;
                        case "SizeNumber":
                            csvLine.SizeNumber = els[i];
                            continue;
                        // Assume all remaining properties are numbers
                        default:
                            double? d;
                            if (els[i] == String.Empty) {
                                d = null;
                            } else {
                                d = double.Parse(els[i]);
                            }
                            MeasurementId mId = (MeasurementId)Enum.Parse(typeof(MeasurementId), headers[i], true);
                            csvLine.Measurements.Add(mId, d);
                            break;
                    }
                    csvReader.QueueWriteObj(csvLine);
                    if ((count % AppConstants.DB_OBJECT_BUFFER_BEFORE_WRITE) == 0)
                    {
                        db.SubmitChanges();
                    }
                    
                }
                db.SubmitChanges();

            }
        }

        




    }
}
