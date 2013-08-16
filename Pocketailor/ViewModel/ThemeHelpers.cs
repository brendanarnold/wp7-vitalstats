using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pocketailor.ViewModel
{
    class ThemeHelpers
    {


        // Also use external library "Windows Phone Theme Manager"

        public static void SetThemePreference(ApplicationTheme theme)
        {
            App.Settings.AddOrUpdateValue("ApplicationTheme", theme as ApplicationTheme?);
        }


        public static ApplicationTheme GetTheme()
        {
            ApplicationTheme? theme = App.Settings.GetValueOrDefault<ApplicationTheme?>("ApplicationTheme", null);
            if (theme == null)
            {
                // Default is global phone theme
                Visibility darkBGVisibility = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
                theme = (darkBGVisibility == Visibility.Visible) ? ApplicationTheme.Dark : ApplicationTheme.Light;
            }
            return (ApplicationTheme)theme;
        }

        public static void LoadThemeDictionary(ApplicationTheme theme)
        {
            // Light theme is loaded by default, only need to do anything if it is dark theme - dictionaries loaded later override dictionaries loaded earlier
            if (theme == ApplicationTheme.Dark)
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(AppConstants.DARK_THEME_RESOURCE_DICTIONARY, UriKind.Relative) });
        }

    }

    public enum ApplicationTheme
    {
        Dark,
        Light
    }
}
