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

        
    
    
    
    }
}
