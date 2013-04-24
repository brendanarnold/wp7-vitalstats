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
            Model.Conversions.TrousersUtils.ReloadCsvToDB(db);
            Model.Conversions.ShirtUtils.ReloadCsvToDB(db);
            Model.Conversions.HatUtils.ReloadCsvToDB(db);
            Model.Conversions.SuitUtils.ReloadCsvToDB(db);
            Model.Conversions.DressSizeUtils.ReloadCsvToDB(db);
            Model.Conversions.BraUtils.ReloadCsvToDB(db);
            Model.Conversions.HosieryUtils.ReloadCsvToDB(db);
            Model.Conversions.ShoesUtils.ReloadCsvToDB(db);
            Model.Conversions.SkiBootsUtils.ReloadCsvToDB(db);
            //Model.Conversions.TennisRaquetSizesUtils.ReloadCsvToDB(db);
            Model.Conversions.WetsuitUtils.ReloadCsvToDB(db);
            
        }


        public static List<T> ReadCsvFile<T>(Uri fn)
        {
            var res = System.Windows.Application.GetResourceStream(fn);
            System.IO.StreamReader fh = new System.IO.StreamReader(res.Stream);

            int count = 0;
            List<string> headers;
            while (!fh.EndOfStream)
            {
                count++;
                string line = fh.ReadLine();
                // Read headers
                if (count == 1)
                {
                    headers = line.Split(AppConstants.CSV_DELIMITERS).ToList<string>();
                    continue;
                }
                // Skip commented lines
                if (line.StartsWith("#")) continue;
                var els = line.Split(AppConstants.CSV_DELIMITERS).ToList<string>();
                T entry = new T();
                Type conversionType = entry.GetType();
                for (int i=0; i < els.Count; i++) 
                {
                    var property = conversionType.GetProperty(headers[i]);
                    double? dval = 0.0;
                    string sval = null;
                    switch (headers[i]) 
                    {
                        case "Retailer":
                            property.SetValue(entry, (RetailId)Enum.Parse(typeof(RetailId), els[i], true), null);
                            break;
                        case "Region":
                            property.SetValue(entry, (RegionIds)Enum.Parse(typeof(RegionIds), els[i], true), null);
                            break;
                        case "Gender":
                        case "SizeLetter":
                        case "SizeNumber":
                            sval = els[0];
                            break;
                        // Assume all remaining properties are numbers
                        default:
                            double d;
                            if (double.TryParse(els[0], out d)) dval = d;
                            break;
                    }
                    
                    if (sval == null) 
                    {
                        property.SetValue(entry, dval, null);
                    } 
                    else 
                    {
                        property.SetValue(entry, sval, null);
                    }
                }
                
                db.Bras.InsertOnSubmit(new Bra()
                {
                    Retailer = retailer,
                    Region = region,
                    Chest = chest,
                    UnderBust = underBust,
                    SizeLetter = sizeLetter,
                    SizeNumber = sizeNumber,
                });
        }

    }
}
