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
    public partial class HelpTextBlock : UserControl
    {
        public HelpTextBlock()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty HelpTextProperty =
           DependencyProperty.Register("HelpText", typeof(string), typeof(HelpTextBlock),
           new PropertyMetadata(String.Empty, new PropertyChangedCallback(HelpTextPropertyChanged)));

        public static void HelpTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HelpTextBlock aBtn = (HelpTextBlock)d;
            aBtn.helpTextBlock.Text = (string)e.NewValue;
        }

        public string HelpText
        {
            get { return (string)GetValue(HelpTextProperty); }
            set { SetValue(HelpTextProperty, value); }
        }

         


    }
}
