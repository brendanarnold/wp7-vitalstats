using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator
{
    public static class AppConstants
    {
        public static string CSV_DATA_DIRECTORY = "Model\\Csv\\";
        public static int CSV_HEADER_LINES = 1;
        public static char[] CSV_DELIMITERS = new char[] { '\t' };
        // Number of objects that are buffered before calling SubmitChanges() on DB
        public static int DB_OBJECT_BUFFER_BEFORE_WRITE = 50;
        
    }
}
