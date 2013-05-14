using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public static class RequiredMeasurements
    {
        public static List<MeasurementId> Bra = new List<MeasurementId>()
        {
            MeasurementId.Chest,
            MeasurementId.UnderBust,
        };

        public static List<MeasurementId> DressSize = new List<MeasurementId>()
        {
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Hips,
        };

        public static List<MeasurementId> Hat = new List<MeasurementId>()
        {
            MeasurementId.Head,
        };

        public static List<MeasurementId> Hosiery = new List<MeasurementId>()
        {
            MeasurementId.Hips,
            MeasurementId.Waist,
            MeasurementId.InsideLeg,
        };

        public static List<MeasurementId> ShirtMens = new List<MeasurementId>()
        {
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            MeasurementId.Sleeve,
        };

        public static List<MeasurementId> ShirtWomens = new List<MeasurementId>()
        {
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            MeasurementId.Sleeve,
            MeasurementId.Hips,
        };

        public static List<MeasurementId> Shoes = new List<MeasurementId>()
        {
            MeasurementId.FootLength,
            MeasurementId.FootWidth,
        };

        public static List<MeasurementId> SkiBoots = new List<MeasurementId>()
        {
            MeasurementId.FootLength,
            MeasurementId.FootWidth,
        };

        public static List<MeasurementId> SuitMens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.Shoulder,
            MeasurementId.Sleeve,
            MeasurementId.Chest,
            MeasurementId.InsideLeg,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            
        };

        public static List<MeasurementId> SuitWomens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.Shoulder,
            MeasurementId.Sleeve,
            MeasurementId.Chest,
            MeasurementId.InsideLeg,
            MeasurementId.Neck,
            MeasurementId.TorsoLength,
            MeasurementId.Hips,
        };

        public static List<MeasurementId> TrousersMens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.InsideLeg,
        };

        public static List<MeasurementId> TrousersWomens = new List<MeasurementId>()
        {
            MeasurementId.Waist,
            MeasurementId.InsideLeg,
            MeasurementId.Hips,
        };

        public static List<MeasurementId> WetsuitMens = new List<MeasurementId>()
        {
            MeasurementId.Height,
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Weight,
        };
        public static List<MeasurementId> WetsuitWomens = new List<MeasurementId>()
        {
            MeasurementId.Height,
            MeasurementId.Chest,
            MeasurementId.Waist,
            MeasurementId.Weight,
            MeasurementId.Hips,
        };



    }
}
