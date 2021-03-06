﻿using Pocketailor.Model;
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
        public BrandId Brand;
        public string Region;
        public ConversionId Conversion;
        public string Size;
        public int SizeId;
        public GenderId Gender;
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
