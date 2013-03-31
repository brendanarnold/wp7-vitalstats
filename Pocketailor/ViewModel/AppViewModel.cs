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

namespace Pocketailor.ViewModel
{

    // This collects together the properties exposed to the view and 
    // the methods that act on them that are required for the MainPage.xaml
    // file to be displayed - see other files in the ViewModel directory for
    // other parts of this class
    public partial class AppViewModel : INotifyPropertyChanged
    {
 
        public IsolatedStorageSettings stngs = IsolatedStorageSettings.ApplicationSettings;





        #region Region methods/properties

        public void SetSelectedRegions(List<RegionTag> regionTags)
        {
            if (this.stngs.Contains("SelectedRegions"))
            {
                this.stngs["SelectedRegions"] = regionTags;
            }
            else
            {
                this.stngs.Add("SelectedRegions", regionTags);
            }
            this.stngs.Save();
        }

        public List<RegionTag> GetSelectedRegions()
        {
            if (this.stngs.Contains("SelectedRegions"))
                return this.stngs["SelectedRegions"] as List<RegionTag>;
            return AppConstants.DEFAULT_REGIONS;
            //return new List<RegionTag>() { RegionTag.UK, RegionTag.Europe };
        }

        private ObservableCollection<RegionContainer> _regions;
        public ObservableCollection<RegionContainer> Regions
        {
            get
            {
                if (this._regions == null) this.LoadRegions();
                return this._regions;
            }
            set
            {
                if (this._regions != value)
                {
                    this._regions = value;
                    this.NotifyPropertyChanged("Regions");
                }
            }
        }

        public void LoadRegions()
        {
            this._regions = new ObservableCollection<RegionContainer>();
            List<RegionTag> selectedRegions = this.GetSelectedRegions();
            foreach (RegionTag r in typeof(RegionTag).GetFields().Where(x => x.IsLiteral).Select(x => x.GetValue(typeof(RegionTag))).Cast<RegionTag>())
            {
                this._regions.Add(new RegionContainer { Name = r.ToString(), Id = r, Selected = selectedRegions.Contains(r) });
            }
        }

        public class RegionContainer
        {
            public string Name { get; set; }
            public RegionTag Id { get; set; }
            public bool Selected { get; set; }
        }

        #endregion

        #region HasMeasurement properties


        // Helkper method to notify the View of possible updates to HasMeasurement properties
        internal void RefreshRequiredMeasurement()
        {
            this.NotifyPropertyChanged("HasDressSizeMeasurements");
            this.NotifyPropertyChanged("HasSuitMeasurements");
            this.NotifyPropertyChanged("HasTrouserMeasurements");
            this.NotifyPropertyChanged("HasShirtMeasurements");
            this.NotifyPropertyChanged("HasHatMeasurements");
            this.NotifyPropertyChanged("HasBraMeasurements");
            this.NotifyPropertyChanged("HasHosieryMeasurements");
            this.NotifyPropertyChanged("HasShoeMeasurements");
            this.NotifyPropertyChanged("HasSkiBootMeasurements");
            this.NotifyPropertyChanged("HasWetsuitMeasurements");
            this.NotifyPropertyChanged("HasTennisGripMeasurements");
        }


        public bool HasRequiredMeasurements(List<MeasurementId> requiredIds)
        {
            IEnumerable<MeasurementId> statIds = from Stat s in this.SelectedProfile.Stats select s.MeasurementId;
            foreach (MeasurementId id in requiredIds)
                if (!statIds.Contains(id)) return false;
            return true;
        }

        public bool HasTrouserMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.TrousersUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.TrousersUtils.RequiredMeasurementsWomens);
                }
            }
        }

        public bool HasShirtMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.ShirtUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.ShirtUtils.RequiredMeasurementsWomens);
                }

            }
        }

        public bool HasHatMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.HatUtils.RequiredMeasurements);
            }
        }

        public bool HasSuitMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.SuitUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.SuitUtils.RequiredMeasurementsWomens);
                }

            }
        }

        public bool HasDressSizeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.DressSizeUtils.RequiredMeasurements);
            }
        }

        public bool HasBraMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.BraUtils.RequiredMeasurements);
            }
        }

        public bool HasHosieryMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.HosieryUtils.RequiredMeasurements);
            }
        }

        public bool HasShoeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.ShoesUtils.RequiredMeasurements);
            }
        }

        public bool HasSkiBootMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.SkiBootsUtils.RequiredMeasurements);
            }
        }

        public bool HasTennisGripMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.TennisRaquetSizesUtils.RequiredMeasurements);
            }
        }

        public bool HasWetsuitMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.WetsuitUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.WetsuitUtils.RequiredMeasurementsWomens);
                }
            }
        }

        #endregion

        #region ConversionsPage methods/properties

        private ConversionId _selectedConversionType;
        public ConversionId SelectedConversionType
        {
            get
            {
                return this._selectedConversionType;
            }
            set
            {
                if (this._selectedConversionType != value)
                {
                    this._selectedConversionType = value;
                    this.NotifyPropertyChanged("SelectedConversionType");
                }
            }
        }

        private string _conversionsByRegionPageTitle;
        public string ConversionsByRegionPageTitle {
            get
            {
                return this._conversionsByRegionPageTitle;
            }
            set
            {
                if (this._conversionsByRegionPageTitle != value)
                {
                    this._conversionsByRegionPageTitle = value;
                    this.NotifyPropertyChanged("ConversionsByRegionPageTitle");
                }
            }
        }

        private BitmapImage _conversionsByRegionPageBGImage;
        public BitmapImage ConversionsByRegionPageBGImage
        { 
            get 
            {
                return this._conversionsByRegionPageBGImage;
            }
            set
            {
                if (this._conversionsByRegionPageBGImage != value)
                {
                    this._conversionsByRegionPageBGImage = value;
                    this.NotifyPropertyChanged("ConversionsByRegionPageBGImage");
                }
                
            }
        }
        
        private ObservableCollection<ConversionRegion> _conversionByRegion;
        public ObservableCollection<ConversionRegion> ConversionsByRegion
        {
            get { return this._conversionByRegion; }
            set 
            {
                if (this._conversionByRegion != value)
                {
                    this._conversionByRegion = value;
                    this.NotifyPropertyChanged("ConversionRegions");
                }
            }
        }

        public void LoadConversionsPageData()
        {
            // Reset the list
            this.ConversionsByRegion = new ObservableCollection<ConversionRegion>();
            // Declare vars in the top scope
            List<double> measuredVals = new List<double>();
            // TODO: remove this hack needed to compile
            IEnumerable<Model.Conversions.IConversionData> dataQuery = appDB.DressSizes.Cast<Model.Conversions.IConversionData>();;
            // Make sure we have region data
            if (this.GetSelectedRegions() == null) return;

            // Get the conversion specific data
            switch (this.SelectedConversionType)
            {
                case ConversionId.TrouserSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.TrousersUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.TrousersUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Trousers.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.ShirtSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShirtUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShirtUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Shirts.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.HatSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.HatUtils.RequiredMeasurements);
                    dataQuery = appDB.Hats.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.SuitSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SuitUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SuitUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Suits.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.DressSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.DressSizeUtils.RequiredMeasurements);
                    dataQuery = appDB.DressSizes.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.BraSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.BraUtils.RequiredMeasurements);
                    dataQuery = appDB.Bras.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.HosierySize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.HosieryUtils.RequiredMeasurements);
                    dataQuery = appDB.Hosiery.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.ShoeSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShoesUtils.RequiredMeasurements);
                    dataQuery = appDB.Shoes.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.SkiBootSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SkiBootsUtils.RequiredMeasurements);
                    dataQuery = appDB.SkiBoots.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.TennisGripSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.TennisRaquetSizesUtils.RequiredMeasurements);
                    dataQuery = appDB.TennisRaquetSizes.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.WetsuitSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.WetsuitUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.WetsuitUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Wetsuits.Cast<Model.Conversions.IConversionData>();
                    break;
                default:
                    return;
            }
            // Check we have all the necessary measurements
            if (measuredVals == null) return;
            // Build up by regions
            foreach (RegionTag region in this.GetSelectedRegions())
            {
                ConversionRegion cr = new ConversionRegion();
                cr.Name = region.ToString(); // TODO: Will need to include some kind of lookup for the actual string
                cr.Conversions = new ObservableCollection<NameValuePair>();
                // TODO: If gender not specified, then return Female measurements
                Gender qGender = (this.SelectedProfile.Gender == Gender.Unspecified) ? Gender.Female : this.SelectedProfile.Gender;
                // TODO: Unresolve this list used for debuggin purposes
                var dataByRegion = dataQuery.Where(ds => (ds.Region == region) && (ds.Gender == qGender)).ToList();
                foreach (RetailId retailId in dataByRegion.Select(ds => ds.Retailer).Distinct())
                {
                    var conversionData = dataByRegion.Where(ds => ds.Retailer == retailId);
                    double lowestChisq = double.MaxValue;
                    Model.Conversions.IConversionData bestFit = null;
                    foreach (Model.Conversions.IConversionData candidateConversion in conversionData)
                    {
                        double chiSq = candidateConversion.GetChiSq(measuredVals);
                        if (chiSq < lowestChisq)
                        {
                            lowestChisq = chiSq;
                            bestFit = candidateConversion;
                        }
                    }
                    cr.Conversions.Add(new NameValuePair()
                    {
                        // TODO: Will need to include some kind of lookup for actual string
                        Name = retailId.ToString(),
                        FormattedValue = bestFit.FormattedValue,
                        Retailer = retailId,
                    });
                }
                this.ConversionsByRegion.Add(cr);
            }
        }


        // Already been checked that these values have been taken
        public List<double> GetRequiredMeasuredValues(List<MeasurementId> requiredIds)
        {
            List<double> requiredValues = new List<double>();
            foreach (MeasurementId mID in requiredIds)
            {
                Stat s = App.VM.SelectedProfile.Stats.FirstOrDefault(x => x.MeasurementId == mID);
                if (s == null) return null;
                double d;
                if (double.TryParse(s.Value, out d))
                {
                    requiredValues.Add(d);
                }
                else
                {
                    return null;
                }
            }
            return requiredValues;
        }

        internal List<MeasurementId> GetMissingMeasurements(List<MeasurementId> requiredIds)
        {
            return requiredIds.Except(this.SelectedProfile.Stats.Select(s => s.MeasurementId)).ToList();
        }

        public class ConversionRegion
        {
            public string Name { get; set; }
            public ObservableCollection<NameValuePair> Conversions { get; set; }
        }

        public class NameValuePair : INotifyPropertyChanged
        {
            public NameValuePair()
            {

                App.VM.HiddenRetailers.CollectionChanged += (s, e) => {
                    this.NotifyPropertyChanged("IsHidden");
                    this.NotifyPropertyChanged("IsVisible");
                };
                App.VM.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "ShowHiddenConversions")
                        this.NotifyPropertyChanged("IsVisible");
                };
            }

            public string Name { get; set; }
            public string FormattedValue { get; set; }
            public RetailId Retailer { get; set; }
            // Flag set to true if user has chosen to hide this retailer e.g. show but grey this entry out
            public bool IsHidden 
            {
                get
                {
                    return App.VM.HiddenRetailers.Contains(this.Retailer);
                }
            }
            // Flag set to true is user wants to see all retailers (even hidden) e.g. show this retailer at all
            public bool IsVisible
            {
                get
                {
                    if (!this.IsHidden) {
                        return true;
                    } else {
                        return App.VM.ShowHiddenConversions;
                    }
                }
            }

            public void ToggleHidden()
            {
                if (this.IsHidden)
                {
                    App.VM.HiddenRetailers.Remove(this.Retailer);
                }
                else
                {
                    App.VM.HiddenRetailers.Add(this.Retailer);
                }
                //this.NotifyPropertyChanged("IsHidden");
                //if (!App.VM.ShowHiddenConversions)
                //{
                //    this.NotifyPropertyChanged("IsVisible");
                //}
                App.VM.SaveHiddenRetailers();
            }

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

        private ObservableCollection<RetailId> _hiddenRetailers;
        public ObservableCollection<RetailId> HiddenRetailers
        {
            get
            {
                if (this._hiddenRetailers == null)
                    this.LoadHiddenRetailers();
                return this._hiddenRetailers;
            }
            set
            {
                if (this._hiddenRetailers != value)
                {
                    this._hiddenRetailers = value;
                    this.NotifyPropertyChanged("HiddenRetailers");
                }
            }
        }

        public void LoadHiddenRetailers()
        {
            if (this.stngs.Contains("HiddenRetailers"))
            {
                this.HiddenRetailers = (ObservableCollection<RetailId>)this.stngs["HiddenRetailers"];
            }
            else
            {
                this.HiddenRetailers = new ObservableCollection<RetailId>();
            }
        }

        public void SaveHiddenRetailers()
        {
            if (this.stngs.Contains("HiddenRetailers"))
            {
                this.stngs["HiddenRetailers"] = this.HiddenRetailers;
            }
            else
            {
                this.stngs.Add("HiddenRetailers", this.HiddenRetailers);
            }
        }

        private bool _showHiddenConversions = false;
        public bool ShowHiddenConversions
        {
            get { return this._showHiddenConversions; }
            set
            {
                if (this._showHiddenConversions != value)
                {
                    this._showHiddenConversions = value;
                    this.NotifyPropertyChanged("ShowHiddenConversions");
                }
            }
        }


        #endregion




        #region PIN locking methods

        private bool _isLocked = true;
        public bool IsLocked
        {
            get { return this._isLocked; }
            set
            {
                if (this._isLocked != value)
                {
                    this._isLocked = value;
                    this.NotifyPropertyChanged("IsLocked");
                }
            }
        }

        public bool IsValidPin(string pin)
        {
            if (pin.Length != 4) return false;
            for (int i = 0; i < 4; i++)
            {
                if (!char.IsDigit(pin[i])) return false;
            }
            return true;
        }

        public bool SetPin(string pin)
        {
            if (this.IsLocked) return false;
            if (this.stngs.Contains("pin"))
            {
                this.stngs["pin"] = pin;
            }
            else
            {
                this.stngs.Add("pin", pin);
            }
            return true;
        }

        public string GetPin()
        {
            string pin;
            try
            {
                pin = (string)this.stngs["pin"];
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                pin = null;
                this.SetPin(null);
            }
            return pin;
        }

        public bool TryUnlock(string pin)
        {
            string appPin;

            appPin = this.GetPin();
            if (appPin == null) 
            {
                this.IsLocked = false;
                if (this.IsValidPin(pin)) this.SetPin(pin);
                return true;
            } 
            else if (appPin == pin)
            {
                this.IsLocked = false;
                return true;
            }
            else
            {
                this.IsLocked = true;
                return false;
            }

        }

        public void Lock()
        {
            this.IsLocked = true;
        }

        #endregion


        #region QuickProfile methods

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
        }

        public void LoadQuickProfilesFromDB()
        {
            // Load from memory if possible, quicker I imagine ...
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
            if (profile.IsQuickProfile) this.QuickProfiles.Add(profile); 
        }

        public void UpdateProfile(Profile profile)
        {
            if (profile.IsQuickProfile && !this.QuickProfiles.Contains(profile)) this.QuickProfiles.Add(profile);
            if (!profile.IsQuickProfile && this.QuickProfiles.Contains(profile)) this.QuickProfiles.Remove(profile);
            App.VM.SaveChangesToDB();
        }

        public void DeleteProfile(Profile profile)
        {
            this.Profiles.Remove(profile);
            if (this.QuickProfiles.Contains(profile))
                this.QuickProfiles.Remove(profile);
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

        private StatTemplate _suggestedStatTemplate;
        public StatTemplate SuggestedStatTemplate
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
            List<MeasurementId> suggIds = (from StatTemplate st in this.StatTemplates
                                      select
                                          st.Id).Except(from Stat st in this.SelectedProfile.Stats
                                                          select st.MeasurementId).ToList();
            // Check if any suggested stats left over
            if (suggIds.Count == 0)
            {
                this.SuggestedStatTemplate = null;
                return;
            }
            int ind = this._suggestedStatTemplateInd % suggIds.Count;
            this.SuggestedStatTemplate = (from StatTemplate st in this.StatTemplates
                                          where st.Id == suggIds[ind]
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
            = new ObservableCollection<StatTemplate>(Model.Static.StatTemplates);
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






    }

    public partial class ViewModelLocator
    {
        public AppViewModel AppViewModel
        {
            get { return App.VM; }
        }
    }


}
