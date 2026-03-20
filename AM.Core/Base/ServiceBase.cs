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
        /// <summary>
        /// 统一报告器。
        /// </summary>
        protected readonly IAppReporter _reporter;

        /// <summary>
        /// 默认消息来源。
        /// 子类可重写。
        /// </summary>
        protected virtual string MessageSourceName
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// 默认结果来源。
        /// 子类可重写。
        /// </summary>
        protected virtual ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        /// <summary>
        /// 默认控制卡编号。
        /// 非控制卡服务通常为空。
        /// </summary>
        protected virtual short? MessageCardId
        {
            get { return null; }
        }

        /// <summary>
        /// 使用全局系统上下文中的统一报告器初始化。
        /// </summary>
        protected ServiceBase()
            : this(SystemContext.Instance.Reporter)
        {
        }

        /// <summary>
        /// 使用指定报告器初始化。
        /// </summary>
        /// <param name="reporter">统一报告器。</param>
        protected ServiceBase(IAppReporter reporter)
        {
            _reporter = reporter;
        }

        /// <summary>
        /// 创建成功结果。
        /// 默认为同时记录日志和发送消息。
        /// </summary>
        protected Result Ok(string message = "OK")
        {
            return Ok(message, ReportChannels.All);
        }
        /// <summary>
        /// 日志和消息通知由 ReportChannels 参数控制，子类可根据需要选择不同的报告渠道。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        protected Result Ok(string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result.Ok(message, DefaultResultSource);
        }
        /// <summary>
        /// 高频调用且不需要日志和消息通知的场景可使用 OkSilent 来避免性能开销。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Result OkSilent(string message = "OK")
        {
            return Ok(message, ReportChannels.None);
        }
        /// <summary>
        /// 只记录日志不发送UI消息。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Result OkLogOnly(string message = "OK")
        {
            return Ok(message, ReportChannels.Log);
        }
        /// <summary>
        /// 只发送UI消息不记录日志。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Result OkMessageOnly(string message = "OK")
        {
            return Ok(message, ReportChannels.Message);
        }

        /// <summary>
        /// 创建带单项数据的成功结果。
        /// </summary>
        protected Result<T> Ok<T>(T item, string message = "OK")
        {
            return Ok(item, message, ReportChannels.All);
        }
        protected Result<T> Ok<T>(T item, string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result<T>.OkItem(item, message, DefaultResultSource);
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

        /// <summary>
        /// 创建带集合数据的成功结果。
        /// </summary>
        protected Result<T> OkList<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.All);
        }
        protected Result<T> OkList<T>(IEnumerable<T> items, string message, ReportChannels channels)
        {
            _reporter?.Info(MessageSourceName, message, null, MessageCardId, channels);
            return Result<T>.OkList(items, message, DefaultResultSource);
        }
        protected Result<T> OkListSilent<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.None);
        }
        protected Result<T> OkListLogOnly<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.Log);
        }
        protected Result<T> OkListMessageOnly<T>(IEnumerable<T> items, string message = "OK")
        {
            return OkList(items, message, ReportChannels.Message);
        }

        /// <summary>
        /// 创建警告结果。
        /// </summary>
        protected Result Warn(int code, string message)
        {
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId);
            return Result.Fail(code, message, DefaultResultSource);
        }

        /// <summary>
        /// 创建带泛型的警告结果。
        /// </summary>
        protected Result<T> Warn<T>(int code, string message)
        {
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId);
            return Result<T>.Fail(code, message, DefaultResultSource);
        }

        /// <summary>
        /// 创建失败结果。
        /// </summary>
        protected Result Fail(int code, string message, Exception ex = null)
        {
            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code, MessageCardId);
            else
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId);

            return Result.Fail(code, message, DefaultResultSource);
        }

        /// <summary>
        /// 创建带泛型的失败结果。
        /// </summary>
        protected Result<T> Fail<T>(int code, string message, Exception ex = null)
        {
            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code, MessageCardId);
            else
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId);

            return Result<T>.Fail(code, message, DefaultResultSource);
        }

        /// <summary>
        /// 按底层错误码统一转换结果。
        /// 0 视为成功，非 0 视为失败。
        /// </summary>
        protected Result HandleError(short code, string message)
        {
            return code == 0 ? Ok(message) : Fail(code, message);
        }

        /// <summary>
        /// 按底层错误码统一转换泛型结果。
        /// </summary>
        protected Result<T> HandleError<T>(short code, string message)
        {
            return code == 0 ? Ok(default(T), message) : Fail<T>(code, message);
        }

        /// <summary>
        /// 将异常包装为统一失败结果。
        /// </summary>
        protected Result HandleException(Exception ex, int code, string message)
        {
            return Fail(code, message, ex);
        }

        /// <summary>
        /// 将异常包装为统一泛型失败结果。
        /// </summary>
        protected Result<T> HandleException<T>(Exception ex, int code, string message)
        {
            return Fail<T>(code, message, ex);
        }

        /// <summary>
        /// 触发报警。
        /// </summary>
        protected void RaiseAlarm(AlarmCode code, AlarmLevel level, string message)
        {
            _reporter?.Alarm(MessageSourceName, code, level, message, MessageCardId);
        }
    }
}