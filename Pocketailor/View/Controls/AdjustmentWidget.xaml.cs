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

namespace Pocketailor.View.Controls
{
    public partial class AdjustmentWidget : UserControl
    {
        public AdjustmentWidget()
        {
            InitializeComponent();
        }

        private void tooBigBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Button).DataContext as ConversionData;
            c.TweakSizeDown();
        }

        private void rightSizeBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.VM.AllowFeedBack == null) this.PromptForFeedbackPermission();
            ConversionData c = (sender as Button).DataContext as ConversionData;
            c.IsAdjusting = false;
            c.AcceptTweaks();
        }

        private void tooSmallBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ConversionData c = (sender as Button).DataContext as ConversionData;
            c.TweakSizeUp();
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
