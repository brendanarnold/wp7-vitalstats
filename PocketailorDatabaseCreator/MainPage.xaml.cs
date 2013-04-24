using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PocketailorDatabaseCreator
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

        }

        

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // In case wat to do twice
            this.doneTextBlock.Visibility = Visibility.Collapsed;

            this.readyTextBlock.Visibility = Visibility.Collapsed;
            this.workingTextBlock.Visibility = Visibility.Visible;

            App.ViewModel.LoadUpCsv();

            this.workingTextBlock.Visibility = Visibility.Collapsed;
            this.doneTextBlock.Visibility = Visibility.Visible;

        }
    }
}