using Pocketailor.Model.Adjustments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    // A place to store some of the frequently accessed part of IsolatedStorage, located on the App namespace
    public class Cache
    {
        // Call when tombstoning
        public void SaveCache()
        {
            this.SaveAdjustments();
            this.SaveQueuedAdjustments();
        }


        #region Adjustments cache

        private List<Adjustment> _adjustments;
        public List<Adjustment> Adjustments {
            get
            {
                if (this._adjustments == null)
                {
                    this.LoadAdjustments();
                }
                return this._adjustments;
            }
            set
            {
                if (this._adjustments != value)
                {
                    this._adjustments = value;
                }
            }
        }

        public void LoadAdjustments()
        {
            this.Adjustments = App.Settings.GetValueOrDefault<List<Adjustment>>("Adjustments", new List<Adjustment>());
        }

        public void FreeAdjustments()
        {
            this.Adjustments = null;
        }

        public void SaveAdjustments()
        {
            App.Settings.AddOrUpdateValue("Adjustments", this.Adjustments);
        }

        #endregion


        #region Queued adjustments cache

        private List<Adjustment> _queuedAdjustments;
        public List<Adjustment> QueuedAdjustments
        {
            get
            {
                if (this._queuedAdjustments == null)
                {
                    this.LoadQueuedAdjustments();
                }
                return this._queuedAdjustments;
            }
            set
            {
                if (this._queuedAdjustments != value)
                {
                    this._queuedAdjustments = value;
                }
            }
        }

        public void LoadQueuedAdjustments()
        {
            this.QueuedAdjustments = App.Settings.GetValueOrDefault<List<Adjustment>>("QueuedAdjustments", new List<Adjustment>());
        }

        public void FreeQueuedAdjustments()
        {
            this.QueuedAdjustments = null;
        }

        public void SaveQueuedAdjustments()
        {
            App.Settings.AddOrUpdateValue("QueuedAdjustements", this.QueuedAdjustments);
        }

        #endregion




    }
}
