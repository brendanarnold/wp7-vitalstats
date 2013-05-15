using Newtonsoft.Json;
#if IS_MAIN_APP
using Pocketailor.Model.Adjustments;
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    // Represents the all the data for a particular region, gender, brand and conversion e.g.
    // UK, Female, M&S, Dress sizes.

    [Table]
    public class ConversionData : INotifyPropertyChanged
    {
        public ConversionData()
        {
            #if IS_MAIN_APP

            // Add a handler so that if user chooses to view/hide the blacklisted conversions
            // the visibility property correctly notifies the View
            if (this.IsBlacklisted)
            {
                App.VM.PropertyChanged += NotifyVisibilityChanged;
            }

            #endif            

        }

        void NotifyVisibilityChanged(object sender, PropertyChangedEventArgs e)
        {
                if (e.PropertyName == "ShowBlacklistedConversions")
                {
                    this.NotifyPropertyChanged("IsVisible");
                }
        }


        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        // A container for all the Jsonified data so that the (de)serialization is only done once
        [Column]
        private string JsonifiedData { get; set; }
        private JsonifiableDataContainer _data;
        private JsonifiableDataContainer Data
        {
            get 
            {
                if (this._data == null && this.JsonifiedData != null)
                {
                    this._data = JsonConvert.DeserializeObject<JsonifiableDataContainer>(this.JsonifiedData);

                }
                else if (this._data == null)
                {
                    this._data = new JsonifiableDataContainer();
                }
                return this._data;
            }
            set 
            {
                this._data = value;
            }
        }
        // Use this just before saving to DB
        public void JsonifyData()
        {
            this.JsonifiedData = JsonConvert.SerializeObject(this._data);
        }


        public Dictionary<MeasurementId, List<double>> Measurements {
            get
            {
                if (this.Data.Measurements == null) this.Data.Measurements = new Dictionary<MeasurementId, List<double>>();
                return this.Data.Measurements;
            }
            set
            {
                this.Data.Measurements = value;
            }
        }

        public List<string> GeneralSizes {
            get
            {
                if (this.Data.GeneralSizes == null) this.Data.GeneralSizes = new List<string>();
                return this.Data.GeneralSizes;
            }
            set 
            {
                this.Data.GeneralSizes = value;
            } 
        }

        public List<string> RegionalSizes {
            get
            {
                if (this.Data.RegionalSizes == null) this.Data.RegionalSizes = new List<string>();
                return this.Data.RegionalSizes;
            }
            set
            {
                this.Data.RegionalSizes = value;
            }
        }

        [Column]
        public RegionId Region { get; set; }

        [Column]
        public BrandId Brand { get; set; }

        [Column]
        public GenderId Gender { get; set; }

        [Column]
        public ConversionId Conversion { get; set; }
        
        // Flag that is set when all the sizes are found to be too small
        public bool AllTooSmall { get; set; }

        // Flag that indicates that there are no bigger sizes available
        public bool NoneBigger
        {
            get 
            {
                return (this.AllTooSmall || ((this.BestFitInd + this.Adjustment) >= this.GeneralSizes.Count - 1));
            }
        }

        // Flag that indicates if there are no smaller sizes availble
        public bool NoneSmaller
        {
            get { return ((this.BestFitInd + this.Adjustment) == 0); }
        }

        
        private int _adjustment;
        public int Adjustment
        {
            get
            {
                return this._adjustment;
            }
            set
            {
                if (this._adjustment != value)
                {
                    this._adjustment = value;
                    this.NotifyPropertyChanged("Adjustment");
                    this.NotifyPropertyChanged("NoneBigger");
                    this.NotifyPropertyChanged("NoneSmaller");
                    this.NotifyPropertyChanged("FormattedValue");
                }
            }
        }

        // Index of the sizes that best fits
        private int _bestFitInd;
        public int BestFitInd
        {
            get
            {
                return this._bestFitInd;
            }
            set
            {
                if (this._bestFitInd != value) this._bestFitInd = value;
                this.NotifyPropertyChanged("BestFitInd");
                this.NotifyPropertyChanged("NoneBigger");
                this.NotifyPropertyChanged("NoneSmaller");
                this.NotifyPropertyChanged("FormattedValue");
            }
        }

        public string BrandName
        {
            get
            {
                return Lookup.Brand[this.Brand];
            }
        }

        // Returns the formatted string of the best fit
        public string FormattedValue
        {
            get
            {
                if (this.AllTooSmall) return "N/A";
                int i = this.BestFitInd + this.Adjustment;
                if (this.GeneralSizes[i] == String.Empty) return String.Format("{0}", this.RegionalSizes[i]);
                if (this.RegionalSizes[i] == String.Empty) return String.Format("({0})", this.RegionalSizes[i]);
                return String.Format("{0} ({1})", this.RegionalSizes[i], this.GeneralSizes[i]);
            }
        }

        // This is necessary since database create app has no reference to App.VM
        #if IS_MAIN_APP


        public Adjustment PersistedAdjustment { get; set; }


        public void FindBestFit(Dictionary<MeasurementId, double> measuredVals)
        {
            // If there is an adjustment, apply that
            Adjustment adj = App.Cache.Adjustments.FirstOrDefault(a =>
                   a.c == this.Conversion
                && a.r == this.Region
                && a.g == this.Gender
                && a.b == this.Brand
            );
            this.PersistedAdjustment = adj;
            this.Adjustment = (this.PersistedAdjustment == null) ? 0 : this.PersistedAdjustment.a;

            // Find index of the closest fit which all values are  if any size is too small then go for the next size up
            int? bestInd = null;
            double bestChiSq = Double.MaxValue;
            int nSizes = this.Measurements.Values.First().Count;
            for (int i = 0; i < nSizes; i++)
            {
                List<double> vals = new List<double>();
                foreach (MeasurementId mID in measuredVals.Keys)
                {
                    vals.Add((this.Measurements[mID][i] != -1) ? (double)this.Measurements[mID][i] - measuredVals[mID] : 0.0);
                }
                if (vals.Any(x => x < 0)) continue;
                double chiSq = vals.Sum(x => x * x);
                if (chiSq < bestChiSq)
                {
                    bestInd = i;
                    bestChiSq = chiSq;
                }
            }
            // Take care of case where all sizes were too small
            if (bestInd == null)
            {
                this.AllTooSmall = true;
                this.BestFitInd = this.GeneralSizes.Count - 1;
            }
            else
            {
                this.AllTooSmall = false;
                this.BestFitInd = (int)BestFitInd;
            }
        }



        public void AcceptAdjustment()
        {
            if (this.PersistedAdjustment != null)
            {
                this.PersistedAdjustment.a = this.Adjustment;
            }
            else
            {
                Adjustment adj = new Adjustment()
                {
                    a = this.Adjustment,
                    b = this.Brand,
                    c = this.Conversion,
                    f = this.BestFitInd,
                    g = this.Gender,
                    i = App.VM.AppGUID,
                    r = this.Region,
                    t = Helpers.GetUnixTime(),
                    v = AppConstants.APP_VERSION,
                };
                App.Cache.Adjustments.Add(adj);
                this.PersistedAdjustment = adj;
            }
            // Send feedback if explicitly allowed
            if (App.VM.AllowFeedBack == true)
                App.FeedbackAgent.QueueFeedback(this.PersistedAdjustment);
            // Prune out adjustements that don't do anything
            if (this.PersistedAdjustment.a == 0)
            {
                App.Cache.Adjustments.Remove(this.PersistedAdjustment);
                this.PersistedAdjustment = null;
            }
            App.Cache.SaveAdjustments();
            }

        public void TweakAdjustment(int delta) 
        {
            this.Adjustment += delta;
        }

        internal void ResetAdjustment()
        {
            this.Adjustment = (this.PersistedAdjustment == null) ? 0 : this.PersistedAdjustment.a;
        }


        public bool IsBlacklisted
        {
            get 
            {
                return App.VM.BlacklistedBrands.Contains(this.Brand);
            }
            set
            {
                if (value)
                {
                    App.VM.BlacklistedBrands.Add(this.Brand);
                }
                else
                {
                    while (App.VM.BlacklistedBrands.Contains(this.Brand))
                    {
                        App.VM.BlacklistedBrands.Remove(this.Brand);
                    }
                }
                this.NotifyPropertyChanged("IsBlacklisted");
            }
        }

        #endif


        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

 
    }


    public class JsonifiableDataContainer
    {
        public Dictionary<MeasurementId, List<double>> Measurements { get; set; }
        public List<string> RegionalSizes { get; set; }
        public List<string> GeneralSizes { get; set; }

    }


}
