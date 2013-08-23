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
using Microsoft.Phone.Controls.Primitives;

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
            NavigationService.Navigate(new Uri(String.Format("/View/Pages/EditProfilePage.xaml?Action={0}&ProfileId={1}", EditProfilePageActions.Edit, p.Id), UriKind.Relative));
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




        private void navToSettingsBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Test();

        }

        private void navToAboutBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.JumpToPanoramaItem(3);

        }

        private void navToOpinionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.JumpToPanoramaItem(2);

        }


        private void navToProfileBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.JumpToPanoramaItem(1);
        }

        public void Test2()
        {
            Panorama pan = this.mainPanorama; // ActualWidth 480
            FrameworkElement grid1 = VisualTreeHelper.GetChild(pan, 0) as FrameworkElement;
            FrameworkElement ptl = VisualTreeHelper.GetChild(grid1, 1) as FrameworkElement;
            FrameworkElement sp2 = VisualTreeHelper.GetChild(ptl, 0) as FrameworkElement;

            Type tp = sp2.RenderTransform.GetType();

            sp2.RenderTransform = new TranslateTransform() { X = 0 };

            //ptl.RenderTransform = new TranslateTransform()

            Storyboard sb = new Storyboard();

            DoubleAnimation da = new DoubleAnimation()
            {
                From = 0,
                To = 100,
                Duration = TimeSpan.FromMilliseconds(500),
                //EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseOut },
            };
            sb.Children.Add(da);
            Storyboard.SetTargetProperty(da, new PropertyPath(TranslateTransform.XProperty));
            Storyboard.SetTarget(da, ptl.RenderTransform);
            sb.Begin();

        }

        public void Test()
        {
            Panorama pan = this.mainPanorama; // ActualWidth 480
            FrameworkElement grid1 = VisualTreeHelper.GetChild(pan, 0) as FrameworkElement;

                FrameworkElement pbl1 = VisualTreeHelper.GetChild(grid1, 0) as FrameworkElement;
                    FrameworkElement sp1 = VisualTreeHelper.GetChild(pbl1, 0) as FrameworkElement;
                        FrameworkElement rect1 = VisualTreeHelper.GetChild(sp1, 0) as FrameworkElement;
                        FrameworkElement cp1 = VisualTreeHelper.GetChild(sp1, 1) as FrameworkElement;
                        FrameworkElement rect2 = VisualTreeHelper.GetChild(sp1, 2) as FrameworkElement;

                FrameworkElement ptl1 = VisualTreeHelper.GetChild(grid1, 1) as FrameworkElement;
                    FrameworkElement sp2 = VisualTreeHelper.GetChild(ptl1, 0) as FrameworkElement;
                        FrameworkElement rect3 = VisualTreeHelper.GetChild(sp2, 0) as FrameworkElement;
                        FrameworkElement cp2 = VisualTreeHelper.GetChild(sp2, 1) as FrameworkElement;
                            FrameworkElement image1 = VisualTreeHelper.GetChild(cp2, 0) as FrameworkElement;
                        FrameworkElement rect4 = VisualTreeHelper.GetChild(sp2, 2) as FrameworkElement;

                FrameworkElement pl = VisualTreeHelper.GetChild(grid1, 2) as FrameworkElement;
                    FrameworkElement sp3 = VisualTreeHelper.GetChild(pl, 0) as FrameworkElement;
                        FrameworkElement rect5 = VisualTreeHelper.GetChild(sp3, 0) as FrameworkElement;
                        FrameworkElement cp4 = VisualTreeHelper.GetChild(sp3, 1) as FrameworkElement;
                            ItemsPresenter ip1 = VisualTreeHelper.GetChild(cp4, 0) as ItemsPresenter;
                                PanoramaPanel pp = VisualTreeHelper.GetChild(ip1, 0) as PanoramaPanel;
                                    PanoramaItem pi1 = VisualTreeHelper.GetChild(pp, 0) as PanoramaItem;
                                     Grid grid2 = VisualTreeHelper.GetChild(pi1, 0) as Grid;
                        FrameworkElement rect6 = VisualTreeHelper.GetChild(sp3, 2) as FrameworkElement;


                        int numPanelsToJump = 4;

                        double panItemWidth = pi1.ActualWidth;

                        //double relfinalPanTitleTranslate = - numPanelsToJump * 160;
                        //double titleJump = -(numPanelsToJump * panItemWidth);
                        //double finalPanTitleTranslate = numPanelsToJump * panItemWidth + relfinalPanTitleTranslate;

                        double relfinalPanTitleTranslate = -numPanelsToJump * 160;
                        //double titleJump = -(image1.ActualWidth + 348);
                        double titleJump = -(ptl1.ActualWidth - rect4.ActualWidth);
                        double finalPanTitleTranslate = -titleJump + relfinalPanTitleTranslate;

                        //rect4.Width = 0;

                        int lastInd = pp.Children.Count - 1;
                        int currInd = 0;
                        int numInd = pp.Children.Count;
                        double t = 550;
                                    
                        IEasingFunction easing = new CubicEase() { EasingMode = EasingMode.EaseInOut, };

                        Transform spTransform;

                        UIElement pi = pp.Children[lastInd];
                        pp.Children.Remove(pi);
                        pp.Children.Insert(0, pi);
                        // This jumps forward and messes up the title position too
                        pan.SetValue(Panorama.SelectedItemProperty, pp.Children[(currInd + 1) & numInd] as PanoramaItem);
                        
                        ptl1.RenderTransform = new TranslateTransform();
                        // Keep a reference to the trasnform o stackpanel since this is used to do ordinary nav animations
                        spTransform = sp2.RenderTransform;
                        // Set this offset since the home PanoramaItem is now the second in the list and the title 
                        // automatically jumps into the second PanoramaItem position 
                        sp2.RenderTransform = new TranslateTransform() { X = 0 };
                        pl.RenderTransform = new TranslateTransform() { X = 0 };

                        this.LayoutRoot.IsHitTestVisible = false;

                        this.LayoutRoot.Width = 3 * App.VM.ScreenWidth;
                        pan.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        pan.Width =  App.VM.ScreenWidth;

                        Storyboard sb = new Storyboard();
                        

                        DoubleAnimation animTitle = new DoubleAnimation()
                        {
                            From = 0,
                            To = finalPanTitleTranslate,
                            Duration = TimeSpan.FromMilliseconds(t),
                            EasingFunction = easing,
                        };
                        sb.Children.Add(animTitle);
                        Storyboard.SetTarget(animTitle, sp2.RenderTransform);
                        Storyboard.SetTargetProperty(animTitle, new PropertyPath(TranslateTransform.XProperty));

                        DoubleAnimationUsingKeyFrames jumpTitleAnim = new DoubleAnimationUsingKeyFrames();
                        //jumpTitleAnim.KeyFrames.Add(new DiscreteDoubleKeyFrame()
                        //{
                        //    KeyTime = TimeSpan.FromMilliseconds(0),
                        //    Value = 0,
                        //});
                        jumpTitleAnim.KeyFrames.Add(new DiscreteDoubleKeyFrame()
                        {
                            KeyTime = TimeSpan.FromMilliseconds(0),
                            Value = titleJump,
                        });
                        sb.Children.Add(jumpTitleAnim);
                        Storyboard.SetTarget(jumpTitleAnim, ptl1.RenderTransform);
                        Storyboard.SetTargetProperty(jumpTitleAnim, new PropertyPath(TranslateTransform.XProperty));


                        DoubleAnimation animMain = new DoubleAnimation()
                        {
                            From = 0,
                            To = panItemWidth,
                            Duration = TimeSpan.FromMilliseconds(t),
                            EasingFunction = easing,
                        };
                        sb.Children.Add(animMain);
                        Storyboard.SetTarget(animMain, pl.RenderTransform);
                        Storyboard.SetTargetProperty(animMain, new PropertyPath(TranslateTransform.XProperty));


                        sb.Begin();

                        sb.Completed += (obj, args) =>
                        {
                            pan.Width = App.VM.ScreenWidth;
                            this.LayoutRoot.Width = App.VM.ScreenWidth;

                            // Append it back to the end
                            pp.Children.Remove(pi);
                            pp.Children.Add(pi);

                            (pl.RenderTransform as TranslateTransform).X = 0;
                            (ptl1.RenderTransform as TranslateTransform).X = 0;

                            (pan.Items[(currInd + 1) & numInd] as PanoramaItem).Visibility = Visibility.Collapsed;
                            pan.SetValue(Panorama.SelectedItemProperty, pan.Items[lastInd]);
                            pan.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            (pan.Items[(currInd + 1) & numInd] as PanoramaItem).Visibility = Visibility.Visible;

                            sp2.RenderTransform = spTransform;
                                
                            this.LayoutRoot.IsHitTestVisible = true;

                        };
                        
        }





        public void JumpToPanoramaItem(int numPanelsToJump)
        {
            double t;
            switch (numPanelsToJump)
            {
                case 1:
                    t = 550;
                    break;
                case 2:
                    t = 550;
                    break;
                case 3:
                    t = 650;
                    break;
                case 4:
                    t = 750;
                    break;
                default:
                    t = 750;
                    break;
            }
            Panorama pan = this.mainPanorama; // ActualWidth 480

            double screenWidth = App.VM.ScreenWidth;

            // double panItemWidth = 432;
            // 5 PanoramaItems each 432 wide = 2160
            // Animate panTitle

            // scrolls back 432
            // Actual rsult is 159 
            // Should scroll back 273

            FrameworkElement grid1 = VisualTreeHelper.GetChild(pan, 0) as FrameworkElement;
            FrameworkElement ptl = VisualTreeHelper.GetChild(grid1, 1) as FrameworkElement; // W 1713
            FrameworkElement pl = VisualTreeHelper.GetChild(grid1, 2) as FrameworkElement; // W 2160

            //double translatePanTitle = -(panLayer.ActualWidth - panTitlePresenter.ActualWidth) / (pan.Items.Count - 1) * 1.5;
            double translatePanTitle = -numPanelsToJump * 160; //
            // (0.5 * pl.ActualWidth / (pan.Items.Count - 1) + 2)

            int curIndex = pan.SelectedIndex;

            if (!(pl.RenderTransform is TranslateTransform)
                || !(ptl.RenderTransform is TranslateTransform))
            {
                pl.RenderTransform = new TranslateTransform();
                ptl.RenderTransform = new TranslateTransform();
            }

            //pan.Width = this.LayoutRoot.Width;
            this.LayoutRoot.Width = (1 + numPanelsToJump) * screenWidth;

            //pan.Width = (1 + numPanelsToJump) * screenWidth;

            IEasingFunction easing = new CircleEase() { EasingMode = EasingMode.EaseInOut };

            // Animate the main panel
            Storyboard sb = new Storyboard();
            DoubleAnimation a = new DoubleAnimation()
            {
                From = 0,
                To = -numPanelsToJump * (pan.Items[curIndex] as PanoramaItem).ActualWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(t)),
                EasingFunction = easing,
            };
            sb.Children.Add(a);
            Storyboard.SetTarget(a, pl.RenderTransform);
            Storyboard.SetTargetProperty(a, new PropertyPath(TranslateTransform.XProperty));

            // Animate title
            // scroll ~200
            DoubleAnimation aTitle = new DoubleAnimation()
            {
                From = 0,
                To = translatePanTitle,
                Duration = new Duration(TimeSpan.FromMilliseconds(t)),
                EasingFunction = easing,
            };
            sb.Children.Add(aTitle);
            Storyboard.SetTarget(aTitle, ptl.RenderTransform);
            Storyboard.SetTargetProperty(aTitle, new PropertyPath(TranslateTransform.XProperty));


            sb.Completed += (obj, args) =>
            {
                this.LayoutRoot.Width = screenWidth;

                (pan.Items[curIndex] as PanoramaItem).Visibility = Visibility.Collapsed;
                pan.SetValue(Panorama.SelectedItemProperty, pan.Items[(curIndex + numPanelsToJump) % pan.Items.Count]);
                pan.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                (pan.Items[curIndex] as PanoramaItem).Visibility = Visibility.Visible;

                (pl.RenderTransform as TranslateTransform).X = 0;
                (ptl.RenderTransform as TranslateTransform).X = 0;
                this.LayoutRoot.IsHitTestVisible = true;

            };

            this.LayoutRoot.IsHitTestVisible = false;

            sb.Begin();

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
            App.VM.EmailAuthor("Feedback on Pocketailor");
        }

        private void reviewBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.RateApp();
        }

        private void twitterBtn2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewTwitter("@LassivCo");
        }

        private void twitterBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewTwitter("@LassivCo #Pocketailor needs: ");
        }

        private void facebookBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.ViewFacebook();
        }

        private void facebookQuestionBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Animations.BlindDown.OpenToHeight(this.facebookAnswer, 113);
        }

        

       


        

        

        



    }
}