using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    
    public partial class AppViewModel : INotifyPropertyChanged
    {


        private ConversionData _selectedConversionData;
        public ConversionData SelectedConversionData 
        { 
            get { return this._selectedConversionData; } 
            set 
            { 
                if (this._selectedConversionData != value) 
                { 
                    this._selectedConversionData = value; 
                    this.NotifyPropertyChanged("SelectedConversionData"); 
                } 
            } 
        }

        public void LoadSelectedConversionData(GenderId gender, ConversionId conversion, RegionId region, BrandId brand)
        {
            this.SelectedConversionData = this.conversiondsDB.ConversionData.Where(c => c.Gender == gender
                && c.Conversion == conversion
                && c.Region == region
                && c.Brand == brand).FirstOrDefault();
        }

        public void LoadSelectedProfile(int profileId)
        {
            this.SelectedProfile = this.appDB.Profiles.Where(p => p.Id == profileId).FirstOrDefault();
        }
    
    
    }
}
