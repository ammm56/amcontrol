using AM.App.Bootstrap;
using AM.Core.Context;
using AM.DBService.Services.Auth;
using AM.PageModel.Navigation;
using AMControlWinF.Views.Auth;
using System;
using System.Windows.Forms;

namespace AMControlWinF
{
    /// <summary>
    /// WinForms 程序入口。
    /// 说明：
    /// 1. 设备、扫描、运行时上下文只允许初始化一次；
    /// 2. 主界面切换用户/退出登录时，必须保证运行期始终只有一个 MainWindow；
    /// 3. 登录成功后的主窗体重开流程严格采用“旧窗体先关，再开新窗体”；
    /// 4. 首次登录取消、退出登录后再次登录取消，均直接退出整个程序。
    /// </summary>
    static class Program
    {
        /// <summary>
        /// WinForms 全局应用上下文。
        /// 统一托管主窗体生命周期，避免出现多个 MainWindow 并存。
        /// </summary>
        private static MainApplicationContext _appContext;

        /// <summary>
        /// 应用程序主入口。
        /// 启动顺序：
        /// 1. WinForms 基础初始化；
        /// 2. AppBootstrap 初始化全局上下文、设备连接与后台扫描；
        /// 3. 同步页面权限目录；
        /// 4. 先显示登录窗体；
        /// 5. 登录成功后进入唯一主窗体。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 组合根初始化：
            // 仅执行一次，负责构建全局设备上下文、系统上下文与后台扫描任务。
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

            // 首次登录必须在主消息循环启动前完成。
            // 取消登录则直接退出整个程序，不创建主窗体。
            if (!ShowLoginDialog(null))
            {
                Environment.Exit(0);
                return;
            }

            _appContext = new MainApplicationContext();
            _appContext.OpenFirstMainWindow();

            // 启动 WinForms 主消息循环。
            // 之后主窗体的关闭/重开统一由 MainApplicationContext 管理。
            Application.Run(_appContext);
        }

        /// <summary>
        /// 供主窗体调用：切换用户。
        /// 规则：
        /// - 登录成功：先关闭旧 MainWindow，旧窗体关闭完成后再打开新 MainWindow；
        /// - 登录取消：保持当前主窗体不变。
        /// </summary>
        internal static void SwitchUser()
        {
            if (_appContext != null)
            {
                _appContext.SwitchUser();
            }
        }

        /// <summary>
        /// 供主窗体调用：退出登录。
        /// 规则：
        /// - 先执行 SignOut；
        /// - 再弹登录框；
        /// - 登录成功：先关旧窗体，再开新窗体；
        /// - 登录取消：关闭当前主窗体并退出整个程序。
        /// </summary>
        internal static void Logout()
        {
            if (_appContext != null)
            {
                _appContext.Logout();
            }
        }

        /// <summary>
        /// 统一显示登录窗体。
        /// owner 为空表示程序首次启动登录；
        /// owner 不为空表示在已有主窗体基础上做切换用户/退出登录后的再次登录。
        /// </summary>
        private static bool ShowLoginDialog(IWin32Window owner)
        {
            using (var loginForm = new LoginForm())
            {
                var result = owner == null
                    ? loginForm.ShowDialog()
                    : loginForm.ShowDialog(owner);

                return result == DialogResult.OK
                    && UserContext.Instance.IsLoggedIn;
            }
        }

        /// <summary>
        /// WinForms 应用上下文。
        /// 核心职责：
        /// - 托管唯一 MainWindow；
        /// - 控制“旧窗体先关，再开新窗体”的顺序；
        /// - 统一处理切换用户、退出登录、退出程序。
        /// </summary>
        private sealed class MainApplicationContext : ApplicationContext
        {
            /// <summary>
            /// 旧主窗体关闭后要执行的动作。
            /// </summary>
            private PendingAction _pendingAction = PendingAction.None;

            /// <summary>
            /// 登录/登出切换流程重入保护。
            /// 避免头像菜单连续点击时重复弹登录框或重复关窗。
            /// </summary>
            private bool _isAuthTransitioning;

            /// <summary>
            /// 首次打开主窗体。
            /// 启动阶段此时不存在旧主窗体，可直接创建。
            /// </summary>
            public void OpenFirstMainWindow()
            {
                var window = new MainWindow();
                MainForm = window;
                window.Show();
            }

            /// <summary>
            /// 切换用户。
            /// 注意：
            /// 不在当前头像弹层点击事件链中立即弹登录框，
            /// 而是延后到主窗体消息循环尾部执行，避免控件仍在使用中。
            /// </summary>
            public void SwitchUser()
            {
                var currentWindow = MainForm as MainWindow;
                if (currentWindow == null || currentWindow.IsDisposed)
                {
                    return;
                }

                RunAuthFlow(currentWindow, new Action(() =>
                {
                    if (currentWindow.IsDisposed || !ReferenceEquals(MainForm, currentWindow))
                    {
                        return;
                    }

                    // 切换用户取消时，保持当前主窗体继续运行。
                    if (!ShowLoginDialog(currentWindow))
                    {
                        return;
                    }

                    CloseCurrentMainWindow(currentWindow, PendingAction.ReopenMainWindow);
                }));
            }

            /// <summary>
            /// 退出登录。
            /// 规则：
            /// - 先清理当前登录态；
            /// - 再弹登录框；
            /// - 登录成功：关闭旧主窗体，关闭完成后重开新主窗体；
            /// - 登录取消：关闭旧主窗体，关闭完成后退出程序。
            /// </summary>
            public void Logout()
            {
                var currentWindow = MainForm as MainWindow;

                RunAuthFlow(currentWindow, new Action(() =>
                {
                    UserContext.Instance.SignOut();

                    var activeWindow = MainForm as MainWindow;
                    var loginOwner = activeWindow != null && !activeWindow.IsDisposed
                        ? (IWin32Window)activeWindow
                        : null;

                    var loginSuccess = ShowLoginDialog(loginOwner);

                    if (activeWindow == null || activeWindow.IsDisposed)
                    {
                        if (loginSuccess)
                        {
                            OpenFirstMainWindow();
                        }
                        else
                        {
                            ExitApplication();
                        }

                        return;
                    }

                    CloseCurrentMainWindow(
                        activeWindow,
                        loginSuccess ? PendingAction.ReopenMainWindow : PendingAction.ExitApplication);
                }));
            }

            /// <summary>
            /// 将认证相关 UI 流程延后到当前消息循环尾部执行，
            /// 避免在头像弹层/按钮点击事件链中直接打开模态窗体。
            /// </summary>
            private void RunAuthFlow(MainWindow currentWindow, Action action)
            {
                if (action == null || _isAuthTransitioning)
                {
                    return;
                }

                _isAuthTransitioning = true;

                var execute = new Action(() =>
                {
                    try
                    {
                        action();
                    }
                    finally
                    {
                        _isAuthTransitioning = false;
                    }
                });

                if (currentWindow != null &&
                    !currentWindow.IsDisposed &&
                    currentWindow.IsHandleCreated)
                {
                    currentWindow.BeginInvoke(execute);
                    return;
                }

                execute();
            }

            /// <summary>
            /// 关闭当前唯一主窗体。
            /// 关键要求：
            /// 1. 先清空 MainForm，避免旧实例仍被 ApplicationContext 视为当前主窗体；
            /// 2. 监听旧窗体 FormClosed；
            /// 3. 必须等旧窗体真正关闭后，再决定是否打开新主窗体或退出程序。
            /// </summary>
            private void CloseCurrentMainWindow(MainWindow currentWindow, PendingAction actionAfterClose)
            {
                if (currentWindow == null || currentWindow.IsDisposed)
                {
                    return;
                }

                _pendingAction = actionAfterClose;

                // 先从 ApplicationContext 解绑当前主窗体。
                // 这样旧窗体关闭后，不会继续作为活动主窗体残留。
                MainForm = null;

                currentWindow.FormClosed -= CurrentWindow_FormClosed;
                currentWindow.FormClosed += CurrentWindow_FormClosed;

                // 延后到当前消息循环尾部再关窗，
                // 避免在当前按钮/弹层事件链中直接 Close 导致关闭时序不稳定。
                currentWindow.BeginInvoke(new Action(() =>
                {
                    if (currentWindow.IsDisposed)
                    {
                        return;
                    }

                    currentWindow.Hide();
                    currentWindow.Close();
                }));
            }

            /// <summary>
            /// 旧主窗体真正关闭后的统一收口。
            /// 只有在这里，才允许：
            /// - 打开新 MainWindow；
            /// - 或退出整个程序。
            /// 这样可确保运行时不会同时存在新旧两个 MainWindow。
            /// </summary>
            private void CurrentWindow_FormClosed(object sender, FormClosedEventArgs e)
            {
                var closedWindow = sender as MainWindow;
                if (closedWindow != null)
                {
                    closedWindow.FormClosed -= CurrentWindow_FormClosed;
                }

                var action = _pendingAction;
                _pendingAction = PendingAction.None;

                switch (action)
                {
                    case PendingAction.ReopenMainWindow:
                        OpenFirstMainWindow();
                        break;

                    case PendingAction.ExitApplication:
                        ExitApplication();
                        break;
                }
            }

            private void ExitApplication()
            {
                ExitThread();
                Environment.Exit(0);
            }

            /// <summary>
            /// 旧主窗体关闭后的待执行动作。
            /// </summary>
            private enum PendingAction
            {
                None = 0,
                ReopenMainWindow = 1,
                ExitApplication = 2
            }
        }
    }
}