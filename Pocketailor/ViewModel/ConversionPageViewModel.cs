using Pocketailor.Model;
using Pocketailor.Model.Conversions;
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

        private string _conversionsPageTitle;
        public string ConversionsPageTitle
        {
            get
            {
                return this._conversionsPageTitle;
            }
            set
            {
                if (this._conversionsPageTitle != value)
                {
                    this._conversionsPageTitle = value;
                    this.NotifyPropertyChanged("ConversionsByRegionPageTitle");
                }
            }
        }

        private BitmapImage _conversionsPageBGImage;
        public BitmapImage ConversionsPageBGImage
        {
            get
            {
                return this._conversionsPageBGImage;
            }
            set
            {
                if (this._conversionsPageBGImage != value)
                {
                    this._conversionsPageBGImage = value;
                    this.NotifyPropertyChanged("ConversionsByRegionPageBGImage");
                }

            }
        }

        private ObservableCollection<LongListSelectorGroup<ConversionData>> _groupedConversions;
        public ObservableCollection<LongListSelectorGroup<ConversionData>> GroupedConversions
        {
            get { return this._groupedConversions; }
            set
            {
                if (this._groupedConversions != value)
                {
                    this._groupedConversions = value;
                    this.NotifyPropertyChanged("ConversionsByRegion");
                }
            }
        }

        public void LoadConversionsPageData()
        {
            // Declare vars in the top scope
            Dictionary<MeasurementId, double> measuredVals = new Dictionary<MeasurementId, double>();
            // Make sure we have region data
            if (this.GetSelectedRegions() == null) return;
            // TODO: If gender not specified, then return Female measurements. Note only perform gener query on tables that have 
            // Gender fields (even after casting) because it still generate SQL to query gender
            Gender qGender = (this.SelectedProfile.Gender == Gender.Unspecified) ? Gender.Female : this.SelectedProfile.Gender; 

            // Get the conversion specific data
            switch (this.SelectedConversionType)
            {
                case ConversionId.TrouserSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.TrousersMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.TrousersWomens);
                    }
                    break;
                case ConversionId.ShirtSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.ShirtMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.ShirtWomens);
                    }
                    break;
                case ConversionId.HatSize:
                    measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.Hat);
                    break;
                case ConversionId.SuitSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.SuitMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.SuitWomens);
                    }
                    break;
                case ConversionId.DressSize:
                    measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.DressSize);
                    break;
                case ConversionId.BraSize:
                    measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.Bra);
                    break;
                case ConversionId.HosierySize:
                    measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.Hosiery);
                    break;
                case ConversionId.ShoeSize:
                    measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.Shoes);
                    break;
                case ConversionId.SkiBootSize:
                    measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.SkiBoots);
                    break;
                case ConversionId.WetsuitSize:
                    if (this.SelectedProfile.Gender == Gender.Male)
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.WetsuitMens);
                    }
                    else
                    {
                        measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.WetsuitWomens);
                    }
                    break;
                default:
                    return;
            }
            // Check we have all the necessary measurements
            if (measuredVals == null) return;
            // Build up by regions
            List<RegionIds> selectedRegions = this.GetSelectedRegions();

            // Do database (Linq-to-sql) stuff first, so this should translate to SQL and run SQL with AsEnumerable
            IEnumerable<ConversionData> conversions = (from d in conversiondsDB.ConversionData
                        where d is ConversionData
                        // Filter to specific region, gender, conversion
                        && selectedRegions.Contains(d.Region)
                        && d.Gender == qGender
                        && d.Conversion == this.SelectedConversionType 
                        select d).AsEnumerable();

            // Now do Linq-to-object stuff using methods, getters etc.
            this.GroupedConversions = new ObservableCollection<LongListSelectorGroup<ConversionData>>(
                        from d in conversions
                        orderby d.BrandName ascending
                        // Group into sublists organised by first letter of the retailer
                        group d by d.BrandName[0].ToString().ToUpper()
                            into g
                            orderby g.Key ascending
                            select new LongListSelectorGroup<Model.Conversions.ConversionData>(g.Key, g)
                        );

            // Now do the fits
            for (int i = 0; i < this.GroupedConversions.Count; i++)
            {
                for (int j = 0; j < this.GroupedConversions[i].Items.Count; j++)
                {
                    this.GroupedConversions[i].Items[j].FindBestFit(measuredVals);
                }
            }

        }


        // Already been checked that these values have been taken
        public Dictionary<MeasurementId, double> GetRequiredMeasuredValues(List<MeasurementId> requiredIds)
        {
            Dictionary<MeasurementId, double> requiredValues = new Dictionary<MeasurementId, double>();
            
            foreach (MeasurementId mID in requiredIds)
            {
                Stat s = App.VM.SelectedProfile.Stats.FirstOrDefault(x => x.MeasurementId == mID);
                if (s == null) return null;
                double d;
                if (double.TryParse(s.Value, out d))
                {
                    requiredValues.Add(mID, d);
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
