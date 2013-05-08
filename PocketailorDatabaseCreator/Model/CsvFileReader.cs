﻿using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model
{
    public static class CsvFileReader
    {
        
        public static void ReadFileIntoDb(ConversionsDataContext db, ConversionId conversionId)
        {
            Uri uri = new Uri(AppConstants.CSV_DATA_DIRECTORY + conversionId.ToString() + ".txt", UriKind.Relative);
            var res = System.Windows.Application.GetResourceStream(uri);
            System.IO.StreamReader fh = new System.IO.StreamReader(res.Stream);

            int lineNum = 0;
            List<string> headers = new List<string>();
            Pocketailor.Model.Conversions.ConversionData cd = null;

            while (!fh.EndOfStream)
            {
                lineNum++;
                string line = fh.ReadLine();
                // Read headers
                if (lineNum == 1)
                {
                    headers = line.Split(AppConstants.CSV_DELIMITERS).ToList<string>();
                    continue;
                }
                // Skip commented lines
                if (line.StartsWith("#")) continue;
                // Skip empty lines
                if (line.Trim() == String.Empty) continue;
                var els = line.Split(AppConstants.CSV_DELIMITERS).ToList<string>();
                // We make the assumption that data for a particular retailer, gender, region, conversion are contiguous, 
                // so when any one of these change, we change the object
                CsvLine csvLine = new CsvLine();
                for (int i = 0; i < els.Count; i++)
                {
                    switch (headers[i])
                    {
                        case "Conversion":
                            csvLine.Conversion = (ConversionId)Enum.Parse(typeof(ConversionId), els[i], true);
                            continue;
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
                }
                // Take into account the first line
                if (cd == null)
                {
                    cd = new Pocketailor.Model.Conversions.ConversionData();
                    cd.Region = csvLine.Region;
                    cd.Retailer = csvLine.Retailer;
                    cd.Conversion = csvLine.Conversion;
                    cd.Gender = csvLine.Gender;
                }
                // See if need to write anew object to the database i.e. the dataset has changed
                if (csvLine.Region != cd.Region
                    || csvLine.Retailer != cd.Retailer
                    || csvLine.Conversion != cd.Conversion
                    || csvLine.Gender != cd.Gender)
                {
                    // Write to DB
                    cd.JsonifyData();
                    db.ConversionData.InsertOnSubmit(cd);
                    db.SubmitChanges();
                    // Create next database object
                    cd = new Pocketailor.Model.Conversions.ConversionData();
                    cd.Region = csvLine.Region;
                    cd.Retailer = csvLine.Retailer;
                    cd.Conversion = csvLine.Conversion;
                    cd.Gender = csvLine.Gender;
                }
                // Add the measurements for this line ...
                foreach (MeasurementId mId in csvLine.Measurements.Keys)
                {
                    if (!cd.Measurements.Keys.Contains(mId)) 
                        cd.Measurements.Add(mId, new List<double?>());
                    cd.Measurements[mId].Add(csvLine.Measurements[mId]);
                }

                // Write to db is obj count too high
                //if ((objNum % AppConstants.DB_OBJECT_BUFFER_BEFORE_WRITE) == 0)
                //{
                //    db.SubmitChanges();
                //}
            }
            // Write any remaining object to the DB
            if (cd != null)
            {
                cd.JsonifyData();
                db.ConversionData.InsertOnSubmit(cd);
                db.SubmitChanges();
            }
        }



    }
}