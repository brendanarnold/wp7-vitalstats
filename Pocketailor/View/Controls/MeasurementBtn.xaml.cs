using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Pocketailor.Model;

namespace Pocketailor.View.Controls
{
    public partial class MeasurementBtn : UserControl
    {
        public MeasurementBtn()
        {
            InitializeComponent();

            if (!TiltEffect.TiltableItems.Contains(typeof(MeasurementBtn)))
                TiltEffect.TiltableItems.Add(typeof(MeasurementBtn));

            this.HasMeasurements = false;

        }


        public bool HasMeasurements { get; set; }

        public void GoToHasMeasurementState()
        {
            this.LayoutRoot.Background = App.Current.Resources["TileBGColor"] as Brush;
            this.missingMeasurementImage.Visibility = Visibility.Collapsed;
            this.valueContainer.Visibility = Visibility.Visible;
            this.HasMeasurements = true;
        }

        public void GoToNoMeasurementState()
        {
            this.LayoutRoot.Background = App.Current.Resources["InactiveTileBGColor"] as Brush;
            this.missingMeasurementImage.Visibility = Visibility.Visible;
            this.valueContainer.Visibility = Visibility.Collapsed;
            this.HasMeasurements = false;
        }



        public static readonly DependencyProperty MeasurementNameProperty =
                    DependencyProperty.Register("MeasurementName", typeof(string), typeof(MeasurementBtn),
                    new PropertyMetadata(String.Empty, new PropertyChangedCallback(MeasurementNamePropertyChanged)));

        public static void MeasurementNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MeasurementBtn mBtn = (MeasurementBtn)d;
            mBtn.titleTextBlock.Text = (string)e.NewValue;
        }

        public string MeasurementName
        {
            get { return (string)GetValue(MeasurementNameProperty); }
            set { SetValue(MeasurementNameProperty, value); }
        }



        public static readonly DependencyProperty UnitCultureProperty =
                    DependencyProperty.Register("UnitCulture", typeof(UnitCultureId?), typeof(MeasurementBtn),
                    new PropertyMetadata(null, new PropertyChangedCallback(UnitCulturePropertyChanged)));

        public static void UnitCulturePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MeasurementBtn mBtn = (MeasurementBtn)d;
            UnitCultureId? val = (UnitCultureId?)e.NewValue;
            if (val == UnitCultureId.Imperial)
            {
                mBtn.showImperialStoryBoard.Begin();
            }
            else
            {
                mBtn.showMetricStoryBoard.Begin();
            }
        }

        public UnitCultureId? UnitCulture
        {
            get { return (UnitCultureId?)GetValue(UnitCultureProperty); }
            set { SetValue(UnitCultureProperty, value); }
        }




        public static readonly DependencyProperty ImperialValueTextProperty =
                    DependencyProperty.Register("ImperialValueText", typeof(string), typeof(MeasurementBtn),
                    new PropertyMetadata(null, new PropertyChangedCallback(ImperialValueTextPropertyChanged)));

        public static void ImperialValueTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MeasurementBtn mBtn = (MeasurementBtn)d;
            string val = (string)e.NewValue;
            
            // Only do the state change in the imperial value event handler to prevent doubling up of animation etc.
            if (val == null || val == String.Empty)
            {
                if (mBtn.HasMeasurements) mBtn.GoToNoMeasurementState();
                mBtn.imperialValueTextBlock.Text = "";
            }
            else
            {
                if (!mBtn.HasMeasurements) mBtn.GoToHasMeasurementState();
                mBtn.imperialValueTextBlock.Text = val;
            }
            
        }

        public string ImperialValueText
        {
            get { return (string)GetValue(ImperialValueTextProperty); }
            set { SetValue(ImperialValueTextProperty, value); }
        }


        public static readonly DependencyProperty MetricValueTextProperty =
                    DependencyProperty.Register("MetricValueText", typeof(string), typeof(MeasurementBtn),
                    new PropertyMetadata(null, new PropertyChangedCallback(MetricValueTextPropertyChanged)));

        public static void MetricValueTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MeasurementBtn mBtn = (MeasurementBtn)d;
            string val = (string)e.NewValue;

            if (val == null || val == String.Empty)
            {
                mBtn.metricValueTextBlock.Text = "";
            }
            else
            {
                mBtn.metricValueTextBlock.Text = val;
            }

        }

        public string MetricValueText
        {
            get { return (string)GetValue(MetricValueTextProperty); }
            set { SetValue(MetricValueTextProperty, value); }
        }


        public static readonly DependencyProperty IsCandidateProperty =
                    DependencyProperty.Register("IsCandidate", typeof(bool), typeof(MeasurementBtn),
                    new PropertyMetadata(false, new PropertyChangedCallback(IsCandidatePropertyChanged)));

        public static void IsCandidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MeasurementBtn mBtn = (MeasurementBtn)d;
            bool isCandidate = (bool)e.NewValue;
            if (isCandidate)
            {
                mBtn.candidateStoryBoard.Begin();
            }
            else
            {
                mBtn.candidateStoryBoard.Stop();
            }
        }

        public bool IsCandidate
        {
            get { return (bool)GetValue(IsCandidateProperty); }
            set { SetValue(IsCandidateProperty, value); }
        }




    }
}
