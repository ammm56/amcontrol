using AM.Core.Base;
using AM.Core.Context;
using AM.DBService.Services.Motion.Point;
using AM.DBService.Services.Motion.Topology;
using AM.Model.Common;
using AM.Model.Entity.Motion.Point;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.Motion.Assembly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Motion.Assembly
{
    /// <summary>
    /// 装配接线工作台查询服务。
    /// 当前阶段负责聚合 IO 映射、IO 公共配置、接线信息与运行时状态。
    /// </summary>
    public class AssemblyWiringQueryService : ServiceBase, IAssemblyWiringQueryService
    {
        private readonly MotionIoMapCrudService _ioMapCrudService;
        private readonly MotionIoPointConfigCrudService _ioPointConfigCrudService;
        private readonly MotionIoWiringCrudService _ioWiringCrudService;
        private readonly MotionCardCrudService _motionCardCrudService;

        protected override string MessageSourceName
        {
            get { return "AssemblyWiringQuery"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public AssemblyWiringQueryService()
        {
            _ioMapCrudService = new MotionIoMapCrudService();
            _ioPointConfigCrudService = new MotionIoPointConfigCrudService();
            _ioWiringCrudService = new MotionIoWiringCrudService();
            _motionCardCrudService = new MotionCardCrudService();
        }

        /// <summary>
        /// 查询全部接线工作台数据。
        /// </summary>
        public Result<AssemblyWiringRowModel> QueryAll()
        {
            return BuildRows(null, null, null);
        }

        /// <summary>
        /// 按控制卡查询接线工作台数据。
        /// </summary>
        public Result<AssemblyWiringRowModel> QueryByCardId(short cardId)
        {
            if (cardId < 0)
            {
                return Fail<AssemblyWiringRowModel>((int)DbErrorCode.InvalidArgument, "控制卡 CardId 无效");
            }

            return BuildRows(cardId, null, null);
        }

        /// <summary>
        /// 按逻辑位与 IO 类型查询接线工作台数据。
        /// </summary>
        public Result<AssemblyWiringRowModel> QueryByLogicalBit(short logicalBit, string ioType)
        {
            if (logicalBit <= 0 || string.IsNullOrWhiteSpace(ioType))
            {
                return Fail<AssemblyWiringRowModel>((int)DbErrorCode.InvalidArgument, "逻辑位号或 IO 类型无效");
            }

            var normalizedIoType = NormalizeIoType(ioType);
            if (normalizedIoType == null)
            {
                return Fail<AssemblyWiringRowModel>((int)DbErrorCode.InvalidArgument, "IO 类型仅支持 DI 或 DO");
            }

            return BuildRows(null, logicalBit, normalizedIoType);
        }

        private Result<AssemblyWiringRowModel> BuildRows(short? cardId, short? logicalBit, string ioType)
        {
            try
            {
                var ioMapResult = _ioMapCrudService.QueryAll();
                if (!ioMapResult.Success)
                {
                    return Result<AssemblyWiringRowModel>.Fail(ioMapResult.Code, ioMapResult.Message, ioMapResult.Source);
                }

                var ioPointConfigResult = _ioPointConfigCrudService.QueryAll();
                if (!ioPointConfigResult.Success)
                {
                    return Result<AssemblyWiringRowModel>.Fail(ioPointConfigResult.Code, ioPointConfigResult.Message, ioPointConfigResult.Source);
                }

                var ioWiringResult = _ioWiringCrudService.QueryAll();
                if (!ioWiringResult.Success)
                {
                    return Result<AssemblyWiringRowModel>.Fail(ioWiringResult.Code, ioWiringResult.Message, ioWiringResult.Source);
                }

                var cardResult = _motionCardCrudService.QueryAll();
                if (!cardResult.Success)
                {
                    return Result<AssemblyWiringRowModel>.Fail(cardResult.Code, cardResult.Message, cardResult.Source);
                }

                IEnumerable<MotionIoMapEntity> query = ioMapResult.Items ?? new List<MotionIoMapEntity>();
                if (cardId.HasValue)
                {
                    query = query.Where(p => p.CardId == cardId.Value);
                }

                if (logicalBit.HasValue)
                {
                    query = query.Where(p => p.LogicalBit == logicalBit.Value);
                }

                if (!string.IsNullOrWhiteSpace(ioType))
                {
                    query = query.Where(p => string.Equals(p.IoType, ioType, StringComparison.OrdinalIgnoreCase));
                }

                var pointConfigLookup = (ioPointConfigResult.Items ?? new List<MotionIoPointConfigEntity>())
                    .GroupBy(p => BuildIoKey(p.LogicalBit, p.IoType))
                    .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

                var wiringLookup = (ioWiringResult.Items ?? new List<MotionIoWiringEntity>())
                    .GroupBy(p => p.IoMapId)
                    .ToDictionary(g => g.Key, g => g.First());

                var cardLookup = (cardResult.Items ?? new List<MotionCardEntity>())
                    .GroupBy(p => p.CardId)
                    .ToDictionary(g => g.Key, g => g.First());

                var rows = query
                    .OrderBy(p => p.CardId)
                    .ThenBy(p => p.IoType)
                    .ThenBy(p => p.LogicalBit)
                    .Select(p => BuildRow(p, pointConfigLookup, wiringLookup, cardLookup))
                    .ToList();

                return OkListLogOnly(rows, "装配接线工作台查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<AssemblyWiringRowModel>(ex, (int)DbErrorCode.QueryFailed, "装配接线工作台查询失败");
            }
        }

        private AssemblyWiringRowModel BuildRow(
            MotionIoMapEntity ioMap,
            IDictionary<string, MotionIoPointConfigEntity> pointConfigLookup,
            IDictionary<int, MotionIoWiringEntity> wiringLookup,
            IDictionary<short, MotionCardEntity> cardLookup)
        {
            MotionIoPointConfigEntity pointConfig;
            pointConfigLookup.TryGetValue(BuildIoKey(ioMap.LogicalBit, ioMap.IoType), out pointConfig);

            MotionIoWiringEntity wiring;
            wiringLookup.TryGetValue(ioMap.Id, out wiring);

            MotionCardEntity card;
            cardLookup.TryGetValue(ioMap.CardId, out card);

            bool currentValue;
            DateTime updateTime;
            var hasValue = TryGetRuntimeValue(ioMap.IoType, ioMap.LogicalBit, out currentValue, out updateTime);

            return new AssemblyWiringRowModel
            {
                IoMapId = ioMap.Id,
                IoType = ioMap.IoType,
                LogicalBit = ioMap.LogicalBit,
                CardId = ioMap.CardId,
                CardDisplayName = GetCardDisplayName(card, ioMap.CardId),
                SoftwareName = ioMap.Name ?? string.Empty,
                DisplayName = pointConfig == null ? string.Empty : pointConfig.DisplayName ?? string.Empty,
                SignalCategory = pointConfig == null ? string.Empty : pointConfig.SignalCategory ?? string.Empty,
                Description = pointConfig == null ? string.Empty : pointConfig.Description ?? string.Empty,
                Core = ioMap.Core,
                IsExtModule = ioMap.IsExtModule,
                HardwareBit = ioMap.HardwareBit,
                IsEnabled = ioMap.IsEnabled,
                TerminalBlock = wiring == null ? string.Empty : wiring.TerminalBlock ?? string.Empty,
                TerminalNo = wiring == null ? string.Empty : wiring.TerminalNo ?? string.Empty,
                ConnectorNo = wiring == null ? string.Empty : wiring.ConnectorNo ?? string.Empty,
                PinNo = wiring == null ? string.Empty : wiring.PinNo ?? string.Empty,
                WireNo = wiring == null ? string.Empty : wiring.WireNo ?? string.Empty,
                DeviceName = wiring == null ? string.Empty : wiring.DeviceName ?? string.Empty,
                DeviceTerminal = wiring == null ? string.Empty : wiring.DeviceTerminal ?? string.Empty,
                CabinetArea = wiring == null ? string.Empty : wiring.CabinetArea ?? string.Empty,
                SignalType = wiring == null ? string.Empty : wiring.SignalType ?? string.Empty,
                IsVerified = wiring != null && wiring.IsVerified,
                VerifiedBy = wiring == null ? string.Empty : wiring.VerifiedBy ?? string.Empty,
                CurrentValueText = hasValue ? (currentValue ? "ON" : "OFF") : "-",
                LastUpdateTimeText = hasValue ? updateTime.ToString("yyyy-MM-dd HH:mm:ss") : "-",
                RuntimeStatusText = hasValue ? "已刷新" : "未刷新",
                WiringStatusText = GetWiringStatusText(wiring),
                RelatedActuatorName = string.Empty,
                RelatedActuatorType = string.Empty,
                CanManualOperate = pointConfig != null && pointConfig.CanManualOperate
            };
        }

        private static string BuildIoKey(short logicalBit, string ioType)
        {
            return (ioType ?? string.Empty).ToUpperInvariant() + ":" + logicalBit;
        }

        private static string NormalizeIoType(string ioType)
        {
            if (string.IsNullOrWhiteSpace(ioType))
            {
                return null;
            }

            var value = ioType.Trim().ToUpperInvariant();
            if (value == "DI" || value == "DO")
            {
                return value;
            }

            return null;
        }

        private static string GetCardDisplayName(MotionCardEntity card, short cardId)
        {
            if (card == null)
            {
                return "控制卡 #" + cardId;
            }

            if (!string.IsNullOrWhiteSpace(card.DisplayName))
            {
                return card.DisplayName;
            }

            if (!string.IsNullOrWhiteSpace(card.Name))
            {
                return card.Name;
            }

            return "控制卡 #" + cardId;
        }

        private static string GetWiringStatusText(MotionIoWiringEntity wiring)
        {
            if (wiring == null)
            {
                return "未定义";
            }

            if (!wiring.IsVerified)
            {
                return "未核对";
            }

            return "已核对";
        }

        private static bool TryGetRuntimeValue(string ioType, short logicalBit, out bool value, out DateTime updateTime)
        {
            value = false;
            updateTime = DateTime.MinValue;

            if (string.Equals(ioType, "DI", StringComparison.OrdinalIgnoreCase))
            {
                var hasValue = RuntimeContext.Instance.MotionIo.TryGetDI(logicalBit, out value);
                if (!hasValue)
                {
                    return false;
                }

                RuntimeContext.Instance.MotionIo.TryGetDIUpdateTime(logicalBit, out updateTime);
                return true;
            }

            var hasDoValue = RuntimeContext.Instance.MotionIo.TryGetDO(logicalBit, out value);
            if (!hasDoValue)
            {
                return false;
            }

            RuntimeContext.Instance.MotionIo.TryGetDOUpdateTime(logicalBit, out updateTime);
            return true;
        }
    }
}
