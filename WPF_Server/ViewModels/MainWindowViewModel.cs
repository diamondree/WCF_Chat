using Common.CommandsBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WCF_Shared_Library;

namespace WPF_Server.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ChatService _service = new ChatService();
        private ServiceHost _host;


        public MainWindowViewModel()
        {
            _service.messages.CollectionChanged += Messages_CollectionChanged;
            _service.users.CollectionChanged += OnUsers_CollectionChanged;
            Messages = new ObservableCollection<string>();
            Users = new List<string>();
        }


        private bool _IsServerStopped = true;
        public bool IsServerStopped
        {
            get { return _IsServerStopped; }
            set 
            { 
                _IsServerStopped = value;
                OnPropertyChanged();
            }
        }


        private int _Port = 3030;
        public int Port
        {
            get { return _Port; }
            set 
            { 
                _Port = value;
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
            get => _Messages;
            set 
            {
                _Messages = value;
                OnPropertyChanged();
            }
        }


        private List<string> _Users;
        public List<string> Users
        {
            get { return _Users; }
            set
            {
                _Users = value;
                OnPropertyChanged();
            }
        }


        public ICommand StartServer
        {
            get
            {
                return new AsyncDelegateCommand(RunServer, (obj) => IsServerStopped);
            }
        }

        public ICommand StopServer
        {
            get
            {
                return new AsyncDelegateCommand(ServerStop, (obj) => !_IsServerStopped);
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
                        _service.users.FirstOrDefault().Value.SendMessageToClient(Message);
                        Messages.Add(RepliesFormatService.MessageFormat("Server", Message));
                        OnPropertyChanged(nameof(Messages));
                    }
                    catch (CommunicationObjectAbortedException)
                    {
                        Messages.Add(RepliesFormatService.MessageFormat("System", "Client is unreachable"));
                        OnPropertyChanged(nameof(Messages));
                        _service.users.Remove(_service.users.FirstOrDefault());
                    }
                    finally
                    {
                        Message = "";
                    }
                }, (obj) => ((!IsServerStopped) && (Message.Length > 0)));
            }
        }

        public void OnServerClosing(object sender, CancelEventArgs e)
        {
            OnServerClosing();
        }

        private void OnServerClosing()
        {
            if (_service.users.Count() > 0)
                foreach (var user in _service.users)
                    user.Value.NotifyOnServerClosing();
        }

        private async Task RunServer()
        {
            await Task.Run(() =>
            {
                try
                {
                    Messages.Add(RepliesFormatService.MessageFormat("System", "Server trying to start, please wait..."));
                    OnPropertyChanged(nameof(Messages));
                    _host = new ServiceHost(_service);

                    string address = $"net.tcp://localhost:{Port}/IChatService";
                    NetTcpBinding binding = new NetTcpBinding();
                    Type contract = typeof(IChatService);

                    _host.AddServiceEndpoint(contract, binding, address);
                    _host.Open();

                    Messages.Add(RepliesFormatService.MessageFormat("System", "Succesfully started"));
                    IsServerStopped = false;
                    OnPropertyChanged(nameof(IsServerStopped));
                    OnPropertyChanged(nameof(Messages));
                }
                catch (AddressAlreadyInUseException)
                {
                    Messages.Add(RepliesFormatService.MessageFormat("System", "Try another port"));
                    OnPropertyChanged(nameof(Messages));
                }
                catch (CommunicationException)
                {
                    Messages.Add(RepliesFormatService.MessageFormat("System", "Try another port"));
                    OnPropertyChanged(nameof(Messages));
                }
            });
        }

        private async Task ServerStop()
        {
            await Task.Run(() =>
            {
                Messages.Add(RepliesFormatService.MessageFormat("System", "Server stopping, please wait..."));
                OnPropertyChanged(nameof(Messages));
                OnServerClosing();
                Users.Clear();
                OnPropertyChanged(nameof(Users));
                _service.users = new ObservableCollection<KeyValuePair<string, IChatServiceCallback>>();
                _host.Close();

                IsServerStopped = true;
                Messages.Add(RepliesFormatService.MessageFormat("System", "Server stopped"));
                OnPropertyChanged(nameof(Messages));
            });
        }


        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Messages.Add(_service.messages.Last());
            OnPropertyChanged(nameof(Messages));
        }

        private void OnUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Users.Clear();
            _service.users.ToList().ForEach(x => Users.Add(x.Key));
            OnPropertyChanged(nameof(Users));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
