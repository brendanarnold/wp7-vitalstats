using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{

    public class HatCsvReader : ICsvReader
    {
        public HatCsvReader()
        {
            this.ConversionId = ConversionId.HatSize;
        }
        public ConversionId ConversionId { get; set; }
        public AppDataContext Db { get; set; }
        public void QueueWriteObj(Pocketailor.Model.SetupDatabase.CsvLine csvLine)
        {
            this.Db.Hats.InsertOnSubmit(new Hat()
            {
                Retailer = csvLine.Retailer,
                Region =  csvLine.Region,
                Head = csvLine.GetMeasurementOrNull(MeasurementId.Head),
                SizeLetter = csvLine.SizeLetter,
                SizeNumber = csvLine.SizeNumber,
            });
        }
    }


    public static class HatUtils
    {
        public static List<MeasurementId> RequiredMeasurements = new List<MeasurementId>()
        {
            MeasurementId.Head,
        };

    }

    [Table]
    public class Hat : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public double? Head { get; set; }

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
                return Gender.Unspecified;
            }
        }

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
            double dHead = (this.Head.HasValue) ? (double)this.Head - enumerator.Current : 0.0;

            double chiSq = dHead * dHead;
            // Extra bit because womens sizes also consider the hips
            return chiSq;
        }

        #endregion

    }
}
