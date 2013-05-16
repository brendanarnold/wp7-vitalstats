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
                if (this._unitCulture == null) this._unitCulture = App.Settings.GetValueOrDefault<UnitCultureId>("UnitCulture", AppConstants.DEFAULT_UNIT_CULTURE);
                return (UnitCultureId)this._unitCulture;
            }
            set
            {
                this._unitCulture = value;
                App.Settings.AddOrUpdateValue("UnitCulture", value);
            }

        }

        #endregion

        
        #region AllowFeedback property

        private bool? _allowFeedback = null;
        public bool? AllowFeedBack
        {
            get
            {
                if (this._allowFeedback == null) this._allowFeedback = App.Settings.GetValueOrDefault<bool?>("AllowFeedback", AppConstants.DEFAULT_ALLOW_FEEDBACK);
                return this._allowFeedback;
            }
            set
            {
                this._allowFeedback = value;
                App.Settings.AddOrUpdateValue("AllowFeedback", value);
            }
        }

        #endregion


        #region App GUID property

        public string AppGuid
        {
            get
            {
                string guid = App.Settings.GetValueOrDefault<string>("AppGuid", String.Empty);
                if (guid == String.Empty)
                {
                    guid = Guid.NewGuid().ToString();
                    App.Settings.AddOrUpdateValue("AppGuid", guid);
                }
                return guid;
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
        #region SuggestedMeasurement methods/proprties

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


        #region SelectedMeasurement methods/properties

        private Measurement _selectedMeasurement;
        public Measurement SelectedMeasurement
        {
            get { return this._selectedMeasurement; }
            set
            {
                this._selectedMeasurement = value;
                this.NotifyPropertyChanged("SelectedMeasurement");
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


        #region MeasurementTemplates methods/properties

        private ObservableCollection<MeasurementTemplate> _measurementTemplates 
            = new ObservableCollection<MeasurementTemplate>(Model.Static.MeasurementTemplates);
        public ObservableCollection<MeasurementTemplate> MeasurementTemplates
        {
            get { return this._measurementTemplates; }
            set
            {
                this._measurementTemplates = value;
                this.NotifyPropertyChanged("MeasurementTemplates");
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
