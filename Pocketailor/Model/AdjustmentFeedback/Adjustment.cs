using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.AdjustmentFeedback
{
    // The class that will get JSONified and sent over the wire to the server
    // Note the names are abbreviated since MongoDB stores fields with each entry
    public class Adjustment
    {
        // Gender of the person the adjustment applies to 
        public Gender g;

        // Brand
        public RetailId b;

        // Region
        public RegionIds r;

        // Adjustment i.e. +1 means next size up, 0 is right, -1 is next size down
        public int a;

        // App ID, a GUID for this installlation
        public Guid i;

        // Conversion
        public ConversionId c;

        // Measurement
        public MeasurementId m;

        // Measurement value
        public double v;

        // Time logged in UNIX time format i.e. seconds since 1/1/1970 UTC
        public double t;
    }
}
