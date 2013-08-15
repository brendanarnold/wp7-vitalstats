using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Pocketailor.View.Controls
{
    public partial class NewlyUnlockedConversionNotificationBtn : UserControl
    {
        public NewlyUnlockedConversionNotificationBtn()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty ConversionNameProperty =
                    DependencyProperty.Register("ConversionName", typeof(string), typeof(NewlyUnlockedConversionNotificationBtn),
                    new PropertyMetadata(String.Empty, new PropertyChangedCallback(ConversionNamePropertyChanged)));

        public static void ConversionNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NewlyUnlockedConversionNotificationBtn mBtn = (NewlyUnlockedConversionNotificationBtn)d;
            mBtn.conversionNameTextBlock.Text = (string)e.NewValue;
        }

        public string ConversionName
        {
            get { return (string)GetValue(ConversionNameProperty); }
            set { SetValue(ConversionNameProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty =
                    DependencyProperty.Register("IsVisible", typeof(bool), typeof(NewlyUnlockedConversionNotificationBtn),
                    new PropertyMetadata(false, new PropertyChangedCallback(IsVisiblePropertyChanged)));

        public static void IsVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NewlyUnlockedConversionNotificationBtn mBtn = (NewlyUnlockedConversionNotificationBtn)d;
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
            {
                mBtn.bounceStoryBoard.Begin();
            }
            else
            {
                mBtn.bounceStoryBoard.Stop();
            }
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }


    }
}
