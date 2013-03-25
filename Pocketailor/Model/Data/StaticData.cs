using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public class Static
    {
        static public List<StatTemplate> StatTemplates = new List<StatTemplate>() 
        {
            new StatTemplate() { Id = MeasurementId.Chest, _measurementTypeId = MeasurementTypeId.Length, Name = "Chest" },
            new StatTemplate() { Id = MeasurementId.Crotch, _measurementTypeId = MeasurementTypeId.Length, Name = "Crotch" },
            new StatTemplate() { Id = MeasurementId.FootLength, _measurementTypeId = MeasurementTypeId.Length, Name = "Foot Length" },
            new StatTemplate() { Id = MeasurementId.FootWidth, _measurementTypeId = MeasurementTypeId.Length, Name = "Foot Width" },
            new StatTemplate() { Id = MeasurementId.Height, _measurementTypeId = MeasurementTypeId.Length, Name = "Height" },
            new StatTemplate() { Id = MeasurementId.Hips, _measurementTypeId = MeasurementTypeId.Length, Name = "Hips" },
            new StatTemplate() { Id = MeasurementId.InsideLeg, _measurementTypeId = MeasurementTypeId.Length, Name = "Inside Leg" },
            new StatTemplate() { Id = MeasurementId.Neck, _measurementTypeId = MeasurementTypeId.Length, Name = "Neck" },
            new StatTemplate() { Id = MeasurementId.OutsideLeg, _measurementTypeId = MeasurementTypeId.Length, Name = "Outside Leg" },
            new StatTemplate() { Id = MeasurementId.Overbust, _measurementTypeId = MeasurementTypeId.Length, Name = "Overbust" },
            new StatTemplate() { Id = MeasurementId.Shoulder, _measurementTypeId = MeasurementTypeId.Length, Name = "Shoulder" },
            new StatTemplate() { Id = MeasurementId.Sleeve, _measurementTypeId = MeasurementTypeId.Length, Name = "Sleeve" },
            new StatTemplate() { Id = MeasurementId.Underbust, _measurementTypeId = MeasurementTypeId.Length, Name = "Underbust" },
            new StatTemplate() { Id = MeasurementId.Waist, _measurementTypeId = MeasurementTypeId.Length, Name = "Waist" },
            new StatTemplate() { Id = MeasurementId.Weight, _measurementTypeId = MeasurementTypeId.Weight, Name = "Weight" },
            new StatTemplate() { Id = MeasurementId.Wrist, _measurementTypeId = MeasurementTypeId.Length, Name = "Wrist" },
            new StatTemplate() { Id = MeasurementId.Head, _measurementTypeId = MeasurementTypeId.Length, Name = "Head" },
            new StatTemplate() { Id = MeasurementId.TennisGrip, _measurementTypeId = MeasurementTypeId.Length, Name = "Tennis Grip" },
        };

        static public List<MeasurementType> MeasurementTypes = new List<MeasurementType>()
        {
            // Define the Length measurement type
            new MeasurementType() 
            {
                Name = "Length",
                Id = MeasurementTypeId.Length,
                DefaultUnitId = UnitId.Centimetre,
                Units = new List<IUnit>() {
                    new SingleValueUnit() { 
                        _id = UnitId.Metre,
                        _name = "Metres", 
                        _conversionFactor = 1.0,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "m",
                        _valueFormat = "{0:f2}",
                    },
                    new SingleValueUnit() {
                        _id = UnitId.Centimetre,
                        _name = "Centimetres",
                        _conversionFactor = 100.0,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "cm",
                        _valueFormat = "{0:f1}",
                    },
                    new SingleValueUnit() {
                        _id = UnitId.Feet,
                        _name = "Feet",
                        _conversionFactor = 3.2808,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "ft",
                        _valueFormat = "{0:f1}",
                    },
                    new SingleValueUnit() {
                        _id = UnitId.Inch,
                        _name = "Inches",
                        _conversionFactor = 39.3701,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "in",
                        _valueFormat = "{0:f1}",
                    },
                    new DoubleValueUnit() {
                        _id = UnitId.FeetInch,
                        _names = new List<string>() { "Feet", "Inches" },
                        _conversionFactors = new List<double>() { 3.2808, 39.3701 },
                        _conversionIntercepts = new List<double>() { 0.0, 0.0 },
                        _shortUnitNames = new List<string>() { "ft", "in" },
                        _valueFormats = new List<string>() { "{0:f0}", "{0:f0}" },
                    },
                },
            },
            // Define the weight measurement type
            new MeasurementType() 
            {
                Name = "Weight",
                Id = MeasurementTypeId.Weight,
                DefaultUnitId = UnitId.Kilogram,
                Units = new List<IUnit>() {
                    new SingleValueUnit() {
                        _id = UnitId.Kilogram,
                        _name = "Kilograms",
                        _conversionFactor = 1.0,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "kg",
                        _valueFormat = "{0:f1}",

                    },
                    new SingleValueUnit() {
                        _id = UnitId.Stone,
                        _name = "Stones",
                        _conversionFactor = 0.157473,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "st",
                        _valueFormat = "{0:f1}",
                    },
                    new SingleValueUnit() {
                        _id = UnitId.Pound,
                        _name = "Pounds",
                        _conversionFactor = 2.20462,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "lb",
                        _valueFormat = "{0:f1}",
                    },
                    new SingleValueUnit() {
                        _id = UnitId.Ounce,
                        _name = "Ounces",
                        _conversionFactor = 35.274,
                        _conversionIntercept = 0.0,
                        _shortUnitName = "oz",
                        _valueFormat = "{0:f1}",
                    },
                    new DoubleValueUnit() {
                        _id = UnitId.PoundOunce,
                        _names = new List<string>() { "Pounds", "Ounces" },
                        _conversionFactors = new List<double>() { 2.20462, 35.274 },
                        _conversionIntercepts = new List<double>() { 0.0, 0.0 },
                        _shortUnitNames = new List<string>() { "lb", "oz" },
                        _valueFormats = new List<string>() { "{0:f0}", "{0:f0}" },
                    },
                },
            },

        };

        
    }

    

    public class StatTemplate
    {
        public MeasurementId Id { get; set; }
        public string Name {get; set;}
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
