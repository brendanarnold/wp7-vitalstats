using Pocketailor.Model;
using System;
using System.Collections.Generic;
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
            this.LoadMeasurements(this.ViewingUnitCulture);
        }

        // Helkper method to notify the View of possible updates to HasMeasurement properties
        internal void RefreshRequiredMeasurement()
        {
            foreach (ConversionDataContainer c in this.Conversions.Values)
                c.NotifyPropertyChanged("HasRequiredMeasurements");
        }

        
        #region Bindable objects for the conversion button data


        // Convenience data structure to access the conversion data containers
        private Dictionary<ConversionId, ConversionDataContainer> _conversions;
        public Dictionary<ConversionId, ConversionDataContainer> Conversions
        {
            get
            {
                if (this._conversions == null)
                    this._conversions = new Dictionary<ConversionId, ConversionDataContainer>()
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


        private ConversionDataContainer _trouserConversion;
        public ConversionDataContainer TrouserConversion
        {
            get
            {
                if (this._trouserConversion == null) this._trouserConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _shirtConversion;
        public ConversionDataContainer ShirtConversion
        {
            get
            {
                if (this._shirtConversion == null) this._shirtConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _dressConversion;
        public ConversionDataContainer DressConversion
        {
            get
            {
                if (this._dressConversion == null) this._dressConversion = new ConversionDataContainer()
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


        private ConversionDataContainer _hatConversion;
        public ConversionDataContainer HatConversion
        {
            get
            {
                if (this._hatConversion == null) this._hatConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _suitConversion;
        public ConversionDataContainer SuitConversion
        {
            get
            {
                if (this._suitConversion == null) this._suitConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _shoeConversion;
        public ConversionDataContainer ShoeConversion
        {
            get
            {
                if (this._shoeConversion == null) this._shoeConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _braConversion;
        public ConversionDataContainer BraConversion
        {
            get
            {
                if (this._braConversion == null) this._braConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _hosieryConversion;
        public ConversionDataContainer HosieryConversion
        {
            get
            {
                if (this._hosieryConversion == null) this._hosieryConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _skiBootConversion;
        public ConversionDataContainer SkiBootConversion
        {
            get
            {
                if (this._skiBootConversion == null) this._skiBootConversion = new ConversionDataContainer()
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

        private ConversionDataContainer _wetsuitConversion;
        public ConversionDataContainer WetsuitConversion
        {
            get
            {
                if (this._wetsuitConversion == null) this._wetsuitConversion = new ConversionDataContainer()
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

        #region Bindable properties with values for the measurements plus loading functions

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

        public void NominateRequiredMeasurements(List<MeasurementId> candidateIds)
        {
            foreach (MeasurementId m in this.Measurements.Keys)
            {
                this.Measurements[m].IsCandidate = candidateIds.Contains(m);
            }
        }

        public void LoadMeasurements(UnitCultureId unitCultureId)
        {
            List<MeasurementId> keys = new List<MeasurementId>(this.Measurements.Keys);
            foreach (MeasurementId mId in keys)
            {
                string val = String.Empty;
                if (this.SelectedProfile.Measurements.Select(m => m.MeasurementId).Contains(mId))
                {
                    val = this.SelectedProfile.Measurements.Where(m => m.MeasurementId == mId)
                        .First().GetFormattedValueOfType(this.ViewingUnitCulture);
                }
                this.Measurements[mId].Value = val;
            }
        }


        private MeasurementDataContainer _measurementChest;
        public MeasurementDataContainer MeasurementChest {
            get 
            {
                if (this._measurementChest == null) this._measurementChest = new MeasurementDataContainer() { MeasurementId = MeasurementId.Chest, };
                return this._measurementChest;
            }
            set
            {
                if (this._measurementChest != value)
                {
                    this._measurementChest = value;
                    this.NotifyPropertyChanged("MeasurementChest");
                }
            }
        }

        private MeasurementDataContainer _measurementFootLength;
        public MeasurementDataContainer MeasurementFootLength
        {
            get
            {
                if (this._measurementFootLength == null) this._measurementFootLength = new MeasurementDataContainer() { MeasurementId = MeasurementId.FootLength, };
                return this._measurementFootLength;
            }
            set
            {
                if (this._measurementFootLength != value)
                {
                    this._measurementFootLength = value;
                    this.NotifyPropertyChanged("MeasurementFootLength");
                }
            }
        }

        private MeasurementDataContainer _measurementFootWidth;
        public MeasurementDataContainer MeasurementFootWidth
        {
            get
            {
                if (this._measurementFootWidth == null) this._measurementFootWidth = new MeasurementDataContainer() { MeasurementId = MeasurementId.FootWidth, };
                return this._measurementFootWidth;
            }
            set
            {
                if (this._measurementFootWidth != value)
                {
                    this._measurementFootWidth = value;
                    this.NotifyPropertyChanged("MeasurementFootWidth");
                }
            }
        }

        private MeasurementDataContainer _measurementHead;
        public MeasurementDataContainer MeasurementHead
        {
            get
            {
                if (this._measurementHead == null) this._measurementHead = new MeasurementDataContainer() { MeasurementId = MeasurementId.Head, };
                return this._measurementHead;
            }
            set
            {
                if (this._measurementHead != value)
                {
                    this._measurementHead = value;
                    this.NotifyPropertyChanged("MeasurementHead");
                }
            }
        }

        private MeasurementDataContainer _measurementHips;
        public MeasurementDataContainer MeasurementHips
        {
            get
            {
                if (this._measurementHips == null) this._measurementHips = new MeasurementDataContainer() { MeasurementId = MeasurementId.Hips, };
                return this._measurementHips;
            }
            set
            {
                if (this._measurementHips != value)
                {
                    this._measurementHips = value;
                    this.NotifyPropertyChanged("MeasurementHips");
                }
            }
        }


        private MeasurementDataContainer _measurementHeight;
        public MeasurementDataContainer MeasurementHeight
        {
            get
            {
                if (this._measurementHeight == null) this._measurementHeight = new MeasurementDataContainer() { MeasurementId = MeasurementId.Height, };
                return this._measurementHeight;
            }
            set
            {
                if (this._measurementHeight != value)
                {
                    this._measurementHeight = value;
                    this.NotifyPropertyChanged("MeasurementHeight");
                }
            }
        }

        private MeasurementDataContainer _measurementInsideLeg;
        public MeasurementDataContainer MeasurementInsideLeg
        {
            get
            {
                if (this._measurementInsideLeg == null) this._measurementInsideLeg = new MeasurementDataContainer() { MeasurementId = MeasurementId.InsideLeg, };
                return this._measurementInsideLeg;
            }
            set
            {
                if (this._measurementInsideLeg != value)
                {
                    this._measurementInsideLeg = value;
                    this.NotifyPropertyChanged("MeasurementInsideLeg");
                }
            }
        }

        private MeasurementDataContainer _measurementNeck;
        public MeasurementDataContainer MeasurementNeck
        {
            get
            {
                if (this._measurementNeck == null) this._measurementNeck = new MeasurementDataContainer() { MeasurementId = MeasurementId.Neck, };
                return this._measurementNeck;
            }
            set
            {
                if (this._measurementNeck != value)
                {
                    this._measurementNeck = value;
                    this.NotifyPropertyChanged("MeasurementNeck");
                }
            }
        }

        private MeasurementDataContainer _measurementShoulder;
        public MeasurementDataContainer MeasurementShoulder
        {
            get
            {
                if (this._measurementShoulder == null) this._measurementShoulder = new MeasurementDataContainer() { MeasurementId = MeasurementId.Shoulder, };
                return this._measurementShoulder;
            }
            set
            {
                if (this._measurementShoulder != value)
                {
                    this._measurementShoulder = value;
                    this.NotifyPropertyChanged("MeasurementShoulder");
                }
            }

        }

        private MeasurementDataContainer _measurementSleeve;
        public MeasurementDataContainer MeasurementSleeve
        {
            get
            {
                if (this._measurementSleeve == null) this._measurementSleeve = new MeasurementDataContainer() { MeasurementId = MeasurementId.Sleeve, };
                return this._measurementSleeve;
            }
            set
            {
                if (this._measurementSleeve != value)
                {
                    this._measurementSleeve = value;
                    this.NotifyPropertyChanged("MeasurementSleeve");
                }
            }
        }

        private MeasurementDataContainer _measurementTorsoLength;
        public MeasurementDataContainer MeasurementTorsoLength
        {
            get
            {
                if (this._measurementTorsoLength == null) this._measurementTorsoLength = new MeasurementDataContainer() { MeasurementId = MeasurementId.TorsoLength, };
                return this._measurementTorsoLength;
            }
            set
            {
                if (this._measurementTorsoLength != value)
                {
                    this._measurementTorsoLength = value;
                    this.NotifyPropertyChanged("MeasurementTorsoLength");
                }
            }
        }

        private MeasurementDataContainer _measurementUnderBust;
        public MeasurementDataContainer MeasurementUnderBust
        {
            get
            {
                if (this._measurementUnderBust == null) this._measurementUnderBust = new MeasurementDataContainer() { MeasurementId = MeasurementId.UnderBust, };
                return this._measurementUnderBust;
            }
            set
            {
                if (this._measurementUnderBust != value)
                {
                    this._measurementUnderBust = value;
                    this.NotifyPropertyChanged("MeasurementUnderBust");
                }
            }
        }

        private MeasurementDataContainer _measurementWaist;
        public MeasurementDataContainer MeasurementWaist
        {
            get
            {
                if (this._measurementWaist == null) this._measurementWaist = new MeasurementDataContainer() { MeasurementId = MeasurementId.Waist, };
                return this._measurementWaist;
            }
            set
            {
                if (this._measurementWaist != value)
                {
                    this._measurementWaist = value;
                    this.NotifyPropertyChanged("MeasurementWaist");
                }
            }
        }

        private MeasurementDataContainer _measurementWeight;
        public MeasurementDataContainer MeasurementWeight
        {
            get
            {
                if (this._measurementWeight == null) this._measurementWeight = new MeasurementDataContainer() { MeasurementId = MeasurementId.Weight, };
                return this._measurementWeight;
            }
            set
            {
                if (this._measurementWeight != value)
                {
                    this._measurementWeight = value;
                    this.NotifyPropertyChanged("MeasurementWeight");
                }
            }
        }


        // Convenience data structure to access the measurement variables
        private Dictionary<MeasurementId, MeasurementDataContainer> _measurements;
        private Dictionary<MeasurementId, MeasurementDataContainer> Measurements {

            get
            {
                if (this._measurements == null) this._measurements = 
                    new Dictionary<MeasurementId, MeasurementDataContainer>()
                    {
                        { MeasurementId.Chest, this.MeasurementChest},
                        { MeasurementId.FootLength, this.MeasurementFootLength},
                        { MeasurementId.FootWidth, this.MeasurementFootWidth},
                        { MeasurementId.Head, this.MeasurementHead},
                        { MeasurementId.Height, this.MeasurementHeight},
                        { MeasurementId.Hips, this.MeasurementHips},
                        { MeasurementId.InsideLeg, this.MeasurementInsideLeg},
                        { MeasurementId.Neck, this.MeasurementNeck},
                        { MeasurementId.Sleeve, this.MeasurementSleeve},
                        { MeasurementId.Shoulder, this.MeasurementShoulder},
                        { MeasurementId.TorsoLength, this.MeasurementTorsoLength},
                        { MeasurementId.UnderBust, this.MeasurementUnderBust},
                        { MeasurementId.Waist, this.MeasurementWaist},
                        { MeasurementId.Weight, this.MeasurementWeight},
                    };
                return this._measurements;
            }
        }

        #endregion

    }


    // A container class for the conversion data
    public class ConversionDataContainer : INotifyPropertyChanged
    {
        private bool _hasMeasurements;
        public bool HasMeasurements
        {
            get { return this._hasMeasurements; }
            set
            {
                if (this._hasMeasurements != value)
                {
                    this._hasMeasurements = value;
                    this.NotifyPropertyChanged("HasMeasurements");
                }
            }
        }


        private List<MeasurementId> _requiredMeasurementsMale;
        public List<MeasurementId> RequiredMeasurementsMale
        {
            get { return this._requiredMeasurementsMale; }
            set { 
                if (this._requiredMeasurementsMale != value) 
                { 
                    this._requiredMeasurementsMale = value;
                    this.NotifyPropertyChanged("RequiredMeasurementsMale");
                } 
            }
        }

        private List<MeasurementId> _requiredMeasurementsFemale;
        public List<MeasurementId> RequiredMeasurementsFemale
        {
            get { return this._requiredMeasurementsFemale; }
            set
            {
                if (this._requiredMeasurementsFemale != value)
                {
                    this._requiredMeasurementsFemale = value;
                    this.NotifyPropertyChanged("RequiredMeasurementsFemale");
                }
            }
        }

        public bool HasRequiredMeasurements 
        {
            get
            {
                List<MeasurementId> requiredIds;
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    requiredIds = this.RequiredMeasurementsMale;
                } else {
                    requiredIds = this.RequiredMeasurementsFemale;
                }
                IEnumerable<MeasurementId> measurementIds = from Measurement s in App.VM.SelectedProfile.Measurements select s.MeasurementId;
                foreach (MeasurementId id in requiredIds)
                    if (!measurementIds.Contains(id)) return false;
                return true;
            }
        }

        public List<MeasurementId> MissingMeasurements
        {
            get
            {
                List<MeasurementId> requiredIds;
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    requiredIds = this.RequiredMeasurementsMale;
                }
                else
                {
                    requiredIds = this.RequiredMeasurementsFemale;
                }
                IEnumerable<MeasurementId> alreadyTakenMeasurementIds = from Measurement s in App.VM.SelectedProfile.Measurements select s.MeasurementId;
                return requiredIds.Except(alreadyTakenMeasurementIds).ToList();

            }
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


    // A container class for measurement data
    public class MeasurementDataContainer : INotifyPropertyChanged
    {
        private string _value;
        public string Value
        {
            get { return this._value; }
            set
            {
                if (this._value != value)
                {
                    this._value = value;
                    this.NotifyPropertyChanged("Value");
                    this.NotifyPropertyChanged("HasValue");
                }
            }
        }

        public bool HasValue
        {
            get { return this._value != null; }
        }

        private bool _isCandidate = false;
        public bool IsCandidate
        {
            get { return this._isCandidate; }
            set
            {
                if (this._isCandidate != value)
                {
                    this._isCandidate = value;
                    this.NotifyPropertyChanged("IsCandidate");
                }
            }
        }

        private MeasurementId _measurementId;
        public MeasurementId MeasurementId
        {
            get { return this._measurementId; }
            set
            {
                if (this._measurementId != value)
                {
                    this._measurementId = value;
                    this.NotifyPropertyChanged("MeasurementId");
                }
            }
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


}
