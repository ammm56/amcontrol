using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControlWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Page> _pageCache = new Dictionary<string, Page>();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            LangAndThemeControl.NavigateRequested += LangAndThemeControl_NavigateRequested;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Home");
        }

        private void LangAndThemeControl_NavigateRequested(string tag)
        {
            NavigateToPage(tag);
        }

        private void SideMenu_SelectionChanged(object sender, HandyControl.Data.FunctionEventArgs<object> e)
        {
            var item = e.Info as HandyControl.Controls.SideMenuItem;
            if (item?.Tag == null)
            {
                return;
            }

            NavigateToPage(item.Tag.ToString());
        }

        private void NavigateToPage(string tag)
        {
            if (!_pageCache.ContainsKey(tag))
            {
                _pageCache[tag] = CreatePage(tag);
            }

            MainFrame.Content = _pageCache[tag];
        }

        private Page CreatePage(string tag)
        {
            switch (tag)
            {
                case "Home":
                    return CreatePlaceholderPage("首页");
                case "ConfigArgs":
                    return CreatePlaceholderPage("参数配置");
                case "Axis":
                    return CreatePlaceholderPage("轴控制");
                case "ConfigIO":
                    return CreatePlaceholderPage("IO 配置");
                case "Logs":
                    return CreatePlaceholderPage("报警日志");
                case "Contributors":
                    return CreatePlaceholderPage("贡献者");
                case "Projects":
                    return CreatePlaceholderPage("项目");
                default:
                    return CreatePlaceholderPage("未定义页面: " + tag);
            }
        }

        private Page CreatePlaceholderPage(string title)
        {
            return new Page
            {
                Content = new Border
                {
                    Padding = new Thickness(24),
                    Child = new TextBlock
                    {
                        Text = title,
                        FontSize = 24,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = Brushes.DimGray,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    }
                }
            };
        }
    }
}
