using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Required Values are listed in the respective conversions files

namespace Pocketailor.Model
{
    public partial class Static
    {
        static public List<MeasurementTemplate> MeasurementTemplates = new List<MeasurementTemplate>() 
        {
            new MeasurementTemplate() { Id = MeasurementId.Chest, _measurementTypeId = MeasurementTypeId.Length},
            //new MeasurementTemplate() { Id = MeasurementId.Crotch, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.FootLength, _measurementTypeId = MeasurementTypeId.Length },
            new MeasurementTemplate() { Id = MeasurementId.FootWidth, _measurementTypeId = MeasurementTypeId.Length },
            new MeasurementTemplate() { Id = MeasurementId.Height, _measurementTypeId = MeasurementTypeId.Length },
            new MeasurementTemplate() { Id = MeasurementId.Hips, _measurementTypeId = MeasurementTypeId.Length },
            new MeasurementTemplate() { Id = MeasurementId.InsideLeg, _measurementTypeId = MeasurementTypeId.Length },
            new MeasurementTemplate() { Id = MeasurementId.Neck, _measurementTypeId = MeasurementTypeId.Length},
            //new MeasurementTemplate() { Id = MeasurementId.OutsideLeg, _measurementTypeId = MeasurementTypeId.Length},
            //new MeasurementTemplate() { Id = MeasurementId.OverBust, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.Shoulder, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.Sleeve, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.UnderBust, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.Waist, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.Weight, _measurementTypeId = MeasurementTypeId.Weight},
            //new MeasurementTemplate() { Id = MeasurementId.Wrist, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.Head, _measurementTypeId = MeasurementTypeId.Length },
            //new MeasurementTemplate() { Id = MeasurementId.TennisGrip, _measurementTypeId = MeasurementTypeId.Length},
            new MeasurementTemplate() { Id = MeasurementId.TorsoLength, _measurementTypeId = MeasurementTypeId.Length},
        };

    }

    
    public class MeasurementTemplate
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
