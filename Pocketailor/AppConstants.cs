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
        
        
        // The name of the field in the POST request
        public static string ADJUSTMENT_REQUEST_POST_FIELD = "adjustment";
        public static string SECRET_REQUEST_POST_FIELD = "pocketailor_adjustments_secret";
        public static string POCKETAILOR_ADJUSTMENTS_SERVER_OK_RESPONSE = "pocketed";
#if DEBUG
        public static int MAX_ADJUSTMENTS_PER_REQUEST = 2;
        public static int MAX_DELIVERY_ATTEMPTS = 3;
        public static string POCKETAILOR_ADJUSTMENTS_WEBSERVICE_URL = "http://localhost:3000/";
        public static string POCKETAILOR_ADJUSTMENTS_SECRET = "testing_secret";
#else
        public static int MAX_ADJUSTMENTS_PER_REQUEST = 50;
        public static int MAX_DELIVERY_ATTEMPTS = 3;
        // This is a random string used by the server to verify that it is a Pocketailor app talking to it 
        public static string POCKETAILOR_ADJUSTMENTS_WEBSERVICE_URL = "https://pocketailor_adjustments.eu01.aws.af.cm/";
        public static string POCKETAILOR_ADJUSTMENTS_SECRET = "7f1f830c-18fa-4e49-af6f-5ea4c2c85f62";
#endif


        public static string APP_VERSION = "0.1.1";
        // TODO: Fill in the paid app GUID
        public static string PAID_APP_GUID = "";
        public static string[] VALUE_DELIMITERS = { "|" };
        public static string FORMATTED_NAME_SEPARATOR = "/";
        public static RegionIds DEFAULT_REGION = RegionIds.Worldwide;
        public static string CONVERSIONS_DB_CONNECTION_STRING = "Data Source = 'appdata:/PocketailorConversions.sdf'; File Mode = read only;";
        public static string APP_DB_CONNECTION_STRING = "Data Source=isostore:/Pocketailor.sdf";
        public static string HELP_IMAGE_DIRECTORY = "/Images/HelpImages/";
        public static string AUTHOR_EMAIL = "brendanarnold@outlook.com";
        public static string WEBSITE_URL = "http://www.lassiv.com/pocketailor/";
        public static string LEGAL_URL = "http://www.lassiv.com/pocketailor/legal.html";
        public static string FACEBOOK_LIKE_URL = "http://facebook.com/Pocketailor";
        public static string FEEDBACK_URL = "http://lassiv.uservoice.com";
        public static UnitCultureId DEFAULT_UNIT_CULTURE = UnitCultureId.Metric;
        public static bool? DEFAULT_ALLOW_FEEDBACK = null;

        // This app generates adjustmnts of this version
        public static int ADJUSTMENT_FORMAT_VERSION = 1;

        
    }
}
