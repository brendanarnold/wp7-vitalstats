using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Pocketailor.Model.Conversions
{
    [Table]
    public class Wetsuit
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

        [Column]
        public RegionTag Region { get; set; }

        [Column]
        public RetailId Retailer { get; set; }

        public string FormattedValue
        {
            get
            {
                if (this.SizeLetter == String.Empty) return String.Format("{0}", this.SizeNumber);
                if (this.SizeNumber == String.Empty) return String.Format("({0})", this.SizeLetter);
                return String.Format("{0} ({1})", this.SizeNumber, this.SizeLetter);
            }
        }


    }
}
