using AM.ViewModel.ViewModels.Am;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Am
{
    /// <summary>
    /// UserManagementView.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private readonly UserManagementViewModel _viewModel;

        public UserManagementView()
        {
            InitializeComponent();

            _viewModel = new UserManagementViewModel();
            DataContext = _viewModel;

            Loaded += UserManagementView_Loaded;
        }

        private async void UserManagementView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserManagementView_Loaded;
            await _viewModel.LoadAsync();
        }
    }
}