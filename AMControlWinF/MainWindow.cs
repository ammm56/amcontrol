using AM.Core.Context;
using AM.PageModel.Main;
using AM.PageModel.Navigation;
using AM.Tools;
using AMControlWinF.Views.Auth;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Label = AntdUI.Label;
using MenuItem = AntdUI.MenuItem;
using Panel = AntdUI.Panel;

namespace AMControlWinF
{
    /// <summary>
    /// WinForms 主窗体壳层。
    /// 仅负责：
    /// 1. 构建一级/二级导航；
    /// 2. 承载右侧工作区与页面缓存；
    /// 3. 协调用户头像菜单；
    /// 4. 协调语言与主题切换。
    /// </summary>
    public partial class MainWindow : AntdUI.Window
    {
        private readonly MainWindowModel _model;
        private readonly Dictionary<string, Control> _pageCache;
        private readonly Dictionary<string, Func<Control>> _pageFactories;
        private readonly Dictionary<string, string> _secondaryIcons;

        private bool _isUpdatingUiState;
        private bool _isDarkMode;

        public MainWindow()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
            UpdateStyles();

            _model = new MainWindowModel();
            _pageCache = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
            _pageFactories = CreatePageFactories();
            _secondaryIcons = CreateSecondaryIconMap();

            BindEvents();
            InitializeShellState();
            LoadModel();
        }

        #region 初始化

        /// <summary>
        /// 统一绑定主窗体事件。
        /// </summary>
        private void BindEvents()
        {
            menuPrimary.SelectChanged += MenuPrimary_SelectChanged;
            menuSecondary.SelectChanged += MenuSecondary_SelectChanged;

            userAvatarMenuControl.SwitchUserRequested += UserAvatarMenuControl_SwitchUserRequested;
            userAvatarMenuControl.ChangePasswordRequested += UserAvatarMenuControl_ChangePasswordRequested;
            userAvatarMenuControl.LogoutRequested += UserAvatarMenuControl_LogoutRequested;

            buttonColorMode.Click += ButtonColorMode_Click;
            dropdownTranslate.SelectedValueChanged += DropdownTranslate_SelectedValueChanged;
            FormClosed += MainWindow_FormClosed;
        }

        /// <summary>
        /// 从全局配置初始化语言和主题。
        /// </summary>
        private void InitializeShellState()
        {
            _isUpdatingUiState = true;

            try
            {
                var setting = ConfigContext.Instance.Config.Setting;
                var language = string.IsNullOrWhiteSpace(setting.Language) ? "zh-CN" : setting.Language;
                var theme = string.IsNullOrWhiteSpace(setting.Theme) ? "SkinDefault" : setting.Theme;

                dropdownTranslate.SelectedValue = IsEnglishLanguage(language) ? "English" : "简体中文";

                _isDarkMode = IsDarkTheme(theme);
                buttonColorMode.Toggle = _isDarkMode;

                ApplyLanguage(language, false);
                ApplyTheme(_isDarkMode, false);
            }
            finally
            {
                _isUpdatingUiState = false;
            }
        }

        /// <summary>
        /// 首次加载导航与默认页面。
        /// </summary>
        private void LoadModel()
        {
            _model.LoadNavigation();
            RefreshShell();
        }

        #endregion

        #region 壳层刷新

        /// <summary>
        /// 直接刷新整个壳层。
        /// 当前页面结构不复杂，优先保持实现简单清晰。
        /// </summary>
        private void RefreshShell()
        {
            SuspendShellLayouts();

            try
            {
                BuildPrimaryMenu();
                BuildSecondaryMenu();
                RefreshHeader();
                RefreshUserMenuControl();
                NavigateToSelectedPage();
            }
            finally
            {
                ResumeShellLayouts(true);
            }
        }

        #endregion

        #region 导航

        /// <summary>
        /// 一级导航为窄栏文本导航。
        /// </summary>
        private void BuildPrimaryMenu()
        {
            menuPrimary.Items.Clear();

            foreach (var item in _model.PrimaryItems)
            {
                menuPrimary.Items.Add(new MenuItem
                {
                    Text = GetPrimaryText(item),
                    Tag = item.Key
                });
            }
        }

        /// <summary>
        /// 二级导航保留图标。
        /// </summary>
        private void BuildSecondaryMenu()
        {
            menuSecondary.Items.Clear();

            foreach (var item in _model.SecondaryItems)
            {
                menuSecondary.Items.Add(new MenuItem
                {
                    Text = GetSecondaryText(item),
                    Tag = item.PageKey,
                    IconSvg = ResolveSecondaryIcon(item.PageKey)
                });
            }
        }

        private void MenuPrimary_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            var key = GetMenuItemTag(e);
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var selected = _model.PrimaryItems.FirstOrDefault(x =>
                string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
            {
                return;
            }

            if (_model.SelectedPrimary != null &&
                string.Equals(_model.SelectedPrimary.Key, selected.Key, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _model.SelectedPrimary = selected;
            _model.LoadSecondaryItems(selected.Key);

            BuildSecondaryMenu();
            RefreshHeader();
            NavigateToSelectedPage();
        }

        private void MenuSecondary_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            var pageKey = GetMenuItemTag(e);
            if (string.IsNullOrWhiteSpace(pageKey))
            {
                return;
            }

            var selected = _model.SecondaryItems.FirstOrDefault(x =>
                string.Equals(x.PageKey, pageKey, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
            {
                return;
            }

            if (_model.SelectedSecondary != null &&
                string.Equals(_model.SelectedSecondary.PageKey, selected.PageKey, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _model.SelectedSecondary = selected;
            RefreshHeader();
            NavigateToSelectedPage();
        }

        /// <summary>
        /// 刷新右侧工作区头部。
        /// </summary>
        private void RefreshHeader()
        {
            if (_model.SelectedPrimary == null)
            {
                labelPrimaryTitleValue.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "No module available"
                    : "无可用模块";
            }
            else
            {
                labelPrimaryTitleValue.Text = GetPrimaryText(_model.SelectedPrimary).Replace("\n", string.Empty);
            }

            if (_model.SelectedSecondary == null)
            {
                labelPageTitleValue.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "No page available"
                    : "无可用页面";

                labelPageDescriptionValue.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "The current user has no accessible pages."
                    : "当前用户没有可访问页面";
            }
            else
            {
                labelPageTitleValue.Text = GetSecondaryText(_model.SelectedSecondary);
                labelPageDescriptionValue.Text = GetSecondaryDescription(_model.SelectedSecondary);
            }
        }

        private string GetPrimaryText(NavPrimaryDef item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            if (IsEnglishLanguage(GetCurrentLanguage()))
            {
                return item.Key;
            }

            switch (item.Key)
            {
                case "MotionConfig":
                    return "运控\n配置";
                case "SysConfig":
                    return "系统\n配置";
                case "AlarmLog":
                    return "报警\n日志";
                default:
                    return item.DisplayName;
            }
        }

        private string GetSecondaryText(NavPageDef item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            if (!IsEnglishLanguage(GetCurrentLanguage()))
            {
                return item.DisplayName;
            }

            var pageKey = item.PageKey ?? string.Empty;
            var index = pageKey.LastIndexOf('.');
            return index >= 0 && index < pageKey.Length - 1
                ? pageKey.Substring(index + 1)
                : pageKey;
        }

        private string GetSecondaryDescription(NavPageDef item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return IsEnglishLanguage(GetCurrentLanguage())
                ? item.PageKey
                : item.Description;
        }

        private string ResolveSecondaryIcon(string pageKey)
        {
            string icon;
            return _secondaryIcons.TryGetValue(pageKey ?? string.Empty, out icon)
                ? icon
                : "AppstoreOutlined";
        }

        private static string GetMenuItemTag(MenuSelectEventArgs e)
        {
            if (e == null || e.Value == null || e.Value.Tag == null)
            {
                return string.Empty;
            }

            return e.Value.Tag.ToString();
        }

        private Dictionary<string, string> CreateSecondaryIconMap()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Home.Overview", "HomeOutlined" },
                { "Home.SysStatus", "MonitorOutlined" },

                { "Motion.DI", "ApiOutlined" },
                { "Motion.DO", "SendOutlined" },
                { "Motion.Monitor", "DashboardOutlined" },
                { "Motion.Axis", "ControlOutlined" },
                { "Motion.Actuator", "ThunderboltOutlined" },

                { "MotionConfig.Card", "CreditCardOutlined" },
                { "MotionConfig.Axis", "PartitionOutlined" },
                { "MotionConfig.IoMap", "DeploymentUnitOutlined" },
                { "MotionConfig.AxisParam", "SlidersOutlined" },
                { "MotionConfig.Actuator", "ToolOutlined" },

                { "AlarmLog.Current", "AlertOutlined" },
                { "AlarmLog.History", "ProfileOutlined" },
                { "AlarmLog.RunLog", "FileTextOutlined" },

                { "System.User", "UserOutlined" },
                { "System.Permission", "SafetyCertificateOutlined" },
                { "System.LoginLog", "AuditOutlined" },

                { "Production.Order", "ProfileOutlined" },
                { "Production.Recipe", "BookOutlined" },
                { "Production.Data", "BarChartOutlined" },
                { "Production.Report", "LineChartOutlined" },
                { "Production.Trace", "SearchOutlined" },
                { "Production.MesStatus", "CloudServerOutlined" },
                { "Production.UploadLog", "UploadOutlined" },

                { "Vision.Monitor", "EyeOutlined" },
                { "Vision.Result", "CheckCircleOutlined" },
                { "Vision.Calibrate", "AimOutlined" },

                { "PLC.Monitor", "ApiOutlined" },
                { "PLC.Register", "DatabaseOutlined" },
                { "PLC.Status", "WifiOutlined" },
                { "PLC.Write", "EditOutlined" },

                { "Peripheral.Scanner", "QrcodeOutlined" },
                { "Peripheral.ScanTest", "PlayCircleOutlined" },
                { "Peripheral.Sensor", "RadarChartOutlined" },
                { "Peripheral.SensorTrend", "AreaChartOutlined" },

                { "SysConfig.Camera", "CameraOutlined" },
                { "SysConfig.Plc", "ApiOutlined" },
                { "SysConfig.Sensor", "RadarChartOutlined" },
                { "SysConfig.Scanner", "QrcodeOutlined" },
                { "SysConfig.Mes", "CloudOutlined" },
                { "SysConfig.Runtime", "SettingOutlined" },

                { "Engineer.Diagnostic", "ToolOutlined" },
                { "Engineer.RawAxis", "ControlOutlined" },
                { "Engineer.RawPlc", "ApiOutlined" },
                { "Engineer.RawCamera", "CameraOutlined" }
            };
        }

        #endregion

        #region 页面缓存与工作区

        /// <summary>
        /// 导航到当前二级页面。
        /// </summary>
        private void NavigateToSelectedPage()
        {
            var page = _model.SelectedSecondary;
            if (page == null)
            {
                ShowPage(
                    CreatePlaceholderPage(
                        IsEnglishLanguage(GetCurrentLanguage())
                            ? "No accessible page"
                            : "当前用户没有可访问页面"),
                    true);

                labelStatusValue.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "No page available"
                    : "无可用页面";
                return;
            }

            var control = GetOrCreatePage(page.PageKey);
            ShowPage(control, false);

            labelStatusValue.Text = (IsEnglishLanguage(GetCurrentLanguage())
                ? "Current Page: "
                : "当前页面：") + page.PageKey;
        }

        private Control GetOrCreatePage(string pageKey)
        {
            Control page;
            if (_pageCache.TryGetValue(pageKey, out page) && page != null && !page.IsDisposed)
            {
                return page;
            }

            page = CreatePage(pageKey);
            page.Dock = DockStyle.Fill;
            ApplyThemeToControlTree(page);
            _pageCache[pageKey] = page;
            return page;
        }

        private void ShowPage(Control page, bool disposeRemovedControls)
        {
            if (page == null)
            {
                return;
            }

            ApplyThemeToControlTree(page);
            panelContent.SuspendLayout();

            try
            {
                foreach (Control control in panelContent.Controls.OfType<Control>().ToList())
                {
                    if (ReferenceEquals(control, page))
                    {
                        continue;
                    }

                    panelContent.Controls.Remove(control);

                    if (disposeRemovedControls)
                    {
                        control.Dispose();
                    }
                }

                if (!panelContent.Controls.Contains(page))
                {
                    panelContent.Controls.Add(page);
                }

                page.Dock = DockStyle.Fill;
                page.BringToFront();
            }
            finally
            {
                panelContent.ResumeLayout();
            }
        }

        /// <summary>
        /// 页面工厂。
        /// 当前阶段先保留占位页，后续逐页替换。
        /// </summary>
        private Control CreatePage(string pageKey)
        {
            Func<Control> factory;
            if (_pageFactories.TryGetValue(pageKey ?? string.Empty, out factory))
            {
                return factory();
            }

            return CreatePlaceholderPage(
                IsEnglishLanguage(GetCurrentLanguage())
                    ? "Page not implemented:\r\n\r\n" + pageKey
                    : "未实现页面：\r\n\r\n" + pageKey);
        }

        private Dictionary<string, Func<Control>> CreatePageFactories()
        {
            return new Dictionary<string, Func<Control>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Home.Overview", () => CreatePagePlaceholder("首页 / 总览看板", "Home / Overview") },
                { "Home.SysStatus", () => CreatePagePlaceholder("首页 / 系统状态", "Home / System Status") },

                { "Motion.DI", () => CreatePagePlaceholder("设备 / DI 监视\r\n\r\n后续接入 DIMonitorPage", "Motion / DI Monitor\r\n\r\nDIMonitorPage will be connected next.") },
                { "Motion.DO", () => CreatePagePlaceholder("设备 / DO 监视\r\n\r\n后续接入 DOMonitorPage", "Motion / DO Monitor\r\n\r\nDOMonitorPage will be connected next.") },
                { "Motion.Monitor", () => CreatePagePlaceholder("设备 / 多轴总览\r\n\r\n后续接入 MotionMonitorPage", "Motion / Axis Monitor\r\n\r\nMotionMonitorPage will be connected next.") },
                { "Motion.Axis", () => CreatePagePlaceholder("设备 / 轴控制", "Motion / Axis Control") },
                { "Motion.Actuator", () => CreatePagePlaceholder("设备 / 执行器控制", "Motion / Actuator Control") },

                { "MotionConfig.Card", () => CreatePagePlaceholder("运控配置 / 控制卡配置", "Motion Config / Card") },
                { "MotionConfig.Axis", () => CreatePagePlaceholder("运控配置 / 轴拓扑配置", "Motion Config / Axis") },
                { "MotionConfig.IoMap", () => CreatePagePlaceholder("运控配置 / IO 映射配置", "Motion Config / IO Map") },
                { "MotionConfig.AxisParam", () => CreatePagePlaceholder("运控配置 / 轴运行参数", "Motion Config / Axis Parameters") },
                { "MotionConfig.Actuator", () => CreatePagePlaceholder("运控配置 / 执行器配置", "Motion Config / Actuator") },

                { "AlarmLog.Current", () => CreatePagePlaceholder("报警与日志 / 当前报警", "Alarm / Current") },
                { "AlarmLog.History", () => CreatePagePlaceholder("报警与日志 / 报警历史", "Alarm / History") },
                { "AlarmLog.RunLog", () => CreatePagePlaceholder("报警与日志 / 运行日志", "Alarm / Run Log") },

                { "System.User", () => CreatePagePlaceholder("系统 / 用户管理", "System / User") },
                { "System.Permission", () => CreatePagePlaceholder("系统 / 权限分配", "System / Permission") },
                { "System.LoginLog", () => CreatePagePlaceholder("系统 / 登录日志", "System / Login Log") }
            };
        }

        private Control CreatePagePlaceholder(string zhText, string enText)
        {
            return CreatePlaceholderPage(IsEnglishLanguage(GetCurrentLanguage()) ? enText : zhText);
        }

        /// <summary>
        /// 占位页只承担壳层联调作用。
        /// </summary>
        private Control CreatePlaceholderPage(string text)
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Back = _isDarkMode ? Color.FromArgb(32, 32, 32) : Color.White;
            panel.Radius = 0;

            var label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            label.ForeColor = _isDarkMode ? Color.FromArgb(228, 228, 228) : Color.DimGray;
            label.Text = text;

            panel.Controls.Add(label);
            return panel;
        }

        private void RecreateAllCachedPages()
        {
            foreach (var pair in _pageCache.ToList())
            {
                if (pair.Value != null && !pair.Value.IsDisposed)
                {
                    pair.Value.Dispose();
                }
            }

            _pageCache.Clear();
        }

        #endregion

        #region 用户菜单

        private void RefreshUserMenuControl()
        {
            userAvatarMenuControl.SetUserInfo(
                _model.CurrentUserDisplayName,
                GetCurrentRoleDisplayName());

            userAvatarMenuControl.ApplyLanguage(GetCurrentLanguage());
            userAvatarMenuControl.ApplyTheme(_isDarkMode);
        }

        private void UserAvatarMenuControl_SwitchUserRequested(object sender, EventArgs e)
        {
            ReopenMainWindowByLogin(false, false);
        }

        private void UserAvatarMenuControl_ChangePasswordRequested(object sender, EventArgs e)
        {
            ShowChangePasswordPlaceholder();
        }

        private void UserAvatarMenuControl_LogoutRequested(object sender, EventArgs e)
        {
            ReopenMainWindowByLogin(true, true);
        }

        /// <summary>
        /// 统一处理切换用户/退出登录后的重新登录流程。
        /// </summary>
        private void ReopenMainWindowByLogin(bool signOutFirst, bool closeCurrentWindowWhenLoginCanceled)
        {
            if (signOutFirst)
            {
                UserContext.Instance.SignOut();
            }

            using (var loginForm = new LoginForm())
            {
                var loginResult = loginForm.ShowDialog();
                if (loginResult == DialogResult.OK && UserContext.Instance.IsLoggedIn)
                {
                    var newMainWindow = new MainWindow();
                    newMainWindow.Show();
                    Close();
                    return;
                }
            }

            if (closeCurrentWindowWhenLoginCanceled)
            {
                Close();
            }
        }

        private void ShowChangePasswordPlaceholder()
        {
            MessageBox.Show(
                IsEnglishLanguage(GetCurrentLanguage())
                    ? "Change password dialog will be added next."
                    : "修改密码弹窗下一步接入。",
                IsEnglishLanguage(GetCurrentLanguage()) ? "Info" : "提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion

        #region 语言

        private void DropdownTranslate_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (_isUpdatingUiState)
            {
                return;
            }

            var value = e == null || e.Value == null
                ? string.Empty
                : e.Value.ToString();

            var language = string.Equals(value, "English", StringComparison.OrdinalIgnoreCase)
                ? "en-US"
                : "zh-CN";

            ApplyLanguage(language, true);
        }

        /// <summary>
        /// 应用语言到壳层。
        /// </summary>
        private void ApplyLanguage(string language, bool saveToConfig)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = "zh-CN";
            }

            ConfigContext.Instance.Config.Setting.Language = language;

            try
            {
                Localization.SetLanguage(language);
            }
            catch
            {
            }

            titlebar.Text = IsEnglishLanguage(language) ? "AM Motion Control" : "AM运动控制";
            titlebar.SubText = IsEnglishLanguage(language) ? "Version 0.0.1" : "版本 0.0.1";
            labelStatusCaption.Text = IsEnglishLanguage(language) ? "System Status:" : "系统状态：";

            RecreateAllCachedPages();
            RefreshShell();

            if (saveToConfig)
            {
                Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
            }
        }

        #endregion

        #region 主题

        private void ButtonColorMode_Click(object sender, EventArgs e)
        {
            ApplyTheme(!_isDarkMode, true);
        }

        /// <summary>
        /// 应用主题。
        /// </summary>
        private void ApplyTheme(bool isDarkMode, bool saveToConfig)
        {
            _isDarkMode = isDarkMode;
            buttonColorMode.Toggle = isDarkMode;
            ConfigContext.Instance.Config.Setting.Theme = isDarkMode ? "SkinDark" : "SkinDefault";

            if (isDarkMode)
            {
                AntdUI.Config.IsDark = true;
                BackColor = Color.FromArgb(31, 31, 31);
                ForeColor = Color.White;
            }
            else
            {
                AntdUI.Config.IsLight = true;
                BackColor = Color.White;
                ForeColor = Color.Black;
            }

            textureBackgroundMain.SetTheme(isDarkMode);
            userAvatarMenuControl.ApplyTheme(isDarkMode);

            ApplyShellTheme();

            foreach (Control control in panelContent.Controls.OfType<Control>().ToList())
            {
                if (control != null && !control.IsDisposed)
                {
                    ApplyThemeToControlTree(control);
                }
            }

            if (saveToConfig)
            {
                Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
            }
        }

        /// <summary>
        /// 同步壳层卡片颜色。
        /// </summary>
        private void ApplyShellTheme()
        {
            var cardBack = _isDarkMode
                ? Color.FromArgb(39, 39, 39)
                : Color.White;

            var primaryText = _isDarkMode
                ? Color.FromArgb(236, 236, 236)
                : Color.FromArgb(24, 39, 58);

            var secondaryText = _isDarkMode
                ? Color.FromArgb(170, 170, 170)
                : Color.FromArgb(120, 120, 120);

            panelLeftCard.Back = cardBack;
            panelSecondaryNavCard.Back = cardBack;
            panelWorkCard.Back = cardBack;
            panelStatusCard.Back = cardBack;

            panelAvatarHost.Back = Color.Transparent;
            panelWorkHeader.Back = Color.Transparent;
            panelContent.Back = Color.Transparent;

            labelPrimaryTitleValue.ForeColor = primaryText;
            labelPageTitleValue.ForeColor = primaryText;
            labelPageDescriptionValue.ForeColor = secondaryText;
            labelStatusCaption.ForeColor = secondaryText;
            labelStatusValue.ForeColor = primaryText;
        }

        private void SuspendShellLayouts()
        {
            SuspendLayout();
            textureBackgroundMain.SuspendLayout();
            gridMainHost.SuspendLayout();
            panelLeftCard.SuspendLayout();
            panelSecondaryNavCard.SuspendLayout();
            panelWorkCard.SuspendLayout();
            panelWorkHeader.SuspendLayout();
            panelContent.SuspendLayout();
        }

        private void ResumeShellLayouts(bool performLayout)
        {
            panelContent.ResumeLayout(performLayout);
            panelWorkHeader.ResumeLayout(performLayout);
            panelWorkCard.ResumeLayout(performLayout);
            panelSecondaryNavCard.ResumeLayout(performLayout);
            panelLeftCard.ResumeLayout(performLayout);
            gridMainHost.ResumeLayout(performLayout);
            textureBackgroundMain.ResumeLayout(performLayout);
            ResumeLayout(performLayout);
        }

        /// <summary>
        /// 对缓存页中的自定义占位控件做最低限度主题同步。
        /// </summary>
        private void ApplyThemeToControlTree(Control root)
        {
            if (root == null || root.IsDisposed)
            {
                return;
            }

            var antPanel = root as Panel;
            if (antPanel != null)
            {
                antPanel.Back = _isDarkMode ? Color.FromArgb(32, 32, 32) : Color.White;
            }

            var antLabel = root as Label;
            if (antLabel != null)
            {
                antLabel.ForeColor = _isDarkMode
                    ? Color.FromArgb(228, 228, 228)
                    : Color.DimGray;
            }

            foreach (Control child in root.Controls)
            {
                ApplyThemeToControlTree(child);
            }
        }

        #endregion

        #region 辅助方法

        private string GetCurrentLanguage()
        {
            var language = ConfigContext.Instance.Config.Setting.Language;
            return string.IsNullOrWhiteSpace(language) ? "zh-CN" : language;
        }

        private string GetCurrentRoleDisplayName()
        {
            if (IsEnglishLanguage(GetCurrentLanguage()))
            {
                if (UserContext.Instance.IsAdmin || UserContext.Instance.HasRole("Am"))
                {
                    return "Administrator";
                }

                if (UserContext.Instance.HasRole("Engineer"))
                {
                    return "Engineer";
                }

                if (UserContext.Instance.HasRole("Operator"))
                {
                    return "Operator";
                }

                return "User";
            }

            if (UserContext.Instance.IsAdmin || UserContext.Instance.HasRole("Am"))
            {
                return "管理员";
            }

            if (UserContext.Instance.HasRole("Engineer"))
            {
                return "工程师";
            }

            if (UserContext.Instance.HasRole("Operator"))
            {
                return "操作员";
            }

            return "用户";
        }

        private static bool IsEnglishLanguage(string language)
        {
            return !string.IsNullOrWhiteSpace(language) &&
                   language.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsDarkTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                return false;
            }

            return string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region 生命周期

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            RecreateAllCachedPages();
        }

        #endregion
    }
}