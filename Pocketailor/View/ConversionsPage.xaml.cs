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


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            App.VM.SaveBlacklistedBrands();
        }


        //private void showHiddenAppBarMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    this.ToggleEditBlacklist();
        //    ApplicationBarMenuItem mi = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
        //    mi.Text = (App.VM.ShowBlacklistedConversions) ? "hide hidden brands" : "edit hidden brands";
        //}


        private void ToggleEditBlacklist()
        {
            if (!App.VM.ShowBlacklistedConversions && App.Settings.GetValueOrDefault<bool>("ShowHiddenBrandsHelp", true))
            {
                MessageBoxResult res = MessageBox.Show("Untick the boxes for all the brands that you are not interested in."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "When done, tap the button under the title to hide them",
                    "Edit hidden brands", MessageBoxButton.OK);
                if (res == MessageBoxResult.OK)
                {
                    App.Settings.AddOrUpdateValue("ShowHiddenBrandsHelp", false);
                    App.Settings.Save();
                }
            }
            App.VM.ShowBlacklistedConversions = !App.VM.ShowBlacklistedConversions;
        }

        private void editHiddenBrandsBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.ToggleEditBlacklist();
        }

        //private void editHiddenBrandsApplicationBarIconButton_Click(object sender, System.EventArgs e)
        //{
        //    this.ToggleEditBlacklist();
        //    ApplicationBarIconButton abBtn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
        //    if (App.VM.ShowBlacklistedConversions)
        //    {
        //        abBtn.IconUri = new Uri("/Images/AppBar/appbar.save.png", UriKind.Relative);
        //        abBtn.Text = "save hidden brands";
        //    }
        //    else
        //    {
        //        abBtn.IconUri = new Uri("/Images/AppBar/appbar.edit.png", UriKind.Relative);
        //        abBtn.Text = "edit hidden brands";
        //    }
        //}

        //private void changeRegionApplicationBarMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    NavigationService.Navigate(new Uri("/View/EditRegionPage.xaml", UriKind.Relative));
        //}

        private void dismissHiddenBranEditBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	this.ToggleEditBlacklist();
        }

        private void editHiddenBrandsBtn_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	this.ToggleEditBlacklist();
        }



    }
}