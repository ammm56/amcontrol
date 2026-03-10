using AM.Model.Common;
using AM.Model.ICommon;
using AM.Model.Structs;
using AM.MotionCard.Googo;
using AM.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AMControlWPF
{
    /// <summary>
    /// GSNA.xaml 的交互逻辑
    /// </summary>
    public partial class GSNA : Window
    {
        public static IMotionCard Card { get; private set;  }

        public GSNA()
        {
            InitializeComponent();


            // 在 ViewModel 初始化时
            //motionCard.OnError += (cardId, msg) =>
            //{
            //    // 回到 UI 线程弹出消息
            //    Application.Current.Dispatcher.Invoke(() =>
            //    {
            //        MessageBox.Show($"控制卡{cardId}报警：{msg}");
            //    });
            //};

            this.btn_init.Click += btn_init_Click;

            tb_statusinfo.Text = string.Empty;
        }

        /// <summary>
        /// 初始化 实例化卡和设置配置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_init_Click(object sender, RoutedEventArgs e)
        {
            // 0. 检查是否已初始化
            if (ConfigSingle.Instance.Card != null)
            {
                MessageBox.Show("控制卡已初始化，请勿重复操作");
                return;
            }

            btn_init.IsEnabled = false; // 禁用按钮防止连续点击

            try
            {
                await Task.Run(() =>
                {
                    // 1. 根据配置创建具体的卡 (实现层)
                    Card = new GoogoMotionCard();
                    
                    // 把配置给具体的卡实例
                    Card.LoadAxisConfig(ConfigSingle.Instance.Config.MotionCardConfig.AxisConfigs);

                    // 3. 订阅全局错误 (UI反馈)
                    Card.OnError += (id, msg) => {
                        // 这里调用通知 UI 的逻辑
                        Console.WriteLine($"[报警] 卡{id}: {msg}");
                        Application.Current.Dispatcher.Invoke(() => {
                            // 这里弹窗，或者绑定到一个 UI 上的“报警列表”
                            //tb_statusinfo.Text += $"报警: {msg} \n";
                            tb_statusinfo.AppendText($"[{DateTime.Now:T}] {msg}\r\n");
                        });
                    };
                    // 4. 赋值到全局单例，统一访问
                    ConfigSingle.Instance.Card = Card;

                });

                tb_statusinfo.AppendText($"[{DateTime.Now:T}] 控制卡初始化参数配置成功\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"异常: {ex.Message}");
            }
            finally
            {
                btn_init.IsEnabled = true; // 无论成功失败都恢复按钮
            }
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            // 串联多卡与事件 (串联逻辑)
            Card.Connect();
        }

        private void btn_clearstatus_Click(object sender, RoutedEventArgs e)
        {
            Card.ClearAllAxisStatus();
        }

        private void btn_enable_Click(object sender, RoutedEventArgs e)
        {
            Card.Enable(1, true);
        }

        private void btn_pointclearzero_Click(object sender, RoutedEventArgs e)
        {
            Card.SetZeroPos(1);
        }

        private void btn_runstaart_Click(object sender, RoutedEventArgs e)
        {
            Card.MoveRelativeMm(1, 2, 1);
        }

        private void btn_runstop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tb_statusinfo_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 只要内容变动，自动聚焦并滚动到末尾
            tb_statusinfo.ScrollToEnd();
        }

        private void btn_clearoutinfo_Click(object sender, RoutedEventArgs e)
        {
            tb_statusinfo.Text = string.Empty;
        }

        private void btn_configaxisarg_Click(object sender, RoutedEventArgs e)
        {
            // 5. 初始化所有轴的硬件参数（手动配置替代 cfg 文件）
            foreach (var axisCfg in ConfigSingle.Instance.Config.MotionCardConfig.AxisConfigs)
            {
                Card.ConfigAxisHardware(axisCfg);
            }
        }

        private void btn_stopall_Click(object sender, RoutedEventArgs e)
        {
            Card?.StopAll(true);
        }

        private void btn_jogmoveZmm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Card?.JogStop(1);
        }

        private void btn_jogmoveZmm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Card?.JogMoveMm(1, true, 10);
        }

        private void btn_jogmoveFmm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Card?.JogMoveMm(1, false, 10);
        }

        private void btn_init_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
