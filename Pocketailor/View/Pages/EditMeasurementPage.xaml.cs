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
    public partial class EditMeasurementPage : PhoneApplicationPage
    {
       
        // Long list selector triggers the OnNavigatedToEvent again, use a flag to avoid triggering the reinitialisation of the measurement
        public bool MeasurementInitialised = false;

        public EditMeasurementPage()
        {
            InitializeComponent();
            this.DataContext = App.VM;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.MeasurementInitialised) return;

            // Parse out the URI and send to the ViewModel
            string s = String.Empty;
            NavigationContext.QueryString.TryGetValue("ProfileId", out s);
            int profileId = int.Parse(s);
            NavigationContext.QueryString.TryGetValue("MeasurementId", out s);
            MeasurementId mId = (MeasurementId)Enum.Parse(typeof(MeasurementId), s, true);
            App.VM.LoadEditMeasurementData(profileId, mId);

            // Load values into the page from ViewModel
            this.LoadMeasurementIntoPage();

            // Take a snapshot to determine if changes have been made later
            this.TakeSnapshotOfForm();

            this.MeasurementInitialised = true;

        }

        private void LoadMeasurementIntoPage()
        {
            // Set the unit name from the selected measurement
            this.pageTitleTextBlock.Text = (App.VM.SelectedMeasurement.Name == null) ? String.Empty : App.VM.SelectedMeasurement.Name.ToLower();
            this.preferredUnitListPicker.ItemsSource = App.VM.SelectedMeasurement.MeasurementType.Units;
            if (App.VM.SelectedMeasurement.PreferredUnit != null)
            {
                this.preferredUnitListPicker.SelectedItem = App.VM.SelectedMeasurement.PreferredUnit;
            }
            else
            {
                this.preferredUnitListPicker.SelectedItem = App.VM.SelectedMeasurement.MeasurementType.DefaultUnit;
            }
            // Load up the value
            if (App.VM.SelectedMeasurement.Value != null) this.LoadValueToTextBox();
            // Load up the help text
            this.helpTextBlock.Text = this.GetHelpText(App.VM.SelectedMeasurement.MeasurementId, App.VM.SelectedProfile.Gender);
            this.helpImg.Source = this.GetHelpImg(App.VM.SelectedMeasurement.MeasurementId, App.VM.SelectedProfile.Gender);
        }

        // Save a snapshot of the form into FormSnapshot
        private void TakeSnapshotOfForm() 
        {
            App.VM.EditMeasurementFormSnapshot = new ViewModel.EditMeasurementFormSnapshot() {
                Value = this.ReadValueFromTextBox(),
                PreferredUnitIndex = this.preferredUnitListPicker.SelectedIndex,
            };
        }

        // Use the FormSnaptshot to determine if there is unsaved data entered
        public bool IsUnsavedData()
        {
            if (App.VM.EditMeasurementFormSnapshot == null) return true;
            List<string> a = this.ReadValueFromTextBox();
            List<string> b = App.VM.EditMeasurementFormSnapshot.Value;
            if (a == null && b != null) return true;
            if (a != null && b == null) return true;
            if (a != null && b != null && !a.SequenceEqual(b)) return true;
            if (this.preferredUnitListPicker.SelectedIndex != App.VM.EditMeasurementFormSnapshot.PreferredUnitIndex) return true;
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
            if (this.ReadValueFromTextBox() == null)
            {
                MessageBox.Show("The measurement must be number and cannot be empty. Please enter a valid value.", "Value empty or invalid", MessageBoxButton.OK);
                return false;
            }
            return true;
        }


        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (!this.ValidateInput()) return;

            // Read values into the Measurement object to be saved
            List<string> valStr = this.ReadValueFromTextBox();
            App.VM.SelectedMeasurement.PreferredUnit = (this.preferredUnitListPicker.SelectedItem as IUnit);
            App.VM.SelectedMeasurement.Value = App.VM.SelectedMeasurement.PreferredUnit.ConvertToDBString(valStr);
            if (App.VM.EditMeasurementsPageAction == ViewModel.EditMeasurementPageActions.Edit) 
            {
                App.VM.SaveChangesToAppDB();
            }
            else
            {
                App.VM.AddMeasurementToProfile(App.VM.SelectedMeasurement, App.VM.SelectedProfile);
                App.VM.SwapInToFullMeasurements(App.VM.SelectedMeasurement);
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
                List<string> vals = pu.ConvertFromDBString(App.VM.SelectedMeasurement.Value);
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
                (this.valueContainer.Children[0] as ValueUnitTextBox).ValueText = App.VM.SelectedMeasurement.Value;
            }
        }

        // Combines text from the value textboxes depending on whether there are more than one value asked for
        // May have many texboxes for multiple inputs or may have non-numerical input
        // Returns String.Empty if all are empty or error encountered
        private List<string> ReadValueFromTextBox()
        {
            List<string> sVals = new List<string>();
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

        public BitmapImage GetHelpImg(MeasurementId measurementId, GenderId gender)
        {
            string fn = (gender == GenderId.Male) ? App.VM.HelpData.HelpImgMale[measurementId] : App.VM.HelpData.HelpImgFemale[measurementId];
            return new BitmapImage(new Uri(AppConstants.HELP_IMAGE_DIRECTORY + fn, UriKind.Relative));
        }

        public string GetHelpText(MeasurementId measurementId, GenderId gender)
        {
            return App.VM.HelpData.HelpText[measurementId];
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

    


    




}
