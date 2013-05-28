using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Pocketailor.Model;

namespace Pocketailor.View
{
    public partial class EditBlacklistedBrands : PhoneApplicationPage
    {
        public EditBlacklistedBrands()
        {
            InitializeComponent();

            this.DataContext = App.VM;

        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!e.IsNavigationInitiator)
            {
                List<BrandId> blacklistedBrands = App.VM.Brands.Where(x => x.Selected == false).Select(x => x.Id).ToList();
                if (PhoneApplicationService.Current.State.ContainsKey("EditBlacklistedBrandsPageBlacklisted"))
                {
                    PhoneApplicationService.Current.State["EditBlacklistedBrandsPageBlacklisted"] = blacklistedBrands;
                }
                else
                {
                    PhoneApplicationService.Current.State.Add("EditBlacklistedBrandsPageBlacklisted", blacklistedBrands);
                }
            }

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!e.IsNavigationInitiator)
            {
                if (PhoneApplicationService.Current.State.ContainsKey("EditBlacklistedBrandsPageBlacklisted"))
                {
                    List<BrandId> blacklistedBrands = (List<BrandId>)PhoneApplicationService.Current.State["EditBlacklistedBrandsPageBlacklisted"];
                    PhoneApplicationService.Current.State.Remove("EditBlacklistedBrandsPageBlacklisted");
                    foreach (Pocketailor.ViewModel.AppViewModel.BrandContainer b in App.VM.Brands)
                    {
                        b.Selected = !blacklistedBrands.Contains(b.Id);
                    }
                }
            }
            else
            {
                foreach (Pocketailor.ViewModel.AppViewModel.BrandContainer b in App.VM.Brands)
                {
                    b.Selected = !App.VM.BlacklistedBrands.Contains(b.Id);
                }
            }
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            App.VM.SkipLoadConversionPageData = true;
            base.OnBackKeyPress(e);
        }


        private void saveApplicationBarIconBtn_Click(object sender, System.EventArgs e)
        {
            List<BrandId> newList = App.VM.Brands.Where(x => x.Selected == false).Select(x => x.Id).ToList();
            newList.Sort();
            App.VM.BlacklistedBrands.Sort();
            if (!newList.SequenceEqual(App.VM.BlacklistedBrands))
            {
                App.VM.BlacklistedBrands = newList;
                App.VM.SaveBlacklistedBrands();
            }
            else
            {
                App.VM.SkipLoadConversionPageData = true;
            }
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}