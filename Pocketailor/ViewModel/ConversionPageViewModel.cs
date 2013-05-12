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
                    this.NotifyPropertyChanged("GroupedConversions");
                }
            }
        }



        public void LoadConversionsPageData()
        {
            // Declare vars in the top scope
            Dictionary<MeasurementId, double> measuredVals = new Dictionary<MeasurementId, double>();
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
            RegionIds selectedRegion = this.SelectedRegion;

            // Do database (Linq-to-sql) stuff first, so this should translate to SQL and run SQL with AsEnumerable
            IEnumerable<ConversionData> conversions = (from d in conversiondsDB.ConversionData
                        where d is ConversionData
                        // Filter to specific region, gender, conversion
                        && d.Region == selectedRegion
                        && d.Gender == qGender
                        && d.Conversion == this.SelectedConversionType 
                        select d).AsEnumerable();

            // Now do Linq-to-object stuff using methods, getters etc.
            this.GroupedConversions = new ObservableCollection<LongListSelectorGroup<ConversionData>>(
                        from d in conversions
                        orderby d.BrandName ascending
                        // Group into sublists organised by first letter of the retailer
                        group d by d.BrandName[0].ToString().ToLower()
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


        internal void ApplyAdjustment(Model.Conversions.ConversionData c, int p)
        {
            // TODO: Implement this
        }


        #region Blacklist properties/methods

        private ObservableCollection<RetailId> _blacklistedRetailers;
        public ObservableCollection<RetailId> BlacklistedRetailers
        {
            get
            {
                if (this._blacklistedRetailers == null)
                    this.LoadBlacklistedRetailers();
                return this._blacklistedRetailers;
            }
            set
            {
                if (this._blacklistedRetailers != value)
                {
                    this._blacklistedRetailers = value;
                    this.NotifyPropertyChanged("BlacklistedRetailers");
                }
            }
        }

        public void LoadBlacklistedRetailers()
        {
            this.BlacklistedRetailers = App.Settings.GetValueOrDefault<ObservableCollection<RetailId>>("BlacklistedRetailers", new ObservableCollection<RetailId>());
        }

        public void SaveBlacklistedRetailers()
        {
            App.Settings.AddOrUpdateValue("BlacklistedRetailers", this.BlacklistedRetailers);
            App.Settings.Save();
        }

        private bool _showBlacklistedConversions = false;
        public bool ShowBlacklistedConversions
        {
            get { return this._showBlacklistedConversions; }
            set
            {
                if (this._showBlacklistedConversions != value)
                {
                    this._showBlacklistedConversions = value;
                    this.NotifyPropertyChanged("ShowBlacklistedConversions");
                }
            }
        }


        #endregion

    }
}
