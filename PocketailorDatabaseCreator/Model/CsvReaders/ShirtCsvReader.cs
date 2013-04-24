using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public class ShirtCsvReader : ICsvReader
    {
        public ShirtCsvReader()
        {
            this.ConversionId = ConversionId.ShirtSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            this.Db.Shirts.InsertOnSubmit(new Pocketailor.Model.Conversions.Shirt()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Gender = csvLine.Gender,
                Chest = csvLine.GetMeasurementOrNull(MeasurementId.Chest),
                Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
                Neck = csvLine.GetMeasurementOrNull(MeasurementId.Neck),
                TorsoLength = csvLine.GetMeasurementOrNull(MeasurementId.TorsoLength),
                Sleeve = csvLine.GetMeasurementOrNull(MeasurementId.Sleeve),
                Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }


}
