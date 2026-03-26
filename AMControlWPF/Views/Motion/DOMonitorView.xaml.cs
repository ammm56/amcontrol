using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AM.ViewModel.ViewModels.Motion;

namespace AMControlWPF.Views.Motion
{
    /// <summary>
    /// DOMonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class DOMonitorView : UserControl
    {
        private readonly DOMonitorViewModel _viewModel;
        private readonly DispatcherTimer _refreshTimer;

        public DOMonitorView()
        {
            InitializeComponent();

            _viewModel = new DOMonitorViewModel();
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            _refreshTimer.Tick += RefreshTimer_Tick;
            DataContext = _viewModel;

            Loaded += DOMonitorView_Loaded;
            Unloaded += DOMonitorView_Unloaded;
        }

        private async void DOMonitorView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
            _refreshTimer.Start();
        }

        private void DOMonitorView_Unloaded(object sender, RoutedEventArgs e)
        {
            _refreshTimer.Stop();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            await _viewModel.RefreshAsync();
        }
    }
}
