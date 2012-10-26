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


namespace VitalStats.Settings
{
    // Settings that apply to the backend
    public class ViewModelSettings
    {
        IsolatedStorageSettings settings;

        // Keynames
        //const string ExampleKeyName = "Example";

        // Default setting value
        //const bool ExampleDefault = true;

        public ViewModelSettings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        // Update the settings
        public bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;
            if (settings.Contains(key))
            {
                if (settings[key] != value)
                {
                    settings[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                settings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        // Get the current value or the default if not set
        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;
            if (settings.Contains(key))
            {
                value = (T)settings[key];
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
            settings.Save();
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
