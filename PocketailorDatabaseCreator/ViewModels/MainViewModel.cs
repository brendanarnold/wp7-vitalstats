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
            PocketailorDatabaseCreator.Model.SetupDatabase.LoadConversions(conversionsDb);
        }


    }
}