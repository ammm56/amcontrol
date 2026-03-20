using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.DBService.Services.Motion.Point;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Point;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.App;
using AM.Model.Interfaces.DB.Motion.Point;
using AM.Model.MotionCard;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Motion.App
{
    /// <summary>
    /// 设备配置应用服务。
    /// 负责将数据库中的控制卡、轴、IO 与轴参数装配成运行时完整的运动控制配置。
    /// </summary>
    public class MachineConfigAppService : ServiceBase, IMachineConfigAppService
    {
        private readonly DBContext _dbContext;
        private readonly IMotionAxisConfigOverlayService _motionAxisConfigOverlayService;
        private readonly IMotionIoPointConfigCrudService _motionIoPointConfigCrudService;

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
                SystemContext.Instance.Reporter)
        {
        }

        public MachineConfigAppService(
            IMotionAxisConfigOverlayService motionAxisConfigOverlayService,
            IMotionIoPointConfigCrudService motionIoPointConfigCrudService,
            IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
            _motionAxisConfigOverlayService = motionAxisConfigOverlayService;
            _motionIoPointConfigCrudService = motionIoPointConfigCrudService;
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
                    typeof(MotionIoPointConfigEntity));

                return Ok("运动控制配置表初始化完成");
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
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.CardId)
                    .ToList();

                if (cardEntities.Count == 0)
                {
                    return OkList(new List<MotionCardConfig>(), "数据库中尚无运动控制卡配置");
                }

                var axisEntities = db.Queryable<MotionAxisEntity>()
                    .Where(p => p.IsEnabled)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalAxis)
                    .ToList();

                var ioEntities = db.Queryable<MotionIoMapEntity>()
                    .Where(p => p.IsEnabled)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();

                var ioPointConfigResult = _motionIoPointConfigCrudService.QueryAll();
                if (!ioPointConfigResult.Success && ioPointConfigResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<MotionCardConfig>(ioPointConfigResult.Code, "读取 IO 点位公共配置失败");
                }

                var ioPointConfigs = ioPointConfigResult.Success
                    ? ioPointConfigResult.Items.Where(p => p != null).ToList()
                    : new List<MotionIoPointConfigEntity>();

                var validateResult = ValidateEntities(cardEntities, axisEntities, ioEntities, ioPointConfigs);
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

                return OkList(motionCards, "读取数据库运动控制配置成功");
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

            return Ok("运行时运动控制配置重载成功");
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
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
                    CardId = cardEntity.CardId,
                    ModeParam = cardEntity.ModeParam,
                    CoreNumber = (ushort)cardEntity.CoreNumber,
                    AxisCountNumber = cardEntity.AxisCountNumber,
                    UseExtModule = cardEntity.UseExtModule,
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
                OutputMode = pointConfig == null || string.IsNullOrWhiteSpace(pointConfig.OutputMode) ? "Keep" : pointConfig.OutputMode,
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
            IList<MotionIoPointConfigEntity> ioPointConfigs)
        {
            var duplicateCardId = cardEntities
                .GroupBy(p => p.CardId)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateCardId != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "控制卡 CardId 重复: " + duplicateCardId.Key, ResultSource.Database);
            }

            var duplicateLogicalAxis = axisEntities
                .GroupBy(p => p.LogicalAxis)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateLogicalAxis != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "逻辑轴重复: " + duplicateLogicalAxis.Key, ResultSource.Database);
            }

            var duplicateIoLogicalBit = ioEntities
                .GroupBy(p => BuildIoPointKey(p.IoType, p.LogicalBit))
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateIoLogicalBit != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "逻辑IO重复: " + duplicateIoLogicalBit.Key, ResultSource.Database);
            }

            var duplicateIoPointConfig = ioPointConfigs
                .GroupBy(p => BuildIoPointKey(p))
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateIoPointConfig != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "IO点位公共配置重复: " + duplicateIoPointConfig.Key, ResultSource.Database);
            }

            var ioKeys = new HashSet<string>(ioEntities.Select(p => BuildIoPointKey(p.IoType, p.LogicalBit)), StringComparer.OrdinalIgnoreCase);
            var orphanPointConfig = ioPointConfigs.FirstOrDefault(p => !ioKeys.Contains(BuildIoPointKey(p)));
            if (orphanPointConfig != null)
            {
                return Result.Fail(
                    (int)DbErrorCode.InvalidArgument,
                    "IO点位公共配置未找到对应映射: " + orphanPointConfig.IoType + " " + orphanPointConfig.LogicalBit,
                    ResultSource.Database);
            }

            return Result.Ok("运动配置校验通过", ResultSource.Database);
        }
    }
}