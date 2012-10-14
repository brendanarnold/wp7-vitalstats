using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Globalization;
using VitalStats.ViewModelNamespace;

namespace VitalStats.View
{
    public partial class MainPage : PhoneApplicationPage
    {

        private ViewModel vm;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            vm = new ViewModel();
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!GlobalState.IsLaunching && this.State.ContainsKey("Profiles"))
            {
                vm = (ViewModel)this.State["Profiles"];
            }
            else
            {
                vm.GetProfiles();
            }
            profileListBox.DataContext = from Profile in vm.Profiles select Profile;


        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (this.State.ContainsKey("Profiles"))
            {
                this.State["Profiles"] = vm;
            }
            else
            {
                this.State.Add("Profiles", vm);
            }
            GlobalState.IsLaunching = false;
        }

    }


    // Convertor for the locked icon visibility
    public class BoolToVisibility : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
           return (Visibility)value == Visibility.Visible;
           
        }

    }


}