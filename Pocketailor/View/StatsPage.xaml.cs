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
using System.Globalization;
using Pocketailor.Model;

namespace Pocketailor.View
{
    public partial class StatsPage : PhoneApplicationPage
    {

        #region Initialise page

        public StatsPage()
        {
            InitializeComponent();

            this.newStatWizardStateGroup.CurrentStateChanged += newStatWizardStateGroup_CurrentStateChanged;
        }

        

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = App.VM;

            // Find the selected profile and assign it  
            int id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
            App.VM.SelectedProfile = (from Profile p in App.VM.Profiles where p.Id == id select p).First();

            // Remove popups
            if (e.IsNavigationInitiator)
            {
                VisualStateManager.GoToState(this, "VSDefault", false);
                this.ApplicationBar.IsVisible = true; // For some reason the CurrentStatChanged event is not fired
            }


            // Load a new suggested app
            App.VM.LoadNextSuggestedStatTemplate();

        }

        #endregion


        private void ConfirmAndDeleteStat(Stat s)
        {
            MessageBoxResult res = MessageBox.Show(String.Format("Are you sure you want to delete the statistic '{0}'?", s.Name), "Delete stat?", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                App.VM.DeleteStatFromProfile(s, App.VM.SelectedProfile);
            }
        }



        #region New stat behaviours

        // User selected a suggested stat template, can move straight to new stat page
        private void suggestedStatGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedStat = new Stat()
            {
                Name = App.VM.SuggestedStatTemplate.Name,
                MeasurementType = App.VM.SuggestedStatTemplate.MeasurementType,
                PreferredUnit = App.VM.SuggestedStatTemplate.MeasurementType.DefaultUnit,
                Value = String.Empty,
            };
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}",
                EditStatPageActions.New), UriKind.Relative));
        }

        // User chose to create a new stat, should be prompted to select a stat template or custom
        private void addStatAppBarBtn_Click(Object sender, EventArgs e) 
        {
            App.VM.SelectedStat = new Stat()
            {
                Name = String.Empty,
                PreferredUnit = null,
                Value = String.Empty,
                MeasurementType = null,
            };
            VisualStateManager.GoToState(this, "VSSelectStatType", false);
        }

        // User has selected a stat template from the popup, can go to new stat page
        private void selectStatTemplateStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Stat s = (sender as StackPanel).DataContext as Stat;
            App.VM.SelectedStat.Name = s.Name;
            App.VM.SelectedStat.MeasurementType = s.MeasurementType;
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}",
                EditStatPageActions.New), UriKind.Relative));
        }

        // User has declined a stat template and instead selected a custom stat, need to select the measurement type
        private void customStatTextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            VisualStateManager.GoToState(this, "VSSelectMeasurementType", false);
        }

        // User has selected a measurement type from the popup, can go to the new stat page
        private void selectMeasurementTypeStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MeasurementType mt = (sender as StackPanel).DataContext as MeasurementType;
            App.VM.SelectedStat.MeasurementType = mt;
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}",
                EditStatPageActions.New), UriKind.Relative));
        }

        // User declined to select a measurement type, custom measurement type has been selected instead, move on to new stat page
        private void customMeasurementTypeTextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}",
                EditStatPageActions.New), UriKind.Relative));
        }

        #endregion

        #region Other page behaviours

        // Delete a stat from the context menu
        private void deleteContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (sender != null)
            {
                Stat s = (sender as MenuItem).DataContext as Stat;
                this.ConfirmAndDeleteStat(s);
            }
        }

        // Select as a stat and show the popup
        private void stat_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedStat = ((sender as StackPanel).DataContext as Stat);
            NavigationService.Navigate(new Uri("/View/StatDetailPage.xaml", UriKind.Relative));
        }

        // navigate to the edit profile page
        private void editProfileAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}", EditProfilePageActions.Edit), UriKind.Relative));
        }


        // Exit the new stat wizard as would expect with the back button
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (this.newStatWizardStateGroup.CurrentState == this.VSSelectMeasurementType)
            {
                VisualStateManager.GoToState(this, "VSSelectStatType", false);
                e.Cancel = true;
            }
            else if (this.newStatWizardStateGroup.CurrentState == this.VSSelectStatType)
            {
                VisualStateManager.GoToState(this, "VSDefault", false);
                e.Cancel = true;
            }
        }

        // Update the menubar buttons when the new stat popup menus are shown/hidden
        void newStatWizardStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            this.ApplicationBar.IsVisible = (e.NewState == this.VSDefault);
        }


        #endregion
















    }


}
