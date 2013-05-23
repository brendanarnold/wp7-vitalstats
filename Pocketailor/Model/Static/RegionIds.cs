using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{

    public enum RegionId
    {
        // Regions
        Worldwide = 0,
        Europe = 1,
        NorthAmerica = 2,
        SouthAmerica = 3,
        Africa = 4,
        Asia = 5,
        Australasia = 6,
        // Countries(-ish)
        UK = 7,
        Ireland = 8,
        France = 9,
        Italy = 10,
        Germany = 11,
        Denmark = 12,
        Sweden = 13,
        Norway = 14,
        Finland = 15,
        Spain = 16,
        US = 17,
        Australia = 18,
        NewZealand = 19,
        Canada = 20,
        Austria = 21,
        Switzerland = 22,
        Netherlands = 23,
        Belgium = 24,
        Portugal = 25,
        Japan = 26,
        Mexico = 27,
        China = 28,
        Korea = 29,
        Russia = 30,
        HongKong = 31,
        Isreal = 32,
        Brazil = 33,

    }

    public static partial class Lookup {

        public static Dictionary<RegionId, string> Regions = new Dictionary<RegionId,string>() 
        {
            { RegionId.Worldwide , "Worldwide" },
            { RegionId.Europe , "Europe" },
            { RegionId.NorthAmerica, "North America" },
            { RegionId.SouthAmerica, "South America" },
            { RegionId.Africa, "Africa" },
            { RegionId.Asia, "Asia" },
            { RegionId.Australasia, "Australasia" },
            { RegionId.UK, "UK" },
            { RegionId.Ireland, "Ireland" },
            { RegionId.France, "France" },
            { RegionId.Italy, "Italy" },
            { RegionId.Germany, "Germany" },
            { RegionId.Denmark, "Denmark" },
            { RegionId.Sweden, "Sweden" },
            { RegionId.Norway, "Norway" },
            { RegionId.Finland, "Finland" },
            { RegionId.Spain, "Spain" },
            { RegionId.US, "United States" },
            { RegionId.Australia, "Australia" },
            { RegionId.NewZealand, "New Zealand" },
            { RegionId.Canada, "Canada" },
            { RegionId.Austria, "Austria" },
            { RegionId.Switzerland, "Switzerland" },
            { RegionId.Netherlands, "Netherlands" },
            { RegionId.Belgium, "Belgium" },
            { RegionId.Portugal, "Portugal" },
            { RegionId.Japan, "Japan" },
            { RegionId.Mexico, "Mexico" },
            { RegionId.China, "China" },
            { RegionId.Korea, "Korea" },
            { RegionId.Russia, "Russia" },
            { RegionId.HongKong, "Hong Kong" },
            { RegionId.Isreal, "Isreal" },
            { RegionId.Brazil, "Brazil" },
        };

    }


}
