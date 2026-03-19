using AM.ViewModel.ViewModels.Am;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Am
{
    /// <summary>
    /// LoginLogView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginLogView : UserControl
    {
        private readonly LoginLogViewModel _viewModel;

        public LoginLogView()
        {
            InitializeComponent();

            _viewModel = new LoginLogViewModel();
            DataContext = _viewModel;

            Loaded += LoginLogView_Loaded;
        }

        private async void LoginLogView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= LoginLogView_Loaded;
            await _viewModel.LoadAsync();
        }
    }
}