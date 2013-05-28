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
        private List<BrandId> _blacklistedBrand;
        public List<BrandId> BlacklistedBrands
        {
            get
            {
                if (this._blacklistedBrand == null)
                    this.LoadBlacklistedBrands();
                return this._blacklistedBrand;
            }
            set
            {
                if (this._blacklistedBrand != value)
                {
                    this._blacklistedBrand = value;
                    this.NotifyPropertyChanged("BlacklistedBrands");
                }
            }
        }

        public void LoadBlacklistedBrands()
        {
            this.BlacklistedBrands = App.Settings.GetValueOrDefault<List<BrandId>>("BlacklistedBrands", new List<BrandId>());
        }

        public void SaveBlacklistedBrands()
        {
            App.Settings.AddOrUpdateValue("BlacklistedBrands", this.BlacklistedBrands);
            App.Settings.Save();
        }

        

        private ObservableCollection<BrandContainer> _brands;
        public ObservableCollection<BrandContainer> Brands
        {
            get
            {
                if (this._brands == null) this.LoadBrandContainers();
                return this._brands;
            }
            set
            {
                if (this._brands != value)
                {
                    this._brands = value;
                    this.NotifyPropertyChanged("Brands");
                }
            }
        }

        public void LoadBrandContainers()
        {
            this._brands = new ObservableCollection<BrandContainer>();

            foreach (BrandId b in typeof(BrandId).GetFields().Where(x => x.IsLiteral).Select(x => x.GetValue(typeof(BrandId))).Cast<BrandId>())
            {
                this._brands.Add(new BrandContainer { Name = Lookup.Brand[b], Id = b, Selected = !this.BlacklistedBrands.Contains(b) });
            }
        }

        public class BrandContainer
        {
            public string Name { get; set; }
            public BrandId Id { get; set; }
            public bool Selected { get; set; }
        }

    }
}
