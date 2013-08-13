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

namespace Pocketailor.View
{
    public partial class EditRegionPage : PhoneApplicationPage
    {
        public EditRegionPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

        }



        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!e.IsNavigationInitiator)
            {
                string rId = App.VM.Regions.Where(x => x.Selected == true).Select(x => x.Id).FirstOrDefault();
                if (PhoneApplicationService.Current.State.ContainsKey("EditRegionPageSelected"))
                {
                    PhoneApplicationService.Current.State["EditRegionPageSelected"] = rId;
                }
                else
                {
                    PhoneApplicationService.Current.State.Add("EditRegionPageSelected", rId);
                }
            }

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!e.IsNavigationInitiator)
            {
                if (PhoneApplicationService.Current.State.ContainsKey("EditRegionPageSelected"))
                {
                    string rId = (string)PhoneApplicationService.Current.State["EditRegionPageSelected"];
                    PhoneApplicationService.Current.State.Remove("EditRegionPageSelected");
                    foreach (Pocketailor.ViewModel.AppViewModel.RegionContainer r in App.VM.Regions)
                    {
                        r.Selected = (r.Id == rId);
                    }
                }
            }
            else
            {
                foreach (Pocketailor.ViewModel.AppViewModel.RegionContainer r in App.VM.Regions)
                {
                    r.Selected = (r.Id == App.VM.SelectedRegion);
                }

            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            App.VM.SkipLoadConversionPageData = true;
            base.OnBackKeyPress(e);
        }

        private void saveApplicationBarIconBtn_Click(object sender, System.EventArgs e)
        {
            string rId = (App.VM.Regions.Where(x => x.Selected == true).Select(x => x.Id).FirstOrDefault());
            if (rId != App.VM.SelectedRegion)
            {
                App.VM.SelectedRegion = rId;
            } 
            else
            {
                App.VM.SkipLoadConversionPageData = true;
            }
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.AppViewModel.RegionContainer checkedContainer = (sender as RadioButton).DataContext as ViewModel.AppViewModel.RegionContainer;
            foreach (ViewModel.AppViewModel.RegionContainer r in App.VM.Regions)
            {
                r.Selected = (r == checkedContainer);
            }
        }

        #region Animation

        private LongListSelector currentSelector;

        private void LongListSelector_GroupViewOpened(object sender, GroupViewOpenedEventArgs e)
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

        private void LongListSelector_GroupViewClosing(object sender, GroupViewClosingEventArgs e)
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





    }
}