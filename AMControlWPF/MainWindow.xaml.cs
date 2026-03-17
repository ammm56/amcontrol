using AMControlWPF.Pages.IO;
using AMControlWPF.Pages.Motion;
using AMControlWPF.Pages.Template;
using AMControlWPF.UserControls.Main;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
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
        private readonly List<PrimaryNavItem> _primaryNavItems = new List<PrimaryNavItem>();
        private readonly Dictionary<string, List<SecondaryNavItem>> _secondaryNavMap = new Dictionary<string, List<SecondaryNavItem>>();

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
            PrimaryNavList.ItemsSource = _primaryNavItems;
            PrimaryNavList.SelectedIndex = 0;
        }

        private void InitializeNavigation()
        {
            _primaryNavItems.Add(new PrimaryNavItem("Home", "首页"));
            _primaryNavItems.Add(new PrimaryNavItem("Production", "生产"));
            _primaryNavItems.Add(new PrimaryNavItem("Motion", "运动"));
            _primaryNavItems.Add(new PrimaryNavItem("IO", "IO"));
            _primaryNavItems.Add(new PrimaryNavItem("Vision", "视觉"));
            _primaryNavItems.Add(new PrimaryNavItem("PLC", "PLC"));
            _primaryNavItems.Add(new PrimaryNavItem("Config", "配置"));
            _primaryNavItems.Add(new PrimaryNavItem("Engineer", "工程"));
            _primaryNavItems.Add(new PrimaryNavItem("AlarmLog", "报警与日志"));

            _secondaryNavMap["Home"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Home.Overview", "总览看板", "设备总览、生产摘要、快捷入口"),
                new SecondaryNavItem("Home.Status", "设备状态", "运动、PLC、相机、IO 总状态")
            };

            _secondaryNavMap["Production"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Production.Data", "生产数据", "产量、节拍、良率、工单"),
                new SecondaryNavItem("Production.Report", "班次统计", "班次与日报汇总")
            };

            _secondaryNavMap["Motion"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Motion.Axis", "轴控制", "手动控制、回零、点动"),
                new SecondaryNavItem("Motion.Status", "位置监视", "规划位置、编码器位置、状态"),
                new SecondaryNavItem("Motion.Alarm", "运动报警", "轴报警与复位")
            };

            _secondaryNavMap["IO"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("IO.DI", "DI 监视", "输入点状态与筛选"),
                new SecondaryNavItem("IO.DO", "DO 监视", "输出点状态与操作"),
                new SecondaryNavItem("IO.Debug", "IO 调试", "工程调试与联动测试")
            };

            _secondaryNavMap["Vision"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Vision.Monitor", "相机监视", "实时画面与触发结果"),
                new SecondaryNavItem("Vision.Config", "相机配置", "相机参数与任务参数")
            };

            _secondaryNavMap["PLC"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("PLC.Config", "PLC 配置", "连接参数与站号配置"),
                new SecondaryNavItem("PLC.Monitor", "点位监视", "寄存器、位变量监视"),
                new SecondaryNavItem("PLC.Debug", "通讯状态", "通讯诊断与报文状态")
            };

            _secondaryNavMap["Config"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Config.Axis", "轴配置编辑", "运行配置层编辑"),
                new SecondaryNavItem("Config.Card", "控制卡配置编辑", "卡参数与映射"),
                new SecondaryNavItem("Config.Camera", "相机配置编辑", "相机与任务配置"),
                new SecondaryNavItem("Config.Plc", "PLC 配置编辑", "PLC 业务配置"),
                new SecondaryNavItem("Config.Runtime", "运行配置编辑", "系统运行参数")
            };

            _secondaryNavMap["Engineer"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("Engineer.RawAxis", "原始轴参数", "控制卡原始参数"),
                new SecondaryNavItem("Engineer.RawPlc", "原始 PLC 参数", "PLC 原始参数"),
                new SecondaryNavItem("Engineer.RawCamera", "原始相机参数", "视觉原始参数"),
                new SecondaryNavItem("Engineer.Diagnostic", "设备诊断", "运行诊断与状态检查"),
                new SecondaryNavItem("Engineer.Debug", "Motion/IO 调试", "联机调试与测试页")
            };

            _secondaryNavMap["AlarmLog"] = new List<SecondaryNavItem>
            {
                new SecondaryNavItem("AlarmLog.Current", "当前报警", "当前活动报警"),
                new SecondaryNavItem("AlarmLog.History", "报警历史", "历史报警查询"),
                new SecondaryNavItem("AlarmLog.RunLog", "运行日志", "系统运行日志")
            };
        }

        private void NonClientArea_NavigateRequested(string tag)
        {
            NavigateToPage(tag);
        }

        private void PrimaryNavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = PrimaryNavList.SelectedItem as PrimaryNavItem;
            if (item == null)
            {
                return;
            }

            TextBlockPrimaryTitle.Text = item.DisplayName;
            SecondaryNavList.ItemsSource = _secondaryNavMap[item.Key];
            SecondaryNavList.SelectedIndex = 0;
        }

        private void SecondaryNavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = SecondaryNavList.SelectedItem as SecondaryNavItem;
            if (item == null)
            {
                return;
            }

            NavigateToPage(item.Key);
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
                    return new MotionAxisPage();
                case "Motion.Status":
                    return CreatePlaceholderPage("运动 / 位置监视");
                case "Motion.Alarm":
                    return CreatePlaceholderPage("运动 / 运动报警");
                case "IO.DI":
                    return new IOMonitorPage();
                case "IO.DO":
                    return new IOMonitorPage();
                case "IO.Debug":
                    return CreatePlaceholderPage("IO / IO 调试");
                case "Vision.Monitor":
                    return CreatePlaceholderPage("视觉 / 相机监视");
                case "Vision.Config":
                    return CreatePlaceholderPage("视觉 / 相机配置");
                case "PLC.Config":
                    return CreatePlaceholderPage("PLC / PLC 配置");
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
            public SecondaryNavItem(string key, string displayName, string description)
            {
                Key = key;
                DisplayName = displayName;
                Description = description;
            }

            public string Key { get; private set; }

            public string DisplayName { get; private set; }

            public string Description { get; private set; }
        }
    }
}