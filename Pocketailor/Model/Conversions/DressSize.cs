using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;

namespace Pocketailor.Model.Conversions
{

    public class DressCsvReader : ICsvReader 
    {
        public DressCsvReader()
        {
            this.ConversionId = ConversionId.DressSize;
        }
        public ConversionId ConversionId { get; set; }
        public AppDataContext Db { get;  set; }
        public void QueueWriteObj(Pocketailor.Model.SetupDatabase.CsvLine csvLine)
        {
            this.Db.DressSizes.InsertOnSubmit(new DressSize()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Chest = csvLine.GetMeasurementOrNull(MeasurementId.Chest),
                Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
                Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }

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
                if (this.SizeLetter == String.Empty) return String.Format("{0}", this.SizeNumber);
                if (this.SizeNumber == String.Empty) return String.Format("({0})", this.SizeLetter);
                return String.Format("{0} ({1})", this.SizeNumber, this.SizeLetter);
            }
        }

        public double GetChiSq(List<double> measuredVals)
        {
            // By convention, the order is determined by DressSizeUtils
            var enumerator = measuredVals.GetEnumerator();
            enumerator.MoveNext();
            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.         
            double dChest = (this.Chest.HasValue) ? (double)this.Chest - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dHips  = (this.Hips.HasValue)  ? (double)this.Hips  - enumerator.Current : 0.0;

            return dChest * dChest + dWaist * dWaist + dHips * dHips;
        }

        #endregion

    }
}
