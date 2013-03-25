using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public enum ConversionId
    {
        DressSize,
        ShoeSize,
        SuitSize,
        TrouserSize,
    }

    public enum MeasurementTypeId
    {
        Length,
        Weight,
    }

    public enum MeasurementId
    {
        Height,
        Weight,
        Waist,
        Chest,
        Hips,
        InsideLeg,
        OutsideLeg,
        Sleeve,
        Wrist,
        Shoulder,
        Neck,
        Overbust,
        Underbust,
        FootLength,
        FootWidth,
        Crotch,
        Head,
        TennisGrip
    }

    public enum UnitId
    {
        // Length units
        Metre,
        Centimetre,
        Feet,
        Inch,
        FeetInch,
        Hand,
        // Weight Units
        Pound,
        Stone,
        Kilogram,
        Ounce,
        PoundOunce,
    }

    public enum RetailId
    {
        ASOS,
        BananaRepublic,
        MarksSpencer,
        Jaeger,
    }


}
