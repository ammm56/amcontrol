using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.MotionCard;
using AM.Model.Runtime;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace AM.Core.Base
{
    /// <summary>
    /// 统一处理结果对象生成、日志、消息通知、报警和异常包装的服务基类。
    /// 
    /// 设计目标：
    /// 1. 统一 Result / Result&lt;T&gt; 生成；
    /// 2. 统一日志、消息、报警输出；
    /// 3. 提供高频后台任务的重复信息节流能力；
    /// 4. 提供后台任务“首次故障 / 持续故障 / 故障恢复”的统一处理入口；
    /// 5. 提供 Motion IO 运行时缓存的公共校验与读取能力，避免各服务重复实现。
    /// </summary>
    public abstract class ServiceBase
    {
        #region 字段

        /// <summary>
        /// 统一报告器。
        /// 封装日志、消息总线、报警上报。
        /// </summary>
        protected readonly IAppReporter _reporter;

        /// <summary>
        /// 重复消息节流缓存。
        /// Key 为调用方定义的稳定标识；Value 为上次允许上报的时间。
        /// </summary>
        private readonly ConcurrentDictionary<string, DateTime> _reportThrottleTimes =
            new ConcurrentDictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 节流缓存允许的最大条目数。
        /// </summary>
        private const int ReportThrottleMaxEntries = 2048;

        /// <summary>
        /// 每累计写入一定次数后触发一次清理。
        /// </summary>
        private const int ReportThrottleCleanupTriggerCount = 128;

        /// <summary>
        /// 节流缓存保留时长。
        /// 超过该时间的键可被清理。
        /// </summary>
        private static readonly TimeSpan ReportThrottleRetention = TimeSpan.FromMinutes(30);

        /// <summary>
        /// 节流缓存写入计数。
        /// 用于低成本触发周期性清理。
        /// </summary>
        private int _reportThrottleWriteCount;

        #endregion

        #region 可重写元数据

        /// <summary>
        /// 当前服务的消息源名称。
        /// 子类可重写为更稳定、更易读的来源名。
        /// </summary>
        protected virtual string MessageSourceName
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// 当前服务默认结果来源。
        /// </summary>
        protected virtual ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        /// <summary>
        /// 当前服务关联的控制卡号。
        /// 对不依赖控制卡的服务保持 null。
        /// </summary>
        protected virtual short? MessageCardId
        {
            get { return null; }
        }

        #endregion

        #region 构造

        protected ServiceBase()
            : this(SystemContext.Instance.Reporter)
        {
        }

        protected ServiceBase(IAppReporter reporter)
        {
            _reporter = reporter;
        }

        #endregion

        #region 成功结果

        protected Result Ok(string message = "OK")
        {
            return Ok(message, ReportChannels.All);
        }

        protected Result Ok(string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result.Ok(message, DefaultResultSource)
                         .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        protected Result OkSilent(string message = "OK")
        {
            return Ok(message, ReportChannels.None);
        }

        protected Result OkLogOnly(string message = "OK")
        {
            return Ok(message, ReportChannels.Log);
        }

        protected Result OkMessageOnly(string message = "OK")
        {
            return Ok(message, ReportChannels.Message);
        }

        protected Result<T> Ok<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.All);
        }

        protected Result<T> Ok<T>(T item, string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result<T>.OkItem(item, message, DefaultResultSource)
                            .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        protected Result<T> OkSilent<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.None);
        }

        protected Result<T> OkLogOnly<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.Log);
        }

        protected Result<T> OkMessageOnly<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.Message);
        }

        protected Result<T> OkList<T>(System.Collections.Generic.IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.All);
        }

        protected Result<T> OkList<T>(System.Collections.Generic.IEnumerable<T> items, string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result<T>.OkList(items, message, DefaultResultSource)
                            .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        protected Result<T> OkListSilent<T>(System.Collections.Generic.IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.None);
        }

        protected Result<T> OkListLogOnly<T>(System.Collections.Generic.IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.Log);
        }

        protected Result<T> OkListMessageOnly<T>(System.Collections.Generic.IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.Message);
        }

        #endregion

        #region 警告结果

        /// <summary>创建警告结果，日志+消息。</summary>
        protected Result Warn(int code, string message)
        {
            return Warn(code, message, ReportChannels.All);
        }

        /// <summary>创建警告结果，支持输出渠道控制。</summary>
        protected Result Warn(int code, string message, ReportChannels channels)
        {
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId, channels);
            return Result.Fail(code, message, DefaultResultSource)
                         .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        protected Result WarnSilent(int code, string message)
        {
            return Warn(code, message, ReportChannels.None);
        }

        protected Result WarnLogOnly(int code, string message)
        {
            return Warn(code, message, ReportChannels.Log);
        }

        protected Result WarnMessageOnly(int code, string message)
        {
            return Warn(code, message, ReportChannels.Message);
        }

        protected Result<T> Warn<T>(int code, string message)
        {
            return Warn<T>(code, message, ReportChannels.All);
        }

        protected Result<T> Warn<T>(int code, string message, ReportChannels channels)
        {
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId, channels);
            return Result<T>.Fail(code, message, DefaultResultSource)
                            .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        protected Result<T> WarnSilent<T>(int code, string message)
        {
            return Warn<T>(code, message, ReportChannels.None);
        }

        protected Result<T> WarnLogOnly<T>(int code, string message)
        {
            return Warn<T>(code, message, ReportChannels.Log);
        }

        #endregion

        #region 失败结果

        /// <summary>创建失败结果，日志+消息。</summary>
        protected Result Fail(int code, string message, Exception ex = null)
        {
            return Fail(code, message, ReportChannels.All, ex);
        }

        /// <summary>创建失败结果，支持输出渠道控制。</summary>
        protected Result Fail(int code, string message, ReportChannels channels, Exception ex = null)
        {
            if (ex == null)
            {
                _reporter?.Error(MessageSourceName, message, code, MessageCardId, channels);
            }
            else
            {
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId, channels);
            }

            return Result.Fail(code, message, DefaultResultSource)
                         .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        protected Result FailSilent(int code, string message, Exception ex = null)
        {
            return Fail(code, message, ReportChannels.None, ex);
        }

        protected Result FailLogOnly(int code, string message, Exception ex = null)
        {
            return Fail(code, message, ReportChannels.Log, ex);
        }

        protected Result FailMessageOnly(int code, string message, Exception ex = null)
        {
            return Fail(code, message, ReportChannels.Message, ex);
        }

        protected Result<T> Fail<T>(int code, string message, Exception ex = null)
        {
            return Fail<T>(code, message, ReportChannels.All, ex);
        }

        protected Result<T> Fail<T>(int code, string message, ReportChannels channels, Exception ex = null)
        {
            if (ex == null)
            {
                _reporter?.Error(MessageSourceName, message, code, MessageCardId, channels);
            }
            else
            {
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId, channels);
            }

            return Result<T>.Fail(code, message, DefaultResultSource)
                            .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        protected Result<T> FailSilent<T>(int code, string message, Exception ex = null)
        {
            return Fail<T>(code, message, ReportChannels.None, ex);
        }

        protected Result<T> FailLogOnly<T>(int code, string message, Exception ex = null)
        {
            return Fail<T>(code, message, ReportChannels.Log, ex);
        }

        #endregion

        #region 通用错误包装

        protected Result HandleError(short code, string message)
        {
            return code == 0 ? Ok(message) : Fail(code, message);
        }

        protected Result<T> HandleError<T>(short code, string message)
        {
            return code == 0 ? Ok(default(T), message) : Fail<T>(code, message);
        }

        protected Result HandleException(Exception ex, int code, string message)
        {
            return Fail(code, message, ex);
        }

        protected Result<T> HandleException<T>(Exception ex, int code, string message)
        {
            return Fail<T>(code, message, ex);
        }

        /// <summary>
        /// 将已有 Result 静默转发为 Result&lt;T&gt;。
        /// 常用于状态读取类接口，把底层失败透传给调用方，但不再次放大日志/消息。
        /// </summary>
        protected Result<T> ForwardSilentFailure<T>(Result result)
        {
            if (result == null)
            {
                return FailSilent<T>((int)MotionErrorCode.Unknown, "未知失败");
            }

            return FailSilent<T>(result.Code, result.Message);
        }

        #endregion

        #region 后台任务节流与状态边沿处理

        /// <summary>
        /// 重复信息节流判断。
        /// 相同 key 在 intervalMs 窗口内只允许上报一次。
        /// </summary>
        protected bool ShouldReportRepeated(string reportKey, int intervalMs)
        {
            if (string.IsNullOrWhiteSpace(reportKey))
            {
                return true;
            }

            if (intervalMs <= 0)
            {
                return true;
            }

            DateTime now = DateTime.Now;
            DateTime lastTime;
            if (_reportThrottleTimes.TryGetValue(reportKey, out lastTime))
            {
                if ((now - lastTime).TotalMilliseconds < intervalMs)
                {
                    return false;
                }
            }

            _reportThrottleTimes[reportKey] = now;

            int currentWriteCount = Interlocked.Increment(ref _reportThrottleWriteCount);
            if (_reportThrottleTimes.Count > ReportThrottleMaxEntries
                || currentWriteCount % ReportThrottleCleanupTriggerCount == 0)
            {
                CleanupReportThrottleCache(now);
            }

            return true;
        }

        /// <summary>
        /// 按节流策略输出 WarnLogOnly。
        /// 返回 true 表示本次实际写入了日志。
        /// </summary>
        protected bool WarnLogOnlyIfRepeated(string reportKey, int code, string message, int intervalMs)
        {
            if (!ShouldReportRepeated(reportKey, intervalMs))
            {
                return false;
            }

            WarnLogOnly(code, message);
            return true;
        }

        /// <summary>
        /// 按节流策略输出 FailLogOnly。
        /// 返回 true 表示本次实际写入了日志。
        /// </summary>
        protected bool FailLogOnlyIfRepeated(string reportKey, int code, string message, int intervalMs, Exception ex = null)
        {
            if (!ShouldReportRepeated(reportKey, intervalMs))
            {
                return false;
            }

            FailLogOnly(code, message, ex);
            return true;
        }

        /// <summary>
        /// 后台任务故障边沿统一处理。
        /// 
        /// 规则：
        /// 1. 首次故障立即上报日志+消息；
        /// 2. 故障文本变化时立即再次上报；
        /// 3. 持续故障期间只按节流周期写日志，不重复发消息；
        /// 4. 如指定报警码，则仅在首次/变化时上报一次报警。
        /// </summary>
        protected void ReportBackgroundFaultIfNeeded(
            ref bool wasFaulted,
            ref string lastFaultMessage,
            string stateKey,
            int code,
            string message,
            int repeatIntervalMs,
            AlarmCode? alarmCode = null,
            AlarmLevel alarmLevel = AlarmLevel.Warning)
        {
            string finalMessage = string.IsNullOrWhiteSpace(message)
                ? "后台任务执行失败"
                : message;

            string finalStateKey = string.IsNullOrWhiteSpace(stateKey)
                ? MessageSourceName
                : stateKey;

            bool isEdgeFault = !wasFaulted;
            bool faultChanged = !string.Equals(lastFaultMessage, finalMessage, StringComparison.Ordinal);

            if (isEdgeFault || faultChanged)
            {
                Warn(code, finalMessage, ReportChannels.Log | ReportChannels.Message);

                if (alarmCode.HasValue)
                {
                    RaiseAlarm(alarmCode.Value, alarmLevel, finalMessage);
                }
            }
            else
            {
                WarnLogOnlyIfRepeated(
                    finalStateKey,
                    code,
                    finalMessage,
                    repeatIntervalMs);
            }

            wasFaulted = true;
            lastFaultMessage = finalMessage;
        }

        /// <summary>
        /// 后台任务恢复边沿统一处理。
        /// 仅在之前确实进入过故障态时输出一条恢复日志。
        /// </summary>
        protected void ReportBackgroundRecoveredIfNeeded(
            ref bool wasFaulted,
            ref string lastFaultMessage,
            string recoveredMessage)
        {
            if (!wasFaulted)
            {
                return;
            }

            OkLogOnly(string.IsNullOrWhiteSpace(recoveredMessage)
                ? "后台任务恢复正常"
                : recoveredMessage);

            wasFaulted = false;
            lastFaultMessage = string.Empty;
        }

        /// <summary>
        /// 重置后台故障态标记。
        /// 常用于运行器启动前、停止后或重新建链时。
        /// </summary>
        protected void ResetBackgroundFaultState(ref bool wasFaulted, ref string lastFaultMessage)
        {
            wasFaulted = false;
            lastFaultMessage = string.Empty;
        }

        #endregion

        #region Motion IO 运行时缓存公共帮助

        /// <summary>
        /// 计算运行时缓存过期阈值。
        /// 规则：取 max(scanInterval * 4, defaultCacheStaleToleranceMs)。
        /// </summary>
        protected int GetMotionIoCacheStaleToleranceMs(
            MotionIoRuntimeState runtimeState,
            int defaultPollIntervalMs,
            int defaultCacheStaleToleranceMs)
        {
            if (runtimeState == null)
            {
                return defaultCacheStaleToleranceMs;
            }

            int scanInterval = runtimeState.ScanIntervalMs > 0
                ? runtimeState.ScanIntervalMs
                : defaultPollIntervalMs;

            int calculated = scanInterval * 4;
            return calculated > defaultCacheStaleToleranceMs
                ? calculated
                : defaultCacheStaleToleranceMs;
        }

        /// <summary>
        /// 校验 Motion IO 运行时缓存是否可用。
        /// 失败时：
        /// 1. 使用专用错误码 `IoRuntimeCacheStale`；
        /// 2. 仅按节流策略写日志；
        /// 3. 返回静默失败，避免调用链重复刷日志。
        /// </summary>
        protected Result ValidateMotionIoRuntimeCache(
            MotionIoRuntimeState runtimeState,
            int defaultPollIntervalMs,
            int defaultCacheStaleToleranceMs,
            int logThrottleIntervalMs)
        {
            if (runtimeState == null)
            {
                return CreateMotionIoRuntimeCacheUnavailableResult(
                    runtimeState,
                    "Motion IO 运行时缓存未初始化",
                    logThrottleIntervalMs);
            }

            if (!runtimeState.IsScanServiceRunning)
            {
                return CreateMotionIoRuntimeCacheUnavailableResult(
                    runtimeState,
                    "Motion IO 扫描工作单元未运行",
                    logThrottleIntervalMs);
            }

            if (!runtimeState.LastScanTime.HasValue)
            {
                return CreateMotionIoRuntimeCacheUnavailableResult(
                    runtimeState,
                    "Motion IO 运行时缓存尚无扫描数据",
                    logThrottleIntervalMs);
            }

            double ageMs = (DateTime.Now - runtimeState.LastScanTime.Value).TotalMilliseconds;
            int staleToleranceMs = GetMotionIoCacheStaleToleranceMs(
                runtimeState,
                defaultPollIntervalMs,
                defaultCacheStaleToleranceMs);

            if (ageMs > staleToleranceMs)
            {
                return CreateMotionIoRuntimeCacheUnavailableResult(
                    runtimeState,
                    string.Format(
                        "Motion IO 运行时缓存已过期 | Age={0:0}ms | Threshold={1}ms",
                        ageMs,
                        staleToleranceMs),
                    logThrottleIntervalMs);
            }

            return OkSilent("Motion IO 运行时缓存可用");
        }

        /// <summary>
        /// 读取 Motion IO 逻辑 DI 缓存。
        /// 包含：
        /// 1. 扫描服务状态校验；
        /// 2. 全局缓存新鲜度校验；
        /// 3. 单点更新时间校验。
        /// </summary>
        protected Result TryReadMotionIoCachedDI(
            short logicalBit,
            int defaultPollIntervalMs,
            int defaultCacheStaleToleranceMs,
            int logThrottleIntervalMs,
            out bool value)
        {
            value = false;

            MotionIoRuntimeState runtimeState = RuntimeContext.Instance.MotionIo;
            Result validateResult = ValidateMotionIoRuntimeCache(
                runtimeState,
                defaultPollIntervalMs,
                defaultCacheStaleToleranceMs,
                logThrottleIntervalMs);

            if (!validateResult.Success)
            {
                return validateResult;
            }

            if (!runtimeState.TryGetDI(logicalBit, out value))
            {
                return Fail(
                    (int)MotionErrorCode.IoMapNotFound,
                    "逻辑DI缓存不存在: " + logicalBit);
            }

            DateTime updateTime;
            if (runtimeState.TryGetDIUpdateTime(logicalBit, out updateTime))
            {
                double ageMs = (DateTime.Now - updateTime).TotalMilliseconds;
                int staleToleranceMs = GetMotionIoCacheStaleToleranceMs(
                    runtimeState,
                    defaultPollIntervalMs,
                    defaultCacheStaleToleranceMs);

                if (ageMs > staleToleranceMs)
                {
                    return CreateMotionIoRuntimeCacheUnavailableResult(
                        runtimeState,
                        string.Format(
                            "逻辑DI缓存值已过期: {0} | Age={1:0}ms | Threshold={2}ms",
                            logicalBit,
                            ageMs,
                            staleToleranceMs),
                        logThrottleIntervalMs);
                }
            }

            return OkSilent("逻辑DI缓存读取成功");
        }

        /// <summary>
        /// 生成 Motion IO 运行时缓存不可用结果。
        /// 
        /// 说明：
        /// 1. 错误码统一使用 `IoRuntimeCacheStale`；
        /// 2. 诊断信息统一附带 LastScan / ScanInterval / Age；
        /// 3. 仅按节流策略写 WarnLogOnly；
        /// 4. 返回静默失败，避免上层高频状态查询反复刷日志。
        /// </summary>
        protected Result CreateMotionIoRuntimeCacheUnavailableResult(
            MotionIoRuntimeState runtimeState,
            string message,
            int logThrottleIntervalMs)
        {
            string finalMessage = message ?? "Motion IO 运行时缓存不可用";

            if (runtimeState != null)
            {
                string lastScanText = runtimeState.LastScanTime.HasValue
                    ? runtimeState.LastScanTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    : "-";

                double ageMs = runtimeState.LastScanTime.HasValue
                    ? (DateTime.Now - runtimeState.LastScanTime.Value).TotalMilliseconds
                    : -1;

                finalMessage = finalMessage
                    + " | LastScan=" + lastScanText
                    + " | ScanInterval=" + runtimeState.ScanIntervalMs + "ms"
                    + (ageMs >= 0 ? " | Age=" + ageMs.ToString("0") + "ms" : string.Empty);
            }

            WarnLogOnlyIfRepeated(
                "MOTION-IO-CACHE-" + MessageSourceName,
                (int)MotionErrorCode.IoRuntimeCacheStale,
                finalMessage,
                logThrottleIntervalMs);

            return FailSilent((int)MotionErrorCode.IoRuntimeCacheStale, finalMessage);
        }

        #endregion

        #region 报警

        protected void RaiseAlarm(AlarmCode code, AlarmLevel level, string message)
        {
            _reporter?.Alarm(MessageSourceName, code, level, message, MessageCardId);
        }

        #endregion

        #region 私有辅助

        private static ResultNotifyMode ChannelsToNotifyMode(ReportChannels channels)
        {
            bool hasLog = (channels & ReportChannels.Log) == ReportChannels.Log;
            bool hasMsg = (channels & ReportChannels.Message) == ReportChannels.Message;

            if (hasLog && hasMsg) return ResultNotifyMode.All;
            if (hasLog) return ResultNotifyMode.LogOnly;
            if (hasMsg) return ResultNotifyMode.MessageOnly;
            return ResultNotifyMode.Silent;
        }

        /// <summary>
        /// 清理节流缓存：
        /// 1. 优先删除超出保留时间的旧键；
        /// 2. 若数量仍过大，则整体清空，避免缓存无限增长。
        /// </summary>
        private void CleanupReportThrottleCache(DateTime now)
        {
            foreach (var pair in _reportThrottleTimes)
            {
                if ((now - pair.Value) > ReportThrottleRetention)
                {
                    DateTime removedTime;
                    _reportThrottleTimes.TryRemove(pair.Key, out removedTime);
                }
            }

            if (_reportThrottleTimes.Count > ReportThrottleMaxEntries * 2)
            {
                _reportThrottleTimes.Clear();
            }
        }

        #endregion
    }
}