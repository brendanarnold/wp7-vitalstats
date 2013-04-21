using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public partial class Static
    {
        static public List<MeasurementType> MeasurementTypes = new List<MeasurementType>()
        {
            // Define the Length measurement type
            new MeasurementType() 
            {
                Name = "Length",
                Id = MeasurementTypeId.Length,
                DefaultUnitDict = new Dictionary<UnitCultureId,UnitId>() { 
                    {UnitCultureId.Metric, UnitId.Centimetre},
                    {UnitCultureId.Imperial, UnitId.Inch} 
                },
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
                DefaultUnitDict = new Dictionary<UnitCultureId,UnitId>() {
                    {UnitCultureId.Metric, UnitId.Kilogram},
                    {UnitCultureId.Imperial, UnitId.Pound},
                },
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
}
