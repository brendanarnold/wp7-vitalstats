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
    public partial class MeasurementBtn : UserControl
    {
        public MeasurementBtn()
        {
            InitializeComponent();

            if (!TiltEffect.TiltableItems.Contains(typeof(MeasurementBtn)))
                TiltEffect.TiltableItems.Add(typeof(MeasurementBtn));

        }


        public void GoToHasMeasurementState()
        {
            this.LayoutRoot.Opacity = 1.0;
        }

        public void GoToNoMeasurementState()
        {
            this.LayoutRoot.Opacity = 0.7;
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



        public static readonly DependencyProperty ValueTextProperty =
                    DependencyProperty.Register("ValueText", typeof(string), typeof(MeasurementBtn),
                    new PropertyMetadata(null, new PropertyChangedCallback(ValueTextPropertyChanged)));

        public static void ValueTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MeasurementBtn mBtn = (MeasurementBtn)d;
            string val = (string)e.NewValue;
            
            if (val == null || val == String.Empty)
            {
                mBtn.GoToNoMeasurementState();
                mBtn.valueTextBlock.Text = "";
            }
            else
            {
                mBtn.GoToHasMeasurementState();
                mBtn.valueTextBlock.Text = val;
            }
            
        }

        public string ValueText
        {
            get { return (string)GetValue(ValueTextProperty); }
            set { SetValue(ValueTextProperty, value); }
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
