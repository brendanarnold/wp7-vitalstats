using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel
    {

       

        // Helkper method to notify the View of possible updates to HasMeasurement properties
        internal void RefreshRequiredMeasurement()
        {
            this.NotifyPropertyChanged("HasDressSizeMeasurements");
            this.NotifyPropertyChanged("HasSuitMeasurements");
            this.NotifyPropertyChanged("HasTrouserMeasurements");
            this.NotifyPropertyChanged("HasShirtMeasurements");
            this.NotifyPropertyChanged("HasHatMeasurements");
            this.NotifyPropertyChanged("HasBraMeasurements");
            this.NotifyPropertyChanged("HasHosieryMeasurements");
            this.NotifyPropertyChanged("HasShoeMeasurements");
            this.NotifyPropertyChanged("HasSkiBootMeasurements");
            this.NotifyPropertyChanged("HasWetsuitMeasurements");
            this.NotifyPropertyChanged("HasTennisGripMeasurements");
        }


        public bool HasRequiredMeasurements(List<MeasurementId> requiredIds)
        {
            IEnumerable<MeasurementId> measurementIds = from Measurement s in this.SelectedProfile.Measurements select s.MeasurementId;
            foreach (MeasurementId id in requiredIds)
                if (!measurementIds.Contains(id)) return false;
            return true;
        }

        #region Bindable properties which call HasRequiredMeasureemnts

        public bool HasTrouserMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.TrousersMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.TrousersWomens);
                }
            }
        }

        public bool HasShirtMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.ShirtMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.ShirtWomens);
                }

            }
        }

        public bool HasHatMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(RequiredMeasurements.Hat);
            }
        }

        public bool HasSuitMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.SuitMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.SuitWomens);
                }

            }
        }

        public bool HasDressSizeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(RequiredMeasurements.DressSize);
            }
        }

        public bool HasBraMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(RequiredMeasurements.Bra);
            }
        }

        public bool HasHosieryMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(RequiredMeasurements.Hosiery);
            }
        }

        public bool HasShoeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(RequiredMeasurements.Shoes);
            }
        }

        public bool HasSkiBootMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(RequiredMeasurements.SkiBoots);
            }
        }

        //public bool HasTennisGripMeasurements
        //{
        //    get
        //    {
        //        return this.HasRequiredMeasurements(Model.Conversions.TennisRaquetSizesUtils.RequiredMeasurements);
        //    }
        //}

        public bool HasWetsuitMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == GenderId.Male)
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.WetsuitMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(RequiredMeasurements.WetsuitWomens);
                }
            }
        }

        #endregion



    }
}
