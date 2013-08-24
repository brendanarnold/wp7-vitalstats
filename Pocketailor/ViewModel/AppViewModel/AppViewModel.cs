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

        #region Phone resolution properties and methods

        public double ScreenWidth
        {
            get { return App.Current.Host.Content.ActualWidth; }
        }

        public double ScreenHeight
        {
            get { return App.Current.Host.Content.ActualHeight; }
        }


        #endregion

        # region Application Theme methods/properties

        private ApplicationTheme? _applicationTheme;
        public ApplicationTheme ApplicationTheme
        {
            get 
            { 
                // This reflects the current application theme - not the stored value
                if (this._applicationTheme == null) this._applicationTheme = ThemeHelpers.GetTheme();
                return (ApplicationTheme)this._applicationTheme;
            }
            
        }


        #endregion


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
                App.Settings.Save();
            }

        }

        #endregion


        #region NumberOfLaunches methods/properties


        public bool ReadyToRate
        {
            get { return this.NumberOfLaunches > AppConstants.NUM_BOOTS_TIL_READY_TO_RATE; }
        }

        public int NumberOfLaunches { get; set; }

        public void AddALaunch() 
        {
            int count = App.Settings.GetValueOrDefault<int>("NumberOfLaunches", -1);
            if (count == -1)
            {
                App.Settings.AddOrUpdateValue("NumberOfLaunches", 1);
            }
            else
            {
                if (count >= Int32.MaxValue)
                {
                    count = AppConstants.NUM_BOOTS_TIL_READY_TO_RATE + 1;
                }
                App.Settings.AddOrUpdateValue("NumberOfLaunches", count + 1);
                this.NumberOfLaunches = count;
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
                App.Settings.Save();
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
                    App.Settings.Save();
                }
                return guid;
            }
        }

        #endregion

        
        #region Conversions database methods

        // No need for write methods since this is a readonly DB
        private string ConversionsDatabaseConnectionString;
        private ConversionsDataContext conversiondsDB;

        public void ConnectToConversionsDB(string conversionsDbConnectionString)
        {
             // Open up conversions db
            this.ConversionsDatabaseConnectionString = conversionsDbConnectionString;
            this.RecreateConversionsDataContext();
        }


        public void RecreateConversionsDataContext()
        {
            this.conversiondsDB = null;
            this.conversiondsDB = new ConversionsDataContext(this.ConversionsDatabaseConnectionString);
        }

        #endregion


        #region App database methods

        private string AppDatabaseConnectionString;
        private AppDataContext appDB;


        public void ConnectToAppDB(string appDbConnectionString)
        {
            // Open up app db
            this.AppDatabaseConnectionString = appDbConnectionString;
            this.RecreateDataContext();
        }

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


        #region Trial methods

        private bool _isTrial = true;
        public bool IsTrial
        {
            get { return this._isTrial; }
        }

        public void UpdateLicenseInfo()
        {
            Microsoft.Phone.Marketplace.LicenseInformation licenseInfo = new Microsoft.Phone.Marketplace.LicenseInformation();
#if TEST_TRIAL
            this._isTrial = true;
#else
            this._isTrial = licenseInfo.IsTrial();
#endif

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
