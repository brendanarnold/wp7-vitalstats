using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace VitalStats.Model
{
    [Table]
    public class Profile : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public Profile()
        {
            this._stats = new EntitySet<Stat>(
                new Action<Stat>(this._attachStats),
                new Action<Stat>(this._detatchStats)
                );
        }

        // This helps with updaing the schema
        [Column(IsVersion = true)]
        private Binary _version;

        private int _id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return this._id; }
            set
            {
                if (this._id != value)
                {
                    this.NotifyPropertyChanging("Id");
                    this._id = value;
                    this.NotifyPropertyChanged("Id");
                }
            }
        }

        private string _name;
        [Column]
        public string Name
        {
            get { return this._name; }
            set
            {
                if (this._name != value)
                {
                    this.NotifyPropertyChanging("Name");
                    this._name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        private bool _isProtected;
        [Column]
        public bool IsProtected
        {
            get { return this._isProtected; }
            set
            {
                if (this._isProtected != value)
                {
                    this.NotifyPropertyChanging("IsProtected");
                    this._isProtected = value;
                    this.NotifyPropertyChanged("IsProtected");
                }
            }
        }

        private EntitySet<Stat> _stats;
        [Association(Storage = "_stats", OtherKey = "Id", ThisKey = "Id")]
        public EntitySet<Stat> Stats
        {
            get { return this._stats; }
            set { this._stats.Assign(value); }
        }
     

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotify PropertyChanging members

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion


        private void _attachStats (Stat stat) 
        {
            this.NotifyPropertyChanging("Stat");
            stat.Profile = this;
        }

        private void _detatchStats(Stat stat) 
        {
            this.NotifyPropertyChanging("Stat");
            stat.Profile = null;
        }

 
        
    }

}
