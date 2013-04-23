using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
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

    public static partial class Lookup
    {
        public static Dictionary<UnitId, string> Unit = new Dictionary<UnitId, string>()
        {
            { UnitId.Metre, "Metres" },
            { UnitId.Centimetre, "Centimetres" },
            { UnitId.Feet, "Feet" },
            { UnitId.Inch, "Inches" },
            { UnitId.FeetInch, "Feet/Inches"},
            { UnitId.Pound, "Pounds"},
            { UnitId.Stone, "Stones" },
            { UnitId.Kilogram, "Kilograms" },
            { UnitId.Ounce, "Ounces" },
            { UnitId.PoundOunce, "Pounds/Ounces"},
        };
    }

}
