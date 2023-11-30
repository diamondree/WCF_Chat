using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows.Input;
using WCF_Shared_Library;
using WPF_Client.Commands;
using WPF_Client.Enums;

namespace WPF_Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DuplexChannelFactory<IChatService> _channelFactory;
        private IChatService _chatService;
        private ChatServiceCallback _chatServiceCallback;

        public MainWindowViewModel()
        {
            Messages = new ObservableCollection<string>();
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


        private ObservableCollection<string> _Messages;
        public ObservableCollection<string> Messages
        {
            get { return _Messages; }
            set
            {
                _Messages = value;
                OnPropertyChanged();
            }
        }


        private ServerStatusEnum _ServerStatus = ServerStatusEnum.Disconnected;
        public ServerStatusEnum ServerStatus
        {
            get
            {
                return _ServerStatus;
            }
            set
            {
                _ServerStatus = value;
                OnPropertyChanged();
            }
        }


        public ICommand Connect
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    try
                    {
                        string address = $"net.tcp://{Ip}:{Port}/IChatService";

                        NetTcpBinding binding = new NetTcpBinding();
                        _chatServiceCallback = new ChatServiceCallback();
                        InstanceContext context = new InstanceContext(_chatServiceCallback);
                        _channelFactory = new DuplexChannelFactory<IChatService>(context, binding, address);
                        _chatService = _channelFactory.CreateChannel();
                        _chatService.Login(Username);
                        _chatServiceCallback.ServerMessages.CollectionChanged += ServerMessages_CollectionChanged;
                        IsDisconnected = false;
                        ServerStatus = ServerStatusEnum.Connected;
                    }
                    catch(EndpointNotFoundException ex)
                    {
                        Messages.Add("Server is offline");
                        OnPropertyChanged(nameof(Messages));
                    }
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
                    ServerStatus = ServerStatusEnum.Disconnected;
                }, (obj) => !IsDisconnected);
            }
        }

        public ICommand SendMsg
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    try
                    {
                        _chatService.SendMessageToServer(Message);
                        Message = null;
                    }
                    catch (CommunicationObjectFaultedException)
                    {
                        Messages.Add($"{DateTime.Now.ToShortTimeString()}: Service error, communication channel fauted, try reconnect");
                        Message = null;
                        IsDisconnected = true;
                        ServerStatus = ServerStatusEnum.Fauted;
                        OnPropertyChanged(nameof(Messages));
                    }
                }, (obj) => !IsDisconnected);
            }
        }


        private void ServerMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Messages.Add(_chatServiceCallback.ServerMessages.Last());
            OnPropertyChanged(nameof(Messages));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
