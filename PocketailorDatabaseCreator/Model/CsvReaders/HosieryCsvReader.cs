using Pocketailor.Model;
using Pocketailor.Model.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public class HosieryCsvReader : ICsvReader
    {
        public HosieryCsvReader()
        {
            this.ConversionId = ConversionId.HosierySize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            //this.Db.Hosiery.InsertOnSubmit(new Hosiery()
            //{
            //    Retailer = csvLine.Retailer,
            //    Region = csvLine.Region,
            //    Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
            //    Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
            //    InsideLeg = csvLine.GetMeasurementOrNull(MeasurementId.InsideLeg),
            //    SizeLetter = csvLine.SizeLetter,
            //    SizeNumber = csvLine.SizeNumber,
            //});
        }
    }
}
