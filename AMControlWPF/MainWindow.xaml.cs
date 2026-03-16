using AM.Tools;
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

namespace AMControlWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 页面缓存：防止重复创建对象
        private readonly Dictionary<string, Page> _pageCache = new Dictionary<string, Page>();

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;


            // 默认加载第一个页面
            //NavigateToPage("Home");

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void SideMenu_SelectionChanged(object sender, HandyControl.Data.FunctionEventArgs<object> e)
        {
            var item = e.Info as HandyControl.Controls.SideMenuItem;
            if(item?.Tag == null) return;

            // 获取 Tag 并跳转页面
            NavigateToPage(item.Tag.ToString());
        }

        private void NavigateToPage(string tag)
        {
            if (!_pageCache.ContainsKey(tag))
            {
                // 根据 Tag 实例化不同的页面
                
            }

            // 切换 Frame 的内容
            MainFrame.Content = _pageCache[tag];
        }
    }
}
