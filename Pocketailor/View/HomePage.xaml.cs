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
using Pocketailor.Model;
using Microsoft.Phone.Shell;

namespace Pocketailor.View
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
            this.DataContext = App.VM;
        }

        private void addProfileBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void toggleQuickListContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            App.VM.ToggleQuickProfile(p);

        }

        private void profileButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as Canvas).DataContext as Profile;
            NavigationService.Navigate(new Uri(String.Format("/View/MeasurementsPage.xaml?Id={0}", p.Id), UriKind.Relative));
        }

        private void editContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            //if (p.IsProtected && App.VM.IsLocked) return;
            App.VM.SelectedProfile = p;
            NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}&ProfileId={1}", EditProfilePageActions.Edit, App.VM.SelectedProfile.Id), UriKind.Relative));
        }

        private void deleteContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile profile = ((sender as MenuItem).DataContext) as Profile;
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Confirm delete",
                Message = String.Format("Are you certain you want to delete the profile for '{0}'?", profile.Name),
                LeftButtonContent = "Yes",
                RightButtonContent = "No",
                IsFullScreen = false
            };
            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        App.VM.DeleteProfile(profile);
                        break;
                    case CustomMessageBoxResult.RightButton:
                    default:
                        break;
                }
            };
            messageBox.Show();
        }

        private void addProfileApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}", EditProfilePageActions.New), UriKind.Relative));
        }

        private void Panorama_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // this is really jerky
            //switch (this.mainPanorama.SelectedIndex)
            //{
            //    case 1:
            //        this.ApplicationBar = (ApplicationBar)Resources["profilesApplicationBar"];
            //        break;
            //    default:
            //        this.ApplicationBar = null;
            //        break;
            //}
        }

        private void addNewProfileButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}", EditProfilePageActions.New), UriKind.Relative));
        }

        



    }
}