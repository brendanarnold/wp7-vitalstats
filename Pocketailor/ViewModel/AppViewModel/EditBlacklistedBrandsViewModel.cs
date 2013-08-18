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


        private bool _sugestedOptionsVisible;
        public bool SuggestedOptionsVisible
        {
            get { return this._sugestedOptionsVisible; }
            set
            {
                if (this._sugestedOptionsVisible != value)
                {
                    this._sugestedOptionsVisible = value;
                    this.NotifyPropertyChanged("SuggestedOptionsVisible");
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
            List<BrandContainer> brands = new List<BrandContainer>();

            foreach (BrandId b in typeof(BrandId).GetFields().Where(x => x.IsLiteral).Select(x => x.GetValue(typeof(BrandId))).Cast<BrandId>())
            {
                brands.Add(new BrandContainer { Name = Lookup.Brand[b], Id = b, Selected = !this.BlacklistedBrands.Contains(b) });
            }
            this._brands = new ObservableCollection<BrandContainer>(brands.OrderBy(b => b.Name));

            // Now group for use in the LongListSelector
            string groupKeys = "#abcdefghijklmnopqrstuvwxyz";
            // Initially store in a dictionary
            Dictionary<string, List<BrandContainer>> groupDict = new Dictionary<string, List<BrandContainer>>();
            foreach (char c in groupKeys)
            {
                groupDict.Add(c.ToString(), new List<BrandContainer>());
            }
            foreach (BrandContainer b in this.Brands)
            {
                // Add to the right group according to the first letter of the brand name
                char key = char.ToLower(b.Name[0]);
                if (key < 'a' || key > 'z') key = '#';
                groupDict[key.ToString()].Add(b);
            }
            List<LongListSelectorGroup<BrandContainer>> buff = new List<LongListSelectorGroup<BrandContainer>>();
            foreach (char key in groupKeys)
            {
                string k = key.ToString();
                buff.Add(new LongListSelectorGroup<BrandContainer>(k, groupDict[k]));
            }
            this.GroupedBrands = new ObservableCollection<LongListSelectorGroup<BrandContainer>>(buff);


        }

        public class BrandContainer
        {
            public string Name { get; set; }
            public BrandId Id { get; set; }
            public bool Selected { get; set; }
        }


        private ObservableCollection<LongListSelectorGroup<BrandContainer>> _groupedBrands;
        public ObservableCollection<LongListSelectorGroup<BrandContainer>> GroupedBrands
        {
            get { return this._groupedBrands; }
            set
            {
                if (this._groupedBrands != value)
                {
                    this._groupedBrands = value;
                    this.NotifyPropertyChanged("GroupedBrands");
                }
            }
        }

    }
}
