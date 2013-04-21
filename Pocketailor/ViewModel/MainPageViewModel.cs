using Microsoft.Phone.Tasks;
using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public partial class AppViewModel
    {

        #region QuickProfile methods

        private ObservableCollection<Profile> _quickProfiles;
        public ObservableCollection<Profile> QuickProfiles
        {
            get { return this._quickProfiles; }
            set
            {
                this._quickProfiles = value;
                this.NotifyPropertyChanged("QuickProfiles");
            }
        }

        public void ToggleQuickProfile(Profile p)
        {
            p.IsQuickProfile = !p.IsQuickProfile;
            this.LoadQuickProfilesFromDB();
        }

        public void LoadQuickProfilesFromDB()
        {
            // Load from memory if possible, quicker I imagine ...
            IEnumerable<Profile> profiles;
            if (this.Profiles.Any())
            {
                profiles = from Profile p in this.Profiles where p.IsQuickProfile == true select p;
            }
            else
            {
                profiles = from Profile p in this.appDB.Profiles where p.IsQuickProfile == true select p;
            }
            this.QuickProfiles = new ObservableCollection<Profile>(profiles);
        }

        #endregion

        #region MainPage actions

        private string _appVersion = AppConstants.APP_VERSION;
        public string AppVersion
        {
            get { return this._appVersion; }
            set { if (this._appVersion != value) this._appVersion = value; }
        }

        public void EmailAuthor()
        {
            EmailComposeTask emailTask = new EmailComposeTask()
            {
                Subject = "Feedback on Pocketailor",
                To = AppConstants.AUTHOR_EMAIL,
            };
            emailTask.Show();
        }

        public void ViewWebsite()
        {
            WebBrowserTask webTask = new WebBrowserTask();
            webTask.Uri = new Uri(AppConstants.WEBSITE_URL, UriKind.Absolute);
            webTask.Show();
        }

        public void ViewLicences()
        {
            WebBrowserTask webTask = new WebBrowserTask();
            webTask.Uri = new Uri(AppConstants.LEGAL_URL, UriKind.Absolute);
            webTask.Show();
        }

        public void RateApp()
        {
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

        public void GotoFacebook()
        {
            WebBrowserTask wbTask = new WebBrowserTask();
            wbTask.Uri = new Uri(AppConstants.FACEBOOK_LIKE_URL, UriKind.Absolute);
            wbTask.Show();
        }

        public void GiveFeedback()
        {
            WebBrowserTask wbTask = new WebBrowserTask();
            wbTask.Uri = new Uri(AppConstants.FEEDBACK_URL, UriKind.Absolute);
            wbTask.Show();
        }


        //public void BuyApp()
        //{
        //    Microsoft.Phone.Tasks.MarketplaceDetailTask mktTask = new Microsoft.Phone.Tasks.MarketplaceDetailTask()
        //    {
        //        ContentIdentifier = AppConstants.PAID_APP_GUID,
        //    };
        //}

        #endregion

    }
}
