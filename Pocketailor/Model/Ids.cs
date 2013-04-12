using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public enum ConversionId
    {
        TrouserSize = 0,
        ShirtSize = 1,
        HatSize = 2,
        SuitSize = 3,
        DressSize = 4,
        BraSize = 5,
        HosierySize = 6,
        ShoeSize = 7,
        SkiBootSize = 8,
        TennisGripSize = 9,
        WetsuitSize = 10,
    }

    public enum MeasurementTypeId
    {
        Length = 0,
        Weight = 1,
    }

    public enum MeasurementId
    {
        Height = 0,
        Weight = 1,
        Waist = 2,
        Chest = 3,
        Hips = 4,
        InsideLeg = 5,
        OutsideLeg = 6,
        Sleeve = 7,
        Wrist = 8,
        Shoulder = 9,
        Neck = 10,
        Overbust = 11,
        Underbust = 12,
        FootLength = 13,
        FootWidth = 14,
        Crotch = 15,
        Head = 16,
        TennisGrip = 17,
        TorsoLength = 18,
    }

    public enum UnitId
    {
        // Length units
        Metre = 0,
        Centimetre = 1,
        Feet = 2,
        Inch = 3,
        FeetInch = 4,
        // Weight Units
        Pound = 5,
        Stone = 6,
        Kilogram = 7,
        Ounce = 8,
        PoundOunce = 9,
    }

    public enum RetailId
    {
        ASOS = 0,
        BananaRepublic = 1,
        MarksSpencer = 2,
        Jaeger = 3,
    }


}
