﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Pocketailor.Model;

namespace Pocketailor.View
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            this.LoadSettingsIntoPage();

#if DEBUG
            this.debugOnlyContainer.Visibility = Visibility.Visible;
#endif

        }

        private void LoadSettingsIntoPage()
        {
            UnitCultureId uId = App.VM.UnitCulture;
            bool isMetric = (uId == UnitCultureId.Metric);
            this.metricRadioBtn.IsChecked = isMetric;
            this.imperialRadioBtn.IsChecked = !isMetric;

            if (App.VM.AllowFeedBack != null)
            {
                this.allowFeedbackRadioBtn.IsChecked = App.VM.AllowFeedBack;
                this.disallowFeedbackRadioBtn.IsChecked = !App.VM.AllowFeedBack;
            }

        }


        private void imperialRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.UnitCulture = UnitCultureId.Imperial;
        }


        private void metricRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.UnitCulture = UnitCultureId.Metric;
        }

        private void saveAppBarIconBtn_Click(object sender, System.EventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

        private void allowFeedbackRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.AllowFeedBack = true;
        }

        private void disallowFeedbackRadioBtn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.VM.AllowFeedBack = false;
        }

        private void debugBtn1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
#if DEBUG
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Clear();
#endif
        }

        private void debugBtn2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
#if DEBUG
            this.debugTextBlock1.Text = String.Empty;
            foreach (string k in System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Keys)
            {
                this.debugTextBlock1.Text += k + Environment.NewLine;
            }
#endif
        }


    }
}