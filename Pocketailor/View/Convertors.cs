﻿using System;
using System.Windows;
using System.Globalization;
using System.Collections.ObjectModel;
using Pocketailor.Model;
using System.Collections;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Controls;


namespace Pocketailor.View
{

    // Generates an Image Source binding from a bool. Parameter gives two possible images separated by '|'
    public class BoolToImage : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            //int i = ((bool)value) ? 0 : 1;
            //string s = ((string)parameter).Split(new char[] { '|' })[i];
            return new BitmapImage(new Uri("/Images/Lock.png", UriKind.Relative));
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

    public class EmptyCollectionToVisible : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var collection = value as System.Collections.IEnumerable;
            if (collection == null)
                throw new ArgumentException("value");

            bool isEmpty = !collection.Cast<object>().Any();
            if (isEmpty)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

    public class EmptyCollectionToCollapsed : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var collection = value as System.Collections.IEnumerable;
            if (collection == null)
                throw new ArgumentException("value");

            bool isEmpty = !collection.Cast<object>().Any();
            if (isEmpty)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }


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

    // Converter to add extra item to databound listpicker on EditStats page
    //public class AddCustomStatTemplateOption : System.Windows.Data.IValueConverter
    //{
    //    public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
    //    {
    //        ObservableCollection<Stat> statTemplates = value as ObservableCollection<Stat>;
    //        statTemplates.Insert(0, new Stat() { Name = "Custom", Id = -1 });
    //        return statTemplates;
    //    }
    //    public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
    //    {
    //        return null;
    //    }
    //}

    // Converter that returns visibility if an Ienumerable is empty based on paramter
    public class CountToVisibility : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string p = (string)parameter;
            ICollection i = (ICollection)value;
            if (i.Count == 0)
            {
                if (p == "VisibleIfEmpty")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                if (p == "CollapsedIfEmpty")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            
        }
        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            return null;
        }        
    }

    // Converter that returns visibility if value is null based on parameter
    public class NullToVisibility : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string p = (string)parameter;
            if (value == null)
            {
                if (p == "VisibleIfNull")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                if (p == "CollapsedIfNull")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

        }
        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }


}


