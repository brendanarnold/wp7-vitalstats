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
    public partial class ConversionValueButton : UserControl
    {
        public ConversionValueButton()
        {
            InitializeComponent();

            if (!TiltEffect.TiltableItems.Contains(typeof(ConversionValueButton)))
                TiltEffect.TiltableItems.Add(typeof(ConversionValueButton));

        }

        // Is nullable so the IsHiddenPropertyChanged event is fired since it will always be assigned from null to true or false
        // Alternate is to design button in one state e.g. Hidden state and set the default PropertyMetaData to true rather than null
        public static readonly DependencyProperty IsHiddenProperty =
            DependencyProperty.Register("IsHidden", typeof(bool?), typeof(ConversionValueButton),
            new PropertyMetadata(null, new PropertyChangedCallback(IsHiddenPropertyChanged)));

        private static void IsHiddenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionValueButton cBtn = (ConversionValueButton)d;
            bool val = (bool)e.NewValue;
            if (val)
            {
                cBtn.toggleHiddenMenuItem.Header = "unhide retailer";
                cBtn.retailerTextBlock.Opacity = 0.5;
                cBtn.clothingSizeTextBlock.Opacity = 0.5;
            }
            else
            {
                cBtn.toggleHiddenMenuItem.Header = "hide retailer";
                cBtn.retailerTextBlock.Opacity = 1.0;
                cBtn.clothingSizeTextBlock.Opacity = 1.0;
            }
        }

        public bool? IsHidden
        {
            get { return (bool)GetValue(IsHiddenProperty); }
            set { SetValue(IsHiddenProperty, value); }
        }


        public static readonly DependencyProperty RetailerNameProperty =
            DependencyProperty.Register("RetailerName", typeof(string), typeof(ConversionValueButton),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback(RetailerNamePropertyChanged)));

        public static void RetailerNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionValueButton cBtn = (ConversionValueButton)d;
            string val = (string)e.NewValue;
            cBtn.retailerTextBlock.Text = val;
        }

        public string RetailerName
        {
            get { return (string)GetValue(RetailerNameProperty); }
            set { SetValue(RetailerNameProperty, value); }
        }


        public static readonly DependencyProperty ClothingSizeProperty =
            DependencyProperty.Register("ClothingSize", typeof(string), typeof(ConversionValueButton),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback(ClothingSizePropertyChanged)));

        public static void ClothingSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionValueButton cBtn = (ConversionValueButton)d;
            string val = (string)e.NewValue;
            cBtn.clothingSizeTextBlock.Text = val;
        }

        public string ClothingSize
        {
            get { return (string)GetValue(ClothingSizeProperty); }
            set { SetValue(ClothingSizeProperty, value); }
        }


        public void toggleHideRetailerContextMenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ViewModel.AppViewModel.NameValuePair nvp = (sender as MenuItem).DataContext as ViewModel.AppViewModel.NameValuePair;
            nvp.ToggleHidden();
        }


    }
}
