using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    class DoubleValueUnit : IUnit
    {

        public UnitId _id { get; set; }

        public List<string> _names { get; set; }

        public List<string> _shortUnitNames { get; set; }

        public List<double> _conversionFactors { get; set; }

        public List<double> _conversionIntercepts { get; set; }

        public List<string> _valueFormats { get; set; }


        #region IUnit properties/methods

        public UnitId Id {
            get
            {
                return this._id;
            }
        }

        public string Name
        {
            get
            {
                return String.Join(AppConstants.FORMATTED_NAME_SEPARATOR, this._names.ToArray());
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
            List<string> formattedVals = this.ConvertFromDBString(val);
            string outStr = String.Empty;
            for (int i = 0; i < formattedVals.Count; i++)
            {
                outStr += formattedVals[i] + " " + this._shortUnitNames[i] + " ";
            }
            return outStr.TrimEnd();
        }

        public List<string> ShortUnitNames {
            get
            {
                return this._shortUnitNames;
            }
        }

        public List<string> ConvertFromDBString(string strVal)
        {
            double remaining;

            if (!double.TryParse(strVal, out remaining)) return null;
            List<string> outVals = new List<string>();
            for (int i = 0; i < this._conversionFactors.Count; i++)
            {
                double whole;
                double result = this._conversionFactors[i] * remaining + this._conversionIntercepts[i];
                whole = Math.Floor(result);
                remaining = (remaining - whole) / this._conversionFactors[i];
                outVals.Add(String.Format(this._valueFormats[i], whole));
            }
            return outVals;
        }

        public string ConvertToDBString(List<string> vals)
        {
            double val = 0.0;
            for (int i = 0; i < vals.Count; i++)
            {
                double d;
                double.TryParse(vals[i], out d);
                val += (d - this._conversionIntercepts[i]) / this._conversionFactors[i];
            }
            return String.Format("{0}", val);
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
