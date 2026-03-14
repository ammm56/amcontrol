using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using AM.Model.Alarm;
using AM.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.Core.Base
{
    /// <summary>
    /// 统一处理结果Result / Result<T> 生成、日志、消息通知、报警、try/catch 统一包装的服务基类，供具体业务服务继承使用，简化代码并确保一致的行为。
    /// </summary>
    public abstract class ServiceBase
    {
        protected readonly IMessageBus _messageBus;
        protected readonly IAMLogger _logger;
        protected readonly AlarmManager _alarmManager;
        protected readonly IAppReporter _reporter;

        /// <summary>
        /// 最近一次执行结果（用于上层诊断/UI）
        /// </summary>
        public Result LastResult { get; protected set; }

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

        protected ServiceBase(): this(SystemContext.Instance.MessageBus,SystemContext.Instance.Logger,SystemContext.Instance.AlarmManager,SystemContext.Instance.Reporter)
        {
        }

        protected ServiceBase(IMessageBus messageBus, IAMLogger logger, AlarmManager alarmManager, IAppReporter reporter)
        {
            _messageBus = messageBus;
            _logger = logger;
            _alarmManager = alarmManager;
            _reporter = reporter;
            LastResult = Result.Ok("OK", DefaultResultSource);
        }

        protected void PublishMessage(SystemMessageType type, string message, string code = null)
        {
            if (_messageBus == null) return;

            _messageBus.Publish(new SystemMessage(
                message,
                type,
                MessageSourceName,
                code,
                MessageCardId));
        }

        /// <summary>
        /// 非泛型成功结果：供命令方法与内部辅助方法使用
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Result Ok(string message = "OK")
        {
            LastResult = Result.Ok(message, DefaultResultSource);
            _reporter?.Info(MessageSourceName, message, null, MessageCardId);
            return LastResult;
        }

        /// <summary>
        /// 泛型成功结果：供查询方法使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Result<T> Ok<T>(T item, string message = "OK")
        {
            LastResult = Result.Ok(message, DefaultResultSource);
            _reporter?.Info(MessageSourceName, message, null, MessageCardId);
            return Result<T>.OkItem(item, message, DefaultResultSource);
        }

        protected Result<T> OkList<T>(IEnumerable<T> items, string message = "OK")
        {
            LastResult = Result.Ok(message, DefaultResultSource);
            _reporter?.Info(MessageSourceName, message, null, MessageCardId);
            return Result<T>.OkList(items, message, DefaultResultSource);
        }

        protected Result Warn(int code, string message)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);
            _reporter?.Warn(MessageSourceName, message, code.ToString(), MessageCardId);
            return LastResult;
        }

        protected Result<T> Warn<T>(int code, string message)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);
            _reporter?.Warn(MessageSourceName, message, code.ToString(), MessageCardId);
            return Result<T>.Fail(code, message, DefaultResultSource);
        }

        /// <summary>
        /// 非泛型失败结果：供命令方法与内部辅助方法使用
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected Result Fail(int code, string message, Exception ex = null)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);

            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code.ToString(), MessageCardId);
            else
                _reporter?.Error(MessageSourceName, ex, message, code.ToString(), MessageCardId);

            return LastResult;
        }

        /// <summary>
        /// 泛型失败结果：供查询方法使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected Result<T> Fail<T>(int code, string message, Exception ex = null)
        {
            LastResult = Result.Fail(code, message, DefaultResultSource);

            if (ex == null)
                _reporter?.Error(MessageSourceName, message, code.ToString(), MessageCardId);
            else
                _reporter?.Error(MessageSourceName, ex, message, code.ToString(), MessageCardId);

            return Result<T>.Fail(code, message, DefaultResultSource);
        }

        /// <summary>
        /// 非泛型 SDK 错误处理：供命令方法与内部辅助方法使用
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Result HandleError(short code, string message)
        {
            return code == 0 ? Ok(message) : Fail(code, message);
        }

        /// <summary>
        /// 泛型 SDK 错误处理：供查询方法使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
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
    }
}