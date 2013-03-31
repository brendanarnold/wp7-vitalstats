using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Pocketailor.View.ConversionPages
{
    public partial class ConversionsPage : PhoneApplicationPage
    {
        public ConversionsPage()
        {
            InitializeComponent();

            App.VM.LoadConversionsPageData();
            this.DataContext = App.VM;
        }

        public void toggleHideRetailerContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ViewModel.AppViewModel.NameValuePair nvp = (sender as MenuItem).DataContext as ViewModel.AppViewModel.NameValuePair;
            nvp.ToggleHidden();
        }

        private void showHiddenAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            App.VM.ShowHiddenConversions = !App.VM.ShowHiddenConversions;
            ApplicationBarMenuItem mi = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
            mi.Text = (App.VM.ShowHiddenConversions) ? "hide hidden retailers" : "show hidden retailers";
        }

    }
}