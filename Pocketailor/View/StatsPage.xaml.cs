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
using Microsoft.Phone.Shell;

namespace Pocketailor.View
{
    public partial class StatsPage : PhoneApplicationPage
    {

        #region Initialise page

        public StatsPage()
        {
            InitializeComponent();

           

            //this.newStatWizardStateGroup.CurrentStateChanged += newStatWizardStateGroup_CurrentStateChanged;
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
                //VisualStateManager.GoToState(this, "VSDefault", false);
                
                //this.ApplicationBar.IsVisible = true; // For some reason the CurrentStatChanged event is not fired
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
        

        private void changeRegionsAppBarBtn_Click(Object sender, EventArgs e) 
        {
            NavigationService.Navigate(new Uri("/View/EditRegionPage.xaml", UriKind.Relative));
        }
        // User has selected a stat template from the popup, can go to new stat page
        private void selectStatTemplateStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StatTemplate s = (sender as StackPanel).DataContext as StatTemplate;
            this.EditStatFromTemplate(s.Id);
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
            //if (this.newStatWizardStateGroup.CurrentState == this.VSSelectMeasurementType)
            //{
            //    VisualStateManager.GoToState(this, "VSSelectStatType", false);
            //    e.Cancel = true;
            //}
            //else if (this.newStatWizardStateGroup.CurrentState == this.VSSelectStatType)
            //{
            //    VisualStateManager.GoToState(this, "VSDefault", false);
            //    e.Cancel = true;
            //}
        }

        // Update the menubar buttons when the new stat popup menus are shown/hidden
        void newStatWizardStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            //this.ApplicationBar.IsVisible = (e.NewState == this.VSDefault);
        }

        private void Pivot_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch ((sender as Pivot).SelectedIndex)
            {
                case 0:
                    this.ApplicationBar = this.Resources["conversionsAppBar"] as ApplicationBar;
                    break;
                case 1:
                    this.ApplicationBar = this.Resources["measurementsAppBar"] as ApplicationBar;
                    break;
                default:
                    break;
            }
        }





        private void EditStatFromTemplate(MeasurementId id)
        {
            StatTemplate st = App.VM.StatTemplates.Where(x => x.Id == id).First();
            App.VM.SelectedStat = new Stat()
            {
                Name = st.Name,
                MeasurementType = st.MeasurementType,
                MeasurementId = st.Id,
            };
            NavigationService.Navigate(new Uri(String.Format("/View/EditStatPage.xaml?Action={0}",
                EditStatPageActions.New), UriKind.Relative));
        }

        private void statsListBox_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
        	// Bit if a hack to get the conversion buttons to update when a stat is added/removed
            App.VM.RefreshRequiredMeasurement();
        }


        private void PromptForMissingMeasurements(List<MeasurementId> missingIds, string conversionName)
        {
            string s = String.Empty;
            foreach (MeasurementId id in missingIds)
            {
                // TODO: Include a proper lookup for this
                s += Environment.NewLine + "  \u2022 " + id.ToString();
            }
            string measurementName = missingIds[0].ToString();
            string msg = String.Format("To calculate {0} conversions the following measurements need to be entered,{1}",
                conversionName, s);
            string title = String.Format("Add {0} measurement?", measurementName.ToLower());
            MessageBoxResult result = MessageBox.Show(msg, title, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                this.EditStatFromTemplate(missingIds[0]);
            }
        }


        private void trouserConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements;
            if (App.VM.SelectedProfile.Gender == Gender.Male)
            {
                missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.TrousersUtils.RequiredMeasurementsMens);
            }
            else
            {
                missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.TrousersUtils.RequiredMeasurementsMens);
            
            }
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.TrouserSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "trouser");
            }
        }

        private void shirtConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> required = (App.VM.SelectedProfile.Gender == Gender.Male) ? Model.Conversions.ShirtUtils.RequiredMeasurementsMens
                : Model.Conversions.ShirtUtils.RequiredMeasurementsWomens;
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(required);
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.ShirtSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "shirt");
            }
        }

        private void hatConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.HatUtils.RequiredMeasurements);
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.HatSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "hat");
            }
        }

        private void suitConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements;
            if (App.VM.SelectedProfile.Gender == Model.Gender.Male)
            {
                missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.SuitUtils.RequiredMeasurementsMens);
            }
            else
            {
                missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.SuitUtils.RequiredMeasurementsWomens);
            }
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.SuitSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "suit");
            }
        }

        private void dressConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.DressSizeUtils.RequiredMeasurements);
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.DressSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "dress size");
            }
        }

        private void braConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.BraUtils.RequiredMeasurements);
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.BraSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "bra");
            }
        }

        private void hosieryConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.HosieryUtils.RequiredMeasurements);
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.HosierySize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "hosiery");
            }
        }

        private void shoeConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.ShoesUtils.RequiredMeasurements);
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.ShoeSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "shoe");
            }
        }

        private void skiBootConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.SkiBootsUtils.RequiredMeasurements);
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.SkiBootSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "ski boot");
            }
        }

        //private void tennisGripBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.TennisRaquetSizesUtils.RequiredMeasurements);
        //    if (missingMeasurements.Count == 0)
        //    {
        //        App.VM.SelectedConversionType = ConversionId.TennisGripSize;
        //        NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
        //    }
        //    else
        //    {
        //        this.PromptForMissingMeasurements(missingMeasurements, "tennis grip");
        //    }
        //}

        private void wetsuitConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements;
            if (App.VM.SelectedProfile.Gender == Model.Gender.Male)
            {
                missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.WetsuitUtils.RequiredMeasurementsMens);
            }
            else
            {
                missingMeasurements = App.VM.GetMissingMeasurements(Model.Conversions.WetsuitUtils.RequiredMeasurementsWomens);
            }
            if (missingMeasurements.Count == 0)
            {
                App.VM.SelectedConversionType = ConversionId.WetsuitSize;
                NavigationService.Navigate(new Uri("/View/ConversionsPage.xaml", UriKind.Relative));
            }
            else
            {
                this.PromptForMissingMeasurements(missingMeasurements, "wetsuit");
            }
        }



        #endregion
















    }


}
