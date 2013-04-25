using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Pocketailor
{
    public static class AppConstants
    {
        public static string APP_VERSION = "0.1.1";
        // TODO: Fill in the paid app GUID
        public static string PAID_APP_GUID = "";
        public static string[] VALUE_DELIMITERS = { "|" };
        public static string FORMATTED_NAME_SEPARATOR = "/";
        public static List<RegionIds> DEFAULT_REGIONS = new List<RegionIds>() { 
            RegionIds.Worldwide 
        };
        public static string CONVERSIONS_DB_CONNECTION_STRING = "Data Source = 'appdata:/PocketailorConversions.sdf'; File Mode = read only;";
        public static string APP_DB_CONNECTION_STRING = "Data Source=isostore:/Pocketailor.sdf";
        public static string HELP_IMAGE_DIRECTORY = "/Images/HelpImages/";
        public static string AUTHOR_EMAIL = "brendanarnold@outlook.com";
        public static string WEBSITE_URL = "http://www.lassiv.com/pocketailor/";
        public static string LEGAL_URL = "http://www.lassiv.com/pocketailor/legal.html";
        public static string FACEBOOK_LIKE_URL = "http://facebook.com/Pocketailor";
        public static string FEEDBACK_URL = "http://lassiv.uservoice.com";
        public static UnitCultureId DEFAULT_UNIT_CULTURE = UnitCultureId.Metric;



    }
}
