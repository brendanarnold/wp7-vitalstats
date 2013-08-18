using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Resources;
using System.IO.IsolatedStorage;
using Pocketailor.ViewModel;

namespace Pocketailor.View
{
    public class ThemeResourceDictionaryResolver : ResourceDictionary
    {

        public ThemeResourceDictionaryResolver() : base()
        {
            ApplicationTheme theme;
            if (System.ComponentModel.DesignerProperties.IsInDesignTool)
            {
                theme = ApplicationTheme.Light;
            }
            else
            {
                theme = ThemeHelpers.GetTheme();
            }
            Uri uri = new Uri(string.Format("Pocketailor;component/View/ThemeDictionaries/{0}ThemeResourceDictionary.xaml", theme), UriKind.RelativeOrAbsolute);
            ResourceDictionary res = this.LoadXaml<ResourceDictionary>(uri);
            this.MergedDictionaries.Add(res);
        }


        // For some reason a simple ResourceDictionary() { Source = uri }; does not work, need to use this instead
        T LoadXaml<T>(Uri uri)
        {
            StreamResourceInfo info = Application.GetResourceStream(uri);
            if (info != null)
            {
                using (StreamReader reader = new StreamReader(info.Stream))
                {
                    return (T)XamlReader.Load(reader.ReadToEnd());
                }
            }
            return default(T);
        }

    }
}
