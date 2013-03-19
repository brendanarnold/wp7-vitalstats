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
        public UnitId DefaultUnitId { get; set; }
        public IUnit DefaultUnit {
            get
            {
                return (from IUnit u in this.Units where u.Id == this.DefaultUnitId select u).First();
            }
            set
            {
                this.DefaultUnitId = value.Id;
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
