using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Globalization;
using VitalStats.ViewModelNamespace;
using System.Windows.Controls;
using VitalStats.Model;
using System.Collections.Generic;
using Microsoft.Phone.Shell;

namespace VitalStats.View
{
    public partial class MainPage : PhoneApplicationPage
    {

        private ViewModel vm;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            vm = new ViewModel();

            this.ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["defaultAppBar"];
            this.addProfilePopUpStateGroup.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(addProfilePopUpStateGroup_CurrentStateChanged);
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            if (!ViewState.IsLaunching && PhoneApplicationService.Current.State.ContainsKey("Profiles"))
            {
                vm = (ViewModel)PhoneApplicationService.Current.State["Profiles"];
            }
            else
            {
                vm.GetProfiles();
            }

            //vm.AddNewProfile("Foo", true);
            profileListBox.ItemsSource = vm.Profiles;

            string action;
            if (NavigationContext.QueryString.TryGetValue("action", out action))
            {
                if (action == MainPage.UriActions.AddProfile)
                {
                    string s = "", name = "";
                    bool isProtected = false;
                    NavigationContext.QueryString.TryGetValue("name", out name);
                    NavigationContext.QueryString.TryGetValue("isProtected", out s);
                    bool.TryParse(s, out isProtected);
                    this.vm.AddNewProfile(name, isProtected);

                }
            }



        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (PhoneApplicationService.Current.State.ContainsKey("Profiles"))
            {
                PhoneApplicationService.Current.State["Profiles"] = vm;
            }
            else
            {
                PhoneApplicationService.Current.State.Add("Profiles", vm);
            }
            ViewState.IsLaunching = false;
        }

        private void deleteContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile profile = ((sender as MenuItem).DataContext) as Profile;
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Confirm delete",
                Message = String.Format("Are you sure you want to delete the profile for '{0}'?", profile.Name),
                LeftButtonContent = "Yes",
                RightButtonContent = "No",
                IsFullScreen = false
            };
            messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            
                            this.vm.DeleteProfile(profile);
                            break;
                        case CustomMessageBoxResult.RightButton:
                            // Do nothing
                            break;
                        default:
                            // Do nothing
                            break;
                    }

                };
            messageBox.Show();
        }

        #region AddProfilePopUp code

        private void addAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (this.addProfilePopUpStateGroup.CurrentState != this.addProfilePopUpOpen)
            {
                VisualStateManager.GoToState(this, "addProfilePopUpOpen", true);
            }
        }

        private void confirmAddBtn_Click(object sender, System.EventArgs e)
        {
            this.vm.AddNewProfile(this.nameTextBox.Text, (bool)this.isProtectedCheckBox.IsChecked);
            VisualStateManager.GoToState(this, "addProfilePopUpClosed", true);
            this.ClearAddPopUp();
        }

        private void ClearAddPopUp()
        {
            this.nameTextBox.Text = String.Empty;
            this.isProtectedCheckBox.IsChecked = false;
        }

        private void cancelAddBtn_Click(object sender, System.EventArgs e)
        {
            if (this.DataInPopUp())
            {
                this.ConfirmExitPopUp();
            }
            else
            {
                VisualStateManager.GoToState(this, "addProfilePopUpClosed", true);
            }
        }

        private bool DataInPopUp()
        {
            // Returns true if data on page that will need to be saved
            if (this.nameTextBox.Text != String.Empty)
                return true;
            if ((bool)this.isProtectedCheckBox.IsChecked)
                return true;
            return false;
        }
        private void ConfirmExitPopUp()
        {
            MessageBoxResult m = MessageBox.Show("You have entered some profile data which will be lost if you leave this page. Are you sure you want to leave this page?",
                "Confirm leave page", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                VisualStateManager.GoToState(this, "addProfilePopUpClosed", true);
                this.ClearAddPopUp();
            }
        }


  
        public void addProfilePopUpStateGroup_CurrentStateChanged(object sender, EventArgs e) 
        {
            if (this.addProfilePopUpStateGroup.CurrentState == this.addProfilePopUpOpen
                && this.ApplicationBar == (Microsoft.Phone.Shell.ApplicationBar)this.Resources["defaultAppBar"])
            {
                this.ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)this.Resources["addProfileAppBar"];
            }
            else if (this.addProfilePopUpStateGroup.CurrentState == this.addProfilePopUpClosed)
            {
                this.ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)this.Resources["defaultAppBar"];
            }
        }


        #endregion






        public static class UriActions
        {
            public static string AddProfile = "addprofile";
        }



        
    }




    // Convertor for the locked icon visibility
    public class BoolToVisibility : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
           return (Visibility)value == Visibility.Visible;
           
        }

    }

    // Convertor for profile tile text
    public class WordsOnNewlines : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string s = (string)value;
            return s.Replace(" ", System.Environment.NewLine);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return null;    
        }
    }

}