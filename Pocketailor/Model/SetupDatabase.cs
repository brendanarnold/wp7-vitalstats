using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.IO;
using Pocketailor.Model.Conversions;

namespace Pocketailor.Model
{
    public class SetupDatabase
    {

        public static void InitialiseDB(AppDataContext db)
        {
            db.CreateDatabase();
            LoadConversions(db);
            db.SubmitChanges();
        }

        public static void EmptyDB(AppDataContext db)
        {
            // The order here is important so as to not leave hanging references
            db.Stats.DeleteAllOnSubmit(db.Stats);
            db.Profiles.DeleteAllOnSubmit(db.Profiles);
            db.DressSizes.DeleteAllOnSubmit(db.DressSizes);
            db.SubmitChanges();
        }

        public static void LoadConversions(AppDataContext db)
        {
            // Load in dress sizes
            var res = System.Windows.Application.GetResourceStream(new Uri("Model\\Data\\DressSizes.csv", UriKind.Relative));
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
                double chest = 100 * double.Parse(els[2]);
                double waist = 100 * double.Parse(els[3]);
                double hips = 100 * double.Parse(els[4]);
                string sizeLetter = els[5];
                string sizeNumber = els[6].TrimEnd();
                db.DressSizes.InsertOnSubmit(new DressSize() {
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
}
