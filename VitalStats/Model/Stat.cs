using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace VitalStats.Model
{
    [Table]
    public class Stat : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Define table columns

        // This helps with updaing the schema
        [Column(IsVersion = true)]
        private Binary _version;

        private int _id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set
            {
                this.NotifyPropertyChanging("Id");
                this._id = value;
                this.NotifyPropertyChanged("Id");
            }
        }

        private string _name;
        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _value;
        [Column]
        public string Value
        {
            get { return _value; }
            set
            {
                NotifyPropertyChanging("Value");
                _value = value;
                NotifyPropertyChanged("Value");
            }
        }

        [Column]
        internal int? _preferredUnitId;
        private EntityRef<Unit> _preferredUnit = new EntityRef<Unit>();
        [Association(Storage = "_preferredUnit", ThisKey = "_preferredUnitId", OtherKey = "Id", IsForeignKey = true)]
        public Unit PreferredUnit
        {
            get { return this._preferredUnit.Entity; }
            set
            {
                this.NotifyPropertyChanging("PreferredUnit");
                this._preferredUnit.Entity = value;
                if (value != null)
                {
                    this._preferredUnitId = value.Id;
                }
                this.NotifyPropertyChanged("PreferredUnit");
            }
        }


        [Column]
        internal int? _profileId;
        private EntityRef<Profile> _profile = new EntityRef<Profile>();
        [Association(Storage = "_profile", ThisKey = "_profileId", OtherKey = "Id", IsForeignKey = true)]
        public Profile Profile
        {
            get { return this._profile.Entity; }
            set
            {
                this.NotifyPropertyChanging("Profile");
                this._profile.Entity = value;
                if (value != null)
                {
                    this._profileId = value.Id;
                }
                this.NotifyPropertyChanged("Profile");
            }
        }

        [Column]
        internal int _measurementTypeId;
        private EntityRef<MeasurementType> _measurementType = new EntityRef<MeasurementType>();
        [Association(Storage="_measurementType", ThisKey="_measurementTypeId", OtherKey="Id", 
            IsForeignKey=true)]
        public MeasurementType MeasurementType
        {
            get { return this._measurementType.Entity; }
            set
            {
                NotifyPropertyChanging("MeasurementType");
                this._measurementType.Entity = value;

                if (value != null)
                {
                    this._measurementTypeId = value.Id;
                }

                NotifyPropertyChanging("MeasurementType");
            }
        }

        #endregion

        #region Misc methods

        public string GetValueString(string unitString)
        {
            return this.MeasurementType.GetValueString(this.Value, unitString);
        }


        #endregion

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




}
