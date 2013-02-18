using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Globalization;
using Pocketailor.ViewModel;
using System.Windows.Controls;
using Pocketailor.Model;
using System.Collections.Generic;
using Microsoft.Phone.Shell;

namespace Pocketailor.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = App.VM;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            App.VM.SaveChangesToDB();
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

        private void toggleProtectionContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            App.VM.ToggleIsProtected(p);
        }


        private void toggleQuickListContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            App.VM.ToggleQuickProfile(p);
            
        }

        private void editContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            App.VM.SelectedProfile = p;
            NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}", EditProfilePageActions.Edit), UriKind.Relative));
        }

        private void addAppBarBtn_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/EditProfilePage.xaml", UriKind.Relative));
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as Button).DataContext as Profile;
            NavigationService.Navigate(new Uri(String.Format("/View/StatsPage.xaml?Id={0}", p.Id), UriKind.Relative));
        }

        private void howtoBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }



    }
}


