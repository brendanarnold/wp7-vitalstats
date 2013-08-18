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


        public MeasurementsPage()
        {
            InitializeComponent();



            this.hideNeededMeasurementsHelpStoryBoard.Completed += (s, e) =>
            {
                if (App.VM.NewlyUnlockedConversions == null)
                    this.toastHelpContainer.Visibility = Visibility.Collapsed;
            };

            this.hideNewlyUnlockedHelpStoryBoard.Completed += (s, e) =>
            {
                if (App.VM.CurrentNominatedConversion == null)
                    this.toastHelpContainer.Visibility = Visibility.Collapsed;
            };
            


            
        }

        #region Animation gayness

        public void ShowNeededMeasurementsHelp()
        {
            this.toastHelpContainer.Visibility = Visibility.Visible;
            this.showNeededMeasurementsHelpStoryBoard.Begin();
        }

        public void HideNeededMeasurementsHelp()
        {
            this.hideNeededMeasurementsHelpStoryBoard.Begin();
        }

        public void ShowNewlyUnlockedConversionsHelp()
        {
            this.toastHelpContainer.Visibility = Visibility.Visible;
            this.showNewlyUnlockedHelpStoryBoard.Begin();
        }

        public void HideNewlyUnlockedConversionsHelp()
        {
            this.hideNewlyUnlockedHelpStoryBoard.Begin();
        }

        #endregion





        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = App.VM;

            // Find the selected profile and assign it  
            int id = Convert.ToInt32(NavigationContext.QueryString["ProfileId"]);
            App.VM.LoadMeasurementsPageData(id);
            //App.VM.RefreshPostMeasurementEdit();

        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Switch off the NewlyUnlockedAnimations
            if (e.IsNavigationInitiator)
            {
                App.VM.CancelNewlyUnlocked();
            }
            // Switch off NominatedConversion
            if (e.IsNavigationInitiator && !e.Uri.ToString().StartsWith("/View/Pages/EditMeasurementPage.xaml"))
            {
                App.VM.UnNominateConversion();
            }

        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (App.VM.CurrentNominatedConversion != null)
            {
                e.Cancel = true;
                App.VM.UnNominateConversion();
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }


        private void ConfirmAndDeleteMeasurement(Measurement s)
        {
            MessageBoxResult res = MessageBox.Show(String.Format("Are you sure you want to delete the measurement '{0}'?", s.Name), "Delete measurement?", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                App.VM.DeleteMeasurementFromProfile(s, App.VM.SelectedProfile);
            }
        }




        private void changeRegionsAppBarBtn_Click(Object sender, EventArgs e) 
        {
            NavigationService.Navigate(new Uri("/View/Pages/EditRegionPage.xaml", UriKind.Relative));
        }

        
        //private void secondaryTileAppBarMenuItem_Click(object sender, System.EventArgs e)
        //{
        //    SecondaryTileHelpers.CreateSecondaryTile(App.VM.SelectedProfile);
        //}



        // navigate to the edit profile page
        private void editProfileAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/Pages/EditProfilePage.xaml?Action={0}&ProfileId={1}", EditProfilePageActions.Edit, App.VM.SelectedProfile.Id), UriKind.Relative));
        }


        private void Pivot_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (App.VM.PendingCurrentNominatedConversion.HasValue)
            {
                // Activate animation through binding
                App.VM.CurrentNominatedConversion = App.VM.PendingCurrentNominatedConversion;
                App.VM.PendingCurrentNominatedConversion = null;
            }

            if (this.ScrollDownAfterPivotJump.HasValue)
            {
                this.conversionsScrollViewer.ScrollToVerticalOffset((int)this.ScrollDownAfterPivotJump);
                this.ScrollDownAfterPivotJump = null;
            }


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





       


        private void JumpToMeasurements()
        {
            this.mainPivot.SelectedIndex = 1;
        }

        private void profileNameBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Format("/View/Pages/EditProfilePage.xaml?ProfileId={0}&Action={1}", 
                App.VM.SelectedProfile.Id, EditProfilePageActions.Edit), UriKind.Relative));
        }


        #region Conversion button handlers

        private void trouserConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.TrouserConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.TrouserSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.TrouserSize);
                this.JumpToMeasurements();
                
            }

        }

        private void shirtConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.ShirtConversion.HasRequiredMeasurements) {
                App.VM.SelectedConversionType = ConversionId.ShirtSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.ShirtSize);
                this.JumpToMeasurements();
                
            }
        }

        private void hatConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.HatConversion.HasRequiredMeasurements) 
            {
                App.VM.SelectedConversionType = ConversionId.HatSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.HatSize);
                this.JumpToMeasurements();
                
            }
        }

        private void suitConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.SuitConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.SuitSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.SuitSize);
                this.JumpToMeasurements();
                
            }
        }

        private void dressConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.DressConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.DressSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.DressSize);
                this.JumpToMeasurements();
                
            }
        }

        private void braConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.BraConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.BraSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.BraSize);
                this.JumpToMeasurements();
                
            }
        }

        private void hosieryConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.HosieryConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.HosierySize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.HosierySize);
                this.JumpToMeasurements();
                
            }
        }

        private void shoeConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.ShoeConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.ShoeSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.ShoeSize);
                this.JumpToMeasurements();
            }
        }

        private void skiBootConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.SkiBootConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.SkiBootSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.SkiBootSize);
                this.JumpToMeasurements();
                
            }
        }


        private void wetsuitConversionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.WetsuitConversion.HasRequiredMeasurements)
            {
                App.VM.SelectedConversionType = ConversionId.WetsuitSize;
                NavigationService.Navigate(new Uri(String.Format("/View/Pages/ConversionsPage.xaml?ProfileId={0}&ConversionId={1}",
                    App.VM.SelectedProfile.Id, App.VM.SelectedConversionType.ToString()), UriKind.Relative));
            }
            else
            {
                App.VM.NominateConversion(ConversionId.WetsuitSize);
                this.JumpToMeasurements();
            }
        }


        #endregion


        private void cancelShowNominatedMeasurementsHelpBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.UnNominateConversion();
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
        }

		private void measurementBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e) 
		{
            Measurement m = (sender as MeasurementBtn).DataContext as Measurement;
            int profileId = App.VM.SelectedProfile.Id;
            NavigationService.Navigate(new Uri(String.Format("/View/Pages/EditMeasurementPage.xaml?MeasurementId={0}&ProfileId={1}",
                m.MeasurementId, profileId), UriKind.Relative));
        }

		private void newlyUnlockedBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
            ConversionId cId = ((sender as NewlyUnlockedConversionNotificationBtn).DataContext as ConversionBtnData).ConversionId;
            this.JumpToConversionBtn(cId);
		}


        public int? ScrollDownAfterPivotJump { get; set; }

        public void JumpToConversionBtn(ConversionId cId)
        {
            List<ConversionId> bottomHalfConversions = new List<ConversionId>()
            {
                ConversionId.WetsuitSize,
                ConversionId.SkiBootSize,
                ConversionId.ShoeSize,
            };
            if (bottomHalfConversions.Contains(cId))
            {
                this.ScrollDownAfterPivotJump = 800;
            }
            if (this.mainPivot.SelectedIndex != 0) this.mainPivot.SelectedIndex = 0;
            
        }



        #region Animations

        //public void ShowNewlyUnlockedBtnHelp()
        //{
        //    Dispatcher.BeginInvoke(() => {

        //        if (this.LayoutRoot.Resources.Contains("showNewlyUnlockedBtnHelp")) {
        //            (this.LayoutRoot.Resources["showNewlyUnlockedBtnHelp"] as Storyboard).Begin();
        //        } else {
        //            IEasingFunction quinticEase = new QuinticEase() { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut };
        //            Duration oneSecDuration = new Duration(TimeSpan.FromMilliseconds(1000));

        //            Storyboard sb = new Storyboard()
        //            sb.Duration = oneSecDuration;
        //            sb.SetValue(NameProperty, "showNewlyUnlockedBtnHelp");

        //            // Animate the container
        //            FrameworkElement container = this.toastHelpContainer as FrameworkElement;
        //            DoubleAnimation containerAnimation = new DoubleAnimation()
        //            {
        //                Duration = oneSecDuration,
        //                From = container.Height,
        //                To = container.Height + 194,
        //            };
        //            Storyboard.SetTargetProperty(containerAnimation, new PropertyPath(HeightProperty)); // Unsure about this latter param
        //            Storyboard.SetTarget(containerAnimation, this.toastHelpContainer); // Unsure about this latter param
        //            sb.Children.Add(containerAnimation);
                    
        //            // Animate the 


        //            // Add to resource dictionary for later reference
        //            this.LayoutRoot.Resources.Add("showNewlyUnlockedBtnHelp", sb);
        //            sb.Begin();
        //        }



        //    });
        //}


        #endregion




    }


}
