using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{

    public static class SuitUtils
    {
        public static List<MeasurementId> RequiredMeasurementsMens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.Shoulder,
            MeasurementId.Sleeve,
            MeasurementId.Chest,
            MeasurementId.InsideLeg,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            
        };

        public static List<MeasurementId> RequiredMeasurementsWomens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.Shoulder,
            MeasurementId.Sleeve,
            MeasurementId.Chest,
            MeasurementId.InsideLeg,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            MeasurementId.Hips,
        };

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.Suits.DeleteAllOnSubmit(db.Suits);
            db.SubmitChanges();
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri(AppConstants.CSV_DATA_DIRECTORY + ConversionId.SuitSize.ToString() + ".txt", UriKind.Relative));
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
                double? waist = null;
                els.MoveNext();
                if (els.Current != String.Empty) waist = 0.01 * double.Parse(els.Current);
                double? shoulder = null;
                els.MoveNext();
                if (els.Current != String.Empty) shoulder = 0.01 * double.Parse(els.Current);
                double? sleeve = null;
                els.MoveNext();
                if (els.Current != String.Empty) sleeve = 0.01 * double.Parse(els.Current);
                double? chest = null;
                els.MoveNext();
                if (els.Current != String.Empty) chest = 0.01 * double.Parse(els.Current);
                double? insideLeg = null;
                els.MoveNext();
                if (els.Current != String.Empty) insideLeg = 0.01 * double.Parse(els.Current);
                double? neck = null;
                els.MoveNext();
                if (els.Current != String.Empty) neck = 0.01 * double.Parse(els.Current);
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
                db.Suits.InsertOnSubmit(new Suit()
                {
                    Retailer = retailer,
                    Region = region,
                    Gender = gender,
                    Waist = waist,
                    Shoulder = shoulder,
                    Sleeve = sleeve,
                    Chest = chest,
                    InsideLeg = insideLeg,
                    Neck = neck,
                    TorsoLength = torsoLength,
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
