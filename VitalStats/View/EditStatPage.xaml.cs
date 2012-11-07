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

            // Load some of the data types if not done so already
            if (App.VM.MeasurementTypes == null) App.VM.LoadMeasurementTypesFromDB();
            if (App.VM.StatTemplates == null) App.VM.LoadStatTemplatesFromDB();

            this.DataContext = App.VM;

            // Flush current pending changes so can tell if changes have been made
            App.VM.SaveChangesToDB();

            // Default action is add a new item
            string action = EditProfilePageActions.New;
            if (NavigationContext.QueryString.ContainsKey("Action")) 
                action = NavigationContext.QueryString["Action"];
                
            // Deal with various actions
            switch (action)
            {
                case EditStatPageActions.Edit:
                    this.IsNewStat = false;
                    int id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
                    App.VM.SelectedStat = (from Stat s in App.VM.SelectedProfile.Stats where s.Id == id select s).First();
                    this.templateListPicker.ItemsSource = null;
                    this.LoadValueToTextBox();
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


        
        //private void BindSelectedStat()
        //{
            //Binding b;

            //b = new Binding("PreferredUnit");
            //b.Mode = BindingMode.TwoWay;
            //b.Source = App.VM.SelectedStat;
            //this.preferredUnitListPicker.SetBinding(ListPicker.SelectedItemProperty, b);

            //b = new Binding("Name");
            //b.Mode = BindingMode.TwoWay;
            //b.Source = App.VM.SelectedStat;
            //this.nameTextBox.SetBinding(TextBox.TextProperty, b);

            //b = new Binding("MeasurementType");
            //b.Mode = BindingMode.TwoWay;
            //b.Source = App.VM.SelectedStat;
            //this.measurementTypeListPicker.SetBinding(ListPicker.SelectedItemProperty, b);

            // TODO deal with multi entry
            
        //}


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (this.IsUnsavedData())
            {
                CustomMessageBox mb = new CustomMessageBox()
                {
                    Caption = "Discard unsaved changes?",
                    Message = "Some details have been changed on this statistic, If you leave they will not be saved. Do you want to continue?",
                    LeftButtonContent = "Yes",
                    RightButtonContent = "No"
                };
                mb.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            App.VM.RecreateDataContext();
                            break;
                        case CustomMessageBoxResult.RightButton:
                            e.Cancel = true;
                            break;
                        default:
                            break;
                    }
                };
                mb.Show();
            }
        }


        // Allows non-numeric values when the stat type is custom i.e. does not allow for conversions
        public bool AllowNonNumericValue()
        {
            return (this.measurementTypeListPicker.SelectedItem as MeasurementType).Name 
                == AppConstants.NAME_CUSTOM_STAT_TEMPLATE;
        }


        public bool IsUnsavedData()
        {
            // Deal with the edited case first
            if (App.VM.IsPendingChangesForDB()) return true;
            if (!this.IsNewStat) return false;
            // Must be a new stat
            if (this.value1TextBox.Text != String.Empty) return true;
            if (this.value2TextBox.Text != String.Empty) return true;
            if (this.value3TextBox.Text != String.Empty) return true;
            if (this.value4TextBox.Text != String.Empty) return true;
            if (this.templateListPicker.SelectedItem == null)
            {
                if (this.nameTextBox.Text != String.Empty) return true;
            }
            else
            {
                if (this.nameTextBox.Text != (templateListPicker.SelectedItem as Stat).Name) return true;
            }
            return false;
        }



        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {

            if (this.nameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter something for the name.");
                return;
            }

            string valStr = this.ReadValueFromTextBox();
            if (valStr == String.Empty)
            {
                MessageBox.Show("Please enter valid values");
            }

            if (this.AllowNonNumericValue())
            {
                App.VM.SelectedStat.PreferredUnit = null;
                App.VM.SelectedStat.MeasurementType = null;
            }
            App.VM.SelectedStat.Value = valStr;
            App.VM.SelectedProfile.Stats.Add(App.VM.SelectedStat);

            App.VM.SaveChangesToDB();

            this.ClearInput();

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

        }


        // Reads value from the object and splits it into necessary parts in each text box
        private void LoadValueToTextBox()
        {
            if (App.VM.SelectedStat.PreferredUnit != null)
            {
                List<double> vals = App.VM.SelectedStat.PreferredUnit.ConvertValuesFromString(App.VM.SelectedStat.Value);
                for (int i = 0; i < this.valueContainer.Children.Count; i++)
                {
                    if (i < vals.Count)
                    {
                        (this.valueContainer.Children[i] as TextBox).Text = String.Format("{0:F3}", vals[i]);
                    }
                    else
                    {
                        (this.valueContainer.Children[i] as TextBox).Text = String.Empty;
                    }
                }
            }
            else
            {
                this.value1TextBox.Text = App.VM.SelectedStat.Value;
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
                return this.value1TextBox.Text;
            }
            List<double> vals = new List<double>();
            foreach (TextBox tb in this.valueContainer.Children) 
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
            return ModelHelpers.JoinList(vals);
        }

        // Wipes the form clean
        // TODO: Do the bound items need resetting in some way? 
        private void ClearInput()
        {
            this.nameTextBox.Text = String.Empty;
            foreach (TextBox tb in this.valueContainer.Children) tb.Text = String.Empty;
        }


        #region Page element behaviours

        // Hide or show the secondary value text boxes if needed
        private void preferredUnitListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i=0;
            if (this.preferredUnitListPicker.SelectedItem != null)
            {
                int nVisible = (this.preferredUnitListPicker.SelectedItem as Unit).GetNumberInputs();
                foreach (TextBox tb in this.valueContainer.Children)
                {
                    if (i < nVisible)
                    {
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
                this.nameTextBox.Text = st.Name;
                this.measurementTypeListPicker.SelectedItem = st.MeasurementType;
            }
        }


        private void measurementTypeListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.preferredUnitListPicker.ItemsSource
                = (this.measurementTypeListPicker.SelectedItem as MeasurementType).Units;
        }


        #endregion

    }


    public static class EditStatPageActions
    {
        public const string NewFromTemplate = "NewFromTemplate";
        public const string New = "New";
        public const string Edit = "Edit";
    }


}
