using AM.Core.Base;
using AM.Core.Context;
using AM.Model.Common;
using AM.Model.Motion;
using AM.Model.MotionCard;
using AM.Model.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Motion.Runtime
{
    /// <summary>
    /// Motion 运行时聚合查询服务。
    /// </summary>
    public class MotionRuntimeQueryService : ServiceBase
    {
        protected override string MessageSourceName
        {
            get { return "MotionRuntimeQuery"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public Result<MotionIoDisplayItem> QueryIoSnapshot(string ioType)
        {
            try
            {
                var normalizedIoType = NormalizeIoType(ioType);
                if (normalizedIoType == null)
                {
                    return Fail<MotionIoDisplayItem>(-1001, "IO 类型仅支持 DI 或 DO");
                }

                var cards = ConfigContext.Instance.Config.MotionCardsConfig ?? new List<MotionCardConfig>();
                var list = new List<MotionIoDisplayItem>();

                foreach (var card in cards.Where(x => x != null))
                {
                    var bitMaps = normalizedIoType == "DI" ? card.DIBitMaps : card.DOBitMaps;
                    if (bitMaps == null)
                    {
                        continue;
                    }

                    foreach (var bitMap in bitMaps.Where(x => x != null).OrderBy(x => x.LogicalBit))
                    {
                        bool currentValue;
                        DateTime updateTime;
                        var hasValue = TryGetRuntimeIoValue(normalizedIoType, bitMap.LogicalBit, out currentValue, out updateTime);

                        list.Add(new MotionIoDisplayItem
                        {
                            IoType = normalizedIoType,
                            LogicalBit = bitMap.LogicalBit,
                            Name = bitMap.Name,
                            DisplayName = bitMap.DisplayName,
                            SignalCategory = bitMap.SignalCategory,
                            SignalCategoryDisplayName = ResolveSignalCategoryDisplayName(normalizedIoType, bitMap.SignalCategory, bitMap.Name, bitMap.DisplayName),
                            CardId = card.CardId,
                            CardDisplayName = string.IsNullOrWhiteSpace(card.DisplayName) ? card.Name : card.DisplayName,
                            Core = bitMap.Core,
                            HardwareBit = bitMap.HardwareBit,
                            IsExtModule = bitMap.IsExtModule,
                            IsEnabled = true,
                            Invert = bitMap.Invert,
                            IsNormallyClosed = bitMap.IsNormallyClosed,
                            CanManualOperate = bitMap.CanManualOperate,
                            DefaultOutputState = bitMap.DefaultOutputState,
                            OutputMode = string.IsNullOrWhiteSpace(bitMap.OutputMode) ? "Keep" : bitMap.OutputMode,
                            DebounceMs = bitMap.DebounceMs,
                            FilterMs = bitMap.FilterMs,
                            Description = bitMap.Description,
                            Remark = bitMap.Remark,
                            LinkObjectName = ResolveLinkedObjects(bitMap.LogicalBit, normalizedIoType),
                            CurrentValue = hasValue && currentValue,
                            LastUpdateTime = hasValue ? (DateTime?)updateTime : null
                        });
                    }
                }

                return OkListSilent(
                    list.OrderBy(x => x.CardId).ThenBy(x => x.LogicalBit).ToList(),
                    "IO 运行快照查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoDisplayItem>(ex, -1002, "IO 运行快照查询失败");
            }
        }

        public Result<MotionCardFilterItem> QueryIoCardFilters(string ioType)
        {
            var snapshotResult = QueryIoSnapshot(ioType);
            if (!snapshotResult.Success)
            {
                return Fail<MotionCardFilterItem>(snapshotResult.Code, snapshotResult.Message);
            }

            var list = snapshotResult.Items
                .GroupBy(x => x.CardId)
                .Select(g => new MotionCardFilterItem
                {
                    CardId = g.Key,
                    DisplayName = g.Select(x => x.CardDisplayName).FirstOrDefault()
                })
                .OrderBy(x => x.CardId)
                .ToList();

            return OkListSilent(list, "IO 控制卡筛选项查询成功");
        }

        /// <summary>
        /// 仅查询轴静态结构信息，不读取控制卡实时状态。
        /// </summary>
        public Result<MotionAxisDisplayItem> QueryAxisDefinitions()
        {
            try
            {
                var cards = ConfigContext.Instance.Config.MotionCardsConfig ?? new List<MotionCardConfig>();
                var list = new List<MotionAxisDisplayItem>();

                foreach (var card in cards.Where(x => x != null).OrderBy(x => x.InitOrder).ThenBy(x => x.CardId))
                {
                    if (card.AxisConfigs == null)
                    {
                        continue;
                    }

                    foreach (var axis in card.AxisConfigs.Where(x => x != null).OrderBy(x => x.SortOrder).ThenBy(x => x.LogicalAxis))
                    {
                        list.Add(new MotionAxisDisplayItem
                        {
                            LogicalAxis = axis.LogicalAxis,
                            CardId = card.CardId,
                            AxisId = axis.AxisId,
                            PhysicalCore = axis.PhysicalCore,
                            PhysicalAxis = axis.PhysicalAxis,
                            Name = axis.Name,
                            DisplayName = axis.DisplayName,
                            AxisCategory = axis.AxisCategory,
                            CardDisplayName = string.IsNullOrWhiteSpace(card.DisplayName) ? card.Name : card.DisplayName,
                            DefaultVelocityMm = ConvertPulseVelocityToMm(axis.DefaultVelocity, axis.K),
                            JogVelocityMm = ConvertPulseVelocityToMm(axis.JogVelocity, axis.K)
                        });
                    }
                }

                return OkListSilent(list, "轴静态结构查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisDisplayItem>(ex, -1003, "轴静态结构查询失败");
            }
        }

        /// <summary>
        /// 将运行态缓存覆盖到指定轴显示项。
        /// </summary>
        public void ApplyAxisRuntime(MotionAxisDisplayItem item)
        {
            if (item == null)
            {
                return;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!RuntimeContext.Instance.MotionAxis.TryGetAxisSnapshot(item.LogicalAxis, out snapshot) || snapshot == null)
            {
                item.IsEnabled = false;
                item.IsAlarm = false;
                item.IsAtHome = false;
                item.PositiveLimit = false;
                item.NegativeLimit = false;
                item.IsDone = false;
                item.IsMoving = false;
                item.CommandPositionPulse = 0D;
                item.EncoderPositionPulse = 0D;
                item.CommandPositionMm = 0D;
                item.EncoderPositionMm = 0D;
                return;
            }

            item.IsEnabled = snapshot.IsEnabled;
            item.IsAlarm = snapshot.IsAlarm;
            item.IsAtHome = snapshot.IsAtHome;
            item.PositiveLimit = snapshot.PositiveLimit;
            item.NegativeLimit = snapshot.NegativeLimit;
            item.IsDone = snapshot.IsDone;
            item.IsMoving = snapshot.IsMoving;
            item.CommandPositionPulse = snapshot.CommandPositionPulse;
            item.EncoderPositionPulse = snapshot.EncoderPositionPulse;
            item.CommandPositionMm = snapshot.CommandPositionMm;
            item.EncoderPositionMm = snapshot.EncoderPositionMm;
        }

        /// <summary>
        /// 将运行态缓存覆盖到轴列表。
        /// </summary>
        public void ApplyAxisRuntime(IEnumerable<MotionAxisDisplayItem> items)
        {
            if (items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                ApplyAxisRuntime(item);
            }
        }

        /// <summary>
        /// 兼容入口：返回静态结构 + 运行态缓存覆盖结果。
        /// </summary>
        public Result<MotionAxisDisplayItem> QueryAxisSnapshot()
        {
            var result = QueryAxisDefinitions();
            if (!result.Success)
            {
                return result;
            }

            ApplyAxisRuntime(result.Items);
            return OkListSilent(result.Items.ToList(), "轴运行快照查询成功");
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

        private static double ConvertPulseVelocityToMm(double pulsePerMs, double k)
        {
            if (k <= 0)
            {
                return 0D;
            }

            return pulsePerMs * 1000D / k;
        }

        private static bool TryGetRuntimeIoValue(string ioType, short logicalBit, out bool value, out DateTime updateTime)
        {
            value = false;
            updateTime = DateTime.MinValue;

            var runtime = RuntimeContext.Instance.MotionIo;
            if (ioType == "DI")
            {
                if (!runtime.TryGetDI(logicalBit, out value))
                {
                    return false;
                }

                runtime.TryGetDIUpdateTime(logicalBit, out updateTime);
                return true;
            }

            if (!runtime.TryGetDO(logicalBit, out value))
            {
                return false;
            }

            runtime.TryGetDOUpdateTime(logicalBit, out updateTime);
            return true;
        }

        private static string ResolveSignalCategoryDisplayName(string ioType, string signalCategory, string name, string displayName)
        {
            var category = string.IsNullOrWhiteSpace(signalCategory) ? string.Empty : signalCategory.Trim();

            if (string.Equals(category, "Button", StringComparison.OrdinalIgnoreCase))
            {
                return "按钮信号";
            }

            if (string.Equals(category, "Safety", StringComparison.OrdinalIgnoreCase))
            {
                return "安全信号";
            }

            if (string.Equals(category, "Cylinder", StringComparison.OrdinalIgnoreCase))
            {
                return ioType == "DO" ? "气缸控制" : "气缸反馈";
            }

            if (string.Equals(category, "Vacuum", StringComparison.OrdinalIgnoreCase))
            {
                return ioType == "DO" ? "真空控制" : "真空反馈";
            }

            if (string.Equals(category, "Gripper", StringComparison.OrdinalIgnoreCase))
            {
                return ioType == "DO" ? "夹爪控制" : "夹爪反馈";
            }

            if (string.Equals(category, "StackLight", StringComparison.OrdinalIgnoreCase)
                || string.Equals(category, "AlarmLamp", StringComparison.OrdinalIgnoreCase)
                || string.Equals(category, "Buzzer", StringComparison.OrdinalIgnoreCase))
            {
                return "灯塔声光";
            }

            if (string.Equals(category, "Sensor", StringComparison.OrdinalIgnoreCase))
            {
                return "通用传感器";
            }

            if (string.Equals(category, "Valve", StringComparison.OrdinalIgnoreCase))
            {
                return "阀门控制";
            }

            var mergedText = (displayName ?? string.Empty) + " " + (name ?? string.Empty);

            if (mergedText.IndexOf("原点", StringComparison.OrdinalIgnoreCase) >= 0
                || mergedText.IndexOf("Home", StringComparison.OrdinalIgnoreCase) >= 0
                || mergedText.IndexOf("限位", StringComparison.OrdinalIgnoreCase) >= 0
                || mergedText.IndexOf("Servo", StringComparison.OrdinalIgnoreCase) >= 0
                || mergedText.IndexOf("伺服", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ioType == "DO" ? "轴控制" : "轴反馈";
            }

            return "未分类";
        }

        private static string ResolveLinkedObjects(short logicalBit, string ioType)
        {
            var actuatorConfig = ConfigContext.Instance.Config.ActuatorConfig;
            if (actuatorConfig == null)
            {
                return "—";
            }

            var names = new List<string>();

            if (actuatorConfig.Cylinders != null)
            {
                names.AddRange(actuatorConfig.Cylinders
                    .Where(x => x != null && IsIoBitMatched(x, logicalBit, ioType))
                    .Select(x => x.Name));
            }

            if (actuatorConfig.Vacuums != null)
            {
                names.AddRange(actuatorConfig.Vacuums
                    .Where(x => x != null && IsIoBitMatched(x, logicalBit, ioType))
                    .Select(x => x.Name));
            }

            if (actuatorConfig.StackLights != null)
            {
                names.AddRange(actuatorConfig.StackLights
                    .Where(x => x != null && IsIoBitMatched(x, logicalBit, ioType))
                    .Select(x => x.Name));
            }

            if (actuatorConfig.Grippers != null)
            {
                names.AddRange(actuatorConfig.Grippers
                    .Where(x => x != null && IsIoBitMatched(x, logicalBit, ioType))
                    .Select(x => x.Name));
            }

            var list = names
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            return list.Count == 0 ? "—" : string.Join(" / ", list);
        }

        private static bool IsIoBitMatched(object config, short logicalBit, string ioType)
        {
            var type = config.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property == null || property.PropertyType != typeof(short))
                {
                    continue;
                }

                var name = property.Name ?? string.Empty;
                var isDo = name.EndsWith("DoBit", StringComparison.OrdinalIgnoreCase);
                var isDi = name.EndsWith("DiBit", StringComparison.OrdinalIgnoreCase);

                if ((ioType == "DO" && !isDo) || (ioType == "DI" && !isDi))
                {
                    continue;
                }

                var value = (short)property.GetValue(config, null);
                if (value == logicalBit)
                {
                    return true;
                }
            }

            return false;
        }
    }
}