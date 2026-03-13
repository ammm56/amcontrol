using AM.Model.Common;
using AM.Model.Interfaces;
using AM.Model.Interfaces.MotionCard;
using AM.Model.Structs;
using AM.MotionCard.Googo;
using AM.Core.Context;
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
        private IMotionCardService MotionCard
        {
            get { return MachineContext.Instance.MotionCard; }
        }

        public GSNA()
        {
            InitializeComponent();


            this.Loaded += GSNA_Loaded;
            this.Closed += GSNA_Closed;
            this.btn_init.Click += btn_init_Click;

            tb_statusinfo.Text = string.Empty;
        }

        private void GSNA_Loaded(object sender, RoutedEventArgs e)
        {
            // 订阅全局错误 (UI反馈)
            MotionCard.OnError += MotionCard_OnError;
            tb_statusinfo.AppendText($"[{DateTime.Now:T}] 控制卡初始化参数配置成功\r\n");
        }
        private void GSNA_Closed(object sender, EventArgs e)
        {
            MotionCard.OnError -= MotionCard_OnError;
        }
        private void MotionCard_OnError(short id, string msg)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // 这里弹窗，或者绑定到一个 UI 上的“报警列表”
                tb_statusinfo.AppendText($"[{DateTime.Now:T}] {msg}\r\n");
            });
        }

        /// <summary>
        /// 初始化 实例化卡和设置配置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_init_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            // 串联多卡与事件 (串联逻辑)
            MotionCard.Connect();
        }

        private void btn_clearstatus_Click(object sender, RoutedEventArgs e)
        {
            MotionCard.ClearAllAxisStatus();
        }

        private void btn_enable_Click(object sender, RoutedEventArgs e)
        {
            MotionCard.Enable(114, true);
        }

        private void btn_pointclearzero_Click(object sender, RoutedEventArgs e)
        {
            MotionCard.SetZeroPos(1);
        }

        private void btn_runstaart_Click(object sender, RoutedEventArgs e)
        {
            MotionCard.MoveRelativeMm(1, 2, 1);
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
            foreach (var axisCfg in ConfigContext.Instance.Config.MotionCardConfig.AxisConfigs)
            {
                MotionCard.ConfigAxisHardware(axisCfg);
            }
        }

        private void btn_stopall_Click(object sender, RoutedEventArgs e)
        {
            MotionCard?.StopAll(true);
        }

        private void btn_jogmoveZmm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MotionCard?.JogStop(1);
        }

        private void btn_jogmoveZmm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MotionCard?.JogMoveMm(1, true, 10);
        }

        private void btn_jogmoveFmm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MotionCard?.JogMoveMm(1, false, 10);
        }

        private void btn_init_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
