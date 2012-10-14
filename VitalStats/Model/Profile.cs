using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace VitalStats.Model
{
    public class Profile : INotifyPropertyChanged
    {
        private string _name;
        public string Name 
        { 
            get 
            {
                return this._name;
            }
            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    this.RaisePopertyChanged("Name");
                }
            }
        }

        private bool _isProtected;
        public bool IsProtected
        {
            get 
            {
                return this._isProtected;
            }
            set 
            {
                if (this._isProtected != value)
                {
                    this._isProtected = value;
                    this.RaisePopertyChanged("IsLocked");
                }
            }
        }

        public string Id { get; set; }

        public Profile()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePopertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        internal object GetCopy()
        {
            return (Profile)this.MemberwiseClone();
        }
    }
}
