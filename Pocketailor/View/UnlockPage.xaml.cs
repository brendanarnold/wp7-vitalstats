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


            this.nounListPicker.ItemsSource = new List<StringHolder>() 
            {
                new StringHolder(){ Name="[noun]"},
                new StringHolder(){ Name="goldfish"},
                new StringHolder(){ Name="cat"},
                new StringHolder(){ Name="dog"},
                new StringHolder(){ Name="boss"},
                new StringHolder(){ Name="first love"},
                new StringHolder(){ Name="towel"},
                new StringHolder(){ Name="high-school teacher"},
                new StringHolder(){ Name="doctor"},
                new StringHolder(){ Name="mouse"}
            };
            this.adverbListPicker.ItemsSource = new List<StringHolder>()
            {
                new StringHolder(){ Name="[adverb]"},
                new StringHolder(){ Name="happily"},
                new StringHolder(){ Name="sadly"},
                new StringHolder(){ Name="quickly"},
                new StringHolder(){ Name="slowly"},
                new StringHolder(){ Name="angrily"},
                new StringHolder(){ Name="peacefully"},
                new StringHolder(){ Name="hopefully"},
                new StringHolder(){ Name="recklessly"},
                new StringHolder(){ Name="disappointedly"},
                new StringHolder(){ Name="drunkenly"}
            };
            this.actionListPicker.ItemsSource = new List<StringHolder>()
            {
                new StringHolder(){ Name="[action]"},
                new StringHolder(){ Name="rents a boat"},
                new StringHolder(){ Name="pays taxes"},
                new StringHolder(){ Name="watches television"},
                new StringHolder(){ Name="applies makeup"},
                new StringHolder(){ Name="makes the bed"},
                new StringHolder(){ Name="mows the lawn"},
                new StringHolder(){ Name="milks the cow"},
                new StringHolder(){ Name="throws the discus"},
                new StringHolder(){ Name="follows the tour guide"},
                new StringHolder(){ Name="gives to charity"}
            };
            this.placeListPicker.ItemsSource = new List<StringHolder>()
            {
                new StringHolder(){ Name="[place]"},
                new StringHolder(){ Name="in Hong Kong"},
                new StringHolder(){ Name="on prehistoric Earth"},
                new StringHolder(){ Name="on the moon"},
                new StringHolder(){ Name="under the sea"},
                new StringHolder(){ Name="at the North Pole"},
                new StringHolder(){ Name="in Kindergarten"},
                new StringHolder(){ Name="in the cinema"},
                new StringHolder(){ Name="in the garden shed"},
                new StringHolder(){ Name="at a Beatles concert"},
                new StringHolder(){ Name="up the Eiffel tower"}
            };

            this.Loaded +=UnlockPage_Loaded;


        }

        private void UnlockPage_Loaded(object sender, RoutedEventArgs e)
        {
            if ((App.VM.GetPin() == null) && !this.NewPinWarningSeen)
            {
                MessageBox.Show("Entering a PIN on this page will set the PIN for the app. This cannot be retrieved once set and so must be remembered.", "New PIN", MessageBoxButton.OK);
                this.NewPinWarningSeen = true;
            }
        }

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

        private void ApplicationBarIconButton_Click_1(object sender, System.EventArgs e)
        {
            if (!App.VM.IsValidPin(this.pinTextBox.Text))
            {
                MessageBox.Show("Make sure the PIN field contains a four digit number only", "Invalid PIN entered", MessageBoxButton.OK);
                return;
            }
            else if (App.VM.TryUnlock(this.pinTextBox.Text))
            {
                if (NavigationService.CanGoBack) NavigationService.GoBack();
                
            }
            else
            {
                MessageBox.Show("Please try a different PIN combination", "Incorrect PIN", MessageBoxButton.OK);
            }
            
        }

    }


    // A class to bind a simple string to a ListPicker
    public class StringHolder
    {
        public string Name { get; set; }
    }


}