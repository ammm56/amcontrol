using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.DBService.Services.Motion.Actuator;
using AM.DBService.Services.Motion.Point;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Actuator;
using AM.Model.Entity.Motion.Point;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.Actuator;
using AM.Model.Interfaces.DB.Motion.App;
using AM.Model.Interfaces.DB.Motion.Point;
using AM.Model.MotionCard;
using AM.Model.MotionCard.Actuator;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Motion.App
{
    /// <summary>
    /// 设备配置应用服务。
    /// 负责将数据库中的控制卡、轴、IO、点位公共配置与第三层对象配置装配成运行时完整配置。
    /// </summary>
    public class MachineConfigAppService : ServiceBase, IMachineConfigAppService
    {
        private readonly DBContext _dbContext;
        private readonly IMotionAxisConfigOverlayService _motionAxisConfigOverlayService;
        private readonly IMotionIoPointConfigCrudService _motionIoPointConfigCrudService;
        private readonly IMotionCylinderConfigCrudService _motionCylinderConfigCrudService;
        private readonly IMotionVacuumConfigCrudService _motionVacuumConfigCrudService;
        private readonly IMotionStackLightConfigCrudService _motionStackLightConfigCrudService;
        private readonly IMotionGripperConfigCrudService _motionGripperConfigCrudService;

        protected override string MessageSourceName
        {
            get { return "MachineConfigApp"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MachineConfigAppService()
            : this(
                new MotionAxisConfigOverlayService(),
                new MotionIoPointConfigCrudService(),
                new MotionCylinderConfigCrudService(),
                new MotionVacuumConfigCrudService(),
                new MotionStackLightConfigCrudService(),
                new MotionGripperConfigCrudService(),
                SystemContext.Instance.Reporter)
        {
        }

        public MachineConfigAppService(
            IMotionAxisConfigOverlayService motionAxisConfigOverlayService,
            IMotionIoPointConfigCrudService motionIoPointConfigCrudService,
            IMotionCylinderConfigCrudService motionCylinderConfigCrudService,
            IMotionVacuumConfigCrudService motionVacuumConfigCrudService,
            IMotionStackLightConfigCrudService motionStackLightConfigCrudService,
            IMotionGripperConfigCrudService motionGripperConfigCrudService,
            IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
            _motionAxisConfigOverlayService = motionAxisConfigOverlayService;
            _motionIoPointConfigCrudService = motionIoPointConfigCrudService;
            _motionCylinderConfigCrudService = motionCylinderConfigCrudService;
            _motionVacuumConfigCrudService = motionVacuumConfigCrudService;
            _motionStackLightConfigCrudService = motionStackLightConfigCrudService;
            _motionGripperConfigCrudService = motionGripperConfigCrudService;
        }

        public Result EnsureTables()
        {
            try
            {
                var db = CreateDb();
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

                EnsureIndexes(db);

                return OkLogOnly("运动控制配置表初始化完成");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.QueryFailed, "运动控制配置表初始化失败");
            }
        }

        public Result<MotionCardConfig> QueryAll()
        {
            try
            {
                var ensureResult = EnsureTables();
                if (!ensureResult.Success)
                {
                    return Fail<MotionCardConfig>(ensureResult.Code, ensureResult.Message);
                }

                var db = CreateDb();

                var cardEntities = db.Queryable<MotionCardEntity>()
                    .Where(p => p.IsEnabled)
                    .ToList()
                    .OrderBy(p => p.InitOrder)
                    .ThenBy(p => p.SortOrder)
                    .ThenBy(p => p.CardId)
                    .ToList();

                var axisEntities = db.Queryable<MotionAxisEntity>()
                    .Where(p => p.IsEnabled)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalAxis)
                    .ToList();

                // 加载全量 IO 映射（含禁用）→ 用于孤立检查
                var allIoEntities = db.Queryable<MotionIoMapEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();
                // 仅启用 → 用于运行时装配 DIBitMaps / DOBitMaps
                var ioEntities = allIoEntities
                    .Where(p => p.IsEnabled)
                    .ToList();

                var ioPointConfigResult = _motionIoPointConfigCrudService.QueryAll();
                if (!ioPointConfigResult.Success && ioPointConfigResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<MotionCardConfig>(ioPointConfigResult.Code, "读取 IO 点位公共配置失败");
                }

                var ioPointConfigs = ioPointConfigResult.Success
                    ? ioPointConfigResult.Items.Where(p => p != null).ToList()
                    : new List<MotionIoPointConfigEntity>();

                var cylinderConfigResult = _motionCylinderConfigCrudService.QueryAll();
                if (!cylinderConfigResult.Success && cylinderConfigResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<MotionCardConfig>(cylinderConfigResult.Code, "读取气缸对象配置失败");
                }
                var cylinderConfigs = cylinderConfigResult.Success
                    ? cylinderConfigResult.Items.Where(p => p != null && p.IsEnabled).ToList()
                    : new List<CylinderConfigEntity>();

                var vacuumConfigResult = _motionVacuumConfigCrudService.QueryAll();
                if (!vacuumConfigResult.Success && vacuumConfigResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<MotionCardConfig>(vacuumConfigResult.Code, "读取真空对象配置失败");
                }
                var vacuumConfigs = vacuumConfigResult.Success
                    ? vacuumConfigResult.Items.Where(p => p != null && p.IsEnabled).ToList()
                    : new List<VacuumConfigEntity>();

                var stackLightConfigResult = _motionStackLightConfigCrudService.QueryAll();
                if (!stackLightConfigResult.Success && stackLightConfigResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<MotionCardConfig>(stackLightConfigResult.Code, "读取灯塔对象配置失败");
                }
                var stackLightConfigs = stackLightConfigResult.Success
                    ? stackLightConfigResult.Items.Where(p => p != null && p.IsEnabled).ToList()
                    : new List<StackLightConfigEntity>();

                var gripperConfigResult = _motionGripperConfigCrudService.QueryAll();
                if (!gripperConfigResult.Success && gripperConfigResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<MotionCardConfig>(gripperConfigResult.Code, "读取夹爪对象配置失败");
                }
                var gripperConfigs = gripperConfigResult.Success
                    ? gripperConfigResult.Items.Where(p => p != null && p.IsEnabled).ToList()
                    : new List<GripperConfigEntity>();

                var validateResult = ValidateEntities(
                    cardEntities,
                    axisEntities,
                    ioEntities,
                    ioPointConfigs,
                    cylinderConfigs,
                    vacuumConfigs,
                    stackLightConfigs,
                    gripperConfigs,
                    allIoEntities);
                if (!validateResult.Success)
                {
                    return Fail<MotionCardConfig>(validateResult.Code, validateResult.Message);
                }

                var motionCards = BuildMotionCards(cardEntities, axisEntities, ioEntities, ioPointConfigs);

                var overlayResult = _motionAxisConfigOverlayService.ApplyToMotionCards(motionCards);
                if (!overlayResult.Success)
                {
                    return Fail<MotionCardConfig>(overlayResult.Code, overlayResult.Message);
                }

                var runtimeActuatorConfig = BuildActuatorConfig(
                    cylinderConfigs,
                    vacuumConfigs,
                    stackLightConfigs,
                    gripperConfigs,
                    ioEntities,
                    ioPointConfigs);
                ConfigContext.Instance.Config.ActuatorConfig = runtimeActuatorConfig;

                if (cardEntities.Count == 0)
                {
                    return OkList(new List<MotionCardConfig>(), "数据库中尚无运动控制卡配置");
                }

                return OkListLogOnly(motionCards, "读取数据库运动控制配置成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionCardConfig>(ex, (int)DbErrorCode.QueryFailed, "读取数据库运动控制配置失败");
            }
        }

        public Result ReloadFromDatabase()
        {
            var queryResult = QueryAll();
            if (!queryResult.Success)
            {
                return Fail(queryResult.Code, "数据库运动控制配置重载失败");
            }

            ConfigContext.Instance.Config.MotionCardsConfig = queryResult.Items == null
                ? new List<MotionCardConfig>()
                : queryResult.Items.ToList();

            if (ConfigContext.Instance.Config.ActuatorConfig == null)
            {
                ConfigContext.Instance.Config.ActuatorConfig = new ActuatorConfig();
            }

            return OkLogOnly("运行时运动控制配置重载成功");
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void EnsureIndexes(SqlSugarClient db)
        {
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_card_cardid ON motion_card(CardId)");
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_card_name ON motion_card(Name)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_axis_logicalaxis ON motion_axis(LogicalAxis)");
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_axis_name ON motion_axis(Name)");
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_axis_cardid_axisid ON motion_axis(CardId, AxisId)");
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_axis_cardid_core_axis ON motion_axis(CardId, PhysicalCore, PhysicalAxis)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_axis_config_axis_param ON motion_axis_config(LogicalAxis, ParamName)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_io_map_type_bit ON motion_io_map(IoType, LogicalBit)");
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_io_point_config_type_bit ON motion_io_point_config(IoType, LogicalBit)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_cylinder_config_name ON motion_cylinder_config(Name)");
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_vacuum_config_name ON motion_vacuum_config(Name)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_stacklight_config_name ON motion_stacklight_config(Name)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_gripper_config_name ON motion_gripper_config(Name)");


        }

        private static List<MotionCardConfig> BuildMotionCards(
            IList<MotionCardEntity> cardEntities,
            IList<MotionAxisEntity> axisEntities,
            IList<MotionIoMapEntity> ioEntities,
            IList<MotionIoPointConfigEntity> ioPointConfigs)
        {
            var axisLookup = axisEntities
                .GroupBy(p => p.CardId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var ioLookup = ioEntities
                .GroupBy(p => p.CardId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var ioPointConfigLookup = ioPointConfigs
                .GroupBy(BuildIoPointKey)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            var result = new List<MotionCardConfig>();

            foreach (var cardEntity in cardEntities)
            {
                List<MotionAxisEntity> axisRows;
                if (!axisLookup.TryGetValue(cardEntity.CardId, out axisRows))
                {
                    axisRows = new List<MotionAxisEntity>();
                }

                List<MotionIoMapEntity> ioRows;
                if (!ioLookup.TryGetValue(cardEntity.CardId, out ioRows))
                {
                    ioRows = new List<MotionIoMapEntity>();
                }

                var cardConfig = new MotionCardConfig
                {
                    CardType = ResolveCardType(cardEntity.CardType),
                    Name = cardEntity.Name,
                    DisplayName = string.IsNullOrWhiteSpace(cardEntity.DisplayName) ? cardEntity.Name : cardEntity.DisplayName,
                    DriverKey = cardEntity.DriverKey,
                    CardId = cardEntity.CardId,
                    ModeParam = cardEntity.ModeParam,
                    OpenConfig = cardEntity.OpenConfig,
                    CoreNumber = (ushort)cardEntity.CoreNumber,
                    AxisCountNumber = cardEntity.AxisCountNumber,
                    UseExtModule = cardEntity.UseExtModule,
                    InitOrder = cardEntity.InitOrder,
                    Description = cardEntity.Description,
                    Remark = cardEntity.Remark,
                    AxisConfigs = axisRows
                        .OrderBy(p => p.SortOrder)
                        .ThenBy(p => p.LogicalAxis)
                        .Select(ToAxisConfig)
                        .ToList(),
                    DIBitMaps = ioRows
                        .Where(IsDI)
                        .OrderBy(p => p.SortOrder)
                        .ThenBy(p => p.LogicalBit)
                        .Select(p => ToIoBitMap(p, ioPointConfigLookup))
                        .ToList(),
                    DOBitMaps = ioRows
                        .Where(IsDO)
                        .OrderBy(p => p.SortOrder)
                        .ThenBy(p => p.LogicalBit)
                        .Select(p => ToIoBitMap(p, ioPointConfigLookup))
                        .ToList()
                };

                result.Add(cardConfig);
            }

            return result;
        }

        private static AxisConfig ToAxisConfig(MotionAxisEntity entity)
        {
            return new AxisConfig
            {
                AxisId = entity.AxisId,
                Name = entity.Name,
                DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? entity.Name : entity.DisplayName,
                AxisCategory = entity.AxisCategory,
                Description = entity.Description,
                SortOrder = entity.SortOrder,
                LogicalAxis = entity.LogicalAxis,
                PhysicalCore = entity.PhysicalCore,
                PhysicalAxis = entity.PhysicalAxis
            };
        }

        private static MotionIoBitMap ToIoBitMap(
            MotionIoMapEntity entity,
            IDictionary<string, MotionIoPointConfigEntity> ioPointConfigLookup)
        {
            MotionIoPointConfigEntity pointConfig;
            ioPointConfigLookup.TryGetValue(BuildIoPointKey(entity.IoType, entity.LogicalBit), out pointConfig);

            return new MotionIoBitMap
            {
                LogicalBit = entity.LogicalBit,
                Name = entity.Name,
                DisplayName = pointConfig == null || string.IsNullOrWhiteSpace(pointConfig.DisplayName)
                    ? entity.Name
                    : pointConfig.DisplayName,
                IoType = entity.IoType,
                SignalCategory = pointConfig == null ? "Other" : pointConfig.SignalCategory,
                Core = entity.Core,
                IsExtModule = entity.IsExtModule,
                HardwareBit = entity.HardwareBit,
                Invert = pointConfig != null && pointConfig.Invert,
                IsNormallyClosed = pointConfig != null && pointConfig.IsNormallyClosed,
                DebounceMs = pointConfig == null ? 0 : pointConfig.DebounceMs,
                FilterMs = pointConfig == null ? 0 : pointConfig.FilterMs,
                CanManualOperate = pointConfig != null && pointConfig.CanManualOperate,
                DefaultOutputState = pointConfig != null && pointConfig.DefaultOutputState,
                OutputMode = pointConfig == null || string.IsNullOrWhiteSpace(pointConfig.OutputMode)
                    ? "Keep"
                    : pointConfig.OutputMode,
                PulseWidthMs = pointConfig == null ? 0 : pointConfig.PulseWidthMs,
                BlinkOnMs = pointConfig == null ? 0 : pointConfig.BlinkOnMs,
                BlinkOffMs = pointConfig == null ? 0 : pointConfig.BlinkOffMs,
                Description = pointConfig == null ? null : pointConfig.Description,
                Remark = pointConfig == null ? entity.Remark : pointConfig.Remark
            };
        }

        private static string BuildIoPointKey(MotionIoPointConfigEntity entity)
        {
            return BuildIoPointKey(entity.IoType, entity.LogicalBit);
        }

        private static string BuildIoPointKey(string ioType, short logicalBit)
        {
            return (ioType ?? string.Empty).Trim().ToUpperInvariant() + "|" + logicalBit;
        }

        private static MotionCardType ResolveCardType(int cardType)
        {
            if (Enum.IsDefined(typeof(MotionCardType), cardType))
            {
                return (MotionCardType)cardType;
            }

            return MotionCardType.Other;
        }

        private static bool IsDI(MotionIoMapEntity entity)
        {
            return string.Equals(entity.IoType, "DI", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsDO(MotionIoMapEntity entity)
        {
            return string.Equals(entity.IoType, "DO", StringComparison.OrdinalIgnoreCase);
        }

        private static Result ValidateEntities(
            IList<MotionCardEntity> cardEntities,
            IList<MotionAxisEntity> axisEntities,
            IList<MotionIoMapEntity> ioEntities,
            IList<MotionIoPointConfigEntity> ioPointConfigs,
            IList<CylinderConfigEntity> cylinderConfigs,
            IList<VacuumConfigEntity> vacuumConfigs,
            IList<StackLightConfigEntity> stackLightConfigs,
            IList<GripperConfigEntity> gripperConfigs,
            IList<MotionIoMapEntity> allIoEntities)
        {
            // ── 控制卡唯一性 ──
            var duplicateCardId = cardEntities
                .GroupBy(p => p.CardId)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateCardId != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "控制卡 CardId 重复: " + duplicateCardId.Key, ResultSource.Database);
            }
            var duplicateCardName = cardEntities
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .GroupBy(p => p.Name.Trim(), StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateCardName != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "控制卡名称重复: " + duplicateCardName.Key, ResultSource.Database);
            }

            // ── 轴唯一性 ──
            var duplicateLogicalAxis = axisEntities
                .GroupBy(p => p.LogicalAxis)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateLogicalAxis != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "逻辑轴重复: " + duplicateLogicalAxis.Key, ResultSource.Database);
            }
            var duplicateAxisName = axisEntities
                .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                .GroupBy(p => p.Name.Trim(), StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateAxisName != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "轴名称重复: " + duplicateAxisName.Key, ResultSource.Database);
            }

            // ── IO 映射唯一性 ──
            var duplicateIoLogicalBit = ioEntities
                .GroupBy(p => BuildIoPointKey(p.IoType, p.LogicalBit))
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateIoLogicalBit != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "逻辑IO重复: " + duplicateIoLogicalBit.Key, ResultSource.Database);
            }

            // ── IO 点位公共配置唯一性 ──
            var duplicateIoPointConfig = ioPointConfigs
                .GroupBy(p => BuildIoPointKey(p))
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateIoPointConfig != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "IO点位公共配置重复: " + duplicateIoPointConfig.Key, ResultSource.Database);
            }

            // ── IO 点位公共配置孤立检查 ──
            // ioKeys：仅启用的 IO 映射，供执行器 IO 位引用校验使用
            var ioKeys = new HashSet<string>(
                ioEntities.Select(p => BuildIoPointKey(p.IoType, p.LogicalBit)),
                StringComparer.OrdinalIgnoreCase);

            // allIoKeys：包含禁用条目，避免将已禁用 IO 的公共配置误判为孤立
            var allIoKeys = new HashSet<string>(
                allIoEntities.Select(p => BuildIoPointKey(p.IoType, p.LogicalBit)),
                StringComparer.OrdinalIgnoreCase);

            var orphanPointConfig = ioPointConfigs.FirstOrDefault(p => !allIoKeys.Contains(BuildIoPointKey(p)));
            if (orphanPointConfig != null)
            {
                return Result.Fail(
                    (int)DbErrorCode.InvalidArgument,
                    "IO点位公共配置未找到对应映射: " + orphanPointConfig.IoType + " " + orphanPointConfig.LogicalBit,
                    ResultSource.Database);
            }

            // ── 气缸对象验证（引用的 IO 位须在启用集合中） ──
            var duplicateCylinderName = cylinderConfigs
                .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateCylinderName != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "气缸名称重复: " + duplicateCylinderName.Key, ResultSource.Database);
            }
            foreach (var cylinder in cylinderConfigs.Where(p => p != null))
            {
                if (string.IsNullOrWhiteSpace(cylinder.Name))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "存在未配置名称的气缸对象", ResultSource.Database);
                }

                if (!ioKeys.Contains(BuildIoPointKey("DO", cylinder.ExtendOutputBit)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "气缸伸出输出位不存在: " + cylinder.ExtendOutputBit, ResultSource.Database);
                }

                if (cylinder.RetractOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", cylinder.RetractOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "气缸缩回输出位不存在: " + cylinder.RetractOutputBit.Value, ResultSource.Database);
                }

                if (cylinder.ExtendFeedbackBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", cylinder.ExtendFeedbackBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "气缸伸出反馈位不存在: " + cylinder.ExtendFeedbackBit.Value, ResultSource.Database);
                }

                if (cylinder.RetractFeedbackBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", cylinder.RetractFeedbackBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "气缸缩回反馈位不存在: " + cylinder.RetractFeedbackBit.Value, ResultSource.Database);
                }
            }

            // ── 真空对象验证 ──
            var duplicateVacuumName = vacuumConfigs
                .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateVacuumName != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "真空名称重复: " + duplicateVacuumName.Key, ResultSource.Database);
            }
            foreach (var vacuum in vacuumConfigs.Where(p => p != null))
            {
                if (string.IsNullOrWhiteSpace(vacuum.Name))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "存在未配置名称的真空对象", ResultSource.Database);
                }

                if (!ioKeys.Contains(BuildIoPointKey("DO", vacuum.VacuumOnOutputBit)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "真空吸附输出位不存在: " + vacuum.VacuumOnOutputBit, ResultSource.Database);
                }

                if (vacuum.BlowOffOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", vacuum.BlowOffOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "真空破真空输出位不存在: " + vacuum.BlowOffOutputBit.Value, ResultSource.Database);
                }

                if (vacuum.VacuumFeedbackBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", vacuum.VacuumFeedbackBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "真空建立反馈位不存在: " + vacuum.VacuumFeedbackBit.Value, ResultSource.Database);
                }

                if (vacuum.ReleaseFeedbackBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", vacuum.ReleaseFeedbackBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "真空释放反馈位不存在: " + vacuum.ReleaseFeedbackBit.Value, ResultSource.Database);
                }

                if (vacuum.WorkpiecePresentBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", vacuum.WorkpiecePresentBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "真空工件检测位不存在: " + vacuum.WorkpiecePresentBit.Value, ResultSource.Database);
                }
            }

            // ── 灯塔对象验证 ──
            var duplicateStackLightName = stackLightConfigs
                .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateStackLightName != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "灯塔名称重复: " + duplicateStackLightName.Key, ResultSource.Database);
            }
            foreach (var stackLight in stackLightConfigs.Where(p => p != null))
            {
                if (string.IsNullOrWhiteSpace(stackLight.Name))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "存在未配置名称的灯塔对象", ResultSource.Database);
                }

                if (stackLight.RedOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", stackLight.RedOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "灯塔红灯输出位不存在: " + stackLight.RedOutputBit.Value, ResultSource.Database);
                }

                if (stackLight.YellowOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", stackLight.YellowOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "灯塔黄灯输出位不存在: " + stackLight.YellowOutputBit.Value, ResultSource.Database);
                }

                if (stackLight.GreenOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", stackLight.GreenOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "灯塔绿灯输出位不存在: " + stackLight.GreenOutputBit.Value, ResultSource.Database);
                }

                if (stackLight.BlueOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", stackLight.BlueOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "灯塔蓝灯输出位不存在: " + stackLight.BlueOutputBit.Value, ResultSource.Database);
                }

                if (stackLight.BuzzerOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", stackLight.BuzzerOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "灯塔蜂鸣器输出位不存在: " + stackLight.BuzzerOutputBit.Value, ResultSource.Database);
                }
            }

            // ── 夹爪对象验证 ──
            var duplicateGripperName = gripperConfigs
                .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateGripperName != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "夹爪名称重复: " + duplicateGripperName.Key, ResultSource.Database);
            }
            foreach (var gripper in gripperConfigs.Where(p => p != null))
            {
                if (string.IsNullOrWhiteSpace(gripper.Name))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "存在未配置名称的夹爪对象", ResultSource.Database);
                }

                if (!ioKeys.Contains(BuildIoPointKey("DO", gripper.CloseOutputBit)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "夹爪夹紧输出位不存在: " + gripper.CloseOutputBit, ResultSource.Database);
                }

                if (gripper.OpenOutputBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DO", gripper.OpenOutputBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "夹爪打开输出位不存在: " + gripper.OpenOutputBit.Value, ResultSource.Database);
                }

                if (gripper.CloseFeedbackBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", gripper.CloseFeedbackBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "夹爪夹紧反馈位不存在: " + gripper.CloseFeedbackBit.Value, ResultSource.Database);
                }

                if (gripper.OpenFeedbackBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", gripper.OpenFeedbackBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "夹爪打开反馈位不存在: " + gripper.OpenFeedbackBit.Value, ResultSource.Database);
                }

                if (gripper.WorkpiecePresentBit.HasValue
                    && !ioKeys.Contains(BuildIoPointKey("DI", gripper.WorkpiecePresentBit.Value)))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument, "夹爪工件检测位不存在: " + gripper.WorkpiecePresentBit.Value, ResultSource.Database);
                }
            }

            return Result.Ok("运动配置校验通过", ResultSource.Database);
        }

        private static ActuatorConfig BuildActuatorConfig(
            IList<CylinderConfigEntity> cylinderEntities,
            IList<VacuumConfigEntity> vacuumEntities,
            IList<StackLightConfigEntity> stackLightEntities,
            IList<GripperConfigEntity> gripperEntities,
            IList<MotionIoMapEntity> ioEntities,
            IList<MotionIoPointConfigEntity> ioPointConfigs)
        {
            return new ActuatorConfig
            {
                Cylinders = BuildCylinderConfigs(cylinderEntities, ioEntities, ioPointConfigs),
                Vacuums = BuildVacuumConfigs(vacuumEntities, ioEntities, ioPointConfigs),
                StackLights = BuildStackLightConfigs(stackLightEntities, ioEntities, ioPointConfigs),
                Grippers = BuildGripperConfigs(gripperEntities, ioEntities, ioPointConfigs)
            };
        }

        private static List<CylinderConfig> BuildCylinderConfigs(
            IList<CylinderConfigEntity> cylinderEntities,
            IList<MotionIoMapEntity> ioEntities,
            IList<MotionIoPointConfigEntity> ioPointConfigs)
        {
            var result = new List<CylinderConfig>();
            if (cylinderEntities == null || cylinderEntities.Count == 0)
            {
                return result;
            }

            foreach (var entity in cylinderEntities
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name))
            {
                result.Add(new CylinderConfig
                {
                    Name = entity.Name,
                    DisplayName = entity.DisplayName,
                    DriveMode = entity.DriveMode,
                    ExtendOutputBit = entity.ExtendOutputBit,
                    RetractOutputBit = entity.RetractOutputBit,
                    ExtendFeedbackBit = entity.ExtendFeedbackBit,
                    RetractFeedbackBit = entity.RetractFeedbackBit,
                    UseFeedbackCheck = entity.UseFeedbackCheck,
                    ExtendTimeoutMs = entity.ExtendTimeoutMs,
                    RetractTimeoutMs = entity.RetractTimeoutMs,
                    AlarmCodeOnExtendTimeout = entity.AlarmCodeOnExtendTimeout,
                    AlarmCodeOnRetractTimeout = entity.AlarmCodeOnRetractTimeout,
                    AllowBothOff = entity.AllowBothOff,
                    AllowBothOn = entity.AllowBothOn,
                    IsEnabled = entity.IsEnabled,
                    SortOrder = entity.SortOrder,
                    Description = entity.Description,
                    Remark = entity.Remark
                });
            }

            return result;
        }

        private static List<VacuumConfig> BuildVacuumConfigs(
            IList<VacuumConfigEntity> vacuumEntities,
            IList<MotionIoMapEntity> ioEntities,
            IList<MotionIoPointConfigEntity> ioPointConfigs)
        {
            var result = new List<VacuumConfig>();
            if (vacuumEntities == null || vacuumEntities.Count == 0)
            {
                return result;
            }

            foreach (var entity in vacuumEntities
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name))
            {
                result.Add(new VacuumConfig
                {
                    Name = entity.Name,
                    DisplayName = entity.DisplayName,
                    VacuumOnOutputBit = entity.VacuumOnOutputBit,
                    BlowOffOutputBit = entity.BlowOffOutputBit,
                    VacuumFeedbackBit = entity.VacuumFeedbackBit,
                    ReleaseFeedbackBit = entity.ReleaseFeedbackBit,
                    WorkpiecePresentBit = entity.WorkpiecePresentBit,
                    UseFeedbackCheck = entity.UseFeedbackCheck,
                    UseWorkpieceCheck = entity.UseWorkpieceCheck,
                    VacuumBuildTimeoutMs = entity.VacuumBuildTimeoutMs,
                    ReleaseTimeoutMs = entity.ReleaseTimeoutMs,
                    AlarmCodeOnBuildTimeout = entity.AlarmCodeOnBuildTimeout,
                    AlarmCodeOnReleaseTimeout = entity.AlarmCodeOnReleaseTimeout,
                    AlarmCodeOnWorkpieceLost = entity.AlarmCodeOnWorkpieceLost,
                    KeepVacuumOnAfterDetected = entity.KeepVacuumOnAfterDetected,
                    IsEnabled = entity.IsEnabled,
                    SortOrder = entity.SortOrder,
                    Description = entity.Description,
                    Remark = entity.Remark
                });
            }

            return result;
        }

        private static List<StackLightConfig> BuildStackLightConfigs(
            IList<StackLightConfigEntity> stackLightEntities,
            IList<MotionIoMapEntity> ioEntities,
            IList<MotionIoPointConfigEntity> ioPointConfigs)
        {
            var result = new List<StackLightConfig>();
            if (stackLightEntities == null || stackLightEntities.Count == 0)
            {
                return result;
            }

            foreach (var entity in stackLightEntities
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name))
            {
                result.Add(new StackLightConfig
                {
                    Name = entity.Name,
                    DisplayName = entity.DisplayName,
                    RedOutputBit = entity.RedOutputBit,
                    YellowOutputBit = entity.YellowOutputBit,
                    GreenOutputBit = entity.GreenOutputBit,
                    BlueOutputBit = entity.BlueOutputBit,
                    BuzzerOutputBit = entity.BuzzerOutputBit,
                    EnableBuzzerOnWarning = entity.EnableBuzzerOnWarning,
                    EnableBuzzerOnAlarm = entity.EnableBuzzerOnAlarm,
                    AllowMultiSegmentOn = entity.AllowMultiSegmentOn,
                    IsEnabled = entity.IsEnabled,
                    SortOrder = entity.SortOrder,
                    Description = entity.Description,
                    Remark = entity.Remark
                });
            }

            return result;
        }

        private static List<GripperConfig> BuildGripperConfigs(
    IList<GripperConfigEntity> gripperEntities,
    IList<MotionIoMapEntity> ioEntities,
    IList<MotionIoPointConfigEntity> ioPointConfigs)
        {
            var result = new List<GripperConfig>();
            if (gripperEntities == null || gripperEntities.Count == 0)
            {
                return result;
            }

            foreach (var entity in gripperEntities
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name))
            {
                result.Add(new GripperConfig
                {
                    Name = entity.Name,
                    DisplayName = entity.DisplayName,
                    DriveMode = entity.DriveMode,
                    CloseOutputBit = entity.CloseOutputBit,
                    OpenOutputBit = entity.OpenOutputBit,
                    CloseFeedbackBit = entity.CloseFeedbackBit,
                    OpenFeedbackBit = entity.OpenFeedbackBit,
                    WorkpiecePresentBit = entity.WorkpiecePresentBit,
                    UseFeedbackCheck = entity.UseFeedbackCheck,
                    UseWorkpieceCheck = entity.UseWorkpieceCheck,
                    CloseTimeoutMs = entity.CloseTimeoutMs,
                    OpenTimeoutMs = entity.OpenTimeoutMs,
                    AlarmCodeOnCloseTimeout = entity.AlarmCodeOnCloseTimeout,
                    AlarmCodeOnOpenTimeout = entity.AlarmCodeOnOpenTimeout,
                    AlarmCodeOnWorkpieceLost = entity.AlarmCodeOnWorkpieceLost,
                    AllowBothOff = entity.AllowBothOff,
                    AllowBothOn = entity.AllowBothOn,
                    IsEnabled = entity.IsEnabled,
                    SortOrder = entity.SortOrder,
                    Description = entity.Description,
                    Remark = entity.Remark
                });
            }

            return result;
        }


    }
}