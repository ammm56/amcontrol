using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Actuator;
using AM.Model.Entity.Motion.Point;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.MotionCard;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace AM.DBService.Services.Motion.App
{
    /// <summary>
    /// 运动设备配置种子服务。
    /// 启动时自动补默认虚拟卡、默认测试轴、默认 DI/DO、默认轴参数。
    /// 仅在运动配置相关表均为空时写入默认种子。
    /// </summary>
    public class MachineConfigSeedService : IMachineConfigSeedService
    {
        private readonly IAppReporter _reporter;
        private readonly DBContext _dbContext;

        public MachineConfigSeedService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MachineConfigSeedService(IAppReporter reporter)
        {
            _reporter = reporter;
            _dbContext = new DBContext();
        }

        public Result EnsureSeedData()
        {
            SqlSugarClient db = null;

            try
            {
                db = _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });

                db.CodeFirst.InitTables(
                    typeof(MotionCardEntity),
                    typeof(MotionAxisEntity),
                    typeof(MotionIoMapEntity),
                    typeof(MotionAxisConfigEntity),
                    typeof(MotionIoPointConfigEntity),
                    typeof(CylinderConfigEntity),
                    typeof(VacuumConfigEntity),
                    typeof(StackLightConfigEntity));

                if (HasAnyMotionConfigData(db))
                {
                    _reporter.Info("MachineConfigSeed", "运动配置种子已存在，跳过初始化");
                    return Result.Ok("运动配置种子已存在", ResultSource.Database);
                }

                db.Ado.BeginTran();

                var cards = CreateDefaultCards();
                var axes = CreateDefaultAxes();
                var ioMaps = CreateDefaultIoMaps();
                var ioPointConfigs = CreateDefaultIoPointConfigs();
                var cylinders = CreateDefaultCylinders();
                var axisConfigs = CreateDefaultAxisConfigs();
                var vacuums = CreateDefaultVacuums();
                var stackLights = CreateDefaultStackLights();

                db.Insertable(cards).ExecuteCommand();
                db.Insertable(axes).ExecuteCommand();
                db.Insertable(ioMaps).ExecuteCommand();
                db.Insertable(ioPointConfigs).ExecuteCommand();
                db.Insertable(cylinders).ExecuteCommand();
                db.Insertable(vacuums).ExecuteCommand();
                db.Insertable(axisConfigs).ExecuteCommand();
                db.Insertable(stackLights).ExecuteCommand();

                db.Ado.CommitTran();

                _reporter.Info("MachineConfigSeed", "默认运动配置种子初始化完成");
                return Result.Ok("默认运动配置种子初始化完成", ResultSource.Database);
            }
            catch (Exception ex)
            {
                if (db != null)
                {
                    try
                    {
                        db.Ado.RollbackTran();
                    }
                    catch
                    {
                    }
                }

                _reporter.Error("MachineConfigSeed", ex, "默认运动配置种子初始化失败");
                return Result.Fail((int)DbErrorCode.SaveFailed, "默认运动配置种子初始化失败", ResultSource.Database);
            }
        }

        private static bool HasAnyMotionConfigData(SqlSugarClient db)
        {
            return db.Queryable<MotionCardEntity>().Any()
                || db.Queryable<MotionAxisEntity>().Any()
                || db.Queryable<MotionIoMapEntity>().Any()
                || db.Queryable<MotionIoPointConfigEntity>().Any()
                || db.Queryable<CylinderConfigEntity>().Any()
                || db.Queryable<VacuumConfigEntity>().Any()
                || db.Queryable<MotionAxisConfigEntity>().Any()
                || db.Queryable<StackLightConfigEntity>().Any();
        }

        private static List<MotionCardEntity> CreateDefaultCards()
        {
            var now = DateTime.Now;

            return new List<MotionCardEntity>
            {
                new MotionCardEntity
                {
                    CardId = 0,
                    CardType = (int)MotionCardType.VIRTUAL,
                    Name = "VirtualCard-0",
                    DisplayName = "默认虚拟卡",
                    DriverKey = "Virtual.Basic",
                    ModeParam = 0,
                    OpenConfig = null,
                    CoreNumber = 1,
                    AxisCountNumber = 2,
                    UseExtModule = false,
                    InitOrder = 1,
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认虚拟运动控制卡",
                    Remark = "系统初始化默认卡",
                    CreateTime = now,
                    UpdateTime = now
                }
            };
        }

        private static List<MotionAxisEntity> CreateDefaultAxes()
        {
            var now = DateTime.Now;

            return new List<MotionAxisEntity>
            {
                new MotionAxisEntity
                {
                    CardId = 0,
                    AxisId = 1,
                    LogicalAxis = 101,
                    Name = "TestAxisX",
                    DisplayName = "测试X轴",
                    AxisCategory = "Linear",
                    PhysicalCore = 1,
                    PhysicalAxis = 1,
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认测试X轴",
                    Remark = "系统初始化默认轴",
                    CreateTime = now,
                    UpdateTime = now
                },
                new MotionAxisEntity
                {
                    CardId = 0,
                    AxisId = 2,
                    LogicalAxis = 102,
                    Name = "TestAxisY",
                    DisplayName = "测试Y轴",
                    AxisCategory = "Linear",
                    PhysicalCore = 1,
                    PhysicalAxis = 2,
                    IsEnabled = true,
                    SortOrder = 2,
                    Description = "默认测试Y轴",
                    Remark = "系统初始化默认轴",
                    CreateTime = now,
                    UpdateTime = now
                }
            };
        }

        private static List<MotionIoMapEntity> CreateDefaultIoMaps()
        {
            return new List<MotionIoMapEntity>
            {
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1001,
                    Name = "XHome",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 1,
                    IsEnabled = true,
                    SortOrder = 1,
                    Remark = "测试X轴原点信号"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1002,
                    Name = "YHome",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 2,
                    IsEnabled = true,
                    SortOrder = 2,
                    Remark = "测试Y轴原点信号"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2001,
                    Name = "XServoEnable",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 1,
                    IsEnabled = true,
                    SortOrder = 3,
                    Remark = "测试X轴使能输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2002,
                    Name = "YServoEnable",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 2,
                    IsEnabled = true,
                    SortOrder = 4,
                    Remark = "测试Y轴使能输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1101,
                    Name = "ClampCylinderExtendArrived",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 3,
                    IsEnabled = true,
                    SortOrder = 5,
                    Remark = "测试夹紧气缸伸出到位"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1102,
                    Name = "ClampCylinderRetractArrived",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 4,
                    IsEnabled = true,
                    SortOrder = 6,
                    Remark = "测试夹紧气缸缩回到位"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2101,
                    Name = "ClampCylinderExtend",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 3,
                    IsEnabled = true,
                    SortOrder = 7,
                    Remark = "测试夹紧气缸伸出输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2102,
                    Name = "ClampCylinderRetract",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 4,
                    IsEnabled = true,
                    SortOrder = 8,
                    Remark = "测试夹紧气缸缩回输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1201,
                    Name = "PickVacuumBuilt",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 5,
                    IsEnabled = true,
                    SortOrder = 9,
                    Remark = "测试真空建立反馈"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1202,
                    Name = "PickVacuumReleased",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 6,
                    IsEnabled = true,
                    SortOrder = 10,
                    Remark = "测试真空释放反馈"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1203,
                    Name = "PickVacuumWorkpiecePresent",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 7,
                    IsEnabled = true,
                    SortOrder = 11,
                    Remark = "测试真空工件检测"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2201,
                    Name = "PickVacuumOn",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 5,
                    IsEnabled = true,
                    SortOrder = 12,
                    Remark = "测试真空吸附输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2202,
                    Name = "PickVacuumBlowOff",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 6,
                    IsEnabled = true,
                    SortOrder = 13,
                    Remark = "测试真空破真空输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2301,
                    Name = "MainTowerRed",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 7,
                    IsEnabled = true,
                    SortOrder = 14,
                    Remark = "测试灯塔红灯输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2302,
                    Name = "MainTowerYellow",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 8,
                    IsEnabled = true,
                    SortOrder = 15,
                    Remark = "测试灯塔黄灯输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2303,
                    Name = "MainTowerGreen",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 9,
                    IsEnabled = true,
                    SortOrder = 16,
                    Remark = "测试灯塔绿灯输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2304,
                    Name = "MainTowerBuzzer",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 10,
                    IsEnabled = true,
                    SortOrder = 17,
                    Remark = "测试灯塔蜂鸣器输出"
                }
            };
        }

        private static List<MotionIoPointConfigEntity> CreateDefaultIoPointConfigs()
        {
            return new List<MotionIoPointConfigEntity>
            {
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1001,
                    DisplayName = "X轴原点",
                    SignalCategory = "Sensor",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 20,
                    FilterMs = 0,
                    CanManualOperate = false,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试X轴原点输入信号",
                    Remark = "默认DI点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1002,
                    DisplayName = "Y轴原点",
                    SignalCategory = "Sensor",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 20,
                    FilterMs = 0,
                    CanManualOperate = false,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试Y轴原点输入信号",
                    Remark = "默认DI点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2001,
                    DisplayName = "X轴使能输出",
                    SignalCategory = "Valve",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试X轴使能控制输出",
                    Remark = "默认DO点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2002,
                    DisplayName = "Y轴使能输出",
                    SignalCategory = "Valve",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试Y轴使能控制输出",
                    Remark = "默认DO点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1101,
                    DisplayName = "夹紧气缸伸出到位",
                    SignalCategory = "Cylinder",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 20,
                    FilterMs = 0,
                    CanManualOperate = false,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试夹紧气缸伸出到位反馈",
                    Remark = "默认气缸 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1102,
                    DisplayName = "夹紧气缸缩回到位",
                    SignalCategory = "Cylinder",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 20,
                    FilterMs = 0,
                    CanManualOperate = false,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试夹紧气缸缩回到位反馈",
                    Remark = "默认气缸 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2101,
                    DisplayName = "夹紧气缸伸出",
                    SignalCategory = "Cylinder",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试夹紧气缸伸出控制输出",
                    Remark = "默认气缸 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2102,
                    DisplayName = "夹紧气缸缩回",
                    SignalCategory = "Cylinder",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试夹紧气缸缩回控制输出",
                    Remark = "默认气缸 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1201,
                    DisplayName = "真空建立反馈",
                    SignalCategory = "Vacuum",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 20,
                    FilterMs = 0,
                    CanManualOperate = false,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试真空建立反馈输入",
                    Remark = "默认真空 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1202,
                    DisplayName = "真空释放反馈",
                    SignalCategory = "Vacuum",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 20,
                    FilterMs = 0,
                    CanManualOperate = false,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试真空释放反馈输入",
                    Remark = "默认真空 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1203,
                    DisplayName = "工件存在检测",
                    SignalCategory = "Vacuum",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 20,
                    FilterMs = 0,
                    CanManualOperate = false,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试真空工件检测输入",
                    Remark = "默认真空 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2201,
                    DisplayName = "真空吸附输出",
                    SignalCategory = "Vacuum",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试真空吸附控制输出",
                    Remark = "默认真空 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2202,
                    DisplayName = "真空破真空输出",
                    SignalCategory = "Vacuum",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试真空破真空控制输出",
                    Remark = "默认真空 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2301,
                    DisplayName = "主灯塔红灯",
                    SignalCategory = "TowerLight",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试主灯塔红灯输出",
                    Remark = "默认灯塔 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2302,
                    DisplayName = "主灯塔黄灯",
                    SignalCategory = "TowerLight",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试主灯塔黄灯输出",
                    Remark = "默认灯塔 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2303,
                    DisplayName = "主灯塔绿灯",
                    SignalCategory = "TowerLight",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试主灯塔绿灯输出",
                    Remark = "默认灯塔 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2304,
                    DisplayName = "主灯塔蜂鸣器",
                    SignalCategory = "TowerLight",
                    Invert = false,
                    IsNormallyClosed = false,
                    DebounceMs = 0,
                    FilterMs = 0,
                    CanManualOperate = true,
                    DefaultOutputState = false,
                    OutputMode = "Keep",
                    PulseWidthMs = 0,
                    BlinkOnMs = 0,
                    BlinkOffMs = 0,
                    Description = "测试主灯塔蜂鸣器输出",
                    Remark = "默认灯塔 DO 点位公共配置"
                }
            };
        }

        private static List<MotionAxisConfigEntity> CreateDefaultAxisConfigs()
        {
            var result = new List<MotionAxisConfigEntity>();
            result.AddRange(CreateAxisConfigRows((short)101, "测试X轴"));
            result.AddRange(CreateAxisConfigRows((short)102, "测试Y轴"));
            return result;
        }

        private static IEnumerable<MotionAxisConfigEntity> CreateAxisConfigRows(short logicalAxis, string axisName)
        {
            yield return CreateConfig(logicalAxis, axisName, "AlarmEnabled", "报警使能", "Bool", 1D);
            yield return CreateConfig(logicalAxis, axisName, "AlarmInvert", "报警取反", "Bool", 0D);
            yield return CreateConfig(logicalAxis, axisName, "EnableInvert", "使能取反", "Bool", 0D);
            yield return CreateConfig(logicalAxis, axisName, "PulseMode", "脉冲模式", "Int16", 0D);
            yield return CreateConfig(logicalAxis, axisName, "DefaultMoveMode", "默认运动模式", "Int16", 1D);
            yield return CreateConfig(logicalAxis, axisName, "EncoderExternal", "外部编码器", "Bool", 0D);
            yield return CreateConfig(logicalAxis, axisName, "EncoderInvert", "编码器取反", "Bool", 0D);
            yield return CreateConfig(logicalAxis, axisName, "LimitHomeInvert", "限位原点取反", "Bool", 0D);
            yield return CreateConfig(logicalAxis, axisName, "LimitMode", "限位模式", "Int16", -1D);
            yield return CreateConfig(logicalAxis, axisName, "TriggerEdge", "捕获沿", "Int16", 1D);

            yield return CreateConfig(logicalAxis, axisName, "Lead", "导程", "Double", 5D);
            yield return CreateConfig(logicalAxis, axisName, "PulsePerRev", "每圈脉冲数", "Int32", 10000D);
            yield return CreateConfig(logicalAxis, axisName, "GearRatio", "减速比", "Double", 1D);

            yield return CreateConfig(logicalAxis, axisName, "DefaultVelocity", "默认点位速度", "Double", 5D);
            yield return CreateConfig(logicalAxis, axisName, "JogVelocity", "默认Jog速度", "Double", 2D);
            yield return CreateConfig(logicalAxis, axisName, "Acc", "加速度", "Double", 0.5D);
            yield return CreateConfig(logicalAxis, axisName, "Dec", "减速度", "Double", 0.5D);
            yield return CreateConfig(logicalAxis, axisName, "SmoothTime", "平滑时间", "Int16", 25D);

            yield return CreateConfig(logicalAxis, axisName, "HomeDeceleration", "回零减速度", "Double", 0.5D);
            yield return CreateConfig(logicalAxis, axisName, "NormalStopDeceleration", "平停减速度", "Double", 0.5D);
            yield return CreateConfig(logicalAxis, axisName, "EmergencyStopDeceleration", "急停减速度", "Double", 2D);

            yield return CreateConfig(logicalAxis, axisName, "StandardHomeMode", "标准回零模式", "Int16", 1D);
            yield return CreateConfig(logicalAxis, axisName, "ResetDirection", "复位运动方向", "Int16", 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeSearchVelocity", "HOME搜索速度", "Double", 1D);
            yield return CreateConfig(logicalAxis, axisName, "IndexSearchVelocity", "INDEX搜索速度", "Double", 0.2D);
            yield return CreateConfig(logicalAxis, axisName, "HomeOffset", "原点偏移量", "Int32", 0D);
            yield return CreateConfig(logicalAxis, axisName, "HomeMaxDistance", "HOME最大搜索距离", "Int32", 0D);
            yield return CreateConfig(logicalAxis, axisName, "IndexMaxDistance", "INDEX最大搜索距离", "Int32", 0D);
            yield return CreateConfig(logicalAxis, axisName, "EscapeStep", "脱离步长", "Int32", 1000D);
            yield return CreateConfig(logicalAxis, axisName, "IndexSearchDirection", "INDEX搜索方向", "Int16", 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeCheck", "回零自检", "Bool", 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeUseHomeSignal", "使用Home信号", "Bool", 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeUseIndexSignal", "使用Index信号", "Bool", 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeUseLimitSignal", "使用限位信号", "Bool", 0D);
            yield return CreateConfig(logicalAxis, axisName, "HomeAutoZeroPos", "回零自动清零", "Bool", 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeTimeoutMs", "回零超时", "Int32", 60000D);

            yield return CreateConfig(logicalAxis, axisName, "SoftLimitEnabled", "软件限位使能", "Bool", 1D);
            yield return CreateConfig(logicalAxis, axisName, "SoftLimitPositive", "正向软件限位", "Double", 500000D);
            yield return CreateConfig(logicalAxis, axisName, "SoftLimitNegative", "负向软件限位", "Double", -500000D);

            yield return CreateConfig(logicalAxis, axisName, "EnableDelayMs", "使能前延时", "Int32", 50D);
            yield return CreateConfig(logicalAxis, axisName, "DisableDelayMs", "失能后延时", "Int32", 50D);

            yield return CreateConfig(logicalAxis, axisName, "EStopId", "急停序号", "Int32", 0D);
            yield return CreateConfig(logicalAxis, axisName, "StopId", "平停序号", "Int32", 0D);
        }

        private static List<CylinderConfigEntity> CreateDefaultCylinders()
        {
            return new List<CylinderConfigEntity>
            {
                new CylinderConfigEntity
                {
                    Name = "ClampCylinder",
                    DisplayName = "夹紧气缸",
                    DriveMode = "Double",
                    ExtendOutputBit = 2101,
                    RetractOutputBit = 2102,
                    ExtendFeedbackBit = 1101,
                    RetractFeedbackBit = 1102,
                    UseFeedbackCheck = true,
                    ExtendTimeoutMs = 1500,
                    RetractTimeoutMs = 1500,
                    AlarmCodeOnExtendTimeout = 91001,
                    AlarmCodeOnRetractTimeout = 91002,
                    AllowBothOff = false,
                    AllowBothOn = false,
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认测试夹紧气缸对象",
                    Remark = "用于演示第三层对象配置"
                }
            };
        }

        private static List<VacuumConfigEntity> CreateDefaultVacuums()
        {
            return new List<VacuumConfigEntity>
            {
                new VacuumConfigEntity
                {
                    Name = "PickVacuum",
                    DisplayName = "取料真空",
                    VacuumOnOutputBit = 2201,
                    BlowOffOutputBit = 2202,
                    VacuumFeedbackBit = 1201,
                    ReleaseFeedbackBit = 1202,
                    WorkpiecePresentBit = 1203,
                    UseFeedbackCheck = true,
                    UseWorkpieceCheck = true,
                    VacuumBuildTimeoutMs = 1500,
                    ReleaseTimeoutMs = 1000,
                    AlarmCodeOnBuildTimeout = 92001,
                    AlarmCodeOnReleaseTimeout = 92002,
                    AlarmCodeOnWorkpieceLost = 92003,
                    KeepVacuumOnAfterDetected = true,
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认测试取料真空对象",
                    Remark = "用于演示第三层真空对象配置"
                }
            };
        }

        private static List<StackLightConfigEntity> CreateDefaultStackLights()
        {
            return new List<StackLightConfigEntity>
            {
                new StackLightConfigEntity
                {
                    Name = "MainTower",
                    DisplayName = "主灯塔",
                    RedOutputBit = 2301,
                    YellowOutputBit = 2302,
                    GreenOutputBit = 2303,
                    BlueOutputBit = null,
                    BuzzerOutputBit = 2304,
                    EnableBuzzerOnWarning = false,
                    EnableBuzzerOnAlarm = true,
                    AllowMultiSegmentOn = false,
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认测试主灯塔对象",
                    Remark = "用于演示第三层灯塔对象配置"
                }
            };
        }

        private static MotionAxisConfigEntity CreateConfig(short logicalAxis, string axisName, string paramName, string displayName, string valueType, double value)
        {
            return new MotionAxisConfigEntity
            {
                LogicalAxis = logicalAxis,
                AxisDisplayName = axisName,
                ParamName = paramName,
                ParamDisplayName = displayName,
                ParamValueType = valueType,
                ParamSetValue = value,
                ParamDefaultValue = value,
                ParamMaxValue = 0D,
                ParamMinValue = 0D
            };
        }
    }
}