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

        private List<LocalAdjustment> _adjustments;
        public List<LocalAdjustment> Adjustments {
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
            this.Adjustments = App.Settings.GetValueOrDefault<List<LocalAdjustment>>("Adjustments", new List<LocalAdjustment>());
        }

        public void FreeAdjustments()
        {
            this.Adjustments = null;
        }

        public void SaveAdjustments()
        {
            App.Settings.AddOrUpdateValue("Adjustments", this.Adjustments);
            App.Settings.Save();
        }

        #endregion


        #region Queued adjustments cache

        private List<FeedbackAdjustment> _queuedFeedback;
        public List<FeedbackAdjustment> QueuedFeedback
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
            this.QueuedFeedback = App.Settings.GetValueOrDefault<List<FeedbackAdjustment>>("QueuedFeedback", new List<FeedbackAdjustment>());
        }

        public void FreeQueuedFeedback()
        {
            this.QueuedFeedback = null;
        }

        public void SaveQueuedFeedback()
        {
            App.Settings.AddOrUpdateValue("QueuedFeedback", this.QueuedFeedback);
            App.Settings.Save();
        }

        #endregion




    }
}
