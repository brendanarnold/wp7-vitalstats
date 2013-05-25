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

namespace Pocketailor
{
    public partial class EditProfilePage : PhoneApplicationPage
    {

        public EditProfilePageState State;

        public string Action;

        public EditProfilePage()
        {
            InitializeComponent();
            this.State = new EditProfilePageState();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("Action"))
                this.Action = NavigationContext.QueryString["Action"];
            else
                this.Action = EditProfilePageActions.New;

            if (NavigationContext.QueryString.ContainsKey("ProfileId"))
            {
                string profileIdStr = NavigationContext.QueryString["ProfileId"];
                int profileId = System.Int32.Parse(profileIdStr);
                if (App.VM.SelectedProfile == null || App.VM.SelectedProfile.Id != profileId)
                    App.VM.SelectedProfile = (from Profile p in App.VM.Profiles where p.Id == profileId select p).FirstOrDefault();
            }
            if (this.Action == EditProfilePageActions.Edit)
            {
                this.titleTextBlock.Text = "edit profile";
            } 
            else 
            {
                this.titleTextBlock.Text = "add profile";
                App.VM.SelectedProfile = new Profile() { Name = String.Empty, Gender = GenderId.Female, IsQuickProfile = false };
            }

            if (!e.IsNavigationInitiator && PhoneApplicationService.Current.State.ContainsKey("EditProfilePageState"))
            {
                this.State = (EditProfilePageState)PhoneApplicationService.Current.State["EditProfilePageState"];
                this.LoadFromState();
            }
            else
            {
                this.LoadFromSelectedProfile();
            }
                
        }


        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!e.IsNavigationInitiator)
            {
                this.TakeSnapshotState();
                if (PhoneApplicationService.Current.State.ContainsKey("EditProfilePageState"))
                {
                    PhoneApplicationService.Current.State["EditProfilePageState"] = this.State;
                }
                else
                {
                    PhoneApplicationService.Current.State.Add("EditProfilePageState", this.State);
                }
            }
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (this.IsUnsavedData())
            {
                MessageBoxResult m = MessageBox.Show("You have entered some profile data which will be lost if you leave this page. Are you sure you want to leave this page?",
                "Confirm leave page", MessageBoxButton.OKCancel);
                if (m == MessageBoxResult.OK)
                {
                    this.ResetPage();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        public void TakeSnapshotState()
        {
            this.State.Name = this.nameTextBox.Text;
            this.State.IsQuickProfile = this.IsQuickListCheckBox.IsChecked;
            this.State.Gender = this.GetSelectedGender();
        }

        public void LoadFromState()
        {
            this.nameTextBox.Text = this.State.Name;
            this.IsQuickListCheckBox.IsChecked = this.State.IsQuickProfile;
            this.SetSelectedGender(this.State.Gender);
        }

        public void LoadFromSelectedProfile()
        {
            this.nameTextBox.Text = App.VM.SelectedProfile.Name;
            this.IsQuickListCheckBox.IsChecked = App.VM.SelectedProfile.IsQuickProfile;
            this.SetSelectedGender(App.VM.SelectedProfile.Gender);
        }


        private void SetSelectedGender(Model.GenderId gender)
        {
            if (gender == Model.GenderId.Female) this.femaleRadioBtn.IsChecked = true;
            if (gender == Model.GenderId.Male) this.maleRadioBtn.IsChecked = true;
        }

        private Model.GenderId GetSelectedGender()
        {
            // n.b. have to use verbose form of conditional since IsChecked is nullable
            //if (this.unspecifiedRadioBtn.IsChecked == true) return Model.Gender.Unspecified;
            if (this.femaleRadioBtn.IsChecked == true) return Model.GenderId.Female;
            if (this.maleRadioBtn.IsChecked == true) return Model.GenderId.Male;
            return Model.GenderId.Unspecified;
        }


        private void saveBtn_Click(object sender, System.EventArgs e)
        {
            this.TakeSnapshotState();
            App.VM.SelectedProfile.Name = this.State.Name;
            App.VM.SelectedProfile.IsQuickProfile = (bool)this.State.IsQuickProfile;
            App.VM.SelectedProfile.Gender = this.State.Gender;

            if (this.Action == EditProfilePageActions.New)
            {
                App.VM.AddProfile(App.VM.SelectedProfile);
            }
            else
            {
                App.VM.UpdateProfile(App.VM.SelectedProfile);
            }
            this.ResetPage();
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }


        
        private bool IsUnsavedData()
        {
            this.TakeSnapshotState();
            if (this.State.Name != App.VM.SelectedProfile.Name) return true;
            if (this.State.IsQuickProfile != App.VM.SelectedProfile.IsQuickProfile) return true;
            if (this.State.Gender != App.VM.SelectedProfile.Gender) return true;
            return false;
        }

        private void ResetPage()
        {
            this.nameTextBox.Text = String.Empty;
            this.IsQuickListCheckBox.IsChecked = false;
            this.femaleRadioBtn.IsChecked = true;
        }



    }

    public static class EditProfilePageActions
    {
        public const string New = "New";
        public const string Edit = "Edit";
    }

    public class EditProfilePageState
    {
        public string Name;
        public GenderId Gender;
        public bool? IsQuickProfile;
    }


}
