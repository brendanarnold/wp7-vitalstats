﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Windows.Media;

namespace Pocketailor.View.Controls
{
    public partial class ConversionBtn : UserControl//, INotifyPropertyChanged
    {
        [Description("A button to use on the Conversions page")]
        public ConversionBtn()
        {
            InitializeComponent();

            if (!TiltEffect.TiltableItems.Contains(typeof(ConversionBtn)))
                TiltEffect.TiltableItems.Add(typeof(ConversionBtn));

            this.Opacity = 0.7;


            //this.DataContext = this;

            //if (DesignerProperties.GetIsInDesignMode(this))
            //{
            //    this.Title = "Title Here";
            //    this.Background = new SolidColorBrush(Colors.Green);
            //}

        }

        public new double Height
        {
            get { return this.btnGrid.Height; }
            set { this.btnGrid.Height = value; }
        }

        public new Brush Background
        {
            get { return this.btnGrid.Background; }
            set { this.btnGrid.Background = value; }
        }


        public string Title
        {
            get { return this.titleTxtBlock.Text; }
            set { this.titleTxtBlock.Text = value; }
        }

        public static readonly DependencyProperty HasMeasurementsProperty =
            DependencyProperty.Register("HasMeasurements", typeof(bool), typeof(ConversionBtn),
            new PropertyMetadata(false, new PropertyChangedCallback(HasMeasurementsPropertyChanged)));

        private static void HasMeasurementsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConversionBtn cBtn = (ConversionBtn)d;
            bool val = (bool)e.NewValue;
            if (val)
            {
                cBtn.notAvailableImg.Visibility = Visibility.Collapsed;
                cBtn.Opacity = 1.0;
            }
            else
            {
                cBtn.notAvailableImg.Visibility = Visibility.Visible;
                cBtn.Opacity = 0.7;
            }
            //cBtn.HasMeasurements = val;
                
        }

        public bool HasMeasurements
        {
            get
            {
                return (bool)GetValue(HasMeasurementsProperty);
            }
            set
            {
                SetValue(HasMeasurementsProperty, value);
                //this.NotifyPropertyChanged("HasMeasurements");
            }
        }

        //#region INotifyPropertyChanged members

        //public event PropertyChangedEventHandler PropertyChanged;

        //internal void NotifyPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //#endregion


    }
}