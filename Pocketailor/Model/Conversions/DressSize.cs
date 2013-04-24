using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;

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

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.DressSizes.DeleteAllOnSubmit(db.DressSizes);
            db.SubmitChanges();
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri(AppConstants.CSV_DATA_DIRECTORY + ConversionId.DressSize.ToString() + ".txt", UriKind.Relative));
            StreamReader fh = new StreamReader(res.Stream);

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
                // Store in DB as metres (input file is is centimetres inline with most charts in shops)
                double? chest = null;
                els.MoveNext();
                if (els.Current != String.Empty) chest = 0.01 * double.Parse(els.Current);
                double? waist = null;
                els.MoveNext();
                if (els.Current != String.Empty) waist = 0.01 * double.Parse(els.Current);
                double? hips = null;
                els.MoveNext();
                if (els.Current != String.Empty) hips = 0.01 * double.Parse(els.Current);
                els.MoveNext();
                string sizeLetter = els.Current;
                els.MoveNext();
                string sizeNumber = els.Current.TrimEnd();
                db.DressSizes.InsertOnSubmit(new DressSize()
                {
                    Retailer = retailer,
                    Region = region,
                    Chest = chest,
                    Waist = waist,
                    Hips = hips,
                    SizeLetter = sizeLetter,
                    SizeNumber = sizeNumber,
                });
                if (count > 50)
                {
                    count = 0;
                    db.SubmitChanges();
                }
            }
            db.SubmitChanges();
        }


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
