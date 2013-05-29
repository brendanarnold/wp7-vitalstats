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


        //public BackgroundWorker bw = new BackgroundWorker();
        

        //public void LoadConversionsPageDataAsync(int profileId, ConversionId conversionId) {

        //    this.IsConversionDataLoading = true;

        //    // Setup the background worker
        //    this.bw.WorkerReportsProgress = false;
        //    this.bw.WorkerSupportsCancellation = true;
        //    this.bw.DoWork += bw_DoWork;
        //    this.bw.RunWorkerCompleted += bw_RunWorkerCompleted;

        //    this.GroupedConversions = null;
            
        //    // Assemble the arguments
        //    GenderId gender = (this.SelectedProfile.Gender == GenderId.Unspecified) ? GenderId.Female : this.SelectedProfile.Gender;
        //    this.LoadRequiredMeasurements(gender, this.SelectedConversionType);
        //    Dictionary<MeasurementId, double> measuredVals = this.ConversionMeasurements.ToDictionary(k => k.MeasurementId, v => Double.Parse(v.Value));
        //    LoadConversionArgs args = new LoadConversionArgs()
        //    {
        //        Region = this.SelectedRegion,
        //        Conversion = this.SelectedConversionType,
        //        Gender = gender,
        //        BlacklistedBrands = this.BlacklistedBrands,
        //        MeasuredValues = measuredVals,
        //    };

        //    if (this.bw.IsBusy)
        //        bw.CancelAsync();
        //    bw.RunWorkerAsync(args);

        
        //}

        //void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    BackgroundWorker worker = sender as BackgroundWorker;
        //    if (!e.Cancelled) {
        //        this.GroupedConversions = e.Result as ObservableCollection<LongListSelectorGroup<ConversionData>>;
        //    }
        //    this.IsConversionDataLoading = false;
        //    if (this.ConversionDataLoaded != null)
        //        this.ConversionDataLoaded(this);
        //}

        //class LoadConversionArgs
        //{
        //    public RegionId Region;
        //    public ConversionId Conversion;
        //    public GenderId Gender;
        //    public List<BrandId> BlacklistedBrands;
        //    public Dictionary<MeasurementId, double> MeasuredValues;
        //}

        //private void bw_DoWork(object sender, DoWorkEventArgs e) {

        //    LoadConversionArgs args = e.Argument as LoadConversionArgs;
        //    BackgroundWorker worker = sender as BackgroundWorker;

        //    if (worker.CancellationPending) 
        //    {
        //        e.Cancel = true;
        //        return; 
        //    }
        //    // Do database (Linq-to-sql) stuff first, so this should translate to SQL and run SQL with AsList
        //    List<ConversionData> conversions = (from d in this.conversiondsDB.ConversionData
        //                                        where d is ConversionData
        //                                            // Filter to specific region, gender, conversion
        //                                        && d.Region == args.Region
        //                                        && d.Gender == args.Gender
        //                                        && d.Conversion == args.Conversion
        //                                        && !args.BlacklistedBrands.Contains(d.Brand)
        //                                        select d).ToList();
        //    if (worker.CancellationPending)
        //    {
        //        e.Cancel = true;
        //        return;
        //    }
        //    conversions.Sort((a, b) => { return a.BrandName.CompareTo(b.BrandName); });
        //    // Group up the brand names
        //    string groupKeys = "#abcdefghijklmnopqrstuvwxyz";
        //    // Initially store in a dictionary
        //    Dictionary<string, List<ConversionData>> groupDict = new Dictionary<string, List<ConversionData>>();
        //    foreach (char c in groupKeys)
        //    {
        //        groupDict.Add(c.ToString(), new List<ConversionData>());
        //    }
        //    if (worker.CancellationPending)
        //    {
        //        e.Cancel = true;
        //        return;
        //    }
        //    foreach (ConversionData cd in conversions)
        //    {
        //        // Find the best fit whilst at it
        //        cd.FindBestFit(args.MeasuredValues);
        //        // Add to the right group according to the first letter of the brand name
        //        char key = char.ToLower(cd.BrandName[0]);
        //        if (key < 'a' || key > 'z') key = '#';
        //        groupDict[key.ToString()].Add(cd);
        //    }
        //    if (worker.CancellationPending)
        //    {
        //        e.Cancel = true;
        //        return;
        //    }
        //    // Buffer first to avoid triggering the NotifyPropertyChanged events on ObservableCollection hundreds of times
        //    List<LongListSelectorGroup<ConversionData>> buff = new List<LongListSelectorGroup<ConversionData>>();
        //    foreach (char key in groupKeys)
        //    {
        //        string k = key.ToString();
        //        buff.Add(new LongListSelectorGroup<ConversionData>(k, groupDict[k]));
        //    }
        //    e.Result = new ObservableCollection<LongListSelectorGroup<ConversionData>>(buff);
        //}



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
                // Do database (Linq-to-sql) stuff first, so this should translate to SQL and run SQL with AsList
                List<ConversionData> conversions = (from d in this.conversiondsDB.ConversionData
                                                    where d is ConversionData
                                                        // Filter to specific region, gender, conversion
                                                    && d.Region == selectedRegion
                                                    && d.Gender == qGender
                                                    && d.Conversion == conversion
                                                    && !blacklistedBrands.Contains(d.Brand)
                                                    select d).ToList();
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
