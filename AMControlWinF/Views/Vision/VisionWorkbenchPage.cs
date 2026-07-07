using AMControlWinF.Tools;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Vision
{
    /// <summary>
    /// 视觉工作台页面。
    /// 仅嵌入 amvision 前端工作台，amcontrol 不管理 amvision 的 workflow/runtime/trigger 配置。
    /// </summary>
    public partial class VisionWorkbenchPage : UserControl
    {
        private const string WorkbenchUrl = "http://127.0.0.1:5601/projects";

        private bool _isInitializing;
        private bool _isWebViewReady;
        private bool _eventsBound;
        private DateTime _navigationStartTime;

        public VisionWorkbenchPage()
        {
            InitializeComponent();
            BindEvents();
            RefreshActionState();
        }

        private void BindEvents()
        {
            Load += VisionWorkbenchPage_Load;
            buttonRefresh.Click += async (s, e) => await ReloadWorkbenchAsync();
            buttonOpenExternal.Click += (s, e) => OpenWorkbenchInExternalBrowser();
            buttonRetry.Click += async (s, e) => await InitializeWebViewAsync(true);
            buttonFallbackOpen.Click += (s, e) => OpenWorkbenchInExternalBrowser();
        }

        private async void VisionWorkbenchPage_Load(object sender, EventArgs e)
        {
            await InitializeWebViewAsync(false);
        }

        private async Task InitializeWebViewAsync(bool forceNavigate)
        {
            if (_isInitializing)
            {
                return;
            }

            _isInitializing = true;
            RefreshActionState();
            SetStatus("正在连接视觉工作台...");
            HideFallback();

            try
            {
                if (webViewWorkbench.CoreWebView2 != null)
                {
                    BindWebViewEventsOnce();
                    ConfigureWebView();
                    _isWebViewReady = true;

                    if (forceNavigate)
                    {
                        NavigateWorkbench();
                    }

                    return;
                }

                var userDataFolder = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "WebView2",
                    "VisionWorkbench");

                Directory.CreateDirectory(userDataFolder);

                var environment = await CoreWebView2Environment.CreateAsync(
                    null,
                    userDataFolder);

                await webViewWorkbench.EnsureCoreWebView2Async(environment);

                if (webViewWorkbench.CoreWebView2 == null)
                {
                    ShowFallback("WebView2 初始化失败", "浏览器内核未完成初始化。");
                    return;
                }

                BindWebViewEventsOnce();
                ConfigureWebView();
                _isWebViewReady = true;

                if (forceNavigate || webViewWorkbench.Source == null ||
                    string.Equals(webViewWorkbench.Source.ToString(), "about:blank", StringComparison.OrdinalIgnoreCase))
                {
                    NavigateWorkbench();
                }
            }
            catch (Exception ex)
            {
                _isWebViewReady = false;
                ShowFallback("视觉工作台无法嵌入", BuildInitializeErrorMessage(ex));
            }
            finally
            {
                _isInitializing = false;
                RefreshActionState();
            }
        }

        private void BindWebViewEventsOnce()
        {
            if (_eventsBound || webViewWorkbench.CoreWebView2 == null)
            {
                return;
            }

            webViewWorkbench.NavigationStarting += WebViewWorkbench_NavigationStarting;
            webViewWorkbench.NavigationCompleted += WebViewWorkbench_NavigationCompleted;
            webViewWorkbench.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;
            _eventsBound = true;
        }

        private void ConfigureWebView()
        {
            if (webViewWorkbench.CoreWebView2 == null)
            {
                return;
            }

            webViewWorkbench.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            webViewWorkbench.CoreWebView2.Settings.AreDevToolsEnabled = true;
            webViewWorkbench.CoreWebView2.Settings.IsStatusBarEnabled = false;
        }

        private void NavigateWorkbench()
        {
            if (webViewWorkbench.CoreWebView2 == null)
            {
                return;
            }

            _navigationStartTime = DateTime.Now;
            webViewWorkbench.CoreWebView2.Navigate(WorkbenchUrl);
        }

        private async Task ReloadWorkbenchAsync()
        {
            if (_isInitializing)
            {
                return;
            }

            if (!_isWebViewReady || webViewWorkbench.CoreWebView2 == null)
            {
                await InitializeWebViewAsync(true);
                return;
            }

            HideFallback();
            SetStatus("正在刷新视觉工作台...");
            _navigationStartTime = DateTime.Now;
            webViewWorkbench.CoreWebView2.Reload();
        }

        private void WebViewWorkbench_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            _navigationStartTime = DateTime.Now;
            HideFallback();
            SetStatus("正在加载 " + WorkbenchUrl);
        }

        private void WebViewWorkbench_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e != null && e.IsSuccess)
            {
                var elapsed = DateTime.Now - _navigationStartTime;
                SetStatus("视觉工作台已连接，耗时 " + Math.Max(0, (int)elapsed.TotalMilliseconds) + " ms");
                HideFallback();
                return;
            }

            var status = e == null ? "Unknown" : e.WebErrorStatus.ToString();
            ShowFallback(
                "视觉工作台加载失败",
                "请确认 amvision 前端服务已启动并监听 " + WorkbenchUrl + "。\r\n错误状态：" + status);
        }

        private void CoreWebView2_ProcessFailed(object sender, CoreWebView2ProcessFailedEventArgs e)
        {
            var reason = e == null ? "Unknown" : e.ProcessFailedKind.ToString();
            ShowFallback(
                "视觉工作台进程异常",
                "WebView2 浏览器进程异常：" + reason + "。可刷新重试，或使用外部浏览器打开。");
        }

        private void OpenWorkbenchInExternalBrowser()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = WorkbenchUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                PageDialogHelper.ShowWarn(this, "打开视觉工作台", ex.Message);
            }
        }

        private void ShowFallback(string title, string message)
        {
            labelFallbackTitle.Text = string.IsNullOrWhiteSpace(title) ? "视觉工作台不可用" : title;
            labelFallbackMessage.Text = string.IsNullOrWhiteSpace(message) ? "请检查视觉前端服务状态。" : message;
            panelFallback.Visible = true;
            panelFallback.BringToFront();
            SetStatus(labelFallbackTitle.Text);
        }

        private void HideFallback()
        {
            panelFallback.Visible = false;
            webViewWorkbench.BringToFront();
        }

        private void SetStatus(string text)
        {
            labelStatus.Text = "";
            //labelStatus.Text = string.IsNullOrWhiteSpace(text) ? "视觉工作台待连接" : text;
        }

        private void RefreshActionState()
        {
            buttonRefresh.Enabled = !_isInitializing;
            buttonRetry.Enabled = !_isInitializing;
            buttonOpenExternal.Enabled = !_isInitializing;
            buttonFallbackOpen.Enabled = !_isInitializing;
        }

        private static string BuildInitializeErrorMessage(Exception ex)
        {
            if (ex == null)
            {
                return "WebView2 初始化失败。";
            }

            return "WebView2 初始化失败：" + ex.Message +
                   "\r\n请确认 WebView2 Runtime 已安装，且 WebView2Loader.dll 已复制到程序输出目录。";
        }
    }
}
