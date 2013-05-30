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

namespace Pocketailor.View
{
    public partial class ConversionAdjustmentPage : PhoneApplicationPage
    {
        public ConversionAdjustmentPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            App.VM.SelectedConversionData.DiscardTweaks();
            App.VM.SkipLoadConversionPageData = true;
            base.OnBackKeyPress(e);
        }

        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (App.VM.AllowFeedBack == null) this.PromptForFeedbackPermission();
            App.VM.SelectedConversionData.AcceptTweaks();
            App.VM.SkipLoadConversionPageData = true;

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }



        private void tooBigBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedConversionData.TweakSizeDown();
        }

        private void tooSmallBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedConversionData.TweakSizeUp();
        }

        private void PromptForFeedbackPermission()
        {

            MessageBoxResult res = MessageBox.Show("You can send your adjustments to the Pocketailor team anonymously over the web so we can make the results better next time. If you would like to help then click 'ok' below, if you would rather not then click 'cancel'."
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
    }
}