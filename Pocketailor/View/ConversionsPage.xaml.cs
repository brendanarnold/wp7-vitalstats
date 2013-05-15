using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Pocketailor.View
{
    public partial class ConversionsPage : PhoneApplicationPage
    {
        public ConversionsPage()
        {
            InitializeComponent();

            App.VM.LoadConversionsPageData();
            this.DataContext = App.VM;

            

        }

        

        private void showHiddenAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            if (!App.VM.ShowBlacklistedConversions && App.Settings.GetValueOrDefault<bool>("ShowHiddenBrandsHelp", true))
            {
                MessageBoxResult res = MessageBox.Show("Edit hidden brands", 
                    "Untick the boxes for all the brands that you are not interested in. When done, click the button at the bottom again to hide them", MessageBoxButton.OK);
                if (res == MessageBoxResult.OK)
                {
                    App.Settings.AddOrUpdateValue("ShowHiddenBrandsHelp", true);
                }
            }

            App.VM.ShowBlacklistedConversions = !App.VM.ShowBlacklistedConversions;
            ApplicationBarMenuItem mi = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
            mi.Text = (App.VM.ShowBlacklistedConversions) ? "hide hidden brands" : "edit hidden brands";
        }

        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/View/EditRegionPage.xaml", UriKind.Relative));
        }



    }
}