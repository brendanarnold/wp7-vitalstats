using Pocketailor.Model;
using Pocketailor.Model.Adjustments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    this.NotifyPropertyChanged("ConversionsPageTitle");
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
                    this.NotifyPropertyChanged("ConversionsPageBGImage");
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

        public void LoadRequiredMeasurements(GenderId gender, ConversionId conversion)
        {
            // Get the conversion specific data
            switch (conversion)
            {
                case ConversionId.TrouserSize:
                    if (gender == GenderId.Male)
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.TrousersMens);
                    }
                    else
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.TrousersWomens);
                    }
                    break;
                case ConversionId.ShirtSize:
                    if (gender == GenderId.Male)
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.ShirtMens);
                    }
                    else
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.ShirtWomens);
                    }
                    break;
                case ConversionId.HatSize:
                    this.LoadConversionMeasurements(RequiredMeasurements.Hat);
                    break;
                case ConversionId.SuitSize:
                    if (gender == GenderId.Male)
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.SuitMens);
                    }
                    else
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.SuitWomens);
                    }
                    break;
                case ConversionId.DressSize:
                    this.LoadConversionMeasurements(RequiredMeasurements.DressSize);
                    break;
                case ConversionId.BraSize:
                    this.LoadConversionMeasurements(RequiredMeasurements.Bra);
                    break;
                case ConversionId.HosierySize:
                    this.LoadConversionMeasurements(RequiredMeasurements.Hosiery);
                    break;
                case ConversionId.ShoeSize:
                    this.LoadConversionMeasurements(RequiredMeasurements.Shoes);
                    break;
                case ConversionId.SkiBootSize:
                    this.LoadConversionMeasurements(RequiredMeasurements.SkiBoots);
                    break;
                case ConversionId.WetsuitSize:
                    if (gender == GenderId.Male)
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.WetsuitMens);
                    }
                    else
                    {
                        this.LoadConversionMeasurements(RequiredMeasurements.WetsuitWomens);
                    }
                    break;
                default:
                    return;
            }
        }


        // Some event to handle the async loading of the hefty conversion data

        public delegate void ConversionDataLoadingEventHandler(object sender);
        public event ConversionDataLoadingEventHandler ConversionDataLoading;

        public delegate void ConversionDataLoadedEventHandler(object sender);
        public event ConversionDataLoadedEventHandler ConversionDataLoaded;

        // Include a bindable property for when the page is loading
        private bool _isConversionDataLoading = false;
        public bool IsConversionDataLoading {
            get { return this._isConversionDataLoading; }
            set
            {
                if (this._isConversionDataLoading != value)
                {
                    this._isConversionDataLoading = value;
                    this.NotifyPropertyChanged("IsConversionDataLoading");
                }
            }
        }

        // If this is set then the conversion page will not run the method below
        public bool SkipLoadConversionPageData { get; set; }




        public async Task LoadConversionsPageDataAsyncTask(int profileId, ConversionId conversionId)
        {
            // Fire the event off
            this.IsConversionDataLoading = true;
            if (this.ConversionDataLoading != null)
                this.ConversionDataLoading(this);

            // Load up the data in case of tombstone situation
            //if (App.VM.SelectedProfile == null || App.VM.SelectedProfile.Id != profileId)
            //    App.VM.SelectedProfile = (from Profile p in App.VM.appDB.Profiles where p.Id == profileId select p).FirstOrDefault();
            //if (App.VM.SelectedConversionType != conversionId) 
            //    App.VM.SelectedConversionType = conversionId;


            // TODO: If gender not specified, then return Female measurements. Note only perform gener query on tables that have 
            // Gender fields (even after casting) because it still generate SQL to query gender
            GenderId qGender = (this.SelectedProfile.Gender == GenderId.Unspecified) ? GenderId.Female : this.SelectedProfile.Gender;

            this.LoadRequiredMeasurements(qGender, this.SelectedConversionType);

            Dictionary<MeasurementId, double> measuredVals = this.ConversionMeasurements.ToDictionary(k => k.MeasurementId, v => Double.Parse(v.Value));
            // Check we have all the necessary measurements
            if (measuredVals == null) return;
            // Build up by regions
            RegionId selectedRegion = this.SelectedRegion;
            // Await the resolution of BlacklistedBrands
            BrandId[] blacklistedBrands = App.VM.BlacklistedBrands.ToArray();
            ConversionId conversion = App.VM.SelectedConversionType;

            this.GroupedConversions = await TaskEx.Run(() =>
            {
                RegionId r = selectedRegion;
                List<ConversionData> conversions = new List<ConversionData>();
                IEnumerable<BrandId> collectedBrands = new List<BrandId>();
                while (true) // I know ...
                {
                    // Do database (Linq-to-sql) stuff first, so this should translate to SQL and run SQL with AsList
                    conversions.AddRange(from d in this.conversiondsDB.ConversionData
                                         where d is ConversionData
                                             // Filter to specific region, gender, conversion
                                         && d.Region == r
                                         && d.Gender == qGender
                                         && d.Conversion == conversion
                                         && !blacklistedBrands.Contains(d.Brand)
                                         && !collectedBrands.Contains(d.Brand)
                                         select d);
                    collectedBrands = from d in conversions select d.Brand;
                    if (r == RegionId.Worldwide) break;
                    r = Static.RegionParents[r];
                }
                conversions.Sort((a, b) => { return a.BrandName.CompareTo(b.BrandName); });
                // Group up the brand names
                string groupKeys = "#abcdefghijklmnopqrstuvwxyz";
                // Initially store in a dictionary
                Dictionary<string, List<ConversionData>> groupDict = new Dictionary<string, List<ConversionData>>();
                foreach (char c in groupKeys)
                {
                    groupDict.Add(c.ToString(), new List<ConversionData>());
                }
                foreach (ConversionData cd in conversions)
                {
                    // Find the best fit whilst at it
                    cd.FindBestFit(measuredVals);
                    // Add to the right group according to the first letter of the brand name
                    char key = char.ToLower(cd.BrandName[0]);
                    if (key < 'a' || key > 'z') key = '#';
                    groupDict[key.ToString()].Add(cd);
                }
                // Buffer first to avoid triggering the NotifyPropertyChanged events on ObservableCollection hundreds of times
                List<LongListSelectorGroup<ConversionData>> buff = new List<LongListSelectorGroup<ConversionData>>();
                foreach (char key in groupKeys)
                {
                    string k = key.ToString();
                    buff.Add(new LongListSelectorGroup<ConversionData>(k, groupDict[k]));
                }
                return new ObservableCollection<LongListSelectorGroup<ConversionData>>(buff);
            });

            // Fire the end event
            if (this.ConversionDataLoaded != null)
                this.ConversionDataLoaded(this);
            this.IsConversionDataLoading = false;

        }


        public void LoadConversionMeasurements(List<MeasurementId> requiredIds)
        {
            this.ConversionMeasurements = new ObservableCollection<Model.Measurement>(
                App.VM.SelectedProfile.Measurements.Where(m => requiredIds.Contains(m.MeasurementId)));
        }

        public ObservableCollection<Measurement> ConversionMeasurements { get; set; }


        internal List<MeasurementId> GetMissingMeasurements(List<MeasurementId> requiredIds)
        {
            return requiredIds.Except(this.SelectedProfile.Measurements.Select(s => s.MeasurementId)).ToList();
        }


        


        
    }
}
