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
                    typeof(StackLightConfigEntity),
                    typeof(GripperConfigEntity));

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
                var grippers = CreateDefaultGrippers();

                db.Insertable(cards).ExecuteCommand();
                db.Insertable(axes).ExecuteCommand();
                db.Insertable(ioMaps).ExecuteCommand();
                db.Insertable(ioPointConfigs).ExecuteCommand();
                db.Insertable(cylinders).ExecuteCommand();
                db.Insertable(vacuums).ExecuteCommand();
                db.Insertable(axisConfigs).ExecuteCommand();
                db.Insertable(stackLights).ExecuteCommand();
                db.Insertable(grippers).ExecuteCommand();

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
                || db.Queryable<StackLightConfigEntity>().Any()
                || db.Queryable<GripperConfigEntity>().Any();
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
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1401,
                    Name = "PickGripperClosed",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 11,
                    IsEnabled = true,
                    SortOrder = 18,
                    Remark = "测试夹爪夹紧到位反馈"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1402,
                    Name = "PickGripperOpened",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 12,
                    IsEnabled = true,
                    SortOrder = 19,
                    Remark = "测试夹爪打开到位反馈"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DI",
                    LogicalBit = 1403,
                    Name = "PickGripperWorkpiecePresent",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 13,
                    IsEnabled = true,
                    SortOrder = 20,
                    Remark = "测试夹爪工件检测"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2401,
                    Name = "PickGripperClose",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 11,
                    IsEnabled = true,
                    SortOrder = 21,
                    Remark = "测试夹爪夹紧输出"
                },
                new MotionIoMapEntity
                {
                    CardId = 0,
                    IoType = "DO",
                    LogicalBit = 2402,
                    Name = "PickGripperOpen",
                    Core = 1,
                    IsExtModule = false,
                    HardwareBit = 12,
                    IsEnabled = true,
                    SortOrder = 22,
                    Remark = "测试夹爪打开输出"
                },
                // ── 安全 IO — DI ──
                new MotionIoMapEntity
                {
                    CardId = 0, IoType = "DI", LogicalBit = 1501, Name = "EStop",
                    Core = 1, IsExtModule = false, HardwareBit = 14, IsEnabled = true,
                    SortOrder = 23, Remark = "急停按钮（NC）"
                },
                new MotionIoMapEntity
                {
                    CardId = 0, IoType = "DI", LogicalBit = 1502, Name = "SafetyDoor1",
                    Core = 1, IsExtModule = false, HardwareBit = 15, IsEnabled = true,
                    SortOrder = 24, Remark = "安全门1（NC）"
                },
                new MotionIoMapEntity
                {
                    CardId = 0, IoType = "DI", LogicalBit = 1503, Name = "LightCurtain1",
                    Core = 1, IsExtModule = false, HardwareBit = 16, IsEnabled = true,
                    SortOrder = 25, Remark = "正面光幕（NC）"
                },
                new MotionIoMapEntity
                {
                    CardId = 0, IoType = "DI", LogicalBit = 1504, Name = "LightCurtain2",
                    Core = 1, IsExtModule = false, HardwareBit = 17, IsEnabled = true,
                    SortOrder = 26, Remark = "侧面光幕（NC）"
                },
                new MotionIoMapEntity
                {
                    CardId = 0, IoType = "DI", LogicalBit = 1505, Name = "SafetyMat1",
                    Core = 1, IsExtModule = false, HardwareBit = 18, IsEnabled = true,
                    SortOrder = 27, Remark = "安全地毯（NC）"
                },
                // ── 安全 IO — DO ──
                new MotionIoMapEntity
                {
                    CardId = 0, IoType = "DO", LogicalBit = 2501, Name = "SafetyRelayReset",
                    Core = 1, IsExtModule = false, HardwareBit = 13, IsEnabled = true,
                    SortOrder = 28, Remark = "安全继电器复位脉冲输出"
                },
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
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1401,
                    DisplayName = "夹爪夹紧到位",
                    SignalCategory = "Gripper",
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
                    Description = "测试夹爪夹紧到位反馈输入",
                    Remark = "默认夹爪 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1402,
                    DisplayName = "夹爪打开到位",
                    SignalCategory = "Gripper",
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
                    Description = "测试夹爪打开到位反馈输入",
                    Remark = "默认夹爪 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI",
                    LogicalBit = 1403,
                    DisplayName = "夹爪工件检测",
                    SignalCategory = "Gripper",
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
                    Description = "测试夹爪工件检测输入",
                    Remark = "默认夹爪 DI 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2401,
                    DisplayName = "夹爪夹紧输出",
                    SignalCategory = "Gripper",
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
                    Description = "测试夹爪夹紧控制输出",
                    Remark = "默认夹爪 DO 点位公共配置"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO",
                    LogicalBit = 2402,
                    DisplayName = "夹爪打开输出",
                    SignalCategory = "Gripper",
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
                    Description = "测试夹爪打开控制输出",
                    Remark = "默认夹爪 DO 点位公共配置"
                },
                // ── 安全 IO 点位公共配置 ──
                new MotionIoPointConfigEntity
                {
                    IoType = "DI", LogicalBit = 1501, DisplayName = "急停按钮",
                    SignalCategory = "Safety", Invert = false, IsNormallyClosed = true,
                    DebounceMs = 5, FilterMs = 0, CanManualOperate = false,
                    DefaultOutputState = false, OutputMode = "Keep",
                    PulseWidthMs = 0, BlinkOnMs = 0, BlinkOffMs = 0,
                    Description = "急停按钮，NC 接线，触发后立即停机", Remark = "安全 DI"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI", LogicalBit = 1502, DisplayName = "安全门1",
                    SignalCategory = "Safety", Invert = false, IsNormallyClosed = true,
                    DebounceMs = 10, FilterMs = 0, CanManualOperate = false,
                    DefaultOutputState = false, OutputMode = "Keep",
                    PulseWidthMs = 0, BlinkOnMs = 0, BlinkOffMs = 0,
                    Description = "安全门1，NC 接线，开门时禁止运动", Remark = "安全 DI"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI", LogicalBit = 1503, DisplayName = "正面光幕",
                    SignalCategory = "Safety", Invert = false, IsNormallyClosed = true,
                    DebounceMs = 5, FilterMs = 0, CanManualOperate = false,
                    DefaultOutputState = false, OutputMode = "Keep",
                    PulseWidthMs = 0, BlinkOnMs = 0, BlinkOffMs = 0,
                    Description = "正面安全光幕，NC 接线，遮光时触发停机", Remark = "安全 DI"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI", LogicalBit = 1504, DisplayName = "侧面光幕",
                    SignalCategory = "Safety", Invert = false, IsNormallyClosed = true,
                    DebounceMs = 5, FilterMs = 0, CanManualOperate = false,
                    DefaultOutputState = false, OutputMode = "Keep",
                    PulseWidthMs = 0, BlinkOnMs = 0, BlinkOffMs = 0,
                    Description = "侧面安全光幕，NC 接线，遮光时触发停机", Remark = "安全 DI"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DI", LogicalBit = 1505, DisplayName = "安全地毯",
                    SignalCategory = "Safety", Invert = false, IsNormallyClosed = true,
                    DebounceMs = 10, FilterMs = 0, CanManualOperate = false,
                    DefaultOutputState = false, OutputMode = "Keep",
                    PulseWidthMs = 0, BlinkOnMs = 0, BlinkOffMs = 0,
                    Description = "安全地毯，NC 接线，踩踏时触发停机", Remark = "安全 DI"
                },
                new MotionIoPointConfigEntity
                {
                    IoType = "DO", LogicalBit = 2501, DisplayName = "安全继电器复位",
                    SignalCategory = "Safety", Invert = false, IsNormallyClosed = false,
                    DebounceMs = 0, FilterMs = 0, CanManualOperate = false,
                    DefaultOutputState = false, OutputMode = "Pulse",
                    PulseWidthMs = 300, BlinkOnMs = 0, BlinkOffMs = 0,
                    Description = "向安全继电器发送复位脉冲，报警确认后恢复使能", Remark = "安全 DO"
                },
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
            // ── 硬件信号（Hardware） ──
            yield return CreateConfig(logicalAxis, axisName, "AlarmEnabled", "报警使能", "Bool", 1D, "Hardware", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "AlarmInvert", "报警取反", "Bool", 0D, "Hardware", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "EnableInvert", "使能取反", "Bool", 0D, "Hardware", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "PulseMode", "脉冲模式", "Int16", 0D, "Hardware", "none", 0D, 3D);
            yield return CreateConfig(logicalAxis, axisName, "DefaultMoveMode", "默认运动模式", "Int16", 1D, "Hardware", "none", 0D, 2D);
            yield return CreateConfig(logicalAxis, axisName, "EncoderExternal", "外部编码器", "Bool", 0D, "Hardware", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "EncoderInvert", "编码器取反", "Bool", 0D, "Hardware", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "LimitHomeInvert", "限位原点取反", "Bool", 0D, "Hardware", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "LimitMode", "限位模式", "Int16", -1D, "Hardware", "none", -1D, 3D);
            yield return CreateConfig(logicalAxis, axisName, "TriggerEdge", "捕获沿", "Int16", 1D, "Hardware", "none", 0D, 1D);

            // ── 单位换算（Scale） ──
            yield return CreateConfig(logicalAxis, axisName, "Lead", "导程", "Double", 5D, "Scale", "mm/rev", 0.001D, 1000D);
            yield return CreateConfig(logicalAxis, axisName, "PulsePerRev", "每圈脉冲数", "Int32", 10000D, "Scale", "pulse/rev", 1D, 10000000D);
            yield return CreateConfig(logicalAxis, axisName, "GearRatio", "减速比", "Double", 1D, "Scale", "—", 0.0001D, 10000D);

            // ── 运动参数（Motion） ──
            yield return CreateConfig(logicalAxis, axisName, "DefaultVelocity", "默认点位速度", "Double", 5D, "Motion", "mm/s", 0.001D, 200D);
            yield return CreateConfig(logicalAxis, axisName, "JogVelocity", "默认Jog速度", "Double", 2D, "Motion", "mm/s", 0.001D, 100D);
            yield return CreateConfig(logicalAxis, axisName, "Acc", "加速度", "Double", 0.5D, "Motion", "g", 0.001D, 100D);
            yield return CreateConfig(logicalAxis, axisName, "Dec", "减速度", "Double", 0.5D, "Motion", "g", 0.001D, 100D);
            yield return CreateConfig(logicalAxis, axisName, "SmoothTime", "平滑时间", "Int16", 25D, "Motion", "ms", 0D, 256D);

            // ── 回零参数（Home） ──
            yield return CreateConfig(logicalAxis, axisName, "HomeDeceleration", "回零减速度", "Double", 0.5D, "Home", "g", 0.001D, 100D);
            yield return CreateConfig(logicalAxis, axisName, "StandardHomeMode", "标准回零模式", "Int16", 1D, "Home", "none", 0D, 7D);
            yield return CreateConfig(logicalAxis, axisName, "ResetDirection", "复位运动方向", "Int16", 1D, "Home", "none", 0D, 0D);   // 取值 -1/1，0/0=不限范围
            yield return CreateConfig(logicalAxis, axisName, "HomeSearchVelocity", "HOME搜索速度", "Double", 1D, "Home", "mm/s", 0.001D, 50D);
            yield return CreateConfig(logicalAxis, axisName, "IndexSearchVelocity", "INDEX搜索速度", "Double", 0.2D, "Home", "mm/s", 0.001D, 20D);
            yield return CreateConfig(logicalAxis, axisName, "HomeOffset", "原点偏移量", "Int32", 0D, "Home", "pulse", 0D, 0D);   // 可负，0/0=不限
            yield return CreateConfig(logicalAxis, axisName, "HomeMaxDistance", "HOME最大搜索距离", "Int32", 0D, "Home", "pulse", 0D, 0D);   // 0=不限
            yield return CreateConfig(logicalAxis, axisName, "IndexMaxDistance", "INDEX最大搜索距离", "Int32", 0D, "Home", "pulse", 0D, 0D);
            yield return CreateConfig(logicalAxis, axisName, "EscapeStep", "脱离步长", "Int32", 1000D, "Home", "pulse", 0D, 1000000D);
            yield return CreateConfig(logicalAxis, axisName, "IndexSearchDirection", "INDEX搜索方向", "Int16", 1D, "Home", "none", 0D, 0D);   // 取值 -1/1
            yield return CreateConfig(logicalAxis, axisName, "HomeCheck", "回零自检", "Bool", 1D, "Home", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeUseHomeSignal", "使用Home信号", "Bool", 1D, "Home", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeUseIndexSignal", "使用Index信号", "Bool", 1D, "Home", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeUseLimitSignal", "使用限位信号", "Bool", 0D, "Home", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeAutoZeroPos", "回零自动清零", "Bool", 1D, "Home", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "HomeTimeoutMs", "回零超时", "Int32", 60000D, "Home", "ms", 1000D, 600000D);

            // ── 软件限位（SoftLimit） ──
            yield return CreateConfig(logicalAxis, axisName, "SoftLimitEnabled", "软件限位使能", "Bool", 1D, "SoftLimit", "none", 0D, 1D);
            yield return CreateConfig(logicalAxis, axisName, "SoftLimitPositive", "正向软件限位", "Double", 500000D, "SoftLimit", "mm", 0D, 0D);  // 机器相关，0/0=不限
            yield return CreateConfig(logicalAxis, axisName, "SoftLimitNegative", "负向软件限位", "Double", -500000D, "SoftLimit", "mm", 0D, 0D);  // 可负，0/0=不限

            // ── 使能时序（Timing） ──
            yield return CreateConfig(logicalAxis, axisName, "EnableDelayMs", "使能前延时", "Int32", 50D, "Timing", "ms", 0D, 10000D);
            yield return CreateConfig(logicalAxis, axisName, "DisableDelayMs", "失能后延时", "Int32", 50D, "Timing", "ms", 0D, 10000D);

            // ── 安全联动（Safety） ──
            yield return CreateConfig(logicalAxis, axisName, "NormalStopDeceleration", "平停减速度", "Double", 0.5D, "Safety", "g", 0.001D, 100D);
            yield return CreateConfig(logicalAxis, axisName, "EmergencyStopDeceleration", "急停减速度", "Double", 2D, "Safety", "g", 0.001D, 100D);
            yield return CreateConfig(logicalAxis, axisName, "EStopId", "急停序号", "Int32", 0D, "Safety", "none", 0D, 999D);
            yield return CreateConfig(logicalAxis, axisName, "StopId", "平停序号", "Int32", 0D, "Safety", "none", 0D, 999D);
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

        private static List<GripperConfigEntity> CreateDefaultGrippers()
        {
            return new List<GripperConfigEntity>
            {
                new GripperConfigEntity
                {
                    Name = "PickGripper",
                    DisplayName = "取料夹爪",
                    DriveMode = "Double",
                    CloseOutputBit = 2401,
                    OpenOutputBit = 2402,
                    CloseFeedbackBit = 1401,
                    OpenFeedbackBit = 1402,
                    WorkpiecePresentBit = 1403,
                    UseFeedbackCheck = true,
                    UseWorkpieceCheck = true,
                    CloseTimeoutMs = 1500,
                    OpenTimeoutMs = 1500,
                    AlarmCodeOnCloseTimeout = 93001,
                    AlarmCodeOnOpenTimeout = 93002,
                    AlarmCodeOnWorkpieceLost = 93003,
                    AllowBothOff = false,
                    AllowBothOn = false,
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认测试取料夹爪对象",
                    Remark = "用于演示第三层夹爪对象配置"
                }
            };
        }

        private static MotionAxisConfigEntity CreateConfig(
            short logicalAxis, string axisName,
            string paramName, string displayName,
            string valueType, double value,
            string group = "Motion",
            string unit = "",
            double minValue = 0D,
            double maxValue = 0D)
        {
            return new MotionAxisConfigEntity
            {
                LogicalAxis = logicalAxis,
                AxisDisplayName = axisName,
                ParamName = paramName,
                ParamDisplayName = displayName,
                ParamGroup = group,
                ParamValueType = valueType,
                ParamSetValue = value,
                ParamDefaultValue = value,
                Unit = unit,
                ParamMinValue = minValue,
                ParamMaxValue = maxValue,
            };
        }
    }
}