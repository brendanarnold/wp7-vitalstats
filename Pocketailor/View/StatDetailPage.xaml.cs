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
    public partial class StatDetailPage : PhoneApplicationPage
    {
        public StatDetailPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

        }



        private void statDetailGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

        private void editStatAppBarItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}",
                EditStatPageActions.Edit), UriKind.Relative));

        }




    }
}