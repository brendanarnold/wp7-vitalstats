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
using Pocketailor.Model;
using System.Windows.Data;
using Pocketailor.View.Controls;
using System.Windows.Media.Imaging;

namespace Pocketailor.View
{
    public partial class EditStatPage : PhoneApplicationPage
    {
        // Determines whether page should render for a new stat or editing an existing stat
        public string PageAction;

        // Long list selector triggers the OnNavigatedToEvent again, use a flag to avoid triggering the reinitialisation of the stat
        public bool StatInitialised = false;

        // A container for comparison when submitting to check if data has been entered
        EditStatFormSnapshot FormSnapshot;

        // Store the help text
        public HelpData HelpData;

        public EditStatPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;
            this.HelpData = new HelpData();


        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.StatInitialised) return;

            if (!NavigationContext.QueryString.TryGetValue("Action", out this.PageAction))
            {
                // Default action is add a new item
                this.PageAction = EditStatPageActions.New;
            }

            // Load values into the page
            this.LoadStatIntoPage();
            // Take a snapshot to determine if changes have been made
            this.TakeSnapshotOfForm();

            this.StatInitialised = true;

        }

        private void LoadStatIntoPage()
        {
            // Set the unit name from the selected stat
            this.pageTitleTextBlock.Text = (App.VM.SelectedStat.Name == null) ? String.Empty : App.VM.SelectedStat.Name.ToLower();
            // Hide the unit selection if the unit type is custom
            if (App.VM.SelectedStat.MeasurementType == null)
            {
                this.preferredUnitListPicker.Visibility = Visibility.Collapsed;
            } 
            else 
            {
                this.preferredUnitListPicker.ItemsSource = App.VM.SelectedStat.MeasurementType.Units;
                if (App.VM.SelectedStat.PreferredUnit != null)
                {
                    this.preferredUnitListPicker.SelectedItem = App.VM.SelectedStat.PreferredUnit;
                }
                else
                {
                    this.preferredUnitListPicker.SelectedItem = App.VM.SelectedStat.MeasurementType.DefaultUnit;
                }
            }
            // Load up the value
            if (App.VM.SelectedStat.Value != null) this.LoadValueToTextBox();
            // Load up the help text
            this.helpTextBlock.Text = this.GetHelpText(App.VM.SelectedStat.MeasurementId, App.VM.SelectedProfile.Gender);
            this.helpImg.Source = this.GetHelpImg(App.VM.SelectedStat.MeasurementId, App.VM.SelectedProfile.Gender);
        }

        // Save a snapshot of the form into FormSnapshot
        private void TakeSnapshotOfForm() 
        {
            this.FormSnapshot = new EditStatFormSnapshot() {
                //Name = this.nameTitledTextBox.Text,
                Value = this.ReadValueFromTextBox(),
                PreferredUnitIndex = this.preferredUnitListPicker.SelectedIndex,
            };
        }

        // Use the FormSnaptshot to determine if there is unsaved data entered
        public bool IsUnsavedData()
        {
            //if (this.nameTitledTextBox.Text != this.FormSnapshot.Name) return true;
            List<string> a = this.ReadValueFromTextBox();
            List<string> b = this.FormSnapshot.Value;
            if (a == null && b != null) return true;
            if (a != null && b == null) return true;
            if (a != null && b != null && !a.SequenceEqual(b)) return true;
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





        // Returns false if input is invalid, true otherwise. 
        // Also raises error to the user via messageboxes
        public bool ValidateInput()
        {
            //if (this.nameTitledTextBox.Text == String.Empty)
            //{
            //    MessageBox.Show("The statistic label cannot be empty. Please enter a value.", "Label missing", MessageBoxButton.OK);
            //    return false;
            //}

            if (this.ReadValueFromTextBox() == null)
            {
                MessageBox.Show("The statistic value must be number unless a custom measurement type is selected. It also cannot be empty. Please enter a valid value.", "Value empty or invalid", MessageBoxButton.OK);
                return false;
            }
            return true;
        }


        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (!this.ValidateInput()) return;

            // Read values into the Stat object to be saved
            List<string> valStr = this.ReadValueFromTextBox();
            //App.VM.SelectedStat.Name = this.nameTitledTextBox.Text;
            if (App.VM.AllowNonNumericValue())
            {
                App.VM.SelectedStat.Value = valStr[0];
                App.VM.SelectedStat.PreferredUnit = null;
                App.VM.SelectedStat.MeasurementType = null;
            }
            else
            {
                App.VM.SelectedStat.PreferredUnit = (this.preferredUnitListPicker.SelectedItem as IUnit);
                App.VM.SelectedStat.Value = App.VM.SelectedStat.PreferredUnit.ConvertToDBString(valStr);
            }
            if (this.PageAction == EditStatPageActions.Edit) 
            {
                App.VM.SaveChangesToDB();
            }
            else
            {
                App.VM.AddStatToProfile(App.VM.SelectedStat, App.VM.SelectedProfile);
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
                IUnit pu = this.preferredUnitListPicker.SelectedItem as IUnit;
                List<string> vals = pu.ConvertFromDBString(App.VM.SelectedStat.Value);
                for (int i = 0; i < pu.ShortUnitNames.Count; i++)
                {
                    ValueUnitTextBox tb = (this.valueContainer.Children[i] as ValueUnitTextBox);
                    tb.UnitText = pu.ShortUnitNames[i];
                    if (vals != null)
                    {
                        tb.ValueText = (i < vals.Count) ? String.Format("{0:F3}", vals[i]) : String.Empty;
                    }
                }
            }
            else
            {
                (this.valueContainer.Children[0] as ValueUnitTextBox).ValueText = App.VM.SelectedStat.Value;
            }
        }

        // Combines text from the value textboxes depending on whether there are more than one value asked for
        // May have many texboxes for multiple inputs or may have non-numerical input
        // Returns String.Empty if all are empty or error encountered
        private List<string> ReadValueFromTextBox()
        {
            List<string> sVals = new List<string>();
            if (App.VM.AllowNonNumericValue())
            {
                sVals.Add((this.valueContainer.Children[0] as ValueUnitTextBox).ValueText);
                return sVals;
            }
            foreach (ValueUnitTextBox tb in this.valueContainer.Children)
            {
                if (tb.Visibility == Visibility.Visible)
                {
                    double d;
                    if (!double.TryParse(tb.ValueText, out d))
                    {
                        return null;
                    }
                    sVals.Add(tb.ValueText);
                }
            }
            return sVals;
        }




        // Wipes the form clean
        // TODO: Do the bound items need resetting in some way? 
        private void ClearInput()
        {
            //this.nameTitledTextBox.Text = String.Empty;
            foreach (ValueUnitTextBox tb in this.valueContainer.Children)
                tb.Clear();
        }

        public BitmapImage GetHelpImg(MeasurementId measurementId, Gender gender)
        {
            string fn = (gender == Gender.Male) ? this.HelpData.HelpImgMale[measurementId] : this.HelpData.HelpImgFemale[measurementId];
            return new BitmapImage(new Uri(AppConstants.HELP_IMAGE_DIRECTORY + fn, UriKind.Relative));
        }

        public string GetHelpText(MeasurementId measurementId, Gender gender)
        {
            return this.HelpData.HelpText[measurementId];
        }


        #region Page element behaviours

        // Hide or show the secondary value text boxes if needed
        private void preferredUnitListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (this.preferredUnitListPicker.SelectedItem != null)
            {
                IUnit pu = this.preferredUnitListPicker.SelectedItem as IUnit;
                int nVisible = pu.ShortUnitNames.Count;
                int i = 0;
                foreach (ValueUnitTextBox tb in this.valueContainer.Children)
                {
                    if (i < nVisible)
                    {
                        tb.UnitText = pu.ShortUnitNames[i];
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

        private void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.TakeSnapshotOfForm();
        }

        private void helpAppBarBtn_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }


        #endregion

    }

    public struct EditStatFormSnapshot 
    {
        //public string Name;
        public List<string> Value;
        public int? PreferredUnitIndex;
    }


    public static class EditStatPageActions
    {
        public const string New = "New";
        public const string Edit = "Edit";
    }


}
