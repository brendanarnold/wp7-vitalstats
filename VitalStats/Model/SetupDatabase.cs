﻿using System;
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
            ResetUnits(db);
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

        // This has to be run when measurementtypes are already in the db
        public static void ResetUnits(AppDataContext db)
        {
            MeasurementType lengthMt = (from MeasurementType mt in db.MeasurementTypes 
                                        where mt.Name == "Length" select mt).First(); 
            MeasurementType weightMt = (from MeasurementType mt in db.MeasurementTypes 
                                        where mt.Name == "Weight" select mt).First();

            lengthMt.Units.AddRange(
             new List<Unit>() { 
                new Unit() {
                    Name = "Centimetres",
                    ConversionFactor = "100.0",
                    ConversionIntercept = "0.0",
                    Format = "{0} cm.",
                },
                new Unit() {
                    Name = "Inches",
                    ConversionFactor = "39.3701",
                    ConversionIntercept = "0.0",
                    Format = "{0} in.",
                },
                new Unit() {
                    Name = "Hands",
                    ConversionFactor = "9.84252",
                    ConversionIntercept = "0.0",
                    Format = "{0} hands",
                },
                new Unit {
                    Name = "Feet/Inches",
                    ConversionFactor = "3.2808|39.3701",
                    ConversionIntercept = "0.0|0.0",
                    Format = "{0} ft. {1} in.",
                },
                new Unit {
                    Name = "Feet",
                    ConversionFactor = "3.2808",
                    ConversionIntercept = "0.0",
                    Format = "{0} ft.",
                },
                new Unit() {
                    Name = "Metres",
                    ConversionFactor = "1.0",
                    ConversionIntercept = "0.0",
                    Format = "{0} m.",
                }
             });

            weightMt.Units.Assign(
                new EntitySet<Unit>() {
                new Unit() {
                    Name = "Ounces",
                    ConversionFactor = "35.274",
                    ConversionIntercept = "0.0",
                    Format = "{0} oz.",
                },
                new Unit() {
                    Name = "Pounds/Ounces",
                    ConversionFactor = "2.20462|35.274",
                    ConversionIntercept = "0.0|0.0",
                    Format = "{0} lbs. {1} oz.",
                },
                new Unit() {
                    Name = "Kilograms",
                    ConversionFactor = "1.0",
                    ConversionIntercept = "0.0",
                    Format = "{0} kg.",
                }, 
                new Unit() {
                    Name = "Stones",
                    ConversionFactor = "0.157473",
                    ConversionIntercept = "0.0",
                    Format = "{0} st.",
                },
            });


        }


        public static void ResetMeasurementTypes(AppDataContext db)
        {
            List<MeasurementType> mts = new List<MeasurementType>()
            {
                new MeasurementType() {
                    Name = "Length",
                    Units = null,                                 
                },

                new MeasurementType() {
                    Name = "Weight",
                    Units = null,
                },

                new MeasurementType() {
                    Name = "Other",
                    Units = null,
                }

            };

            db.MeasurementTypes.DeleteAllOnSubmit(db.MeasurementTypes);
            db.MeasurementTypes.InsertAllOnSubmit(mts);
        }
    }
}
