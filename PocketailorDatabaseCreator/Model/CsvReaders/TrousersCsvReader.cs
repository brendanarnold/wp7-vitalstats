using Pocketailor.Model;
using Pocketailor.Model.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public class TrousersCsvReader : ICsvReader
    {
        public TrousersCsvReader()
        {
            this.ConversionId = ConversionId.TrouserSize;
        }
        public ConversionId ConversionId { get; set; }
        public ConversionsDataContext Db { get; set; }
        public void QueueWriteObj(CsvLine csvLine)
        {
            this.Db.Trousers.InsertOnSubmit(new Trousers()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Gender = csvLine.Gender,
                Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
                Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
                InsideLeg = csvLine.GetMeasurementOrNull(MeasurementId.InsideLeg),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }


}
