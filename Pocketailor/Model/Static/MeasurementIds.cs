using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public enum MeasurementId
    {
        Height = 0,
        Weight = 1,
        Waist = 2,
        Chest = 3,
        Hips = 4,
        InsideLeg = 5,
        OutsideLeg = 6,
        Sleeve = 7,
        Wrist = 8,
        Shoulder = 9,
        Neck = 10,
        OverBust = 11,
        UnderBust = 12,
        FootLength = 13,
        FootWidth = 14,
        Crotch = 15,
        Head = 16,
        TennisGrip = 17,
        TorsoLength = 18,
    }

    public static partial class Lookup
    {
        public static Dictionary<MeasurementId, string> Measurements = new Dictionary<MeasurementId, string>()
        {
           { MeasurementId.Height, "Height"},
           { MeasurementId.Weight, "Weight"},
           { MeasurementId.Waist, "Waist"},
           { MeasurementId.Chest, "Chest"},
           { MeasurementId.Hips, "Hips"},
           { MeasurementId.InsideLeg, "Inside Leg"},
           { MeasurementId.OutsideLeg, "Outside Leg"},
           { MeasurementId.Sleeve, "Sleeve"},
           { MeasurementId.Wrist, "Wrist"},
           { MeasurementId.Shoulder, "Shoulder"},
           { MeasurementId.Neck, "Neck"},
           { MeasurementId.OverBust, "Overbust"},
           { MeasurementId.UnderBust, "Underbust"},
           { MeasurementId.FootLength, "Foot Length"},
           { MeasurementId.FootWidth, "Foot Width"},
           { MeasurementId.Crotch, "Crotch"},
           { MeasurementId.Head, "Head"},
           { MeasurementId.TennisGrip, "Tennis Grip"},
           { MeasurementId.TorsoLength, "Torso Length"},
        };
    }


}
