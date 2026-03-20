using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Interfaces.DB;
using AM.Model.MotionCard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 轴运行时配置应用服务。
    /// 负责强类型轴配置的读取、保存，并通过统一热重载入口同步到运行时上下文。
    /// </summary>
    public class AxisConfigAppService : ServiceBase, IAxisConfigAppService
    {
        private readonly IMotionAxisConfigService _motionAxisConfigService;
        private readonly IMachineConfigReloadService _machineConfigReloadService;

        protected override string MessageSourceName
        {
            get { return "AxisConfigApp"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public AxisConfigAppService()
            : this(
                new MotionAxisConfigService(),
                new MachineConfigReloadService(),
                SystemContext.Instance.Reporter)
        {
        }

        public AxisConfigAppService(
            IMotionAxisConfigService motionAxisConfigService,
            IMachineConfigReloadService machineConfigReloadService,
            IAppReporter reporter)
            : base(reporter)
        {
            _motionAxisConfigService = motionAxisConfigService;
            _machineConfigReloadService = machineConfigReloadService;
        }

        /// <summary>
        /// 查询当前所有运行时轴配置。
        /// 数据来源：ConfigContext。
        /// </summary>
        public Result<AxisConfig> QueryAll()
        {
            var motionCards = ConfigContext.Instance.Config.MotionCardsConfig;
            if (motionCards == null)
            {
                return Warn<AxisConfig>((int)DbErrorCode.NotFound, "未找到运动控制卡配置");
            }

            var axisConfigs = motionCards
                .Where(p => p != null && p.AxisConfigs != null)
                .SelectMany(p => p.AxisConfigs)
                .Where(p => p != null)
                .Select(CloneAxisConfig)
                .ToList();

            return OkList(axisConfigs, "读取运行时轴配置成功");
        }

        /// <summary>
        /// 按逻辑轴查询当前运行时轴配置。
        /// 数据来源：ConfigContext。
        /// </summary>
        public Result<AxisConfig> QueryByLogicalAxis(short logicalAxis)
        {
            if (logicalAxis <= 0)
            {
                return Fail<AxisConfig>((int)DbErrorCode.InvalidArgument, "逻辑轴参数无效");
            }

            var axisConfig = FindAxisConfig(logicalAxis);
            if (axisConfig == null)
            {
                return Warn<AxisConfig>((int)DbErrorCode.NotFound, "未找到对应逻辑轴配置");
            }

            return Ok(CloneAxisConfig(axisConfig), "读取运行时轴配置成功");
        }

        /// <summary>
        /// 保存指定轴的运行时配置。
        /// 新流程：强类型配置写入 motion_axis_config -> 统一 ReloadAndRebuild。
        /// </summary>
        public Result Save(AxisConfig axisConfig)
        {
            if (axisConfig == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "轴配置不能为空");
            }

            if (axisConfig.LogicalAxis <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "逻辑轴参数无效");
            }

            var existing = FindAxisConfig(axisConfig.LogicalAxis);
            if (existing == null)
            {
                return Fail((int)DbErrorCode.NotFound, "未找到要保存的逻辑轴配置");
            }

            var saveTarget = CloneAxisConfig(existing);
            CopyEditableFields(axisConfig, saveTarget);

            var persistResult = PersistAxisConfigToDatabase(saveTarget);
            if (!persistResult.Success)
            {
                return persistResult;
            }

            var reloadResult = _machineConfigReloadService.ReloadAndRebuild();
            if (!reloadResult.Success)
            {
                return Fail(reloadResult.Code, "轴配置已保存到数据库，但运行时配置重建失败");
            }

            return Ok("运行时轴配置保存成功");
        }

        /// <summary>
        /// 从数据库重新加载并重建运行时配置。
        /// </summary>
        public Result ReloadFromDatabase()
        {
            var reloadResult = _machineConfigReloadService.ReloadAndRebuild();
            if (!reloadResult.Success)
            {
                return Fail(reloadResult.Code, "数据库参数重新加载失败");
            }

            return Ok("数据库参数重新加载成功");
        }

        /// <summary>
        /// 查找指定逻辑轴的配置。
        /// </summary>
        private static AxisConfig FindAxisConfig(short logicalAxis)
        {
            var motionCards = ConfigContext.Instance.Config.MotionCardsConfig;
            if (motionCards == null)
            {
                return null;
            }

            foreach (var card in motionCards)
            {
                if (card == null || card.AxisConfigs == null)
                {
                    continue;
                }

                var axis = card.AxisConfigs.FirstOrDefault(p => p != null && p.LogicalAxis == logicalAxis);
                if (axis != null)
                {
                    return axis;
                }
            }

            return null;
        }

        /// <summary>
        /// 复制允许编辑的字段。
        /// </summary>
        private static void CopyEditableFields(AxisConfig source, AxisConfig target)
        {
            target.Name = source.Name;
            target.PhysicalCore = source.PhysicalCore;
            target.PhysicalAxis = source.PhysicalAxis;

            target.AlarmEnabled = source.AlarmEnabled;
            target.AlarmInvert = source.AlarmInvert;
            target.EnableInvert = source.EnableInvert;
            target.PulseMode = source.PulseMode;
            target.DefaultMoveMode = source.DefaultMoveMode;
            target.EncoderExternal = source.EncoderExternal;
            target.EncoderInvert = source.EncoderInvert;
            target.LimitHomeInvert = source.LimitHomeInvert;
            target.LimitMode = source.LimitMode;
            target.TriggerEdge = source.TriggerEdge;

            target.Lead = source.Lead;
            target.PulsePerRev = source.PulsePerRev;
            target.GearRatio = source.GearRatio;

            target.DefaultVelocity = source.DefaultVelocity;
            target.JogVelocity = source.JogVelocity;

            target.Acc = source.Acc;
            target.Dec = source.Dec;
            target.SmoothTime = source.SmoothTime;
            target.HomeDeceleration = source.HomeDeceleration;
            target.NormalStopDeceleration = source.NormalStopDeceleration;
            target.EmergencyStopDeceleration = source.EmergencyStopDeceleration;

            target.StandardHomeMode = source.StandardHomeMode;
            target.ResetDirection = source.ResetDirection;
            target.HomeSearchVelocity = source.HomeSearchVelocity;
            target.IndexSearchVelocity = source.IndexSearchVelocity;
            target.HomeOffset = source.HomeOffset;
            target.HomeMaxDistance = source.HomeMaxDistance;
            target.IndexMaxDistance = source.IndexMaxDistance;
            target.EscapeStep = source.EscapeStep;
            target.IndexSearchDirection = source.IndexSearchDirection;
            target.HomeCheck = source.HomeCheck;
            target.HomeUseHomeSignal = source.HomeUseHomeSignal;
            target.HomeUseIndexSignal = source.HomeUseIndexSignal;
            target.HomeUseLimitSignal = source.HomeUseLimitSignal;
            target.HomeAutoZeroPos = source.HomeAutoZeroPos;
            target.HomeTimeoutMs = source.HomeTimeoutMs;

            target.SoftLimitEnabled = source.SoftLimitEnabled;
            target.SoftLimitPositive = source.SoftLimitPositive;
            target.SoftLimitNegative = source.SoftLimitNegative;

            target.EnableDelayMs = source.EnableDelayMs;
            target.DisableDelayMs = source.DisableDelayMs;

            target.EStopId = source.EStopId;
            target.StopId = source.StopId;
        }

        /// <summary>
        /// 将强类型轴配置保存到数据库参数表。
        /// </summary>
        private Result PersistAxisConfigToDatabase(AxisConfig axisConfig)
        {
            var queryResult = _motionAxisConfigService.QueryByLogicalAxis(axisConfig.LogicalAxis);

            var existingMap = new Dictionary<string, MotionAxisConfigEntity>(StringComparer.OrdinalIgnoreCase);
            if (queryResult.Success)
            {
                existingMap = queryResult.Items
                    .Where(p => p != null && !string.IsNullOrWhiteSpace(p.ParamName))
                    .ToDictionary(p => p.ParamName, p => p, StringComparer.OrdinalIgnoreCase);
            }
            else if (queryResult.Code != (int)DbErrorCode.NotFound)
            {
                return Fail(queryResult.Code, "读取数据库轴参数失败");
            }

            var saveRows = BuildAxisConfigRows(axisConfig)
                .Select(p =>
                {
                    MotionAxisConfigEntity existing;
                    if (existingMap.TryGetValue(p.ParamName, out existing))
                    {
                        p.Id = existing.Id;
                    }

                    return p;
                })
                .ToList();

            var saveResult = _motionAxisConfigService.SaveRange(saveRows);
            if (!saveResult.Success)
            {
                return Fail(saveResult.Code, "保存数据库轴参数失败");
            }

            return Ok("数据库轴参数保存成功");
        }

        /// <summary>
        /// 克隆轴配置，避免外部直接编辑运行时对象。
        /// </summary>
        private static AxisConfig CloneAxisConfig(AxisConfig source)
        {
            return new AxisConfig
            {
                AxisId = source.AxisId,
                Name = source.Name,
                LogicalAxis = source.LogicalAxis,
                PhysicalCore = source.PhysicalCore,
                PhysicalAxis = source.PhysicalAxis,

                AlarmEnabled = source.AlarmEnabled,
                AlarmInvert = source.AlarmInvert,
                EnableInvert = source.EnableInvert,
                PulseMode = source.PulseMode,
                DefaultMoveMode = source.DefaultMoveMode,
                EncoderExternal = source.EncoderExternal,
                EncoderInvert = source.EncoderInvert,
                LimitHomeInvert = source.LimitHomeInvert,
                LimitMode = source.LimitMode,
                TriggerEdge = source.TriggerEdge,

                Lead = source.Lead,
                PulsePerRev = source.PulsePerRev,
                GearRatio = source.GearRatio,

                DefaultVelocity = source.DefaultVelocity,
                JogVelocity = source.JogVelocity,

                Acc = source.Acc,
                Dec = source.Dec,
                SmoothTime = source.SmoothTime,
                HomeDeceleration = source.HomeDeceleration,
                NormalStopDeceleration = source.NormalStopDeceleration,
                EmergencyStopDeceleration = source.EmergencyStopDeceleration,

                StandardHomeMode = source.StandardHomeMode,
                ResetDirection = source.ResetDirection,
                HomeSearchVelocity = source.HomeSearchVelocity,
                IndexSearchVelocity = source.IndexSearchVelocity,
                HomeOffset = source.HomeOffset,
                HomeMaxDistance = source.HomeMaxDistance,
                IndexMaxDistance = source.IndexMaxDistance,
                EscapeStep = source.EscapeStep,
                IndexSearchDirection = source.IndexSearchDirection,
                HomeCheck = source.HomeCheck,
                HomeUseHomeSignal = source.HomeUseHomeSignal,
                HomeUseIndexSignal = source.HomeUseIndexSignal,
                HomeUseLimitSignal = source.HomeUseLimitSignal,
                HomeAutoZeroPos = source.HomeAutoZeroPos,
                HomeTimeoutMs = source.HomeTimeoutMs,

                SoftLimitEnabled = source.SoftLimitEnabled,
                SoftLimitPositive = source.SoftLimitPositive,
                SoftLimitNegative = source.SoftLimitNegative,

                EnableDelayMs = source.EnableDelayMs,
                DisableDelayMs = source.DisableDelayMs,

                EStopId = source.EStopId,
                StopId = source.StopId,

                IsServoOn = source.IsServoOn
            };
        }

        /// <summary>
        /// 生成数据库参数对象集合。
        /// </summary>
        private static IEnumerable<MotionAxisConfigEntity> BuildAxisConfigRows(AxisConfig axisConfig)
        {
            var logicalAxis = axisConfig.LogicalAxis;
            var axisName = axisConfig.Name;

            yield return CreateRow(logicalAxis, axisName, "AlarmEnabled", "报警使能", "Bool", axisConfig.AlarmEnabled ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "AlarmInvert", "报警取反", "Bool", axisConfig.AlarmInvert ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "EnableInvert", "使能取反", "Bool", axisConfig.EnableInvert ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "PulseMode", "脉冲模式", "Int16", axisConfig.PulseMode);
            yield return CreateRow(logicalAxis, axisName, "DefaultMoveMode", "默认运动模式", "Int16", axisConfig.DefaultMoveMode);
            yield return CreateRow(logicalAxis, axisName, "EncoderExternal", "外部编码器", "Bool", axisConfig.EncoderExternal ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "EncoderInvert", "编码器取反", "Bool", axisConfig.EncoderInvert ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "LimitHomeInvert", "限位原点取反", "Bool", axisConfig.LimitHomeInvert ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "LimitMode", "限位模式", "Int16", axisConfig.LimitMode);
            yield return CreateRow(logicalAxis, axisName, "TriggerEdge", "捕获沿", "Int16", axisConfig.TriggerEdge);

            yield return CreateRow(logicalAxis, axisName, "Lead", "导程", "Double", axisConfig.Lead);
            yield return CreateRow(logicalAxis, axisName, "PulsePerRev", "每圈脉冲数", "Int32", axisConfig.PulsePerRev);
            yield return CreateRow(logicalAxis, axisName, "GearRatio", "减速比", "Double", axisConfig.GearRatio);

            yield return CreateRow(logicalAxis, axisName, "DefaultVelocity", "默认点位速度", "Double", axisConfig.DefaultVelocity);
            yield return CreateRow(logicalAxis, axisName, "JogVelocity", "默认Jog速度", "Double", axisConfig.JogVelocity);

            yield return CreateRow(logicalAxis, axisName, "Acc", "加速度", "Double", axisConfig.Acc);
            yield return CreateRow(logicalAxis, axisName, "Dec", "减速度", "Double", axisConfig.Dec);
            yield return CreateRow(logicalAxis, axisName, "SmoothTime", "平滑时间", "Int16", axisConfig.SmoothTime);
            yield return CreateRow(logicalAxis, axisName, "HomeDeceleration", "回零减速度", "Double", axisConfig.HomeDeceleration);
            yield return CreateRow(logicalAxis, axisName, "NormalStopDeceleration", "平停减速度", "Double", axisConfig.NormalStopDeceleration);
            yield return CreateRow(logicalAxis, axisName, "EmergencyStopDeceleration", "急停减速度", "Double", axisConfig.EmergencyStopDeceleration);

            yield return CreateRow(logicalAxis, axisName, "StandardHomeMode", "标准回零模式", "Int16", axisConfig.StandardHomeMode);
            yield return CreateRow(logicalAxis, axisName, "ResetDirection", "复位运动方向", "Int16", axisConfig.ResetDirection);
            yield return CreateRow(logicalAxis, axisName, "HomeSearchVelocity", "HOME搜索速度", "Double", axisConfig.HomeSearchVelocity);
            yield return CreateRow(logicalAxis, axisName, "IndexSearchVelocity", "INDEX搜索速度", "Double", axisConfig.IndexSearchVelocity);
            yield return CreateRow(logicalAxis, axisName, "HomeOffset", "原点偏移量", "Int32", axisConfig.HomeOffset);
            yield return CreateRow(logicalAxis, axisName, "HomeMaxDistance", "HOME最大搜索距离", "Int32", axisConfig.HomeMaxDistance);
            yield return CreateRow(logicalAxis, axisName, "IndexMaxDistance", "INDEX最大搜索距离", "Int32", axisConfig.IndexMaxDistance);
            yield return CreateRow(logicalAxis, axisName, "EscapeStep", "脱离步长", "Int32", axisConfig.EscapeStep);
            yield return CreateRow(logicalAxis, axisName, "IndexSearchDirection", "INDEX搜索方向", "Int16", axisConfig.IndexSearchDirection);
            yield return CreateRow(logicalAxis, axisName, "HomeCheck", "回零自检", "Bool", axisConfig.HomeCheck ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "HomeUseHomeSignal", "使用Home信号", "Bool", axisConfig.HomeUseHomeSignal ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "HomeUseIndexSignal", "使用Index信号", "Bool", axisConfig.HomeUseIndexSignal ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "HomeUseLimitSignal", "使用限位信号", "Bool", axisConfig.HomeUseLimitSignal ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "HomeAutoZeroPos", "回零自动清零", "Bool", axisConfig.HomeAutoZeroPos ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "HomeTimeoutMs", "回零超时", "Int32", axisConfig.HomeTimeoutMs);

            yield return CreateRow(logicalAxis, axisName, "SoftLimitEnabled", "软件限位使能", "Bool", axisConfig.SoftLimitEnabled ? 1D : 0D);
            yield return CreateRow(logicalAxis, axisName, "SoftLimitPositive", "正向软件限位", "Double", axisConfig.SoftLimitPositive);
            yield return CreateRow(logicalAxis, axisName, "SoftLimitNegative", "负向软件限位", "Double", axisConfig.SoftLimitNegative);

            yield return CreateRow(logicalAxis, axisName, "EnableDelayMs", "使能前延时", "Int32", axisConfig.EnableDelayMs);
            yield return CreateRow(logicalAxis, axisName, "DisableDelayMs", "失能后延时", "Int32", axisConfig.DisableDelayMs);

            yield return CreateRow(logicalAxis, axisName, "EStopId", "急停序号", "Int32", axisConfig.EStopId);
            yield return CreateRow(logicalAxis, axisName, "StopId", "平停序号", "Int32", axisConfig.StopId);
        }

        /// <summary>
        /// 创建数据库参数对象。
        /// </summary>
        private static MotionAxisConfigEntity CreateRow(int logicalAxis, string axisName, string paramName, string displayName, string valueType, double value)
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