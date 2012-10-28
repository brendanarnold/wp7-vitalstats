using System;
using System.Windows;
using System.Globalization;


namespace VitalStats.View
{

    // Converters a boolean to a visibility enum
    public class BoolToVisibility : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
           return (Visibility)value == Visibility.Visible;
           
        }

    }

    // Converter for profile tile text
    public class WordsOnNewlines : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string s = (string)value;
            return s.Replace(" ", System.Environment.NewLine);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return null;    
        }
    }

    // Converter that returns a default if string passed in paramter if is empty
    public class StringOrDefaultOnEmpty : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string deflt = parameter as string;
            string s = value as string;
            if (s == String.Empty)
            {
                return deflt;
            }
            else
            {
                return s;
            }
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            return null;
        }

    }

    // Coverter that returns string passed in parameter separated by a '|' 
    // character depending on a boolean value
    public class BoolToStrings : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string s = (string)parameter;
            string[] split = s.Split(new Char[] { '|' });
            bool b = (bool)value;
            if (b)
            {
                return split[0];
            }
            else
            {
                return split[1];
            }
        }

        public object ConvertBack(Object value, Type targetType, Object paramter, CultureInfo cultureInfo)
        {
            return null;
        }

    }

    // Converter that returns a lowercase string
    public class StringToLower : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string s = (string)value;
            return s.ToLower();
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            return null;    
        }
    }

    // Converter that returns an uppercase string
    public class StringToUpper : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string s = (string)value;
            return s.ToUpper();
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }


}



