using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace AMControlWPF.UserControls.Main
{
    /// <summary>
    /// NonClientAreaContent.xaml 的交互逻辑
    /// 放到 GlowWindow 非客户区的内容（承载 LangAndTheme）
    /// </summary>
    public partial class NonClientAreaContent: UserControl
    {
        // 向外抛出导航请求
        public event Action<string> NavigateRequested;

        public string VersionInfo { get; }

        public NonClientAreaContent()
        {
            InitializeComponent();

            VersionInfo = $"v{Assembly.GetExecutingAssembly().GetName().Version}";
            // 将内部 LangAndTheme 的事件透传出去（如果需要）
            LangAndThemeControl.NavigateRequested += tag => NavigateRequested?.Invoke(tag);

            // 设置 DataContext 以绑定 VersionInfo xaml中 bingding 使用
            DataContext = this;
        }

        private void ContributorsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateRequested?.Invoke("Contributors");
        }

        private void ProjectsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateRequested?.Invoke("Projects");
        }

        // 点击标题栏区域可拖动窗口
        private void Root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 只在非右键时拖动
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
            {
                var wnd = Window.GetWindow(this);
                try
                {
                    wnd?.DragMove();
                }
                catch
                {
                    // DragMove 在某些情况下会抛异常，忽略
                }
            }
        }

        // 左侧图标按钮点击
        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Tag is string tag)
            {
                NavigateRequested?.Invoke(tag);
            }
        }

        // 双击标题栏最大化/还原
        private void Root_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            var wnd = Window.GetWindow(this);
            if (wnd == null) return;

            wnd.WindowState = wnd.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}
