using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public class SingleValueUnit : IUnit
    {

        public UnitId _id { get; set; }

        public string _name { get; set; }

        public string _valueFormat { get; set; }

        public string _shortUnitName { get; set; }

        public double _conversionFactor { get; set; }

        public double _conversionIntercept { get; set; }





        #region IUnit properties/methods

        public UnitId Id {
            get
            {
                return this._id;
            }
        }

        public string Name {
            get
            {
                return this._name;
            }
        }

        
        public string FormattedValue
        {
            get
            {
                return this.GetFormattedValue(App.VM.SelectedStat.Value);
            }
        }

        public string GetFormattedValue(string val)
        {
            List<string> strs = this.ConvertFromDBString(val);
            return String.Format(this._valueFormat, strs.Cast<object>().ToArray()) + " " + this.ShortUnitNames[0];
        }


        public List<string> ShortUnitNames {
            get
            {
                return new List<string>() { this._shortUnitName };
            }
        }

        public string ConvertToDBString(List<string> vals)
        {
            double val;
            if (!double.TryParse(vals[0], out val)) return String.Empty;
            val = (val - this._conversionIntercept) / this._conversionFactor;
            return String.Format("{0}", val);
        }


        public List<string> ConvertFromDBString(string strValue)
        {
            double val;
            if (!double.TryParse(strValue, out val)) return null;
            List<string> outVals = new List<string>();
            outVals.Add(String.Format(this._valueFormat, this._conversionFactor * val + this._conversionIntercept));
            return outVals;
        }


        #endregion


        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    
    
    
    }
}
