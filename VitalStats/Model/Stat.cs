using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace VitalStats.Model
{
    [Table]
    public class Stat : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public Stat()
        {
            this.PropertyChanged += new PropertyChangedEventHandler(Stat_PropertyChanged);
            this.UpdateValueInUnits();
        }

        void Stat_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "Value") || (e.PropertyName == "MeasurementType"))
            {
                this.UpdateValueInUnits();
            }
        }

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
                // Units do not have a reference to the value 
                // so update the value on each unit
                this.UpdateValueInUnits();
            }
        }

        [Column]
        internal int? _preferredUnitId;
        private EntityRef<Unit> _preferredUnit = new EntityRef<Unit>();
        [Association(Storage = "_preferredUnit", ThisKey = "_preferredUnitId", IsForeignKey = true)]
        public Unit PreferredUnit
        {
            get { return this._preferredUnit.Entity; }
            set
            {
                Unit u = this._preferredUnit.Entity;
                if (u != value)
                {
                    this.NotifyPropertyChanging("PreferredUnit");
                    if (u != null)
                    {
                        this._preferredUnit.Entity = null;
                    }
                    this._preferredUnit.Entity = value;
                    this.NotifyPropertyChanged("PreferredUnit");
                }
            }
        }

        [Column]
        internal int? _profileId;
        private EntityRef<Profile> _profile = new EntityRef<Profile>();
        [Association(Storage = "_profile", ThisKey = "_profileId", OtherKey = "Id", 
            IsForeignKey= true)]
        public Profile Profile
        {
            get { return this._profile.Entity; }
            set
            {
                Profile p = this._profile.Entity;
                if (p != value)
                {
                    NotifyPropertyChanging("Profile");
                    if (p != null)
                    {
                        this._profile.Entity = null;
                        p.Stats.Remove(this);
                    }
                    this._profile.Entity = value;
                    if (value != null)
                    {
                        value.Stats.Add(this);
                    }
                    NotifyPropertyChanged("Profile");
                }

            }
        }

        [Column]
        internal int _measurementTypeId;
        private EntityRef<MeasurementType> _measurementType = new EntityRef<MeasurementType>();
        [Association(Storage = "_measurementType", ThisKey = "_measurementTypeId", OtherKey = "Id", 
            IsForeignKey = true)]
        public MeasurementType MeasurementType
        {
            get { return this._measurementType.Entity; }
            set
            {
                MeasurementType mt = this._measurementType.Entity;
                if (mt != value)
                {
                    NotifyPropertyChanging("MeasurementType");
                    if (mt != null)
                    {
                        this._measurementType.Entity = null;
                        mt.Stats.Remove(this);
                    }
                    this._measurementType.Entity = value;
                    if (value != null)
                    {
                        value.Stats.Add(this);
                    }
                    NotifyPropertyChanged("MeasurementType");
                }
            }
        }

        #endregion

        #region Misc methods

        public string FormattedValue
        {
            get
            {
                if (this.PreferredUnit == null)
                {
                    return this.Value;
                }
                else
                {
                    return this.PreferredUnit.GetFormattedValue(this.Value);
                }
            }
        }

        private void UpdateValueInUnits()
        {
            if (this.MeasurementType != null)
            {
                foreach (Unit u in this.MeasurementType.Units)
                {
                    u._value = this.Value;
                }
            }
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
