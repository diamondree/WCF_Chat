using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows.Input;
using WCF_Shared_Library;
using WPF_Server.Commands;

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
            Messages = new ObservableCollection<string>();            
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Messages.Add(_service.messages.Last());
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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


        public ICommand StartServer
        {
            get
            {
                    return new DelegateCommand((obj) =>
                    {
                        try
                        {
                            _host = new ServiceHost(_service);

                            string address = $"net.tcp://localhost:{Port}/IChatService";
                            NetTcpBinding binding = new NetTcpBinding();
                            Type contract = typeof(IChatService);

                            _host.AddServiceEndpoint(contract, binding, address);
                            _host.Open();

                            Messages.Add("Succesfully started");
                            IsServerStopped = false;
                            OnPropertyChanged("Messages");
                        }
                        catch (AddressAlreadyInUseException ex) 
                        {
                            Messages.Add("Try another port");
                            OnPropertyChanged("Messages");
                        }
                        catch(CommunicationException ex)
                        {
                            Messages.Add("Try another port");
                            OnPropertyChanged("Messages");
                        }
                        
                    }, (obj) => _IsServerStopped);
            }
        }

        public ICommand StopServer
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    _host.Close();
                    
                    IsServerStopped = true;
                    Messages.Add("Server stopped");
                    OnPropertyChanged("Messages");
                }, (obj) => !_IsServerStopped);
            }
        }
    }
}
