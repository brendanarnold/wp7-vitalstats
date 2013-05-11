using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Pocketailor.Model.Conversions;

namespace Pocketailor.View.Controls
{
    public partial class ConversionResultsBtn : UserControl
    {
        public ConversionResultsBtn()
        {
            InitializeComponent();

            if (!TiltEffect.TiltableItems.Contains(typeof(ConversionValueButton)))
                TiltEffect.TiltableItems.Add(typeof(ConversionValueButton));

        }

        // Is nullable so the IsHiddenPropertyChanged event is fired since it will always be assigned from null to true or false
        // Alternate is to design button in one state e.g. Hidden state and set the default PropertyMetaData to true rather than null
        public static readonly DependencyProperty IsHiddenProperty =
            DependencyProperty.Register("IsHidden", typeof(bool?), typeof(ConversionResultsBtn),
            new PropertyMetadata(null, new PropertyChangedCallback(IsHiddenPropertyChanged)));

        private static void IsHiddenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionResultsBtn cBtn = (ConversionResultsBtn)d;
            bool val = (bool)e.NewValue;
            if (val)
            {
                cBtn.hideBrandBtn.Content = "unhide brand";
                cBtn.conversionResultContainerGrid.Opacity = 0.5;
            }
            else
            {
                cBtn.hideBrandBtn.Content = "hide brand";
                cBtn.conversionResultContainerGrid.Opacity = 1.0;
            }
        }

        public bool? IsHidden
        {
            get { return (bool)GetValue(IsHiddenProperty); }
            set { SetValue(IsHiddenProperty, value); }
        }



        public static readonly DependencyProperty BrandNameProperty =
            DependencyProperty.Register("BrandName", typeof(string), typeof(ConversionResultsBtn),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback(BrandNamePropertyChanged)));

        public static void BrandNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionResultsBtn cBtn = (ConversionResultsBtn)d;
            string val = (string)e.NewValue;
            cBtn.brandNameTextBlock.Text = val;
        }

        public string BrandName
        {
            get { return (string)GetValue(BrandNameProperty); }
            set { SetValue(BrandNameProperty, value); }
        }


        public static readonly DependencyProperty ClothingSizeProperty =
            DependencyProperty.Register("ClothingSize", typeof(string), typeof(ConversionResultsBtn),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback(ClothingSizePropertyChanged)));

        public static void ClothingSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionResultsBtn cBtn = (ConversionResultsBtn)d;
            string val = (string)e.NewValue;
            cBtn.clothingSizeTextBlock.Text = val;
        }

        public string ClothingSize
        {
            get { return (string)GetValue(ClothingSizeProperty); }
            set { SetValue(ClothingSizeProperty, value); }
        }





        private void conversionGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.adjustmentContainerGrid.Visibility == Visibility.Collapsed)
            {
                this.adjustmentContainerGrid.Visibility = Visibility.Visible;
            }
            else
            {
                this.adjustmentContainerGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void tooBigBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Button).DataContext as ConversionData;
            App.VM.ApplyAdjustment(c, 1);
        }

        private void tooSmallBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Button).DataContext as ConversionData;
            App.VM.ApplyAdjustment(c, -1);
        }

        private void hideBrandBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Button).DataContext as ConversionData;
            c.ToggleHidden();
        }



    }
}
