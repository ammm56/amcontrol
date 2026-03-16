using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Entity;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 轴运行时配置应用服务。
    /// 负责强类型轴配置的读取、保存和运行时同步。
    /// </summary>
    public class AxisConfigAppService : ServiceBase, IAxisConfigAppService
    {
        private readonly IConfigAxisArgService _configAxisArgService;
        private readonly IAxisConfigOverlayService _axisConfigOverlayService;

        /// <summary>
        /// 消息来源名称。
        /// </summary>
        protected override string MessageSourceName
        {
            get { return "AxisConfigApp"; }
        }

        /// <summary>
        /// 默认结果来源。
        /// </summary>
        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        /// <summary>
        /// 使用全局上下文初始化。
        /// </summary>
        public AxisConfigAppService()
            : this(new ConfigAxisArgService(), new AxisConfigOverlayService(), SystemContext.Instance.Reporter)
        {
        }

        /// <summary>
        /// 使用指定依赖初始化。
        /// </summary>
        public AxisConfigAppService(
            IConfigAxisArgService configAxisArgService,
            IAxisConfigOverlayService axisConfigOverlayService,
            IAppReporter reporter)
            : base(reporter)
        {
            _configAxisArgService = configAxisArgService;
            _axisConfigOverlayService = axisConfigOverlayService;
        }

        /// <summary>
        /// 查询当前所有运行时轴配置。
        /// </summary>
        public Result<AxisConfig> QueryAll()
        {
            var motionCards = ConfigContext.Instance.Config.MotionCardsConfig;
            if (motionCards == null)
                return Warn<AxisConfig>((int)DbErrorCode.NotFound, "未找到运动控制卡配置");

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
        /// </summary>
        public Result<AxisConfig> QueryByLogicalAxis(short logicalAxis)
        {
            if (logicalAxis <= 0)
                return Fail<AxisConfig>((int)DbErrorCode.InvalidArgument, "逻辑轴参数无效");

            var axisConfig = FindAxisConfig(logicalAxis);
            if (axisConfig == null)
                return Warn<AxisConfig>((int)DbErrorCode.NotFound, "未找到对应逻辑轴配置");

            return Ok(CloneAxisConfig(axisConfig), "读取运行时轴配置成功");
        }

        /// <summary>
        /// 保存指定轴的运行时配置，并同步到数据库和运行时上下文。
        /// </summary>
        public Result Save(AxisConfig axisConfig)
        {
            if (axisConfig == null)
                return Fail((int)DbErrorCode.InvalidArgument, "轴配置不能为空");

            var target = FindAxisConfig(axisConfig.LogicalAxis);
            if (target == null)
                return Fail((int)DbErrorCode.NotFound, "未找到要保存的逻辑轴配置");

            CopyEditableFields(axisConfig, target);

            var persistResult = PersistAxisConfigToDatabase(target);
            if (!persistResult.Success)
                return persistResult;

            var overlayResult = _axisConfigOverlayService.ApplyToMotionCards(ConfigContext.Instance.Config.MotionCardsConfig);
            if (!overlayResult.Success)
                return Fail(overlayResult.Code, "数据库参数覆盖运行配置失败");

            ReloadMachineAxisConfigs();

            return Ok("运行时轴配置保存成功");
        }

        /// <summary>
        /// 从数据库重新覆盖当前运行时轴配置。
        /// </summary>
        public Result ReloadFromDatabase()
        {
            var overlayResult = _axisConfigOverlayService.ApplyToMotionCards(ConfigContext.Instance.Config.MotionCardsConfig);
            if (!overlayResult.Success)
                return Fail(overlayResult.Code, "数据库参数覆盖运行配置失败");

            ReloadMachineAxisConfigs();
            return Ok("数据库参数重新加载成功");
        }

        /// <summary>
        /// 查找指定逻辑轴的配置。
        /// </summary>
        private static AxisConfig FindAxisConfig(short logicalAxis)
        {
            var motionCards = ConfigContext.Instance.Config.MotionCardsConfig;
            if (motionCards == null) return null;

            foreach (var card in motionCards)
            {
                if (card == null || card.AxisConfigs == null) continue;

                var axis = card.AxisConfigs.FirstOrDefault(p => p != null && p.LogicalAxis == logicalAxis);
                if (axis != null) return axis;
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
            var queryAllResult = _configAxisArgService.QueryAll();
            if (!queryAllResult.Success)
                return Fail(queryAllResult.Code, "读取数据库原始参数失败");

            var existingMap = queryAllResult.Items
                .Where(p => p.Axis == axisConfig.LogicalAxis)
                .ToDictionary(p => p.ParamName, p => p, StringComparer.OrdinalIgnoreCase);

            foreach (var arg in BuildAxisArgs(axisConfig))
            {
                ConfigAxisArg existing;
                if (existingMap.TryGetValue(arg.ParamName, out existing))
                {
                    arg.Id = existing.Id;
                }

                var saveResult = _configAxisArgService.Save(arg);
                if (!saveResult.Success)
                    return Fail(saveResult.Code, "保存数据库轴参数失败: " + arg.ParamName);
            }

            return Ok("数据库轴参数保存成功");
        }

        /// <summary>
        /// 重新将配置加载到已创建的控制卡服务。
        /// </summary>
        private static void ReloadMachineAxisConfigs()
        {
            var machine = MachineContext.Instance;
            var motionCardsConfig = ConfigContext.Instance.Config.MotionCardsConfig;
            if (motionCardsConfig == null) return;

            foreach (var cardConfig in motionCardsConfig)
            {
                if (cardConfig == null) continue;

                IMotionCardService motionService;
                if (machine.MotionCards.TryGetValue(cardConfig.CardId, out motionService))
                {
                    motionService.LoadAxisConfig(cardConfig.AxisConfigs);
                }
            }
        }

        /// <summary>
        /// 克隆轴配置，避免 UI 直接编辑运行时对象。
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
        private static IEnumerable<ConfigAxisArg> BuildAxisArgs(AxisConfig axisConfig)
        {
            var axis = axisConfig.LogicalAxis;

            yield return CreateArg(axis, "AlarmEnabled", "报警使能", "Bool", axisConfig.AlarmEnabled ? 1D : 0D);
            yield return CreateArg(axis, "AlarmInvert", "报警取反", "Bool", axisConfig.AlarmInvert ? 1D : 0D);
            yield return CreateArg(axis, "EnableInvert", "使能取反", "Bool", axisConfig.EnableInvert ? 1D : 0D);
            yield return CreateArg(axis, "PulseMode", "脉冲模式", "Int16", axisConfig.PulseMode);
            yield return CreateArg(axis, "DefaultMoveMode", "默认运动模式", "Int16", axisConfig.DefaultMoveMode);
            yield return CreateArg(axis, "EncoderExternal", "外部编码器", "Bool", axisConfig.EncoderExternal ? 1D : 0D);
            yield return CreateArg(axis, "EncoderInvert", "编码器取反", "Bool", axisConfig.EncoderInvert ? 1D : 0D);
            yield return CreateArg(axis, "LimitHomeInvert", "限位原点取反", "Bool", axisConfig.LimitHomeInvert ? 1D : 0D);
            yield return CreateArg(axis, "LimitMode", "限位模式", "Int16", axisConfig.LimitMode);
            yield return CreateArg(axis, "TriggerEdge", "捕获沿", "Int16", axisConfig.TriggerEdge);

            yield return CreateArg(axis, "Lead", "导程", "Double", axisConfig.Lead);
            yield return CreateArg(axis, "PulsePerRev", "每圈脉冲数", "Int32", axisConfig.PulsePerRev);
            yield return CreateArg(axis, "GearRatio", "减速比", "Double", axisConfig.GearRatio);

            yield return CreateArg(axis, "DefaultVelocity", "默认点位速度", "Double", axisConfig.DefaultVelocity);
            yield return CreateArg(axis, "JogVelocity", "默认Jog速度", "Double", axisConfig.JogVelocity);

            yield return CreateArg(axis, "Acc", "加速度", "Double", axisConfig.Acc);
            yield return CreateArg(axis, "Dec", "减速度", "Double", axisConfig.Dec);
            yield return CreateArg(axis, "SmoothTime", "平滑时间", "Int16", axisConfig.SmoothTime);
            yield return CreateArg(axis, "HomeDeceleration", "回零减速度", "Double", axisConfig.HomeDeceleration);
            yield return CreateArg(axis, "NormalStopDeceleration", "平停减速度", "Double", axisConfig.NormalStopDeceleration);
            yield return CreateArg(axis, "EmergencyStopDeceleration", "急停减速度", "Double", axisConfig.EmergencyStopDeceleration);

            yield return CreateArg(axis, "StandardHomeMode", "标准回零模式", "Int16", axisConfig.StandardHomeMode);
            yield return CreateArg(axis, "ResetDirection", "复位运动方向", "Int16", axisConfig.ResetDirection);
            yield return CreateArg(axis, "HomeSearchVelocity", "HOME搜索速度", "Double", axisConfig.HomeSearchVelocity);
            yield return CreateArg(axis, "IndexSearchVelocity", "INDEX搜索速度", "Double", axisConfig.IndexSearchVelocity);
            yield return CreateArg(axis, "HomeOffset", "原点偏移量", "Int32", axisConfig.HomeOffset);
            yield return CreateArg(axis, "HomeMaxDistance", "HOME最大搜索距离", "Int32", axisConfig.HomeMaxDistance);
            yield return CreateArg(axis, "IndexMaxDistance", "INDEX最大搜索距离", "Int32", axisConfig.IndexMaxDistance);
            yield return CreateArg(axis, "EscapeStep", "脱离步长", "Int32", axisConfig.EscapeStep);
            yield return CreateArg(axis, "IndexSearchDirection", "INDEX搜索方向", "Int16", axisConfig.IndexSearchDirection);
            yield return CreateArg(axis, "HomeCheck", "回零自检", "Bool", axisConfig.HomeCheck ? 1D : 0D);
            yield return CreateArg(axis, "HomeUseHomeSignal", "使用Home信号", "Bool", axisConfig.HomeUseHomeSignal ? 1D : 0D);
            yield return CreateArg(axis, "HomeUseIndexSignal", "使用Index信号", "Bool", axisConfig.HomeUseIndexSignal ? 1D : 0D);
            yield return CreateArg(axis, "HomeUseLimitSignal", "使用限位信号", "Bool", axisConfig.HomeUseLimitSignal ? 1D : 0D);
            yield return CreateArg(axis, "HomeAutoZeroPos", "回零自动清零", "Bool", axisConfig.HomeAutoZeroPos ? 1D : 0D);
            yield return CreateArg(axis, "HomeTimeoutMs", "回零超时", "Int32", axisConfig.HomeTimeoutMs);

            yield return CreateArg(axis, "SoftLimitEnabled", "软件限位使能", "Bool", axisConfig.SoftLimitEnabled ? 1D : 0D);
            yield return CreateArg(axis, "SoftLimitPositive", "正向软件限位", "Double", axisConfig.SoftLimitPositive);
            yield return CreateArg(axis, "SoftLimitNegative", "负向软件限位", "Double", axisConfig.SoftLimitNegative);

            yield return CreateArg(axis, "EnableDelayMs", "使能前延时", "Int32", axisConfig.EnableDelayMs);
            yield return CreateArg(axis, "DisableDelayMs", "失能后延时", "Int32", axisConfig.DisableDelayMs);

            yield return CreateArg(axis, "EStopId", "急停序号", "Int32", axisConfig.EStopId);
            yield return CreateArg(axis, "StopId", "平停序号", "Int32", axisConfig.StopId);
        }

        /// <summary>
        /// 创建数据库参数对象。
        /// </summary>
        private static ConfigAxisArg CreateArg(int axis, string name, string nameCn, string valueType, double value)
        {
            return new ConfigAxisArg
            {
                Axis = axis,
                ParamName = name,
                ParamName_Cn = nameCn,
                ParamValueType = valueType,
                ParamSetVal = value,
                ParamDefaultVal = value,
                ParamMaxVal = 0D,
                ParamMinVal = 0D
            };
        }
    }
}