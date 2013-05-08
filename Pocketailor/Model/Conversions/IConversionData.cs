using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    public interface IConversionData
    {
        double FindBestFit(Dictionary<MeasurementId, double> measuredVals);
        RegionIds Region { get; }
        RetailId Retailer { get; }
        Gender Gender { get; }
        string FormattedValue { get; }
    }
}
