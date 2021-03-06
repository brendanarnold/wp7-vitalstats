﻿using System;
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
            string dbConnectionString;
            if (VersionHelpers.IsWP8)
            {
                dbConnectionString = "Data Source=isostore:/PocketailorConversionsWP8.sdf";
            }
            else
            {
                dbConnectionString = "Data Source=isostore:/PocketailorConversionsWP7.sdf";
            }
            ConversionsDataContext conversionsDb = new ConversionsDataContext(dbConnectionString);
            if (conversionsDb.DatabaseExists())
            {
                conversionsDb.DeleteDatabase();
            }
            conversionsDb.CreateDatabase();

            //this.TestLoadingWithoutCsv(conversionsDb);
            PocketailorDatabaseCreator.Model.SetupDatabase.LoadConversions(conversionsDb);
        }


        public void TestLoadingWithoutCsv(ConversionsDataContext db)
        {
            Pocketailor.Model.ConversionData cd = new Pocketailor.Model.ConversionData()
            {
                Measurements = new Dictionary<MeasurementId, List<double>>()
                {
                    {MeasurementId.Chest, new List<double>() {-1, 2, 3, 4, -1 }},
                    {MeasurementId.Hips, new List<double>() {1, 2, 3, 4, 5 }},
                    {MeasurementId.Waist, new List<double>() {1, 2, 3, 4, 5 }},
                },
                Conversion = ConversionId.DressSize,
                Region = "GB",
                Brand = BrandId.MarksSpencer,
                Gender = GenderId.Female,
                Sizes = new List<string>() { "XS", "S", "M", "L", "XL" },
            };
            cd.JsonifyData();
            db.ConversionData.InsertOnSubmit(cd);
            db.SubmitChanges();
        }


    }
}