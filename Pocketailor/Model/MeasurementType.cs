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
        public Dictionary<UnitCultureId, UnitId> DefaultUnitDict;
        public IUnit DefaultUnit {
            get
            {
                UnitId id = this.DefaultUnitDict[App.VM.UnitCulture];
                return (from IUnit u in this.Units where u.Id == id select u).First();
            }
        }
        public List<IUnit> Units { get; set; }
        public string FormattedValue
        {
            get
            {
                if (this.DefaultUnit == null) return App.VM.SelectedStat.Value;
                return this.DefaultUnit.FormattedValue;
            }
        }
    }
}
