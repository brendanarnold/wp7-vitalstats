using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace VitalStats.View
{
    public partial class AddProfilePage : PhoneApplicationPage
    {

        public AddProfilePage()
        {
            InitializeComponent();
        }


        private void ConfirmLeave(Uri uri)
        {
            MessageBoxResult m = MessageBox.Show("You have entered some profile data which will be lost if you leave this page. Are you sure you want to leave this page?", 
                "Confirm leave page", MessageBoxButton.OKCancel);    
            if (m == MessageBoxResult.OK) 
            {
                NavigationService.Navigate(uri);
            }
        }

        private bool DataOnPage()
        {
            // Returns true if data on page that will need to be saved
            if (this.nameTextBox.Text != String.Empty)
                return true;
            if ((bool)this.protectCheckBox.IsChecked)
                return true;
            return false;
        }

        private void cancelBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if (this.DataOnPage())
            {
                this.ConfirmLeave(new Uri("/View/MainPage.xaml", UriKind.Relative));
                
            }
            else
            {
                NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
            }


		}

        private void addBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            bool isProtected = (bool)this.protectCheckBox.IsChecked;
            string name = this.nameTextBox.Text;
            Uri uri = new Uri(String.Format("/View/MainPage.xaml?action={0}&name={1}&isProtected={2}", 
                Uri.EscapeDataString(MainPage.UriActions.AddProfile), Uri.EscapeDataString(name), isProtected.ToString()), UriKind.Relative);
            NavigationService.Navigate(uri);
        }
    }
}
