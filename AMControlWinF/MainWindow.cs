// AMControlWinF\MainWindow.cs
using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Messaging;
using AM.DBService.Services.Auth;
using AM.DBService.Services.Plc.Runtime;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Interfaces.MotionCard;
using AM.Model.Runtime;
using AM.PageModel.Main;
using AM.PageModel.Navigation;
using AM.Tools;
using AMControlWinF.Tools;
using AMControlWinF.Views.AlarmLog;
using AMControlWinF.Views.Am;
using AMControlWinF.Views.Assembly;
using AMControlWinF.Views.Auth;
using AMControlWinF.Views.Main;
using AMControlWinF.Views.Motion;
using AMControlWinF.Views.MotionConfig;
using AMControlWinF.Views.Plc;
using AMControlWinF.Views.SysConfig;
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
    /// 3. 协调用户头像菜单（切换用户 / 退出登录）；
    /// 4. 协调语言与主题切换；
    /// 5. 订阅系统消息并更新底部状态栏；
    /// 6. 汇总 Motion / PLC 连接状态；
    /// 7. 打开活动报警抽屉面板。
    /// 
    /// 主题切换策略（与 LoginForm 一致）：
    /// - AppThemeHelper.Apply() → AntdUI 全局自动跟随；
    /// - TextureBackground.SetTheme() → 自定义纹理背景同步。
    /// </summary>
    public partial class MainWindow : AntdUI.Window
    {
        private readonly MainWindowModel _model;
        private readonly Dictionary<string, Control> _pageCache;
        private readonly Dictionary<string, Func<Control>> _pageFactories;
        private readonly Dictionary<string, string> _secondaryIcons;
        private readonly PlcRuntimeQueryService _plcRuntimeQueryService;
        private readonly Timer _statusIndicatorTimer;

        private AlarmManager _alarmManager;
        private bool _isDarkMode;
        private bool _isUpdatingUiState;
        private int _activeAlarmCount;
        private SystemMessageType _lastStatusMessageType = SystemMessageType.Status;

        /// <summary>
        /// 关闭原因，供 Program.cs 主循环读取。
        /// 默认为 Exit（正常关闭 → 退出程序）。
        /// </summary>
        public MainWindowExitReason ExitReason { get; private set; }

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
            _plcRuntimeQueryService = new PlcRuntimeQueryService();
            _statusIndicatorTimer = new Timer();
            _statusIndicatorTimer.Interval = 1000;

            BindEvents();
            InitializeShellState();
            LoadModel();
        }

        #region 初始化

        private void BindEvents()
        {
            menuPrimary.SelectChanged += MenuPrimary_SelectChanged;
            menuSecondary.SelectChanged += MenuSecondary_SelectChanged;

            userAvatarMenuControl.SwitchUserRequested += UserAvatarMenuControl_SwitchUserRequested;
            userAvatarMenuControl.ChangePasswordRequested += UserAvatarMenuControl_ChangePasswordRequested;
            userAvatarMenuControl.LogoutRequested += UserAvatarMenuControl_LogoutRequested;

            buttonColorMode.Click += ButtonColorMode_Click;
            dropdownTranslate.SelectedValueChanged += DropdownTranslate_SelectedValueChanged;
            buttonAlarmIndicator.Click += ButtonAlarmIndicator_Click;
            _statusIndicatorTimer.Tick += StatusIndicatorTimer_Tick;
            FormClosed += MainWindow_FormClosed;
        }

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

        private void LoadModel()
        {
            _model.LoadNavigation();
            RefreshShell();
            SubscribeSystemMessages();
            BindAlarmManager();
            SetDefaultStatusMessage();
            RefreshBottomIndicators();
            StartStatusIndicatorTimer();
        }

        #endregion

        #region 壳层刷新

        private void RefreshShell()
        {
            SuspendLayout();
            panelLeftCard.SuspendLayout();
            panelSecondaryNavCard.SuspendLayout();
            panelContent.SuspendLayout();
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
                panelContent.ResumeLayout(true);
                panelSecondaryNavCard.ResumeLayout(true);
                panelLeftCard.ResumeLayout(true);
                ResumeLayout(true);
            }
        }

        private void RefreshHeader()
        {
            labelPrimaryTitleValue.Text = _model.SelectedPrimary == null
                ? (IsEnglishLanguage(GetCurrentLanguage()) ? "No module available" : "无可用模块")
                : GetPrimaryText(_model.SelectedPrimary).Replace("\n", string.Empty);
        }

        private void RefreshUserMenuControl()
        {
            userAvatarMenuControl.SetUserInfo(_model.CurrentUserDisplayName, GetCurrentRoleDisplayName());
            userAvatarMenuControl.ApplyLanguage(GetCurrentLanguage());
        }

        #endregion

        #region 导航

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
                return;

            var selected = _model.PrimaryItems.FirstOrDefault(x =>
                string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
                return;

            if (_model.SelectedPrimary != null &&
                string.Equals(_model.SelectedPrimary.Key, selected.Key, StringComparison.OrdinalIgnoreCase))
                return;

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
                return;

            var selected = _model.SecondaryItems.FirstOrDefault(x =>
                string.Equals(x.PageKey, pageKey, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
                return;

            if (_model.SelectedSecondary != null &&
                string.Equals(_model.SelectedSecondary.PageKey, selected.PageKey, StringComparison.OrdinalIgnoreCase))
                return;

            _model.SelectedSecondary = selected;
            RefreshHeader();
            NavigateToSelectedPage();
        }

        public void NavigateToPage(string pageKey)
        {
            if (string.IsNullOrWhiteSpace(pageKey))
                return;

            var targetPage = NavigationCatalog.All.FirstOrDefault(x =>
                string.Equals(x.PageKey, pageKey, StringComparison.OrdinalIgnoreCase));
            if (targetPage == null)
                return;

            var targetPrimary = _model.PrimaryItems.FirstOrDefault(x =>
                string.Equals(x.Key, targetPage.ModuleKey, StringComparison.OrdinalIgnoreCase));
            if (targetPrimary == null)
                return;

            _model.SelectedPrimary = targetPrimary;
            _model.LoadSecondaryItems(targetPrimary.Key);

            var targetSecondary = _model.SecondaryItems.FirstOrDefault(x =>
                string.Equals(x.PageKey, pageKey, StringComparison.OrdinalIgnoreCase));
            if (targetSecondary == null)
                return;

            _model.SelectedSecondary = targetSecondary;
            RefreshShell();
        }

        public void RefreshNavigationByCurrentUser()
        {
            _model.LoadNavigation();
            RefreshShell();
        }

        private string GetPrimaryText(NavPrimaryDef item)
        {
            if (item == null)
                return string.Empty;

            if (IsEnglishLanguage(GetCurrentLanguage()))
                return item.Key;

            switch (item.Key)
            {
                case "MotionConfig": return "运控\n配置";
                case "SysConfig": return "系统\n配置";
                case "AlarmLog": return "报警\n日志";
                default: return item.DisplayName;
            }
        }

        private string GetSecondaryText(NavPageDef item)
        {
            if (item == null)
                return string.Empty;

            if (!IsEnglishLanguage(GetCurrentLanguage()))
                return item.DisplayName;

            var pageKey = item.PageKey ?? string.Empty;
            var index = pageKey.LastIndexOf('.');
            return index >= 0 && index < pageKey.Length - 1
                ? pageKey.Substring(index + 1)
                : pageKey;
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
                return string.Empty;

            return e.Value.Tag.ToString();
        }

        private Dictionary<string, string> CreateSecondaryIconMap()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Home.Overview",          "HomeOutlined" },
                { "Home.SysStatus",         "MonitorOutlined" },

                { "Motion.DI",              "ApiOutlined" },
                { "Motion.DO",              "SendOutlined" },
                { "Motion.Monitor",         "DashboardOutlined" },
                { "Motion.Axis",            "ControlOutlined" },
                { "Motion.Actuator",        "ThunderboltOutlined" },

                { "Assembly.Wiring",       "DeploymentUnitOutlined" },

                { "MotionConfig.Card",      "CreditCardOutlined" },
                { "MotionConfig.Axis",      "PartitionOutlined" },
                { "MotionConfig.IoMap",     "DeploymentUnitOutlined" },
                { "MotionConfig.AxisParam", "SlidersOutlined" },
                { "MotionConfig.Actuator",  "ToolOutlined" },

                { "AlarmLog.Current",       "AlertOutlined" },
                { "AlarmLog.History",       "ProfileOutlined" },
                { "AlarmLog.RunLog",        "FileTextOutlined" },

                { "System.User",            "UserOutlined" },
                { "System.Permission",      "SafetyCertificateOutlined" },
                { "System.LoginLog",        "AuditOutlined" },

                { "Production.Order",       "ProfileOutlined" },
                { "Production.Recipe",      "BookOutlined" },
                { "Production.Data",        "BarChartOutlined" },
                { "Production.Report",      "LineChartOutlined" },
                { "Production.Trace",       "SearchOutlined" },
                { "Production.MesStatus",   "CloudServerOutlined" },
                { "Production.UploadLog",   "UploadOutlined" },

                { "Vision.Monitor",         "EyeOutlined" },
                { "Vision.Result",          "CheckCircleOutlined" },
                { "Vision.Calibrate",       "AimOutlined" },

                { "PLC.Monitor",            "ApiOutlined" },
                { "PLC.Status",             "WifiOutlined" },
                { "PLC.Debug",              "EditOutlined" },

                { "Peripheral.Scanner",     "QrcodeOutlined" },
                { "Peripheral.ScanTest",    "PlayCircleOutlined" },
                { "Peripheral.Sensor",      "RadarChartOutlined" },
                { "Peripheral.SensorTrend", "AreaChartOutlined" },

                { "SysConfig.Camera",       "CameraOutlined" },
                { "SysConfig.Plc",          "ApiOutlined" },
                { "SysConfig.Sensor",       "RadarChartOutlined" },
                { "SysConfig.Scanner",      "QrcodeOutlined" },
                { "SysConfig.Mes",          "CloudOutlined" },
                { "SysConfig.Runtime",      "SettingOutlined" },

                { "Engineer.Diagnostic",    "ToolOutlined" },
                { "Engineer.RawAxis",       "ControlOutlined" },
                { "Engineer.RawPlc",        "ApiOutlined" },
                { "Engineer.RawCamera",     "CameraOutlined" }
            };
        }

        #endregion

        #region 页面缓存

        private void NavigateToSelectedPage()
        {
            var page = _model.SelectedSecondary;
            if (page == null)
            {
                ShowPage(CreatePlaceholderPage(
                    IsEnglishLanguage(GetCurrentLanguage())
                        ? "No accessible page"
                        : "当前用户没有可访问页面"), true);
                return;
            }

            ShowPage(GetOrCreatePage(page.PageKey), false);
        }

        private Control GetOrCreatePage(string pageKey)
        {
            Control page;
            if (_pageCache.TryGetValue(pageKey, out page) && page != null && !page.IsDisposed)
                return page;

            page = CreatePage(pageKey);
            page.Dock = DockStyle.Fill;
            _pageCache[pageKey] = page;

            return page;
        }

        private void ShowPage(Control page, bool disposeRemovedControls)
        {
            if (page == null)
                return;

            panelContent.SuspendLayout();
            try
            {
                var removedControls = new List<Control>();

                foreach (Control control in panelContent.Controls.OfType<Control>().ToList())
                {
                    if (ReferenceEquals(control, page))
                        continue;

                    panelContent.Controls.Remove(control);

                    if (disposeRemovedControls)
                        removedControls.Add(control);
                }

                if (!panelContent.Controls.Contains(page))
                    panelContent.Controls.Add(page);

                page.Dock = DockStyle.Fill;
                page.BringToFront();

                if (disposeRemovedControls && removedControls.Count > 0)
                    ControlDisposeHelper.DisposeControlsDeferred(this, removedControls);
            }
            finally
            {
                panelContent.ResumeLayout();
            }
        }

        private Control CreatePage(string pageKey)
        {
            Func<Control> factory;
            if (_pageFactories.TryGetValue(pageKey ?? string.Empty, out factory))
                return factory();

            return CreatePlaceholderPage(
                IsEnglishLanguage(GetCurrentLanguage())
                    ? "Page not implemented:\r\n\r\n" + pageKey
                    : "未实现页面：\r\n\r\n" + pageKey);
        }

        private Dictionary<string, Func<Control>> CreatePageFactories()
        {
            return new Dictionary<string, Func<Control>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Home.Overview",          () => CreatePlaceholderPage("首页 / 总览看板") },
                { "Home.SysStatus",         () => CreatePlaceholderPage("首页 / 系统状态") },

                { "Motion.DI",              () => new DIMotionPage() },
                { "Motion.DO",              () => new DOMotionPage() },
                { "Motion.Monitor",         () => new MotionMonitorPage() },
                { "Motion.Axis",            () => new MotionAxisPage() },
                { "Motion.Actuator",        () => new MotionActuatorPage() },

                { "Assembly.Wiring",        () => new AssemblyWiringPage() },

                { "MotionConfig.Card",      () => new MotionCardManagementPage() },
                { "MotionConfig.Axis",      () => new MotionAxisManagementPage() },
                { "MotionConfig.IoMap",     () => new MotionIoMapManagementPage() },
                { "MotionConfig.AxisParam", () => new MotionAxisParamManagementPage() },
                { "MotionConfig.Actuator",  () => new ActuatorManagementPage() },

                { "PLC.Status",             () => new PlcStatusPage() },
                { "PLC.Monitor",            () => new PlcMonitorPage() },
                { "PLC.Debug",              () => new PlcDebugPage() },

                { "SysConfig.Plc",          () => new PlcConfigManagementPage() },

                { "AlarmLog.Current",       () => new CurrentAlarmPage() },
                { "AlarmLog.History",       () => new AlarmHistoryPage() },
                { "AlarmLog.RunLog",        () => new RunLogPage() },

                { "System.User",            () => new UserManagementPage() },
                { "System.Permission",      () => new UserPermissionPage() },
                { "System.LoginLog",        () => new LoginLogPage() }
            };
        }

        private static Control CreatePlaceholderPage(string text)
        {
            var panel = new Panel { Dock = DockStyle.Fill, Radius = 0 };
            var label = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold),
                Text = text
            };
            panel.Controls.Add(label);
            return panel;
        }

        private void DisposeAllCachedPages()
        {
            var controls = _pageCache.Values
                .Where(x => x != null && !x.IsDisposed)
                .Distinct()
                .ToList();

            foreach (var control in controls)
            {
                try
                {
                    if (control.Parent != null)
                        control.Parent.Controls.Remove(control);
                }
                catch
                {
                }
            }

            _pageCache.Clear();

            if (controls.Count > 0)
                ControlDisposeHelper.DisposeControlsDeferred(this, controls);
        }

        #endregion

        #region 系统消息

        private void SubscribeSystemMessages()
        {
            var bus = SystemContext.Instance.MessageBus;
            if (bus == null)
                return;

            bus.Unsubscribe(this);
            bus.Subscribe(this, OnSystemMessageReceived);
        }

        private void OnSystemMessageReceived(SystemMessage message)
        {
            if (message == null || IsDisposed)
                return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action(() => OnSystemMessageReceived(message))); }
                catch { }
                return;
            }

            UpdateStatusMessage(message);
            ShowMessageNotification(message);
        }

        private void SetDefaultStatusMessage()
        {
            _lastStatusMessageType = SystemMessageType.Status;
            labelStatusValue.Text = IsEnglishLanguage(GetCurrentLanguage())
                ? "Application ready."
                : "应用已启动，等待操作。";
            labelStatusValue.ForeColor = GetStatusTextColor(_lastStatusMessageType);
        }

        private void UpdateStatusMessage(SystemMessage message)
        {
            if (message == null)
                return;

            _lastStatusMessageType = message.Type;
            labelStatusValue.Text = BuildStatusText(message);
            labelStatusValue.ForeColor = GetStatusTextColor(message.Type);
        }

        private void ShowMessageNotification(SystemMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Message) || !Visible)
                return;

            AntdUI.Message.open(new AntdUI.Message.Config(
                this,
                BuildNotificationText(message),
                GetNoticeType(message.Type))
            {
                Align = TAlignFrom.BL,
                AutoClose = GetNoticeAutoClose(message.Type)
            });
        }

        private static string BuildNotificationText(SystemMessage message)
        {
            var prefix = string.IsNullOrWhiteSpace(message.Source) ? string.Empty : "[" + message.Source + "] ";
            var code = string.IsNullOrWhiteSpace(message.Code) ? string.Empty : " [" + message.Code + "]";
            return prefix + message.Message + code;
        }

        private static string BuildStatusText(SystemMessage message)
        {
            var prefix = string.IsNullOrWhiteSpace(message.Source) ? string.Empty : "[" + message.Source + "] ";
            var code = string.IsNullOrWhiteSpace(message.Code) ? string.Empty : " [" + message.Code + "]";
            return "[" + message.Time.ToString("yyyy-MM-dd HH:mm:ss") + "]  " + prefix + message.Message + code;
        }

        /// <summary>
        /// 状态栏语义色：报错/报警为红，警告为橙，其余跟随明/暗主题中性色。
        /// </summary>
        private Color GetStatusTextColor(SystemMessageType type)
        {
            switch (type)
            {
                case SystemMessageType.Warning:
                    return Color.FromArgb(230, 145, 56);
                case SystemMessageType.Error:
                case SystemMessageType.Alarm:
                    return Color.FromArgb(220, 84, 84);
                default:
                    return _isDarkMode ? Color.FromArgb(228, 228, 228) : Color.FromArgb(90, 90, 90);
            }
        }

        private static TType GetNoticeType(SystemMessageType type)
        {
            switch (type)
            {
                case SystemMessageType.Warning:
                case SystemMessageType.Alarm:
                    return TType.Warn;
                case SystemMessageType.Error:
                    return TType.Error;
                default:
                    return TType.Info;
            }
        }

        private static int GetNoticeAutoClose(SystemMessageType type)
        {
            switch (type)
            {
                case SystemMessageType.Error:
                case SystemMessageType.Alarm:
                    return 6;
                case SystemMessageType.Warning:
                    return 5;
                default:
                    return 3;
            }
        }

        #endregion

        #region 底部状态栏与报警抽屉

        private enum BottomIndicatorState
        {
            Unknown,
            Normal,
            Warning,
            Error,
            Inactive
        }

        private void BindAlarmManager()
        {
            UnbindAlarmManager();

            _alarmManager = SystemContext.Instance.AlarmManager;
            if (_alarmManager == null)
            {
                _activeAlarmCount = 0;
                RefreshAlarmIndicator();
                return;
            }

            _alarmManager.AlarmStateChanged += AlarmManager_AlarmStateChanged;
            RefreshAlarmIndicator();
        }

        private void UnbindAlarmManager()
        {
            if (_alarmManager != null)
            {
                _alarmManager.AlarmStateChanged -= AlarmManager_AlarmStateChanged;
                _alarmManager = null;
            }
        }

        private void AlarmManager_AlarmStateChanged()
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(RefreshAlarmIndicator));
                }
                catch
                {
                }

                return;
            }

            RefreshAlarmIndicator();
        }

        private void StartStatusIndicatorTimer()
        {
            if (!_statusIndicatorTimer.Enabled)
            {
                _statusIndicatorTimer.Start();
            }
        }

        private void StopStatusIndicatorTimer()
        {
            if (_statusIndicatorTimer.Enabled)
            {
                _statusIndicatorTimer.Stop();
            }
        }

        private void StatusIndicatorTimer_Tick(object sender, EventArgs e)
        {
            if (IsDisposed || !Visible)
                return;

            RefreshConnectionIndicators();
        }

        private void RefreshBottomIndicators()
        {
            RefreshConnectionIndicators();
            RefreshAlarmIndicator();
        }

        private void RefreshConnectionIndicators()
        {
            RefreshMotionIndicator();
            RefreshPlcIndicator();
        }

        /// <summary>
        /// 汇总所有运动控制卡真实连接状态。
        /// 状态来源统一通过 IMotionCardConnection.IsConnected() 读取，
        /// 不直接依赖具体驱动实现类的私有字段。
        /// </summary>
        private void RefreshMotionIndicator()
        {
            IList<IMotionCardService> cards = MachineContext.Instance.MotionCards == null
                ? new List<IMotionCardService>()
                : MachineContext.Instance.MotionCards.Values
                    .Where(x => x != null)
                    .Distinct()
                    .ToList();

            if (cards.Count <= 0)
            {
                labelMotionStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "Motion: None"
                    : "Motion: 未配置";
                ApplyIndicatorStyle(labelMotionStatus, BottomIndicatorState.Inactive);
                return;
            }

            int connectedCount = 0;

            foreach (IMotionCardService card in cards)
            {
                try
                {
                    Result<bool> result = card.IsConnected();
                    if (result != null && result.Success && result.Item)
                    {
                        connectedCount++;
                    }
                }
                catch
                {
                }
            }

            int totalCount = cards.Count;

            if (connectedCount <= 0)
            {
                labelMotionStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "Motion: Offline"
                    : "Motion: 未连接";
                ApplyIndicatorStyle(labelMotionStatus, BottomIndicatorState.Error);
                return;
            }

            if (connectedCount >= totalCount)
            {
                labelMotionStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "Motion: Connected"
                    : "Motion: 已连接";
                ApplyIndicatorStyle(labelMotionStatus, BottomIndicatorState.Normal);
                return;
            }

            labelMotionStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                ? string.Format("Motion: {0}/{1}", connectedCount, totalCount)
                : string.Format("Motion: {0}/{1}", connectedCount, totalCount);
            ApplyIndicatorStyle(labelMotionStatus, BottomIndicatorState.Warning);
        }

        private void RefreshPlcIndicator()
        {
            try
            {
                Result<PlcStationRuntimeSnapshot> result = _plcRuntimeQueryService.QueryAllStations();
                if (!result.Success)
                {
                    labelPlcStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                        ? "PLC: Unknown"
                        : "PLC: 状态未知";
                    ApplyIndicatorStyle(labelPlcStatus, BottomIndicatorState.Unknown);
                    return;
                }

                List<PlcStationRuntimeSnapshot> stations = result.Items == null
                    ? new List<PlcStationRuntimeSnapshot>()
                    : result.Items
                        .Where(p => p != null && p.IsEnabled)
                        .ToList();

                if (stations.Count == 0 && result.Items != null)
                {
                    stations = result.Items.Where(p => p != null).ToList();
                }

                if (stations.Count == 0)
                {
                    labelPlcStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                        ? "PLC: None"
                        : "PLC: 未配置";
                    ApplyIndicatorStyle(labelPlcStatus, BottomIndicatorState.Inactive);
                    return;
                }

                int connectedCount = stations.Count(p => p.IsConnected);
                int totalCount = stations.Count;

                if (connectedCount <= 0)
                {
                    labelPlcStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                        ? "PLC: Offline"
                        : "PLC: 未连接";
                    ApplyIndicatorStyle(labelPlcStatus, BottomIndicatorState.Error);
                    return;
                }

                if (connectedCount >= totalCount)
                {
                    labelPlcStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                        ? "PLC: Connected"
                        : "PLC: 已连接";
                    ApplyIndicatorStyle(labelPlcStatus, BottomIndicatorState.Normal);
                    return;
                }

                labelPlcStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? string.Format("PLC: {0}/{1}", connectedCount, totalCount)
                    : string.Format("PLC: {0}/{1}", connectedCount, totalCount);
                ApplyIndicatorStyle(labelPlcStatus, BottomIndicatorState.Warning);
            }
            catch
            {
                labelPlcStatus.Text = IsEnglishLanguage(GetCurrentLanguage())
                    ? "PLC: Unknown"
                    : "PLC: 状态未知";
                ApplyIndicatorStyle(labelPlcStatus, BottomIndicatorState.Unknown);
            }
        }

        private void RefreshAlarmIndicator()
        {
            List<AlarmInfo> alarms = _alarmManager == null
                ? new List<AlarmInfo>()
                : _alarmManager.GetActiveAlarms();

            _activeAlarmCount = alarms == null ? 0 : alarms.Count;

            buttonAlarmIndicator.Text = IsEnglishLanguage(GetCurrentLanguage())
                ? "Alarm: " + _activeAlarmCount
                : "报警: " + _activeAlarmCount;

            ApplyAlarmButtonStyle(_activeAlarmCount);
        }

        private void ApplyIndicatorStyle(Label label, BottomIndicatorState state)
        {
            if (label == null)
                return;

            switch (state)
            {
                case BottomIndicatorState.Normal:
                    label.ForeColor = Color.FromArgb(82, 196, 26);
                    break;
                case BottomIndicatorState.Warning:
                    label.ForeColor = Color.FromArgb(230, 145, 56);
                    break;
                case BottomIndicatorState.Error:
                    label.ForeColor = Color.FromArgb(220, 84, 84);
                    break;
                case BottomIndicatorState.Inactive:
                    label.ForeColor = _isDarkMode
                        ? Color.FromArgb(160, 160, 160)
                        : Color.FromArgb(120, 120, 120);
                    break;
                default:
                    label.ForeColor = _isDarkMode
                        ? Color.FromArgb(228, 228, 228)
                        : Color.FromArgb(90, 90, 90);
                    break;
            }
        }

        private void ApplyAlarmButtonStyle(int count)
        {
            buttonAlarmIndicator.Type = count > 0
                ? TTypeMini.Error
                : TTypeMini.Default;
        }

        private void ButtonAlarmIndicator_Click(object sender, EventArgs e)
        {
            if (_activeAlarmCount <= 0)
                return;

            OpenAlarmDrawer();
        }

        private void OpenAlarmDrawer()
        {
            ActiveAlarmDrawerControl content = new ActiveAlarmDrawerControl();
            content.BindAlarmManager(SystemContext.Instance.AlarmManager);
            content.Size = new Size(760, 620);
            content.RefreshAlarms();

            AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, content)
            {
                Align = TAlignMini.Right,
                Mask = true,
                MaskClosable = true,
                DisplayDelay = 0,
                Dispose = true
            });
        }

        #endregion

        #region 用户菜单

        /// <summary>
        /// 切换用户：在当前主窗体上弹出模态登录框。
        /// - 登录成功：设置 ExitReason 并关闭，Program 主循环会创建新 MainWindow；
        /// - 登录取消：保持当前主窗体不变。
        /// </summary>
        private void UserAvatarMenuControl_SwitchUserRequested(object sender, EventArgs e)
        {
            bool loginOk;
            using (var loginForm = new LoginForm())
            {
                loginOk = loginForm.ShowDialog(this) == DialogResult.OK
                    && UserContext.Instance.IsLoggedIn;
            }

            if (!loginOk)
                return;

            ExitReason = MainWindowExitReason.SwitchUser;
            Close();
        }

        private void UserAvatarMenuControl_ChangePasswordRequested(object sender, EventArgs e)
        {
            var currentUser = UserContext.Instance.CurrentUser;
            if (currentUser == null)
                return;

            using (var resetPwdForm = new ResetUserPasswordDialog())
            {
                resetPwdForm.Text = IsEnglishLanguage(GetCurrentLanguage()) ? "Change Password" : "修改密码";
                resetPwdForm.TargetLoginName = currentUser.LoginName;
                resetPwdForm.TargetDisplayName = currentUser.UserName;

                if (resetPwdForm.ShowDialog(this) != DialogResult.OK)
                    return;

                var authService = new AuthService();
                authService.ResetUserPassword(currentUser.Id, resetPwdForm.NewPassword);
            }
        }

        /// <summary>
        /// 退出登录：直接关闭主窗体，Program 主循环会执行 SignOut + 弹登录框。
        /// </summary>
        private void UserAvatarMenuControl_LogoutRequested(object sender, EventArgs e)
        {
            ExitReason = MainWindowExitReason.Logout;
            Close();
        }

        #endregion

        #region 语言

        private void DropdownTranslate_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (_isUpdatingUiState)
                return;

            var value = e == null || e.Value == null ? string.Empty : e.Value.ToString();
            var language = string.Equals(value, "English", StringComparison.OrdinalIgnoreCase)
                ? "en-US"
                : "zh-CN";

            ApplyLanguage(language, true);
        }

        private void ApplyLanguage(string language, bool saveToConfig)
        {
            language = string.IsNullOrWhiteSpace(language) ? "zh-CN" : language;

            ConfigContext.Instance.Config.Setting.Language = language;

            try { Localization.SetLanguage(language); }
            catch { }

            titlebar.Text = IsEnglishLanguage(language) ? "AM Motion Control" : "AM运动控制";
            titlebar.SubText = IsEnglishLanguage(language) ? "Version 0.0.1" : "版本 0.0.1";

            DisposeAllCachedPages();
            RefreshShell();

            if (string.IsNullOrWhiteSpace(labelStatusValue.Text))
                SetDefaultStatusMessage();

            RefreshBottomIndicators();

            if (saveToConfig)
                AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
        }

        #endregion

        #region 主题

        private void ButtonColorMode_Click(object sender, EventArgs e)
        {
            ApplyTheme(!_isDarkMode, true);
        }

        /// <summary>
        /// 切换明/暗主题（与 LoginForm.ApplyThemeFromConfig 同一策略）。
        /// AntdUI.Panel 带 Shadow 时原生主题渲染已包含正确的卡片背景、边界和阴影，
        /// 完全交由 AntdUI 原生处理。
        /// </summary>
        private void ApplyTheme(bool isDarkMode, bool saveToConfig)
        {
            _isDarkMode = isDarkMode;

            AppThemeHelper.Apply(this, isDarkMode);
            textureBackgroundMain.SetTheme(isDarkMode);
            labelStatusValue.ForeColor = GetStatusTextColor(_lastStatusMessageType);

            buttonColorMode.Toggle = isDarkMode;
            ConfigContext.Instance.Config.Setting.Theme = isDarkMode ? "SkinDark" : "SkinDefault";

            RefreshBottomIndicators();

            if (saveToConfig)
                AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
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
            var isEn = IsEnglishLanguage(GetCurrentLanguage());

            if (UserContext.Instance.IsAdmin || UserContext.Instance.HasRole("Am"))
                return isEn ? "Administrator" : "管理员";

            if (UserContext.Instance.HasRole("Engineer"))
                return isEn ? "Engineer" : "工程师";

            if (UserContext.Instance.HasRole("Operator"))
                return isEn ? "Operator" : "操作员";

            return isEn ? "User" : "用户";
        }

        private static bool IsEnglishLanguage(string language)
        {
            return !string.IsNullOrWhiteSpace(language) &&
                   language.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsDarkTheme(string theme)
        {
            return !string.IsNullOrWhiteSpace(theme) && (
                string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region 生命周期

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopStatusIndicatorTimer();
            UnbindAlarmManager();
            SystemContext.Instance.MessageBus?.Unsubscribe(this);
            DisposeAllCachedPages();

            try
            {
                _statusIndicatorTimer.Dispose();
            }
            catch
            {
            }
        }

        #endregion
    }
}