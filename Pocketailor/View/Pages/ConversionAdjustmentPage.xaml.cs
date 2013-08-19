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
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Pocketailor.View
{
    public partial class ConversionAdjustmentPage : PhoneApplicationPage
    {
        public ConversionAdjustmentPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

           


        }

        #region Slider behaviour


        enum PendingSlide
        {
            None,
            Down,
            Up,
        }

        PendingSlide PendingJump { get; set; }


        bool CrossThresholdUp(double threshold, double x, double delta)
        {
            return (x <= threshold && (x + delta) > threshold);
        }

        bool CrossThresholdDown(double threshold, double x, double delta)
        {
            return (x >= threshold && (x + delta) < threshold);
        }

        void gestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            // Beware reversed geometry - more negative translation means grid more to 
            // the left and more towards jumping up a size
            double sizeDownThreshold = (-App.VM.ScreenWidth + App.VM.ScreenWidth / 6);
            double sizeUpThreshold = (-App.VM.ScreenWidth - App.VM.ScreenWidth / 6);
            double x = this.sliderTransform.TranslateX;
            // Start with most common first ...
            // Test for passing the switch down threshold
            if (this.CrossThresholdUp(sizeDownThreshold, x, e.HorizontalChange))
            {
                this.PendingJump = PendingSlide.Down;
                this.SwitchToNextSizeDown(e.HorizontalChange);
            }
            // Test for passing the switch up threshold
            else if (this.CrossThresholdDown(sizeUpThreshold, x, e.HorizontalChange))
            {
                this.PendingJump = PendingSlide.Up;
                this.SwitchToNextSizeUp(e.HorizontalChange);
            }
            // Test for passing back past the switch down threshold (cancel jump)
            else if (this.CrossThresholdDown(sizeDownThreshold, x, e.HorizontalChange))
            {
                this.PendingJump = PendingSlide.None;
                this.SwitchToNextSizeUp(e.HorizontalChange);
            }
            else if (this.CrossThresholdUp(sizeUpThreshold, x, e.HorizontalChange))
            {
                this.PendingJump = PendingSlide.None;
                this.SwitchToNextSizeDown(e.HorizontalChange);
            }
            else
            {
                this.sliderTransform.TranslateX += e.HorizontalChange;
            }

        }

        void gestureListener_DragStarted(object sender, DragStartedGestureEventArgs e)
        {


        }

        
        void gestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {

            this.JumpToCentralMeasurement();
        }

        void JumpToCentralMeasurement()
        {
            this.PendingJump = PendingSlide.None;
            
            double finalX = -App.VM.ScreenWidth;
            double initialX = this.sliderTransform.TranslateX;

            Dispatcher.BeginInvoke(() =>
            {
                Storyboard sb = new Storyboard();
                IEasingFunction easing = new QuarticEase() { EasingMode = EasingMode.EaseOut };

                DoubleAnimation xTransAnim = new DoubleAnimation()
                {
                    From = initialX,
                    To = finalX,
                    Duration = TimeSpan.FromMilliseconds(750),
                    EasingFunction = easing,
                };

                Storyboard.SetTarget(xTransAnim, this.sliderTransform);
                Storyboard.SetTargetProperty(xTransAnim, new PropertyPath(CompositeTransform.TranslateXProperty));
                sb.Children.Add(xTransAnim);
                sb.Begin();
            });
            
        }

        void gestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            if (this.PendingJump == PendingSlide.None)
            {
                if (Math.Abs(e.HorizontalVelocity) > 600)
                {
                    if (e.HorizontalVelocity > 0)
                    {
                        this.SwitchToNextSizeDown(0);
                        this.JumpToCentralMeasurement();
                    }
                    else
                    {
                        this.SwitchToNextSizeUp(0);
                        this.JumpToCentralMeasurement();
                    }
                }
            }
        }


        void SwitchToNextSizeUp(double delta)
        {
            this.sliderTransform.TranslateX += (delta + App.VM.ScreenWidth);
            Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromMilliseconds(10));
        }

        void SwitchToNextSizeDown(double delta)
        {
            this.sliderTransform.TranslateX += (delta - App.VM.ScreenWidth);
            Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromMilliseconds(10));
        }



        #endregion


        public ConversionId ConversionId { get; set; }
        public BrandId BrandId { get; set; }
        public GenderId GenderId { get; set; }
        public string RegionId { get; set; }
        public int ProfileId { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Take care of the tombstone situation
            if (!e.IsNavigationInitiator)
            {
                this.ParseUriQuery();
                if (App.VM.SelectedConversionData == null
                    || App.VM.SelectedConversionData.Brand != this.BrandId
                    || App.VM.SelectedConversionData.Conversion != this.ConversionId
                    || App.VM.SelectedConversionData.Region != this.RegionId
                    || App.VM.SelectedConversionData.Gender != this.GenderId)
                {
                    App.VM.LoadSelectedConversionData(this.GenderId, this.ConversionId, this.RegionId, this.BrandId);
                }
                if (App.VM.SelectedProfile == null
                    || App.VM.SelectedProfile.Id != this.ProfileId)
                {
                    App.VM.LoadSelectedProfile(this.ProfileId);
                }
                
            }
        }

        public void ParseUriQuery()
        {
            this.ProfileId = int.Parse(NavigationContext.QueryString["ProfileId"]);
            this.GenderId = (GenderId)Enum.Parse(typeof(GenderId), NavigationContext.QueryString["GenderId"], true);
            this.ConversionId = (ConversionId)Enum.Parse(typeof(ConversionId), NavigationContext.QueryString["ConversionId"], true);
            this.RegionId = NavigationContext.QueryString["RegionId"];
            this.BrandId = (BrandId)Enum.Parse(typeof(BrandId), NavigationContext.QueryString["BrandId"], true);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (App.VM.SelectedConversionData.IsUnsavedTweaks)
            {
                MessageBoxResult res = MessageBox.Show("You have made an adjustment but have not saved it."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "To save the changes hit 'cancel' and then the save icon at the bottom. To discard the changes hit 'ok", "Discard adjustment?", MessageBoxButton.OKCancel);
                if (res != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
            }
            App.VM.SelectedConversionData.DiscardTweaks();
            App.VM.SkipLoadConversionPageData = true;
            base.OnBackKeyPress(e);
        }

        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (App.VM.AllowFeedBack == null) this.PromptForFeedbackPermission();
            App.VM.SelectedConversionData.AcceptTweaks();
            App.VM.SkipLoadConversionPageData = true;

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }



        //private void tooBigBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    App.VM.SelectedConversionData.TweakSizeDown();
        //    this.sizeDownStoryboard.Begin();
        //}

        //private void tooSmallBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    App.VM.SelectedConversionData.TweakSizeUp();
        //    this.sizeDownStoryboard.Begin();
        //}

        private void PromptForFeedbackPermission()
        {

            MessageBoxResult res = MessageBox.Show("You can send your adjustments to the Pocketailor team anonymously over the web so we can make the results better next time. If you would like to contribute then click 'ok' below, if you would rather not then click 'cancel'."
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
    }
}