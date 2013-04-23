﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{

    public enum RegionIds
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

    }

    public static partial class Lookup {

        public static Dictionary<RegionIds, string> Regions = new Dictionary<RegionIds,string>() 
        {
            { RegionIds.Worldwide , "Worldwide" },
            { RegionIds.Europe , "Europe" },
            { RegionIds.NorthAmerica, "North America" },
            { RegionIds.SouthAmerica, "South America" },
            { RegionIds.Africa, "Africa" },
            { RegionIds.Asia, "Asia" },
            { RegionIds.Australasia, "Australasia" },
            { RegionIds.UK, "UK" },
            { RegionIds.Ireland, "Ireland" },
            { RegionIds.France, "France" },
            { RegionIds.Italy, "Italy" },
            { RegionIds.Germany, "Germany" },
            { RegionIds.Denmark, "Denmark" },
            { RegionIds.Sweden, "Sweden" },
            { RegionIds.Norway, "Norway" },
            { RegionIds.Finland, "Finland" },
            { RegionIds.Spain, "Spain" },
            { RegionIds.US, "United States" },
            { RegionIds.Australia, "Australia" },
            { RegionIds.NewZealand, "New Zealand" },
            { RegionIds.Canada, "Canada" },

        };

    }


}