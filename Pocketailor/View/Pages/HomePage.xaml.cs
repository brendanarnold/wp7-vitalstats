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
using Pocketailor.ViewModel;

namespace Pocketailor.View
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();

            this.LoadSettingsIntoPage();
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = App.VM;
            App.VM.LoadMainPageData();
        }



        #region Profile button methods

        private void profileBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as Button).DataContext as Profile;
            NavigationService.Navigate(new Uri(String.Format("/View/Pages/MeasurementsPage.xaml?ProfileId={0}", p.Id), UriKind.Relative));
        }

        private void toggleQuickListContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            App.VM.ToggleQuickProfile(p);

        }

        private void editContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile p = (sender as MenuItem).DataContext as Profile;
            App.VM.SelectedProfile = p;
            NavigationService.Navigate(new Uri(String.Format("/View/Pages/EditProfilePage.xaml?Action={0}&ProfileId={1}", EditProfilePageActions.Edit, App.VM.SelectedProfile.Id), UriKind.Relative));
        }

        private void deleteContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Profile profile = ((sender as MenuItem).DataContext) as Profile;
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "Confirm delete",
                Message = String.Format("Are you certain you want to delete the profile for '{0}'?", profile.Name),
                LeftButtonContent = "Yes",
                RightButtonContent = "No",
                IsFullScreen = false
            };
            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        App.VM.DeleteProfile(profile);
                        break;
                    case CustomMessageBoxResult.RightButton:
                    default:
                        break;
                }
            };
            messageBox.Show();
        }


        #endregion

        private void Panorama_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // this is really jerky
            //switch (this.mainPanorama.SelectedIndex)
            //{
            //    case 1:
            //        this.ApplicationBar = (ApplicationBar)Resources["profilesApplicationBar"];
            //        break;
            //    default:
            //        this.ApplicationBar = null;
            //        break;
            //}
        }

        private void addNewProfileButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	NavigationService.Navigate(new Uri(String.Format("/View/Pages/EditProfilePage.xaml?Action={0}", EditProfilePageActions.New), UriKind.Relative));
        }


        #region Settings methods

        // Initialise the checkboxes
        private void LoadSettingsIntoPage()
        {
            
            this.metricRadioBtn.IsChecked = (App.VM.UnitCulture == UnitCultureId.Metric);
            this.imperialRadioBtn.IsChecked = !(App.VM.UnitCulture == UnitCultureId.Metric);

            this.darkThemeRadioBtn.IsChecked = (ThemeHelpers.GetTheme() == ApplicationTheme.Dark);
            this.lightThemeRadioBtn.IsChecked = (ThemeHelpers.GetTheme() == ApplicationTheme.Light);

            if (App.VM.AllowFeedBack != null)
            {
                this.allowFeedbackRadioBtn.IsChecked = App.VM.AllowFeedBack;
                this.disallowFeedbackRadioBtn.IsChecked = !App.VM.AllowFeedBack;
            }

        }


        private void imperialRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.UnitCulture = UnitCultureId.Imperial;
        }


        private void metricRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.UnitCulture = UnitCultureId.Metric;
        }

        

        private void allowFeedbackRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.AllowFeedBack = true;
        }

        private void disallowFeedbackRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.AllowFeedBack = false;
        }

        private void darkThemeRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.ThemeHelpers.SetThemePreference(ViewModel.ApplicationTheme.Dark);
        }

        private void lightThemeRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
           ViewModel.ThemeHelpers.SetThemePreference(ViewModel.ApplicationTheme.Light);
        }

        #endregion



        #region Navigation methods


        private void navToSettingsBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.JumpFromPanormaHome(4);
        }

        private void navToAboutBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.JumpFromPanormaHome(3);
        }

        private void navToOpinionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.JumpFromPanormaHome(2);
        } 


        private void navToProfileBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.mainPanorama.DefaultItem = this.mainPanorama.Items[2];

            this.JumpFromPanormaHome(1);

            //Panorama pan = this.mainPanorama;
            //pan.SetValue(Panorama.SelectedItemProperty, pan.Items[2]);
        
            //this.slidePanorama(this.mainPanorama);
            //this.slidePanorama(this.mainPanorama);
            
        }

        private void JumpFromPanormaHome(int count)
        {
            Panorama pan = this.mainPanorama;
            int curIndex = this.mainPanorama.SelectedIndex;
            for (int i=0; i< count; i++)
                (pan.Items[curIndex + i] as PanoramaItem).Visibility = Visibility.Collapsed;

            pan.SetValue(Panorama.SelectedItemProperty, pan.Items[(curIndex + count) % pan.Items.Count]);
            pan.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            
            for (int i=0; i< count; i++)
                (pan.Items[curIndex + i] as PanoramaItem).Visibility = Visibility.Visible;
            
        }

        private void slidePanorama(Panorama pan)
        {
            FrameworkElement panWrapper = VisualTreeHelper.GetChild(pan, 0) as FrameworkElement;
            FrameworkElement panTitle = VisualTreeHelper.GetChild(panWrapper, 1) as FrameworkElement;
            //Get the panorama layer to calculate all panorama items size
            FrameworkElement panLayer = VisualTreeHelper.GetChild(panWrapper, 2) as FrameworkElement;
            //Get the title presenter to calculate the title size
            FrameworkElement panTitlePresenter = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(panTitle, 0) as FrameworkElement, 1) as FrameworkElement;
            //Current panorama item index
            int curIndex = pan.SelectedIndex;
            //Get the next of next panorama item
            FrameworkElement third = VisualTreeHelper.GetChild(pan.Items[(curIndex + 2) % pan.Items.Count] as PanoramaItem, 0) as FrameworkElement;
            //Be sure the RenderTransform is TranslateTransform
            if (!(pan.RenderTransform is TranslateTransform)
                || !(panTitle.RenderTransform is TranslateTransform))
            {
                pan.RenderTransform = new TranslateTransform();
                panTitle.RenderTransform = new TranslateTransform();
            }
            //Increase width of panorama to let it render the next slide (if not, default panorama is 480px and the null area appear if we transform it)
            pan.Width = 960;
            //Animate panorama control to the right
            Storyboard sb = new Storyboard();
            DoubleAnimation a = new DoubleAnimation();
            a.From = 0;
            a.To = -(pan.Items[curIndex] as PanoramaItem).ActualWidth;
            //Animate the x transform to a width of one item
            a.Duration = new Duration(TimeSpan.FromMilliseconds(700));
            a.EasingFunction = new CircleEase();
            //This is default panorama easing effect
            sb.Children.Add(a);
            Storyboard.SetTarget(a, pan.RenderTransform);
            Storyboard.SetTargetProperty(a, new PropertyPath(TranslateTransform.XProperty));
            //Animate panorama title separately
            DoubleAnimation aTitle = new DoubleAnimation();
            aTitle.From = 0;
            aTitle.To = (panLayer.ActualWidth - panTitlePresenter.ActualWidth) / (pan.Items.Count - 1) * 1.5;
            //Calculate where should the title animate to
            aTitle.Duration = a.Duration;
            aTitle.EasingFunction = a.EasingFunction;
            //This is default panorama easing effect
            sb.Children.Add(aTitle);
            Storyboard.SetTarget(aTitle, panTitle.RenderTransform);
            Storyboard.SetTargetProperty(aTitle, new PropertyPath(TranslateTransform.XProperty));
            //Start the effect
            sb.Begin();
            //After effect completed, we change the selected item
            a.Completed += (obj, args) =>
            {
                //Reset panorama width
                pan.Width = 480;
                //Change the selected item
                (pan.Items[curIndex] as PanoramaItem).Visibility = Visibility.Collapsed;
                pan.SetValue(Panorama.SelectedItemProperty, pan.Items[(curIndex + 1) % pan.Items.Count]);
                pan.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                (pan.Items[curIndex] as PanoramaItem).Visibility = Visibility.Visible;
                //Reset panorama render transform
                (pan.RenderTransform as TranslateTransform).X = 0;
                //Reset title render transform
                (panTitle.RenderTransform as TranslateTransform).X = 0;
                //Because of the next of next item will be load after we change the selected index to next item
                //I do not want it appear immediately without any effect, so I create a custom effect for it
                if (!(third.RenderTransform is TranslateTransform))
                {
                    third.RenderTransform = new TranslateTransform();
                }
                Storyboard sb2 = new Storyboard();
                DoubleAnimation aThird = new DoubleAnimation() { From = 100, To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(300)) };
                sb2.Children.Add(aThird);
                Storyboard.SetTarget(aThird, third.RenderTransform);
                Storyboard.SetTargetProperty(aThird, new PropertyPath(TranslateTransform.XProperty));
                sb2.Begin();
            };
        }


        #endregion




        private void legalThanksBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewLicences();
        }

        private void emailBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.EmailAuthor();
        }

        private void reviewBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.RateApp();
        }

        private void twitterBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewTwitter();
        }

        private void facebookBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewFacebook();
        }

        

       


        

        

        



    }
}