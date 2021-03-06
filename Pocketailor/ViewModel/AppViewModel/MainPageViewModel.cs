﻿using Microsoft.Phone.Tasks;
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


        public void LoadMainPageData()
        {
            if (this.Profiles == null)
            {
                this.LoadProfilesFromDB();
            }
            if (this.QuickProfiles == null)
            {
                this.LoadQuickProfilesFromDB();
            }
        }



        #region QuickProfile methods

        private ObservableCollection<Profile> _quickProfiles;
        public ObservableCollection<Profile> QuickProfiles
        {
            get 
            {
                if (this._quickProfiles == null)
                {
                    this.LoadQuickProfilesFromDB();
                }
                return this._quickProfiles; 
            }
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
            this.SaveChangesToAppDB();
            
        }

        public void LoadQuickProfilesFromDB()
        {
            // Load from memory if possible, quicker I imagine ...
            IEnumerable<Profile> profiles;
            if (this.Profiles != null)
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


        #region Profiles collection methods/properties

        private ObservableCollection<Profile> _profiles;
        public ObservableCollection<Profile> Profiles
        {
            get 
            {
                if (this._profiles == null)
                {
                    this.LoadProfilesFromDB();
                }
                return this._profiles; 
            }
            set
            {
                this._profiles = value;
                this.NotifyPropertyChanged("Profiles");
            }
        }

        public void LoadProfilesFromDB()
        {
            var profiles = from Profile p in this.appDB.Profiles select p;
            this.Profiles = new ObservableCollection<Profile>(profiles);
        }

        public void AddProfile(Profile profile)
        {
            this.appDB.Profiles.InsertOnSubmit(profile);
            this.appDB.SubmitChanges();
            this.Profiles.Add(profile);
            this.NotifyPropertyChanged("Profiles");
            if (profile.IsQuickProfile)
            {
                this.QuickProfiles.Add(profile);
                this.NotifyPropertyChanged("QuickProfiles");
            }
        }

        public void UpdateProfile(Profile profile)
        {
            if (profile.IsQuickProfile && !this.QuickProfiles.Contains(profile)) this.QuickProfiles.Add(profile);
            if (!profile.IsQuickProfile && this.QuickProfiles.Contains(profile)) this.QuickProfiles.Remove(profile);
            App.VM.SaveChangesToAppDB();
        }

        public void DeleteProfile(Profile profile)
        {
            this.Profiles.Remove(profile);
            if (this.QuickProfiles.Contains(profile))
                this.QuickProfiles.Remove(profile);
            this.appDB.Profiles.DeleteOnSubmit(profile);
            this.appDB.SubmitChanges();
            this.NotifyPropertyChanged("Profiles");
            this.NotifyPropertyChanged("QuickProfiles");
        }

        

        #endregion


        #region MainPage actions

        private string _appVersion = AppConstants.APP_VERSION;
        public string AppVersion
        {
            get { return this._appVersion; }
            set { if (this._appVersion != value) this._appVersion = value; }
        }

        public void EmailAuthor(string subject)
        {
            EmailComposeTask emailTask = new EmailComposeTask()
            {
                Subject = subject,
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

        public void UpgradeApp()
        {
            MarketplaceDetailTask mdTask = new MarketplaceDetailTask();
            mdTask.Show();
        }

        public void GiveFeedback()
        {
            WebBrowserTask wbTask = new WebBrowserTask();
            wbTask.Uri = new Uri(AppConstants.FEEDBACK_URL, UriKind.Absolute);
            wbTask.Show();
        }


        internal void ViewTwitter(string s)
        {
            ShareStatusTask sTask = new ShareStatusTask();
            sTask.Status = s;
            sTask.Show();
            //WebBrowserTask wbTask = new WebBrowserTask();
            //wbTask.Uri = new Uri(AppConstants.TWITTER_URL, UriKind.Absolute);
            //wbTask.Show();
        }

        internal void ViewFacebook()
        {
            WebBrowserTask wbTask = new WebBrowserTask();
            wbTask.Uri = new Uri(AppConstants.FACEBOOK_URL, UriKind.Absolute);
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
