using Pocketailor.Model.Adjustments;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
 


    public static class BraUtils
    {
        
    }

    [Table] 
    public class Bra : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public Dictionary<MeasurementId, List<double?>> Measurements { get; set; }

        [Column]
        public List<string> GeneralSizes { get; set; }

        [Column]
        public List<string> RegionalSizes { get; set; }

        public bool AllTooSmall { get; set; }

        public int BestFitInd { get; set; }

        #region IConversionData methods/properties

        [Column]
        public RegionIds Region { get; set; }

        [Column]
        public RetailId Retailer { get; set; }

        public Gender Gender {
            get
            {
                return Gender.Female;
            }
        }

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
                   a.c == ConversionId.DressSize
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
        #endregion

    }
}
