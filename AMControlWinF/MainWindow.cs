using AM.Core.Context;
using AM.PageModel.Main;
using AM.PageModel.Navigation;
using AM.Tools;
using AMControlWinF.Views.Auth;
using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// 
    /// 当前职责：
    /// 1. 构建一级/二级导航；
    /// 2. 承载右侧工作区与页面缓存；
    /// 3. 协调用户头像菜单；
    /// 4. 协调语言与主题切换；
    /// 5. 不承载具体业务页面逻辑。
    /// </summary>
    public partial class MainWindow : AntdUI.Window
    {
        private readonly MainWindowModel _model;
        private readonly Dictionary<string, Control> _pageCache;

        private bool _isUpdatingUiState;
        private bool _isDarkMode;

        public MainWindow()
        {
            InitializeComponent();

            _model = new MainWindowModel();
            _pageCache = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);

            BindEvents();
            LoadModel();
            InitializeShellState();
        }

        #region 初始化

        /// <summary>
        /// 统一绑定主窗体事件。
        /// </summary>
        private void BindEvents()
        {
            _model.PropertyChanged += Model_PropertyChanged;

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
        /// 首次加载导航与默认页面。
        /// </summary>
        private void LoadModel()
        {
            _model.LoadNavigation();

            BuildPrimaryMenu();
            BuildSecondaryMenu();

            RefreshHeader();
            RefreshUserMenuControl();
            NavigateToSelectedPage();
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

        #endregion

        #region Model 协调

        /// <summary>
        /// 统一处理页面模型状态变化。
        /// </summary>
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowModel.PrimaryItems))
            {
                BuildPrimaryMenu();
                RefreshHeader();
                return;
            }

            if (e.PropertyName == nameof(MainWindowModel.SecondaryItems))
            {
                BuildSecondaryMenu();
                RefreshHeader();
                return;
            }

            if (e.PropertyName == nameof(MainWindowModel.SelectedPrimary) ||
                e.PropertyName == nameof(MainWindowModel.SelectedSecondary))
            {
                RefreshHeader();
                NavigateToSelectedPage();
                return;
            }

            if (e.PropertyName == nameof(MainWindowModel.CurrentUserDisplayName) ||
                e.PropertyName == nameof(MainWindowModel.CurrentRoleDisplayName))
            {
                RefreshUserMenuControl();
            }
        }

        #endregion

        #region 导航

        /// <summary>
        /// 一级导航为窄栏文本导航。
        /// 不显示图标，只显示短文本；较长文本主动换行。
        /// </summary>
        private void BuildPrimaryMenu()
        {
            menuPrimary.Items.Clear();

            foreach (var item in _model.PrimaryItems)
            {
                menuPrimary.Items.Add(new MenuItem
                {
                    Text = GetPrimaryText(item),
                    Tag = item.Key,
                    
                });
            }
        }

        /// <summary>
        /// 二级导航保留图标，图标按实际页面类型映射。
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
            if (e == null || e.Value == null || e.Value.Tag == null)
            {
                return;
            }

            var key = e.Value.Tag.ToString();
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
            if (e == null || e.Value == null || e.Value.Tag == null)
            {
                return;
            }

            var pageKey = e.Value.Tag.ToString();
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
        /// 第三列是一个大卡片，内部再细分头部与内容区。
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
                labelPrimaryTitleValue.Text = GetPrimaryText(_model.SelectedPrimary).Replace("\n", "");
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

            // 一级导航窄栏只显示文字，不显示图标。
            // 对较长中文名称主动换行，贴近当前 WPF 布局。
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

        private static string ResolveSecondaryIcon(string pageKey)
        {
            switch (pageKey)
            {
                case "Home.Overview": return "HomeOutlined";
                case "Home.SysStatus": return "MonitorOutlined";

                case "Motion.DI": return "ApiOutlined";
                case "Motion.DO": return "SendOutlined";
                case "Motion.Monitor": return "DashboardOutlined";
                case "Motion.Axis": return "ControlOutlined";
                case "Motion.Actuator": return "ThunderboltOutlined";

                case "MotionConfig.Card": return "CreditCardOutlined";
                case "MotionConfig.Axis": return "PartitionOutlined";
                case "MotionConfig.IoMap": return "DeploymentUnitOutlined";
                case "MotionConfig.AxisParam": return "SlidersOutlined";
                case "MotionConfig.Actuator": return "ToolOutlined";

                case "AlarmLog.Current": return "AlertOutlined";
                case "AlarmLog.History": return "ProfileOutlined";
                case "AlarmLog.RunLog": return "FileTextOutlined";

                case "System.User": return "UserOutlined";
                case "System.Permission": return "SafetyCertificateOutlined";
                case "System.LoginLog": return "AuditOutlined";

                case "Production.Order": return "ProfileOutlined";
                case "Production.Recipe": return "BookOutlined";
                case "Production.Data": return "BarChartOutlined";
                case "Production.Report": return "LineChartOutlined";
                case "Production.Trace": return "SearchOutlined";
                case "Production.MesStatus": return "CloudServerOutlined";
                case "Production.UploadLog": return "UploadOutlined";

                case "Vision.Monitor": return "EyeOutlined";
                case "Vision.Result": return "CheckCircleOutlined";
                case "Vision.Calibrate": return "AimOutlined";

                case "PLC.Monitor": return "ApiOutlined";
                case "PLC.Register": return "DatabaseOutlined";
                case "PLC.Status": return "WifiOutlined";
                case "PLC.Write": return "EditOutlined";

                case "Peripheral.Scanner": return "QrcodeOutlined";
                case "Peripheral.ScanTest": return "PlayCircleOutlined";
                case "Peripheral.Sensor": return "RadarChartOutlined";
                case "Peripheral.SensorTrend": return "AreaChartOutlined";

                case "SysConfig.Camera": return "CameraOutlined";
                case "SysConfig.Plc": return "ApiOutlined";
                case "SysConfig.Sensor": return "RadarChartOutlined";
                case "SysConfig.Scanner": return "QrcodeOutlined";
                case "SysConfig.Mes": return "CloudOutlined";
                case "SysConfig.Runtime": return "SettingOutlined";

                case "Engineer.Diagnostic": return "ToolOutlined";
                case "Engineer.RawAxis": return "ControlOutlined";
                case "Engineer.RawPlc": return "ApiOutlined";
                case "Engineer.RawCamera": return "CameraOutlined";

                default: return "AppstoreOutlined";
            }
        }

        #endregion

        #region 页面缓存与工作区

        /// <summary>
        /// 导航到当前二级页面。
        /// 页面使用缓存复用，避免频繁重建。
        /// </summary>
        private void NavigateToSelectedPage()
        {
            var page = _model.SelectedSecondary;
            if (page == null)
            {
                ShowPage(CreatePlaceholderPage(
                    IsEnglishLanguage(GetCurrentLanguage())
                        ? "No accessible page"
                        : "当前用户没有可访问页面"), false);

                labelStatusValue.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "No page available"
                    : "无可用页面";
                return;
            }

            var control = GetOrCreatePage(page.PageKey);
            ShowPage(control, true);
            labelStatusValue.Text = (IsEnglishLanguage(GetCurrentLanguage()) ? "Current Page: " : "当前页面：") + page.PageKey;
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

        private void ShowPage(Control page, bool useCache)
        {
            if (page == null)
            {
                return;
            }

            panelContent.SuspendLayout();

            try
            {
                foreach (Control control in panelContent.Controls.OfType<Control>().ToList())
                {
                    if (!ReferenceEquals(control, page))
                    {
                        panelContent.Controls.Remove(control);

                        if (!useCache)
                        {
                            control.Dispose();
                        }
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
            switch (pageKey)
            {
                case "Home.Overview":
                    return CreatePlaceholderPage("首页 / 总览看板");

                case "Home.SysStatus":
                    return CreatePlaceholderPage("首页 / 系统状态");

                case "Motion.DI":
                    return CreatePlaceholderPage("设备 / DI 监视\r\n\r\n后续接入 DIMonitorPage");

                case "Motion.DO":
                    return CreatePlaceholderPage("设备 / DO 监视\r\n\r\n后续接入 DOMonitorPage");

                case "Motion.Monitor":
                    return CreatePlaceholderPage("设备 / 多轴总览\r\n\r\n后续接入 MotionMonitorPage");

                case "Motion.Axis":
                    return CreatePlaceholderPage("设备 / 轴控制");

                case "Motion.Actuator":
                    return CreatePlaceholderPage("设备 / 执行器控制");

                case "MotionConfig.Card":
                    return CreatePlaceholderPage("运控配置 / 控制卡配置");

                case "MotionConfig.Axis":
                    return CreatePlaceholderPage("运控配置 / 轴拓扑配置");

                case "MotionConfig.IoMap":
                    return CreatePlaceholderPage("运控配置 / IO 映射配置");

                case "MotionConfig.AxisParam":
                    return CreatePlaceholderPage("运控配置 / 轴运行参数");

                case "MotionConfig.Actuator":
                    return CreatePlaceholderPage("运控配置 / 执行器配置");

                case "AlarmLog.Current":
                    return CreatePlaceholderPage("报警与日志 / 当前报警");

                case "AlarmLog.History":
                    return CreatePlaceholderPage("报警与日志 / 报警历史");

                case "AlarmLog.RunLog":
                    return CreatePlaceholderPage("报警与日志 / 运行日志");

                case "System.User":
                    return CreatePlaceholderPage("系统 / 用户管理");

                case "System.Permission":
                    return CreatePlaceholderPage("系统 / 权限分配");

                case "System.LoginLog":
                    return CreatePlaceholderPage("系统 / 登录日志");

                default:
                    return CreatePlaceholderPage("未实现页面：\r\n\r\n" + pageKey);
            }
        }

        /// <summary>
        /// 占位页只承担壳层联调作用。
        /// 颜色跟随当前主题。
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
            var keys = _pageCache.Keys.ToList();
            foreach (var key in keys)
            {
                var control = _pageCache[key];
                if (control != null && !control.IsDisposed)
                {
                    control.Dispose();
                }

                _pageCache.Remove(key);
            }
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
            ShowLoginForSwitchUser();
        }

        private void UserAvatarMenuControl_ChangePasswordRequested(object sender, EventArgs e)
        {
            ShowChangePasswordPlaceholder();
        }

        private void UserAvatarMenuControl_LogoutRequested(object sender, EventArgs e)
        {
            LogoutAndReturnToLogin();
        }

        private void ShowLoginForSwitchUser()
        {
            using (var loginForm = new LoginForm())
            {
                var loginResult = loginForm.ShowDialog();
                if (loginResult == DialogResult.OK && UserContext.Instance.IsLoggedIn)
                {
                    var newMainWindow = new MainWindow();
                    newMainWindow.Show();
                    Close();
                }
            }
        }

        private void LogoutAndReturnToLogin()
        {
            UserContext.Instance.SignOut();

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

            Close();
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

            var value = e == null || e.Value == null ? string.Empty : e.Value.ToString();
            var language = string.Equals(value, "English", StringComparison.OrdinalIgnoreCase)
                ? "en-US"
                : "zh-CN";

            ApplyLanguage(language, true);
        }

        /// <summary>
        /// 应用语言到壳层。
        /// 当前阶段英文导航仍使用 Key / PageKey 简化映射。
        /// </summary>
        private void ApplyLanguage(string language, bool saveToConfig)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = "zh-CN";
            }

            try
            {
                Localization.SetLanguage(language);
            }
            catch
            {
            }

            titlebar.Text = "AM运动控制";
            titlebar.SubText = IsEnglishLanguage(language) ? "Version 0.0.1" : "版本 0.0.1";
            labelStatusCaption.Text = IsEnglishLanguage(language) ? "System Status:" : "系统状态：";

            BuildPrimaryMenu();
            BuildSecondaryMenu();
            RefreshHeader();
            RefreshUserMenuControl();
            RecreateAllCachedPages();
            NavigateToSelectedPage();

            if (saveToConfig)
            {
                ConfigContext.Instance.Config.Setting.Language = language;
                Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
            }
        }

        #endregion

        #region 主题

        private void ButtonColorMode_Click(object sender, EventArgs e)
        {
            _isDarkMode = !_isDarkMode;
            buttonColorMode.Toggle = _isDarkMode;
            ApplyTheme(_isDarkMode, true);
        }

        /// <summary>
        /// 应用主题。
        /// 原则：
        /// 1. 暗色模式优先沿用 AntdUI 默认主题；
        /// 2. 主窗体只负责壳层卡片与自定义占位页同步；
        /// 3. 不强行覆盖 AntdUI 菜单内部 hover/selected 状态色。
        /// </summary>
        private void ApplyTheme(bool isDarkMode, bool saveToConfig)
        {
            _isDarkMode = isDarkMode;

            // AntdUI 全局主题开关
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

            // 自定义背景纹理
            textureBackgroundMain.SetTheme(isDarkMode);

            // 左下头像菜单控件
            userAvatarMenuControl.ApplyTheme(isDarkMode);

            // 主壳层卡片同步
            ApplyShellTheme();

            // 缓存页最小同步
            foreach (var pair in _pageCache)
            {
                if (pair.Value != null && !pair.Value.IsDisposed)
                {
                    ApplyThemeToControlTree(pair.Value);
                }
            }

            if (saveToConfig)
            {
                ConfigContext.Instance.Config.Setting.Theme = isDarkMode ? "SkinDark" : "SkinDefault";
                Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
            }
        }

        /// <summary>
        /// 统一同步主窗体壳层卡片颜色。
        /// 只处理自定义壳层，不接管 AntdUI 控件内部状态色。
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
    }
}