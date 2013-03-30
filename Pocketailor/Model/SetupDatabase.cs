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
            Model.Conversions.TennisRaquetSizesUtils.ReloadCsvToDB(db);
            Model.Conversions.WetsuitUtils.ReloadCsvToDB(db);
            
        }


    }
}
