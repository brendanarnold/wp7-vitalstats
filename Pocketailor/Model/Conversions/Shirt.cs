using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    public static class ShirtUtils
    {
        public static List<MeasurementId> RequiredMeasurementsMens = new List<MeasurementId>()
        {
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            MeasurementId.Sleeve,
        };

        public static List<MeasurementId> RequiredMeasurementsWomens = new List<MeasurementId>()
        {
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            MeasurementId.Sleeve,
            MeasurementId.Hips,
        };

        // Retailer	Region	Gender	Chest	Neck	Sleeve	BodyLength	Hips	SizeLetter	SizeNumber
        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.Shirts.DeleteAllOnSubmit(db.Shirts);
            db.SubmitChanges();
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\Shirt.txt", UriKind.Relative));
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
                els.MoveNext();
                Gender gender = (Gender)Enum.Parse(typeof(Gender), els.Current, true);
                // Store in DB as metres (input file is is centimetres inline with most charts in shops)
                double? chest = null;
                els.MoveNext();
                if (els.Current != String.Empty) chest = 0.01 * double.Parse(els.Current);
                double? waist = null;
                els.MoveNext();
                if (els.Current != String.Empty) waist = 0.01 * double.Parse(els.Current);
                double? neck = null;
                els.MoveNext();
                if (els.Current != String.Empty) neck = 0.01 * double.Parse(els.Current);
                double? sleeve = null;
                els.MoveNext();
                if (els.Current != String.Empty) sleeve = 0.01 * double.Parse(els.Current);
                double? torsoLength = null;
                els.MoveNext();
                if (els.Current != String.Empty) torsoLength = 0.01 * double.Parse(els.Current);
                double? hips = null;
                els.MoveNext();
                if (els.Current != String.Empty) hips = 0.01 * double.Parse(els.Current);
                els.MoveNext();
                string sizeLetter = els.Current;
                els.MoveNext();
                string sizeNumber = els.Current.TrimEnd();
                db.Shirts.InsertOnSubmit(new Shirt()
                {
                    Retailer = retailer,
                    Region = region,
                    Gender = gender,
                    Chest = chest,
                    Waist = waist,
                    Neck = neck,
                    TorsoLength = torsoLength,
                    Sleeve = sleeve,
                    Hips = hips,
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
    public class Shirt : IConversionData
    {
        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public double? Chest { get; set; }

        [Column]
        public double? Waist { get; set; }

        [Column]
        public double? Neck { get; set; }

        [Column]
        public double? Sleeve { get; set; }

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
        public RegionTag Region { get; set; }

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
            double dChest = (this.Chest.HasValue) ? (double)this.Chest - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dNeck = (this.Neck.HasValue) ? (double)this.Neck - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dSleeve = (this.Sleeve.HasValue) ? (double)this.Sleeve - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dTorsoLength = (this.TorsoLength.HasValue) ? (double)this.TorsoLength - enumerator.Current : 0.0;

            double chiSq = dWaist * dWaist + dChest * dChest + dNeck * dNeck + dSleeve * dSleeve + dTorsoLength * dTorsoLength;
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
