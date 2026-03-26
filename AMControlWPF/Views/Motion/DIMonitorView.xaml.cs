using AM.ViewModel.ViewModels.Motion;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AMControlWPF.Views.Motion
{
    /// <summary>
    /// DIMonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class DIMonitorView : UserControl
    {
        private readonly DIMonitorViewModel _viewModel;
        private readonly DispatcherTimer _refreshTimer;

        public DIMonitorView()
        {
            InitializeComponent();

            _viewModel = new DIMonitorViewModel();
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            _refreshTimer.Tick += RefreshTimer_Tick;
            DataContext = _viewModel;

            Loaded += DIMonitorView_Loaded;
            Unloaded += DIMonitorView_Unloaded;
        }

        private async void DIMonitorView_Loaded(object sender, RoutedEventArgs e)
        {
            _refreshTimer.Stop();
            await _viewModel.LoadAsync();
            _refreshTimer.Start();
        }

        private void DIMonitorView_Unloaded(object sender, RoutedEventArgs e)
        {
            _refreshTimer.Stop();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            await _viewModel.RefreshAsync();
        }
    }
}
