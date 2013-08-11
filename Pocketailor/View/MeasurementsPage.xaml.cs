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
using Pocketailor.ViewModel;
using Pocketailor.View.Controls;

namespace Pocketailor.View
{
    public partial class MeasurementsPage : PhoneApplicationPage
    {

        #region Initialise page

        public MeasurementsPage()
        {
            InitializeComponent();
            
        }

        

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = App.VM;

            // Find the selected profile and assign it  
            int id = Convert.ToInt32(NavigationContext.QueryString["ProfileId"]);
            App.VM.LoadMeasurementsPageData(id);
            App.VM.RefreshRequiredMeasurement();

        }

        #endregion


        private void ConfirmAndDeleteMeasurement(Measurement s)
        {
            MessageBoxResult res = MessageBox.Show(String.Format("Are you sure you want to delete the measurement '{0}'?", s.Name), "Delete measurement?", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                App.VM.DeleteMeasurementsFromProfile(s, App.VM.SelectedProfile);
            }
        }



        #region New Measurement behaviours


        private void changeRegionsAppBarBtn_Click(Object sender, EventArgs e) 
        {
            NavigationService.Navigate(new Uri("/View/EditRegionPage.xaml", UriKind.Relative));
        }


        // User declined to select a measurement type, custom measurement type has been selected instead, move on to new measurement page
        //private void customMeasurementTypeTextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    NavigationService.Navigate(new Uri(String.Format("/View/EditMeasurementPage.xaml?Action={0}",
        //        EditMeasurementPageActions.New), UriKind.Relative));
        //}

        #endregion

        #region Other page behaviours

        
        // Delete a measurement from the context menu
        //private void deleteContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    if (sender != null)
        //    {
        //        Measurement s = (sender as MenuItem).DataContext as Measurement;
        //        this.ConfirmAndDeleteMeasurement(s);
        //    }
        //}

        //private void editContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    if (sender != null)
        //    {
        //        Measurement s = (sender as MenuItem).DataContext as Measurement;
        //        App.VM.SelectedMeasurement = s;
        //        NavigationService.Navigate(new Uri(String.Format("/View/EditMeasurementPage.xaml?Action={0}", EditMeasurementPageActions.Edit), UriKind.Relative));
        //    }
        //}

        //private void secondaryTileAppBarMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    SecondaryTileHelpers.CreateSecondaryTile(App.VM.SelectedProfile);
        //}



        // navigate to the edit profile page
        private void editProfileAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}&ProfileId={1}", EditProfilePageActions.Edit, App.VM.SelectedProfile.Id), UriKind.Relative));
        }


        private void Pivot_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //switch ((sender as Pivot).SelectedIndex)
            //{
            //    case 0:
            //        this.ApplicationBar = this.Resources["conversionsAppBar"] as ApplicationBar;
            //        break;
            //    case 1:
            //        this.ApplicationBar = this.Resources["measurementsAppBar"] as ApplicationBar;
            //        break;
            //    default:
            //        break;
            //}
        }





        //private void EditMeasurementFromTemplate(MeasurementId id)
        //{
        //    MeasurementTemplate st = App.VM.MeasurementTemplates.Where(x => x.Id == id).First();
        //    App.VM.SelectedMeasurement = new Measurement()
        //    {
        //        Name = st.Name,
        //        MeasurementType = st.MeasurementType,
        //        MeasurementId = st.Id,
        //    };
        //    NavigationService.Navigate(new Uri(String.Format("/View/EditMeasurementPage.xaml?Action={0}",
        //        EditMeasurementPageActions.New), UriKind.Relative));
        //}

        


        //private void PromptForMissingMeasurements(List<MeasurementId> missingIds, string conversionName)
        //{
        //    string s = String.Empty;
        //    foreach (MeasurementId id in missingIds)
        //    {
        //        s += Environment.NewLine + "  \u2022 " +  Lookup.Measurements[id].ToLower();
        //    }
        //    string measurementName = Lookup.Measurements[missingIds[0]];
        //    string msg = String.Format("To calculate {0} conversions the following measurements need to be entered,{1}",
        //        conversionName, s);
        //    string title = String.Format("Add {0} measurement?", measurementName.ToLower());
        //    string btnTitle = String.Format("add {0} measurement", measurementName.ToLower());

            
        //    MessageBoxResult result = MessageBox.Show(msg, title, MessageBoxButton.OKCancel);
        //    if (result == MessageBoxResult.OK)
        //    {
        //        this.EditMeasurementFromTemplate(missingIds[0]);
        //    }
        //}

        private void JumpToMeasurements()
        {
            this.mainPivot.SelectedIndex = 1;
        }


        private void trouserConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.TrouserConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.TrouserSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.TrouserConversion.MissingMeasurements);
            }

        }

        private void shirtConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.ShirtConversion.HasRequiredMeasurements) {
                App.VM.SelectedConversionType = ConversionId.ShirtSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.ShirtConversion.MissingMeasurements);
            }
        }

        private void hatConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.HatConversion.HasRequiredMeasurements) 
            {
                App.VM.SelectedConversionType = ConversionId.HatSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.HatConversion.MissingMeasurements);
            }
        }

        private void suitConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.SuitConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.SuitSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.SuitConversion.MissingMeasurements);
            }
        }

        private void dressConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.DressConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.DressSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.DressConversion.MissingMeasurements);
            }
        }

        private void braConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.BraConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.BraSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.BraConversion.MissingMeasurements);
            }
        }

        private void hosieryConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.HosieryConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.HosierySize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.HosieryConversion.MissingMeasurements);
            }
        }

        private void shoeConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.ShoeConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.ShoeSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.ShoeConversion.MissingMeasurements);
            }
        }

        private void skiBootConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.SkiBootConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.SkiBootSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.SkiBootConversion.MissingMeasurements);
            }
        }


        private void wetsuitConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.WetsuitConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.WetsuitSize;
                NavigationService.Navigate(new Uri(String.Format("/View/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                this.JumpToMeasurements();
                App.VM.NominateRequiredMeasurements(App.VM.WetsuitConversion.MissingMeasurements);
            }
        }

        // Toggle the visibility
        private void measurementGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid g = sender as Grid;
            ListBox lb = g.FindName("otherUnitsListBox") as ListBox;
            if (lb.Visibility == Visibility.Visible) lb.Visibility = Visibility.Collapsed; else lb.Visibility = Visibility.Visible;
        }

        private void switchUnitsBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.ViewingUnitCulture == UnitCultureId.Imperial) 
            {
                App.VM.ViewingUnitCulture = UnitCultureId.Metric;
            } else 
            {
                App.VM.ViewingUnitCulture = UnitCultureId.Imperial;
            }
            App.VM.LoadMeasurements(App.VM.ViewingUnitCulture);
        }

		private void measurementBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e) 
		{
            MeasurementDataContainer m = (sender as MeasurementBtn).DataContext as MeasurementDataContainer;
            int profileId = App.VM.SelectedProfile.Id;
            NavigationService.Navigate(new Uri(String.Format("/View/EditMeasurementPage.xaml?MeasurementId={0}&ProfileId={1}",
                m.MeasurementId, profileId), UriKind.Relative));
		}

        #endregion
















    }


}
