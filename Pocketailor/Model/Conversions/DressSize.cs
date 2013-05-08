using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using Pocketailor.Model.Adjustments;

namespace Pocketailor.Model.Conversions
{

    

    public static class DressSizeUtils
    {

    }


    [Table]
    public class DressSize : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        // New properties

        [Column]
        public List<double?> WaistSizes { get; set; }

        [Column]
        public List<double?> ChestSizes { get; set; }

        [Column]
        public List<double?> HipSizes { get; set; }

        [Column]
        public List<string> GeneralSizes { get; set; }

        [Column]
        public List<string> RegionalSizes { get; set; }

        public int BestFitInd { get; set; }

        public bool AllTooSmall { get; set; }

        #region IConversionData methods/properties

        [Column]
        public RegionIds Region { get; set; }

        [Column]
        public RetailId Retailer { get; set; }

        public Gender Gender
        {
            get
            {
                return Gender.Female;
            }
        }

        public string FormattedValue {
            get
            {
                if (this.AllTooSmall) return "N/A";
                if (this.GeneralSizes[this.BestFitInd] == String.Empty) return String.Format("{0}", this.RegionalSizes[this.BestFitInd]);
                if (this.RegionalSizes[this.BestFitInd] == String.Empty) return String.Format("({0})", this.GeneralSizes[this.BestFitInd]);
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
            for (int i = 0; i < this.ChestSizes.Count; i++)
            {
                double[] vals = new double[] {
                    // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.         
                    (this.ChestSizes[i].HasValue) ? (double)this.ChestSizes[i] * scaleFactor - measuredVals[MeasurementId.Chest] : 0.0,
                    (this.WaistSizes[i].HasValue) ? (double)this.WaistSizes[i] * scaleFactor - measuredVals[MeasurementId.Waist] : 0.0,
                    (this.HipSizes[i].HasValue)   ? (double)this.HipSizes[i] * scaleFactor   - measuredVals[MeasurementId.Hips] : 0.0,
                };
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
