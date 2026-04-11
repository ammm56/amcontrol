using AM.Model.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.PageModel.Navigation
{
    /// <summary>
    /// 全局页面目录。
    /// 导航菜单与权限目录的唯一来源。
    /// </summary>
    public static class NavigationCatalog
    {
        private static readonly List<NavPageDef> _all = new List<NavPageDef>
        {
            D("Home",        "首页",       "Home.Overview",          "总览看板",        "设备总览、生产摘要与快捷入口。",                         "Operator,Engineer,Am", "低", 10),
            D("Home",        "首页",       "Home.SysStatus",         "系统状态",        "显示整机运动、PLC、相机、IO 总体在线状态。",             "Operator,Engineer,Am", "低", 20),

            D("Motion",      "设备",       "Motion.DI",              "DI 监视",         "查看全部数字输入点位状态与逻辑描述。",                   "Operator,Engineer,Am", "低", 10),
            D("Motion",      "设备",       "Motion.DO",              "DO 监视",         "查看全部数字输出点位状态与联动对象。",                   "Operator,Engineer,Am", "低", 20),
            D("Motion",      "设备",       "Motion.Monitor",         "多轴总览",        "查看所有轴的位置、速度与运动状态总览。",                 "Operator,Engineer,Am", "低", 30),
            D("Motion",      "设备",       "Motion.Axis",            "轴控制",          "单轴点动、回零与使能操作。",                             "Engineer,Am",          "高", 40),
            D("Motion",      "设备",       "Motion.Actuator",        "执行器控制",      "手动操作气缸、真空、夹爪等执行器。",                     "Engineer,Am",          "高", 50),

            D("Production",  "生产",       "Production.Order",       "工单管理",        "管理生产工单，新建、下发与完结。",                       "Operator,Engineer,Am", "中", 10),
            D("Production",  "生产",       "Production.Recipe",      "配方管理",        "管理多型号产品配方，配置轴位、视觉与工艺参数。",         "Engineer,Am",          "高", 20),
            D("Production",  "生产",       "Production.Data",        "生产数据",        "查看产量、节拍、良率与工单统计信息。",                   "Operator,Engineer,Am", "低", 30),
            D("Production",  "生产",       "Production.Report",      "班次统计",        "查看班次与日报汇总统计报表。",                           "Operator,Engineer,Am", "低", 40),
            D("Production",  "生产",       "Production.Trace",       "追溯查询",        "按条码或工单查询产品全流程追溯记录。",                   "Operator,Engineer,Am", "低", 50),
            D("Production",  "生产",       "Production.MesStatus",   "MES 状态",        "查看 MES 连接状态与数据上传实时状态。",                  "Operator,Engineer,Am", "低", 60),
            D("Production",  "生产",       "Production.UploadLog",   "上传记录",        "查看 MES 数据上传历史记录与失败重传情况。",              "Engineer,Am",          "低", 70),

            D("Vision",      "视觉",       "Vision.Monitor",         "相机监视",        "查看相机实时画面与触发状态。",                           "Operator,Engineer,Am", "低", 10),
            D("Vision",      "视觉",       "Vision.Result",          "检测结果",        "查看视觉检测结果、OK/NG 记录与图像回放。",               "Operator,Engineer,Am", "低", 20),
            D("Vision",      "视觉",       "Vision.Calibrate",       "标定管理",        "执行相机标定与精度验证操作。",                           "Engineer,Am",          "高", 30),

            D("PLC",         "PLC",        "PLC.Status",             "通讯状态",        "查看 PLC 通讯链路状态与诊断信息。",                      "Operator,Engineer,Am", "低", 30),
            D("PLC",         "PLC",        "PLC.Monitor",            "点位监视",        "查看 PLC 数字输入/输出点位实时状态。",                   "Operator,Engineer,Am", "低", 10),
            D("PLC",         "PLC",        "PLC.Register",           "寄存器监视",      "查看 PLC 寄存器地址与数据值。",                          "Engineer,Am",          "中", 20),
            D("PLC",         "PLC",        "PLC.Write",              "写入调试",        "手动向 PLC 寄存器写入调试值，仅限工程调试使用。",        "Engineer,Am",          "高", 40),

            D("Peripheral",  "外设",       "Peripheral.Scanner",     "扫码监视",        "查看扫码器在线状态与最近扫码记录。",                     "Operator,Engineer,Am", "低", 10),
            D("Peripheral",  "外设",       "Peripheral.ScanTest",    "扫码测试",        "手动触发扫码器扫描，验证通讯与码格式。",                 "Engineer,Am",          "中", 20),
            D("Peripheral",  "外设",       "Peripheral.Sensor",      "传感器监视",      "查看 Modbus/RS232/USB 传感器实时采样数据。",             "Operator,Engineer,Am", "低", 30),
            D("Peripheral",  "外设",       "Peripheral.SensorTrend", "数据趋势",        "查看传感器历史数据曲线与统计分析。",                     "Operator,Engineer,Am", "低", 40),

            D("MotionConfig","运控配置",   "MotionConfig.Card",      "控制卡配置",      "维护控制卡基础信息与驱动参数配置。",                     "Engineer,Am",          "高", 10),
            D("MotionConfig","运控配置",   "MotionConfig.Axis",      "轴拓扑配置",      "维护逻辑轴、物理轴与控制卡归属关系。",                   "Engineer,Am",          "高", 20),
            D("MotionConfig","运控配置",   "MotionConfig.IoMap",     "IO 映射配置",     "维护 DI/DO 逻辑点位与硬件点位映射关系。",                "Engineer,Am",          "高", 30),
            D("MotionConfig","运控配置",   "MotionConfig.AxisParam", "轴运行参数",      "维护轴运动参数、回零流程与软件限位配置。",               "Engineer,Am",          "高", 40),
            D("MotionConfig","运控配置",   "MotionConfig.Actuator",  "执行器配置",      "维护气缸、真空、灯塔、夹爪等执行器对象配置。",           "Engineer,Am",          "高", 50),

            D("SysConfig",   "系统配置",   "SysConfig.Camera",       "相机配置",        "维护视觉相机型号、连接与任务参数。",                     "Engineer,Am",          "高", 10),
            D("SysConfig",   "系统配置",   "SysConfig.Plc",          "PLC 配置",        "维护 PLC 通讯协议与业务地址映射。",                      "Engineer,Am",          "高", 20),
            D("SysConfig",   "系统配置",   "SysConfig.Sensor",       "传感器配置",      "维护 Modbus/RS232/USB 传感器连接参数。",                 "Engineer,Am",          "高", 30),
            D("SysConfig",   "系统配置",   "SysConfig.Scanner",      "扫码器配置",      "维护扫码器型号、接口与触发参数。",                       "Engineer,Am",          "高", 40),
            D("SysConfig",   "系统配置",   "SysConfig.Mes",          "MES 配置",        "维护 MES 服务地址、认证与上传策略。",                    "Engineer,Am",          "高", 50),
            D("SysConfig",   "系统配置",   "SysConfig.Runtime",      "运行配置",        "维护系统运行模式与全局运行参数。",                       "Engineer,Am",          "高", 60),

            D("Engineer",    "工程",       "Engineer.Diagnostic",    "设备诊断",        "执行整机在线诊断与子系统自检流程。",                     "Engineer,Am",          "中", 10),
            D("Engineer",    "工程",       "Engineer.RawAxis",       "原始轴参数",      "直接读写控制卡原始轴寄存器，供底层调试使用。",           "Engineer,Am",          "高", 20),
            D("Engineer",    "工程",       "Engineer.RawPlc",        "原始 PLC 参数",   "直接读写 PLC 原始内存地址，供底层调试使用。",            "Engineer,Am",          "高", 30),
            D("Engineer",    "工程",       "Engineer.RawCamera",     "原始相机参数",    "直接访问相机 SDK 底层参数，供底层调试使用。",            "Engineer,Am",          "高", 40),

            D("AlarmLog",    "报警与日志", "AlarmLog.Current",       "当前报警",        "查看当前所有活动报警与处理入口。",                       "Operator,Engineer,Am", "低", 10),
            D("AlarmLog",    "报警与日志", "AlarmLog.History",       "报警历史",        "查看所有历史报警记录与详情。",                           "Operator,Engineer,Am", "低", 20),
            D("AlarmLog",    "报警与日志", "AlarmLog.RunLog",        "运行日志",        "查看系统运行日志，支持筛选与关键字搜索。",               "Engineer,Am",          "低", 30),

            D("System",      "系统",       "System.User",            "用户管理",        "维护用户账户、角色与账号状态。",                         "Am",                   "高", 10),
            D("System",      "系统",       "System.Permission",      "权限分配",        "按用户分配页面级访问权限。",                             "Am",                   "高", 20),
            D("System",      "系统",       "System.LoginLog",        "登录日志",        "查看用户登录操作审计日志。",                             "Am",                   "中", 30)
        };

        public static IReadOnlyList<NavPageDef> All
        {
            get { return _all; }
        }

        public static IReadOnlyList<NavPrimaryDef> GetPrimaryItems()
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var result = new List<NavPrimaryDef>();

            foreach (var item in _all)
            {
                if (seen.Add(item.ModuleKey))
                {
                    result.Add(new NavPrimaryDef(item.ModuleKey, item.ModuleName));
                }
            }

            return result;
        }

        public static IReadOnlyList<NavPageDef> GetSecondaryItems(string moduleKey)
        {
            return _all.Where(x => x.ModuleKey == moduleKey).ToList();
        }

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
                RecommendedRoles = (x.DefaultRoleCodes ?? string.Empty).Replace(",", " / "),
                RiskLevel = x.RiskLevel,
                SortOrder = x.SortOrder,
                IsEnabled = true,
                CreateTime = DateTime.Now
            }).ToList();
        }

        private static NavPageDef D(
            string moduleKey,
            string moduleName,
            string pageKey,
            string displayName,
            string description,
            string defaultRoleCodes,
            string riskLevel,
            int sortOrder)
        {
            return new NavPageDef(
                moduleKey,
                moduleName,
                pageKey,
                displayName,
                description,
                defaultRoleCodes,
                riskLevel,
                sortOrder);
        }
    }
}