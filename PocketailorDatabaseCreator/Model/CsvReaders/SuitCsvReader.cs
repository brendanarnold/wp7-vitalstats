using Pocketailor.Model;
using Pocketailor.Model.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{

    public class SuitCsvReader : ICsvReader
    {
        public SuitCsvReader()
        {
            this.ConversionId = ConversionId.SuitSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            //this.Db.Suits.InsertOnSubmit(new Suit()
            //{
            //    Retailer = csvLine.Retailer,
            //    Region = csvLine.Region,
            //    Gender = csvLine.Gender,
            //    Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
            //    Shoulder = csvLine.GetMeasurementOrNull(MeasurementId.Shoulder),
            //    Sleeve = csvLine.GetMeasurementOrNull(MeasurementId.Sleeve),
            //    Chest = csvLine.GetMeasurementOrNull(MeasurementId.Chest),
            //    InsideLeg = csvLine.GetMeasurementOrNull(MeasurementId.InsideLeg),
            //    Neck = csvLine.GetMeasurementOrNull(MeasurementId.Neck),
            //    TorsoLength = csvLine.GetMeasurementOrNull(MeasurementId.TorsoLength),
            //    Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
            //    SizeLetter = csvLine.SizeLetter,
            //    SizeNumber = csvLine.SizeNumber,
            //});
        }
    }


}
