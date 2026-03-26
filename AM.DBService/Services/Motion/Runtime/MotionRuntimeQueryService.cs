using AM.Core.Base;
using AM.Core.Context;
using AM.Model.Common;
using AM.Model.Model.Motion;
using AM.Model.MotionCard;
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

        public Result<MotionAxisDisplayItem> QueryAxisSnapshot()
        {
            try
            {
                var cards = ConfigContext.Instance.Config.MotionCardsConfig ?? new List<MotionCardConfig>();
                var motionHub = MachineContext.Instance.MotionHub;
                var list = new List<MotionAxisDisplayItem>();

                foreach (var card in cards.Where(x => x != null).OrderBy(x => x.InitOrder).ThenBy(x => x.CardId))
                {
                    if (card.AxisConfigs == null)
                    {
                        continue;
                    }

                    foreach (var axis in card.AxisConfigs.Where(x => x != null).OrderBy(x => x.SortOrder).ThenBy(x => x.LogicalAxis))
                    {
                        var item = new MotionAxisDisplayItem
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
                        };

                        if (motionHub != null)
                        {
                            FillAxisRuntime(item, motionHub);
                        }

                        list.Add(item);
                    }
                }

                return OkListSilent(list, "轴运行快照查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisDisplayItem>(ex, -1003, "轴运行快照查询失败");
            }
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

        private static void FillAxisRuntime(MotionAxisDisplayItem item, global::AM.Model.Interfaces.MotionCard.IMotionCardService motionHub)
        {
            var statusResult = motionHub.GetAxisStatus(item.LogicalAxis);
            if (statusResult.Success)
            {
                item.IsEnabled = statusResult.Item.IsEnabled;
                item.IsAlarm = statusResult.Item.IsAlarm;
                item.IsAtHome = statusResult.Item.IsAtHome;
                item.PositiveLimit = statusResult.Item.PositiveLimit;
                item.NegativeLimit = statusResult.Item.NegativeLimit;
                item.IsDone = statusResult.Item.IsDone;
            }

            var movingResult = motionHub.IsMoving(item.LogicalAxis);
            if (movingResult.Success)
            {
                item.IsMoving = movingResult.Item;
            }

            var cmdPulseResult = motionHub.GetCommandPosition(item.LogicalAxis);
            if (cmdPulseResult.Success)
            {
                item.CommandPositionPulse = cmdPulseResult.Item;
            }

            var encPulseResult = motionHub.GetEncoderPosition(item.LogicalAxis);
            if (encPulseResult.Success)
            {
                item.EncoderPositionPulse = encPulseResult.Item;
            }

            var cmdMmResult = motionHub.GetCommandPositionMm(item.LogicalAxis);
            if (cmdMmResult.Success)
            {
                item.CommandPositionMm = cmdMmResult.Item;
            }

            var encMmResult = motionHub.GetEncoderPositionMm(item.LogicalAxis);
            if (encMmResult.Success)
            {
                item.EncoderPositionMm = encMmResult.Item;
            }
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
            var links = new List<string>();
            var machine = MachineContext.Instance;

            if (ioType == "DI")
            {
                foreach (var item in machine.Cylinders.Values.Where(x => x != null))
                {
                    if (item.ExtendFeedbackBit == logicalBit || item.RetractFeedbackBit == logicalBit)
                    {
                        links.Add("气缸:" + GetDisplayName(item.DisplayName, item.Name));
                    }
                }

                foreach (var item in machine.Vacuums.Values.Where(x => x != null))
                {
                    if (item.VacuumFeedbackBit == logicalBit
                        || item.ReleaseFeedbackBit == logicalBit
                        || item.WorkpiecePresentBit == logicalBit)
                    {
                        links.Add("真空:" + GetDisplayName(item.DisplayName, item.Name));
                    }
                }

                foreach (var item in machine.Grippers.Values.Where(x => x != null))
                {
                    if (item.CloseFeedbackBit == logicalBit
                        || item.OpenFeedbackBit == logicalBit
                        || item.WorkpiecePresentBit == logicalBit)
                    {
                        links.Add("夹爪:" + GetDisplayName(item.DisplayName, item.Name));
                    }
                }
            }
            else
            {
                foreach (var item in machine.Cylinders.Values.Where(x => x != null))
                {
                    if (item.ExtendOutputBit == logicalBit || item.RetractOutputBit == logicalBit)
                    {
                        links.Add("气缸:" + GetDisplayName(item.DisplayName, item.Name));
                    }
                }

                foreach (var item in machine.Vacuums.Values.Where(x => x != null))
                {
                    if (item.VacuumOnOutputBit == logicalBit || item.BlowOffOutputBit == logicalBit)
                    {
                        links.Add("真空:" + GetDisplayName(item.DisplayName, item.Name));
                    }
                }

                foreach (var item in machine.Grippers.Values.Where(x => x != null))
                {
                    if (item.CloseOutputBit == logicalBit || item.OpenOutputBit == logicalBit)
                    {
                        links.Add("夹爪:" + GetDisplayName(item.DisplayName, item.Name));
                    }
                }

                foreach (var item in machine.StackLights.Values.Where(x => x != null))
                {
                    if (item.RedOutputBit == logicalBit
                        || item.YellowOutputBit == logicalBit
                        || item.GreenOutputBit == logicalBit
                        || item.BlueOutputBit == logicalBit
                        || item.BuzzerOutputBit == logicalBit)
                    {
                        links.Add("灯塔:" + GetDisplayName(item.DisplayName, item.Name));
                    }
                }
            }

            return links.Count == 0
                ? "—"
                : string.Join(" / ", links.Distinct(StringComparer.OrdinalIgnoreCase));
        }

        private static string GetDisplayName(string displayName, string name)
        {
            return string.IsNullOrWhiteSpace(displayName) ? (name ?? "未命名对象") : displayName;
        }
    }
}