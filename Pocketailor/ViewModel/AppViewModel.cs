using System.Collections.ObjectModel;
using Pocketailor.Model;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq;
using Microsoft.Phone.Data.Linq.Mapping;
using System.Collections;
using System.IO.IsolatedStorage;
using System;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace Pocketailor.ViewModel
{

    // This collects together the properties exposed to the view and 
    // the methods that act on them that are required for the MainPage.xaml
    // file to be displayed - see other files in the ViewModel directory for
    // other parts of this class
    public partial class AppViewModel : INotifyPropertyChanged
    {
        // Empty constructor needed for design-time data created in XAML
        public AppViewModel() { }

        public AppViewModel(string appDbConnectionString, string conversionsDbConnectionString)
        {
            // Open up app db
            this.AppDatabaseConnectionString = appDbConnectionString;
            this.RecreateDataContext();
            // Open up conversions db
            this.ConversionsDatabaseConnectionString = conversionsDbConnectionString;
            this.RecreateConversionsDataContext();
        }


        #region UnitCulture property

        // Cache this for fast access
        private UnitCultureId? _unitCulture = null;
        public UnitCultureId UnitCulture
        {
            get
            {
                if (this._unitCulture == null) this._unitCulture = App.Settings.GetValueOrDefault<UnitCultureId>("_unitCulture", AppConstants.DEFAULT_UNIT_CULTURE);
                return (UnitCultureId)this._unitCulture;
            }
            set
            {
                this._unitCulture = value;
                App.Settings.AddOrUpdateValue("_unitCulture", value);
            }

        }

        #endregion


        #region Conversions database methods

        // No need for write methods since this is a readonly DB
        private string ConversionsDatabaseConnectionString;
        private ConversionsDataContext conversiondsDB;

        public void RecreateConversionsDataContext()
        {
            this.conversiondsDB = null;
            this.conversiondsDB = new ConversionsDataContext(this.ConversionsDatabaseConnectionString);
        }

        #endregion


        #region App database methods

        private string AppDatabaseConnectionString;
        private AppDataContext appDB;

        // Checks for uncommitted changes in the ORM
        public bool IsPendingChangesForDB()
        {
            ChangeSet changes = this.appDB.GetChangeSet();
            return ((changes.Updates.Count > 0) || (changes.Inserts.Count > 0) || (changes.Deletes.Count > 0));
        }

        // Recreates the database context, cleaning the uncommitted changes from the ORM
        public void RecreateDataContext()
        {
            this.appDB = null;
            this.appDB = new AppDataContext(this.AppDatabaseConnectionString);
        }

        // Save the cache's changes in the ORM to the DB
        public void SaveChangesToAppDB()
        {
            this.appDB.SubmitChanges();
        }

        #endregion


       
     
        // Not used
        #region PIN locking methods

        //private bool _isLocked = true;
        //public bool IsLocked
        //{
        //    get { return this._isLocked; }
        //    set
        //    {
        //        if (this._isLocked != value)
        //        {
        //            this._isLocked = value;
        //            this.NotifyPropertyChanged("IsLocked");
        //        }
        //    }
        //}

        //public bool IsValidPin(string pin)
        //{
        //    if (pin.Length != 4) return false;
        //    for (int i = 0; i < 4; i++)
        //    {
        //        if (!char.IsDigit(pin[i])) return false;
        //    }
        //    return true;
        //}

        //public bool SetPin(string pin)
        //{
        //    if (this.IsLocked) return false;
        //    if (this.stngs.Contains("pin"))
        //    {
        //        this.stngs["pin"] = pin;
        //    }
        //    else
        //    {
        //        this.stngs.Add("pin", pin);
        //    }
        //    return true;
        //}

        //public string GetPin()
        //{
        //    string pin;
        //    try
        //    {
        //        pin = (string)this.stngs["pin"];
        //    }
        //    catch (System.Collections.Generic.KeyNotFoundException)
        //    {
        //        pin = null;
        //        this.SetPin(null);
        //    }
        //    return pin;
        //}

        //public bool TryUnlock(string pin)
        //{
        //    string appPin;

        //    appPin = this.GetPin();
        //    if (appPin == null) 
        //    {
        //        this.IsLocked = false;
        //        if (this.IsValidPin(pin)) this.SetPin(pin);
        //        return true;
        //    } 
        //    else if (appPin == pin)
        //    {
        //        this.IsLocked = false;
        //        return true;
        //    }
        //    else
        //    {
        //        this.IsLocked = true;
        //        return false;
        //    }

        //}

        //public void Lock()
        //{
        //    this.IsLocked = true;
        //}

        #endregion
      

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
            this.NotifyPropertyChanged("Profiles");
            if (profile.IsQuickProfile)
            {
                this.QuickProfiles.Add(profile);
                this.NotifyPropertyChanged("QuickProfiles");
            }
        }

        public void UpdateProfile(Profile profile)
        {
            if (profile.IsQuickProfile && !this.QuickProfiles.Contains(profile)) this.QuickProfiles.Add(profile);
            if (!profile.IsQuickProfile && this.QuickProfiles.Contains(profile)) this.QuickProfiles.Remove(profile);
            App.VM.SaveChangesToAppDB();
        }

        public void DeleteProfile(Profile profile)
        {
            this.Profiles.Remove(profile);
            if (this.QuickProfiles.Contains(profile))
                this.QuickProfiles.Remove(profile);
            this.appDB.Profiles.DeleteOnSubmit(profile);
            this.appDB.SubmitChanges();
            this.NotifyPropertyChanged("Profiles");
            this.NotifyPropertyChanged("QuickProfiles");
        }

        public void AddStatToProfile(Stat stat, Profile profile)
        {
            stat.Profile = profile;
            profile.Stats.Add(stat);
            this.appDB.Stats.InsertOnSubmit(stat);
            this.appDB.SubmitChanges();
            profile.NotifyPropertyChanged("Stats");
        }

        public void DeleteStatFromProfile(Stat stat, Profile profile)
        {
            profile.Stats.Remove(stat);
            this.appDB.Stats.DeleteOnSubmit(stat);
            this.appDB.SubmitChanges();
            profile.NotifyPropertyChanged("Stats");
        }

        //public void ToggleIsProtected(Profile profile)
        //{
        //    profile.IsProtected = !profile.IsProtected; 
        //}

        #endregion


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

        // Not used
        #region SuggestedStat methods/proprties

        //private StatTemplate _suggestedStatTemplate;
        //public StatTemplate SuggestedStatTemplate
        //{
        //    get { return this._suggestedStatTemplate; }
        //    set
        //    {
        //        this._suggestedStatTemplate = value;
        //        this.NotifyPropertyChanged("SuggestedStatTemplate");
        //    }
        //}

        //// Following is some code to present a different suggested stat that has not already been
        //// used in the selectedProfile
        //private int _suggestedStatTemplateInd = 0;
        //public void LoadNextSuggestedStatTemplate()
        //{

        //    this._suggestedStatTemplateInd += 1;
        //    List<MeasurementId> suggIds = (from StatTemplate st in this.StatTemplates
        //                              select
        //                                  st.Id).Except(from Stat st in this.SelectedProfile.Stats
        //                                                  select st.MeasurementId).ToList();
        //    // Check if any suggested stats left over
        //    if (suggIds.Count == 0)
        //    {
        //        this.SuggestedStatTemplate = null;
        //        return;
        //    }
        //    int ind = this._suggestedStatTemplateInd % suggIds.Count;
        //    this.SuggestedStatTemplate = (from StatTemplate st in this.StatTemplates
        //                                  where st.Id == suggIds[ind]
        //                                  select st).First();
        //}


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
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion


        #region MeasurementTypes methods/properties

        private ObservableCollection<MeasurementType> _measurementTypes = new ObservableCollection<MeasurementType>(Static.MeasurementTypes);
        public ObservableCollection<MeasurementType> MeasurementTypes
        {
            get { return this._measurementTypes; }
            set
            {
                this._measurementTypes = value;
                this.NotifyPropertyChanged("MeasurementTypes");
            }
        }


        #endregion


        #region StatTemplates methods/properties

        private ObservableCollection<StatTemplate> _statTemplates 
            = new ObservableCollection<StatTemplate>(Model.Static.MeasurementTemplates);
        public ObservableCollection<StatTemplate> StatTemplates
        {
            get { return this._statTemplates; }
            set
            {
                this._statTemplates = value;
                this.NotifyPropertyChanged("StatTemplates");
            }
        }

        #endregion


        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion



 
    }



}
