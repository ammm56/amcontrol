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
    /// WinForms 主窗体。
    /// 职责：
    /// 1. 承载壳层布局（标题栏、导航卡片、内容区、状态栏）；
    /// 2. 协调一级/二级导航与页面缓存；
    /// 3. 协调主题、语言、用户菜单；
    /// 4. 不承载具体业务页面逻辑。
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

        /// <summary>
        /// 统一绑定 UI 事件。
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
        /// 首次装载导航和默认页面。
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
        /// 从配置初始化语言、主题等壳层状态。
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
        /// Model 属性变化后的统一协调入口。
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

        /// <summary>
        /// 根据可见一级导航重建左侧主菜单。
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
                    IconSvg = ResolvePrimaryIcon(item.Key)
                });
            }
        }

        /// <summary>
        /// 根据当前选中的一级导航重建二级菜单。
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
                    IconSvg = "AppstoreOutlined"
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
        /// 刷新页面头信息。
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
                labelPrimaryTitleValue.Text = GetPrimaryText(_model.SelectedPrimary);
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

        /// <summary>
        /// 将当前用户信息同步到左下角头像菜单控件。
        /// </summary>
        private void RefreshUserMenuControl()
        {
            userAvatarMenuControl.SetUserInfo(
                _model.CurrentUserDisplayName,
                GetCurrentRoleDisplayName());

            userAvatarMenuControl.ApplyLanguage(GetCurrentLanguage());
            userAvatarMenuControl.ApplyTheme(_isDarkMode);
        }

        /// <summary>
        /// 根据当前二级导航显示页面。
        /// 页面采用缓存复用模式，避免反复销毁重建。
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
        /// 当前阶段先保留占位页，后续逐个替换为真实业务页面。
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
            label.ForeColor = _isDarkMode ? Color.Gainsboro : Color.DimGray;
            label.Text = text;

            panel.Controls.Add(label);
            return panel;
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

        private void ButtonColorMode_Click(object sender, EventArgs e)
        {
            _isDarkMode = !_isDarkMode;
            buttonColorMode.Toggle = _isDarkMode;
            ApplyTheme(_isDarkMode, true);
        }

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
        /// 应用语言到壳层 UI。
        /// 当前阶段导航英文先使用 Key / PageKey 简化映射。
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

            titlebar.Text = "AMControl WinForms";
            titlebar.SubText = IsEnglishLanguage(language) ? "Industrial Control" : "工业设备控制";

            labelPrimaryNavTitle.Text = IsEnglishLanguage(language) ? "Primary Navigation" : "一级导航";
            labelSecondaryNavTitle.Text = IsEnglishLanguage(language) ? "Secondary Navigation" : "二级导航";
            labelPrimaryTitleTitle.Text = IsEnglishLanguage(language) ? "Module" : "当前模块";
            labelPageTitleTitle.Text = IsEnglishLanguage(language) ? "Page" : "当前页面";
            labelPageDescriptionTitle.Text = IsEnglishLanguage(language) ? "Description" : "页面说明";
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

        /// <summary>
        /// 应用主题到壳层与缓存页面。
        /// </summary>
        private void ApplyTheme(bool isDarkMode, bool saveToConfig)
        {
            _isDarkMode = isDarkMode;

            var windowBack = isDarkMode ? Color.FromArgb(24, 24, 24) : Color.FromArgb(245, 247, 250);
            var cardBack = isDarkMode ? Color.FromArgb(36, 36, 36) : Color.White;
            var accentBack = isDarkMode ? Color.FromArgb(42, 42, 42) : Color.FromArgb(250, 250, 250);
            var primaryText = isDarkMode ? Color.Gainsboro : Color.FromArgb(24, 39, 58);
            var secondaryText = isDarkMode ? Color.Silver : Color.Gray;

            BackColor = windowBack;
            layoutRoot.BackColor = windowBack;
            panelStage.BackColor = windowBack;
            layoutShell.BackColor = Color.Transparent;

            textureBackgroundMain.SetTheme(isDarkMode);

            panelLeftCard.Back = cardBack;
            panelPrimaryNav.Back = cardBack;
            panelAvatarHost.Back = cardBack;
            panelSecondaryNavCard.Back = cardBack;
            panelHeaderCard.Back = cardBack;
            panelContentCard.Back = cardBack;
            panelContent.Back = cardBack;
            panelStatusBar.Back = accentBack;

            labelPrimaryNavTitle.ForeColor = primaryText;
            labelSecondaryNavTitle.ForeColor = primaryText;
            labelPrimaryTitleTitle.ForeColor = secondaryText;
            labelPrimaryTitleValue.ForeColor = primaryText;
            labelPageTitleTitle.ForeColor = secondaryText;
            labelPageTitleValue.ForeColor = primaryText;
            labelPageDescriptionTitle.ForeColor = secondaryText;
            labelPageDescriptionValue.ForeColor = secondaryText;
            labelStatusCaption.ForeColor = secondaryText;
            labelStatusValue.ForeColor = primaryText;

            buttonColorMode.Toggle = isDarkMode;

            userAvatarMenuControl.ApplyTheme(isDarkMode);

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
        /// 对缓存页中的 AntdUI 容器/文本进行最低限度主题同步。
        /// </summary>
        private void ApplyThemeToControlTree(Control root)
        {
            if (root == null || root.IsDisposed)
            {
                return;
            }

            var backColor = _isDarkMode ? Color.FromArgb(32, 32, 32) : Color.White;
            var foreColor = _isDarkMode ? Color.Gainsboro : Color.DimGray;

            var antPanel = root as Panel;
            if (antPanel != null)
            {
                antPanel.Back = backColor;
            }

            var antLabel = root as Label;
            if (antLabel != null)
            {
                antLabel.ForeColor = foreColor;
            }

            foreach (Control child in root.Controls)
            {
                ApplyThemeToControlTree(child);
            }
        }

        /// <summary>
        /// 语言切换后重建缓存页面，避免旧页面文本/颜色残留。
        /// </summary>
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

        private string GetPrimaryText(NavPrimaryDef item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return IsEnglishLanguage(GetCurrentLanguage()) ? item.Key : item.DisplayName;
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

            return IsEnglishLanguage(GetCurrentLanguage()) ? item.PageKey : item.Description;
        }

        private static string ResolvePrimaryIcon(string moduleKey)
        {
            switch (moduleKey)
            {
                case "Home": return "HomeOutlined";
                case "Motion": return "DashboardOutlined";
                case "Production": return "DatabaseOutlined";
                case "Vision": return "EyeOutlined";
                case "PLC": return "ApiOutlined";
                case "Peripheral": return "UsbOutlined";
                case "MotionConfig": return "SettingOutlined";
                case "SysConfig": return "ControlOutlined";
                case "Engineer": return "ToolOutlined";
                case "AlarmLog": return "AlertOutlined";
                case "System": return "UserOutlined";
                default: return "AppstoreOutlined";
            }
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
    }
}