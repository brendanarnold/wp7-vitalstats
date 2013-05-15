using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{

    public enum BrandId
    {
        ASOS = 0,
        BananaRepublic = 1,
        MarksSpencer = 2,
        Jaeger = 3,
        Gap = 4,
	    FrenchConnection = 5,
    }

    public static partial class Lookup
    {
        public static Dictionary<BrandId, string> Brand = new Dictionary<BrandId, string>()
        {
            { BrandId.ASOS, "ASOS" },
            { BrandId.BananaRepublic, "Banana Republic" },
            { BrandId.MarksSpencer, "Marks and Spencer" },
            { BrandId.Jaeger, "Jaeger" },
            { BrandId.FrenchConnection, "French Connection" },
            { BrandId.Gap, "Gap" },
        };
    }

}
