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
using VitalStats.Model;

namespace VitalStats
{
    public partial class EditProfilePage : PhoneApplicationPage
    {

        public bool IsNewProfile = true;

        public EditProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("Action"))
            {
                if (NavigationContext.QueryString["Action"] == EditProfilePageActions.Edit)
                {
                    int id;
                    int.TryParse(NavigationContext.QueryString["Id"], out id);
                    App.VM.SelectedProfile = (from Profile p in App.VM.Profiles where p.Id == id select p).First();
                    this.nameTextBox.Text = App.VM.SelectedProfile.Name;
                    this.isProtectedCheckBox.IsChecked = App.VM.SelectedProfile.IsProtected;
                    this.IsNewProfile = false;
                }
            }
        }


        private void saveBtn_Click(object sender, System.EventArgs e)
        {
            if (this.IsNewProfile)
            {
                App.VM.AddProfile(new Profile() { Name = this.nameTextBox.Text, IsProtected = (bool)this.isProtectedCheckBox.IsChecked });
            }
            else
            {
                App.VM.SelectedProfile.Name = this.nameTextBox.Text;
                App.VM.SelectedProfile.IsProtected = (bool)this.isProtectedCheckBox.IsChecked;
            }
            App.VM.SaveChangesToDB();
            this.ClearAddPopUp();
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void cancelAddBtn_Click(object sender, System.EventArgs e)
        {
            if (this.DataChanged())
            {
                this.ConfirmExitPopUp();
            }
            else
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
 
        }

        private void ConfirmExitPopUp()
        {
            MessageBoxResult m = MessageBox.Show("You have entered some profile data which will be lost if you leave this page. Are you sure you want to leave this page?",
                "Confirm leave page", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                this.ClearAddPopUp();
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }

        private bool DataChanged()
        {
            if (this.IsNewProfile)
            {
                // Returns true if data on page that will need to be saved
                if (this.nameTextBox.Text != String.Empty)
                    return true;
                if ((bool)this.isProtectedCheckBox.IsChecked)
                    return true;
                return false;
            }
            else
            {
                // Returns true if data has been changed from the existing profile
                if (this.nameTextBox.Text != App.VM.SelectedProfile.Name)
                    return true;
                if ((bool)this.isProtectedCheckBox.IsChecked != App.VM.SelectedProfile.IsProtected)
                    return true;
                return false;
            }
        }

        private void ClearAddPopUp()
        {
            this.nameTextBox.Text = String.Empty;
            this.isProtectedCheckBox.IsChecked = false;
        }

    }

    public static class EditProfilePageActions
    {
        public const string New = "New";
        public const string Edit = "Edit";
    }


}
