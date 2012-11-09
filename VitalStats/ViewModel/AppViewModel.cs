﻿using System.Collections.ObjectModel;
using VitalStats.Model;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq;
using Microsoft.Phone.Data.Linq.Mapping;
using System.Collections;

namespace VitalStats.ViewModel
{

    // This collects together the properties exposed to the view and 
    // the methods that act on them that are required for the MainPage.xaml
    // file to be displayed - see other files in the ViewModel directory for
    // other parts of this class
    public partial class AppViewModel : INotifyPropertyChanged
    {

        #region Profiles collection methods/properties

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

        public void LoadProfilesFromDB()
        {
            var profiles = from Profile p in this.appDB.Profiles select p;
            this.Profiles = new ObservableCollection<Profile>(profiles);
        }

        public void AddProfile(Profile profile)
        {
            this.appDB.Profiles.InsertOnSubmit(profile);
            this.appDB.SubmitChanges();
            this.Profiles.Add(profile);
        }

        public void DeleteProfile(Profile profile)
        {
            this.Profiles.Remove(profile);
            this.appDB.Profiles.DeleteOnSubmit(profile);
            this.appDB.SubmitChanges();
        }

        #endregion

        public void AddStat(Stat stat)
        {
            this.appDB.Stats.InsertOnSubmit(stat);
            this.appDB.SubmitChanges();
        }

        #region SelectedProfile methods/properties

        private Profile _selectedProfile;
        public Profile SelectedProfile
        {
            get { return this._selectedProfile; }
            set
            {
                this._selectedProfile = value;
                this.NotifyPropertyChanged("SelectedProfile");
            }
        }

        #endregion

        #region SuggestedStat methods/proprties

        private Stat _suggestedStatTemplate;
        public Stat SuggestedStatTemplate
        {
            get { return this._suggestedStatTemplate; }
            set
            {
                this._suggestedStatTemplate = value;
                this.NotifyPropertyChanged("SuggestedStatTemplate");
            }
        }

        // Following is some code to present a different suggested stat that has not already been
        // used in the selectedProfile
        private int _suggestedStatTemplateInd = 0;
        public void LoadNextSuggestedStatTemplate()
        {
            this._suggestedStatTemplateInd += 1;
            if (this.StatTemplates == null) 
            { 
                this.LoadStatTemplatesFromDB(); 
            }
            List<string> suggNames = (from Stat st in this.StatTemplates
                                      select
                                          st.Name).Except(from Stat st in this.SelectedProfile.Stats
                                                          select st.Name).ToList();
            // Check if any suggested stats left over
            if (suggNames.Count == 0)
            {
                this.SuggestedStatTemplate = null;
                return;
            }
            int ind = this._suggestedStatTemplateInd % suggNames.Count;
            this.SuggestedStatTemplate = (from Stat st in this.StatTemplates
                                          where st.Name == suggNames[ind]
                                          select st).First();
        }


        #endregion

        #region SelectedStat methods/properties

        private Stat _selectedStat;
        public Stat SelectedStat
        {
            get { return this._selectedStat; }
            set
            {
                this._selectedStat = value;
                this.NotifyPropertyChanged("SelectedStat");
            }
        }

        #endregion

        #region MeasurementTypes methods/properties

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

        public void LoadMeasurementTypesFromDB()
        {
            var mts = from MeasurementType mt in this.appDB.MeasurementTypes select mt;
            this.MeasurementTypes = new ObservableCollection<MeasurementType>(mts);
        }

        #endregion

        #region StatTemplates methods/properties

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

        public void LoadStatTemplatesFromDB()
        {
            var stats = from Stat s in this.appDB.StatTemplates select s;
            this.StatTemplates = new ObservableCollection<Stat>(stats);
        }

        #endregion

        #region Values (for EditStatPage) methods/properties

        // Following is used in the edit stat page
        private string _value1 = string.Empty;
        public string Value1
        {
            get { return this._value1; }
            set
            {
                this._value1 = value;
                this.NotifyPropertyChanged("Value1");
            }
        }
        private string _value2 = string.Empty;
        public string Value2
        {
            get { return this._value2; }
            set
            {
                this._value2 = value;
                this.NotifyPropertyChanged("Value2");
            }
        }
        private string _value3 = string.Empty;
        public string Value3
        {
            get { return this._value3; }
            set
            {
                this._value3 = value;
                this.NotifyPropertyChanged("Value3");
            }
        }
        private string _value4 = string.Empty;
        public string Value4
        {
            get { return this._value4; }
            set
            {
                this._value4 = value;
                this.NotifyPropertyChanged("Value4");
            }
        }

        #endregion



    }
}
