﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{

    public static class SuitUtils
    {
 
        

    }

    [Table]
    public class Suit : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public double? Waist { get; set; }

        [Column]
        public double? Shoulder { get; set; }

        [Column]
        public double? Sleeve { get; set; }

        [Column]
        public double? Chest { get; set; }

        [Column]
        public double? InsideLeg { get; set; }

        [Column]
        public double? Neck { get; set; }

        [Column]
        public double? TorsoLength { get; set; }

        [Column]
        public double? Hips { get; set; }

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
            // By convention, the order is determined by DressSizeUtils
            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.   
            var enumerator = measuredVals.GetEnumerator();
            enumerator.MoveNext();
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dShoulder = (this.Shoulder.HasValue) ? (double)this.Shoulder - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dSleeve = (this.Sleeve.HasValue) ? (double)this.Sleeve - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dChest = (this.Chest.HasValue) ? (double)this.Chest - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dInsideLeg = (this.InsideLeg.HasValue) ? (double)this.InsideLeg - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dNeck = (this.Neck.HasValue) ? (double)this.Neck - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dTorsoLength = (this.TorsoLength.HasValue) ? (double)this.TorsoLength - enumerator.Current : 0.0;

            double chiSq = dWaist * dWaist + dSleeve * dSleeve + dChest * dChest + dInsideLeg * dInsideLeg 
                + dNeck * dNeck + dTorsoLength * dTorsoLength;

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
