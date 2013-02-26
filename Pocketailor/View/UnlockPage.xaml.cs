using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Pocketailor.View
{
    public partial class UnlockPage : PhoneApplicationPage
    {
        public UnlockPage()
        {
            InitializeComponent();
        }

        private void nounListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.UpdatePinTextBox();
        }

        private void adverbListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.UpdatePinTextBox();
        }

        private void actionListPicker_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            this.UpdatePinTextBox();
        }

        private void placeListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.UpdatePinTextBox();
        }

        public void UpdatePinTextBox()
        {
            int[] inds = new int[] {0, 0, 0, 0};
            string s = String.Empty;
            char c;

            if (this.nounListPicker == null) return;
            if (this.adverbListPicker == null) return;
            if (this.actionListPicker == null) return;
            if (this.placeListPicker == null) return;

            inds[0] = this.nounListPicker.SelectedIndex;
            inds[1] = this.adverbListPicker.SelectedIndex;
            inds[2] = this.actionListPicker.SelectedIndex;
            inds[3] = this.placeListPicker.SelectedIndex;

            for (int i = 0; i < 4; i++)
            {
                c = (inds[i] > 0) ? (inds[i] - 1).ToString()[0] : '-';
                s = s + c;
            }

            this.pinTextBox.Text = s;

        }

        private void ApplicationBarIconButton_Click_1(object sender, System.EventArgs e)
        {
            if (App.VM.TryUnlock(this.pinTextBox.Text))
            {
                if (NavigationService.CanGoBack) NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Please try a different PIN combination", "Incorrect PIN", MessageBoxButton.OK);
            }
            
        }

    }
}