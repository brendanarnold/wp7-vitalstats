using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel
    {
        #region ConversionsPage methods/properties

        private ConversionId _selectedConversionType;
        public ConversionId SelectedConversionType
        {
            get
            {
                return this._selectedConversionType;
            }
            set
            {
                if (this._selectedConversionType != value)
                {
                    this._selectedConversionType = value;
                    this.NotifyPropertyChanged("SelectedConversionType");
                }
            }
        }

        private string _conversionsByRegionPageTitle;
        public string ConversionsByRegionPageTitle
        {
            get
            {
                return this._conversionsByRegionPageTitle;
            }
            set
            {
                if (this._conversionsByRegionPageTitle != value)
                {
                    this._conversionsByRegionPageTitle = value;
                    this.NotifyPropertyChanged("ConversionsByRegionPageTitle");
                }
            }
        }

        private BitmapImage _conversionsByRegionPageBGImage;
        public BitmapImage ConversionsByRegionPageBGImage
        {
            get
            {
                return this._conversionsByRegionPageBGImage;
            }
            set
            {
                if (this._conversionsByRegionPageBGImage != value)
                {
                    this._conversionsByRegionPageBGImage = value;
                    this.NotifyPropertyChanged("ConversionsByRegionPageBGImage");
                }

            }
        }

        private ObservableCollection<ConversionRegion> _conversionByRegion;
        public ObservableCollection<ConversionRegion> ConversionsByRegion
        {
            get { return this._conversionByRegion; }
            set
            {
                if (this._conversionByRegion != value)
                {
                    this._conversionByRegion = value;
                    this.NotifyPropertyChanged("ConversionsByRegion");
                }
            }
        }

        public void LoadConversionsPageData()
        {
            // Reset the list
            this.ConversionsByRegion = new ObservableCollection<ConversionRegion>();
            // Declare vars in the top scope
            List<double> measuredVals = new List<double>();
            // TODO: remove this hack needed to compile
            IEnumerable<Model.Conversions.IConversionData> dataQuery = appDB.DressSizes.Cast<Model.Conversions.IConversionData>(); ;
            // Make sure we have region data
            if (this.GetSelectedRegions() == null) return;

            // Get the conversion specific data
            switch (this.SelectedConversionType)
            {
                case ConversionId.TrouserSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.TrousersUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.TrousersUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Trousers.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.ShirtSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShirtUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShirtUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Shirts.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.HatSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.HatUtils.RequiredMeasurements);
                    dataQuery = appDB.Hats.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.SuitSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SuitUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SuitUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Suits.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.DressSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.DressSizeUtils.RequiredMeasurements);
                    dataQuery = appDB.DressSizes.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.BraSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.BraUtils.RequiredMeasurements);
                    dataQuery = appDB.Bras.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.HosierySize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.HosieryUtils.RequiredMeasurements);
                    dataQuery = appDB.Hosiery.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.ShoeSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShoesUtils.RequiredMeasurements);
                    dataQuery = appDB.Shoes.Cast<Model.Conversions.IConversionData>();
                    break;
                case ConversionId.SkiBootSize:
                    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SkiBootsUtils.RequiredMeasurements);
                    dataQuery = appDB.SkiBoots.Cast<Model.Conversions.IConversionData>();
                    break;
                //case ConversionId.TennisGripSize:
                //    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.TennisRaquetSizesUtils.RequiredMeasurements);
                //    dataQuery = appDB.TennisRaquetSizes.Cast<Model.Conversions.IConversionData>();
                //    break;
                case ConversionId.WetsuitSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.WetsuitUtils.RequiredMeasurementsMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.WetsuitUtils.RequiredMeasurementsWomens);
                    }
                    dataQuery = appDB.Wetsuits.Cast<Model.Conversions.IConversionData>();
                    break;
                default:
                    return;
            }
            // Check we have all the necessary measurements
            if (measuredVals == null) return;
            // Build up by regions
            foreach (RegionIds region in this.GetSelectedRegions())
            {
                ConversionRegion cr = new ConversionRegion();
                cr.Name = Lookup.Regions[region];
                cr.Conversions = new ObservableCollection<NameValuePair>();
                // TODO: If gender not specified, then return Female measurements
                Gender qGender = (this.SelectedProfile.Gender == Gender.Unspecified) ? Gender.Female : this.SelectedProfile.Gender;
                // TODO: Unresolve this list used for debuggin purposes
                var dataByRegion = dataQuery.Where(ds => (ds.Region == region) && (ds.Gender == qGender)).ToList();
                foreach (RetailId retailId in dataByRegion.Select(ds => ds.Retailer).Distinct())
                {
                    var conversionData = dataByRegion.Where(ds => ds.Retailer == retailId);
                    double lowestChisq = double.MaxValue;
                    Model.Conversions.IConversionData bestFit = null;
                    foreach (Model.Conversions.IConversionData candidateConversion in conversionData)
                    {
                        double chiSq = candidateConversion.GetChiSq(measuredVals);
                        if (chiSq < lowestChisq)
                        {
                            lowestChisq = chiSq;
                            bestFit = candidateConversion;
                        }
                    }
                    cr.Conversions.Add(new NameValuePair()
                    {
                        // TODO: Will need to include some kind of lookup for actual string
                        FormattedValue = bestFit.FormattedValue,
                        Retailer = retailId,
                    });
                }
                this.ConversionsByRegion.Add(cr);
            }
        }


        // Already been checked that these values have been taken
        public List<double> GetRequiredMeasuredValues(List<MeasurementId> requiredIds)
        {
            List<double> requiredValues = new List<double>();
            foreach (MeasurementId mID in requiredIds)
            {
                Stat s = App.VM.SelectedProfile.Stats.FirstOrDefault(x => x.MeasurementId == mID);
                if (s == null) return null;
                double d;
                if (double.TryParse(s.Value, out d))
                {
                    requiredValues.Add(d);
                }
                else
                {
                    return null;
                }
            }
            return requiredValues;
        }

        internal List<MeasurementId> GetMissingMeasurements(List<MeasurementId> requiredIds)
        {
            return requiredIds.Except(this.SelectedProfile.Stats.Select(s => s.MeasurementId)).ToList();
        }

        public class ConversionRegion
        {
            public string Name { get; set; }
            public ObservableCollection<NameValuePair> Conversions { get; set; }
        }

        public class NameValuePair : INotifyPropertyChanged
        {

            public NameValuePair()
            {
                App.VM.HiddenRetailers.CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null)
                    {
                        if (e.NewItems.Contains(this.Retailer))
                        {
                            this.NotifyPropertyChanged("IsVisible");
                            this.NotifyPropertyChanged("IsHidden");
                        }
                    }
                    if (e.OldItems != null)
                    {
                        if (e.OldItems.Contains(this.Retailer))
                        {
                            this.NotifyPropertyChanged("IsVisible");
                            this.NotifyPropertyChanged("IsHidden");
                        }
                    }
                };

                App.VM.PropertyChanged += (s, e) =>
                {
                    if ((e.PropertyName == "ShowHiddenConversions")
                        || (e.PropertyName == "HiddenRetailers"))
                        this.NotifyPropertyChanged("IsVisible");
                };
            }

            public string Name {
                get { return Lookup.Retail[this.Retailer]; }
            }
            public string FormattedValue { get; set; }
            public RetailId Retailer { get; set; }
            // Flag set to true if user has chosen to hide this retailer e.g. show but grey this entry out
            public bool IsHidden
            {
                get
                {
                    return App.VM.HiddenRetailers.Contains(this.Retailer);
                }
            }
            // Flag set to true is user wants to see all retailers (even hidden) e.g. show this retailer at all
            public bool IsVisible
            {
                get
                {
                    if (!this.IsHidden)
                    {
                        return true;
                    }
                    else
                    {
                        return App.VM.ShowHiddenConversions;
                    }
                }
            }

            public void ToggleHidden()
            {
                if (this.IsHidden)
                {
                    App.VM.HiddenRetailers.Remove(this.Retailer);
                }
                else
                {
                    if (!App.VM.HiddenRetailers.Contains(this.Retailer))
                        App.VM.HiddenRetailers.Add(this.Retailer);
                }
                App.VM.SaveHiddenRetailers();
            }

            #region INotifyPropertyChanged members

            public event PropertyChangedEventHandler PropertyChanged;

            internal void NotifyPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion

        }

        private ObservableCollection<RetailId> _hiddenRetailers;
        public ObservableCollection<RetailId> HiddenRetailers
        {
            get
            {
                if (this._hiddenRetailers == null)
                    this.LoadHiddenRetailers();
                return this._hiddenRetailers;
            }
            set
            {
                if (this._hiddenRetailers != value)
                {
                    this._hiddenRetailers = value;
                    this.NotifyPropertyChanged("HiddenRetailers");
                }
            }
        }

        public void LoadHiddenRetailers()
        {
            this.HiddenRetailers = App.Settings.GetValueOrDefault<ObservableCollection<RetailId>>("HiddenRetailers", new ObservableCollection<RetailId>());
        }

        public void SaveHiddenRetailers()
        {
            App.Settings.AddOrUpdateValue("HiddenRetailers", this.HiddenRetailers);
            App.Settings.Save();
        }

        private bool _showHiddenConversions = false;
        public bool ShowHiddenConversions
        {
            get { return this._showHiddenConversions; }
            set
            {
                if (this._showHiddenConversions != value)
                {
                    this._showHiddenConversions = value;
                    this.NotifyPropertyChanged("ShowHiddenConversions");
                }
            }
        }


        #endregion

    }
}
