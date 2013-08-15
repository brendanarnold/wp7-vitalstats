using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel
    {

        // Should load only the required data if it isn't present
        public void LoadMeasurementsPageData(int profileId)
        {
            // Only load data if it is not present or is for the wrong profile
            if (this.SelectedProfile == null || this.SelectedProfile.Id != profileId)
            {
                this.SelectedProfile = (from Profile p in this.Profiles where p.Id == profileId select p).First();
                if (this.ViewingUnitCulture == null) this.ViewingUnitCulture = this.UnitCulture;
                this.LoadFullMeasurements();
            } else {
                if (this.ViewingUnitCulture == null) this.ViewingUnitCulture = this.UnitCulture;
                if (this.FullMeasurements == null) this.LoadFullMeasurements();
            }
            
        }

        // Helkper method to notify the View of possible updates to HasMeasurement properties
        // TODO: May not be necessary now add profile updated
        internal void RefreshPostMeasurementEdit()
        {
            foreach (ConversionBtnData c in this.Conversions.Values)
                c.NotifyPropertyChanged("HasRequiredMeasurements");
            if (this.CurrentNominatedConversion.HasValue) this.NominateConversion((ConversionId)this.CurrentNominatedConversion);
        }

        // Stores newly unlocked conversions ready for an alert animation in the UI
        private ObservableCollection<ConversionBtnData> _newlyUnlockedConversions;
        public ObservableCollection<ConversionBtnData> NewlyUnlockedConversions 
        {
            get { return this._newlyUnlockedConversions; }
            set
            {
                if (this._newlyUnlockedConversions != value)
                {
                    this._newlyUnlockedConversions = value;
                    this.NotifyPropertyChanged("NewlyUnlockedConversions");
                }
            }
        }


        // Lame variable needed to show the help box
        //public bool ShowMeasurementPageHelpContainer
        //{
        //    get
        //    {
        //        return (this.NewlyUnlockedConversions != null)
        //        && this.CurrentNominatedConversion.HasValue;
        //    }
        //}


        #region Add/remove measurements to/from profile

        public void AddMeasurementToProfile(Measurement measurement, Profile profile)
        {
            // Need to calculate which conversions are unlocked. Take a tally of which are 
            // available before attaching to the profile. Use a List to resolve now.
            List<ConversionBtnData> unlockedBefore = this.Conversions.Values.Where(c => c.HasRequiredMeasurements).ToList();
            
            // Attach to the profile
            measurement.Profile = profile;
            profile.Measurements.Add(measurement);
            this.appDB.Measurements.InsertOnSubmit(measurement);
            this.appDB.SubmitChanges();
            profile.NotifyPropertyChanged("Measurements");
            
            // Need to swap out the placeholder measurement in FullMeasurements for the one attached to the profile
            this.SwapInToFullMeasurements(App.VM.SelectedMeasurement);
            
            // Compare to before
            IEnumerable<ConversionBtnData> unlockedAfter = this.Conversions.Values.Where(c => c.HasRequiredMeasurements);
            List<ConversionBtnData> newlyUnlocked = unlockedAfter.Except(unlockedBefore).ToList();
            List<ConversionBtnData> newlyLocked = unlockedBefore.Except(unlockedAfter).ToList();

            // Update the UI
            foreach (ConversionBtnData c in newlyLocked)
            {
                c.NotifyPropertyChanged("HasRequiredMeasurements");
            }
            // Set the NewlyUnlocked flag on the conversions and update UI
            foreach (ConversionBtnData c in newlyUnlocked)
            {
                c.IsNewlyUnlocked = true;
                c.NotifyPropertyChanged("HasRequiredMeasurements");
            }
            if (newlyUnlocked.Count == 0)
                this.NewlyUnlockedConversions = null;
            else 
                this.NewlyUnlockedConversions = new ObservableCollection<ConversionBtnData>(newlyUnlocked);
            
            //this.NotifyPropertyChanged("ShowMeasurementPageHelpContainer");

            // Check if final measurement entered for currently nominated conversion
            if (this.CurrentNominatedConversion.HasValue
                && this.NewlyUnlockedConversions != null
                && this.NewlyUnlockedConversions.Where(c => c.ConversionId == this.CurrentNominatedConversion).Any())
            {
                // This will activate animations etc. maybe too soon?
                this.CurrentNominatedConversion = null;
            }
        }

        public void CancelNewlyUnlocked()
        {
            foreach (ConversionBtnData c in this.Conversions.Values)
            {
                c.IsNewlyUnlocked = false;
            }
            this.NewlyUnlockedConversions = null;
            this.NotifyPropertyChanged("NewlyUnlockedConversions");
            //this.NotifyPropertyChanged("ShowMeasurementPageHelpContainer");
        }


        public void DeleteMeasurementsFromProfile(Measurement measurement, Profile profile)
        {
            profile.Measurements.Remove(measurement);
            this.appDB.Measurements.DeleteOnSubmit(measurement);
            this.appDB.SubmitChanges();
            profile.NotifyPropertyChanged("Measurements");
            // TODO: similar to above
            
        }

        #endregion

        #region Bindable 'conversion' objects for the conversion button data


        // Convenience data structure to access the conversion data containers
        private Dictionary<ConversionId, ConversionBtnData> _conversions;
        public Dictionary<ConversionId, ConversionBtnData> Conversions
        {
            get
            {
                if (this._conversions == null)
                    this._conversions = new Dictionary<ConversionId, ConversionBtnData>()
                    {
                        { ConversionId.ShirtSize, this.ShirtConversion },
                        { ConversionId.TrouserSize, this.TrouserConversion },
                        { ConversionId.HatSize, this.HatConversion },
                        { ConversionId.SuitSize, this.SuitConversion },
                        { ConversionId.DressSize, this.DressConversion },
                        { ConversionId.BraSize, this.BraConversion },
                        { ConversionId.HosierySize, this.HosieryConversion },
                        { ConversionId.ShoeSize, this.ShoeConversion },
                        { ConversionId.SkiBootSize, this.SkiBootConversion },
                        { ConversionId.WetsuitSize, this.WetsuitConversion },
                    };
                return this._conversions;
            }
        }


        private ConversionBtnData _trouserConversion;
        public ConversionBtnData TrouserConversion
        {
            get
            {
                if (this._trouserConversion == null) this._trouserConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.TrouserSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.TrousersWomens,
                    RequiredMeasurementsMale = RequiredMeasurements.TrousersMens,
                };
                return this._trouserConversion;
            }
            set
            {
                if (this._trouserConversion != value)
                {
                    this._trouserConversion = value;
                    this.NotifyPropertyChanged("TrouserConversion");
                }
            }
        }

        private ConversionBtnData _shirtConversion;
        public ConversionBtnData ShirtConversion
        {
            get
            {
                if (this._shirtConversion == null) this._shirtConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.ShirtSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.ShirtWomens,
                    RequiredMeasurementsMale = RequiredMeasurements.ShirtMens,
                };
                return this._shirtConversion;
            }
            set
            {
                if (this._shirtConversion != value)
                {
                    this._shirtConversion = value;
                    this.NotifyPropertyChanged("ShirtConversion");
                }
            }
        }

        private ConversionBtnData _dressConversion;
        public ConversionBtnData DressConversion
        {
            get
            {
                if (this._dressConversion == null) this._dressConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.DressSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.DressSize,
                    RequiredMeasurementsMale = RequiredMeasurements.DressSize,
                };
                return this._dressConversion;
            }
            set
            {
                if (this._dressConversion != value)
                {
                    this._dressConversion = value;
                    this.NotifyPropertyChanged("DressConversion");
                }
            }
        }


        private ConversionBtnData _hatConversion;
        public ConversionBtnData HatConversion
        {
            get
            {
                if (this._hatConversion == null) this._hatConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.HatSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.Hat,
                    RequiredMeasurementsMale = RequiredMeasurements.Hat,
                };
                return this._hatConversion;
            }
            set
            {
                if (this._hatConversion != value)
                {
                    this._hatConversion = value;
                    this.NotifyPropertyChanged("HatConversion");
                }
            }
        }

        private ConversionBtnData _suitConversion;
        public ConversionBtnData SuitConversion
        {
            get
            {
                if (this._suitConversion == null) this._suitConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.SuitSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.SuitWomens,
                    RequiredMeasurementsMale = RequiredMeasurements.SuitMens,
                };
                return this._suitConversion;
            }
            set
            {
                if (this._suitConversion != value)
                {
                    this._suitConversion = value;
                    this.NotifyPropertyChanged("SuitConversion");
                }
            }
        }

        private ConversionBtnData _shoeConversion;
        public ConversionBtnData ShoeConversion
        {
            get
            {
                if (this._shoeConversion == null) this._shoeConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.ShoeSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.Shoes,
                    RequiredMeasurementsMale = RequiredMeasurements.Shoes,
                };
                return this._shoeConversion;
            }
            set
            {
                if (this._shoeConversion != value)
                {
                    this._shoeConversion = value;
                    this.NotifyPropertyChanged("ShoeConversion");
                }
            }
        }

        private ConversionBtnData _braConversion;
        public ConversionBtnData BraConversion
        {
            get
            {
                if (this._braConversion == null) this._braConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.BraSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.Bra,
                    RequiredMeasurementsMale = RequiredMeasurements.Bra,
                };
                return this._braConversion;
            }
            set
            {
                if (this._braConversion != value)
                {
                    this._braConversion = value;
                    this.NotifyPropertyChanged("BraConversion");
                }
            }
        }

        private ConversionBtnData _hosieryConversion;
        public ConversionBtnData HosieryConversion
        {
            get
            {
                if (this._hosieryConversion == null) this._hosieryConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.HosierySize,
                    RequiredMeasurementsFemale = RequiredMeasurements.Hosiery,
                    RequiredMeasurementsMale = RequiredMeasurements.Hosiery,
                };
                return this._hosieryConversion;
            }
            set
            {
                if (this._hosieryConversion != value)
                {
                    this._hosieryConversion = value;
                    this.NotifyPropertyChanged("HosieryConversion");
                }
            }
        }

        private ConversionBtnData _skiBootConversion;
        public ConversionBtnData SkiBootConversion
        {
            get
            {
                if (this._skiBootConversion == null) this._skiBootConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.SkiBootSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.SkiBoots,
                    RequiredMeasurementsMale = RequiredMeasurements.SkiBoots,
                };
                return this._skiBootConversion;
            }
            set
            {
                if (this._skiBootConversion != value)
                {
                    this._skiBootConversion = value;
                    this.NotifyPropertyChanged("SkiBootConversion");
                }
            }
        }

        private ConversionBtnData _wetsuitConversion;
        public ConversionBtnData WetsuitConversion
        {
            get
            {
                if (this._wetsuitConversion == null) this._wetsuitConversion = new ConversionBtnData()
                {
                    ConversionId = Model.ConversionId.WetsuitSize,
                    RequiredMeasurementsFemale = RequiredMeasurements.WetsuitWomens,
                    RequiredMeasurementsMale = RequiredMeasurements.WetsuitMens,
                };
                return this._wetsuitConversion;
            }
            set
            {
                if (this._wetsuitConversion != value)
                {
                    this._wetsuitConversion = value;
                    this.NotifyPropertyChanged("WetsuitConversion");
                }
            }
        }

        
        #endregion


        #region Methods to manage the nomination of a conversion for unlocking

        // A proeprty that is not null when measurements should be highlighted
        // Contains the ConversionId that defines which measurements need to be highlighted 
        private ConversionId? _currentNominatedConversion;
        public ConversionId? CurrentNominatedConversion 
        {
            get
            {
                return this._currentNominatedConversion;
            }
            set
            {
                if (this._currentNominatedConversion != value)
                {
                    this._currentNominatedConversion = value;
                    this.NotifyPropertyChanged("CurrentNominatedConversion");
                    this.NotifyPropertyChanged("CurrentNominatedConversionName");
                    //this.NotifyPropertyChanged("ShowMeasurementPageHelpContainer");
                }
            }
        }

        // A temporary store for CurrentNominatedConversion which activates animations through binding when changed
        // so that the animations can occur at the right time
        public ConversionId? PendingCurrentNominatedConversion { get; set; }

        public string CurrentNominatedConversionName
        {
            get
            {
                if (this.CurrentNominatedConversion == null)
                    return String.Empty;
                else
                    return Lookup.Conversions[(ConversionId)this.CurrentNominatedConversion].ToLower();
            }
        }

        public void NominateConversion(ConversionId cId) 
        {
            List<MeasurementId> missingMeasurementIds = this.Conversions[cId].MissingMeasurements;
            foreach (Measurement m in this.FullMeasurements)
            {
                m.IsNeeded = missingMeasurementIds.Contains(m.MeasurementId);
            }
            this.PendingCurrentNominatedConversion = cId;
        }

        public void UnNominateConversion()
        {
            foreach (Measurement m in this.FullMeasurements)
            {
                m.IsNeeded = false;
            }
            this.CurrentNominatedConversion = null;
        }

        #endregion


        #region ViewingUnitCulture properties/methods

        // The unit type (metric/imperial) that is being viewed at the moment
        private UnitCultureId? _viewingUnitCulture;
        public UnitCultureId? ViewingUnitCulture
        {
            get 
            { 
                return this._viewingUnitCulture; 
            }
            set
            {
                if (this._viewingUnitCulture != value)
                {
                    this._viewingUnitCulture = value;
                    this.NotifyPropertyChanged("ViewingUnitCulture");
                }
            }
        }

        #endregion


        #region FullMeasurements and helper methods

        public void LoadFullMeasurements()
        {
            IEnumerable<MeasurementId> fullListIds = (this.SelectedProfile.Gender == GenderId.Male) ?
                Model.MeasurmentIdsByGender.Male : Model.MeasurmentIdsByGender.Female;
            IEnumerable<MeasurementId> missingMeasurementIds = fullListIds.Except<MeasurementId>(this.SelectedProfile.Measurements.Select(m => m.MeasurementId));
            // Need to operate on a buffer to overcome limitations of ObservableCollection (no AddRange method)
            List<Measurement> buff = new List<Measurement>(); 
            buff.AddRange(this.SelectedProfile.Measurements.ToList());
            foreach (MeasurementId mId in missingMeasurementIds)
            {
                MeasurementTemplate mt = Model.Static.MeasurementTemplates.Where(m => m.Id == mId).First();
                buff.Add(new Measurement()
                {
                    MeasurementId = mId,
                    Name = mt.Name,
                    Value = null,
                    MeasurementType = mt.MeasurementType,
                });
            }
            buff.Sort((x, y) => x.Name.CompareTo(y.Name));
            this.FullMeasurements = new ObservableCollection<Measurement>(buff);
        }

        // Swap in the new Measurement object which is tied to a profile (as a result of editing) instead
        // of the placeholder one used to pad the UI
        public void SwapInToFullMeasurements(Measurement newMeasurement)
        {
            List<Measurement> buff = App.VM.FullMeasurements.ToList();
            buff.Remove(buff.Where(m => m.MeasurementId == newMeasurement.MeasurementId).First());
            buff.Add(newMeasurement);
            buff.Sort((x, y) => x.Name.CompareTo(y.Name));
            App.VM.FullMeasurements = new System.Collections.ObjectModel.ObservableCollection<Measurement>(buff);
        }

        private ObservableCollection<Measurement> _fullMeasurements;
        public ObservableCollection<Measurement> FullMeasurements
        {
            get { return this._fullMeasurements; }
            set
            {
                if (this._fullMeasurements != value)
                {
                    this._fullMeasurements = value;
                    this.NotifyPropertyChanged("FullMeasurements");
                }
            }
        }

        #endregion

        
    }



}
