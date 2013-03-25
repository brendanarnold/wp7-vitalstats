using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Pocketailor.View.ConversionPages
{
    public partial class DressConversionPage : PhoneApplicationPage
    {
        public DressConversionPage()
        {
            InitializeComponent();

            App.VM.LoadConversionRegions(Model.ConversionId.DressSize);
            this.DataContext = App.VM;



        }





    }
}