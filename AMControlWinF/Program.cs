using AM.App.Bootstrap;
using AM.Core.Context;
using AM.DBService.Services.Auth;
using AM.PageModel.Navigation;
using AMControlWinF.Views.Auth;
using System;
using System.Windows.Forms;

namespace AMControlWinF
{
    static class Program
    {
        /// <summary>
        /// 应用程序主入口。
        /// 启动顺序：
        /// 1. WinForms 基础初始化
        /// 2. AppBootstrap 初始化全局上下文与硬件/后台任务
        /// 3. 以 NavigationCatalog 为权威来源同步页面权限目录
        /// 4. 显示登录窗体
        /// 5. 登录成功后进入主窗体
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppBootstrap.Initialize();

            try
            {
                var authSeed = new AuthSeedService(SystemContext.Instance.Reporter);
                authSeed.SyncPagePermissions(NavigationCatalog.ToPermissionEntities());
            }
            catch
            {
                // 权限目录同步失败不阻断主流程，避免迁移阶段无法进入登录页。
            }

            if (!ShowLoginDialog())
            {
                return;
            }

            while (true)
            {
                using (var mainWindow = new MainWindow())
                {
                    Application.Run(mainWindow);

                    if (mainWindow.ReopenRequested)
                    {
                        continue;
                    }

                    break;
                }
            }
        }

        private static bool ShowLoginDialog()
        {
            using (var loginForm = new LoginForm())
            {
                return loginForm.ShowDialog() == DialogResult.OK
                    && UserContext.Instance.IsLoggedIn;
            }
        }
    }
}