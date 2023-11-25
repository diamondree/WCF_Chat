using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Server.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}
