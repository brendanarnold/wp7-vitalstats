using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Required Values are listed in the respective conversions files

namespace Pocketailor.Model
{
    public partial class Static
    {
        static public List<StatTemplate> MeasurementTemplates = new List<StatTemplate>() 
        {
            new StatTemplate() { Id = MeasurementId.Chest, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Crotch, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.FootLength, _measurementTypeId = MeasurementTypeId.Length },
            new StatTemplate() { Id = MeasurementId.FootWidth, _measurementTypeId = MeasurementTypeId.Length },
            new StatTemplate() { Id = MeasurementId.Height, _measurementTypeId = MeasurementTypeId.Length },
            new StatTemplate() { Id = MeasurementId.Hips, _measurementTypeId = MeasurementTypeId.Length },
            new StatTemplate() { Id = MeasurementId.InsideLeg, _measurementTypeId = MeasurementTypeId.Length },
            new StatTemplate() { Id = MeasurementId.Neck, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.OutsideLeg, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Overbust, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Shoulder, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Sleeve, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Underbust, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Waist, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Weight, _measurementTypeId = MeasurementTypeId.Weight},
            new StatTemplate() { Id = MeasurementId.Wrist, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.Head, _measurementTypeId = MeasurementTypeId.Length },
            new StatTemplate() { Id = MeasurementId.TennisGrip, _measurementTypeId = MeasurementTypeId.Length},
            new StatTemplate() { Id = MeasurementId.TorsoLength, _measurementTypeId = MeasurementTypeId.Length},
        };

    }

    
    public class StatTemplate
    {
        public MeasurementId Id { get; set; }
        public string Name {
            get { return Lookup.Measurements[this.Id]; }
        }
        public MeasurementTypeId? _measurementTypeId { get; set; }
        public MeasurementType MeasurementType
        {
            get 
            {
                if (this._measurementTypeId == null) return null;
                return (from MeasurementType mt in App.VM.MeasurementTypes where mt.Id == this._measurementTypeId select mt).First(); 
            }
        }
    }


}
