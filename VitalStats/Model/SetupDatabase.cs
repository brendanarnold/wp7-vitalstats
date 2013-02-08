using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;

namespace VitalStats.Model
{
    public class SetupDatabase
    {

        public static void InitialiseDB(AppDataContext db)
        {
            db.CreateDatabase();
            ResetMeasurementTypes(db);
            db.SubmitChanges();
            ResetStatTemplates(db);
            db.SubmitChanges();
        }

        public static void EmptyDB(AppDataContext db)
        {
            // The order here is important so as to not leave hanging references
            db.Units.DeleteAllOnSubmit(db.Units);
            db.Stats.DeleteAllOnSubmit(db.Stats);
            db.MeasurementTypes.DeleteAllOnSubmit(db.MeasurementTypes);
            db.Profiles.DeleteAllOnSubmit(db.Profiles);
            db.SubmitChanges();
        }

        // This has to be run when measurement types are already in the db
        public static void ResetStatTemplates(AppDataContext db)
        {
           // Maybe should get this from XML or something
           List<Stat> sts = new List<Stat>() {
                new Stat() {
                    Name = "Shoe Size",
                    Value = null,
                    PreferredUnit = null,
                    MeasurementType = (from MeasurementType mt in db.MeasurementTypes 
                                       where mt.Name == "Length" select mt).First(),
                },
                new Stat() {
                    Name = "Weight",
                    Value = null,
                    PreferredUnit = null,
                    MeasurementType = (from MeasurementType mt in db.MeasurementTypes 
                                       where mt.Name == "Weight" select mt).First(),
                },
                new Stat() {
                    Name = "Height",
                    Value = null,
                    PreferredUnit = null,
                    MeasurementType = (from MeasurementType mt in db.MeasurementTypes 
                                       where mt.Name == "Length" select mt).First(),
                },
                new Stat() {
                    Name = "Tennis grip",
                    Value = null,
                    PreferredUnit = null,
                    MeasurementType = (from MeasurementType mt in db.MeasurementTypes 
                                       where mt.Name == "Length" select mt).First(),
                },
                new Stat() {
                    Name = "Collar Size",
                    Value = null,
                    PreferredUnit = null,
                    MeasurementType = (from MeasurementType mt in db.MeasurementTypes 
                                       where mt.Name == "Length" select mt).First(),
                },
            };

            //db.StatTemplates.DeleteAllOnSubmit(db.StatTemplates);
            db.StatTemplates.InsertAllOnSubmit(sts);
            
        }


        public static void ResetMeasurementTypes(AppDataContext db)
        {
            List<MeasurementType> mts = new List<MeasurementType>()
            {
                new MeasurementType() {
                    Name = "Length",
                    Units = { 
                        new Unit() {
                            NameString = "Centimetres",
                            ConversionFactorString = "100.0",
                            ConversionInterceptString = "0.0",
                            Format = "{0:f1} cm.",
                        },
                        new Unit() {
                            NameString = "Inches",
                            ConversionFactorString = "39.3701",
                            ConversionInterceptString = "0.0",
                            Format = "{0:F1} in.",
                        },
                        new Unit() {
                            NameString = "Hands",
                            ConversionFactorString = "9.84252",
                            ConversionInterceptString = "0.0",
                            Format = "{0:f1} hands",
                        },
                        new Unit {
                            NameString = "Feet|Inches",
                            ConversionFactorString = "3.2808|39.3701",
                            ConversionInterceptString = "0.0|0.0",
                            Format = "{0:f0} ft. {1:f0} in.",
                        },
                        new Unit {
                            NameString = "Feet",
                            ConversionFactorString = "3.2808",
                            ConversionInterceptString = "0.0",
                            Format = "{0:f1} ft.",
                        },
                        new Unit() {
                            NameString = "Metres",
                            ConversionFactorString = "1.0",
                            ConversionInterceptString = "0.0",
                            Format = "{0:f2} m.",
                        }
                    },                                 
                },

                new MeasurementType() {
                    Name = "Weight",
                    Units = {
                        new Unit() {
                            NameString = "Ounces",
                            ConversionFactorString = "35.274",
                            ConversionInterceptString = "0.0",
                            Format = "{0:f0} oz.",
                        },
                        new Unit() {
                            NameString = "Pounds|Ounces",
                            ConversionFactorString = "2.20462|35.274",
                            ConversionInterceptString = "0.0|0.0",
                            Format = "{0:f0} lbs. {1:f0} oz.",
                        },
                        new Unit() {
                            NameString = "Kilograms",
                            ConversionFactorString = "1.0",
                            ConversionInterceptString = "0.0",
                            Format = "{0:f1} kg.",
                        }, 
                        new Unit() {
                            NameString = "Stones",
                            ConversionFactorString = "0.157473",
                            ConversionInterceptString = "0.0",
                            Format = "{0:f1} st.",
                        },
                    },
                },


            };

            db.MeasurementTypes.DeleteAllOnSubmit(db.MeasurementTypes);
            db.MeasurementTypes.InsertAllOnSubmit(mts);
        }
    }
}
