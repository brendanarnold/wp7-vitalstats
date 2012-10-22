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

        private void ConfirmLeave()
        {
        }

        public void GoToProfilePage()
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        public void cancelBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
//            this.ConfirmLeave();
            if ((this.nameTextBox.Text != String.Empty) || (bool)this.protectCheckBox.IsChecked)
            {
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = "Confirm leave page",
                    Message = "You have entered some profile data which will be lost if you leave this page. Are you sure you want to leave this page?",
                    LeftButtonContent = "no",
                    RightButtonContent = "yes"
                };

                messageBox.Dismissed += (s1, e1) =>
                {
                    if (e1.Result == CustomMessageBoxResult.RightButton)
                    {
                        GoToProfilePage();
                    }
                };
                messageBox.Show();
            }
            else
            {
                GoToProfilePage();
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
