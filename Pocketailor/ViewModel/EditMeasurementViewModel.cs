using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel
    {
        // Should the page be geared towards editing or new measurements
        public EditMeasurementPageActions EditMeasurementsPageAction;

        // A container for comparison when submitting to check if data has been entered
        public EditMeasurementFormSnapshot EditMeasurementFormSnapshot { get; set; }

        // Store the help text
        private HelpData _helpData;
        public HelpData HelpData
        {
            get
            {
                if (this._helpData == null)
                {
                    this._helpData = new HelpData();
                }
                return this._helpData;
            }
        }

        // Call OnNavigatingTo to make sure that you have all the necessary state
        public void LoadEditMeasurementData(int profileId, MeasurementId mId)
        {
            // Load up the profile
            if (this.SelectedProfile == null 
                || this.SelectedProfile.Id != profileId)
            {
                this.SelectedProfile = this.Profiles.Where(p => p.Id == profileId).First();
            }
            // Determine if it is a new measurement or an edit
            this.EditMeasurementsPageAction =  (this.SelectedProfile.Measurements.Select(m => m.MeasurementId).Contains(mId)) ?
                EditMeasurementPageActions.Edit : EditMeasurementPageActions.New;
            // Load up the selected measurement if necessary
            if (this.SelectedMeasurement == null ||
                this.SelectedMeasurement.Profile.Id != profileId ||
                this.SelectedMeasurement.MeasurementId != mId)
            {
                if (this.EditMeasurementsPageAction == EditMeasurementPageActions.New)
                {
                    MeasurementTemplate st = this.MeasurementTemplates.Where(t => t.Id == mId).First();
                    this.SelectedMeasurement = new Measurement()
                    {
                        Name = st.Name,
                        MeasurementType = st.MeasurementType,
                        MeasurementId = st.Id,
                    };
                }
                else
                {
                    this.SelectedMeasurement = this.SelectedProfile.Measurements.Where(m => m.MeasurementId == mId).First();
                }
            }
        }





    }

    public class EditMeasurementFormSnapshot
    {
        //public string Name;
        public List<string> Value { get; set; }
        public int? PreferredUnitIndex { get; set; }
    }

    public enum EditMeasurementPageActions
    {
        New,
        Edit,
    }


}
