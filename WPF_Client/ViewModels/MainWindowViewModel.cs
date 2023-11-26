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
                    _chatService.Login("User");
                    _chatService.SendMessageToServer("hello");
                    IsDisconnected = false;
                },(obj)=>IsDisconnected);
            }
        }
    }
}
