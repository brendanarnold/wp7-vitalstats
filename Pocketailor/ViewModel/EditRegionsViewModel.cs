using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel : INotifyPropertyChanged
    {


        private string _selectedRegion;
        public string SelectedRegion
        {
            get
            {
                if (this._selectedRegion == null)
                {
                    // Set if it exists already, otherwise try to get from the environment, otherwise revert to the default
                    this._selectedRegion = App.Settings.GetValueOrDefault<string>("SelectedRegion", null);
                    if (this._selectedRegion == null)
                    {
                        string[] els = CultureInfo.CurrentCulture.Name.Split(new char[] { '-' });
                        this._selectedRegion = (els.Length == 2) ? els[1] : AppConstants.DEFAULT_REGION;
                    }
                }
                return  (string)this._selectedRegion;
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
                    //if (this.GroupedConversions != null)
                    //    this.LoadConversionsPageData();
                }
            }
        }

        public string SelectedRegionString
        {
            get
            {
                return Globalisation.Helpers.GetRegionNameFromIso(this.SelectedRegion);
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
            string selectedRegion = this.SelectedRegion;
            foreach (string r in Globalisation.CustomRegions.RegionParents.Keys)
            {
                this._regions.Add(new RegionContainer { Name = Globalisation.Helpers.GetRegionNameFromIso(r), Id = r, Selected = (selectedRegion == r) });
            }
            this._regions = new ObservableCollection<RegionContainer>(this._regions.OrderBy(r => r.Name));
        }

        public class RegionContainer
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public bool Selected { get; set; }
        }

    }
}
