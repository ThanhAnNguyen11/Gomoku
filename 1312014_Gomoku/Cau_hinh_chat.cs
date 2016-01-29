using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;

namespace _1312014_Gomoku
{
    class Cau_hinh_chat : INotifyPropertyChanged
    {
        string _Ten;
        string _Noi_dung_chat;
        string _Thoi_gian;
        public string Ten
        {
            get
            {
                return _Ten;
            }
            set
            {
                _Ten = value;
                OnPropertyChanged("Ten");
            }

        }
        public string Noi_dung_chat
        {
            get
            {
                return _Noi_dung_chat;
            }
            set
            {
                _Noi_dung_chat = value;
                OnPropertyChanged("Noi_dung_chat");
            }
        }
        public string Thoi_gian
        {
            get
            {
                return _Thoi_gian;
            }
            set
            {
                _Thoi_gian = value;
                OnPropertyChanged("Thoi_gian");
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
