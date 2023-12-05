using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPF_Client.ViewModels;

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = DataContext as MainWindowViewModel;
            Closing += _viewModel.OnWidnowClosing;
        }

        private void ip_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = (!Regex.IsMatch(e.Text, "\\.")) && (!Regex.IsMatch(e.Text, "[0-9]"));
        }

        private void port_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
        }
    }
}
