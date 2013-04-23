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

    public static partial class Lookup
    {
        public static Dictionary<ConversionId, string> Conversions = new Dictionary<ConversionId, string>()
        {
            { ConversionId.TrouserSize, "Trouser" },
            { ConversionId.ShirtSize, "Shirt" },
            { ConversionId.HatSize, "Hat" },
            { ConversionId.SuitSize, "Suit" },
            { ConversionId.DressSize, "Dress" },
            { ConversionId.BraSize, "Bra" },
            { ConversionId.HosierySize, "Hosiery" },
            { ConversionId.ShoeSize, "Shoe" },
            { ConversionId.SkiBootSize, "Ski Boot" },
            { ConversionId.TennisGripSize, "Tennis Grip" },
            { ConversionId.WetsuitSize, "Wetsuit" },
        };
    }

}
