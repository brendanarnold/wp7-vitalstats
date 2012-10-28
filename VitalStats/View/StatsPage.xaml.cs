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
            this.ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["defaultAppBar"];

        }


        #region Show stat detail pop-up

        // Simulate page back behaviour using the Back button for the pop up
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (this.statDetailPopUpStateGroup.CurrentState == this.statDetailPopUpOpen)
            {
                e.Cancel = true;
                VisualStateManager.GoToState(this, "addProfilePopUpClosed", true);
            }
        }



        #endregion



        private void addStatAppBarBtn_Click(Object sender, EventArgs e) 
        {
            // Launch app editing code
        }

        private void editProfileAppBarBtn_Click(Object sender, EventArgs e)
        {
            // Launch profile editing code
        }

        private void editStatAppBarBtn_Click(Object sender, EventArgs e) 
        {
        }

        private void statStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            VisualStateManager.GoToState(this, "statDetailPopUpOpen", true);
        }
        


    }


}
