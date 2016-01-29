using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace _1312014_Gomoku
{
    class Chess : INotifyPropertyChanged
    {
        int _Row;
        int _Column;
        Brush _Background;
        int _IsTurn;
        ImageSource _myImageSource;
        bool _OptionBlink;
        public bool OptionBlink
        {

            get { return _OptionBlink; }
            set
            {
                _OptionBlink = value;
                OnPropertyChanged("OptionBlink");
            }
        }

        public ImageSource MyImageSource
        {
            get { return _myImageSource; }
            set
            {
                _myImageSource = value;
                OnPropertyChanged("MyImageSource");
            }
        }     
        public int IsTurn
        {
            get
            {
                return _IsTurn;
            }
            set
            {
                _IsTurn = value;
                OnPropertyChanged("Row");
            }
        }

        public int Row
        {
            get {
                return _Row; 
            }
            set { 
                _Row = value; OnPropertyChanged("Row"); 
            }
        }

        public int Column
        {
            get { 
                return _Column;
            }
            set {
                _Column = value; OnPropertyChanged("Column");
            }
        }

        public Brush Background
        {
            get {
                return _Background;
            }
            set {
                _Background = value; OnPropertyChanged("Background");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, args);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
