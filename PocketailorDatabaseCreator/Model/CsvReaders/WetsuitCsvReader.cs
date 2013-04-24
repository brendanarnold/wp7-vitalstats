using Pocketailor.Model;
using Pocketailor.Model.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public class WetsuitCsvReader : ICsvReader
    {
        public WetsuitCsvReader()
        {
            this.ConversionId = ConversionId.WetsuitSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            this.Db.Wetsuits.InsertOnSubmit(new Wetsuit()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Gender = csvLine.Gender,
                Height = csvLine.GetMeasurementOrNull(MeasurementId.Height),
                Chest = csvLine.GetMeasurementOrNull(MeasurementId.Chest),
                Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
                Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
                Weight = csvLine.GetMeasurementOrNull(MeasurementId.Weight),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }

}
