using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator
{
    class VersionHelpers
    {
        private static Version WP8Version = new Version(8, 0);

        public static bool IsWP8
        {
            get
            {
                return Environment.OSVersion.Version >= VersionHelpers.WP8Version;
            }
        }

        static Version SupportsWideTileVersion = new Version(7, 10, 8858);

        public static bool IsWideTileCapable
        {
            get
            {
                return Environment.OSVersion.Version > VersionHelpers.SupportsWideTileVersion;
            }
        }


    }
}
