﻿using System;
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
using VitalStats.View.Controls;

namespace VitalStats.View
{
    public partial class EditStatPage : PhoneApplicationPage
    {
        // Determines whether page should render for a new stat or editing an existing stat
        public bool IsNewStat;

        // A container for comparison when submitting to check if data has been entered
        EditStatFormSnapshot FormSnapshot;

        public EditStatPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

            // Load some of the data types if not done so already
            if (App.VM.MeasurementTypes == null) App.VM.LoadMeasurementTypesFromDB();
            if (App.VM.StatTemplates == null) App.VM.LoadStatTemplatesFromDB();

            // Need to bind explicitly for some reason when this particular ListPicker
            // control is inside a ScrollViewer
            this.templateListPicker.ItemsSource = App.VM.StatTemplates;
            this.measurementTypeListPicker.ItemsSource = App.VM.MeasurementTypes;



        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Default action is add a new item
            this.IsNewStat = true;
            string action = EditProfilePageActions.New;
            NavigationContext.QueryString.TryGetValue("Action", out action);
            int id = 0;
            if (NavigationContext.QueryString.ContainsKey("Id"))
            {
                id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
            }

            // Deal with various actions
            switch (action)
            {
                case EditStatPageActions.Edit:
                    this.IsNewStat = false;
                    VisualStateManager.GoToState(this, "EditState", false);
                    App.VM.SelectedStat = (from Stat s in App.VM.SelectedProfile.Stats where s.Id == id select s).First();
                    this.templateListPicker.ItemsSource = null;
                    this.LoadValueToTextBox();
                    break;
                case EditStatPageActions.NewFromTemplate:
                    this.IsNewStat = true;
                    VisualStateManager.GoToState(this, "AddState", false);
                    App.VM.SelectedStat = new Stat();
                    this.templateListPicker.SelectedItem = (from Stat s in App.VM.StatTemplates where s.Id == id select s).First();
                    break;
                case EditStatPageActions.New:
                    this.IsNewStat = true;
                    VisualStateManager.GoToState(this, "AddStat", false);
                    App.VM.SelectedStat = new Stat();
                    break;
                default:
                    break;
            }

            // Load values into the page
            this.LoadStatIntoPage();
            // Take a snapshot to determine if changes have been made
            this.TakeSnapshotOfForm();

        }

        private void LoadStatIntoPage()
        {
            if (App.VM.SelectedStat.Name != null) this.nameTitledTextBox.Text = App.VM.SelectedStat.Name;
            if (App.VM.SelectedStat.MeasurementType != null) this.measurementTypeListPicker.SelectedItem = App.VM.SelectedStat.MeasurementType;
            if (App.VM.SelectedStat.PreferredUnit != null) this.preferredUnitListPicker.SelectedItem = App.VM.SelectedStat.PreferredUnit;
            if (App.VM.SelectedStat.Value != null) this.LoadValueToTextBox();
        }

        private void TakeSnapshotOfForm() 
        {
            this.FormSnapshot = new EditStatFormSnapshot() {
                Name = this.nameTitledTextBox.Text,
                Value = this.ReadValueFromTextBox(),
                MeasurementTypeIndex = this.measurementTypeListPicker.SelectedIndex,
                PreferredUnitIndex = this.preferredUnitListPicker.SelectedIndex,
            };
        }

        public bool IsUnsavedData()
        {
            if (this.nameTitledTextBox.Text != this.FormSnapshot.Name) return true;
            if (this.ReadValueFromTextBox() != this.FormSnapshot.Value) return true;
            if (this.measurementTypeListPicker.SelectedIndex != this.FormSnapshot.MeasurementTypeIndex) return true;
            if (this.preferredUnitListPicker.SelectedIndex != this.FormSnapshot.PreferredUnitIndex) return true;
            return false;
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (this.IsUnsavedData())
            {
                MessageBoxResult res = MessageBox.Show("You have entered some data which has not been saved. Are you sure you want to return to the previous page?",
                    "Discard unsaved changes?", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }


        // Allows non-numeric values when the stat type is custom i.e. does not allow for conversions
        public bool AllowNonNumericValue()
        {
            if (this.measurementTypeListPicker.SelectedItem != null)
            {
                return !(this.measurementTypeListPicker.SelectedItem as MeasurementType).IsConvertible();
            }
            else
            {
                return true;
            }
        }


        // Returns false if input is invalid, true otherwise. 
        // Also raises error to the user via messageboxes
        public bool ValidateInput()
        {
            if (this.nameTitledTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter something for the name.");
                return false;
            }

            if (this.ReadValueFromTextBox() == String.Empty)
            {
                MessageBox.Show("Please enter valid values");
                return false;
            }
            return true;
        }


        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (!this.ValidateInput()) return;

            // Read values into the Stat object to be saved
            string valStr = this.ReadValueFromTextBox();
            App.VM.SelectedStat.Name = this.nameTitledTextBox.Text;
            if (this.AllowNonNumericValue())
            {
                App.VM.SelectedStat.Value = valStr;
                App.VM.SelectedStat.PreferredUnit = null;
                App.VM.SelectedStat.MeasurementType = null;
            }
            else
            {
                App.VM.SelectedStat.PreferredUnit = (this.preferredUnitListPicker.SelectedItem as Unit);
                App.VM.SelectedStat.Value = App.VM.SelectedStat.PreferredUnit.ConvertValuesToString(valStr);
                App.VM.SelectedStat.MeasurementType = (this.measurementTypeListPicker.SelectedItem as MeasurementType);
            }
            if (this.IsNewStat) {
                App.VM.SelectedStat.Profile = App.VM.SelectedProfile;
                App.VM.SelectedProfile.Stats.Add(App.VM.SelectedStat);
                App.VM.AddStat(App.VM.SelectedStat);
            }
            else
            {
                App.VM.SaveChangesToDB();
            }

            this.ClearInput();

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

        }


        /// <summary>
        /// Reads value from the object and splits it into necessary parts in each text box
        /// </summary>
        private void LoadValueToTextBox()
        {
            if (this.preferredUnitListPicker.SelectedItem != null)
            {
                Unit pu = this.preferredUnitListPicker.SelectedItem as Unit;
                List<double> vals = pu.ConvertValuesFromString(App.VM.SelectedStat.Value);
                for (int i = 0; i < this.valueContainer.Children.Count; i++)
                {
                    if (i < vals.Count)
                    {
                        (this.valueContainer.Children[i] as TitledTextBox).Text = String.Format("{0:F3}", vals[i]);
                    }
                    else
                    {
                        (this.valueContainer.Children[i] as TitledTextBox).Text = String.Empty;
                    }
                }
            }
            else
            {
                this.value1TitledTextBox.Text = App.VM.SelectedStat.Value;
            }
        }

        // Combines text from the value textboxes depending on whether there are more than one value asked for
        // May have many texboxes for multiple inputs or may have non-numerical input
        // Returns String.Empty if all are empty or error encountered
        private string ReadValueFromTextBox()
        {
            List<string> sVals = new List<string>();
            if (this.AllowNonNumericValue())
            {
                return this.value1TitledTextBox.Text;
            }
            List<double> vals = new List<double>();
            foreach (TitledTextBox tb in this.valueContainer.Children) 
            {
                if (tb.Visibility == Visibility.Visible)
                {
                    double d;
                    if (!double.TryParse(tb.Text, out d))
                    {
                        return String.Empty;
                    }
                    vals.Add(d);    
                }
            }
            return ModelHelpers.PickleDoubles(vals);
        }

        // Wipes the form clean
        // TODO: Do the bound items need resetting in some way? 
        private void ClearInput()
        {
            this.nameTitledTextBox.Text = String.Empty;
            foreach (TitledTextBox tb in this.valueContainer.Children) tb.Text = String.Empty;
            this.measurementTypeListPicker.SelectedIndex = 0;
            this.preferredUnitListPicker.SelectedIndex = 0;
        }


        #region Page element behaviours

        // Hide or show the secondary value text boxes if needed
        private void preferredUnitListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (this.preferredUnitListPicker.SelectedItem != null)
            {
                Unit pu = this.preferredUnitListPicker.SelectedItem as Unit;
                int nVisible = pu.GetNumberInputs();
                int i = 0;
                foreach (TitledTextBox tb in this.valueContainer.Children)
                {
                    if (i < nVisible)
                    {
                        tb.Title = pu.Names[i];
                        tb.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tb.Visibility = Visibility.Collapsed;
                    }
                    i += 1;
                }
            }
        }

        // Updates rest of fields when template is changed
        private void templateListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Convention of Id = -1 defined in Converter.cs
            Stat st = this.templateListPicker.SelectedItem as Stat;
            if (st == null) return;
            if ((st.Id != -1) && (this.measurementTypeListPicker.Items.Count > 0))
            {
                this.nameTitledTextBox.Text = st.Name;
                this.measurementTypeListPicker.SelectedItem = st.MeasurementType;
            }
        }


        private void measurementTypeListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.measurementTypeListPicker.SelectedItem != null)
            {
                this.preferredUnitListPicker.ItemsSource
                    = (this.measurementTypeListPicker.SelectedItem as MeasurementType).Units;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.TakeSnapshotOfForm();
        }


        #endregion

    }

    public struct EditStatFormSnapshot 
    {
        public string Name, Value;
        public int? MeasurementTypeIndex, PreferredUnitIndex;
    }


    public static class EditStatPageActions
    {
        public const string NewFromTemplate = "NewFromTemplate";
        public const string New = "New";
        public const string Edit = "Edit";
    }


}
