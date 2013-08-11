using System;
using System.Windows;
using System.Globalization;
using System.Collections.ObjectModel;
using Pocketailor.Model;
using System.Collections;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;


namespace Pocketailor.View
{




    public class UnitCultureToString : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type taretType, object parameter, CultureInfo cultureInfo)
        {
            string[] s = (parameter as string).Split(new char[] { '|' });
            UnitCultureId unitCulture = (UnitCultureId)value;
            if (unitCulture == UnitCultureId.Metric) 
            {
                return s[0];
            } 
            else 
            {
                return s[1];
            }
        }

        public object ConvertBack(object value, Type taretType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Converter which binds to ApplicationTheme enum. Takes as a parameter two pipe 
    /// delimited filenames to first the light theme image, then the dark theme image.
    /// </summary>
    public class ApplicationThemeToImage : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            Pocketailor.ViewModel.ApplicationTheme theme = (Pocketailor.ViewModel.ApplicationTheme)value;
            string[] fns = (parameter as string).Split(new char[] { '|' });
            if (theme == Pocketailor.ViewModel.ApplicationTheme.Light)
            {
                return new BitmapImage(new Uri(fns[0], UriKind.Relative));
            }
            else
            {
                return new BitmapImage(new Uri(fns[1], UriKind.Relative));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Takes a pipe delimited string as a parameter and returns a string prefixed by the part before the pipe and suffixed y the part after the pipe
    /// </summary>
    public class StringToAffixedString : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string[] components = (parameter as string).Split(new char[] { '|' });
            if (components.Length == 1)
            {
                return (value as string) + components[1];
            } 
            else 
            {
                return components[0] + (value as string) + components[1];
            }


        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }


    // Used specifically to obtain the right colours in the GroupItem dialog on ConversionsPage.xaml
    public class BoolToGroupItemBGColor : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            Color activeColor = Color.FromArgb(255, 86, 119, 28);
            Color inactiveColor = Color.FromArgb(255, 138, 190, 45);
            bool b = (bool)value;
            if (b)
            {
                return new SolidColorBrush(inactiveColor);                
            }
            else
            {
                return new SolidColorBrush(activeColor);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToNextString : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string s = (string)value;
            List<string> allStrings = (parameter as string).Split(new char[] { '|' }).ToList();
            // Find the index of the next string along
            int i = allStrings.IndexOf(s);
            i = (i + 1) % allStrings.Count;
            return allStrings[i];
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityToOpposite : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            Visibility vis = (Visibility)value;
            return (vis == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
        
    }


    public class BoolToNotBool : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            bool b = (bool)value;
            return !b;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            bool b = (bool)value;
            return !b;
        }
    }

    public class BoolToNotCheckBox : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            bool b = (bool)value;
            return (bool?)!b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            bool b = (bool)value;
            return !b;
        }

    }

    public class BoolToCheckBox : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            bool b = (bool)value;
            return (bool?)b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            bool b = (bool)value;
            return b;
        }

    }
    

    public class SelectedConversionToBGImage : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            // Stoopid hack to get this to work at designtime too
            ConversionId conversionId;
            if (value.GetType() == typeof(string))
            {
                conversionId = (ConversionId)Enum.Parse(typeof(ConversionId), (string)value, true);
            }
            else
            {
                conversionId = (ConversionId)value;
            }
            switch (conversionId)
            {
                case ConversionId.TrouserSize:
                    return new BitmapImage(new Uri("/Images/trousers-bg.jpg", UriKind.Relative));
                case ConversionId.ShirtSize:
                    return new BitmapImage(new Uri("/Images/shirt-bg.jpg", UriKind.Relative));
                case ConversionId.HatSize:
                    return new BitmapImage(new Uri("/Images/hat-bg.jpg", UriKind.Relative));
                case ConversionId.SuitSize:
                    return new BitmapImage(new Uri("/Images/suit-bg.jpg", UriKind.Relative));
                case ConversionId.DressSize:
                    return new BitmapImage(new Uri("/Images/dress-bg.jpg", UriKind.Relative));
                case ConversionId.BraSize:
                    //return new BitmapImage(new Uri("/Images/bra-bg.jpg", UriKind.Relative));
                    return new BitmapImage(new Uri("/Images/default-bg.png", UriKind.Relative));
                case ConversionId.HosierySize:
                    //return new BitmapImage(new Uri("/Images/hosiery-bg.jpg", UriKind.Relative));
                    return new BitmapImage(new Uri("/Images/default-bg.png", UriKind.Relative));
                case ConversionId.ShoeSize:
                    return new BitmapImage(new Uri("/Images/shoes-bg.jpg", UriKind.Relative));
                case ConversionId.SkiBootSize:
                    return new BitmapImage(new Uri("/Images/skiboots-bg.jpg", UriKind.Relative));
                case ConversionId.WetsuitSize:
                    return new BitmapImage(new Uri("/Images/wetsuit-bg.jpg", UriKind.Relative));
                default:
                    return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedConversionToPageTitleString : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            // Stoopid hack to get this to work at designtime too
            ConversionId conversionId;
            if (value.GetType() == typeof(string))
            {
                conversionId = (ConversionId)Enum.Parse(typeof(ConversionId), (string)value, true);
            }
            else
            {
                conversionId = (ConversionId)value;
            }
            switch (conversionId)
            {
                case ConversionId.TrouserSize:
                    return "trousers";
                case ConversionId.ShirtSize:
                    return "shirts";
                case ConversionId.HatSize:
                    return "hats";
                case ConversionId.SuitSize:
                    return "suits";
                case ConversionId.DressSize:
                    return "dresses";
                case ConversionId.BraSize:
                    return "bras";
                case ConversionId.HosierySize:
                    return "hosiery";
                case ConversionId.ShoeSize:
                    return "shoes";
                case ConversionId.SkiBootSize:
                    return "ski boots";
                case ConversionId.TennisGripSize:
                    return "tennis grip";
                case ConversionId.WetsuitSize:
                    return "wetsuits";
                default:
                    return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }


    // Generates an Image Source binding from a Gender enum. Parameter gives two possible images separated by '|'
    public class GenderToImage : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            GenderId g;
            if (value.GetType() == typeof(string))
            {
                g = (GenderId)Enum.Parse(typeof(GenderId), (string)value, true);
            }
            else
            {
                g = (GenderId)value;
            }
            int i = (g == GenderId.Female) ? 1 : 0;
            string s = ((string)parameter).Split(new char[] { '|' })[i];
            return new BitmapImage(new Uri(s, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }

    // Generates an Image Source binding from a bool. Parameter gives two possible images separated by '|'
    public class BoolToImage : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            int i = ((bool)value) ? 0 : 1;
            string s = ((string)parameter).Split(new char[] { '|' })[i];
            return new BitmapImage(new Uri(s, UriKind.Relative));
            
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

    // Converters a boolean to a visibility enum
    public class BoolToCollapsed : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return (Visibility)value == Visibility.Collapsed;

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

    // Current possible values:
    //    HideIfMale
    public class GenderToVisibility : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            Model.GenderId? g = value as Model.GenderId?;
            string p = parameter as string;
            if (g == null) return null; // Is this the right thing to do?
            if ((p == "HideIfMale") && (g == Model.GenderId.Male)) return Visibility.Collapsed;
            return Visibility.Visible;
        }
        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }

    }

    // At moment sets opacity to 50% if false, full if true
    public class BoolToOpacity : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string s = (string)parameter;
            bool b = (bool)value;
            if (s == "TrueIsOpaque")
                return b ? 1.0 : 0.5;
            else
                return b ? 0.5 : 1.0;
        }
        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }


    // Converter that returns visibility if an Ienumerable is empty based on paramter
    public class CountToVisibility : System.Windows.Data.IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo cultureInfo)
        {
            string p = (string)parameter;
            ICollection i = (value != null) ? (ICollection)value : new int[] {};
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



