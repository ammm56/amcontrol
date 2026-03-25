using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Alarm;
using AM.Model.Common;
using System;
using System.Collections.Generic;

namespace AM.Core.Base
{
    /// <summary>
    /// 统一处理结果对象生成、日志、消息通知、报警和异常包装的服务基类。
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>统一报告器。</summary>
        protected readonly IAppReporter _reporter;

        /// <summary>默认消息来源，子类可重写。</summary>
        protected virtual string MessageSourceName
        {
            get { return GetType().Name; }
        }

        /// <summary>默认结果来源，子类可重写。</summary>
        protected virtual ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        /// <summary>默认控制卡编号，非控制卡服务通常为空。</summary>
        protected virtual short? MessageCardId
        {
            get { return null; }
        }

        /// <summary>使用全局系统上下文中的统一报告器初始化。</summary>
        protected ServiceBase()
            : this(SystemContext.Instance.Reporter)
        {
        }

        /// <summary>使用指定报告器初始化。</summary>
        protected ServiceBase(IAppReporter reporter)
        {
            _reporter = reporter;
        }

        // ── 成功结果：Result ──────────────────────────────────────────────

        /// <summary>创建成功结果（全量通知）。</summary>
        protected Result Ok(string message = "OK")
        {
            return Ok(message, ReportChannels.All);
        }

        /// <summary>
        /// 创建成功结果，通知渠道由 ReportChannels 控制，并同步写入 Result.NotifyMode。
        /// UI 层可通过 result.ShouldNotifyUi 决定是否弹出通知，无需硬编码判断。
        /// </summary>
        protected Result Ok(string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result.Ok(message, DefaultResultSource)
                         .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        /// <summary>静默成功结果（不写日志，不发 UI 消息）。高频/后台场景使用。</summary>
        protected Result OkSilent(string message = "OK")
        {
            return Ok(message, ReportChannels.None);
        }

        /// <summary>仅写日志，不发 UI 消息。适用于常规 CRUD 等不需要弹框的成功操作。</summary>
        protected Result OkLogOnly(string message = "OK")
        {
            return Ok(message, ReportChannels.Log);
        }

        /// <summary>仅发 UI 消息，不写日志。</summary>
        protected Result OkMessageOnly(string message = "OK")
        {
            return Ok(message, ReportChannels.Message);
        }

        // ── 成功结果：Result<T> 单项 ─────────────────────────────────────

        /// <summary>创建带单项数据的成功结果（全量通知）。</summary>
        protected Result<T> Ok<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.All);
        }

        /// <summary>创建带单项数据的成功结果，通知渠道由 ReportChannels 控制。</summary>
        protected Result<T> Ok<T>(T item, string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result<T>.OkItem(item, message, DefaultResultSource)
                            .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        /// <summary>带单项数据的静默成功结果。</summary>
        protected Result<T> OkSilent<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.None);
        }

        /// <summary>带单项数据，仅写日志。</summary>
        protected Result<T> OkLogOnly<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.Log);
        }

        /// <summary>带单项数据，仅发 UI 消息。</summary>
        protected Result<T> OkMessageOnly<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.Message);
        }

        // ── 成功结果：Result<T> 集合 ─────────────────────────────────────

        /// <summary>创建带集合数据的成功结果（全量通知）。</summary>
        protected Result<T> OkList<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.All);
        }

        /// <summary>创建带集合数据的成功结果，通知渠道由 ReportChannels 控制。</summary>
        protected Result<T> OkList<T>(IEnumerable<T> items, string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result<T>.OkList(items, message, DefaultResultSource)
                            .WithNotifyMode(ChannelsToNotifyMode(channels));
        }

        /// <summary>带集合数据的静默成功结果。</summary>
        protected Result<T> OkListSilent<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.None);
        }

        /// <summary>带集合数据，仅写日志。</summary>
        protected Result<T> OkListLogOnly<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.Log);
        }

        /// <summary>带集合数据，仅发 UI 消息。</summary>
        protected Result<T> OkListMessageOnly<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.Message);
        }

        // ── 警告 / 失败 ───────────────────────────────────────────────────

        /// <summary>创建警告结果。</summary>
        protected Result Warn(int code, string message)
        {
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId);
            return Result.Fail(code, message, DefaultResultSource);
        }

        /// <summary>创建带泛型的警告结果。</summary>
        protected Result<T> Warn<T>(int code, string message)
        {
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId);
            return Result<T>.Fail(code, message, DefaultResultSource);
        }

        /// <summary>创建失败结果。</summary>
        protected Result Fail(int code, string message, Exception ex = null)
        {
            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code, MessageCardId);
            else
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId);

            return Result.Fail(code, message, DefaultResultSource);
        }

        /// <summary>创建带泛型的失败结果。</summary>
        protected Result<T> Fail<T>(int code, string message, Exception ex = null)
        {
            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code, MessageCardId);
            else
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId);

            return Result<T>.Fail(code, message, DefaultResultSource);
        }

        // ── 错误码转换 ────────────────────────────────────────────────────

        /// <summary>按底层错误码统一转换结果，0 为成功，非 0 为失败。</summary>
        protected Result HandleError(short code, string message)
        {
            return code == 0 ? Ok(message) : Fail(code, message);
        }

        /// <summary>按底层错误码统一转换泛型结果。</summary>
        protected Result<T> HandleError<T>(short code, string message)
        {
            return code == 0 ? Ok(default(T), message) : Fail<T>(code, message);
        }

        /// <summary>将异常包装为统一失败结果。</summary>
        protected Result HandleException(Exception ex, int code, string message)
        {
            return Fail(code, message, ex);
        }

        /// <summary>将异常包装为统一泛型失败结果。</summary>
        protected Result<T> HandleException<T>(Exception ex, int code, string message)
        {
            return Fail<T>(code, message, ex);
        }

        // ── 报警 ──────────────────────────────────────────────────────────

        /// <summary>触发报警。</summary>
        protected void RaiseAlarm(AlarmCode code, AlarmLevel level, string message)
        {
            _reporter?.Alarm(MessageSourceName, code, level, message, MessageCardId);
        }

        // ── 内部辅助 ──────────────────────────────────────────────────────

        /// <summary>
        /// 将 ReportChannels 映射为 ResultNotifyMode，使 Result 携带与 Reporter 一致的通知意图。
        /// </summary>
        private static ResultNotifyMode ChannelsToNotifyMode(ReportChannels channels)
        {
            var hasLog = (channels & ReportChannels.Log)     == ReportChannels.Log;
            var hasMsg = (channels & ReportChannels.Message) == ReportChannels.Message;
            if (hasLog && hasMsg) return ResultNotifyMode.All;
            if (hasLog)           return ResultNotifyMode.LogOnly;
            if (hasMsg)           return ResultNotifyMode.MessageOnly;
            return ResultNotifyMode.Silent;
        }
    }
}