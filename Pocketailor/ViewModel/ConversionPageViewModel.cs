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

        private ObservableCollection<Group<NameValuePair>> _groupedConversions;
        public ObservableCollection<Group<NameValuePair>> GroupedConversions
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
            // Reset the list
            this.GroupedConversions = new ObservableCollection<Group<NameValuePair>>();
            // Declare vars in the top scope
            Dictionary<MeasurementId, double> measuredVals = new Dictionary<MeasurementId, double>();
            IQueryable dataQuery = null;
            // Make sure we have region data
            if (this.GetSelectedRegions() == null) return;
            // TODO: If gender not specified, then return Female measurements. Note only perform gener query on tables that have 
            // Gender fields (even after casting) because it still generate SQL to query gender
            Gender qGender = (this.SelectedProfile.Gender == Gender.Unspecified) ? Gender.Female : this.SelectedProfile.Gender; 
            // Get the conversion specific data
            //switch (this.SelectedConversionType)
            //{
            //    case ConversionId.TrouserSize:
            //        if (this.SelectedProfile.Gender == Gender.Male)
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.TrousersMens);
            //        }
            //        else
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(RequiredMeasurements.TrousersWomens);
            //        }
            //        dataQuery = this.conversiondsDB.Trousers.Where(q => q.Gender == qGender);
            //        break;
            //    case ConversionId.ShirtSize:
            //        if (this.SelectedProfile.Gender == Gender.Male)
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShirtUtils.RequiredMeasurementsMens);
            //        }
            //        else
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShirtUtils.RequiredMeasurementsWomens);
            //        }
            //        dataQuery = this.conversiondsDB.Shirts.Where(q => q.Gender == qGender);
            //        break;
            //    case ConversionId.HatSize:
            //        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.HatUtils.RequiredMeasurements);
            //        dataQuery = this.conversiondsDB.Hats;
            //        break;
            //    case ConversionId.SuitSize:
            //        if (this.SelectedProfile.Gender == Gender.Male)
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SuitUtils.RequiredMeasurementsMens);
            //        }
            //        else
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SuitUtils.RequiredMeasurementsWomens);
            //        }
            //        dataQuery = this.conversiondsDB.Suits.Where(q => q.Gender == qGender);
            //        break;
            //    case ConversionId.DressSize:
            //        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.DressSizeUtils.RequiredMeasurements);
            //        dataQuery = this.conversiondsDB.DressSizes;
            //        break;
            //    case ConversionId.BraSize:
            //        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.BraUtils.RequiredMeasurements);
            //        dataQuery = this.conversiondsDB.Bras;
            //        break;
            //    case ConversionId.HosierySize:
            //        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.HosieryUtils.RequiredMeasurements);
            //        dataQuery = this.conversiondsDB.Hosiery;
            //        break;
            //    case ConversionId.ShoeSize:
            //        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.ShoesUtils.RequiredMeasurements);
            //        dataQuery = this.conversiondsDB.Shoes.Where(q => q.Gender == qGender);
            //        break;
            //    case ConversionId.SkiBootSize:
            //        measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.SkiBootsUtils.RequiredMeasurements);
            //        dataQuery = this.conversiondsDB.SkiBoots.Where(q => q.Gender == qGender);
            //        break;
            //    //case ConversionId.TennisGripSize:
            //    //    measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.TennisRaquetSizesUtils.RequiredMeasurements);
            //    //    dataQuery = appDB.TennisRaquetSizes.Cast<Model.Conversions.IConversionData>();
            //    //    break;
            //    case ConversionId.WetsuitSize:
            //        if (this.SelectedProfile.Gender == Gender.Male)
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.WetsuitUtils.RequiredMeasurementsMens);
            //        }
            //        else
            //        {
            //            measuredVals = this.GetRequiredMeasuredValues(Model.Conversions.WetsuitUtils.RequiredMeasurementsWomens);
            //        }
            //        dataQuery = this.conversiondsDB.Wetsuits.Where(q => q.Gender == qGender);
            //        break;
            //    default:
            //        return;
            //}
            // Check we have all the necessary measurements
            if (measuredVals == null) return;
            // Build up by regions
            List<RegionIds> selectedRegions = this.GetSelectedRegions();
            //dataQuery = dataQuery.Cast<Model.Conversions.IConversionData>().Where(q => selectedRegions.Contains(q.Region));


            //var newQuery = dataQuery.Cast<IConversionData>().GroupBy(c => c.Retailer).Select(g => 
                
                
                
                
                //from cnv in dataQuery.Cast<Model.Conversions.IConversionData>()
                //           group cnv.GetChiSq(measuredVals) by cnv.Retailer into cnvEnumerable
                //           select cnvEnumerable.Min()
                           


                           //orderby Lookup.Retail[cnvEnumerable.Key]
                           //select new Group<IConversionData>(Lookup.Retail[cnvEnumerable.Key], cnvEnumerable);
                           


        //    foreach (RegionIds region in this.GetSelectedRegions())
        //    {
        //        ConversionRegion cr = new ConversionRegion();
        //        cr.Name = Lookup.Regions[region];
        //        cr.Conversions = new ObservableCollection<NameValuePair>();
        //        var dataByRegion = dataQuery.Cast<Model.Conversions.IConversionData>().Where(q => q.Region == region);
        //        foreach (RetailId retailId in dataByRegion.Select(ds => ds.Retailer).Distinct())
        //        {
        //            var conversionData = dataByRegion.Where(ds => ds.Retailer == retailId);
        //            double lowestChisq = double.MaxValue;
        //            Model.Conversions.IConversionData bestFit = null;
        //            foreach (Model.Conversions.IConversionData candidateConversion in conversionData)
        //            {
        //                double chiSq = candidateConversion.GetChiSq(measuredVals);
        //                if (chiSq < lowestChisq)
        //                {
        //                    lowestChisq = chiSq;
        //                    bestFit = candidateConversion;
        //                }
        //            }
        //            cr.Conversions.Add(new NameValuePair()
        //            {
        //                // TODO: Will need to include some kind of lookup for actual string
        //                FormattedValue = bestFit.FormattedValue,
        //                Retailer = retailId,
        //            });
        //        }
        //        this.GroupedConversions.Add(cr);
        //    }
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

        public class ConversionRegion
        {
            public string Name { get; set; }
            public ObservableCollection<NameValuePair> Conversions { get; set; }
        }

        public class Group<T> : IEnumerable<T>
        {
            public Group(string name, IEnumerable<T> items)
            {
                this.Title = name;
                this.Items = new List<T>(items);
            }

            public override bool Equals(object obj)
            {
                Group<T> that = obj as Group<T>;

                return (that != null) && (this.Title.Equals(that.Title));
            }

            public string Title
            {
                get;
                set;
            }

            public IList<T> Items
            {
                get;
                set;
            }

            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                return this.Items.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.Items.GetEnumerator();
            }

            #endregion
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
