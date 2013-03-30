using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    public static class TennisRaquetSizesUtils
    {
        public static List<MeasurementId> RequiredMeasurements = new List<MeasurementId>()
        {
            MeasurementId.TennisGrip,
        };

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.TennisRaquetSizes.DeleteAllOnSubmit(db.TennisRaquetSizes);
            db.SubmitChanges();
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\TennisRaquetSize.txt", UriKind.Relative));
            System.IO.StreamReader fh = new System.IO.StreamReader(res.Stream);

            int count = 0;
            while (!fh.EndOfStream)
            {
                count++;
                string line = fh.ReadLine();
                // Skip headers
                if (count <= AppConstants.CSV_HEADER_LINES) continue;
                // Skip commented lines
                if (line.StartsWith("#")) continue;
                var els = line.Split(AppConstants.CSV_DELIMITERS).Cast<string>().GetEnumerator();
                els.MoveNext();
                RetailId retailer = (RetailId)Enum.Parse(typeof(RetailId), els.Current, true);
                els.MoveNext();
                RegionTag region = (RegionTag)Enum.Parse(typeof(RegionTag), els.Current, true);
                // Store in DB as metres (input file is is centimetres inline with most charts in shops)
                double? tennisGrip = null;
                els.MoveNext();
                if (els.Current != String.Empty) tennisGrip = 0.01 * double.Parse(els.Current);
                els.MoveNext();
                string sizeLetter = els.Current;
                els.MoveNext();
                string sizeNumber = els.Current.TrimEnd();
                db.TennisRaquetSizes.InsertOnSubmit(new TennisRaquetSize()
                {
                    Retailer = retailer,
                    Region = region,
                    TennisGrip = tennisGrip,
                    SizeLetter = sizeLetter,
                    SizeNumber = sizeNumber,
                });
                if (count > AppConstants.DB_OBJECT_BUFFER_BEFORE_WRITE)
                {
                    count = 0;
                    db.SubmitChanges();
                }
            }
            db.SubmitChanges();
        }


    }

    [Table]
    public class TennisRaquetSize : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public double? TennisGrip { get; set; }

        [Column]
        public string SizeLetter { get; set; }

        [Column]
        public string SizeNumber { get; set; }


        #region IConversionData methods/properties

        [Column]
        public RegionTag Region { get; set; }

        [Column]
        public RetailId Retailer { get; set; }

        public Gender Gender {
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
            // By convention, the order is determined by DressSizeUtils
            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.   
            var enumerator = measuredVals.GetEnumerator();
            enumerator.MoveNext();
            double dTennisGrip = (this.TennisGrip.HasValue) ? (double)this.TennisGrip - enumerator.Current : 0.0;

            double chiSq = dTennisGrip * dTennisGrip;

            return chiSq;
        }

        #endregion

    }
}
