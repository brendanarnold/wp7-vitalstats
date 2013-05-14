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
            this.SaveQueuedFeedback();
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

        private List<Adjustment> _queuedFeedback;
        public List<Adjustment> QueuedFeedback
        {
            get
            {
                if (this._queuedFeedback == null)
                {
                    this.LoadQueuedFeedback();
                }
                return this._queuedFeedback;
            }
            set
            {
                if (this._queuedFeedback != value)
                {
                    this._queuedFeedback = value;
                }
            }
        }

        public void LoadQueuedFeedback()
        {
            this.QueuedFeedback = App.Settings.GetValueOrDefault<List<Adjustment>>("QueuedFeedback", new List<Adjustment>());
        }

        public void FreeQueuedFeedback()
        {
            this.QueuedFeedback = null;
        }

        public void SaveQueuedFeedback()
        {
            App.Settings.AddOrUpdateValue("QueuedFeedback", this.QueuedFeedback);
        }

        #endregion




    }
}
