using AM.Core.Context;
using AMControlWPF.UserControls.Main;
using AMControlWPF.Views.Auth;
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

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
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

        private void InitializeCurrentUserCard()
        {
            var userName = string.IsNullOrWhiteSpace(UserContext.Instance.UserName)
                ? UserContext.Instance.LoginName
                : UserContext.Instance.UserName;

            var roleName = GetCurrentRoleDisplayName();

            TextBlockCurrentUserName.Text = string.IsNullOrWhiteSpace(userName) ? "未登录" : userName;
            TextBlockCurrentUserRole.Text = roleName;
            TextBlockPopupUserTitle.Text = TextBlockCurrentUserName.Text + " / " + roleName;

            GravatarCurrentUser.Id = string.IsNullOrWhiteSpace(UserContext.Instance.LoginName)
                ? "guest"
                : UserContext.Instance.LoginName;

            ButtonManageUsers.Visibility = UserContext.Instance.IsAdmin
                ? Visibility.Visible
                : Visibility.Collapsed;
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
            _allPrimaryNavItems.Add(new PrimaryNavItem("Home", "首页"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("Production", "生产"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("Motion", "运动"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("IO", "IO"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("Vision", "视觉"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("PLC", "PLC"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("Config", "配置"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("Engineer", "工程"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("AlarmLog", "报警与日志"));
            _allPrimaryNavItems.Add(new PrimaryNavItem("System", "系统"));

            _allSecondaryNavMap["Home"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Home.Overview", "总览看板", "设备总览、生产摘要、快捷入口"),
                new SecondaryNavItem("Home.Status", "设备状态", "运动、PLC、相机、IO 总状态")
            };

            _allSecondaryNavMap["Production"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Production.Data", "生产数据", "产量、节拍、良率、工单", "Operator", "Engineer", "Am"),
                new SecondaryNavItem("Production.Report", "班次统计", "班次与日报汇总", "Engineer", "Am")
            };

            _allSecondaryNavMap["Motion"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Motion.Axis", "轴控制", "当前选中轴的控制、状态、位置与动作记录", "Operator", "Engineer", "Am"),
                new SecondaryNavItem("Motion.Status", "位置监视", "多轴位置、速度、状态总览", "Operator", "Engineer", "Am"),
                new SecondaryNavItem("Motion.Alarm", "运动报警", "轴报警记录、处理与复位", "Operator", "Engineer", "Am")
            };

            _allSecondaryNavMap["IO"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("IO.DI", "DI 监视", "输入点状态、条件判断与点位详情", "Operator", "Engineer", "Am"),
                new SecondaryNavItem("IO.DO", "DO 监视", "输出点状态、输出控制与联动对象", "Operator", "Engineer", "Am"),
                new SecondaryNavItem("IO.Debug", "IO 调试", "IO 调试记录与联动测试", "Engineer", "Am")
            };

            _allSecondaryNavMap["Vision"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Vision.Monitor", "相机监视", "实时画面与触发结果", "Operator", "Engineer", "Am")
            };

            _allSecondaryNavMap["PLC"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("PLC.Monitor", "点位监视", "寄存器、位变量监视", "Operator", "Engineer", "Am"),
                new SecondaryNavItem("PLC.Debug", "通讯状态", "通讯诊断与报文状态", "Operator", "Engineer", "Am")
            };

            _allSecondaryNavMap["Config"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Config.Axis", "轴配置编辑", "运行配置层编辑", "Engineer", "Am"),
                new SecondaryNavItem("Config.Card", "控制卡配置编辑", "卡参数与映射", "Engineer", "Am"),
                new SecondaryNavItem("Config.Camera", "相机配置编辑", "相机与任务配置", "Engineer", "Am"),
                new SecondaryNavItem("Config.Plc", "PLC 配置编辑", "PLC 业务配置", "Engineer", "Am"),
                new SecondaryNavItem("Config.Runtime", "运行配置编辑", "系统运行参数", "Engineer", "Am")
            };

            _allSecondaryNavMap["Engineer"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Engineer.RawAxis", "原始轴参数", "控制卡原始参数", "Engineer", "Am"),
                new SecondaryNavItem("Engineer.RawPlc", "原始 PLC 参数", "PLC 原始参数", "Engineer", "Am"),
                new SecondaryNavItem("Engineer.RawCamera", "原始相机参数", "视觉原始参数", "Engineer", "Am"),
                new SecondaryNavItem("Engineer.Diagnostic", "设备诊断", "运行诊断与状态检查", "Engineer", "Am"),
                new SecondaryNavItem("Engineer.Debug", "Motion/IO 调试", "联机调试与测试页", "Engineer", "Am")
            };

            _allSecondaryNavMap["AlarmLog"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("AlarmLog.Current", "当前报警", "当前活动报警", "Operator", "Engineer", "Am"),
                new SecondaryNavItem("AlarmLog.History", "报警历史", "历史报警查询", "Engineer", "Am"),
                new SecondaryNavItem("AlarmLog.RunLog", "运行日志", "系统运行日志", "Engineer", "Am")
            };

            _allSecondaryNavMap["System"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("System.User", "用户管理", "用户、角色、密码重置与启停管理", "Am"),
                new SecondaryNavItem("System.LoginLog", "登录日志", "用户登录历史与登录结果记录", "Am")
            };
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

            if (UserContext.Instance.IsAdmin)
            {
                return true;
            }

            if (item.AllowedRoles.Count == 0)
            {
                return true;
            }

            return item.AllowedRoles.Any(UserContext.Instance.HasRole);
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
                case "Config.Axis":
                    return CreatePlaceholderPage("配置 / 轴配置编辑");
                case "Config.Card":
                    return CreatePlaceholderPage("配置 / 控制卡配置编辑");
                case "Config.Camera":
                    return CreatePlaceholderPage("配置 / 相机配置编辑");
                case "Config.Plc":
                    return CreatePlaceholderPage("配置 / PLC 配置编辑");
                case "Config.Runtime":
                    return CreatePlaceholderPage("配置 / 运行配置编辑");
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
                    return CreatePlaceholderPage("报警与日志 / 当前报警");
                case "AlarmLog.History":
                    return CreatePlaceholderPage("报警与日志 / 报警历史");
                case "AlarmLog.RunLog":
                    return CreatePlaceholderPage("报警与日志 / 运行日志");
                case "System.User":
                    return CreatePlaceholderPage("系统 / 用户管理");
                case "System.LoginLog":
                    return CreatePlaceholderPage("系统 / 登录日志");
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
            ShowLoginAndReopenMainWindow();
        }

        private void ButtonChangePassword_OnClick(object sender, RoutedEventArgs e)
        {
            PopupUserMenu.IsOpen = false;
            TextBlockWorkAreaTitle.Text = "修改密码";
            TextBlockWorkAreaDescription.Text = "当前登录用户密码修改";
            MainFrame.Content = CreatePlaceholderPage("用户 / 修改密码");
        }

        private void ButtonManageUsers_OnClick(object sender, RoutedEventArgs e)
        {
            PopupUserMenu.IsOpen = false;
            NavigateToByTag("System.User");
        }

        private void ButtonLogout_OnClick(object sender, RoutedEventArgs e)
        {
            PopupUserMenu.IsOpen = false;
            UserContext.Instance.SignOut();
            ShowLoginAndReopenMainWindow();
        }

        private void NavigateToByTag(string tag)
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

            TextBlockWorkAreaTitle.Text = navItem.DisplayName;
            TextBlockWorkAreaDescription.Text = navItem.Description;
            NavigateToPage(navItem.Key);
        }

        private void ShowLoginAndReopenMainWindow()
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

            Application.Current.Shutdown();
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
    }
}