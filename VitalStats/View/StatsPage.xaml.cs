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



        #region Page behaviours

        private void addStatAppBarBtn_Click(Object sender, EventArgs e) 
        {
            App.VM.SelectedStat = new Stat()
            {
                Name = String.Empty,
                PreferredUnit = null,
                Value = String.Empty,
                MeasurementType = null,
            };
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}", 
                EditStatPageActions.New), UriKind.Relative));
        }

        private void suggestedStatGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedStat = new Stat()
            {
                Name = App.VM.SuggestedStatTemplate.Name,
                MeasurementType = App.VM.SuggestedStatTemplate.MeasurementType,
                PreferredUnit = App.VM.SuggestedStatTemplate.PreferredUnit,
                Value = String.Empty,
            };
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}",
                EditStatPageActions.New), UriKind.Relative));
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


        private void stat_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedStat = ((sender as StackPanel).DataContext as Stat);
        }

        private void editProfileAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void selectStatTemplateStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
			// Open up the edit new stat page
        }

        private void customStatTextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
			// Open up pop-up to select measurement type
        }








    }


}
