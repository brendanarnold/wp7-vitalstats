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
using System.Data.Linq;
using Pocketailor.Model;
using System.ComponentModel;

namespace Pocketailor.ViewModel
{
    // The boring code for the AppViewModel, the interesting stuff is in AppViewModel.cs
    public partial class AppViewModel
    {

        private string ConnectionString;
        private AppDataContext appDB;

        // Empty constructor needed for design-time data created in XAML
        public AppViewModel() { }

        public AppViewModel(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.RecreateDataContext();
        }

        

        /// <summary>
        /// Checks for uncommitted changes in the ORM
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsPendingChangesForDB()
        {
            ChangeSet changes = this.appDB.GetChangeSet();
            return ((changes.Updates.Count > 0) || (changes.Inserts.Count > 0) || (changes.Deletes.Count > 0));
        }

        /// <summary>
        /// Recreates the database context, cleaning the uncommitted changes from the ORM
        /// </summary>
        public void RecreateDataContext()
        {
            this.appDB = null;
            this.appDB = new AppDataContext(this.ConnectionString);
        }

        /// <summary>
        /// Save the cches changes in the ORM to the DB
        /// </summary>
        public void SaveChangesToDB()
        {
            this.appDB.SubmitChanges();
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
