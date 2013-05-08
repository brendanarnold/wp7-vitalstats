using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Adjustments
{
    // The class that will get JSONified and sent over the wire to the server
    // Note the names are abbreviated since MongoDB stores fields with each entry
    public class Adjustment
    {
        // Adjustment version format
        public int v = AppConstants.ADJUSTMENT_FORMAT_VERSION;

        // Gender of the person the adjustment applies to 
        public Gender g;

        // Brand
        public RetailId b;

        // Region
        public RegionIds r;

        // Adjustment factor, the factor by which the chart is scaled so that the correct size is obtained
        public double a;

        // App ID, a GUID for this installlation
        public Guid i;

        // Conversion
        public ConversionId c;

        // Measurements, a list of the measurements for this conversion
        public List<double> m;

        // Time logged in UNIX time format i.e. seconds since 1/1/1970 UTC
        public double t;
    }
}
