using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;

namespace Pocketailor.Model.Conversions
{
    public static class WetsuitUtils
    {
        public static List<MeasurementId> RequiredMeasurementsMens = new List<MeasurementId>()
        {
            MeasurementId.Height,
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Weight,
        };
        public static List<MeasurementId> RequiredMeasurementsWomens = new List<MeasurementId>()
        {
            MeasurementId.Height,
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Weight,
            MeasurementId.Hips,
        };

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.DressSizes.DeleteAllOnSubmit(db.DressSizes);
            db.SubmitChanges();
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\Wetsuit.txt", UriKind.Relative));
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
                double? height = null;
                if (els[3] != String.Empty) height = 0.01 * double.Parse(els[3]);
                double? chest = null;
                if (els[4] != String.Empty) chest = 0.01 * double.Parse(els[4]);
                double? waist = null;
                if (els[5] != String.Empty) waist = 0.01 * double.Parse(els[5]);
                double? hips = null;
                if (els[6] != String.Empty) hips = 0.01 * double.Parse(els[6]);
                double? weight = null;
                if (els[7] != String.Empty) weight = double.Parse(els[7]);
                string sizeLetter = els[8];
                string sizeNumber = els[9].TrimEnd();
                db.Wetsuits.InsertOnSubmit(new Wetsuit()
                {
                    Retailer = retailer,
                    Region = region,
                    Gender = gender,
                    Height = height,
                    Chest = chest,
                    Waist = waist,
                    Hips = hips,
                    Weight = weight,
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
    public class Wetsuit : IConversionData
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
        public double? Height { get; set; }

        [Column]
        public double? Weight { get; set; }

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
            double dHeight = (this.Height.HasValue) ? (double)this.Height - measuredVals[0] : 0.0;           
            double dChest =  (this.Chest.HasValue)  ? (double)this.Chest  - measuredVals[1] : 0.0;
            double dWaist =  (this.Waist.HasValue)  ? (double)this.Waist  - measuredVals[2] : 0.0;
            double dWeight = (this.Weight.HasValue) ? (double)this.Weight - measuredVals[3] : 0.0;

            double chiSq = dHeight * dHeight + dChest * dChest + dWaist * dWaist + dWeight * dWeight;
            // Extra bit because womens sizes also consider the hips
            if (measuredVals.Count == 5)
            {
                double dHips = (this.Hips.HasValue) ? (double)this.Hips - measuredVals[4] : 0.0;
                chiSq += dHips * dHips;
            }
            return chiSq;
        }

        #endregion

    }
}
