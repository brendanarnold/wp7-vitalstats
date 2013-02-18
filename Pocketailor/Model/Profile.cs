using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Pocketailor.Model
{
    [Table]
    public class Profile : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public Profile()
        {
            #region EntitySet bookkeeping

            this._stats = new EntitySet<Stat>(
                delegate(Stat entity)
                {
                    this.NotifyPropertyChanging("Stats");
                    entity.Profile = this;
                },
                delegate(Stat entity)
                {
                    this.NotifyPropertyChanging("Stats");
                    entity.Profile = null;
                });

            #endregion
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

        private bool _isQuickProfile = false;
        [Column]
        public bool IsQuickProfile
        {
            get { return this._isQuickProfile; }
            set
            {
                if (this._isQuickProfile != value)
                {
                    this.NotifyPropertyChanging("IsQuickProfile");
                    this._isQuickProfile = value;
                    this.NotifyPropertyChanged("IsQuickProfile");
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
        [Association(Storage = "_stats", OtherKey = "_profileId", ThisKey = "Id", DeleteRule="CASCADE")]
        public EntitySet<Stat> Stats
        {
            get { return this._stats; }
            set { this._stats.Assign(value); }
        }

        private Model.Gender _gender = Model.Gender.Unspecified;
        [Column]
        public Model.Gender Gender
        {
            get { return this._gender; }
            set
            {
                if (this._gender != value)
                {
                    this.NotifyPropertyChanging("Gender");
                    this._gender = value;
                    this.NotifyPropertyChanged("Gender");
                }
            }
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



 
        
    }

    
    public enum Gender
    {
        Unspecified,
        Female,
        Male
    }

}
