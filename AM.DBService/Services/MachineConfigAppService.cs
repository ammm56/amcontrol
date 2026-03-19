using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Interfaces.DB;
using AM.Model.MotionCard;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 设备配置应用服务。
    /// 负责将数据库中的控制卡、轴、IO 与轴参数装配成运行时完整的运动控制配置。
    /// </summary>
    public class MachineConfigAppService : ServiceBase, IMachineConfigAppService
    {
        private readonly DBContext _dbContext;
        private readonly IMotionAxisConfigOverlayService _motionAxisConfigOverlayService;

        protected override string MessageSourceName
        {
            get { return "MachineConfigApp"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MachineConfigAppService()
            : this(new MotionAxisConfigOverlayService(), SystemContext.Instance.Reporter)
        {
        }

        public MachineConfigAppService(
            IMotionAxisConfigOverlayService motionAxisConfigOverlayService,
            IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
            _motionAxisConfigOverlayService = motionAxisConfigOverlayService;
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
                    typeof(MotionAxisConfigEntity));

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

                var validateResult = ValidateEntities(cardEntities, axisEntities, ioEntities);
                if (!validateResult.Success)
                {
                    return Fail<MotionCardConfig>(validateResult.Code, validateResult.Message);
                }

                var motionCards = BuildMotionCards(cardEntities, axisEntities, ioEntities);

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
            IList<MotionIoMapEntity> ioEntities)
        {
            var axisLookup = axisEntities
                .GroupBy(p => p.CardId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var ioLookup = ioEntities
                .GroupBy(p => p.CardId)
                .ToDictionary(g => g.Key, g => g.ToList());

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
                        .Select(ToIoBitMap)
                        .ToList(),
                    DOBitMaps = ioRows
                        .Where(IsDO)
                        .OrderBy(p => p.SortOrder)
                        .ThenBy(p => p.LogicalBit)
                        .Select(ToIoBitMap)
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

        private static MotionIoBitMap ToIoBitMap(MotionIoMapEntity entity)
        {
            return new MotionIoBitMap
            {
                LogicalBit = entity.LogicalBit,
                Name = entity.Name,
                Core = entity.Core,
                IsExtModule = entity.IsExtModule,
                HardwareBit = entity.HardwareBit
            };
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
            IList<MotionIoMapEntity> ioEntities)
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

            var duplicateAxisId = axisEntities
                .GroupBy(p => new { p.CardId, p.AxisId })
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateAxisId != null)
            {
                return Result.Fail(
                    (int)DbErrorCode.InvalidArgument,
                    "同一卡下 AxisId 重复: CardId=" + duplicateAxisId.Key.CardId + ", AxisId=" + duplicateAxisId.Key.AxisId,
                    ResultSource.Database);
            }

            var duplicateDI = ioEntities
                .Where(IsDI)
                .GroupBy(p => p.LogicalBit)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateDI != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "逻辑DI重复: " + duplicateDI.Key, ResultSource.Database);
            }

            var duplicateDO = ioEntities
                .Where(IsDO)
                .GroupBy(p => p.LogicalBit)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateDO != null)
            {
                return Result.Fail((int)DbErrorCode.InvalidArgument, "逻辑DO重复: " + duplicateDO.Key, ResultSource.Database);
            }

            var cardIdSet = new HashSet<short>(cardEntities.Select(p => p.CardId));

            var orphanAxis = axisEntities.FirstOrDefault(p => !cardIdSet.Contains(p.CardId));
            if (orphanAxis != null)
            {
                return Result.Fail(
                    (int)DbErrorCode.InvalidArgument,
                    "轴配置找不到所属控制卡: LogicalAxis=" + orphanAxis.LogicalAxis + ", CardId=" + orphanAxis.CardId,
                    ResultSource.Database);
            }

            var orphanIo = ioEntities.FirstOrDefault(p => !cardIdSet.Contains(p.CardId));
            if (orphanIo != null)
            {
                return Result.Fail(
                    (int)DbErrorCode.InvalidArgument,
                    "IO配置找不到所属控制卡: IoType=" + orphanIo.IoType + ", LogicalBit=" + orphanIo.LogicalBit + ", CardId=" + orphanIo.CardId,
                    ResultSource.Database);
            }

            foreach (var card in cardEntities)
            {
                var axisCount = axisEntities.Count(p => p.CardId == card.CardId);
                if (card.AxisCountNumber > 0 && axisCount > card.AxisCountNumber)
                {
                    return Result.Fail(
                        (int)DbErrorCode.InvalidArgument,
                        "轴数量超出控制卡定义: CardId=" + card.CardId,
                        ResultSource.Database);
                }
            }

            return Result.Ok("设备配置校验通过", ResultSource.Database);
        }
    }
}