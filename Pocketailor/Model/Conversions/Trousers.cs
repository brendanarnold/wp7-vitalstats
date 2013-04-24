using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{

    public class TrousersCsvReader : ICsvReader
    {
        public TrousersCsvReader()
        {
            this.ConversionId = ConversionId.TrouserSize;
        }
        public ConversionId ConversionId { get; set; }
        public AppDataContext Db { get; set; }
        public void QueueWriteObj(Pocketailor.Model.SetupDatabase.CsvLine csvLine)
        {
            this.Db.Trousers.InsertOnSubmit(new Trousers()
            {
                Retailer = csvLine.Retailer,
                Region = csvLine.Region,
                Gender = csvLine.Gender,
                Waist = csvLine.GetMeasurementOrNull(MeasurementId.Waist),
                Hips = csvLine.GetMeasurementOrNull(MeasurementId.Hips),
                InsideLeg = csvLine.GetMeasurementOrNull(MeasurementId.InsideLeg),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }


    public static class TrousersUtils
    {
        public static List<MeasurementId> RequiredMeasurementsMens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.InsideLeg,
        };

        public static List<MeasurementId> RequiredMeasurementsWomens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.InsideLeg,
            MeasurementId.Hips,
        };

    

    }

    [Table]
    public class Trousers : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public double? Waist { get; set; }

        [Column]
        public double? Hips { get; set; }

        [Column]
        public double? InsideLeg { get; set; }

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
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dInsideLeg = (this.InsideLeg.HasValue) ? (double)this.InsideLeg - enumerator.Current : 0.0;

            double chiSq = dWaist * dWaist + dInsideLeg * dInsideLeg;
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
