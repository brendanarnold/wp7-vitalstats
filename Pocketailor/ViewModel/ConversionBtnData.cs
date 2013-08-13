using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    // A container class for the conversion data
    public class ConversionBtnData : INotifyPropertyChanged
    {
        private bool _hasMeasurements;
        public bool HasMeasurements
        {
            get { return this._hasMeasurements; }
            set
            {
                if (this._hasMeasurements != value)
                {
                    this._hasMeasurements = value;
                    this.NotifyPropertyChanged("HasMeasurements");
                }
            }
        }


        private List<MeasurementId> _requiredMeasurementsMale;
        public List<MeasurementId> RequiredMeasurementsMale
        {
            get { return this._requiredMeasurementsMale; }
            set
            {
                if (this._requiredMeasurementsMale != value)
                {
                    this._requiredMeasurementsMale = value;
                    this.NotifyPropertyChanged("RequiredMeasurementsMale");
                }
            }
        }

        private List<MeasurementId> _requiredMeasurementsFemale;
        public List<MeasurementId> RequiredMeasurementsFemale
        {
            get { return this._requiredMeasurementsFemale; }
            set
            {
                if (this._requiredMeasurementsFemale != value)
                {
                    this._requiredMeasurementsFemale = value;
                    this.NotifyPropertyChanged("RequiredMeasurementsFemale");
                }
            }
        }

        public bool HasRequiredMeasurements
        {
            get
            {
                List<MeasurementId> requiredIds;
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    requiredIds = this.RequiredMeasurementsMale;
                }
                else
                {
                    requiredIds = this.RequiredMeasurementsFemale;
                }
                IEnumerable<MeasurementId> measurementIds = from Measurement s in App.VM.SelectedProfile.Measurements select s.MeasurementId;
                foreach (MeasurementId id in requiredIds)
                    if (!measurementIds.Contains(id)) return false;
                return true;
            }
        }

        public List<MeasurementId> MissingMeasurements
        {
            get
            {
                List<MeasurementId> requiredIds;
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    requiredIds = this.RequiredMeasurementsMale;
                }
                else
                {
                    requiredIds = this.RequiredMeasurementsFemale;
                }
                IEnumerable<MeasurementId> alreadyTakenMeasurementIds = from Measurement s in App.VM.SelectedProfile.Measurements select s.MeasurementId;
                return requiredIds.Except(alreadyTakenMeasurementIds).ToList();

            }
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    }



}
