using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
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

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.Trousers.DeleteAllOnSubmit(db.Trousers);
            db.SubmitChanges();
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\Trousers.txt", UriKind.Relative));
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
                string[] els = line.Split(new char[] { '\t' });
                RetailId retailer = (RetailId)Enum.Parse(typeof(RetailId), els[0], true);
                RegionTag region = (RegionTag)Enum.Parse(typeof(RegionTag), els[1], true);
                Gender gender = (Gender)Enum.Parse(typeof(Gender), els[2], true);
                // Store in DB as metres (input file is is centimetres inline with most charts in shops)
                double? waist = null;
                if (els[3] != String.Empty) waist = 0.01 * double.Parse(els[3]);
                double? hips = null;
                if (els[4] != String.Empty) hips = 0.01 * double.Parse(els[4]);
                double? insideLeg = null;
                if (els[5] != String.Empty) insideLeg = 0.01 * double.Parse(els[5]);
                string sizeLetter = els[6];
                string sizeNumber = els[7].TrimEnd();
                db.Trousers.InsertOnSubmit(new Trousers()
                {
                    Retailer = retailer,
                    Region = region,
                    Gender = gender,
                    Waist = waist,
                    Hips = hips,
                    InsideLeg = insideLeg,
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

            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.         
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - measuredVals[0] : 0.0;
            double dInsideLeg = (this.InsideLeg.HasValue) ? (double)this.InsideLeg - measuredVals[1] : 0.0;

            double chiSq = dWaist * dWaist + dInsideLeg * dInsideLeg;
            // Extra bit because womens sizes also consider the hips
            if (measuredVals.Count == 5)
            {
                double dHips = (this.Hips.HasValue) ? (double)this.Hips - measuredVals[2] : 0.0;
                chiSq += dHips * dHips;
            }
            return chiSq;
        }

        #endregion


    }
}
