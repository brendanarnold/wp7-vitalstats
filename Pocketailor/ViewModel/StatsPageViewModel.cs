using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel
    {

        #region HasMeasurement properties

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
            IEnumerable<MeasurementId> statIds = from Stat s in this.SelectedProfile.Stats select s.MeasurementId;
            foreach (MeasurementId id in requiredIds)
                if (!statIds.Contains(id)) return false;
            return true;
        }

        public bool HasTrouserMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.TrousersUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.TrousersUtils.RequiredMeasurementsWomens);
                }
            }
        }

        public bool HasShirtMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.ShirtUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.ShirtUtils.RequiredMeasurementsWomens);
                }

            }
        }

        public bool HasHatMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.HatUtils.RequiredMeasurements);
            }
        }

        public bool HasSuitMeasurements
        {
            get
            {
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.SuitUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.SuitUtils.RequiredMeasurementsWomens);
                }

            }
        }

        public bool HasDressSizeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.DressSizeUtils.RequiredMeasurements);
            }
        }

        public bool HasBraMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.BraUtils.RequiredMeasurements);
            }
        }

        public bool HasHosieryMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.HosieryUtils.RequiredMeasurements);
            }
        }

        public bool HasShoeMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.ShoesUtils.RequiredMeasurements);
            }
        }

        public bool HasSkiBootMeasurements
        {
            get
            {
                return this.HasRequiredMeasurements(Model.Conversions.SkiBootsUtils.RequiredMeasurements);
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
                if (App.VM.SelectedProfile.Gender == Gender.Male)
                {
                    return this.HasRequiredMeasurements(Model.Conversions.WetsuitUtils.RequiredMeasurementsMens);
                }
                else
                {
                    return this.HasRequiredMeasurements(Model.Conversions.WetsuitUtils.RequiredMeasurementsWomens);
                }
            }
        }

        #endregion



    }
}
