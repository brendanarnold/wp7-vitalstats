using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pocketailor.ViewModel
{
    class ThemeHelpers
    {

        public static void SetThemePreference(ApplicationTheme theme)
        {
            App.Settings.AddOrUpdateValue("ApplicationTheme", theme as ApplicationTheme?);
        }


        public static ApplicationTheme GetTheme()
        {
            ApplicationTheme? theme = App.Settings.GetValueOrDefault<ApplicationTheme?>("ApplicationTheme", null);
            if (theme == null)
            {
                Visibility darkBGVisibility = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
                theme = (darkBGVisibility == Visibility.Visible) ? ApplicationTheme.Dark : ApplicationTheme.Light;
            }
            return (ApplicationTheme)theme;
        }

        public static void LoadThemeDictionary(ApplicationTheme theme)
        {
            //Application.LoadComponent(Application.Current.Resources, new Uri(AppConstants.DARK_THEME_RESOURCE_DICTIONARY, UriKind.Relative));
            PresentationFrameworkCollection<ResourceDictionary> currResourceDictionaries = Application.Current.Resources.MergedDictionaries;
            currResourceDictionaries.Clear();
            ResourceDictionary d = (theme == ApplicationTheme.Dark)
                ? new ResourceDictionary() { Source = new Uri(AppConstants.DARK_THEME_RESOURCE_DICTIONARY, UriKind.Relative) }
                : new ResourceDictionary() { Source = new Uri(AppConstants.LIGHT_THEME_RESOURCE_DICTIONARY, UriKind.Relative) };
            currResourceDictionaries.Add(d);
            d = new ResourceDictionary() { Source = new Uri(AppConstants.MAIN_RESOURCE_DICTIONARY, UriKind.Relative) };
            currResourceDictionaries.Add(d);
        }

    }

    public enum ApplicationTheme
    {
        Dark,
        Light
    }
}
