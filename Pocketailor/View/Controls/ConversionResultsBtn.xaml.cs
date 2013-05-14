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
using System.ComponentModel;

namespace Pocketailor.View.Controls
{
    public partial class ConversionResultsBtn : UserControl, INotifyPropertyChanged
    {
        public ConversionResultsBtn()
        {
            InitializeComponent();

            //if (!TiltEffect.TiltableItems.Contains(typeof(ConversionResultsBtn)))
            //    TiltEffect.TiltableItems.Add(typeof(ConversionResultsBtn));


            

        }

        // IsBlacklistedPropertyChanged event is fired on instantiation only if the bound property is different to the default value
        // so need to design the button to be in the default value state at instantiation
        public static readonly DependencyProperty BtnIsBlacklistedProperty =
            DependencyProperty.Register("BtnIsBlacklisted", 
                typeof(bool), 
                typeof(ConversionResultsBtn),
                new PropertyMetadata(false, new PropertyChangedCallback(BtnIsBlacklistedPropertyChanged))
            );

        private static void BtnIsBlacklistedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionResultsBtn cBtn = (ConversionResultsBtn)d;
            bool val = (bool)e.NewValue;
            if (val)
            {
                cBtn.conversionResultContainerGrid.Opacity = 0.5;
            }
            else
            {
                cBtn.conversionResultContainerGrid.Opacity = 1.0;
            }
            cBtn.NotifyPropertyChanged("BtnIsBlacklisted");
        }

        public bool BtnIsBlacklisted
        {
            get { return (bool)GetValue(BtnIsBlacklistedProperty); }
            set { SetValue(BtnIsBlacklistedProperty, value); }
        }

        // ShowBlacklistedPropertyChanged event is fired on instantiation only if the bound property is different to the default value
        // so need to design the button to be in the default value state at instantiation
        public static readonly DependencyProperty BtnShowBlacklistedProperty =
            DependencyProperty.Register("BtnShowBlacklisted", typeof(bool), typeof(ConversionResultsBtn),
            new PropertyMetadata(true, new PropertyChangedCallback(BtnShowBlacklistedPropertyChanged)));

        private static void BtnShowBlacklistedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionResultsBtn cBtn = (ConversionResultsBtn)d;
            bool val = (bool)e.NewValue;
            if (val)
            {
                cBtn.notBlacklistedCheckBox.Visibility = Visibility.Visible;
                cBtn.Visibility = Visibility.Visible;
            }
            else
            {
                cBtn.notBlacklistedCheckBox.Visibility = Visibility.Collapsed;
                if ((bool)cBtn.BtnIsBlacklisted)
                {
                    cBtn.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool BtnShowBlacklisted
        {
            get { return (bool)GetValue(BtnShowBlacklistedProperty); }
            set { SetValue(BtnShowBlacklistedProperty, value); }
        }




        public static readonly DependencyProperty BtnBrandNameProperty =
            DependencyProperty.Register("BtnBrandName", typeof(string), typeof(ConversionResultsBtn),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback(BtnBrandNamePropertyChanged)));

        public static void BtnBrandNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //ConversionResultsBtn cBtn = (ConversionResultsBtn)d;
            //string val = (string)e.NewValue;
            //cBtn.brandNameTextBlock.Text = val;
        }

        public string BtnBrandName
        {
            get { return (string)GetValue(BtnBrandNameProperty); }
            set { SetValue(BtnBrandNameProperty, value); }
        }


        public static readonly DependencyProperty BtnClothingSizeProperty =
            DependencyProperty.Register("BtnClothingSize", typeof(string), typeof(ConversionResultsBtn),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback(BtnClothingSizePropertyChanged)));

        public static void BtnClothingSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //ConversionResultsBtn cBtn = (ConversionResultsBtn)d;
            //string val = (string)e.NewValue;
            //cBtn.clothingSizeTextBlock.Text = val;
        }

        public string BtnClothingSize
        {
            get { return (string)GetValue(BtnClothingSizeProperty); }
            set { SetValue(BtnClothingSizeProperty, value); }
        }



        private void EnterAdjustConversionState()
        {
            this.adjustmentContainerGrid.Visibility = Visibility.Visible;
            this.questionTextBlock.Visibility = Visibility.Visible;
            this.AdjustmentAnimation.Begin();

        }

        private void LeaveAdjustConversionState()
        {
            this.adjustmentContainerGrid.Visibility = Visibility.Collapsed;
            this.questionTextBlock.Visibility = Visibility.Collapsed;
            this.AdjustmentAnimation.Stop();
        }

        private void AbortAdjustment() 
        {
            this.LeaveAdjustConversionState();
            ConversionData c = this.DataContext as ConversionData;
            c.ResetAdjustment();
        }

        private void conversionGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.adjustmentContainerGrid.Visibility == Visibility.Collapsed)
            {
                this.EnterAdjustConversionState();
            }
            else
            {
                this.AbortAdjustment();
            }
        }

        private void tooBigBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = this.DataContext as ConversionData;
            c.TryAdjustment(-1);
        }

        private void rightSizeBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.AllowFeedBack == null) this.PromptForFeedbackPermission();
            this.LeaveAdjustConversionState();
            ConversionData c = this.DataContext as ConversionData;
            c.AdjustValue();
        }

        private void tooSmallBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = this.DataContext as ConversionData;
            c.TryAdjustment(1);
        }

       

        private void PromptForFeedbackPermission()
        {

            MessageBoxResult res = MessageBox.Show("You can send any adjustments to the Pocketailor team anonymously over the web so we can improve our conversions. If you would like to help then click 'ok' below, if you would rather not then click 'cancel'."
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
