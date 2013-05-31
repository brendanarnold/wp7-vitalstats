using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Pocketailor.Model;

namespace Pocketailor.View
{
    public partial class ConversionAdjustmentPage : PhoneApplicationPage
    {
        public ConversionAdjustmentPage()
        {
            InitializeComponent();

            this.DataContext = App.VM;

        }

        public ConversionId ConversionId { get; set; }
        public BrandId BrandId { get; set; }
        public GenderId GenderId { get; set; }
        public RegionId RegionId { get; set; }
        public int ProfileId { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Take care of the tombstone situation
            if (!e.IsNavigationInitiator)
            {
                this.ParseUriQuery();
                if (App.VM.SelectedConversionData == null
                    || App.VM.SelectedConversionData.Brand != this.BrandId
                    || App.VM.SelectedConversionData.Conversion != this.ConversionId
                    || App.VM.SelectedConversionData.Region != this.RegionId
                    || App.VM.SelectedConversionData.Gender != this.GenderId)
                {
                    App.VM.LoadSelectedConversionData(this.GenderId, this.ConversionId, this.RegionId, this.BrandId);
                }
                if (App.VM.SelectedProfile == null
                    || App.VM.SelectedProfile.Id != this.ProfileId)
                {
                    App.VM.LoadSelectedProfile(this.ProfileId);
                }
                
            }
        }

        public void ParseUriQuery()
        {
            this.ProfileId = int.Parse(NavigationContext.QueryString["ProfileId"]);
            this.GenderId = (GenderId)Enum.Parse(typeof(GenderId), NavigationContext.QueryString["GenderId"], true);
            this.ConversionId = (ConversionId)Enum.Parse(typeof(ConversionId), NavigationContext.QueryString["ConversionId"], true);
            this.RegionId = (RegionId)Enum.Parse(typeof(RegionId), NavigationContext.QueryString["RegionId"], true);
            this.BrandId = (BrandId)Enum.Parse(typeof(BrandId), NavigationContext.QueryString["BrandId"], true);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (App.VM.SelectedConversionData.IsUnsavedTweaks)
            {
                MessageBoxResult res = MessageBox.Show("You have made an adjustment but have not saved it."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "To discard the changes hit 'OK'. To save the adjustment hit cancel and then the save icon at the bottom", "Discard adjustment?", MessageBoxButton.OKCancel);
                if (res != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
            }
            App.VM.SelectedConversionData.DiscardTweaks();
            App.VM.SkipLoadConversionPageData = true;
            base.OnBackKeyPress(e);
        }

        private void saveAppBarBtn_Click(object sender, System.EventArgs e)
        {
            if (App.VM.AllowFeedBack == null) this.PromptForFeedbackPermission();
            App.VM.SelectedConversionData.AcceptTweaks();
            App.VM.SkipLoadConversionPageData = true;

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }



        private void tooBigBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedConversionData.TweakSizeDown();
            this.sizeDownStoryboard.Begin();
        }

        private void tooSmallBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.VM.SelectedConversionData.TweakSizeUp();
            this.sizeDownStoryboard.Begin();
        }

        private void PromptForFeedbackPermission()
        {

            MessageBoxResult res = MessageBox.Show("You can send your adjustments to the Pocketailor team anonymously over the web so we can make the results better next time. If you would like to help then click 'ok' below, if you would rather not then click 'cancel'."
                    + Environment.NewLine + Environment.NewLine
                    + "You can change this anytime in the settings.",
                    "Allow feedback?",
                    MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                App.VM.AllowFeedBack = true;
            }
            else
            {
                App.VM.AllowFeedBack = false;
            }

        }
    }
}