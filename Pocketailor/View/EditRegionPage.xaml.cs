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
    public partial class EditRegionPage : PhoneApplicationPage
    {
        public EditRegionPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!e.IsNavigationInitiator)
            {
                RegionId rId = App.VM.Regions.Where(x => x.Selected == true).Select(x => x.Id).FirstOrDefault();
                if (PhoneApplicationService.Current.State.ContainsKey("EditRegionPageSelected"))
                {
                    PhoneApplicationService.Current.State["EditRegionPageSelected"] = rId;
                }
                else
                {
                    PhoneApplicationService.Current.State.Add("EditRegionPageSelected", rId);
                }
            }

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!e.IsNavigationInitiator)
            {
                if (PhoneApplicationService.Current.State.ContainsKey("EditRegionPageSelected"))
                {
                    RegionId rId = (RegionId)PhoneApplicationService.Current.State["EditRegionPageSelected"];
                    PhoneApplicationService.Current.State.Remove("EditRegionPageSelected");
                    foreach (Pocketailor.ViewModel.AppViewModel.RegionContainer r in App.VM.Regions)
                    {
                        r.Selected = (r.Id == rId);
                    }
                }
            }
            else
            {
                foreach (Pocketailor.ViewModel.AppViewModel.RegionContainer r in App.VM.Regions)
                {
                    r.Selected = (r.Id == App.VM.SelectedRegion);
                }
            }
        }


        private void saveApplicationBarIconBtn_Click(object sender, System.EventArgs e)
        {
            App.VM.SelectedRegion = (App.VM.Regions.Where(x => x.Selected == true).Select(x => x.Id).FirstOrDefault());
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}