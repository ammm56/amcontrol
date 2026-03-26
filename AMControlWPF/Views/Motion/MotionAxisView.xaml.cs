using AM.ViewModel.ViewModels.Motion;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AMControlWPF.Views.Motion
{
    /// <summary>
    /// MotionAxisView.xaml 的交互逻辑
    /// </summary>
    public partial class MotionAxisView : UserControl
    {
        private readonly MotionAxisViewModel _viewModel;
        private readonly DispatcherTimer _refreshTimer;

        public MotionAxisView()
        {
            InitializeComponent();

            _viewModel = new MotionAxisViewModel();
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            _refreshTimer.Tick += RefreshTimer_Tick;
            DataContext = _viewModel;

            Loaded += MotionAxisView_Loaded;
            Unloaded += MotionAxisView_Unloaded;
        }

        private async void MotionAxisView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
            _refreshTimer.Start();
        }

        private void MotionAxisView_Unloaded(object sender, RoutedEventArgs e)
        {
            _refreshTimer.Stop();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            await _viewModel.RefreshAsync();
        }
    }
}
