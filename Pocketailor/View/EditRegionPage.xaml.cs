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