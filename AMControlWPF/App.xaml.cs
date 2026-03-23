using AM.App.Bootstrap;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.DBService.Services.Auth;
using AM.Model.Common;
using AM.Tools;
using AM.Tools.Logging;
using AM.Tools.Messaging;
using AMControlWPF.Navigation;
using AMControlWPF.Tools.Helper;
using AMControlWPF.Views.Auth;
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

            LangThemeHelper.ApplyFromConfig();

            // 以 NavigationCatalog 为权威来源，全量同步权限目录到 DB
            // 每次启动执行，确保新增/删除的页面自动反映到权限管理界面
            try
            {
                var authSeed = new AuthSeedService(SystemContext.Instance.Reporter);
                authSeed.SyncPagePermissions(NavigationCatalog.ToPermissionEntities());
            }
            catch (Exception ex)
            {
                Console.WriteLine("权限目录同步失败：" + ex.Message);
            }

            // 登录窗口是模态的，必须在主窗口显示之前处理登录逻辑
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var loginView = new LoginView();
            var loginResult = loginView.ShowDialog();
            if (loginResult != true || !UserContext.Instance.IsLoggedIn)
            {
                Shutdown();
                return;
            }
            var mainWindow = new MainWindow();
            MainWindow = mainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            mainWindow.Show();
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
                
            }
            finally
            {
                base.OnExit(e);
            }
            Console.WriteLine("4.OnExit被触发");
        }
    }
}
