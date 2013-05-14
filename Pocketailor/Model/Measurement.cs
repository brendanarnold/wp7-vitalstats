using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;


namespace Pocketailor.Model
{
    [Table]
    public class Measurement : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public Measurement()
        {
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

        private MeasurementId _measurementId;
        [Column]
        public MeasurementId MeasurementId
        {
            get { return this._measurementId; }
            set
            {
                this.NotifyPropertyChanging("MeasurementId");
                this._measurementId = value;
                NotifyPropertyChanged("MeasurementId");
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
                // Changing this changes the formatted value so need to notify UI
                NotifyPropertyChanged("FormattedValue");
                this.NotifyPropertyChanged("OtherUnitFormattedValues");
                // Also need to notify units attached to this stat
                if (this.MeasurementType != null)
                {
                    foreach (IUnit u in this.MeasurementType.Units) 
                        u.NotifyPropertyChanged("FormattedValue");
                }
            }
        }

        [Column]
        internal UnitId? _preferredUnitId { get; set; }
        
        public IUnit PreferredUnit
        {
            get 
            {
                if (this._preferredUnitId == null) return null;
                return (from IUnit u in this.MeasurementType.Units where u.Id == this._preferredUnitId select u).First(); 
            }
            set
            {
                IUnit u = this.PreferredUnit;
                if (u != value)
                {
                    this.NotifyPropertyChanging("PreferredUnit");
                    this._preferredUnitId = value.Id;
                    this.NotifyPropertyChanged("PreferredUnit");
                    // This also affects the formatted value on the stat
                    this.NotifyPropertyChanged("FormattedValue");
                    this.NotifyPropertyChanged("OtherUnitFormattedValues");
                }
            }
        }


        [Column]
        internal MeasurementTypeId? _measurementTypeId { get; set; }

        public MeasurementType MeasurementType
        {
            get 
            {
                if (this._measurementTypeId == null) return null;
                return (from MeasurementType mt in App.VM.MeasurementTypes where mt.Id == this._measurementTypeId select mt).First(); 
            }
            set
            {
                MeasurementType mt = this.MeasurementType;
                if (mt != value)
                {
                    NotifyPropertyChanging("MeasurementType");
                    this._measurementTypeId = value.Id;
                    NotifyPropertyChanged("MeasurementType");
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

        public ObservableCollection<string> OtherUnitFormattedValues
        {
            get
            {
                if (this.MeasurementType == null) return null;
                ObservableCollection<string> ret = new ObservableCollection<string>();
                foreach (IUnit u in this.MeasurementType.Units)
                {
                    if (u.Id == this.PreferredUnit.Id) continue;
                    ret.Add(u.GetFormattedValue(this.Value));
                }
                return ret;
            }
        }

        public Measurement GetCopy()
        {
            return this.MemberwiseClone() as Measurement;
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
