using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel : INotifyPropertyChanged
    {


        private RegionId? _selectedRegion;
        public RegionId SelectedRegion
        {
            get
            {
                if (this._selectedRegion == null)
                {
                    this._selectedRegion = App.Settings.GetValueOrDefault<RegionId>("SelectedRegion", AppConstants.DEFAULT_REGION);
                }
                return  (RegionId)this._selectedRegion;
            }
            set 
            {
                if (this._selectedRegion != value)
                {
                    this._selectedRegion = value;
                    App.Settings.AddOrUpdateValue("SelectedRegion", value);
                    App.Settings.Save();
                    this.NotifyPropertyChanged("SelectedRegion");
                    this.NotifyPropertyChanged("SelectedRegionString");
                    if (this.GroupedConversions != null)
                        this.LoadConversionsPageData();
                }
            }
        }

        public string SelectedRegionString
        {
            get
            {
                return Lookup.Regions[this.SelectedRegion];
            }
        }
        

        private ObservableCollection<RegionContainer> _regions;
        public ObservableCollection<RegionContainer> Regions
        {
            get
            {
                if (this._regions == null) this.LoadRegionContainers();
                return this._regions;
            }
            set
            {
                if (this._regions != value)
                {
                    this._regions = value;
                    this.NotifyPropertyChanged("Regions");
                }
            }
        }

        public void LoadRegionContainers()
        {
            this._regions = new ObservableCollection<RegionContainer>();
            RegionId selectedRegion = this.SelectedRegion;
            foreach (RegionId r in typeof(RegionId).GetFields().Where(x => x.IsLiteral).Select(x => x.GetValue(typeof(RegionId))).Cast<RegionId>())
            {
                this._regions.Add(new RegionContainer { Name = Lookup.Regions[r], Id = r, Selected = (selectedRegion == r) });
            }
        }

        public class RegionContainer
        {
            public string Name { get; set; }
            public RegionId Id { get; set; }
            public bool Selected { get; set; }
        }

    }
}
