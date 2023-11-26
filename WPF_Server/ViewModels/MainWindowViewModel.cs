using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public bool IsServerRunned;
        private readonly ChatService _service = new ChatService();
        private ServiceHost _host;

        public ObservableCollection<string> Messages { get; set; }

        public MainWindowViewModel()
        {
            IsServerRunned = false;
            Messages = new ObservableCollection<string>();            
        }


        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _Port = 3030;

        public int Port
        {
            get { return _Port; }
            set { 
                _Port = value;
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
                            IsServerRunned = true;
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
                    }, (obj) => !IsServerRunned);
            }
        }

        public ICommand StopServer
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    _host.Close();
                    
                    IsServerRunned = false;
                    Messages.Add("Server stopped");
                    OnPropertyChanged("Messages");
                }, (obj) => IsServerRunned);
            }
        }
    }
}
