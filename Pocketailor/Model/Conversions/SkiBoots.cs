using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    public static class SkiBootsUtils
    {
        public static List<MeasurementId> RequiredMeasurements = new List<MeasurementId>()
        {
            MeasurementId.FootLength,
            MeasurementId.FootWidth,
        };

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.SkiBoots.DeleteAllOnSubmit(db.SkiBoots);
            db.SubmitChanges();
            var res = System.Windows.Application.GetResourceStream(new Uri(AppConstants.CSV_DATA_DIRECTORY + ConversionId.SkiBootSize.ToString() +  ".txt", UriKind.Relative));
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
                RegionIds region = (RegionIds)Enum.Parse(typeof(RegionIds), els.Current, true);
                els.MoveNext();
                Gender gender = (Gender)Enum.Parse(typeof(Gender), els.Current, true);
                // Store in DB as metres (input file is is centimetres inline with most charts in shops)
                double? footLength = null;
                els.MoveNext();
                if (els.Current != String.Empty) footLength = 0.01 * double.Parse(els.Current);
                double? footWidth = null;
                els.MoveNext();
                if (els.Current != String.Empty) footWidth = 0.01 * double.Parse(els.Current);
                els.MoveNext();
                string sizeLetter = els.Current;
                els.MoveNext();
                string sizeNumber = els.Current.TrimEnd();
                db.SkiBoots.InsertOnSubmit(new SkiBoots()
                {
                    Retailer = retailer,
                    Region = region,
                    Gender = gender,
                    FootLength = footLength,
                    FootWidth = footWidth,
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
    public class SkiBoots : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public double? FootLength { get; set; }

        [Column]
        public double? FootWidth { get; set; }

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
            double dFootLength = (this.FootLength.HasValue) ? (double)this.FootLength - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dFootWidth = (this.FootWidth.HasValue) ? (double)this.FootWidth - enumerator.Current : 0.0;

            double chiSq = dFootLength * dFootLength + dFootWidth * dFootWidth;

            return chiSq;
        }

        #endregion

    }
}
