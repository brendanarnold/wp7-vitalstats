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
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Pocketailor.View
{
    public partial class ConversionAdjustmentPage : PhoneApplicationPage
    {
        public ConversionAdjustmentPage()
        {
            this.InitSliderVars();

            InitializeComponent();

            this.DataContext = App.VM;

            this.SwapRulerImages();
            this.HighlightAnimation();


        }

        #region Slider behaviour

        public double SliderContainerWidth { get; set; }
        public double SliderRestTranslateX { get; set; }
        public double SizeDownThreshold { get; set; }
        public double SizeUpThreshold { get; set; }
        public double RestrictedDragThreshold { get; set; }
        public double FlickVelocityThreshold { get; set; }

        Slide PendingSlide { get; set; }
        Storyboard HighlightStoryBoard;
        Storyboard SlideStoryBoard;
        BitmapImage EndRuler { get; set; }
        BitmapImage Ruler { get; set; }


        void InitSliderVars() 
        {
            // These values seem to work OK
            this.SliderContainerWidth = 480;
            this.SliderRestTranslateX = (App.VM.ScreenWidth - this.SliderContainerWidth) / 2 - this.SliderContainerWidth;
            this.NotifyPropertyChanged("SliderContainerWidth");
            this.NotifyPropertyChanged("SliderRestTranslateX");
            this.SizeDownThreshold = this.SliderRestTranslateX + App.VM.ScreenWidth / 6;
            this.SizeUpThreshold = this.SliderRestTranslateX - App.VM.ScreenWidth / 6;
            this.RestrictedDragThreshold = App.VM.ScreenWidth / 10;
            this.FlickVelocityThreshold = 600;

            this.HighlightStoryBoard = new Storyboard();
            this.SlideStoryBoard = new Storyboard();
            this.SlideStoryBoard.Completed += (s, e) =>
            {
                this.HighlightStoryBoard.Begin();
            };
            this.Ruler = new BitmapImage(new Uri("/Images/ruler_w480.png", UriKind.Relative));
            this.EndRuler = new BitmapImage(new Uri("/Images/ruler_w480_end.png", UriKind.Relative));
        }

        enum Slide
        {
            None,
            Down,
            Up,
        }

        void SwapRulerImages()
        {
            if (App.VM.SelectedConversionData.NoneBigger)
            {
                this.sizeRulerImage.Source = this.EndRuler;
                this.sizeRulerImage.HorizontalAlignment = HorizontalAlignment.Left;
                this.sizeRulerImageTransform.ScaleX = -1;
            }
            else if (App.VM.SelectedConversionData.NoneSmaller)
            {
                this.sizeRulerImage.Source = this.EndRuler;
                this.sizeRulerImage.HorizontalAlignment = HorizontalAlignment.Right;
                this.sizeRulerImageTransform.ScaleX = 1;
            }
            else
            {
                this.sizeRulerImage.Source = this.Ruler;
            }
        }

        bool CrossThresholdUp(double threshold, double x, double delta)
        {
            return (x <= threshold && ((x + delta) > threshold));
        }

        bool CrossThresholdDown(double threshold, double x, double delta)
        {
            return (x >= threshold && ((x + delta) < threshold));
        }

        void gestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            // Beware reversed geometry - a negative translation delta means grid moves to 
            // the left and more towards jumping up a size
            
            // Save a bit of typing ...
            double x = this.sliderTransform.TranslateX;

            // Confines the dragging when at the first and last sizes 
            if (App.VM.SelectedConversionData.NoneBigger
                && (x + e.HorizontalChange) <= (this.SliderRestTranslateX - this.RestrictedDragThreshold))
            {
                this.sliderTransform.TranslateX = (this.SliderRestTranslateX - this.RestrictedDragThreshold);
            }
            else if (App.VM.SelectedConversionData.NoneSmaller
                && (x + e.HorizontalChange) >= (this.SliderRestTranslateX + this.RestrictedDragThreshold))
            {
                this.sliderTransform.TranslateX = (this.SliderRestTranslateX + this.RestrictedDragThreshold);
            }
            // Test for passing the switch down threshold
            else if (this.CrossThresholdUp(this.SizeDownThreshold, x, e.HorizontalChange))
            {
                this.PendingSlide = Slide.Down;
                this.SwapInNextSizeDown(e.HorizontalChange);
            }
            // Test for passing the switch up threshold
            else if (this.CrossThresholdDown(this.SizeUpThreshold, x, e.HorizontalChange))
            {
                this.PendingSlide = Slide.Up;
                this.SwapInNextSizeUp(e.HorizontalChange);
            }
            // Test for passing back past the switch down threshold (cancel jump)
            else if (this.PendingSlide == Slide.Down
                 && this.CrossThresholdDown(this.SizeDownThreshold - this.SliderContainerWidth, x, e.HorizontalChange))
            {
                this.PendingSlide = Slide.None;
                this.SwapInNextSizeUp(e.HorizontalChange);
            }
            // Test for passing back past the switch up threshold (cancel jump)
            else if (this.PendingSlide == Slide.Up
                && this.CrossThresholdUp(this.SizeUpThreshold + this.SliderContainerWidth, x, e.HorizontalChange))
            {
                this.PendingSlide = Slide.None;
                this.SwapInNextSizeDown(e.HorizontalChange);
            }
            // Just a normal drag
            else
            {
                this.sliderTransform.TranslateX += e.HorizontalChange;
            }

        }

        void gestureListener_DragStarted(object sender, DragStartedGestureEventArgs e)
        {
            this.CancelHighlight();
        }
        
        void gestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            this.SlideToCentralMeasurement();
        }


        

        void HighlightAnimation()
        {
            // Max out the scaling at 1.2, also allow for 6px padding
            double scaleFactor = Math.Min(Math.Abs(this.SliderContainerWidth / (this.sizeContainer.ActualWidth - 12)), 1.2);
            this.HighlightStoryBoard.Stop();

            //Dispatcher.BeginInvoke(() =>
            //{
                this.HighlightStoryBoard.Children.Clear();
                IEasingFunction easing = new QuadraticEase() { EasingMode = EasingMode.EaseOut };

                // Animate the highlight
                DoubleAnimation mainScaleAnimX = new DoubleAnimation()
                {
                    From = 1.0,
                    To = scaleFactor,
                    Duration = TimeSpan.FromMilliseconds(500),
                    EasingFunction = easing,
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                Storyboard.SetTarget(mainScaleAnimX, this.sizeContainer.RenderTransform);
                Storyboard.SetTargetProperty(mainScaleAnimX, new PropertyPath(CompositeTransform.ScaleXProperty));
                this.HighlightStoryBoard.Children.Add(mainScaleAnimX);
                DoubleAnimation mainScaleAnimY = new DoubleAnimation()
                {
                    From = 1.0,
                    To = scaleFactor,
                    Duration = TimeSpan.FromMilliseconds(500),
                    EasingFunction = easing,
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                Storyboard.SetTarget(mainScaleAnimY, this.sizeContainer.RenderTransform);
                Storyboard.SetTargetProperty(mainScaleAnimY, new PropertyPath(CompositeTransform.ScaleYProperty));
                this.HighlightStoryBoard.Children.Add(mainScaleAnimY);

                this.HighlightStoryBoard.Begin();
            //});
        }

        //private List<Storyboard> _slideTransitions;
        //public List<Storyboard> SlideTransitions
        //{
        //    get
        //    {
        //        if (this._slideTransitions == null) this._slideTransitions = new List<Storyboard>();
        //        return this._slideTransitions;
        //    }
        //    set
        //    {
        //        this._slideTransitions = value;
        //    }

        //}


        // An animation that restores the slider to the central, 'relaxed' position
        void SlideToCentralMeasurement()
        {
            this.PendingSlide = Slide.None;
            
            double finalX = this.SliderRestTranslateX;
            double initialX = this.sliderTransform.TranslateX;

            this.CancelHighlight();
            this.SlideStoryBoard.Stop();
            this.SlideStoryBoard.Children.Clear();

            //Dispatcher.BeginInvoke(() =>
            //{
                IEasingFunction easing = new QuarticEase() { EasingMode = EasingMode.EaseOut };

                // Animate back to rest position
                DoubleAnimation xTransAnim = new DoubleAnimation()
                {
                    From = initialX,
                    To = finalX,
                    Duration = TimeSpan.FromMilliseconds(750),
                    EasingFunction = easing,
                    };
                Storyboard.SetTarget(xTransAnim, this.sliderTransform);
                Storyboard.SetTargetProperty(xTransAnim, new PropertyPath(CompositeTransform.TranslateXProperty));
                this.SlideStoryBoard.Children.Add(xTransAnim);
                
                this.SlideStoryBoard.Begin();
            //});


            
        }


        void CancelHighlight()
        {
            this.HighlightStoryBoard.Stop();
            this.sizeContainerTransform.ScaleX = 1.0;
            this.sizeContainerTransform.ScaleY = 1.0;
            //this.sizeDownContainerTransform.ScaleX = 1.0;
            //this.sizeDownContainerTransform.ScaleY = 1.0;
            //this.sizeUpContainerTransform.ScaleX = 1.0;
            //this.sizeUpContainerTransform.ScaleY = 1.0;
            //this.sizeDownContainer.Opacity = 1.0;
            //this.sizeUpContainer.Opacity = 1.0;
        }

        void gestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            // Avoid sliding twice when already dragged beyond one of the thresholds
            if (this.PendingSlide == Slide.None)
            {
                if (Math.Abs(e.HorizontalVelocity) > this.FlickVelocityThreshold)
                {
                    this.CancelHighlight();
                    // In the size down direction
                    if (e.HorizontalVelocity > 0)
                    {
                        // If no more measurements, just do a token jump in the direction
                        if (App.VM.SelectedConversionData.NoneSmaller)
                        {
                            this.sliderTransform.TranslateX = this.SliderRestTranslateX + this.RestrictedDragThreshold;
                        }
                        else
                        {
                            this.SwapInNextSizeDown(0);
                        }
                    }
                        // In the size up direction
                    else
                    {
                        // If no more measurements, just do a token jump in the direction
                        if (App.VM.SelectedConversionData.NoneBigger)
                        {
                            this.sliderTransform.TranslateX = this.SliderRestTranslateX - this.RestrictedDragThreshold;
                        }
                        else
                        {
                            this.SwapInNextSizeUp(0);
                        }
                    }
                    this.SlideToCentralMeasurement();
                }
            }
        }

        // Switch the slider to the next size up
        void SwapInNextSizeUp(double delta)
        {
            App.VM.SelectedConversionData.TweakSizeUp();
            this.SwapRulerImages();
            this.sliderTransform.TranslateX += (delta + this.SliderContainerWidth);
            Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromMilliseconds(10));

        }

        // Switch the slider tot he next size down
        void SwapInNextSizeDown(double delta)
        {
            App.VM.SelectedConversionData.TweakSizeDown();
            this.SwapRulerImages();
            this.sliderTransform.TranslateX += (delta - this.SliderContainerWidth);
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

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion




    }
}