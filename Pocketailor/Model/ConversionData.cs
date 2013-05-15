﻿using Newtonsoft.Json;
#if !IS_DATABASE_HELPER_APP
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

        }

    
        #region Database properties and their accessors, necessary for DatabaseCreator

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

        public List<int> SizeIds
        {
            get
            {
                if (this.Data.SizeIds == null) this.Data.SizeIds = new List<int>();
                return this.Data.SizeIds;
            }
            set
            {
                this.Data.SizeIds = value;
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

        #endregion



        // This is necessary since database create app has no reference to App.VM
        #if !IS_DATABASE_HELPER_APP
        

        // Flag that indicates that there are no bigger sizes available
        public bool NoneBigger
        {
            get
            {
                return this.GetAdbsIndAdjustment() == AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_SMALL;
            }
        }

        // Flag that indicates if there are no smaller sizes availble
        public bool NoneSmaller
        {
            get 
            { 
                return this.GetAdbsIndAdjustment() == AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_BIG; 
            }
        }


        private int _relIndAdjustment;
        public int RelativeIndAdjustment
        {
            get
            {
                return this._relIndAdjustment;
            }
            set
            {
                if (this._relIndAdjustment != value)
                {
                    this._relIndAdjustment = value;
                    this.NotifyPropertyChanged("RelativeIndAdjustment");
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
                int i = this.GetAdbsIndAdjustment();
                if (i == AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_BIG) return "all too big";
                if (i == AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_SMALL) return "all too small";
                if (this.GeneralSizes[i] == String.Empty) return String.Format("{0}", this.RegionalSizes[i]);
                if (this.RegionalSizes[i] == String.Empty) return String.Format("({0})", this.RegionalSizes[i]);
                return String.Format("{0} ({1})", this.RegionalSizes[i], this.GeneralSizes[i]);
            }
        }


        public LocalAdjustment PersistedAdjustment { get; set; }


        public void FindBestFit(Dictionary<MeasurementId, double> measuredVals)
        {
            // Find index of the closest fit which all values are  if any size is too small then go for the next size up
            int? bestInd = null;
            double bestChiSq = Double.MaxValue;
            int nSizes = this.SizeIds.Count;
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
            this.BestFitInd = (bestInd == null) ? 
                AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_SMALL : (int)bestInd;
            // If there is an adjustment, also load this
            LocalAdjustment adj = App.Cache.Adjustments.FirstOrDefault(a =>
                   a.Conversion == this.Conversion
                && a.Profile == App.VM.SelectedProfile
                && a.Gender == this.Gender
                && a.Brand == this.Brand
            );
            this.PersistedAdjustment = adj;
            this.RelativeIndAdjustment = this.GetRelativeIndAdjustment();
        }


        #region Methods that ae used in the View e.g. ConversionResultsBtn, 

        // 'Tweaks' are temporary until accepted - after which they become and Adjustment

        public void AcceptTweaks()
        {
            if (this.PersistedAdjustment != null)
            {
                this.PersistedAdjustment.SizeId = this.SizeIds[this.GetAdbsIndAdjustment()];
            }
            else
            {
                LocalAdjustment adj = new LocalAdjustment()
                {
                    Profile = App.VM.SelectedProfile,
                    Brand = this.Brand,
                    Conversion = this.Conversion,
                    Gender = this.Gender,
                    SizeId = this.SizeIds[this.GetAdbsIndAdjustment()],
                };
                App.Cache.Adjustments.Add(adj);
                this.PersistedAdjustment = adj;
            }
            // Send feedback if explicitly allowed
            if (App.VM.AllowFeedBack == true)
            {
                FeedbackAdjustment feedbackAdj = new FeedbackAdjustment()
                {
                    b = this.Brand,
                    c = this.Conversion,
                    f = this.BestFitInd,
                    g = this.Gender,
                    i = App.VM.AppGUID,
                    s = this.SizeIds[this.GetAdbsIndAdjustment()],
                    t = Helpers.GetUnixTime(),
                    v = AppConstants.APP_VERSION,
                };
                App.FeedbackAgent.QueueFeedback(feedbackAdj);
            }
            // Prune out adjustements that don't do anything
            if (this.GetRelativeIndAdjustment() == 0)
            {
                App.Cache.Adjustments.Remove(this.PersistedAdjustment);
                this.PersistedAdjustment = null;
            }
            App.Cache.SaveAdjustments();
        }


        public void TweakSizeDown() 
        {
            if (this.GetAdbsIndAdjustment() - 1 < 0)
            {
                this.SetAbsIndAdjustment(AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_BIG);
            }
            else if (this.GetAdbsIndAdjustment() == AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_SMALL)
            {
                this.SetAbsIndAdjustment(this.SizeIds.Count - 1);
            }
            else
            {
                this.RelativeIndAdjustment -= 1;
            }
        }

        public void TweakSizeUp() 
        {
            if (this.GetAdbsIndAdjustment() + 1 >= this.SizeIds.Count)
            {
                this.SetAbsIndAdjustment(AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_SMALL);
            }
            else if (this.GetAdbsIndAdjustment() == AppConstants.ADJUSTMENT_SIZEID_ALL_TOO_BIG)
            {
                this.SetAbsIndAdjustment(0);
            }
            else
            {
                this.RelativeIndAdjustment += 1;
            }
        }
        

        // Whatever tweaks have been made, this resets back to the original value
        public void DiscardTweaks()
        {
            this.RelativeIndAdjustment = this.GetRelativeIndAdjustment();
        }


        #endregion

        // Obtain the relative adjustment based on what the persisted adjustment is
        public int GetRelativeIndAdjustment()
        {
            return (this.PersistedAdjustment == null) ? 0
               : this.SizeIds.IndexOf(this.PersistedAdjustment.SizeId) - this.BestFitInd;
        }

        // Resolves the bestfit and the relative adjustment to get the current SizeId index 
        public int GetAdbsIndAdjustment()
        {
            return this.RelativeIndAdjustment + this.BestFitInd;
        }


        // Sets the relative adjustment so that the absolute result corresponds to the index passed
        public void SetAbsIndAdjustment(int ind)
        {
            this.RelativeIndAdjustment = ind - this.BestFitInd;
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
        public List<int> SizeIds { get; set; }

    }


}