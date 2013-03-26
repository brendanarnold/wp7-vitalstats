using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Pocketailor.Model.Conversions
{

    public static class DressSizeUtils
    {
        public static List<MeasurementId> RequiredMeasurements = new List<MeasurementId>()
        {
            // This order is used in the GetChiSq method below
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Hips,
        };

    }


    [Table]
    public class DressSize : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public double? Waist { get; set; }

        [Column]
        public double? Chest { get; set; }

        [Column]
        public double? Hips { get; set; }

        [Column]
        public string SizeLetter { get; set; }

        [Column]
        public string SizeNumber { get; set; }


        #region IConversionData methods/properties

        [Column]
        public RegionTag Region { get; set; }

        [Column]
        public RetailId Retailer { get; set; }

        public string FormattedValue {
            get
            {
                if (this.SizeLetter == String.Empty) return String.Format("{0}", this.SizeNumber);
                if (this.SizeNumber == String.Empty) return String.Format("({0})", this.SizeLetter);
                return String.Format("{0} ({1})", this.SizeNumber, this.SizeLetter);
            }
        }

        public double GetChiSq(List<double> measuredVals)
        {
            // By convention, the order is determined by DressSizeUtils

            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.         
            double dChest = (this.Chest.HasValue) ? (double)this.Chest - measuredVals[0] : 0.0;
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - measuredVals[1] : 0.0;
            double dHips  = (this.Hips.HasValue)  ? (double)this.Hips  - measuredVals[2] : 0.0;

            return dChest * dChest + dWaist * dWaist + dHips * dHips;
        }

        #endregion

    }
}
