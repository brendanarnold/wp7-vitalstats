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
        // TODO: These need to be changed on each release and in the XAP file too
        public static int ADJUSTMENT_FORMAT_VERSION = 1;
        public static string APP_VERSION = "2.1.0";


        // These are the 'magic' SizeId values which signify that all the sizes are too big or small
        public static int ADJUSTMENT_SIZEID_ALL_TOO_BIG = -1000;
        public static int ADJUSTMENT_SIZEID_ALL_TOO_SMALL = 1000;
        // The name of the fields in the POST request
        public static string ADJUSTMENT_REQUEST_POST_FIELD = "adjustment";
        public static string SECRET_REQUEST_POST_FIELD = "pocketailor_adjustments_secret";
#if DEBUG
        // If there are more than this number of items queued for delivey, dump the feedback
        public static int MAX_FEEDBACK_ITEMS_FOR_DELIVERY = 15;
        public static int MAX_ADJUSTMENTS_PER_REQUEST = 50;
        public static int MAX_DELIVERY_ATTEMPTS = 3;
        public static string POCKETAILOR_ADJUSTMENTS_WEBSERVICE_URL = "http://whitby:7000/";
        public static string POCKETAILOR_ADJUSTMENTS_SECRET = "testing_secret";
#else
        public static int MAX_FEEDBACK_ITEMS_FOR_DELIVERY = 15;
        public static int MAX_ADJUSTMENTS_PER_REQUEST = 50;
        public static int MAX_DELIVERY_ATTEMPTS = 3;
        // This is a random string used by the server to verify that it is a Pocketailor app talking to it 
        public static string POCKETAILOR_ADJUSTMENTS_WEBSERVICE_URL = "https://pocketailor_adjustments.eu01.aws.af.cm";
        public static string POCKETAILOR_ADJUSTMENTS_SECRET = "7f1f830c-18fa-4e49-af6f-5ea4c2c85f62";
#endif

        // TODO: Fill in the paid app GUID
        public static string PAID_APP_GUID = "";
        public static string[] VALUE_DELIMITERS = { "|" };
        public static string FORMATTED_NAME_SEPARATOR = "/";
        public static string DEFAULT_REGION = Globalisation.CustomRegions.GlobalRegion;
        public static string CONVERSIONS_DB_WP8_CONNECTION_STRING = "Data Source = 'appdata:/PocketailorConversionsWP8.sdf'; File Mode = read only;";
        public static string CONVERSIONS_DB_WP7_CONNECTION_STRING = "Data Source = 'appdata:/PocketailorConversionsWP7.sdf'; File Mode = read only;";
        
        public static string APP_DB_CONNECTION_STRING = "Data Source=isostore:/Pocketailor.sdf";
        public static string HELP_IMAGE_DIRECTORY = "/Images/HelpImages/";
        public static string AUTHOR_EMAIL = "brendan@lassiv.com";
        public static UnitCultureId DEFAULT_UNIT_CULTURE = UnitCultureId.Metric;
        public static bool? DEFAULT_ALLOW_FEEDBACK = null;
        public static int NUM_BOOTS_TIL_READY_TO_RATE = 3;

        public static string WEBSITE_URL = "http://bit.ly/190LBuj";
        public static string LEGAL_URL = "http://bit.ly/16K1ukL";
        public static string FEEDBACK_URL = "http://bit.ly/1at8chu";
        public static string FACEBOOK_URL = "http://on.fb.me/17mPyJh";
        public static string TWITTER_URL = "http://bit.ly/17mPLMB";

        public static string MAIN_RESOURCE_DICTIONARY = "/View/ThemeDictionaries/MainResourceDictionary.xaml";
        public static string DARK_THEME_RESOURCE_DICTIONARY = "/View/ThemeDictionaries/DarkThemeResourceDictionary.xaml";
        public static string LIGHT_THEME_RESOURCE_DICTIONARY = "/View/ThemeDictionaries/LightThemeResourceDictionary.xaml";

        public static string ADDUPLEX_APP_ID = "39175";

    }
}
