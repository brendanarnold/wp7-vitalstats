using System.Collections.ObjectModel;
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




        private ObservableCollection<Profile> _quickProfiles;
        public ObservableCollection<Profile> QuickProfiles
        {
            get { return this._quickProfiles; }
            set
            {
                this._quickProfiles = value;
                this.NotifyPropertyChanged("QuickProfiles");
            }
        }

        public void ToggleQuickProfile(Profile p)
        {
            p.IsQuickProfile = !p.IsQuickProfile;
            this.LoadQuickProfilesFromDB();
            this.NotifyPropertyChanged("QuickProfiles");
            
        }

        public void LoadQuickProfilesFromDB()
        {
            IEnumerable<Profile> profiles;
            if (this.Profiles.Any())
            {
                profiles = from Profile p in this.Profiles where p.IsQuickProfile == true select p;
            }
            else
            {
                profiles = from Profile p in this.appDB.Profiles where p.IsQuickProfile == true select p;
            }
            this.QuickProfiles = new ObservableCollection<Profile>(profiles);
        }

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
            if (this.QuickProfiles.Contains(profile))
            {
                this.QuickProfiles.Remove(profile);
                this.NotifyPropertyChanged("QuickProfiles");
            }
            this.appDB.Profiles.DeleteOnSubmit(profile);
            this.appDB.SubmitChanges();
        }

        public void ToggleIsProtected(Profile profile)
        {
            profile.IsProtected = !profile.IsProtected; 
        }

        #endregion

        public void AddStatToProfile(Stat stat, Profile profile)
        {
            stat.Profile = profile;
            profile.Stats.Add(stat);
            this.appDB.Stats.InsertOnSubmit(stat);
            this.appDB.SubmitChanges();
        }

        public void DeleteStatFromProfile(Stat stat, Profile profile)
        {
            profile.Stats.Remove(stat);
            this.appDB.Stats.DeleteOnSubmit(stat);
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

        


        // Allows non-numeric values when the stat type is custom i.e. does not allow for conversions
        public bool AllowNonNumericValue()
        {
            if (this.SelectedStat.MeasurementType != null)
            {
                return !this.SelectedStat.MeasurementType.IsConvertible();
            }
            else
            {
                return true;
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

        // Inject an 'Other' measurement type at the start
        public void LoadMeasurementTypesFromDB()
        {
            List<MeasurementType> mts = (from MeasurementType mt in this.appDB.MeasurementTypes select mt).ToList();
            //MeasurementType customMt = new MeasurementType()
            //{
            //    Name = AppConstants.NAME_CUSTOM_MEASUREMENT_TYPE,
            //    Units = null,
            //};
            //mts.Insert(0, customMt);
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

        // Inject 'Other' template at the start of the list
        public void LoadStatTemplatesFromDB()
        {
            List<Stat> stats = (from Stat s in this.appDB.StatTemplates select s).ToList();
            //Stat customStat = new Stat()
            //{
            //    Name = AppConstants.NAME_CUSTOM_STAT_TEMPLATE
            //};
            //stats.Insert(0, customStat);
            this.StatTemplates = new ObservableCollection<Stat>(stats);
        }

        #endregion

        


    }
}
