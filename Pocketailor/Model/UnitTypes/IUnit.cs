using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{

    public interface IUnit : INotifyPropertyChanged
    {
        // Used to specify preferred unit when stored in db
        UnitId Id
        {
            get;
        }
        // A single string containing all the unit names
        string Name
        {
            get;
        }
        // A single string containing all the formatted values (of the selected stat) with unit names
        string FormattedValue
        {
            get;
        }
        // A single string containing all the formatted values (of an arbitrary value) with unit names
        string GetFormattedValue(string val);
        // A list of the shortened unit names
        List<string> ShortUnitNames
        {
            get;
        }
        // Takes input from TextBoxes (after validation) and converts them to the database normalised string value
        string ConvertToDBString(List<string> vals);
        // Takes the normalised database value string and converts to the formatted values for display in UI
        List<string> ConvertFromDBString(string val);


        // UI binds to property on units (e.g. FormattedValue) so necessary to expose method when selected stat value changes
        void NotifyPropertyChanged(string propertyName);
    }


}
