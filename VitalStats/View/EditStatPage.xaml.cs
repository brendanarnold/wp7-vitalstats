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
using VitalStats.Model;

namespace VitalStats.View
{
    public partial class EditStatPage : PhoneApplicationPage
    {
        // Determines whether page should render for a new stat or editing an existing stat
        public bool IsNewStat = true;

        public EditStatPage()
        {
            InitializeComponent();

            this.IsNewStat = false;

            
            

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = App.VM;

            // Update the stat to be edited
            if (NavigationContext.QueryString.ContainsKey("Id"))
            {
                int id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
                App.VM.SelectedStat = (from Stat s in App.VM.SelectedProfile.Stats where s.Id == id select s).First();
                this.IsNewStat = false;
            }
            

        }

        private void templateListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Convention of Id = -1 defined in Converter.cs
            Stat st = this.templateListPicker.SelectedItem as Stat;
            if (st == null) return;
            if (st.Id != -1)
            {
                this.nameTextBox.Text = st.Name;
                this.measurementTypeListPicker.SelectedItem = st.MeasurementType;
            }
        }

        private void measurementTypeListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.preferredUnitListPicker.ItemsSource = (this.measurementTypeListPicker.SelectedItem as MeasurementType).Units;
        }

        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (this.nameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter something for the name.");
                return;
            }
            if (this.valueTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter a value for the stat");
                return;
            }
            if ((this.measurementTypeListPicker.SelectedItem as MeasurementType).Name == "Other")
            {
                App.VM.SelectedProfile.Stats.Add(new Stat()
                {
                    Name = this.nameTextBox.Text,
                    Value = this.valueTextBox.Text,
                    MeasurementType = null,
                    PreferredUnit = null,
                });
            }
            else
            {
                App.VM.SelectedProfile.Stats.Add(new Stat()
                {
                    Name = this.nameTextBox.Text,
                    Value = this.valueTextBox.Text,
                    MeasurementType = this.measurementTypeListPicker.SelectedItem as MeasurementType,
                    PreferredUnit = this.preferredUnitListPicker.SelectedItem as Unit,
                });
            }

        }



		
		
		
    }
}
