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
using VitalStats.Model;

namespace VitalStats.View
{
    public partial class StatsPage : PhoneApplicationPage
    {

        #region Initialise page

        public StatsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = App.VM;

            // Find the selected profile and assign it  
            int id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
            App.VM.SelectedProfile = (from Profile p in App.VM.Profiles where p.Id == id select p).First();
 
            // Bind appbar
            this.ApplyApplicationBar(statDetailPopUpStateGroup.CurrentState);
            
            // Load a new suggested app
            App.VM.LoadNextSuggestedStatTemplate();

            // Bind an event to switch the application bar
            this.statDetailPopUpStateGroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(statDetailPopUpStateGroup_CurrentStateChanging);

        }

        #endregion


        #region Misc methods

        private void ConfirmAndDeleteStat(Stat s)
        {
            MessageBoxResult res = MessageBox.Show(String.Format("Are you sure you want to delete the statistic '{0}'?", s.Name), "Delete stat?", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                App.VM.DeleteStatFromProfile(s, App.VM.SelectedProfile);
            }
        }


        private void ApplyApplicationBar(VisualState vs)
        {
            if ((vs == this.statDetailPopUpOpen) &&
                (this.ApplicationBar != (Microsoft.Phone.Shell.ApplicationBar)this.Resources["showStatAppBar"]))
            {
                this.ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)this.Resources["showStatAppBar"];
            }
            else if (((vs == this.statDetailPopUpClosed) || (vs == null)) &&
                (this.ApplicationBar != (Microsoft.Phone.Shell.ApplicationBar)this.Resources["defaultAppBar"]))
            {
                this.ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)this.Resources["defaultAppBar"];
            }
        }

        #endregion


        #region Page behaviours

        private void addStatAppBarBtn_Click(Object sender, EventArgs e) 
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}", 
                EditStatPageActions.New), UriKind.Relative));
        }

        private void editProfileAppBarBtn_Click(Object sender, EventArgs e)
        {
            
        }

        private void editStatAppBarBtn_Click(Object sender, EventArgs e) 
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}&Id={1}",
                EditStatPageActions.Edit, App.VM.SelectedStat.Id), UriKind.Relative));
        }


        private void suggestedStatGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}&Id={1}",
                EditStatPageActions.NewFromTemplate, App.VM.SuggestedStatTemplate.Id), UriKind.Relative));
        }

        private void deleteContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (sender != null)
            {
                Stat s = (sender as MenuItem).DataContext as Stat;
                this.ConfirmAndDeleteStat(s);
            }
        }

        #endregion


        #region Pop-up behaviours

        void statDetailPopUpStateGroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            this.ApplyApplicationBar(e.NewState);
        }

        // Simulate page back behaviour using the Back button for the pop up
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (this.statDetailPopUpStateGroup.CurrentState == this.statDetailPopUpOpen)
            {
                e.Cancel = true;
                VisualStateManager.GoToState(this, "statDetailPopUpClosed", true);
            }
        }

        private void stat_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedStat = ((sender as StackPanel).DataContext as Stat);
            VisualStateManager.GoToState(this, "statDetailPopUpOpen", true);
        }

        private void statDetailGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            VisualStateManager.GoToState(this, "statDetailPopUpClosed", true);
        }

        #endregion



    }


}
