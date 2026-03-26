using AM.ViewModel.ViewModels.Motion;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Motion
{
    /// <summary>
    /// MotionAxisView.xaml 的交互逻辑
    /// </summary>
    public partial class MotionAxisView : UserControl
    {
        private readonly MotionAxisViewModel _viewModel;

        public MotionAxisView()
        {
            InitializeComponent();

            _viewModel = new MotionAxisViewModel();
            DataContext = _viewModel;

            Loaded += MotionAxisView_Loaded;
            Unloaded += MotionAxisView_Unloaded;
        }

        private async void MotionAxisView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MotionAxisView_Loaded;
            await _viewModel.LoadAsync();
        }

        private void MotionAxisView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Dispose();
        }
    }
}