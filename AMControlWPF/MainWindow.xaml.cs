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
        public MainWindow()
        {
            InitializeComponent();

            // SourceInitialized	创建窗体源时引发此事件
            // Activated 当前窗体成为前台窗体时引发此事件
            // Loaded 当前窗体内部所有元素完成布局和呈现时引发此事件
            // ContentRendered 当前窗体的内容呈现之后引发此事件
            // Closing 当前窗体关闭之前引发此事件
            // Deactivated 当前窗体成为后台窗体时引发此事件
            // Closed 当前窗体关闭之后引发此事件
            // Unloaded 当前窗体从元素树中删除时引发此事件
            this.SourceInitialized += MainWindow_SourceInitialized;
            this.Activated += (s, e) => Console.WriteLine("2.MainWindow的Activated被执行");
            this.Loaded += MainWindow_Loaded;
            this.ContentRendered += (s, e) => Console.WriteLine("4.MainWindow的ContentRendered被执行");
            this.Deactivated += (s, e) => Console.WriteLine("5.MainWindow的Deactivated被执行");
            this.Closing += (s, e) => Console.WriteLine("6.MainWindow的Closing被执行");
            this.Closed += (s, e) => Console.WriteLine("7.MainWindow的Closed被执行");
            this.Unloaded += (s, e) => Console.WriteLine("8.MainWindow的Unloaded被执行");


        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("3.MainWindow的Loaded被执行");

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(1000).Wait();

                    // 只有创建控件的线程（主线程）才能修改控件。task后台线程中这样不行
                    // tb_test1.Text = $"测试{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

                    tb_test1.Dispatcher.Invoke(() =>
                    {
                        tb_test1.Text = $"测试{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
                    });

                }

            });
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            Console.WriteLine("1.MainWindow的SourceInitialized被执行");
        }

        private void btn_loadcfg_Click(object sender, RoutedEventArgs e)
        {
            CfgInfo cfgInfo = new CfgInfo();
            cfgInfo.Show();
        }

        private void btn_querydbaxistable_Click(object sender, RoutedEventArgs e)
        {
            AxisTableInfo axisTableInfo = new AxisTableInfo();
            axisTableInfo.Show();
        }
    }
}
