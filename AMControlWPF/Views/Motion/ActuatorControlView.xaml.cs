using AM.ViewModel.ViewModels.Motion;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Motion
{
    /// <summary>
    /// ActuatorControlView.xaml 的交互逻辑。
    ///
    /// 重要说明：
    /// 当前主窗口对二级页面采用缓存复用模式（见 MainWindow 的 _pageCache）。
    /// 因此该页面实例在导航切换后不会被销毁，而是会被再次显示。
    ///
    /// 在这种模式下：
    /// 1. 不要在 Unloaded 中释放 ViewModel，否则会断开 MotionIo.SnapshotChanged 订阅。
    /// 2. 首次加载执行 LoadAsync 从 MachineContext 读取执行器列表，后续依赖事件驱动刷新状态。
    /// </summary>
    public partial class ActuatorControlView : UserControl
    {
        private readonly ActuatorControlViewModel _viewModel;
        private bool _isFirstLoaded;

        public ActuatorControlView()
        {
            InitializeComponent();

            _viewModel = new ActuatorControlViewModel();
            DataContext = _viewModel;

            Loaded += ActuatorControlView_Loaded;
        }

        private async void ActuatorControlView_Loaded(object sender, RoutedEventArgs e)
        {
            // 页面被 MainWindow 缓存后会重复进入。
            // 首次进入加载执行器列表，后续进入不再重复初始化，实时状态由 MotionIo.SnapshotChanged 事件驱动。
            if (_isFirstLoaded)
            {
                return;
            }

            _isFirstLoaded = true;
            await _viewModel.LoadAsync();
        }
    }
}
