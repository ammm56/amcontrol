using AM.App.Bootstrap;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Model.Common;
using AM.Tools;
using AM.Tools.Logging;
using AM.Tools.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AMControlWPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 输入关键词 override 按一下 空格，VS 会自动弹出一个下拉列表，列出所有可重写的生命周期方法。
        /// WinForms 倾向于使用“事件订阅”（Event Subscription）。
        /// WPF App 建议优先使用“方法重写”（Method Overriding），因为这样可以更早地拦截启动逻辑，且不需要在 XAML 中额外注册。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Console.WriteLine("1.OnStartup被触发");

            // 调用系统、服务、配置、日志等的初始化
            AppBootstrap.Initialize();

        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Console.WriteLine("2.OnActivated被触发");
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            Console.WriteLine("3.OnDeactivated被触发");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                MachineContext.Instance.MotionCard?.Disconnect();
            }
            finally
            {
                base.OnExit(e);
            }
            Console.WriteLine("4.OnExit被触发");
        }
    }
}
