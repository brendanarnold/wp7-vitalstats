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
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\DressSizes.txt", UriKind.Relative));
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
                // Store in DB as metres (input file is is centimetres inline with most charts in shops)
                double? chest = null;
                if (els[2] != String.Empty) chest = 0.01 * double.Parse(els[2]);
                double? waist = null;
                if (els[3] != String.Empty) waist = 0.01 * double.Parse(els[3]);
                double? hips = null;
                if (els[4] != String.Empty) hips = 0.01 * double.Parse(els[4]);
                string sizeLetter = els[5];
                string sizeNumber = els[6].TrimEnd();
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
        public RegionTag Region { get; set; }

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

            // Some retailers do no provide all conversion measurements. These are defined to fit 'perfectly' in the least square fits.         
            double dChest = (this.Chest.HasValue) ? (double)this.Chest - measuredVals[0] : 0.0;
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - measuredVals[1] : 0.0;
            double dHips  = (this.Hips.HasValue)  ? (double)this.Hips  - measuredVals[2] : 0.0;

            return dChest * dChest + dWaist * dWaist + dHips * dHips;
        }

        #endregion

    }
}
