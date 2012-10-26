using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace VitalStats.Model
{
    [Table]
    public class Unit : INotifyPropertyChanged, INotifyPropertyChanging 
    {

        // This helps with updaing the schema
        [Column(IsVersion = true)]
        private Binary _version;

        private int _id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false, AutoSync = AutoSync.OnUpdate)]
        public int Id
        {
            get { return this._id; }
            set
            {
                if (this._id != value)
                {
                    this.NotifyPropertyChanging("ProfileId");
                    this._id = value;
                    this.NotifyPropertyChanged("ProfileId");
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

        private string _format;
        [Column]
        public string Format
        {
            get { return this._format; }
            set
            {
                if (this._format != value)
                {
                    this.NotifyPropertyChanging("Format");
                    this._format = value;
                    this.NotifyPropertyChanged("Format");
                }
            }
        }

        private string _conversionFactor;
        [Column]
        public string ConversionFactor
        {
            get { return this._conversionFactor; }
            set
            {
                if (this._conversionFactor != value)
                {
                    this.NotifyPropertyChanging("ConversionFactor");
                    this._conversionFactor = value;
                    this.NotifyPropertyChanged("ConversionFactor");
                }
            }
        }

        private string _conversionIntercept;
        [Column]
        public string ConversionFormat
        {
            get { return this._conversionIntercept; }
            set
            {
                if (this._conversionIntercept != value)
                {
                    this.NotifyPropertyChanging("ConversionIntercept");
                    this._conversionIntercept = value;
                    this.NotifyPropertyChanged("ConversionIntercept");
                }
            }
        }

        [Column]
        internal int _measurementTypeId;
        private EntityRef<MeasurementType> _measurementType;
        [Association(Storage = "_measurementType", ThisKey = "_measurementTypeId", OtherKey = "Id",
            IsForeignKey = true)]
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


        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (propertyName != null)
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

        // Have to have this for binding for some reason ...
        internal object GetCopy()
        {
            return (Profile)this.MemberwiseClone();
        }
 
    }
}
