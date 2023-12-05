using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPF_Server.ViewModels;

namespace WPF_Server
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
            Closing += _viewModel.OnWindowClosing;
        }

        private void port_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
        }
    }
}
