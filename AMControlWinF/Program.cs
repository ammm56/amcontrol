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
        private static MainApplicationContext _appContext;

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

            if (!ShowLoginDialog(null))
            {
                Environment.Exit(0);
                return;
            }

            _appContext = new MainApplicationContext();
            _appContext.OpenFirstMainWindow();
            Application.Run(_appContext);
        }

        internal static void SwitchUser()
        {
            if (_appContext != null)
            {
                _appContext.SwitchUser();
            }
        }

        internal static void Logout()
        {
            if (_appContext != null)
            {
                _appContext.Logout();
            }
        }

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

        private sealed class MainApplicationContext : ApplicationContext
        {
            private PendingAction _pendingAction = PendingAction.None;

            public void OpenFirstMainWindow()
            {
                var window = new MainWindow();
                MainForm = window;
                window.Show();
            }

            public void SwitchUser()
            {
                var currentWindow = MainForm as MainWindow;
                if (currentWindow == null || currentWindow.IsDisposed)
                {
                    return;
                }

                if (!ShowLoginDialog(currentWindow))
                {
                    return;
                }

                CloseCurrentMainWindow(currentWindow, PendingAction.ReopenMainWindow);
            }

            public void Logout()
            {
                var currentWindow = MainForm as MainWindow;

                UserContext.Instance.SignOut();

                var loginSuccess = currentWindow != null && !currentWindow.IsDisposed
                    ? ShowLoginDialog(currentWindow)
                    : ShowLoginDialog(null);

                if (currentWindow == null || currentWindow.IsDisposed)
                {
                    if (loginSuccess)
                    {
                        OpenFirstMainWindow();
                    }
                    else
                    {
                        ExitThread();
                        Environment.Exit(0);
                    }

                    return;
                }

                CloseCurrentMainWindow(
                    currentWindow,
                    loginSuccess ? PendingAction.ReopenMainWindow : PendingAction.ExitApplication);
            }

            private void CloseCurrentMainWindow(MainWindow currentWindow, PendingAction actionAfterClose)
            {
                if (currentWindow == null || currentWindow.IsDisposed)
                {
                    return;
                }

                _pendingAction = actionAfterClose;

                MainForm = null;
                currentWindow.FormClosed -= CurrentWindow_FormClosed;
                currentWindow.FormClosed += CurrentWindow_FormClosed;

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
                        ExitThread();
                        Environment.Exit(0);
                        break;
                }
            }

            private enum PendingAction
            {
                None = 0,
                ReopenMainWindow = 1,
                ExitApplication = 2
            }
        }
    }
}