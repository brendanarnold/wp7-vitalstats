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

        public bool HasTrouserMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_TROUSER);
            }
        }

        public bool HasShirtMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_SHIRT);
            }
        }

        public bool HasHatMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_HAT);
            }
        }

        public bool HasSuitMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_SUIT_MENS);
                }
                else
                {
                    return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_SUIT_WOMENS);
                }
            }
        }

        public bool HasDressSizeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_DRESS_SIZE);
            }
        }

        public bool HasBraMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_BRA);
            }
        }

        public bool HasHosieryMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_HOSIERY);
            }
        }

        public bool HasShoeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_SHOES);
            }
        }

        public bool HasSkiBootMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_SKIBOOTS);
            }
        }

        public bool HasTennisGripMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_TENNISGRIP);
            }
        }

        public bool HasWetsuitMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(AppConstants.REQUIRED_MEASUREMENTS_WETSUIT);
            }
        }

        #endregion

        #region Conversion methods/properties

        
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

        public void LoadConversionRegions(Model.ConversionId conversionId)
        {
            this.ConversionsByRegion = new ObservableCollection<ConversionRegion>();
            switch (conversionId)
            {
                case ConversionId.DressSize:
                    // First check we have the necessary measurements on the profile as well as some regions selected
                    Stat chest = App.VM.SelectedProfile.Stats.FirstOrDefault(x => x.MeasurementId == MeasurementId.Chest);
                    Stat waist = App.VM.SelectedProfile.Stats.FirstOrDefault(x => x.MeasurementId == MeasurementId.Waist);
                    Stat hips = App.VM.SelectedProfile.Stats.FirstOrDefault(x => x.MeasurementId == MeasurementId.Hips);
                    if (chest == null || waist == null || hips == null 
                        || this.GetSelectedRegions() == null)
                    {
                        return;
                    }
                    foreach (RegionTag region in this.GetSelectedRegions())
                    {
                        ConversionRegion cr = new ConversionRegion();
                        cr.Name = region.ToString(); // TODO: Wll need to include some kind of lookup for the actual string
                        cr.Conversions = new ObservableCollection<NameValuePair>();
                        var regionalDressSizes = appDB.DressSizes.Where(ds => ds.Region == region);
                        foreach (RetailId retailId in regionalDressSizes.Select(ds => ds.Retailer).Distinct())
                        {
                            NameValuePair nvp = new NameValuePair();
                            nvp.Name = retailId.ToString(); // TODO: Will need to include some kind of lookup for actual string
                            var conversionData = regionalDressSizes.Where(ds => ds.Retailer == retailId);
                            // Do a least squares 'fit' to find best size
                            double lowestChisq = double.MaxValue;
                            Model.Conversions.DressSize bestFitDressSize = null;
                            foreach (Model.Conversions.DressSize ds in conversionData)
                            {
                                // If at this stage should always be measured values, however some retailers do no provide measurements for
                                // chest, hips and waist in their tables. Ignore these value in the least square fits.
                                double measuredChest = double.Parse(chest.Value);
                                double measuredWaist = double.Parse(waist.Value);
                                double measuredHips = double.Parse(hips.Value);
                                double numChest = (ds.Chest.HasValue) ? (double)ds.Chest : measuredChest;
                                double numWaist = (ds.Waist.HasValue) ? (double)ds.Waist : measuredWaist;
                                double numHips = (ds.Hips.HasValue) ? (double)ds.Hips : measuredHips;
                                double chisq = Math.Pow(measuredChest - numChest, 2)
                                    + Math.Pow(measuredWaist - numWaist, 2)
                                    + Math.Pow(measuredHips - numHips, 2);
                                if (chisq < lowestChisq)
                                {
                                    lowestChisq = chisq;
                                    bestFitDressSize = ds;
                                }
                            }
                            nvp.FormattedValue = bestFitDressSize.FormattedValue;
                            cr.Conversions.Add(nvp);
                        }
                        this.ConversionsByRegion.Add(cr);
                    }
                    break;
                default:
                    break;
            }
        }

        public bool HasRequiredMeasurements(List<MeasurementId> requiredIds)
        {
            IEnumerable<MeasurementId>  statIds = from Stat s in this.SelectedProfile.Stats select s.MeasurementId;
            foreach (MeasurementId id in requiredIds)
                if (!statIds.Contains(id)) return false;
            return true;
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

        public class NameValuePair
        {
            public string Name { get; set; }
            public string FormattedValue { get; set; }
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







        // Notify the View of possible updates to the conversion availabilities
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
    }

    public partial class ViewModelLocator
    {
        public AppViewModel AppViewModel
        {
            get { return App.VM; }
        }
    }


}
