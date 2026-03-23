using AM.Core.Context;
using AM.Core.Messaging;
using AM.Model.Alarm;
using AM.Model.Auth;
using AMControlWPF.Navigation;
using AMControlWPF.UserControls.Main;
using AMControlWPF.Views.Alarm;
using AMControlWPF.Views.Am;
using AMControlWPF.Views.Auth;
using AMControlWPF.Views.Config;
using AMControlWPF.Views.IO;
using AMControlWPF.Views.Motion;
using AMControlWPF.Views.Template;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControlWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : GlowWindow
    {
        private readonly Dictionary<string, UserControl> _pageCache = new Dictionary<string, UserControl>();
        private readonly List<PrimaryNavItem> _allPrimaryNavItems = new List<PrimaryNavItem>();
        private readonly Dictionary<string, List<SecondaryNavItem>> _allSecondaryNavMap = new Dictionary<string, List<SecondaryNavItem>>();
        private readonly List<PrimaryNavItem> _visiblePrimaryNavItems = new List<PrimaryNavItem>();
        private readonly Dictionary<string, List<SecondaryNavItem>> _visibleSecondaryNavMap = new Dictionary<string, List<SecondaryNavItem>>();
        private bool _isAlarmPanelVisible;

        private const double MainLayoutScaleWhenAlarmVisible = 0.975d;
        private const double MainLayoutOpacityWhenAlarmVisible = 0.92d;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
            InitializeNavigation();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            var nonClientArea = new NonClientAreaContent();
            nonClientArea.NavigateRequested += NonClientArea_NavigateRequested;
            NonClientAreaContent = nonClientArea;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyNavigationByUser();
            InitializeCurrentUserCard();

            SeedDebugAlarmsOnStartup();
            SubscribeSystemMessages();

            ActiveAlarmPanelControl.BindAlarmManager(SystemContext.Instance.AlarmManager);
            ActiveAlarmPanelControl.AlarmCountChanged += ActiveAlarmPanelControl_AlarmCountChanged;
            RefreshAlarmPanel();
            SetAlarmPanelVisible(ActiveAlarmPanelControl.ActiveAlarmCount > 0);

            PrimaryNavList.ItemsSource = _visiblePrimaryNavItems;

            if (_visiblePrimaryNavItems.Count > 0)
            {
                PrimaryNavList.SelectedIndex = 0;
            }
            else
            {
                TextBlockPrimaryTitle.Text = "无可用模块";
                TextBlockWorkAreaTitle.Text = "无可用模块";
                TextBlockWorkAreaDescription.Text = "当前用户没有可访问的页面";
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            SystemContext.Instance.MessageBus?.Unsubscribe(this);
            ActiveAlarmPanelControl.AlarmCountChanged -= ActiveAlarmPanelControl_AlarmCountChanged;
        }

        private void SubscribeSystemMessages()
        {
            SystemContext.Instance.MessageBus?.Unsubscribe(this);
            SystemContext.Instance.MessageBus?.Subscribe(this, OnSystemMessageReceived);
        }


        private void OnSystemMessageReceived(SystemMessage message)
        {
            if (message == null)
            {
                return;
            }

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() => OnSystemMessageReceived(message)));
                return;
            }

            UpdateStatusMessage(message);
            ShowMessageNotification(message);

            if (message.Type == SystemMessageType.Alarm && !_isAlarmPanelVisible)
            {
                SetAlarmPanelVisible(true);
            }
        }
        private void ShowMessageNotification(SystemMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Message))
            {
                return;
            }

            switch (message.Type)
            {
                case SystemMessageType.Info:
                case SystemMessageType.Status:
                    Growl.Info(message.Message);
                    break;
                case SystemMessageType.Warning:
                    Growl.Warning(message.Message);
                    break;
                case SystemMessageType.Error:
                    Growl.Error(message.Message);
                    break;
                case SystemMessageType.Alarm:
                    Growl.Warning(message.Message);
                    break;
            }
        }

        private void UpdateStatusMessage(SystemMessage message)
        {
            if (message == null)
            {
                return;
            }

            var prefix = string.IsNullOrWhiteSpace(message.Source) ? string.Empty : "[" + message.Source + "] ";
            TextBlockStatusMessage.Text = prefix + message.Message;

            switch (message.Type)
            {
                case SystemMessageType.Warning:
                    TextBlockStatusMessage.Foreground = FindBrush("WarningBrush", Brushes.DarkOrange);
                    break;
                case SystemMessageType.Error:
                case SystemMessageType.Alarm:
                    TextBlockStatusMessage.Foreground = FindBrush("DangerBrush", Brushes.IndianRed);
                    break;
                default:
                    TextBlockStatusMessage.Foreground = FindBrush("PrimaryTextBrush", Brushes.Black);
                    break;
            }
        }

        private void RefreshAlarmPanel()
        {
            ActiveAlarmPanelControl.RefreshAlarms();
            UpdateAlarmIndicator(ActiveAlarmPanelControl.ActiveAlarmCount);

            if (ActiveAlarmPanelControl.ActiveAlarmCount == 0 && _isAlarmPanelVisible)
            {
                SetAlarmPanelVisible(false);
            }
        }

        private void ActiveAlarmPanelControl_AlarmCountChanged(int count)
        {
            UpdateAlarmIndicator(count);

            if (count == 0 && _isAlarmPanelVisible)
            {
                SetAlarmPanelVisible(false);
            }
        }

        private void UpdateAlarmIndicator(int count)
        {
            ButtonAlarmIndicator.Content = "报警: " + count;

            if (count > 0)
            {
                ButtonAlarmIndicator.Background = FindBrush("DangerBrush", Brushes.IndianRed);
                ButtonAlarmIndicator.Foreground = Brushes.White;
            }
            else
            {
                ButtonAlarmIndicator.Background = FindBrush("SecondaryRegionBrush", Brushes.LightGray);
                ButtonAlarmIndicator.Foreground = FindBrush("PrimaryTextBrush", Brushes.Black);
            }
        }

        private void ButtonAlarmIndicator_OnClick(object sender, RoutedEventArgs e)
        {
            if (ActiveAlarmPanelControl.ActiveAlarmCount == 0)
            {
                return;
            }

            SetAlarmPanelVisible(!_isAlarmPanelVisible);
        }

        private void ButtonCloseAlarmPanel_OnClick(object sender, RoutedEventArgs e)
        {
            SetAlarmPanelVisible(false);
        }

        private void AlarmOverlayMask_OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetAlarmPanelVisible(false);
        }

        private void SetAlarmPanelVisible(bool visible)
        {
            _isAlarmPanelVisible = visible;

            BorderAlarmPanel.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            AlarmOverlayMask.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            MainLayoutScaleTransform.ScaleX = visible ? MainLayoutScaleWhenAlarmVisible : 1d;
            MainLayoutScaleTransform.ScaleY = visible ? MainLayoutScaleWhenAlarmVisible : 1d;
            MainLayoutHost.Opacity = visible ? MainLayoutOpacityWhenAlarmVisible : 1d;
        }

        private Brush FindBrush(string key, Brush fallback)
        {
            var brush = TryFindResource(key) as Brush;
            return brush ?? fallback;
        }

        private void InitializeCurrentUserCard()
        {
            var userName = string.IsNullOrWhiteSpace(UserContext.Instance.UserName)
                ? UserContext.Instance.LoginName
                : UserContext.Instance.UserName;

            var roleName = GetCurrentRoleDisplayName();
            var user = string.IsNullOrWhiteSpace(userName) ? "未登录" : userName;

            TextBlockPopupUserName.Text = user;
            TextBlockPopupUserRole.Text = roleName;

            GravatarCurrentUser.Id = string.IsNullOrWhiteSpace(UserContext.Instance.LoginName)
                ? "guest"
                : UserContext.Instance.LoginName;
        }

        private string GetCurrentRoleDisplayName()
        {
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

        private void InitializeNavigation()
        {
            // 主导航：从 NavigationCatalog 按首次出现顺序生成
            foreach (var primary in NavigationCatalog.GetPrimaryItems())
            {
                _allPrimaryNavItems.Add(new PrimaryNavItem(primary.Key, primary.DisplayName));
            }

            // 二级导航：每个模块下的页面列表
            foreach (var primary in NavigationCatalog.GetPrimaryItems())
            {
                _allSecondaryNavMap[primary.Key] = NavigationCatalog
                    .GetSecondaryItems(primary.Key)
                    .Select(x => new SecondaryNavItem(x.PageKey, x.DisplayName, x.Description, x.AllowedRoles))
                    .ToList();
            }
        }

        private void ApplyNavigationByUser()
        {
            _visiblePrimaryNavItems.Clear();
            _visibleSecondaryNavMap.Clear();

            foreach (var primary in _allPrimaryNavItems)
            {
                List<SecondaryNavItem> secondaryItems;
                if (!_allSecondaryNavMap.TryGetValue(primary.Key, out secondaryItems))
                {
                    continue;
                }

                var visibleItems = secondaryItems
                    .Where(CanAccess)
                    .ToList();

                if (visibleItems.Count == 0)
                {
                    continue;
                }

                _visiblePrimaryNavItems.Add(primary);
                _visibleSecondaryNavMap[primary.Key] = visibleItems;
            }
        }

        private bool CanAccess(SecondaryNavItem item)
        {
            if (item == null)
            {
                return false;
            }

            return UserContext.Instance.HasPagePermission(item.Key);
        }

        private void NonClientArea_NavigateRequested(string tag)
        {
            var navItem = _visibleSecondaryNavMap.Values
                .SelectMany(x => x)
                .FirstOrDefault(x => string.Equals(x.Key, tag, StringComparison.OrdinalIgnoreCase));

            if (navItem == null)
            {
                TextBlockWorkAreaTitle.Text = "扩展页面";
                TextBlockWorkAreaDescription.Text = tag;
                MainFrame.Content = CreatePlaceholderPage("扩展页面 / " + tag);
                return;
            }

            UpdateWorkAreaHeader(navItem.DisplayName, navItem.Description);
            NavigateToPage(navItem.Key);
        }

        private void PrimaryNavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = PrimaryNavList.SelectedItem as PrimaryNavItem;
            if (item == null)
            {
                return;
            }

            TextBlockPrimaryTitle.Text = item.DisplayName;

            List<SecondaryNavItem> items;
            if (!_visibleSecondaryNavMap.TryGetValue(item.Key, out items))
            {
                SecondaryNavList.ItemsSource = null;
                return;
            }

            SecondaryNavList.ItemsSource = items;
            SecondaryNavList.SelectedIndex = items.Count > 0 ? 0 : -1;
        }

        private void SecondaryNavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = SecondaryNavList.SelectedItem as SecondaryNavItem;
            if (item == null)
            {
                return;
            }

            UpdateWorkAreaHeader(item.DisplayName, item.Description);
            NavigateToPage(item.Key);
        }

        private void UpdateWorkAreaHeader(string title, string description)
        {
            TextBlockWorkAreaTitle.Text = title;
            TextBlockWorkAreaDescription.Text = description;
        }

        private void NavigateToPage(string tag)
        {
            if (!_pageCache.ContainsKey(tag))
            {
                _pageCache[tag] = CreatePage(tag);
            }

            MainFrame.Content = _pageCache[tag];
        }

        private UserControl CreatePage(string tag)
        {
            switch (tag)
            {
                case "Home.Overview":
                    return CreatePlaceholderPage("首页 / 总览看板");
                case "Home.Status":
                    return CreatePlaceholderPage("首页 / 设备状态");

                case "Production.Data":
                    return CreatePlaceholderPage("生产 / 生产数据");
                case "Production.Report":
                    return CreatePlaceholderPage("生产 / 班次统计");

                case "Motion.Axis":
                    return new MotionAxisView();
                case "Motion.Status":
                    return CreatePlaceholderPage("运动 / 位置监视");
                case "Motion.Alarm":
                    return CreatePlaceholderPage("运动 / 运动报警");

                case "IO.DI":
                    return new DIMonitorView();
                case "IO.DO":
                    return new DOMonitorView();
                case "IO.Debug":
                    return CreatePlaceholderPage("IO / IO 调试");

                case "Vision.Monitor":
                    return CreatePlaceholderPage("视觉 / 相机监视");

                case "PLC.Monitor":
                    return CreatePlaceholderPage("PLC / 点位监视");
                case "PLC.Debug":
                    return CreatePlaceholderPage("PLC / 通讯状态");

                case "Config.Card":
                    return new MotionCardManagementView();
                case "Config.Axis":
                    return new MotionAxisManagementView();
                case "Config.IoMap":
                    return CreatePlaceholderPage("配置 / IO 映射配置");
                case "Config.AxisParam":
                    return CreatePlaceholderPage("配置 / 轴运行参数");
                case "Config.Actuator":
                    return CreatePlaceholderPage("配置 / 执行器配置");
                case "Config.Runtime":
                    return CreatePlaceholderPage("配置 / 运行配置编辑");
                case "Config.Camera":
                    return CreatePlaceholderPage("配置 / 相机配置编辑");
                case "Config.Plc":
                    return CreatePlaceholderPage("配置 / PLC 配置编辑");

                case "Engineer.RawAxis":
                    return CreatePlaceholderPage("工程 / 原始轴参数");
                case "Engineer.RawPlc":
                    return CreatePlaceholderPage("工程 / 原始 PLC 参数");
                case "Engineer.RawCamera":
                    return CreatePlaceholderPage("工程 / 原始相机参数");
                case "Engineer.Diagnostic":
                    return CreatePlaceholderPage("工程 / 设备诊断");
                case "Engineer.Debug":
                    return CreatePlaceholderPage("工程 / Motion/IO 调试");

                case "AlarmLog.Current":
                    return new CurrentAlarmView();
                case "AlarmLog.History":
                    return CreatePlaceholderPage("报警与日志 / 报警历史");
                case "AlarmLog.RunLog":
                    return CreatePlaceholderPage("报警与日志 / 运行日志");

                case "System.User":
                    return new UserManagementView();
                case "System.Permission":
                    return new UserPermissionView();
                case "System.LoginLog":
                    return new LoginLogView();

                default:
                    return CreatePlaceholderPage("未定义页面: " + tag);
            }
        }

        private UserControl CreatePlaceholderPage(string title)
        {
            return new UserControl
            {
                Background = Brushes.Transparent,
                Content = new Border
                {
                    Padding = new Thickness(24),
                    Child = new TextBlock
                    {
                        Text = title,
                        FontSize = 24,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = Brushes.DimGray,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    }
                }
            };
        }

        private void ButtonUserCard_OnClick(object sender, RoutedEventArgs e)
        {
            PopupUserMenu.IsOpen = !PopupUserMenu.IsOpen;
        }

        private void ButtonSwitchUser_OnClick(object sender, RoutedEventArgs e)
        {
            PopupUserMenu.IsOpen = false;
            ShowLoginForSwitchUser();
        }

        private void ButtonChangePassword_OnClick(object sender, RoutedEventArgs e)
        {
            PopupUserMenu.IsOpen = false;
            ShowChangePasswordDialog();
        }

        private void ButtonLogout_OnClick(object sender, RoutedEventArgs e)
        {
            PopupUserMenu.IsOpen = false;
            LogoutAndReturnToLogin();
        }

        private void ShowLoginForSwitchUser()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginView = new LoginView();
            var loginResult = loginView.ShowDialog();

            if (loginResult == true && UserContext.Instance.IsLoggedIn)
            {
                var newMainWindow = new MainWindow();
                Application.Current.MainWindow = newMainWindow;
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                newMainWindow.Show();
                Close();
                return;
            }

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void LogoutAndReturnToLogin()
        {
            UserContext.Instance.SignOut();

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginView = new LoginView();
            var loginResult = loginView.ShowDialog();

            if (loginResult == true && UserContext.Instance.IsLoggedIn)
            {
                var newMainWindow = new MainWindow();
                Application.Current.MainWindow = newMainWindow;
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                newMainWindow.Show();
                Close();
                return;
            }

            Application.Current.Shutdown();
        }

        private void ShowChangePasswordDialog()
        {
            var dialog = new ChangePasswordDialog
            {
                Owner = this
            };

            dialog.ShowDialog();
        }

        public void RefreshNavigationByCurrentUser()
        {
            var currentPrimaryKey = (PrimaryNavList.SelectedItem as PrimaryNavItem)?.Key;
            var currentSecondaryKey = (SecondaryNavList.SelectedItem as SecondaryNavItem)?.Key;

            ApplyNavigationByUser();
            PrimaryNavList.ItemsSource = null;
            PrimaryNavList.ItemsSource = _visiblePrimaryNavItems;

            if (_visiblePrimaryNavItems.Count == 0)
            {
                TextBlockPrimaryTitle.Text = "无可用模块";
                TextBlockWorkAreaTitle.Text = "无可用模块";
                TextBlockWorkAreaDescription.Text = "当前用户没有可访问的页面";
                MainFrame.Content = null;
                return;
            }

            var primary = _visiblePrimaryNavItems.FirstOrDefault(x => x.Key == currentPrimaryKey) ?? _visiblePrimaryNavItems[0];
            PrimaryNavList.SelectedItem = primary;

            List<SecondaryNavItem> items;
            if (!_visibleSecondaryNavMap.TryGetValue(primary.Key, out items) || items == null || items.Count == 0)
            {
                SecondaryNavList.ItemsSource = null;
                MainFrame.Content = null;
                return;
            }

            SecondaryNavList.ItemsSource = items;

            var secondary = items.FirstOrDefault(x => x.Key == currentSecondaryKey) ?? items[0];
            SecondaryNavList.SelectedItem = secondary;

            UpdateWorkAreaHeader(secondary.DisplayName, secondary.Description);
            NavigateToPage(secondary.Key);
        }

        private void SeedDebugAlarmsOnStartup()
        {
            var alarmManager = SystemContext.Instance.AlarmManager;
            if (alarmManager == null)
            {
                return;
            }

            if (alarmManager.GetActiveAlarms().Count > 0)
            {
                return;
            }

            alarmManager.RaiseAlarm(
                AlarmCode.PLCDisconnect,
                AlarmLevel.Warning,
                "PLC 通讯延迟异常，当前处于重连观察状态。",
                "PLC",
                null,
                "启动联调阶段注入的调试报警，用于验证报警抽屉、分页与详情联动效果。",
                "检查网口链路、PLC 心跳周期与重连策略。");

            alarmManager.RaiseAlarm(
                AlarmCode.AxisServoAlarm,
                AlarmLevel.Alarm,
                "X 轴伺服报警，设备动作已被拦截。",
                "Motion",
                1,
                "伺服驱动器反馈异常，当前禁止继续下发运动命令。",
                "检查伺服报警代码、驱动器使能状态与急停回路。");

            alarmManager.RaiseAlarm(
                AlarmCode.CameraGrabFailed,
                AlarmLevel.Critical,
                "相机取像失败，当前批次视觉定位中断。",
                "Vision",
                null,
                "连续取像失败达到停机阈值，系统已切换为人工确认模式。",
                "检查相机供电、网线、触发源与曝光参数。");
        }

        private sealed class PrimaryNavItem
        {
            public PrimaryNavItem(string key, string displayName)
            {
                Key = key;
                DisplayName = displayName;
            }

            public string Key { get; private set; }

            public string DisplayName { get; private set; }
        }

        private sealed class SecondaryNavItem
        {
            public SecondaryNavItem(string key, string displayName, string description, params string[] allowedRoles)
            {
                Key = key;
                DisplayName = displayName;
                Description = description;
                AllowedRoles = allowedRoles == null ? new List<string>() : allowedRoles.ToList();
            }

            public string Key { get; private set; }

            public string DisplayName { get; private set; }

            public string Description { get; private set; }

            public List<string> AllowedRoles { get; private set; }
        }

        private sealed class AlarmDisplayItem
        {
            public string CodeText { get; set; }

            public string LevelText { get; set; }

            public string Message { get; set; }

            public DateTime Time { get; set; }

            public string Source { get; set; }

            public short? CardId { get; set; }

            public string Description { get; set; }

            public string Suggestion { get; set; }

            public string SummaryText
            {
                get
                {
                    var source = string.IsNullOrWhiteSpace(Source) ? "-" : Source;
                    return LevelText + " · " + source + " · " + Time.ToString("HH:mm:ss");
                }
            }
        }
    }
}