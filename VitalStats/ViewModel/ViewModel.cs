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
using System.Collections.ObjectModel;
using VitalStats.Model;
using System.IO.IsolatedStorage;

namespace VitalStats.ViewModelNamespace
{
    public class ViewModel
    {
        public ObservableCollection<Profile> Profiles { get; set; }

        public void GetProfiles()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Count > 0)
            {
                this.GetSavedProfiles();
            }
            else
            {
                this.GetDefaultProfiles();
            }
        }

        public void GetDefaultProfiles()
        {
            ObservableCollection<Profile> profiles = new ObservableCollection<Profile>{
                new Profile() { Name="John Smith", IsProtected=false},
                new Profile() { Name = "Me", IsProtected=false},
                new Profile() { Name = "Jane Smith", IsProtected=true}
            };
            this.Profiles = profiles;
        }

        public void GetSavedProfiles()
        {
            ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();
            foreach (Object o in IsolatedStorageSettings.ApplicationSettings.Values)
            {
                profiles.Add((Profile)o);
            }
            this.Profiles = profiles;
        }

        public void SaveProfiles()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            foreach (Profile p in this.Profiles)
            {
                string pKey = "Profile:" + p.Id;
                if (settings.Contains(pKey))
                {
                    settings[pKey] = p.GetCopy();
                }
                else
                {
                    settings.Add(pKey, p.GetCopy());
                }
            }
            settings.Save();
        }

    }
}
