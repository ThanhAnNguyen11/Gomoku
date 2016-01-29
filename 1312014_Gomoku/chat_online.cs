using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json.Linq;

using Quobject.SocketIoClientDotNet.Client;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace _1312014_Gomoku
{
    class chat_online : INotifyPropertyChanged
    {

        static public event EventHandler SomethingHappened;
        static public event EventHandler Name;

        string time;
        public Socket socket;

        #region
        private ObservableCollection<Cau_hinh_chat> _list_chat;

        public ObservableCollection<Cau_hinh_chat> list_chat
        {
            get
            {
                if (_list_chat == null)
                    _list_chat = new ObservableCollection<Cau_hinh_chat>();


                return _list_chat;
            }
            set { _list_chat = value; OnPropertyChanged("list_chat"); }
        }

        public ICommand Send
        {
            get;
            internal set;
        }

        public ICommand Start
        {
            get;
            internal set;
        }

        string _Text_noi_dung_chat;
        string _Text_button_Start;


        public string Text_button_Start
        {
            get
            {
                return _Text_button_Start;
            }
            set
            {
                _Text_button_Start = value;
                OnPropertyChanged("Text_button_Start");
            }
        }

        string _Text_textbox_name;
        public static string Name_player2;

        public string Text_textbox_name
        {
            get
            {
                return _Text_textbox_name;
            }
            set
            {
                _Text_textbox_name = value;
                OnPropertyChanged("Text_textbox_name");
            }
        }

        public string Text_noi_dung_chat
        {
            get
            {
                return _Text_noi_dung_chat;
            }
            set
            {
                OnPropertyChanged("Text_noi_dung_chat");
                _Text_noi_dung_chat = value;
            }
        }
        #endregion
        BackgroundWorker bgworker = new BackgroundWorker();

        public chat_online()
        {
            socket = null;
            Send = new CommandHandler(Create_Send, true);

            Text_button_Start = "Start!";
            Text_textbox_name = "Guest";
            Start = new CommandHandler(Create_Start, true);

            bgworker.WorkerReportsProgress = true;
            bgworker.DoWork += bgworker_DoWork;
            bgworker.ProgressChanged += bgworker_ProgressChanged;
            //bgworker.RunWorkerCompleted += bgworker_RunWorkerCompleted;
        }
        
        //void bgworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{

        //}

        void bgworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<Cau_hinh_chat> list = (List<Cau_hinh_chat>)e.UserState;
            foreach (Cau_hinh_chat item in list)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    list_chat.Add(item);
                }));
            }
        }

        void bgworker_DoWork(object sender, DoWorkEventArgs e)
        {
            Chat value = (Chat)e.Argument;
            List<Cau_hinh_chat> list = new List<Cau_hinh_chat>();
            for (int i = 0; i < 1; i++)
            {
                time = DateTime.Now.ToString("HH:mm:ss");
                list.Add(new Cau_hinh_chat { Ten = value.name, Noi_dung_chat = value.context, Thoi_gian = time });
                Thread.Sleep(1000);
                bgworker.ReportProgress(i, list);
            }
        }

        bool flag_start = false;

        public void Create_Start()
        {
            if (Text_button_Start == "Start!" || Text_button_Start == "New Game!")
            {
                socket = IO.Socket("ws://gomoku-lajosveres.rhcloud.com:8000");

                socket.On("ChatMessage", (data) =>
                {
                    string str = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();

                    if (!bgworker.IsBusy)
                    {
                        string name = "Server";
                        Chat chat;

                        if (((Newtonsoft.Json.Linq.JObject)data).ToString().Contains('"' + "from" + '"'))
                        {
                            name = ((Newtonsoft.Json.Linq.JObject)data)["from"].ToString();
                        }

                        if (str.Contains("<br />"))
                        {
                            str = str.Replace("<br />", "\n");
                        }

                        chat = new Chat { name = name, context = str };
                        bgworker.RunWorkerAsync(chat);

                    }
                    if (((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() == "Welcome!")
                    {


                        socket.Emit("MyNameIs", Text_textbox_name);

                        socket.Emit("ConnectToOtherPlayer");
                    }

                    if (str.Contains("second"))
                    {
                        string[] tokens = str.Split(' ');
                        Name_player2 = tokens[0];


                        board.turn = 1;
                        if (SomethingHappened != null)
                            SomethingHappened(this, null);

                    }
                    if (str.Contains("first"))
                    {
                        board.turn = 0;
                        if (SomethingHappened != null)
                            SomethingHappened(this, null);
                    }
                    if (str.Contains("now called"))
                    {
                        string[] tokens = str.Split(' ');
                        if (flag_start)
                        {
                        }
                        else
                        {
                            Name_player2 = tokens[4];
                        }
                        flag_start = false;
                    }

                });

                socket.On("EndGame", (data) =>
                {
                    Text_button_Start = "New Game!";
                    string str = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                    if (!bgworker.IsBusy)
                    {
                        string name = "Server";
                        Chat chat;

                        chat = new Chat { name = name, context = str };
                        bgworker.RunWorkerAsync(chat);
                    }

                });


                Text_button_Start = "Change!";
            }
            else
            {
                socket.Emit("MyNameIs", Text_textbox_name);
                flag_start = true;
            }
            board.socket = socket;
        }
        public void Create_Send()
        {
            if (socket == null)
            {
                if (Text_noi_dung_chat != "")
                {
                    time = DateTime.Now.ToString("HH:mm:ss");
                    list_chat.Add(new Cau_hinh_chat { Ten = Text_textbox_name, Noi_dung_chat = Text_noi_dung_chat, Thoi_gian = time });
                }
            }
            if (socket != null && Text_noi_dung_chat != "")
            {
                socket.Emit("ChatMessage", Text_noi_dung_chat);
                Text_noi_dung_chat = "";
            }
            Text_noi_dung_chat = string.Empty;
               
        }






        #region

        public class Command<T> : ICommand
        {
            public Action<T> Action { get; set; }

            public void Execute(object parameter)
            {
                if (Action != null && parameter is T)
                    Action((T)parameter);
            }

            public bool CanExecute(object parameter)
            {
                return IsEnabled;
            }

            private bool _isEnabled = true;
            public bool IsEnabled
            {
                get { return _isEnabled; }
                set
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                        CanExecuteChanged(this, EventArgs.Empty);
                }
            }

            public event EventHandler CanExecuteChanged;

            public Command(Action<T> action)
            {
                Action = action;
            }
        }


        public class CommandHandler : ICommand
        {
            private Action _action;
            private bool _canExecute;
            public CommandHandler(Action action, bool canExecute)
            {
                _action = action;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _action();
            }
        }



        #endregion
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
