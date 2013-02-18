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

namespace Pocketailor
{
    public partial class EditProfilePage : PhoneApplicationPage
    {

        public string Action;

        public EditProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("Action"))
                this.Action = NavigationContext.QueryString["Action"];
            else
                this.Action = EditProfilePageActions.New;

            if (this.Action != EditProfilePageActions.Edit)
            {
                App.VM.SelectedProfile = new Profile() { Name = String.Empty, Gender = Gender.Unspecified, IsProtected = false, IsQuickProfile = false };
            }
            this.nameTextBox.Text = App.VM.SelectedProfile.Name;
            this.isProtectedCheckBox.IsChecked = App.VM.SelectedProfile.IsProtected;
            this.IsQuickListCheckBox.IsChecked = App.VM.SelectedProfile.IsQuickProfile;
            this.SetSelectedGender(App.VM.SelectedProfile.Gender);
                
        }

        private void SetSelectedGender(Model.Gender gender)
        {
            if (gender == Model.Gender.Unspecified) this.unspecifiedRadioBtn.IsChecked = true;
            if (gender == Model.Gender.Female) this.femaleRadioBtn.IsChecked = true;
            if (gender == Model.Gender.Male) this.maleRadioBtn.IsChecked = true;
        }


        private void saveBtn_Click(object sender, System.EventArgs e)
        {
            App.VM.SelectedProfile.Name = this.nameTextBox.Text;
            App.VM.SelectedProfile.IsProtected = (bool)this.isProtectedCheckBox.IsChecked;
            App.VM.SelectedProfile.IsQuickProfile = (bool)this.IsQuickListCheckBox.IsChecked;
            App.VM.SelectedProfile.Gender = this.GetSelectedGender();

            if (this.Action == EditProfilePageActions.New)
            {
                App.VM.AddProfile(App.VM.SelectedProfile);
            }
            else
            {
                App.VM.UpdateProfile(App.VM.SelectedProfile);
            }
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

            // Returns true if data has been changed
            if (this.nameTextBox.Text != App.VM.SelectedProfile.Name)
                return true;
            if ((bool)this.isProtectedCheckBox.IsChecked != App.VM.SelectedProfile.IsProtected)
                return true;
            if (this.GetSelectedGender() != App.VM.SelectedProfile.Gender)
                return true;
            if ((bool)this.IsQuickListCheckBox.IsChecked != App.VM.SelectedProfile.IsQuickProfile)
                return true;
            return false;
        }

        private void ClearAddPopUp()
        {
            this.nameTextBox.Text = String.Empty;
            this.isProtectedCheckBox.IsChecked = false;
            this.IsQuickListCheckBox.IsChecked = false;
            this.unspecifiedRadioBtn.IsChecked = true;
        }


        private Model.Gender GetSelectedGender()
        {
            // n.b. have to use verbose form of conditional since IsChecked is nullable
            if (this.unspecifiedRadioBtn.IsChecked == true) return Model.Gender.Unspecified;
            if (this.femaleRadioBtn.IsChecked == true) return Model.Gender.Female;
            if (this.maleRadioBtn.IsChecked == true) return Model.Gender.Male;
            return Model.Gender.Unspecified;
        }


    }

    public static class EditProfilePageActions
    {
        public const string New = "New";
        public const string Edit = "Edit";
    }


}
