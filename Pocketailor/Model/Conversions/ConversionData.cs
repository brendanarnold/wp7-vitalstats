﻿using Newtonsoft.Json;
using Pocketailor.Model.Adjustments;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    // Represents the all the data for a particular region, gender, brand and conversion e.g.
    // UK, Female, M&S, Dress sizes.

    [Table]
    public class ConversionData
    {
        public ConversionData()
        {
            
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


        public Dictionary<MeasurementId, List<double?>> Measurements {
            get
            {
                if (this.Data.Measurements == null) this.Data.Measurements = new Dictionary<MeasurementId, List<double?>>();
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
        public RegionIds Region { get; set; }

        [Column]
        public RetailId Retailer { get; set; }

        [Column]
        public Gender Gender { get; set; }

        [Column]
        public ConversionId Conversion { get; set; }
        
        // Flag that is set when all the sizes are found to be too small
        public bool AllTooSmall { get; set; }

        // Index of the sizes that best fits
        public int BestFitInd { get; set; }

        // Returns the formatted strin of the best fit
        public string FormattedValue
        {
            get
            {
                if (this.AllTooSmall) return "N/A";
                if (this.GeneralSizes[this.BestFitInd] == String.Empty) return String.Format("{0}", this.RegionalSizes[this.BestFitInd]);
                if (this.RegionalSizes[this.BestFitInd] == String.Empty) return String.Format("({0})", this.RegionalSizes[this.BestFitInd]);
                return String.Format("{0} ({1})", this.RegionalSizes[this.BestFitInd], this.GeneralSizes[this.BestFitInd]);
            }
        }


        public void FindBestFit(Dictionary<MeasurementId, double> measuredVals)
        {
            // If there is an adjustment, apply that
            double scaleFactor = 1.0;
            Adjustment adj = App.Cache.Adjustments.FirstOrDefault(a =>
                   a.c == this.Conversion
                && a.r == this.Region
                && a.g == this.Gender
                && a.b == this.Retailer
            );
            if (adj != null) scaleFactor = adj.v;

            // Find index of the closest fit which all values are  if any size is too small then go for the next size up
            int? bestInd = null;
            double bestChiSq = Double.MaxValue;
            int nSizes = this.Measurements.Values.First().Count;
            for (int i = 0; i < nSizes; i++)
            {
                List<double> vals = new List<double>();
                foreach (MeasurementId mID in measuredVals.Keys)
                {
                    vals.Add((this.Measurements[mID][i].HasValue) ? (double)this.Measurements[mID][i] - measuredVals[mID] : 0.0);
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
            }
            else
            {
                this.AllTooSmall = false;
                this.BestFitInd = (int)BestFitInd;
            }
        }


    }


    public class JsonifiableDataContainer
    {
        public Dictionary<MeasurementId, List<double?>> Measurements { get; set; }
        public List<string> RegionalSizes { get; set; }
        public List<string> GeneralSizes { get; set; }
    }


}