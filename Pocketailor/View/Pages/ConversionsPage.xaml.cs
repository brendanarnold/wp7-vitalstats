﻿using System;
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
using Pocketailor.ViewModel;

namespace Pocketailor.View
{
    public partial class ConversionsPage : PhoneApplicationPage
    {
        public ConversionsPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

            this.IsKeyMeasurementsDisplayed = false;

        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
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
                    // Have to raise these synchronously to avoid crashes
                    App.VM.NotifyPropertyChanged("SelectedProfile");
                    App.VM.NotifyPropertyChanged("SelectedConversionType");
            }
            else
            {
                if (App.VM.SkipLoadConversionPageData)
                    App.VM.SkipLoadConversionPageData = false;
            }

            // Add the advert is not paid
            if (!(bool)App.Settings.GetValueOrDefault("DisableAds", false)
                && this.adControl == null)
            {
                this.adControl = new AdDuplex.AdControl()
                {
                    AppId = AppConstants.ADDUPLEX_APP_ID,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                this.LayoutRoot.Children.Add(this.adControl);
                Grid.SetColumn(this.adControl, 0);
                Grid.SetRow(this.adControl, 3);
            }


        }


        public AdDuplex.AdControl adControl;

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
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


       

        private void conversionResultContainerGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Grid).DataContext as ConversionData;
            App.VM.SelectedConversionData = c;
            string uriStr = String.Format("/View/Pages/ConversionAdjustmentPage.xaml?GenderId={0}&BrandId={1}&ConversionId={2}&RegionId={3}&ProfileId={4}",
                App.VM.SelectedProfile.Gender, c.Brand, c.Conversion, c.Region, App.VM.SelectedProfile.Id);
            NavigationService.Navigate(new Uri(uriStr, UriKind.Relative));
        }


        private void profileNameBtn_Tap(object sender, System.EventArgs e)
        {

        }

       

        #endregion

        private bool IsKeyMeasurementsDisplayed { get; set; }

        private void showKeyMeasurementsBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.IsKeyMeasurementsDisplayed)
            {
                this.IsKeyMeasurementsDisplayed = false;
                VisualStateManager.GoToState(this, "hiddenKeyMeasurements", true);
            }
            else
            {
                this.IsKeyMeasurementsDisplayed = true;
                VisualStateManager.GoToState(this, "displayedKeyMeasurements", true);
            }
        }

    }
}