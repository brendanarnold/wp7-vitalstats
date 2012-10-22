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
using System.Runtime.Serialization;
using System.ComponentModel;

namespace VitalStats.ViewModelNamespace
{
    [DataContract]
    public class ViewModel
    {
        [DataMember]
        public ObservableCollection<Profile> Profiles { get; set; }

        
        public void GetProfiles()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Count > 0)
            {
                this.GetSavedProfiles();
            }
            else
            {
                this.Profiles = new ObservableCollection<Profile>();
            }
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


        internal void AddNewProfile(string name, bool isProtected)
        {
            this.Profiles.Add(new Profile() { Name = name, IsProtected = isProtected });
        }

        internal void DeleteProfile(Profile profile)
        {
            this.Profiles.Remove(profile);
        }
    }
}
