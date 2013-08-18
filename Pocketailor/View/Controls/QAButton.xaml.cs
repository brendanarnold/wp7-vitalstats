using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace Pocketailor.View.Controls
{
    public partial class QAButton : UserControl
    {
        public QAButton()
        {
            InitializeComponent();

            if (!TiltEffect.TiltableItems.Contains(typeof(QAButton)))
                TiltEffect.TiltableItems.Add(typeof(QAButton));
        }

        // Title
        public string Title
        {
            get { return this.questionTextBlock.Text; }
            set { this.questionTextBlock.Text = value; }
        }

        // Element name of answer box
        public string AnswerContainerName { get; set; }
        public double AnswerContainerHeight { get; set; }

        public static QAButton CurrentlyShowing { get; set; }


        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this == CurrentlyShowing)
            {
                this.HideAnswer();
                CurrentlyShowing = null;
            }
            else
            {
                if (CurrentlyShowing != null) CurrentlyShowing.HideAnswer();
                this.ShowAnswer();
                CurrentlyShowing = this;
            }
        }

        public void ShowAnswer()
        {
            FrameworkElement answerContainer = ViewHelpers.FindSiblingByName(this, this.AnswerContainerName);
            Dispatcher.BeginInvoke(() =>
            {
                IEasingFunction easeOut = new QuarticEase() { EasingMode = EasingMode.EaseOut };

                Storyboard sb = new Storyboard();


                // Animate the height
                DoubleAnimation heightAnim = new DoubleAnimation()
                {
                    From = 0,
                    To = this.AnswerContainerHeight,
                    Duration = TimeSpan.FromMilliseconds(750),
                    EasingFunction = easeOut,
                };
                Storyboard.SetTargetProperty(heightAnim, new PropertyPath(FrameworkElement.HeightProperty));
                Storyboard.SetTarget(heightAnim, answerContainer);
                sb.Children.Add(heightAnim);
                // 'Animate' the visibility
                ObjectAnimationUsingKeyFrames visibilityAnim = new ObjectAnimationUsingKeyFrames();
                visibilityAnim.KeyFrames.Add(new DiscreteObjectKeyFrame()
                {
                    KeyTime = TimeSpan.FromMilliseconds(0),
                    Value = Visibility.Visible,
                });
                Storyboard.SetTarget(visibilityAnim, answerContainer);
                Storyboard.SetTargetProperty(visibilityAnim, new PropertyPath(FrameworkElement.VisibilityProperty));
                sb.Children.Add(visibilityAnim);

                // Add child animations
                //var children = ViewHelpers.GetChildren(answerContainer);
                //double offset = 0;
                //foreach (FrameworkElement child in children)
                //{
                //    offset += 100;
                //    DoubleAnimation rotAnim = new DoubleAnimation()
                //    {
                //        From = 90,
                //        To = 0,
                //        Duration = TimeSpan.FromMilliseconds(1000),
                //        EasingFunction = easeOut,
                //        BeginTime = TimeSpan.FromMilliseconds(offset),
                //    };
                //    Storyboard.SetTargetProperty(rotAnim, new PropertyPath(PlaneProjection.RotationXProperty));
                //    Storyboard.SetTarget(rotAnim, child);
                //    sb.Children.Add(rotAnim);
                //}

                sb.Begin();

            });
        }

        public void HideAnswer()
        {
            FrameworkElement answerContainer = ViewHelpers.FindSiblingByName(this, this.AnswerContainerName);
            Dispatcher.BeginInvoke(() =>
            {
                IEasingFunction easeOut = new QuarticEase() { EasingMode = EasingMode.EaseOut };

                Storyboard sb = new Storyboard();

                // Animate the height
                DoubleAnimation heightAnim = new DoubleAnimation()
                {
                    From = this.AnswerContainerHeight,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(750),
                    EasingFunction = easeOut,
                };
                Storyboard.SetTargetProperty(heightAnim, new PropertyPath(FrameworkElement.HeightProperty));
                Storyboard.SetTarget(heightAnim, answerContainer);
                sb.Children.Add(heightAnim);

                // 'Animate' the visibility
                ObjectAnimationUsingKeyFrames visibilityAnim = new ObjectAnimationUsingKeyFrames();
                visibilityAnim.KeyFrames.Add(new DiscreteObjectKeyFrame()
                {
                    KeyTime = TimeSpan.FromMilliseconds(750),
                    Value = Visibility.Collapsed,
                });
                Storyboard.SetTarget(visibilityAnim, answerContainer);
                Storyboard.SetTargetProperty(visibilityAnim, new PropertyPath(FrameworkElement.VisibilityProperty));
                sb.Children.Add(visibilityAnim);

                // Add child animations
                //var children = ViewHelpers.GetChildren(answerContainer);
                //double offset = 0;
                //foreach (FrameworkElement child in children)
                //{
                //    offset += 100;
                //    DoubleAnimation rotAnim = new DoubleAnimation()
                //    {
                //        From = 90,
                //        To = 0,
                //        Duration = TimeSpan.FromMilliseconds(1000),
                //        EasingFunction = easeOut,
                //        BeginTime = TimeSpan.FromMilliseconds(offset),
                //    };
                //    Storyboard.SetTargetProperty(rotAnim, new PropertyPath(PlaneProjection.RotationXProperty));
                //    Storyboard.SetTarget(rotAnim, child);
                //    sb.Children.Add(rotAnim);
                //}

                sb.Begin();

            });
        }



    }
}
