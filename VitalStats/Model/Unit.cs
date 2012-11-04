
using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;



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
        public string ConversionIntercept
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
        private EntityRef<MeasurementType> _measurementType = new EntityRef<MeasurementType>();
        [Association(Storage = "_measurementType", ThisKey = "_measurementTypeId", IsForeignKey = true)]
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
                        mt.Units.Remove(this);
                    }
                    this._measurementType.Entity = value;
                    if (value != null)
                    {
                        value.Units.Add(this);
                    }
                    NotifyPropertyChanged("MeasurementType");
                }
            }
        }

        internal string _value;

        public string FormattedValue
        {
            get
            {
                return this.GetFormattedValue(App.VM.SelectedStat.Value);
            }
        }

        public List<double> GetValuesAsDoubles(string value)
        {
            double val, a, b;
            val = Convert.ToDouble(value);
            string[] aVals = this.ConversionFactor.Split(AppConstants.VALUE_DELIMITERS);
            string[] bVals = this.ConversionIntercept.Split(AppConstants.VALUE_DELIMITERS);
            List<double> convVals = new List<double>();
            for (int i = 0; i < aVals.Length; i++)
            {
                a = Convert.ToDouble(aVals[i]);
                b = Convert.ToDouble(bVals[i]);
                if (aVals.Length == 1)
                {
                    convVals.Add(a * val + b);
                }
                else
                {
                    double rem = a * val + b;
                    convVals.Add(Math.Floor(rem));
                    val = (rem - convVals.Last()) / a;
                }
            }
            return convVals;
        }


        public string GetFormattedValue(string value)
        {
            List<double> convVals = this.GetValuesAsDoubles(value);
            return String.Format(this.Format, convVals.Cast<object>().ToArray());
        }

        // Takes input from text and converts it to the database normalised string value
        // If multiple values (e.g. ft. inches.) then are delimited by pipe.
        public string GetDBValue(string value)
        {
            double a, b, val = 0.0;
            string[] vals = value.Split(AppConstants.VALUE_DELIMITERS);
            string[] aVals = this.ConversionFactor.Split(AppConstants.VALUE_DELIMITERS);
            string[] bVals = this.ConversionIntercept.Split(AppConstants.VALUE_DELIMITERS);
            for (int i = 0; i < vals.Length; i++)
            {
                a = Convert.ToDouble(aVals[i]);
                b = Convert.ToDouble(bVals[i]);
                val += (Convert.ToDouble(vals[i]) - b) / a;
            }
            return String.Format("{0}", val);
        }


        public int GetNumberInputs() 
        {
            return this.ConversionFactor.Split(AppConstants.VALUE_DELIMITERS).Count();
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
}
