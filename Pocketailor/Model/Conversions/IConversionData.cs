﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    public interface IConversionData
    {
        double GetChiSq(List<double> measuredVals);
        RegionTag Region { get; }
        RetailId Retailer { get; }
        Gender Gender { get; }
        string FormattedValue { get; }
    }
}