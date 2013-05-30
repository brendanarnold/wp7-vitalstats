using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Pocketailor.Model;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Pocketailor.View.Controls;

namespace Pocketailor.View
{
    public partial class ConversionsPage : PhoneApplicationPage
    {
        public ConversionsPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

            App.VM.ConversionDataLoaded += (s) =>
            {
                this.AdRotatorControl.DefaultHouseAdBody = "Pocketailor.LocalAd";
                this.AdRotatorControl.Invalidate();
            };

        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.AdRotatorControl.Invalidate();
            base.OnNavigatedTo(e);
            

            // Load up the data using a query string in case of tombstoning
            string profileIdStr, conversionIdStr;
            NavigationContext.QueryString.TryGetValue("ProfileId", out profileIdStr);
            int profileId = System.Int32.Parse(profileIdStr);
            NavigationContext.QueryString.TryGetValue("ConversionId", out conversionIdStr);
            ConversionId conversionId = (ConversionId)Enum.Parse(typeof(ConversionId), conversionIdStr, true);
            if (App.VM.GroupedConversions == null
                || !App.VM.SkipLoadConversionPageData)
            {
                if (App.VM.SkipLoadConversionPageData)
                    App.VM.SkipLoadConversionPageData = false;
                    await App.VM.LoadConversionsPageDataAsyncTask(profileId, conversionId);
            }
            else
            {
                if (App.VM.SkipLoadConversionPageData)
                    App.VM.SkipLoadConversionPageData = false;
            }
            

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.AdRotatorControl.Dispose();
            base.OnNavigatedFrom(e);
            App.VM.SaveBlacklistedBrands();
        }


        #region Animation

        private LongListSelector currentSelector;

        private void conversionsLongListSelector_GroupViewOpened(object sender, GroupViewOpenedEventArgs e)
        {
            //Hold a reference to the active long list selector.
            currentSelector = sender as LongListSelector;

               

            //Dispatch the swivel animation for performance on the UI thread.
            Dispatcher.BeginInvoke(() =>
            {
                //Construct and begin a swivel animation to pop in the group view.
                IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
                Storyboard _swivelShow = new Storyboard();
                _swivelShow.SetValue(NameProperty, "_swivelShowStoryboard");
                ItemsControl groupItems = e.ItemsControl;

                foreach (var item in groupItems.Items)
                {
                    UIElement container = groupItems.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
                    if (container != null)
                    {
                        Border content = VisualTreeHelper.GetChild(container, 0) as Border;
                        if (content != null)
                        {
                            DoubleAnimationUsingKeyFrames showAnimation = new DoubleAnimationUsingKeyFrames();

                            EasingDoubleKeyFrame showKeyFrame1 = new EasingDoubleKeyFrame();
                            showKeyFrame1.KeyTime = TimeSpan.FromMilliseconds(0);
                            showKeyFrame1.Value = -60;
                            showKeyFrame1.EasingFunction = quadraticEase;

                            EasingDoubleKeyFrame showKeyFrame2 = new EasingDoubleKeyFrame();
                            showKeyFrame2.KeyTime = TimeSpan.FromMilliseconds(85);
                            showKeyFrame2.Value = 0;
                            showKeyFrame2.EasingFunction = quadraticEase;

                            showAnimation.KeyFrames.Add(showKeyFrame1);
                            showAnimation.KeyFrames.Add(showKeyFrame2);

                            Storyboard.SetTargetProperty(showAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
                            Storyboard.SetTarget(showAnimation, content.Projection);

                            _swivelShow.Children.Add(showAnimation);
                        }
                    }
                }

                _swivelShow.Begin();
            });
        }

        private void conversionsLongListSelector_GroupViewClosing(object sender, GroupViewClosingEventArgs e)
        {

            //Cancelling automatic closing and scrolling to do it manually.
            e.Cancel = true;
            if (e.SelectedGroup != null)
            {
                currentSelector.ScrollToGroup(e.SelectedGroup);
            }
            

            //Dispatch the swivel animation for performance on the UI thread.
            Dispatcher.BeginInvoke(() =>
            {
                //Construct and begin a swivel animation to pop out the group view.
                IEasingFunction quadraticEase = new QuadraticEase { EasingMode = EasingMode.EaseOut };
                Storyboard _swivelHide = new Storyboard();
                _swivelHide.SetValue(NameProperty, "_swivelHideStoryboard");
                ItemsControl groupItems = e.ItemsControl;

                foreach (var item in groupItems.Items)
                {
                    UIElement container = groupItems.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
                    if (container != null)
                    {
                        Border content = VisualTreeHelper.GetChild(container, 0) as Border;
                        if (content != null)
                        {
                            DoubleAnimationUsingKeyFrames showAnimation = new DoubleAnimationUsingKeyFrames();

                            EasingDoubleKeyFrame showKeyFrame1 = new EasingDoubleKeyFrame();
                            showKeyFrame1.KeyTime = TimeSpan.FromMilliseconds(0);
                            showKeyFrame1.Value = 0;
                            showKeyFrame1.EasingFunction = quadraticEase;

                            EasingDoubleKeyFrame showKeyFrame2 = new EasingDoubleKeyFrame();
                            showKeyFrame2.KeyTime = TimeSpan.FromMilliseconds(125);
                            showKeyFrame2.Value = 90;
                            showKeyFrame2.EasingFunction = quadraticEase;

                            showAnimation.KeyFrames.Add(showKeyFrame1);
                            showAnimation.KeyFrames.Add(showKeyFrame2);

                            Storyboard.SetTargetProperty(showAnimation, new PropertyPath(PlaneProjection.RotationXProperty));
                            Storyboard.SetTarget(showAnimation, content.Projection);

                            _swivelHide.Children.Add(showAnimation);
                        }
                    }
                }

                _swivelHide.Completed += _swivelHide_Completed;
                _swivelHide.Begin();

            });

            

        }

        private void _swivelHide_Completed(object sender, EventArgs e)
        {
            //Close group view.
            if (currentSelector != null)
            {
                currentSelector.CloseGroupView();
                currentSelector = null;
            }
        }


        #endregion




        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (ConversionData.CurrentlyAdjustingConversionData != null)
            {
                e.Cancel = true;
                ConversionData.CurrentlyAdjustingConversionData.DiscardTweaks();
                ConversionData.CurrentlyAdjustingConversionData.IsAdjusting = false;
            }
            base.OnBackKeyPress(e);
        }


        #region Adjustments methods/properties

        // Main set of methods for adjustments found in AdjustmentWidget.xaml.cs


        //private void conversionResultContainerGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    ConversionData c = (sender as Grid).DataContext as ConversionData;
        //    c.IsAdjusting = !c.IsAdjusting;
        //    if (c.IsAdjusting)
        //    {
        //        // First time adjusting, give a prompt
        //        if (App.Settings.GetValueOrDefault<bool>("ShowHelpAdjustment", true))
        //        {
        //            MessageBoxResult res = MessageBox.Show("Tap a brand name to adjust the sizes."
        //                + Environment.NewLine
        //                + Environment.NewLine
        //                + "If you don't want to make an adjustment, tap the brand name again or hit the back button.",
        //                "Making an adjustment", MessageBoxButton.OK);
        //            App.Settings.AddOrUpdateValue("ShowHelpAdjustment", false);
        //            App.Settings.Save();
        //        }
        //    }
        //    else
        //    {
        //        c.DiscardTweaks();
        //    }

            
        //}


        private void tooBigBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Button).DataContext as ConversionData;
            c.TweakSizeDown();
        }

        private void rightSizeBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.AllowFeedBack == null) this.PromptForFeedbackPermission();
            ConversionData c = (sender as Button).DataContext as ConversionData;
            c.IsAdjusting = false;
            c.AcceptTweaks();
        }

        private void tooSmallBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Button).DataContext as ConversionData;
            c.TweakSizeUp();
        }

        private void PromptForFeedbackPermission()
        {

            MessageBoxResult res = MessageBox.Show("You can send your adjustments to the Pocketailor team anonymously over the web so we can make the results better next time. If you would like to help then click 'ok' below, if you would rather not then click 'cancel'."
                    + Environment.NewLine + Environment.NewLine
                    + "You can change this anytime in the settings.",
                    "Allow feedback?",
                    MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                App.VM.AllowFeedBack = true;
            }
            else
            {
                App.VM.AllowFeedBack = false;
            }

        }

        private void conversionResultContainerGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Grid).DataContext as ConversionData;
            App.VM.SelectedConversionData = c;
            string uriStr = String.Format("/View/ConversionAdjustmentPage.xaml?GenderId={0}&BrandId={1}&ConversionId={2}&RegionId={3}&ProfileId={4}",
                App.VM.SelectedProfile.Gender, c.Brand, c.Conversion, c.Region, App.VM.SelectedProfile.Id);
            NavigationService.Navigate(new Uri(uriStr, UriKind.Relative));
        }


        #endregion

    }
}