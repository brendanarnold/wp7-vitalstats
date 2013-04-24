using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{

    public enum RetailId
    {
        ASOS = 0,
        BananaRepublic = 1,
        MarksSpencer = 2,
        Jaeger = 3,
        Gap = 4,
    }

    public static partial class Lookup
    {
        public static Dictionary<RetailId, string> Retail = new Dictionary<RetailId, string>()
        {
            { RetailId.ASOS, "ASOS" },
            { RetailId.BananaRepublic, "Banana Republic" },
            { RetailId.MarksSpencer, "Marks and Spencer" },
            { RetailId.Jaeger, "Jaeger" },
        };
    }

}
