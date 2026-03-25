using AM.ViewModel.ViewModels.Alarm;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Alarm
{
    /// <summary>
    /// AlarmHistoryView.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmHistoryView : UserControl
    {
        private readonly AlarmHistoryViewModel _viewModel;

        public AlarmHistoryView()
        {
            InitializeComponent();

            _viewModel = new AlarmHistoryViewModel();
            DataContext = _viewModel;

            Loaded += AlarmHistoryView_Loaded;
        }

        private async void AlarmHistoryView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AlarmHistoryView_Loaded;
            await _viewModel.LoadAsync();
        }
    }
}