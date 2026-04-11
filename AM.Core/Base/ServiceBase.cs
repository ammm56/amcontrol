using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Alarm;
using AM.Model.Common;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace AM.Core.Base
{
    /// <summary>
    /// 统一处理结果对象生成、日志、消息通知、报警和异常包装的服务基类。
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>统一报告器。</summary>
        protected readonly IAppReporter _reporter;

        /// <summary>重复消息节流缓存。</summary>
        private readonly ConcurrentDictionary<string, DateTime> _reportThrottleTimes =
            new ConcurrentDictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

        private const int ReportThrottleMaxEntries = 2048;
        private const int ReportThrottleCleanupTriggerCount = 128;
        private static readonly TimeSpan ReportThrottleRetention = TimeSpan.FromMinutes(30);

        private int _reportThrottleWriteCount;

        protected virtual string MessageSourceName
        {
            get { return GetType().Name; }
        }

        protected virtual ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        protected virtual short? MessageCardId
        {
            get { return null; }
        }

        protected ServiceBase()
            : this(SystemContext.Instance.Reporter)
        {
        }

        protected ServiceBase(IAppReporter reporter)
        {
            _reporter = reporter;
        }

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

        /// <summary>创建失败结果，日志+消息。</summary>
        protected Result Fail(int code, string message, Exception ex = null)
        {
            return Fail(code, message, ReportChannels.All, ex);
        }

        /// <summary>创建失败结果，支持输出渠道控制。</summary>
        protected Result Fail(int code, string message, ReportChannels channels, Exception ex = null)
        {
            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code, MessageCardId, channels);
            else
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId, channels);

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
                _reporter?.Error(MessageSourceName, message, code, MessageCardId, channels);
            else
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId, channels);

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

        protected void RaiseAlarm(AlarmCode code, AlarmLevel level, string message)
        {
            _reporter?.Alarm(MessageSourceName, code, level, message, MessageCardId);
        }

        private static ResultNotifyMode ChannelsToNotifyMode(ReportChannels channels)
        {
            var hasLog = (channels & ReportChannels.Log) == ReportChannels.Log;
            var hasMsg = (channels & ReportChannels.Message) == ReportChannels.Message;
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
    }
}