using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model
{
    public class CsvLine
    {
        public CsvLine()
        {
            this.Measurements = new Dictionary<MeasurementId, double>();
        }
        public Dictionary<MeasurementId, double> Measurements;
        public RetailId Retailer;
        public RegionIds Region;
        public ConversionId Conversion;
        public string RegionalSize;
        public string GeneralSize;
        public Gender Gender;
        public double GetMeasurementOrNull(MeasurementId id)
        {
            double d;
            if (this.Measurements.TryGetValue(id, out d))
            {
                return d;
            }
            else
            {
                return -1;
            }
        }


    }

}
