using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pocketailor.Globalisation
{
    public static class Helpers
    {
        public static string GetRegionNameFromIso(string iso)
        {
            if (CustomRegions.CustomRegionLookup.Keys.Contains(iso))
            {
                return CustomRegions.CustomRegionLookup[iso];
            }
            else
            {
                try
                {
                    RegionInfo region = new RegionInfo(iso);
                    return region.DisplayName;
                }
                catch
                {
                    return null;
                }
            }

        }

        

       

        

    }
}
