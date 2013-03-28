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
            MeasurementId.Crotch,
            MeasurementId.TorsoLength,
            MeasurementId.Height,
            
        };

        public static List<MeasurementId> RequiredMeasurementsWomens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.Shoulder,
            MeasurementId.Sleeve,
            MeasurementId.Chest,
            MeasurementId.InsideLeg,
            MeasurementId.Neck,
            MeasurementId.Crotch,
            MeasurementId.TorsoLength,
            MeasurementId.Height,
            MeasurementId.Hips,
        };

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.Shirts.DeleteAllOnSubmit(db.Shirts);
            db.SubmitChanges();
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\Shirts.txt", UriKind.Relative));
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
                string[] els = line.Split(new char[] { '\t' });
                RetailId retailer = (RetailId)Enum.Parse(typeof(RetailId), els[0], true);
                RegionTag region = (RegionTag)Enum.Parse(typeof(RegionTag), els[1], true);
                Gender gender = (Gender)Enum.Parse(typeof(Gender), els[2], true);
                // Store in DB as metres (input file is is centimetres inline with most charts in shops)
                double? waist = null;
                if (els[3] != String.Empty) waist = 0.01 * double.Parse(els[3]);
                double? shoulder = null;
                if (els[4] != String.Empty) shoulder = 0.01 * double.Parse(els[4]);
                double? sleeve = null;
                if (els[5] != String.Empty) sleeve = 0.01 * double.Parse(els[5]);
                double? chest = null;
                if (els[6] != String.Empty) chest = 0.01 * double.Parse(els[6]);
                double? insideLeg = null;
                if (els[7] != String.Empty) insideLeg = 0.01 * double.Parse(els[7]);
                double? neck = null;
                if (els[8] != String.Empty) neck = 0.01 * double.Parse(els[8]);
                double? crotch = null;
                if (els[9] != String.Empty) crotch = 0.01 * double.Parse(els[9]);
                double? torsoLength = null;
                if (els[10] != String.Empty) torsoLength = 0.01 * double.Parse(els[10]);
                double? height = null;
                if (els[11] != String.Empty) height = 0.01 * double.Parse(els[11]);
                double? hips = null;
                if (els[12] != String.Empty) hips = 0.01 * double.Parse(els[12]);
                string sizeLetter = els[13];
                string sizeNumber = els[14].TrimEnd();
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
                    Crotch = crotch,
                    TorsoLength = torsoLength,
                    Height = height,
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
    public class Suit
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
        public double? Crotch { get; set; }

        [Column]
        public double? TorsoLength { get; set; }

        [Column]
        public double? Height {get; set; }

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
            // By convention, the order is determined by DressSizeUtils

            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.         
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - measuredVals[1] : 0.0;
            double dSleeve = (this.Sleeve.HasValue) ? (double)this.Sleeve - measuredVals[1] : 0.0;
            double dChest = (this.Chest.HasValue) ? (double)this.Chest - measuredVals[0] : 0.0;
            double dInsideLeg = (this.InsideLeg.HasValue) ? (double)this.InsideLeg - measuredVals[1] : 0.0;
            double dNeck = (this.Neck.HasValue) ? (double)this.Neck - measuredVals[1] : 0.0;
            double dCrotch = (this.Crotch.HasValue) ? (double)this.Crotch - measuredVals[1] : 0.0;
            double dTorsoLength = (this.TorsoLength.HasValue) ? (double)this.TorsoLength - measuredVals[1] : 0.0;
            double dHeight = (this.Height.HasValue) ? (double)this.Height - measuredVals[1] : 0.0;

            double chiSq = dWaist * dWaist + dSleeve * dSleeve + dChest * dChest + dInsideLeg * dInsideLeg 
                + dNeck * dNeck + dCrotch * dCrotch + dTorsoLength * dTorsoLength + dHeight * dHeight;

            if (measuredVals.Count == 10)
            {
                double dHips = (this.Hips.HasValue) ? (double)this.Hips - measuredVals[4] : 0.0;
                chiSq += dHips * dHips;
            }
            return chiSq;
        }

        #endregion

    }
}
