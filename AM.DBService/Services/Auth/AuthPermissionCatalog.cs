using AM.Model.Entity.Auth;
using System;
using System.Collections.Generic;

namespace AM.DBService.Services.Auth
{
    internal static class AuthPermissionCatalog
    {
        public static List<SysPagePermissionEntity> CreateDefaultPagePermissions()
        {
            return new List<SysPagePermissionEntity>
            {
                new SysPagePermissionEntity { ModuleKey = "Home", ModuleName = "首页", PageKey = "Home.Overview", DisplayName = "总览看板", Description = "设备总览、生产摘要与快捷入口。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Home", ModuleName = "首页", PageKey = "Home.Status", DisplayName = "设备状态", Description = "显示整机运动、PLC、相机、IO 总体状态。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "Production", ModuleName = "生产", PageKey = "Production.Data", DisplayName = "生产数据", Description = "查看产量、节拍、良率与工单信息。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Production", ModuleName = "生产", PageKey = "Production.Report", DisplayName = "班次统计", Description = "查看班次与日报汇总统计。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "低", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "Motion", ModuleName = "运动", PageKey = "Motion.Axis", DisplayName = "轴控制", Description = "查看当前轴状态并进入轴控制页面。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "中", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Motion", ModuleName = "运动", PageKey = "Motion.Status", DisplayName = "位置监视", Description = "查看多轴位置、速度、状态总览。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Motion", ModuleName = "运动", PageKey = "Motion.Alarm", DisplayName = "运动报警", Description = "查看运动域报警记录与处理信息。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 30, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "IO", ModuleName = "IO", PageKey = "IO.DI", DisplayName = "DI 监视", Description = "查看输入点位状态与条件判断结果。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "IO", ModuleName = "IO", PageKey = "IO.DO", DisplayName = "DO 监视", Description = "查看输出点位状态与联动对象。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "中", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "IO", ModuleName = "IO", PageKey = "IO.Debug", DisplayName = "IO 调试", Description = "进行 IO 调试与联动测试。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 30, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "Vision", ModuleName = "视觉", PageKey = "Vision.Monitor", DisplayName = "相机监视", Description = "查看相机实时画面与触发结果。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "PLC", ModuleName = "PLC", PageKey = "PLC.Monitor", DisplayName = "点位监视", Description = "查看 PLC 点位与寄存器状态。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "PLC", ModuleName = "PLC", PageKey = "PLC.Debug", DisplayName = "通讯状态", Description = "查看 PLC 通讯诊断信息。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "中", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "Config", ModuleName = "配置", PageKey = "Config.Axis", DisplayName = "轴配置编辑", Description = "维护轴正式配置参数。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Config", ModuleName = "配置", PageKey = "Config.Card", DisplayName = "控制卡配置编辑", Description = "维护控制卡参数与映射。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Config", ModuleName = "配置", PageKey = "Config.Camera", DisplayName = "相机配置编辑", Description = "维护相机正式配置。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 30, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Config", ModuleName = "配置", PageKey = "Config.Plc", DisplayName = "PLC 配置编辑", Description = "维护 PLC 业务配置。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 40, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Config", ModuleName = "配置", PageKey = "Config.Runtime", DisplayName = "运行配置编辑", Description = "维护系统运行配置。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 50, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "Engineer", ModuleName = "工程", PageKey = "Engineer.RawAxis", DisplayName = "原始轴参数", Description = "查看或调试原始轴参数。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Engineer", ModuleName = "工程", PageKey = "Engineer.RawPlc", DisplayName = "原始 PLC 参数", Description = "查看或调试原始 PLC 参数。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Engineer", ModuleName = "工程", PageKey = "Engineer.RawCamera", DisplayName = "原始相机参数", Description = "查看或调试原始相机参数。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 30, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Engineer", ModuleName = "工程", PageKey = "Engineer.Diagnostic", DisplayName = "设备诊断", Description = "查看设备诊断结果与检查信息。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "中", SortOrder = 40, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "Engineer", ModuleName = "工程", PageKey = "Engineer.Debug", DisplayName = "Motion/IO 调试", Description = "执行工程调试与测试。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "高", SortOrder = 50, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "AlarmLog", ModuleName = "报警与日志", PageKey = "AlarmLog.Current", DisplayName = "当前报警", Description = "查看当前活动报警。", DefaultRoleCodes = "Operator,Engineer,Am", RecommendedRoles = "Operator / Engineer / Am", RiskLevel = "低", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "AlarmLog", ModuleName = "报警与日志", PageKey = "AlarmLog.History", DisplayName = "报警历史", Description = "查看历史报警记录。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "低", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "AlarmLog", ModuleName = "报警与日志", PageKey = "AlarmLog.RunLog", DisplayName = "运行日志", Description = "查看系统运行日志。", DefaultRoleCodes = "Engineer,Am", RecommendedRoles = "Engineer / Am", RiskLevel = "低", SortOrder = 30, IsEnabled = true, CreateTime = DateTime.Now },

                new SysPagePermissionEntity { ModuleKey = "System", ModuleName = "系统", PageKey = "System.User", DisplayName = "用户管理", Description = "维护用户、角色、启停与密码。", DefaultRoleCodes = "Am", RecommendedRoles = "Am", RiskLevel = "高", SortOrder = 10, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "System", ModuleName = "系统", PageKey = "System.Permission", DisplayName = "权限分配", Description = "维护用户页面权限。", DefaultRoleCodes = "Am", RecommendedRoles = "Am", RiskLevel = "高", SortOrder = 20, IsEnabled = true, CreateTime = DateTime.Now },
                new SysPagePermissionEntity { ModuleKey = "System", ModuleName = "系统", PageKey = "System.LoginLog", DisplayName = "登录日志", Description = "查看登录审计日志。", DefaultRoleCodes = "Am", RecommendedRoles = "Am", RiskLevel = "中", SortOrder = 30, IsEnabled = true, CreateTime = DateTime.Now }
            };
        }
    }
}