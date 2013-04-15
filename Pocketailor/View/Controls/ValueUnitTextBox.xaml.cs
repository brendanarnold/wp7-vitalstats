using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Pocketailor.View.Controls
{
    public partial class ValueUnitTextBox : UserControl
    {
        public ValueUnitTextBox()
        {
            InitializeComponent();
        }

        public string UnitText
        {
            get { return this.txtBlock.Text; }
            set { this.txtBlock.Text = value; }
        }

        public string ValueText
        {
            get { return this.txtBox.Text; }
            set { this.txtBox.Text = value; }
        }

        public void Clear()
        {
            this.UnitText = "unit";
            this.ValueText = String.Empty;
        }

    }
}
