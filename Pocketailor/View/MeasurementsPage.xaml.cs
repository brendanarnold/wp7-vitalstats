﻿using System;
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
            int id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
            App.VM.SelectedProfile = (from Profile p in App.VM.Profiles where p.Id == id select p).First();

            
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
        private void customMeasurementTypeTextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditMeasurementPage.xaml?Action={0}",
                EditMeasurementPageActions.New), UriKind.Relative));
        }

        #endregion

        #region Other page behaviours

        
        // Delete a measurement from the context menu
        private void deleteContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (sender != null)
            {
                Measurement s = (sender as MenuItem).DataContext as Measurement;
                this.ConfirmAndDeleteMeasurement(s);
            }
        }

        private void editContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (sender != null)
            {
                Measurement s = (sender as MenuItem).DataContext as Measurement;
                App.VM.SelectedMeasurement = s;
                NavigationService.Navigate(new Uri(String.Format("/View/EditMeasurementPage.xaml?Action={0}", EditMeasurementPageActions.Edit), UriKind.Relative));
            }
        }

        //private void secondaryTileAppBarMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    SecondaryTileHelpers.CreateSecondaryTile(App.VM.SelectedProfile);
        //}



        // navigate to the edit profile page
        private void editProfileAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/EditProfilePage.xaml?Action={0}", EditProfilePageActions.Edit), UriKind.Relative));
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





        private void EditMeasurementFromTemplate(MeasurementId id)
        {
            MeasurementTemplate st = App.VM.MeasurementTemplates.Where(x => x.Id == id).First();
            App.VM.SelectedMeasurement = new Measurement()
            {
                Name = st.Name,
                MeasurementType = st.MeasurementType,
                MeasurementId = st.Id,
            };
            NavigationService.Navigate(new Uri(String.Format("/View/EditMeasurementPage.xaml?Action={0}",
                EditMeasurementPageActions.New), UriKind.Relative));
        }

        private void measurementsListBox_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            // Bit if a hack to get the conversion buttons to update when a measurement is added/removed
            App.VM.RefreshRequiredMeasurement();
        }


        private void PromptForMissingMeasurements(List<MeasurementId> missingIds, string conversionName)
        {
            string s = String.Empty;
            foreach (MeasurementId id in missingIds)
            {
                s += Environment.NewLine + "  \u2022 " +  Lookup.Measurements[id].ToLower();
            }
            string measurementName = Lookup.Measurements[missingIds[0]];
            string msg = String.Format("To calculate {0} conversions the following measurements need to be entered,{1}",
                conversionName, s);
            string title = String.Format("Add {0} measurement?", measurementName.ToLower());
            string btnTitle = String.Format("add {0} measurement", measurementName.ToLower());

            //CustomMessageBox cMb = new CustomMessageBox()
            //{
            //    Caption = title,
            //    Message = msg,
            //    LeftButtonContent = "add",
            //    RightButtonContent = "cancel",
            //    IsFullScreen = false,
            //};
            // Add this so have access in the callback
            //cMb.Tag = missingIds[0];

            //cMb.Dismissed += (s1, e) =>
            //{
            //    switch (e.Result)
            //    {
            //        case CustomMessageBoxResult.LeftButton:
            //            this.EditStatFromTemplate((MeasurementId)((s1 as CustomMessageBox).Tag));
            //            break;
            //        case CustomMessageBoxResult.RightButton:
            //        case CustomMessageBoxResult.None:
            //            break;
            //        default:
            //            break;
            //    }
            //};

            //cMb.Show();

            MessageBoxResult result = MessageBox.Show(msg, title, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                this.EditMeasurementFromTemplate(missingIds[0]);
            }
        }


        private void trouserConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            List<MeasurementId> missingMeasurements;
            if (App.VM.SelectedProfile.Gender == GenderId.Male)
            {
                missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.TrousersMens);
            }
            else
            {
                missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.TrousersWomens);
            
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
            List<MeasurementId> required = (App.VM.SelectedProfile.Gender == GenderId.Male) ? RequiredMeasurements.ShirtMens
                : RequiredMeasurements.ShirtWomens;
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
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.Hat);
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
            if (App.VM.SelectedProfile.Gender == Model.GenderId.Male)
            {
                missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.SuitMens);
            }
            else
            {
                missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.SuitWomens);
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
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.DressSize);
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
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.Bra);
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
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.Hosiery);
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
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.Shoes);
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
            List<MeasurementId> missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.SkiBoots);
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
            if (App.VM.SelectedProfile.Gender == Model.GenderId.Male)
            {
                missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.WetsuitMens);
            }
            else
            {
                missingMeasurements = App.VM.GetMissingMeasurements(RequiredMeasurements.WetsuitWomens);
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

        // Toggle the visibility
        private void measurementGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid g = sender as Grid;
            ListBox lb = g.FindName("otherUnitsListBox") as ListBox;
            if (lb.Visibility == Visibility.Visible) lb.Visibility = Visibility.Collapsed; else lb.Visibility = Visibility.Visible;
        }



        #endregion
















    }


}