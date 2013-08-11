using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public class MeasurementType
    {
        public string Name { get; set; }
        public MeasurementTypeId Id { get; set; }
        // A dictionary set with the default units for Imperial and Metric
        public Dictionary<UnitCultureId, UnitId> DefaultUnitDict;
        // A dictionary set with the alternate units for Imperial and Metric e.g. height is in ft. in. not in.
        public Dictionary<UnitCultureId, UnitId> AltDefaultUnitDict;
        public IUnit GetDefaultUnit(UnitCultureId unitCultureId)
        {
            UnitId id = this.DefaultUnitDict[unitCultureId];
            return (from IUnit u in this.Units where u.Id == id select u).First();
        }
        public IUnit GetAltDefaultUnit(UnitCultureId unitCultureId)
        {
            UnitId id = this.AltDefaultUnitDict[unitCultureId];
            return (from IUnit u in this.Units where u.Id == id select u).First();
        }
        public IUnit DefaultUnit {
            get
            {
                return this.GetDefaultUnit(App.VM.UnitCulture);
            }
        }
        public IUnit AltDefaultUnit
        {
            get
            {
                return this.GetAltDefaultUnit(App.VM.UnitCulture);
            }
        }
        public List<IUnit> Units { get; set; }
        public string FormattedValue
        {
            get
            {
                if (this.DefaultUnit == null) return App.VM.SelectedMeasurement.Value;
                return this.DefaultUnit.FormattedValue;
            }
        }
    }
}
