using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;


namespace Pocketailor.Model
{
    // Settings that apply to the backend
    public class SettingsHelpers
    {
        IsolatedStorageSettings Settings;

        // Keynames
        //const string ExampleKeyName = "Example";

        // Default setting value
        //const bool ExampleDefault = true;

        public SettingsHelpers()
        {
            this.Settings = IsolatedStorageSettings.ApplicationSettings;
        }

        // Update the settings
        public bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;
            if (Settings.Contains(key))
            {
                if (Settings[key] != value)
                {
                    Settings[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                Settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        // Get the current value or the default if not set
        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;
            if (Settings.Contains(key))
            {
                value = (T)Settings[key];
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        // Save the settings
        public void Save()
        {
            Settings.Save();
        }

        // Example setting property
        //public bool Example
        //{
        //    get 
        //    {
        //        return this.GetValueOrDefault<bool>(ExampleKeyName, ExampleDefault);
        //    }
        //    set
        //    {
        //        if (this.AddOrUpdateValue(ExampleKeyName, value))
        //        {
        //            this.Save();
        //        }
        //    }
        //}

    }
}
