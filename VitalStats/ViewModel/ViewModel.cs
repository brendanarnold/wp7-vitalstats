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
using System.Collections.ObjectModel;
using VitalStats.Model;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VitalStats.ViewModelNamespace
{
    [DataContract]
    public class ViewModel
    {
        [DataMember]
        public ObservableCollection<Profile> Profiles { get; set; }

        [IgnoreDataMember]
        public ObservableCollection<MeasurementType> StockMeasurementTypes { get; set; }

        [DataMember]
        public ObservableCollection<Stat> StockStats { get; set; }

        public void GetStockStats()
        {
            // In future load this from XML or something
            
            this.StockStats = new ObservableCollection<Stat>() 
            {
                new Stat() {
                    Name = "Shoe Size",
                    Value = null,
                    PreferredUnit = "Centimetre",
                    MeasurementType = this.GetMeasurementType("Shoe Size (Toe-to-Heel)"),
                },
                new Stat() {
                    Name = "Weight",
                    Value = null,
                    PreferredUnit = "Kilogram",
                    MeasurementType = this.GetMeasurementType("Weight"),
                },
                new Stat() {
                    Name = "Height",
                    Value = null,
                    PreferredUnit = "Metre",
                    MeasurementType = this.GetMeasurementType("Length"),
                },
                new Stat() {
                    Name = "Tennis grip",
                    Value = null,
                    PreferredUnit = "Inch",
                    MeasurementType = this.GetMeasurementType("Length"),
                },
                new Stat() {
                    Name = "Collar Size",
                    Value = null,
                    PreferredUnit = "Metre",
                    MeasurementType = this.GetMeasurementType("Length"),
                },
            };
        }

        public MeasurementType GetMeasurementType(string name)
        {
            return this.StockMeasurementTypes.FirstOrDefault(mt => mt.Name.Contains(name));
        }

        public void GetMeasurementTypes()
        {
            // In future load this from XML or somesuch

            this.StockMeasurementTypes = new ObservableCollection<MeasurementType>() 
            {
                new MeasurementType() {
                    Name = "Length",
                    BaseUnit = "Metre",
                    IsConvertible = true,
                    UnitNames = new List<string>() {
                        "Metre", "Centimetre", "Feet/Inches", "Inch"
                    },
                    UnitFormats = new List<string>() {
                        "{0} m.", "{0} cm.", "{0} ft. {1} in.", "{0} in."
                    },
                    ConversionFactors = new List<string>() {
                        "1.0", "100.0", "3.2808|39.3701", "39.3701"
                    },
                    ConversionIntercepts = new List<string>() {
                        "0.0", "0.0", "0.0", "0.0"
                    }
                },
                new MeasurementType() {
                    Name = "Weight",
                    BaseUnit = "Kilogram",
                    IsConvertible = true,
                    UnitNames = new List<string>() {
                        "Kilogram", "Pound", "Stone"
                    },
                    UnitFormats = new List<string>() {
                        "{0} kg.", "{0} lbs.", "{0} st."
                    },
                    ConversionFactors = new List<string>() {
                        "1.0", "0.4536", "6.3503"
                    },
                    ConversionIntercepts = new List<string>() {
                        "0.0", "0.0", "0.0"
                    }
                },
                new MeasurementType() {
                    Name = "Shoe Size (Toe-to-Heel)",
                    BaseUnit = "Centimetre",
                    IsConvertible = true,
                    UnitNames = new List<string>() {
                        "Centimetres", "UK Mens", "UK Womens", "UK Girls", "UK Boys"
                    },
                    UnitFormats = new List<string>() {
                        "{0} cm.", "{0}", "{0}"
                    },
                    ConversionFactors = new List<string>() {
                        "1.0", "0.4536", "6.3503"
                    },
                    ConversionIntercepts = new List<string>() {
                        "0.0", "0.0", "0.0"
                    }
                },
            };
        }
        
        public void GetProfiles()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Count > 0)
            {
                this.GetSavedProfiles();
            }
            else
            {
                this.Profiles = new ObservableCollection<Profile>();
            }
        }
    
    

        public void GetSavedProfiles()
        {
            ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();
            foreach (Object o in IsolatedStorageSettings.ApplicationSettings.Values)
            {
                profiles.Add((Profile)o);
            }
            this.Profiles = profiles;
        }

        public void SaveProfiles()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            foreach (Profile p in this.Profiles)
            {
                string pKey = "Profile:" + p.Id;
                if (settings.Contains(pKey))
                {
                    settings[pKey] = p.GetCopy();
                }
                else
                {
                    settings.Add(pKey, p.GetCopy());
                }
            }
            settings.Save();
        }


        internal void AddNewProfile(string name, bool isProtected)
        {
            this.Profiles.Add(new Profile() { Name = name, IsProtected = isProtected });
        }

        internal void DeleteProfile(Profile profile)
        {
            this.Profiles.Remove(profile);
        }
    }
}
