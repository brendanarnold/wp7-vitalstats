using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Pocketailor.Model.Adjustments
{
    // Cache the feedback for a few days before sending at an opportune time
    public class FeedbackAgent
    {
        public FeedbackAgent()
        {
            App.Cache.LoadQueuedFeedback();
        }


        // An Async method that sends off the adjustments in batches
        public async System.Threading.Tasks.Task DeliverAdjustmentsTaskAsync()
        {
            if (App.Cache.QueuedFeedback.Count == 0) return;
            // Take MAX_ADJUSTMENTS_PER_REQUEST adjustments at a time from the cache and send on
            int attempt = AppConstants.MAX_DELIVERY_ATTEMPTS;
            do
            {
                // Give up after a certain amount of attempts
                if (attempt == 0) break;
                // Take a batch of adjustments 
                int nvals = Math.Min(App.Cache.QueuedFeedback.Count, AppConstants.MAX_ADJUSTMENTS_PER_REQUEST);
                List<Adjustment> buff = App.Cache.QueuedFeedback.GetRange(0, nvals);
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
                    App.Cache.QueuedFeedback.RemoveRange(0, nvals);
                    attempt = AppConstants.MAX_DELIVERY_ATTEMPTS;
                }
                else
                {
                    attempt--;
                }
            } while (App.Cache.QueuedFeedback.Count > 0);
            App.Cache.SaveQueuedFeedback();
        }


        internal void QueueFeedback(Adjustment adj)
        {
            App.Cache.QueuedFeedback.Add(adj);
            App.Cache.SaveQueuedFeedback();
        }
    }
}
