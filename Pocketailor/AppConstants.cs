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
        // Increment this when new data added to DB
        public static int CONVERSION_DATA_VERSION = 1;
        public static string APP_VERSION = "0.1." + CONVERSION_DATA_VERSION.ToString();
        // TODO: Fill in the paid app GUID
        public static string PAID_APP_GUID = "";
        // Number of objects that are buffered before calling SubmitChanges() on DB
        public static int DB_OBJECT_BUFFER_BEFORE_WRITE = 50;
        public static string[] VALUE_DELIMITERS = { "|" };
        //public static string NAME_CUSTOM_STAT_TEMPLATE = "Other";
        //public static string NAME_CUSTOM_MEASUREMENT_TYPE = "Other";
        public static string FORMATTED_NAME_SEPARATOR = "/";
        public static int CSV_HEADER_LINES = 1;
        public static char[] CSV_DELIMITERS = new char[] { '\t' };
        public static List<RegionIds> DEFAULT_REGIONS = new List<RegionIds>() { 
            RegionIds.Worldwide 
        };
        public static string HELP_IMAGE_DIRECTORY = "/Images/HelpImages/";
        public static string CSV_DATA_DIRECTORY = "Model\\Csv\\";
        public static string AUTHOR_EMAIL = "brendanarnold@outlook.com";
        public static string WEBSITE_URL = "http://www.lassiv.com/pocketailor/";
        public static string LEGAL_URL = "http://www.lassiv.com/pocketailor/legal.html";
        public static string FACEBOOK_LIKE_URL = "http://facebook.com/Pocketailor";
        public static string FEEDBACK_URL = "http://lassiv.uservoice.com";


    }
}
