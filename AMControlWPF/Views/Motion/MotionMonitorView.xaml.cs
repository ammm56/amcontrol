using AM.ViewModel.ViewModels.Motion;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Motion
{
    /// <summary>
    /// MotionMonitorView.xaml 的交互逻辑。
    ///
    /// 重要说明：
    /// 当前主窗口对二级页面采用缓存复用模式（见 MainWindow 的 _pageCache）。
    /// 因此该页面实例在导航切换后不会被销毁，而是会被再次显示。
    ///
    /// 在这种模式下：
    /// 1. 不要在 Unloaded 中释放 ViewModel，否则会断开运行态订阅，导致再次进入页面后不再实时刷新。
    /// 2. 首次加载与后续再次显示要区别处理，首次进入执行 LoadAsync，后续依赖运行态事件持续刷新。
    /// </summary>
    public partial class MotionMonitorView : UserControl
    {
        private readonly MotionMonitorViewModel _viewModel;
        private bool _isFirstLoaded;

        public MotionMonitorView()
        {
            InitializeComponent();

            _viewModel = new MotionMonitorViewModel();
            DataContext = _viewModel;

            Loaded += MotionMonitorView_Loaded;
        }

        private async void MotionMonitorView_Loaded(object sender, RoutedEventArgs e)
        {
            // 页面被 MainWindow 缓存后会重复进入。
            // 首次进入加载静态结构，后续进入不再重复初始化，实时数据由运行态事件驱动。
            if (_isFirstLoaded)
            {
                return;
            }

            _isFirstLoaded = true;
            await _viewModel.LoadAsync();
        }
    }
}