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

        // Helper method for tombstoning
        public void LoadMeasurementsPageData(int profileId)
        {
            if (this.SelectedProfile == null || this.SelectedProfile.Id != profileId)
            {
                this.SelectedProfile = (from Profile p in this.Profiles where p.Id == profileId select p).First();
            }
            this.ViewingUnitCulture = this.UnitCulture;
            this.LoadFullMeasurements();
        }

        // Helkper method to notify the View of possible updates to HasMeasurement properties
        internal void RefreshRequiredMeasurement()
        {
            foreach (ConversionBtnData c in this.Conversions.Values)
                c.NotifyPropertyChanged("HasRequiredMeasurements");
        }

        
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
                }
            }
        }

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
            this.CurrentNominatedConversion = cId;
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

        // The unit type (metric/imperial) that is being viewed at the moment
        private UnitCultureId? _viewingUnitCulture;
        public UnitCultureId ViewingUnitCulture
        {
            get 
            { 
                if (this._viewingUnitCulture == null) 
                    return UnitCultureId.Metric; 
                else 
                    return (UnitCultureId)this._viewingUnitCulture; 
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



        
    }



}
