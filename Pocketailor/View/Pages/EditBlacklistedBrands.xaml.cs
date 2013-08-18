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
    public partial class EditBlacklistedBrands : PhoneApplicationPage
    {
        public EditBlacklistedBrands()
        {
            InitializeComponent();

            this.DataContext = App.VM;

            

        }




        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!e.IsNavigationInitiator)
            {
                List<BrandId> blacklistedBrands = App.VM.Brands.Where(x => x.Selected == false).Select(x => x.Id).ToList();
                if (PhoneApplicationService.Current.State.ContainsKey("EditBlacklistedBrandsPageBlacklisted"))
                {
                    PhoneApplicationService.Current.State["EditBlacklistedBrandsPageBlacklisted"] = blacklistedBrands;
                }
                else
                {
                    PhoneApplicationService.Current.State.Add("EditBlacklistedBrandsPageBlacklisted", blacklistedBrands);
                }
            }

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!e.IsNavigationInitiator)
            {
                if (PhoneApplicationService.Current.State.ContainsKey("EditBlacklistedBrandsPageBlacklisted"))
                {
                    List<BrandId> blacklistedBrands = (List<BrandId>)PhoneApplicationService.Current.State["EditBlacklistedBrandsPageBlacklisted"];
                    PhoneApplicationService.Current.State.Remove("EditBlacklistedBrandsPageBlacklisted");
                    foreach (Pocketailor.ViewModel.AppViewModel.BrandContainer b in App.VM.Brands)
                    {
                        b.Selected = !blacklistedBrands.Contains(b.Id);
                    }
                }
            }
            else
            {
                foreach (Pocketailor.ViewModel.AppViewModel.BrandContainer b in App.VM.Brands)
                {
                    b.Selected = !App.VM.BlacklistedBrands.Contains(b.Id);
                }
            }
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (App.VM.SuggestedOptionsVisible)
            {
                e.Cancel = true;
                App.VM.SuggestedOptionsVisible = false;
            }
            else
            {
                App.VM.SkipLoadConversionPageData = true;
                base.OnBackKeyPress(e);
            }
        }


        private void saveApplicationBarIconBtn_Click(object sender, System.EventArgs e)
        {
            List<BrandId> newList = App.VM.Brands.Where(x => x.Selected == false).Select(x => x.Id).ToList();
            newList.Sort();
            App.VM.BlacklistedBrands.Sort();
            if (!newList.SequenceEqual(App.VM.BlacklistedBrands))
            {
                App.VM.BlacklistedBrands = newList;
                App.VM.SaveBlacklistedBrands();
            }
            else
            {
                App.VM.SkipLoadConversionPageData = true;
            }
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
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

        private void suggestBrandByEmailBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.EmailAuthor("Brand suggestion for Pocketailor");
        }


        private void suggestbrandByTwitterBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	App.VM.ViewTwitter("@LassivCo #Pocketailor needs: ");
        }

        private void suggestBrandBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SuggestedOptionsVisible = !App.VM.SuggestedOptionsVisible;
            //if (App.VM.SuggestedOptionsVisible)
            //    VisualStateManager.GoToState(this, "optionsVisibleVisualState", true);
            //else
            //    VisualStateManager.GoToState(this, "optionsCollapsedVisualState", true);
        }


        


    }
}