using AM.Model.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMControlWPF.Navigation
{
    /// <summary>
    /// 全局页面目录 — 导航菜单与权限 DB 种子的唯一来源。
    /// 新增 / 删除页面只需修改此文件，权限 DB 和导航将在下次启动时自动同步。
    /// </summary>
    internal static class NavigationCatalog
    {
        // ─── 定义列表（顺序即主导航 + 二级导航显示顺序） ───
        private static readonly List<NavPageDef> _all = new List<NavPageDef>
        {
            // ── 首页 ──
            D("Home", "首页",        "Home.Overview",   "总览看板",        "设备总览、生产摘要与快捷入口。",         "Operator,Engineer,Am", "低", 10),
            D("Home", "首页",        "Home.Status",     "设备状态",        "显示整机运动、PLC、相机、IO 总体状态。", "Operator,Engineer,Am", "低", 20),

            // ── 生产 ──
            D("Production", "生产",  "Production.Data", "生产数据",        "查看产量、节拍、良率与工单信息。",       "Operator,Engineer,Am", "低", 10),
            D("Production", "生产",  "Production.Report","班次统计",       "查看班次与日报汇总统计。",               "Engineer,Am",          "低", 20),

            // ── 运动 ──
            D("Motion", "运动",      "Motion.Axis",     "轴控制",          "查看当前轴状态并进入轴控制页面。",       "Operator,Engineer,Am", "中", 10),
            D("Motion", "运动",      "Motion.Status",   "位置监视",        "查看多轴位置、速度、状态总览。",         "Operator,Engineer,Am", "低", 20),
            D("Motion", "运动",      "Motion.Alarm",    "运动报警",        "查看运动域报警记录与处理信息。",         "Operator,Engineer,Am", "低", 30),

            // ── IO ──
            D("IO", "IO",            "IO.DI",           "DI 监视",         "查看输入点位状态与条件判断结果。",       "Operator,Engineer,Am", "低", 10),
            D("IO", "IO",            "IO.DO",           "DO 监视",         "查看输出点位状态与联动对象。",           "Operator,Engineer,Am", "中", 20),
            D("IO", "IO",            "IO.Debug",        "IO 调试",         "进行 IO 调试与联动测试。",               "Engineer,Am",          "高", 30),

            // ── 视觉 ──
            D("Vision", "视觉",      "Vision.Monitor",  "相机监视",        "查看相机实时画面与触发结果。",           "Operator,Engineer,Am", "低", 10),

            // ── PLC ──
            D("PLC", "PLC",          "PLC.Monitor",     "点位监视",        "查看 PLC 点位与寄存器状态。",            "Operator,Engineer,Am", "低", 10),
            D("PLC", "PLC",          "PLC.Debug",       "通讯状态",        "查看 PLC 通讯诊断信息。",                "Operator,Engineer,Am", "中", 20),

            // ── 配置 ──
            D("Config", "配置",      "Config.Card",     "控制卡配置",      "维护控制卡基础信息与驱动配置。",         "Engineer,Am", "高", 10),
            D("Config", "配置",      "Config.Axis",     "轴拓扑配置",      "维护逻辑轴、物理轴与归属关系。",         "Engineer,Am", "高", 20),
            D("Config", "配置",      "Config.IoMap",    "IO 映射配置",     "维护 DI/DO 逻辑点与硬件点映射。",        "Engineer,Am", "高", 30),
            D("Config", "配置",      "Config.AxisParam", "轴运行参数",     "维护轴运行参数、回零与限位配置。",       "Engineer,Am", "高", 40),
            D("Config", "配置",      "Config.Actuator", "执行器配置",      "维护气缸、真空、灯塔、夹爪对象配置。",  "Engineer,Am", "高", 50),
            D("Config", "配置",      "Config.Runtime",  "运行配置编辑",    "维护系统运行配置。",                     "Engineer,Am", "高", 60),
            D("Config", "配置",      "Config.Camera",   "相机配置编辑",    "维护相机与任务配置。",                   "Engineer,Am", "高", 70),
            D("Config", "配置",      "Config.Plc",      "PLC 配置编辑",    "维护 PLC 业务配置。",                    "Engineer,Am", "高", 80),

            // ── 工程 ──
            D("Engineer", "工程",    "Engineer.RawAxis",    "原始轴参数",      "查看或调试原始轴参数。",             "Engineer,Am", "高", 10),
            D("Engineer", "工程",    "Engineer.RawPlc",     "原始 PLC 参数",   "查看或调试原始 PLC 参数。",          "Engineer,Am", "高", 20),
            D("Engineer", "工程",    "Engineer.RawCamera",  "原始相机参数",    "查看或调试原始相机参数。",           "Engineer,Am", "高", 30),
            D("Engineer", "工程",    "Engineer.Diagnostic", "设备诊断",        "查看设备诊断结果与检查信息。",       "Engineer,Am", "中", 40),
            D("Engineer", "工程",    "Engineer.Debug",      "Motion/IO 调试",  "执行工程调试与测试。",               "Engineer,Am", "高", 50),

            // ── 报警与日志 ──
            D("AlarmLog", "报警与日志", "AlarmLog.Current", "当前报警",     "查看当前活动报警。",                     "Operator,Engineer,Am", "低", 10),
            D("AlarmLog", "报警与日志", "AlarmLog.History", "报警历史",     "查看历史报警记录。",                     "Engineer,Am",          "低", 20),
            D("AlarmLog", "报警与日志", "AlarmLog.RunLog",  "运行日志",     "查看系统运行日志。",                     "Engineer,Am",          "低", 30),

            // ── 系统 ──
            D("System", "系统",      "System.User",       "用户管理",      "维护用户、角色、启停与密码。",           "Am", "高", 10),
            D("System", "系统",      "System.Permission", "权限分配",      "维护用户页面权限。",                     "Am", "高", 20),
            D("System", "系统",      "System.LoginLog",   "登录日志",      "查看登录审计日志。",                     "Am", "中", 30),
        };

        // ─── 公开访问 ───

        public static IReadOnlyList<NavPageDef> All => _all;

        /// <summary>按首次出现顺序返回主模块列表。</summary>
        public static IReadOnlyList<NavPrimaryDef> GetPrimaryItems()
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var result = new List<NavPrimaryDef>();
            foreach (var p in _all)
            {
                if (seen.Add(p.ModuleKey))
                {
                    result.Add(new NavPrimaryDef(p.ModuleKey, p.ModuleName));
                }
            }
            return result;
        }

        /// <summary>返回指定模块的二级导航页面。</summary>
        public static IReadOnlyList<NavPageDef> GetSecondaryItems(string moduleKey)
        {
            return _all.Where(x => x.ModuleKey == moduleKey).ToList();
        }

        /// <summary>
        /// 将目录转换为 SysPagePermissionEntity 列表，供 AuthSeedService 同步 DB。
        /// </summary>
        public static List<SysPagePermissionEntity> ToPermissionEntities()
        {
            return _all.Select(x => new SysPagePermissionEntity
            {
                ModuleKey = x.ModuleKey,
                ModuleName = x.ModuleName,
                PageKey = x.PageKey,
                DisplayName = x.DisplayName,
                Description = x.Description,
                DefaultRoleCodes = x.DefaultRoleCodes,
                RecommendedRoles = x.DefaultRoleCodes.Replace(",", " / "),
                RiskLevel = x.RiskLevel,
                SortOrder = x.SortOrder,
                IsEnabled = true,
                CreateTime = DateTime.Now
            }).ToList();
        }

        private static NavPageDef D(
            string moduleKey, string moduleName,
            string pageKey, string displayName, string description,
            string defaultRoleCodes, string riskLevel, int sortOrder)
        {
            return new NavPageDef(
                moduleKey, moduleName, pageKey, displayName,
                description, defaultRoleCodes, riskLevel, sortOrder);
        }
    }

    internal sealed class NavPrimaryDef
    {
        public NavPrimaryDef(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }

        public string Key { get; }
        public string DisplayName { get; }
    }

    internal sealed class NavPageDef
    {
        public NavPageDef(
            string moduleKey, string moduleName,
            string pageKey, string displayName, string description,
            string defaultRoleCodes, string riskLevel, int sortOrder)
        {
            ModuleKey = moduleKey;
            ModuleName = moduleName;
            PageKey = pageKey;
            DisplayName = displayName;
            Description = description;
            DefaultRoleCodes = defaultRoleCodes;
            RiskLevel = riskLevel;
            SortOrder = sortOrder;
        }

        public string ModuleKey { get; }
        public string ModuleName { get; }
        public string PageKey { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public string DefaultRoleCodes { get; }
        public string RiskLevel { get; }
        public int SortOrder { get; }

        /// <summary>解析后的角色数组，用于导航可见性过滤。</summary>
        public string[] AllowedRoles =>
            DefaultRoleCodes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }
}