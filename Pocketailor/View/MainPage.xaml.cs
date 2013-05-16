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
            //this.lockedStateGroup.CurrentStateChanged += lockedStateGroup_StateChanged;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = App.VM;
            //this.UpdateUILockState();
            
            // Hack to get QuickProfiles to update when modified off this page
            App.VM.NotifyPropertyChanged("QuickProfiles");


            // Show the welcome messagebox if appropriate
            if (App.Settings.GetValueOrDefault<bool>("ShowWelcome", true))
            {
                
                ListPicker listPicker = new ListPicker()
                {
                    Header = "Please choose your preferred units:",
                    ItemsSource = new string[] { "Metric (m/cm/kg)", "Imperial (ft/in/lb)" },
                    Margin = new Thickness(12, 42, 24, 18)
                };


                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = "Welcome to Pocketailor!",
                    Message =
                        "Pocketailor calculates the best clothing sizes for brands worldwide based on your measurements."
                        + Environment.NewLine + Environment.NewLine
                        + "Whilst we believe Pocketailer gives great results, we cannot accept any responsibility if, for some reason, clothes bought are not the correct size!",
                    Content = listPicker,
                    LeftButtonContent = "ok",
                    IsFullScreen = false,
                };

                messageBox.Dismissing += (s1, e1) =>
                {
                    if (listPicker.ListPickerMode == ListPickerMode.Expanded)
                    {
                        e1.Cancel = true;
                    }
                };

                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.None:
                        case CustomMessageBoxResult.LeftButton:
                            if (listPicker.SelectedIndex == 0)
                            {
                                App.VM.UnitCulture = UnitCultureId.Metric;
                            }
                            else
                            {
                                App.VM.UnitCulture = UnitCultureId.Imperial;
                            }
                            App.Settings.AddOrUpdateValue("ShowWelcome", false);
                            break;
                        case CustomMessageBoxResult.RightButton:
                            break;
                        default:
                            break;
                    }
                };

                messageBox.Show();
            }

        }

        //private void UpdateUILockState()
        //{
        //    (this.ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).Text = (App.VM.IsLocked) ? "unlock" : "lock";
        //}

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            App.VM.SaveChangesToAppDB();
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

        //private void toggleProtectionContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    Profile p = (sender as MenuItem).DataContext as Profile;
        //    if (p.IsProtected && App.VM.IsLocked) return;
        //    App.VM.ToggleIsProtected(p);
        //}


        private void toggleQuickListContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            App.VM.ToggleQuickProfile(p);
            
        }

        private void editContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            //if (p.IsProtected && App.VM.IsLocked) return;
            App.VM.SelectedProfile = p;
            NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}", EditProfilePageActions.Edit), UriKind.Relative));
        }

        //private void secondaryTileContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    Profile p = (sender as MenuItem).DataContext as Profile;
        //    SecondaryTileHelpers.CreateSecondaryTile(p);
        //}

        private void addAppBarBtn_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/EditProfilePage.xaml", UriKind.Relative));
        }

        

        // Select a profile button
        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as Canvas).DataContext as Profile;
            //if (p.IsProtected && App.VM.IsLocked)
            //{
            //    MessageBoxResult res = MessageBox.Show(String.Format("Profile '{0}' is locked. Do you want to unlock the profile?", p.Name),
            //        "Profile locked", MessageBoxButton.OKCancel);
            //    if (res == MessageBoxResult.OK)
            //        NavigationService.Navigate(new Uri("/View/UnlockPage.xaml", UriKind.Relative));
            //}
            //else
            //{
                NavigationService.Navigate(new Uri(String.Format("/View/MeasurementsPage.xaml?Id={0}", p.Id), UriKind.Relative));
            //}
        }


        //private void unlockAppBarMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    if (App.VM.IsLocked)
        //    {
        //        NavigationService.Navigate(new Uri("/View/UnlockPage.xaml", UriKind.Relative));
        //    }
        //    else
        //    {
        //        App.VM.IsLocked = true;
        //        this.UpdateUILockState();
        //    }
        //}

        

        private void settingsAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            this.GoToSettingsPage();
        }

        private void settingsAppBarBtn_Click(object sender, System.EventArgs e)
        {
            this.GoToSettingsPage();
        }

        public void GoToSettingsPage()
        {
            NavigationService.Navigate(new Uri("/View/SettingsPage.xaml", UriKind.Relative));
        }


        public void lockedStateGroup_StateChanged(object sender, VisualStateChangedEventArgs e)
        {
            //for (Image im in this.profileWrapPanel.ItemTemplate
        }

        private void contactBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.EmailAuthor();
        }

        private void rateBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.RateApp();
        }

        private void legalBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewLicences();
        }

        private void websiteBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewWebsite();	
        }

        private void facebookBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.GotoFacebook();
        }

        private void feedbackBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.GiveFeedback();
        }

        //private void buyAppBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    App.VM.BuyApp();
        //}

        private void Pivot_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (this.mainPivot.SelectedIndex)
            {
                case 0:
                    this.ApplicationBar = (ApplicationBar)Resources["peopleApplicationBar"];
                    break;
                case 1:
                    this.ApplicationBar = (ApplicationBar)Resources["aboutApplicationBar"];
                    break;
                default:
                    break;
            }
        }

        


    }
}


