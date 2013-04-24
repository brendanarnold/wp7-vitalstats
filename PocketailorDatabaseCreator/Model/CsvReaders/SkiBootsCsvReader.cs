using Pocketailor.Model;
using Pocketailor.Model.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public class SkiBootsCsvReader : ICsvReader
    {
        public SkiBootsCsvReader()
        {
            this.ConversionId = ConversionId.SkiBootSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            this.Db.SkiBoots.InsertOnSubmit(new SkiBoots()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Gender = csvLine.Gender,
                FootLength = csvLine.GetMeasurementOrNull(MeasurementId.FootLength),
                FootWidth = csvLine.GetMeasurementOrNull(MeasurementId.FootWidth),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }

}
