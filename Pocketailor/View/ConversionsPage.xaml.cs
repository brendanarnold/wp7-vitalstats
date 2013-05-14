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
            App.VM.ShowBlacklistedConversions = !App.VM.ShowBlacklistedConversions;
            ApplicationBarMenuItem mi = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
            mi.Text = (App.VM.ShowBlacklistedConversions) ? "hide hidden retailers" : "show hidden retailers";
        }

        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/View/EditRegionPage.xaml", UriKind.Relative));
        }



    }
}