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
using System.Windows.Resources;
using System.Linq;
using System.Collections.Generic;

namespace VitalStats.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {

        private AppDataContext appDB;

        public AppViewModel()
        {
        }

        public AppViewModel(string connectionString)
        {
            this.appDB = new AppDataContext(connectionString);
        }


        private ObservableCollection<Profile> _profiles;
        public ObservableCollection<Profile> Profiles 
        {
            get { return this._profiles; }
            set 
            {
                this._profiles = value;
                this.NotifyPropertyChanged("Profiles");
            }
        }

        private ObservableCollection<MeasurementType> _measurementTypes;
        public ObservableCollection<MeasurementType> MeasurementTypes
        {
            get { return this._measurementTypes; }
            set 
            {
                this._measurementTypes = value;
                this.NotifyPropertyChanged("MeasurementTypes");
            }
        }


        private ObservableCollection<Stat> _statTemplates;
        public ObservableCollection<Stat> StatTemplates
        {
            get { return this._statTemplates; }
            set
            {
                this._statTemplates = value;
                this.NotifyPropertyChanged("StatTemplates");
            }
        }

        

        #region Load collections from DB

        public void LoadAllFromDB()
        {
            this.LoadMeasurementTypesFromDB();
            this.LoadProfilesFromDB();
            this.LoadStatTemplatesFromDB();
        }

        public void LoadProfilesFromDB()
        {
            var profiles = from Profile p in this.appDB.Profiles select p;
            this.Profiles = new ObservableCollection<Profile>(profiles);
        }

        public void LoadMeasurementTypesFromDB() 
        {
            var mts = from MeasurementType mt in this.appDB.MeasurementTypes select mt;
            this.MeasurementTypes = new ObservableCollection<MeasurementType>(mts);
        }

        public void LoadStatTemplatesFromDB() 
        {
            var stats = from Stat s in this.appDB.StatTemplates select s;
            this.StatTemplates = new ObservableCollection<Stat>(stats);
        }

        #endregion

        public void AddProfile(Profile profile)
        {
            this.appDB.Profiles.InsertOnSubmit(profile);
            this.appDB.SubmitChanges();
            this.Profiles.Add(profile);
        }

        public void AddStatTemplate(Stat stat)
        {
            this.appDB.Stats.InsertOnSubmit(stat);
            this.appDB.SubmitChanges();
            this.StatTemplates.Add(stat);
        }

        public void DeleteProfile(Profile profile)
        {
            this.Profiles.Remove(profile);
            this.appDB.Profiles.DeleteOnSubmit(profile);
            this.appDB.SubmitChanges();
        }

        public void DeleteStatTemplate(Stat stat)
        {
            this.StatTemplates.Remove(stat);
            this.appDB.StatTemplates.DeleteOnSubmit(stat);
            this.appDB.SubmitChanges();
        }


        







        public void SaveChangesToDB()
        {
            this.appDB.SubmitChanges();
        }



        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion



    }
}
