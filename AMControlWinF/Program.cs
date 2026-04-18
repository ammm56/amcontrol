using AM.App.Bootstrap;
using AM.Core.Context;
using AM.DBService.Services.Auth;
using AM.DBService.Services.System;
using AM.PageModel.Main;
using AM.PageModel.Navigation;
using AMControlWinF.Views.Auth;
using System;
using System.Windows.Forms;

namespace AMControlWinF
{
    /// <summary>
    /// WinForms 程序入口。
    /// 
    /// 生命周期采用主循环模式：
    ///   while → 登录 → Application.Run(MainWindow) → 根据退出原因决定重登录或退出。
    /// 
    /// 核心保障：
    /// - 设备/扫描/运行时上下文全局只初始化一次；
    /// - Application.Run 同步阻塞，同一时刻最多存在一个 MainWindow；
    /// - 旧 MainWindow 的 using 块确保资源释放后才创建新实例。
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            UsageEventBufferService usageEventBufferService = null;

            try
            {
                // 全局唯一初始化：设备上下文、系统上下文、后台扫描。
                AppBootstrap.Initialize();
                SyncPagePermissions();

                // 启动后记录程序启动事件。
                usageEventBufferService = new UsageEventBufferService();
                usageEventBufferService.SaveAppStart();

                // 首次登录，取消则直接退出。
                if (!ShowLogin(null))
                    return;

                // 主循环：MainWindow 关闭后根据退出原因决定后续流程。
                while (true)
                {
                    MainWindowExitReason exitReason;

                    using (var window = new MainWindow())
                    {
                        Application.Run(window);
                        exitReason = window.ExitReason;
                    }

                    switch (exitReason)
                    {
                        case MainWindowExitReason.SwitchUser:
                            // 切换用户：LoginForm 已在 MainWindow 内以模态完成认证，
                            // 直接进入下一轮创建新 MainWindow。
                            continue;

                        case MainWindowExitReason.Logout:
                            // 退出登录：清除登录态，重新弹出登录窗。
                            UserContext.Instance.SignOut();
                            if (ShowLogin(null))
                                continue;
                            return;

                        default:
                            // 正常关闭（关闭按钮）→ 退出程序。
                            return;
                    }
                }
            }
            finally
            {
                try
                {
                    if (SystemContext.Instance.RuntimeTaskManager != null)
                    {
                        SystemContext.Instance.RuntimeTaskManager.StopAllAsync().GetAwaiter().GetResult();
                    }
                }
                catch
                {
                }

                try
                {
                    usageEventBufferService?.SaveAppExit();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 显示登录窗体。返回 true 表示登录成功。
        /// </summary>
        private static bool ShowLogin(IWin32Window owner)
        {
            using (var form = new LoginForm())
            {
                var result = owner != null
                    ? form.ShowDialog(owner)
                    : form.ShowDialog();

                return result == DialogResult.OK
                    && UserContext.Instance.IsLoggedIn;
            }
        }

        /// <summary>
        /// 同步页面权限目录到数据库。
        /// 失败不阻断主流程，避免迁移阶段无法进入登录页。
        /// </summary>
        private static void SyncPagePermissions()
        {
            try
            {
                var authSeed = new AuthSeedService(SystemContext.Instance.Reporter);
                authSeed.SyncPagePermissions(NavigationCatalog.ToPermissionEntities());
            }
            catch
            {
            }
        }

    }
}