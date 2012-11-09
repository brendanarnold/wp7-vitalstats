using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace VitalStats.View.Controls
{
    public partial class TitledTextBox : UserControl
    {

        [Description("A TextBox with built in title")]
        public TitledTextBox()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this)  )
            {
                this.Title = "Title Here";  
            }
        }


        public string Title
        {
            get { return this.titleTextBlock.Text; }
            set { this.titleTextBlock.Text = value; }
        }

        public string Text
        {
            get { return this.inputTextBox.Text; }
            set { this.inputTextBox.Text = value; }
        }




    }
}
