﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    public static class HosieryUtils
    {
        public static List<MeasurementId> RequiredMeasurements = new List<MeasurementId>()
        {
            MeasurementId.Hips,
            MeasurementId.Waist,
            MeasurementId.InsideLeg,
        };

        public static void ReloadCsvToDB(AppDataContext db)
        {
            db.Hosiery.DeleteAllOnSubmit(db.Hosiery);
            db.SubmitChanges();
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\Hosiery.txt", UriKind.Relative));
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
                double? waist = null;
                els.MoveNext();
                if (els.Current != String.Empty) waist = 0.01 * double.Parse(els.Current);
                double? hips = null;
                els.MoveNext();
                if (els.Current != String.Empty) hips = 0.01 * double.Parse(els.Current);
                double? insideLeg = null;
                els.MoveNext();
                if (els.Current != String.Empty) insideLeg = 0.01 * double.Parse(els.Current);
                els.MoveNext();
                string sizeLetter = els.Current;
                els.MoveNext();
                string sizeNumber = els.Current.TrimEnd();
                db.Hosiery.InsertOnSubmit(new Hosiery()
                {
                    Retailer = retailer,
                    Region = region,
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
    public class Hosiery : IConversionData
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

        public Gender Gender {
            get
            {
                return Gender.Female;
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
            double dWaist = (this.Waist.HasValue) ? (double)this.Waist - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dHips = (this.Hips.HasValue) ? (double)this.Hips - enumerator.Current : 0.0;
            enumerator.MoveNext();
            double dInsideLeg = (this.InsideLeg.HasValue) ? (double)this.InsideLeg - enumerator.Current : 0.0;

            double chiSq = dWaist * dWaist + dHips * dHips + dInsideLeg * dInsideLeg; 

            return chiSq;
        }

        #endregion

    }
}
