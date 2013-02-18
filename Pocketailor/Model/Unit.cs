
using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;



namespace Pocketailor.Model
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

        private string _nameString;
        [Column]
        public string NameString
        {
            get { return this._nameString; }
            set
            {
                if (this._nameString != value)
                {
                    this.NotifyPropertyChanging("Name");
                    this._nameString = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }
        public List<string> Names
        {
            get { return ModelHelpers.UnpickleStrings(this.NameString); }
            set { this.NameString = ModelHelpers.PickleStrings(value); }
        }
        public string Name
        {
            get { return String.Join(AppConstants.FORMATTED_NAME_SEPARATOR, this.Names.ToArray()); }
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

        private string _conversionFactorString;
        [Column]
        public string ConversionFactorString
        {
            get { return this._conversionFactorString; }
            set
            {
                if (this._conversionFactorString != value)
                {
                    this.NotifyPropertyChanging("ConversionFactor");
                    this._conversionFactorString = value;
                    this.NotifyPropertyChanged("ConversionFactor");
                }
            }
        }
        public List<double> ConversionFactors
        {
            get { return ModelHelpers.UnpickleDoubles(this.ConversionFactorString);  }
            set { this.ConversionFactorString = ModelHelpers.PickleDoubles(value); }
        }

        private string _conversionInterceptString;
        [Column]
        public string ConversionInterceptString
        {
            get { return this._conversionInterceptString; }
            set
            {
                if (this._conversionInterceptString != value)
                {
                    this.NotifyPropertyChanging("ConversionIntercept");
                    this._conversionInterceptString = value;
                    this.NotifyPropertyChanged("ConversionIntercept");
                }
            }
        }
        public List<double> ConversionIntercepts
        {
            get { return ModelHelpers.UnpickleDoubles(this.ConversionInterceptString); }
            set { this.ConversionInterceptString = ModelHelpers.PickleDoubles(value); }
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
                    NotifyPropertyChanged("Formattedvalue");
                }
            }
        }

        public string FormattedValue
        {
            get
            {
                return this.GetFormattedValue(App.VM.SelectedStat.Value);
            }
        }

        public string GetFormattedValue(string value)
        {
            List<double> convVals = this.ConvertValuesFromString(value);
            if (convVals.Count == 0) return String.Empty;
            return String.Format(this.Format, convVals.Cast<object>().ToArray());
        }

        public List<double> ConvertValuesFromString(string value)
        {
            double val;
            if (!double.TryParse(value, out val)) return new List<double>();
            List<double> convVals = new List<double>();
            for (int i = 0; i < this.ConversionFactors.Count; i++)
            {
                if (this.ConversionFactors.Count == 1)
                {
                    convVals.Add(this.ConversionFactors[i] * val + this.ConversionIntercepts[i]);
                }
                else
                {
                    double rem = this.ConversionFactors[i] * val + this.ConversionIntercepts[i];
                    convVals.Add(Math.Floor(rem));
                    val = (rem - convVals.Last()) / this.ConversionFactors[i];
                }
            }
            return convVals;
        }

        // Takes input from text and converts it to the database normalised string value
        // If multiple values (e.g. ft. inches.) then are delimited by pipe.
        public string ConvertValuesToString(string value)
        {
            double val = 0.0;
            List<double> vals = ModelHelpers.UnpickleDoubles(value);
            for (int i = 0; i < vals.Count; i++)
            {
                val += (vals[i] - this.ConversionIntercepts[i]) / this.ConversionFactors[i];
            }
            return String.Format("{0}", val);
        }


        public int GetNumberInputs() 
        {
            return this.ConversionFactors.Count;
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName)
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
