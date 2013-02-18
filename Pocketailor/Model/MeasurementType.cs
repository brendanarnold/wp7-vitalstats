using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace Pocketailor.Model
{
    [Table]
    public class MeasurementType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public MeasurementType()
        {
            #region EntitySet bookkeeping

            this._stats = new EntitySet<Stat>(
                delegate(Stat entity) {
                    this.NotifyPropertyChanging("Stats");
                    entity.MeasurementType = this;
                },
                delegate(Stat entity) {
                    this.NotifyPropertyChanging("Stats");
                    entity.MeasurementType = null;
                });
            this._units = new EntitySet<Unit>(
                delegate(Unit entity)
                {
                    this.NotifyPropertyChanging("Units");
                    entity.MeasurementType = this;
                },
                delegate(Unit entity) {
                    this.NotifyPropertyChanging("Units");
                    entity.MeasurementType = null;
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
            get { return _id; }
            set
            {
                this.NotifyPropertyChanging("Id");
                _id = value;
                this.NotifyPropertyChanged("Id");
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

        private EntitySet<Stat> _stats;
        [Association(Storage = "_stats", OtherKey = "_measurementTypeId")]
        public EntitySet<Stat> Stats
        {
            get { return this._stats; }
            set { this._stats.Assign(value); }
        }

        private EntitySet<Unit> _units;
        [Association(Storage = "_units", OtherKey="_measurementTypeId")]
        public EntitySet<Unit> Units
        {
            get { return this._units; }
            set { this._units.Assign(value); }
        }

        #region Misc methods

        public bool IsConvertible()
        {
            return this.Units.Count > 0;
        }


        public string GetValueString(string strValue, string unitString)
        {
        //    double value, factor, intercept;
        //    string[] convFactors, convIntercepts;
        //    string fmt;
        //    int ind;
        //    List<double> vals = new List<double>();

        //    if (!this.IsConvertible) return strValue;
        //    if (!double.TryParse(strValue, out value)) return strValue;
        //    if (!this.UnitNames.Contains(unitString)) return strValue;
        //    ind = this.UnitNames.IndexOf(unitString);
        //    fmt = this.UnitFormats[ind];
        //    convFactors = this.ConversionFactors[ind].Split(new char[] { '|' });
        //    convIntercepts = this.ConversionIntercepts[ind].Split(new char[] { '|' });
        //    if (convFactors.Length == 1)
        //    {
        //        double.TryParse(convFactors[0], out factor);
        //        double.TryParse(convIntercepts[0], out intercept);
        //        return string.Format(fmt, value * factor + intercept);
        //    }
        //    else
        //    {

        //        for (int i = 0; i < convFactors.Length; i++)
        //        {
        //            double.TryParse(convFactors[i], out factor);
        //            double.TryParse(convIntercepts[i], out intercept);
        //            vals.Add(Math.Floor(value * factor + intercept));
        //            value = value - vals[vals.Count - 1];
        //        }
        //        return string.Format(fmt, vals.ToArray());
        //    }

        return "5 ft. 8 in.";
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
