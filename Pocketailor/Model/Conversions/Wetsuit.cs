using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;

namespace Pocketailor.Model.Conversions
{


    public static class WetsuitUtils
    {

       



    }


    [Table]
    public class Wetsuit : IConversionData
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
        public double? Height { get; set; }

        [Column]
        public double? Weight { get; set; }

        [Column]
        public string SizeLetter { get; set; }

        [Column]
        public string SizeNumber { get; set; }

        
        #region IConversionData methods/properties

        [Column]
        public RegionIds Region { get; set; }

        [Column]
        public RetailId Retailer { get; set; }

        [Column]
        public Gender Gender { get; set; }

        public string FormattedValue
        {
            get
            {
                if (this.SizeLetter == String.Empty) return String.Format("{0}", this.SizeNumber);
                if (this.SizeNumber == String.Empty) return String.Format("({0})", this.SizeLetter);
                return String.Format("{0} ({1})", this.SizeNumber, this.SizeLetter);
            }
        }

        public double GetChiSq(List<double> measuredVals)
        {
            // By convention, the order is determined by WetsuitUtils
            var enumerator = measuredVals.GetEnumerator();
            enumerator.MoveNext();
            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.         
            double dHeight = (this.Height.HasValue) ? (double)this.Height - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dChest =  (this.Chest.HasValue)  ? (double)this.Chest  - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dWaist =  (this.Waist.HasValue)  ? (double)this.Waist  - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dWeight = (this.Weight.HasValue) ? (double)this.Weight - enumerator.Current : 0.0;

            double chiSq = dHeight * dHeight + dChest * dChest + dWaist * dWaist + dWeight * dWeight;
            // Extra bit because womens sizes also consider the hips
            if (enumerator.MoveNext())
            {
                double dHips = (this.Hips.HasValue) ? (double)this.Hips - enumerator.Current : 0.0;
                chiSq += dHips * dHips;
            }
            return chiSq;
        }

        #endregion

    }
}
