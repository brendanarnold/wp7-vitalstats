using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public enum ConversionId
    {
        TrouserSize,
        ShirtSize,
        HatSize,
        SuitSize,
        DressSize,
        BraSize,
        HosierySize,
        ShoeSize,
        SkiBootSize,
        TennisGripSize,
        WetsuitSize,
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
        TennisGrip,
        TorsoLength,
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
