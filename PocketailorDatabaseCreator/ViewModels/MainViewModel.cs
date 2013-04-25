using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Pocketailor.Model;


namespace PocketailorDatabaseCreator
{
    public class MainViewModel
    {
        public MainViewModel()
        {
        }

        public void LoadUpCsv() 
        {
            string dbConnectionString = "Data Source=isostore:/PocketailorConversions.sdf";
            ConversionsDataContext conversionsDb = new ConversionsDataContext(dbConnectionString);
            if (conversionsDb.DatabaseExists())
            {
                conversionsDb.DeleteDatabase();
            }
            conversionsDb.CreateDatabase();

            this.TestLoadingWithoutCsv(conversionsDb);
            //PocketailorDatabaseCreator.Model.SetupDatabase.LoadConversions(conversionsDb);
        }


        public void TestLoadingWithoutCsv(ConversionsDataContext db)
        {
            db.DressSizes.InsertOnSubmit(new Pocketailor.Model.Conversions.DressSize()
            {
                Chest = 50.0,
                Waist = 50.0,
                Hips = 50.0,
                SizeLetter = "XS",
                SizeNumber = "8",
                Region = RegionIds.Worldwide,
                Retailer = RetailId.ASOS,
            });
            db.SubmitChanges();
        }


    }
}