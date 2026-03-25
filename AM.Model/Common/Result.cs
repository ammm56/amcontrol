using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.Model.Common
{
    /// <summary>
    /// 结果对象的通知模式，控制成功结果的日志与 UI 消息推送行为。
    /// 失败结果始终走全量通知，不受此控制。
    /// 此枚举仅作为标志位由 ServiceBase 设置，Result 本身不执行任何通知行为。
    /// </summary>
    public enum ResultNotifyMode
    {
        /// <summary>同时记录日志并发送 UI 消息（默认）。</summary>
        All = 0,
        /// <summary>仅记录日志，不发送 UI 消息。</summary>
        LogOnly = 1,
        /// <summary>仅发送 UI 消息，不记录日志。</summary>
        MessageOnly = 2,
        /// <summary>静默，既不记录日志也不发送 UI 消息。</summary>
        Silent = 3
    }

    /// <summary>
    /// 统一返回结果（无数据）。
    /// 纯数据载体，不执行任何日志或通知行为。
    /// 通知行为由 ServiceBase 的 Ok/OkLogOnly/OkSilent 等方法统一控制。
    /// </summary>
    public class Result
    {
        /// <summary>是否成功。</summary>
        public bool Success { get; set; }

        /// <summary>结果码。0 通常表示成功，非 0 表示失败或警告。</summary>
        public int Code { get; set; }

        /// <summary>返回信息。</summary>
        public string Message { get; set; }

        /// <summary>来源。</summary>
        public ResultSource Source { get; set; }

        /// <summary>时间。</summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 通知模式。由 ServiceBase 在创建结果时写入，UI 层只读不写。
        /// 失败结果此值无效，始终全量通知。
        /// </summary>
        public ResultNotifyMode NotifyMode { get; set; } = ResultNotifyMode.All;

        /// <summary>成功时是否应向 UI 推送消息通知。UI 层在调用 Growl 前应检查此属性。</summary>
        public bool ShouldNotifyUi
        {
            get { return NotifyMode == ResultNotifyMode.All || NotifyMode == ResultNotifyMode.MessageOnly; }
        }

        /// <summary>成功时是否应写入日志。</summary>
        public bool ShouldLog
        {
            get { return NotifyMode == ResultNotifyMode.All || NotifyMode == ResultNotifyMode.LogOnly; }
        }

        protected Result()
        {
            Time    = DateTime.Now;
            Message = string.Empty;
            Source  = ResultSource.Unknown;
        }

        protected Result(bool success, int code, string message, ResultSource source)
        {
            Success = success;
            Code    = code;
            Message = message ?? string.Empty;
            Source  = source;
            Time    = DateTime.Now;
        }

        // ── 成功工厂方法 ──────────────────────────────────────────────────

        /// <summary>创建成功结果，NotifyMode 默认 All。通常由 ServiceBase 内部调用后附加 WithNotifyMode。</summary>
        public static Result Ok(string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result(true, 0, message, source);
        }

        // ── 失败工厂方法 ──────────────────────────────────────────────────

        /// <summary>创建失败结果（始终全量通知）。</summary>
        public static Result Fail(int code, string message, ResultSource source = ResultSource.Unknown)
        {
            return new Result(false, code, message, source);
        }

        // ── 链式调用 ──────────────────────────────────────────────────────

        /// <summary>
        /// 设置通知模式（链式调用）。
        /// 由 ServiceBase 在构建结果时调用，UI 层不应直接调用此方法。
        /// </summary>
        public Result WithNotifyMode(ResultNotifyMode mode)
        {
            NotifyMode = mode;
            return this;
        }
    }

    /// <summary>
    /// 统一返回结果（带数据）。
    /// 纯数据载体，不执行任何日志或通知行为。
    /// </summary>
    public class Result<T> : Result
    {
        /// <summary>单项数据。</summary>
        public T Item { get; set; }

        /// <summary>数据集合，只读。</summary>
        public IReadOnlyList<T> Items { get; protected set; } = new List<T>();

        protected Result() : base()
        {
            Items = new List<T>();
        }

        public Result(bool success, int code, string message, ResultSource source)
            : base(success, code, message, source)
        {
            Items = new List<T>();
        }

        // ── OkItem 工厂方法 ───────────────────────────────────────────────

        /// <summary>创建带单项数据的成功结果，NotifyMode 默认 All。</summary>
        public static Result<T> OkItem(T item, string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = true, Code = 0, Message = message ?? string.Empty,
                Source  = source, Time = DateTime.Now,
                Item    = item,  Items = new List<T>()
            };
        }

        // ── OkList 工厂方法 ───────────────────────────────────────────────

        /// <summary>创建带集合数据的成功结果（List 重载），NotifyMode 默认 All。</summary>
        public static Result<T> OkList(List<T> items, string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = true, Code = 0, Message = message ?? string.Empty,
                Source  = source, Time = DateTime.Now,
                Items   = items ?? new List<T>()
            };
        }

        /// <summary>创建带集合数据的成功结果（IEnumerable 重载），NotifyMode 默认 All。</summary>
        public static Result<T> OkList(IEnumerable<T> items, string message = "OK", ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = true, Code = 0, Message = message ?? string.Empty,
                Source  = source, Time = DateTime.Now,
                Items   = items == null ? new List<T>() : items.ToList()
            };
        }

        // ── 失败 ──────────────────────────────────────────────────────────

        /// <summary>创建失败结果（始终全量通知）。</summary>
        public static new Result<T> Fail(int code, string message, ResultSource source = ResultSource.Unknown)
        {
            return new Result<T>
            {
                Success = false, Code = code, Message = message ?? string.Empty,
                Source  = source, Time = DateTime.Now,
                Items   = new List<T>()
            };
        }

        // ── 链式调用 ──────────────────────────────────────────────────────

        /// <summary>设置通知模式（链式调用），返回强类型 Result&lt;T&gt;。</summary>
        public new Result<T> WithNotifyMode(ResultNotifyMode mode)
        {
            NotifyMode = mode;
            return this;
        }
    }
}
