using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public class DressCsvReader : ICsvReader
    {
        public DressCsvReader()
        {
            this.ConversionId = ConversionId.DressSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            this.Db.DressSizes.InsertOnSubmit(new Pocketailor.Model.Conversions.DressSize()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Chest = csvLine.GetMeasurementOrNull(MeasurementId.Chest),
                Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
                Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }
}
