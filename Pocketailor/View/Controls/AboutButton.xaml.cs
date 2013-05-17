using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace Pocketailor.View.Controls
{
    public partial class AboutButton : UserControl
    {
        public AboutButton()
        {
            InitializeComponent();

            if (!TiltEffect.TiltableItems.Contains(typeof(AboutButton)))
                TiltEffect.TiltableItems.Add(typeof(AboutButton));


        }


        public string IconImage
        {
            get
            {
                // TODO: This might not give the string as expected
                return this.iconImage.Source.ToString();
            }
            set
            {
                this.iconImage.Source = new BitmapImage(new Uri(value, UriKind.Relative));
            }
        }


        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register("TitleText", typeof(string), typeof(AboutButton),
            new PropertyMetadata(String.Empty, new PropertyChangedCallback(TitleTextPropertyChanged)));

        public static void TitleTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AboutButton aBtn = (AboutButton)d;
            aBtn.titleTextBlock.Text = (string)e.NewValue;
        }

        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }







    }
}
