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

        public ObservableCollection<string> Messages { get; set; }

        public MainWindowViewModel()
        {
            Messages = new ObservableCollection<string>();
        }
        

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _Port;

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
                    Uri address = new Uri("net.tcp://localhost:3030/IChatService");

                    NetTcpBinding binding = new NetTcpBinding();
                    Type contract = typeof(IChatService);
                    ChatService service = new ChatService();
                    ServiceHost host = new ServiceHost(service);

                    host.AddServiceEndpoint(contract, binding, address);


                    host.Open();
                    
                    Messages.Add("Succesfully started");
                    OnPropertyChanged("Messages");
                });
            }
        }
    }
}
