using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows.Input;
using WCF_Shared_Library;
using WPF_Client.Commands;

namespace WPF_Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DuplexChannelFactory<IChatService> _channelFactory;
        private IChatService _chatService;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private string _Username = "Username";
        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged();
            }
        }


        private int _Port = 3030;
        public int Port
        {
            get { return _Port;}
            set
            {
                _Port = value;
                OnPropertyChanged();
            }
        }


        private string _Ip = "localhost";
        public string Ip
        {
            get { return _Ip; }
            set
            {
                _Ip = value;
                OnPropertyChanged();
            }
        }


        private bool _IsDisconnected = true;
        public bool IsDisconnected
        {
            get => _IsDisconnected;
            set
            {
                _IsDisconnected = value;
                OnPropertyChanged();
            }
        }


        private string _Message = "Type your message here";
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                OnPropertyChanged();
            }
        }


        public ICommand Connect
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    string address = $"net.tcp://{Ip}:{Port}/IChatService";

                    NetTcpBinding binding = new NetTcpBinding();
                    InstanceContext context = new InstanceContext(new ChatServiceCallback());
                    _channelFactory = new DuplexChannelFactory<IChatService>(context, binding, address);
                    _chatService = _channelFactory.CreateChannel();
                    _chatService.Login(Username);
                    //_chatService.SendMessageToServer(Username);
                    IsDisconnected = false;
                }, (obj) => IsDisconnected);
            }
        }

        public ICommand Disconnect
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    _chatService.Logout(Username);
                    IsDisconnected = true;
                }, (obj) => !IsDisconnected);
            }
        }

        public ICommand SendMsg
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    _chatService.SendMessageToServer(Message);
                }, (obj) => !IsDisconnected);
            }
        }
    }
}
