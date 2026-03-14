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
        /// 最近一次执行结果。
        /// 用于上层诊断与界面展示。
        /// </summary>
        public Result LastResult { get; protected set; }

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
            LastResult = Result.Ok("OK", ResultSource.Unknown);
        }

        /// <summary>
        /// 创建成功结果。
        /// </summary>
        protected Result Ok(string message = "OK")
        {
            LastResult = Result.Ok(message, DefaultResultSource);
            _reporter?.Info(MessageSourceName, message, null, MessageCardId);
            return LastResult;
        }

        /// <summary>
        /// 创建带单项数据的成功结果。
        /// </summary>
        protected Result<T> Ok<T>(T item, string message = "OK")
        {
            LastResult = Result.Ok(message, DefaultResultSource);
            _reporter?.Info(MessageSourceName, message, null, MessageCardId);
            return Result<T>.OkItem(item, message, DefaultResultSource);
        }

        /// <summary>
        /// 创建带集合数据的成功结果。
        /// </summary>
        protected Result<T> OkList<T>(IEnumerable<T> items, string message = "OK")
        {
            LastResult = Result.Ok(message, DefaultResultSource);
            _reporter?.Info(MessageSourceName, message, null, MessageCardId);
            return Result<T>.OkList(items, message, DefaultResultSource);
        }

        /// <summary>
        /// 创建警告结果。
        /// </summary>
        protected Result Warn(int code, string message)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId);
            return LastResult;
        }

        /// <summary>
        /// 创建带泛型的警告结果。
        /// </summary>
        protected Result<T> Warn<T>(int code, string message)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);
            _reporter?.Warn(MessageSourceName, message, code, MessageCardId);
            return Result<T>.Fail(code, message, DefaultResultSource);
        }

        /// <summary>
        /// 创建失败结果。
        /// </summary>
        protected Result Fail(int code, string message, Exception ex = null)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);

            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code, MessageCardId);
            else
                _reporter?.Error(MessageSourceName, ex, message, code, MessageCardId);

            return LastResult;
        }

        /// <summary>
        /// 创建带泛型的失败结果。
        /// </summary>
        protected Result<T> Fail<T>(int code, string message, Exception ex = null)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);

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