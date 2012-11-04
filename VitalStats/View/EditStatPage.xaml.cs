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
using System.Windows.Data;

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

            if (NavigationContext.QueryString.ContainsKey("Action"))
            {
                // Deal with various actions
                switch (NavigationContext.QueryString["Action"])
                {
                    case EditStatPageActions.Edit:
                        // Flush DB so any changes can be detected
                        App.VM.SaveChangesToDB();
                        this.IsNewStat = false;
                        int id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
                        App.VM.SelectedStat = (from Stat s in App.VM.SelectedProfile.Stats where s.Id == id select s).First();
                        break;
                    case EditStatPageActions.NewFromTemplate:
                        App.VM.SelectedStat = new Stat();
                        int templateId = Convert.ToInt32(NavigationContext.QueryString["Id"]);
                        this.templateListPicker.SelectedItem = (from Stat s in App.VM.StatTemplates where s.Id == templateId select s).First();
                        break;
                    case EditStatPageActions.New:
                        App.VM.SelectedStat = new Stat();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                App.VM.SelectedStat = new Stat();
            }
            this.BindSelectedStat();

        }

        private void BindSelectedStat()
        {
            Binding b;
            this.templateListPicker.ItemsSource = null;

            b = new Binding("PreferredUnit");
            b.Mode = BindingMode.TwoWay;
            b.Source = App.VM.SelectedStat;
            this.preferredUnitListPicker.SetBinding(ListPicker.SelectedItemProperty, b);

            b = new Binding("Name");
            b.Mode = BindingMode.TwoWay;
            b.Source = App.VM.SelectedStat;
            this.nameTextBox.SetBinding(TextBox.TextProperty, b);

            b = new Binding("MeasurementType");
            b.Mode = BindingMode.TwoWay;
            b.Source = App.VM.SelectedStat;
            this.measurementTypeListPicker.SetBinding(ListPicker.SelectedItemProperty, b);

            // TODO deal with multi entry
            
        }

        private void templateListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Convention of Id = -1 defined in Converter.cs
            Stat st = this.templateListPicker.SelectedItem as Stat;
            if (st == null) return;
            if ((st.Id != -1) && (this.measurementTypeListPicker.Items.Count > 0))
            {
                this.nameTextBox.Text = st.Name;
                this.measurementTypeListPicker.SelectedItem = st.MeasurementType;
            }
        }

        private void measurementTypeListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.preferredUnitListPicker.ItemsSource = (this.measurementTypeListPicker.SelectedItem as MeasurementType).Units;
        }

        public bool IsDataChanged()
        {
            if (this.IsNewStat)
            {
                if (this.nameTextBox.Text != String.Empty) return true;
                if (this.valueTextBox.Text != String.Empty) return true;
                if (((this.preferredUnitListPicker.SelectedItem as Unit).GetNumberInputs() > 1)
                    && (this.secondaryValueTextBox.Text != String.Empty)) return true;
            }
            else
            {

            }
        }


        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {

            // TODO: This is only for new stats, need to do actions for edited stats
            if (this.nameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter something for the name.");
                return;
            }
            if (this.valueTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter something in the value box");
                return;
            }
            if ((this.secondaryValueTextBox.Visibility == Visibility.Visible) && (this.secondaryValueTextBox.Text == String.Empty))
            {
                MessageBox.Show("Please enter something in the second value box");
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
                double d;
                if (!double.TryParse(this.valueTextBox.Text, out d))
                {
                    MessageBox.Show("Please enter a number as the value.");
                    return;
                }
                Unit prefUnit = this.preferredUnitListPicker.SelectedItem as Unit;
                string strValue = prefUnit.GetDBValue(this.valueTextBox.Text);
                App.VM.SelectedProfile.Stats.Add(new Stat()
                {
                    Name = this.nameTextBox.Text,
                    Value = strValue,
                    MeasurementType = this.measurementTypeListPicker.SelectedItem as MeasurementType,
                    PreferredUnit = prefUnit,
                });
            }

            App.VM.SaveChangesToDB();

            this.ClearInput();

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

        }

        // Wipes the form clean
        // TODO: Do the bound items need resetting in some way? 
        private void ClearInput()
        {
            this.nameTextBox.Text = String.Empty;
            this.valueTextBox.Text = String.Empty;
        }

        // Hide or show the secondary value text box if needed
        private void preferredUnitListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.preferredUnitListPicker.SelectedItem != null)
            {
                Unit u = this.preferredUnitListPicker.SelectedItem as Unit;
                if (u.GetNumberInputs() > 1)
                {
                    this.secondaryValueTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    this.secondaryValueTextBox.Visibility = Visibility.Collapsed;
                }
            }

        }

    }


    public static class EditStatPageActions
    {
        public const string NewFromTemplate = "NewFromTemplate";
        public const string New = "New";
        public const string Edit = "Edit";
    }


}
