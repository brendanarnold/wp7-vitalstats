using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{

    public class HatCsvReader : ICsvReader
    {
        public HatCsvReader()
        {
            this.ConversionId = ConversionId.HatSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            //this.Db.Hats.InsertOnSubmit(new Pocketailor.Model.Conversions.Hat()
            //{
            //    Retailer = csvLine.Retailer,
            //    Region = csvLine.Region,
            //    Head = csvLine.GetMeasurementOrNull(MeasurementId.Head),
            //    SizeLetter = csvLine.SizeLetter,
            //    SizeNumber = csvLine.SizeNumber,
            //});
        }
    }

}
