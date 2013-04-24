using Pocketailor.Model;
using PocketailorDatabaseCreator.Model.CsvReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model
{
    public class SetupDatabase
    {

        public static void LoadConversions(ConversionsDataContext db)
        {
            // The CsvReader classes are defined in Model.Conversions files to keep definitions for formats in same place
            ReadCsvFileIntoDb(db, new SuitCsvReader());
            ReadCsvFileIntoDb(db, new ShirtCsvReader());
            ReadCsvFileIntoDb(db, new TrousersCsvReader());
            ReadCsvFileIntoDb(db, new HatCsvReader());
            ReadCsvFileIntoDb(db, new DressCsvReader());
            ReadCsvFileIntoDb(db, new BraCsvReader());
            ReadCsvFileIntoDb(db, new HosieryCsvReader());
            ReadCsvFileIntoDb(db, new ShoesCsvReader());
            ReadCsvFileIntoDb(db, new SkiBootsCsvReader());
            ReadCsvFileIntoDb(db, new WetsuitCsvReader());
        }

 

        public static void ReadCsvFileIntoDb(ConversionsDataContext db, ICsvReader csvReader)
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
                            if (els[i] == String.Empty)
                            {
                                d = null;
                            }
                            else
                            {
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
