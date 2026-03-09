using AM.Model.Common;
using AM.Model.ICommon;
using AM.Model.Structs;
using AM.MotionCard.Googo;
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
        }

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
                    // 2. 串联多卡与事件 (串联逻辑)
                    Card.Connect(1);

                    // 把配置给具体的卡实例
                    Card.LoadAxisConfig(ConfigSingle.Instance.Config.AxisConfigs);

                    // 绑定轴参数 (单位转换)
                    foreach (var axisCfg in ConfigSingle.Instance.Config.AxisConfigs)
                    {
                        Card.SetAxisParam(axisCfg.AxisId, new AxisParam
                        {
                            Lead = axisCfg.Lead,
                            PulsePerRev = axisCfg.PulsePerRev
                        });
                    }
                    // 3. 订阅全局错误 (UI反馈)
                    Card.OnError += (id, msg) => {
                        // 这里调用通知 UI 的逻辑
                        Console.WriteLine($"[报警] 卡{id}: {msg}");
                        Application.Current.Dispatcher.Invoke(() => {
                            // 这里弹窗，或者绑定到一个 UI 上的“报警列表”
                            tb_statusinfo.Text = $"报警: {msg}";
                        });
                    };
                    // 4. 赋值到全局单例，统一访问
                    ConfigSingle.Instance.Card = Card;

                });

                MessageBox.Show("控制卡初始化成功");
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

        private void btn_clearstatus_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_enable_Click(object sender, RoutedEventArgs e)
        {
            ConfigSingle.Instance.Card.Enable(1, true);
        }

        private void btn_pointclearzero_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_runstaart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_runstop_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
