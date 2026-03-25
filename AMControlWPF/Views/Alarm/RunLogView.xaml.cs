using AM.ViewModel.ViewModels.Alarm;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Alarm
{
    /// <summary>
    /// RunLogView.xaml 的交互逻辑
    /// </summary>
    public partial class RunLogView : UserControl
    {
        private readonly RunLogViewModel _viewModel;

        public RunLogView()
        {
            InitializeComponent();

            _viewModel = new RunLogViewModel();
            DataContext = _viewModel;

            Loaded += RunLogView_Loaded;
        }

        private async void RunLogView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= RunLogView_Loaded;
            await _viewModel.InitAsync();
        }
    }
}