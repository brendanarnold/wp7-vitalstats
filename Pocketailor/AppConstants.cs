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
        // Number of objects that are buffered before calling SubmitChanges() on DB
        public static int DB_OBJECT_BUFFER_BEFORE_WRITE = 50;
        public static string[] VALUE_DELIMITERS = { "|" };
        //public static string NAME_CUSTOM_STAT_TEMPLATE = "Other";
        //public static string NAME_CUSTOM_MEASUREMENT_TYPE = "Other";
        public static string FORMATTED_NAME_SEPARATOR = "/";
        public static int CSV_HEADER_LINES = 1;
        public static List<RegionTag> DEFAULT_REGIONS = new List<RegionTag>() { RegionTag.Worldwide };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_DRESS_SIZE = new List<MeasurementId>() {
            MeasurementId.Chest, 
            MeasurementId.Waist, 
            MeasurementId.Hips };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_SUIT_MENS = new List<MeasurementId>() { 
            MeasurementId.Chest, 
            MeasurementId.Waist, 
            MeasurementId.Sleeve, 
            MeasurementId.InsideLeg, 
            MeasurementId.Shoulder, 
            MeasurementId.Crotch, 
            MeasurementId.Neck, 
            MeasurementId.Wrist };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_SUIT_WOMENS = new List<MeasurementId>() { 
            MeasurementId.Chest, 
            MeasurementId.Waist, 
            MeasurementId.Sleeve, 
            MeasurementId.InsideLeg, 
            MeasurementId.Shoulder, 
            MeasurementId.Crotch, 
            MeasurementId.Neck, 
            MeasurementId.Wrist, 
            MeasurementId.Hips };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_TROUSER = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.InsideLeg,
        };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_SHIRT = new List<MeasurementId>()
        {
            MeasurementId.Neck,
            MeasurementId.Sleeve,
            MeasurementId.Chest,
        };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_HAT = new List<MeasurementId>()
        {
            MeasurementId.Head,
        };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_BRA = new List<MeasurementId>()
        {
            MeasurementId.Overbust,
            MeasurementId.Chest,
            MeasurementId.Underbust,
        };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_HOSIERY = new List<MeasurementId>()
        {
            // TODO: refine necessary measurements for hosiery
            MeasurementId.Height,
            MeasurementId.Hips,
            MeasurementId.InsideLeg,
        };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_SHOES = new List<MeasurementId>() 
        {
            MeasurementId.FootLength,
        };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_SKIBOOTS = new List<MeasurementId>()
        {
            MeasurementId.FootLength,
            MeasurementId.FootWidth,
        };
        public static List<MeasurementId> REQUIRED_MEASUREMENTS_TENNISGRIP = new List<MeasurementId>()
        {
            MeasurementId.TennisGrip,
        };



       
    }
}
