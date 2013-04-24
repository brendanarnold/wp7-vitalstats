using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public class BraCsvReader : ICsvReader
    {
        public BraCsvReader()
        {
            this.ConversionId = ConversionId.BraSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            this.Db.Bras.InsertOnSubmit(new Pocketailor.Model.Conversions.Bra()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Chest = csvLine.GetMeasurementOrNull(MeasurementId.Chest),
                UnderBust = csvLine.GetMeasurementOrNull(MeasurementId.UnderBust),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }
}
