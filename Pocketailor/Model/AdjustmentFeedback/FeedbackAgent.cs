using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Pocketailor.Model.AdjustmentFeedback
{
    // Cache the feedback for a few days before sending at an opportune time
    public class FeedbackAgent
    {
        public FeedbackAgent()
        {
            this.LoadCache();
        }

        public List<Adjustment> Adjustments;

        public void CacheAdjustment(Adjustment adj)
        {
            this.Adjustments.Add(adj);
        }

        public void LoadCache()
        {
            this.Adjustments = App.Settings.GetValueOrDefault<List<Adjustment>>("AdjustmentFeedbackCache", new List<Adjustment>());
        }

        // This should be persisted when tombstoning
        public void SaveCache()
        {
            App.Settings.AddOrUpdateValue("AdjustmentFeedbackCache", this.Adjustments);
        }

        // An Async method that sends off the adjustments in batches
        public async System.Threading.Tasks.Task DeliverAdjustmentsTaskAsync()
        {
            if (this.Adjustments.Count == 0) return;
            // Take MAX_ADJUSTMENTS_PER_REQUEST adjustments at a time from the cache and send on
            int attempt = AppConstants.MAX_DELIVERY_ATTEMPTS;
            do
            {
                // Give up after a certain amount of attempts
                if (attempt == 0) break;
                // Take a batch of adjustments 
                int nvals = Math.Min(this.Adjustments.Count, AppConstants.MAX_ADJUSTMENTS_PER_REQUEST);
                List<Adjustment> buff = this.Adjustments.GetRange(0, nvals);
                // JSONify
                string adjustmentsJson = Newtonsoft.Json.JsonConvert.SerializeObject(buff);
                // Send over web
                FormUrlEncodedContent postContent = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { AppConstants.SECRET_REQUEST_POST_FIELD, AppConstants.POCKETAILOR_ADJUSTMENTS_SECRET },
                    { AppConstants.ADJUSTMENT_REQUEST_POST_FIELD, adjustmentsJson },
                });
                HttpClient http = new HttpClient();
                HttpResponseMessage res = await http.PostAsync(AppConstants.POCKETAILOR_ADJUSTMENTS_WEBSERVICE_URL, postContent);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    // Remove adjustments from cache
                    this.Adjustments.RemoveRange(0, nvals);
                    attempt = AppConstants.MAX_DELIVERY_ATTEMPTS;
                }
                else
                {
                    attempt--;
                }
            } while (this.Adjustments.Count > 0);
        }

    }
}
