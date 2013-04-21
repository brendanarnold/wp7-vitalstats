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

        private bool NewPinWarningSeen = false;

        public UnlockPage()
        {
            InitializeComponent();



            this.nounListPicker.ItemsSource = new List<string>() 
            {
                "[noun]",
                "goldfish",
                "cat",
                "dog",
                "boss",
                "first love",
                "towel",
                "high-school teacher",
                "doctor",
                "mouse"
            };

            this.adverbListPicker.ItemsSource = new List<string>()
            {
                "[adverb]",
                "happily",
                "sadly",
                "quickly",
                "slowly",
                "angrily",
                "peacefully",
                "hopefully",
                "recklessly",
                "disappointedly",
                "drunkenly"
            };
            this.actionListPicker.ItemsSource = new List<string>()
            {
                "[action]",
                "rents a boat",
                "pays taxes",
                "watches television",
                "applies makeup",
                "makes the bed",
                "mows the lawn",
                "milks the cow",
                "throws the discus",
                "follows the tour guide",
                "gives to charity"
            };
            this.placeListPicker.ItemsSource = new List<string>()
            {
                "[place]",
                "in Hong Kong",
                "on prehistoric Earth",
                "on the moon",
                "under the sea",
                "at the North Pole",
                "in Kindergarten",
                "in the cinema",
                "in the garden shed",
                "at a Beatles concert",
                "up the Eiffel tower"
            };

           // this.Loaded +=UnlockPage_Loaded;


        }

        //private void UnlockPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if ((App.VM.GetPin() == null) && !this.NewPinWarningSeen)
        //    {
        //        MessageBox.Show("Entering a PIN on this page will set the PIN for the app. This cannot be retrieved once set and so must be remembered.", "New PIN", MessageBoxButton.OK);
        //        this.NewPinWarningSeen = true;
        //    }
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);



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

        //private void ApplicationBarIconButton_Click_1(object sender, System.EventArgs e)
        //{
        //    if (!App.VM.IsValidPin(this.pinTextBox.Text))
        //    {
        //        MessageBox.Show("Make sure the PIN field contains a four digit number only", "Invalid PIN entered", MessageBoxButton.OK);
        //        return;
        //    }
        //    else if (App.VM.TryUnlock(this.pinTextBox.Text))
        //    {
        //        if (NavigationService.CanGoBack) NavigationService.GoBack();
                
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please try a different PIN combination", "Incorrect PIN", MessageBoxButton.OK);
        //    }
            
        //}

        private void pinTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.pinTextBox.Text = String.Empty;
        }

    }




}