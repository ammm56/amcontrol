using System;

namespace AM.Core.Messaging
{
    /// <summary>
    /// 系统消息模型。
    /// 用于 UI 消息通知、状态栏、日志转发和模块间消息广播。
    /// </summary>
    public class SystemMessage
    {
        /// <summary>
        /// 消息类型。
        /// </summary>
        public SystemMessageType Type { get; }

        /// <summary>
        /// 主消息内容。
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 消息产生时间。
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// 消息来源，例如 MotionCard / DB / PLC / Vision / Tools。
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// 业务或设备错误码，允许为空。
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 错误详细说明。
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 处理建议。
        /// </summary>
        public string Suggestion { get; }

        /// <summary>
        /// 控制卡编号，非控制卡消息可为空。
        /// </summary>
        public short? CardId { get; }

        /// <summary>
        /// 兼容旧调用方式。
        /// </summary>
        /// <param name="msg">消息内容。</param>
        /// <param name="type">消息类型。</param>
        public SystemMessage(string msg, SystemMessageType type): this(msg, type, null, null, null, null, null)
        {
        }

        /// <summary>
        /// 兼容旧调用方式。
        /// </summary>
        /// <param name="msg">消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="source">消息来源。</param>
        /// <param name="code">错误码。</param>
        /// <param name="cardId">控制卡编号。</param>
        public SystemMessage(string msg, SystemMessageType type, string source, string code, short? cardId): this(msg, type, source, code, null, null, cardId)
        {
        }

        /// <summary>
        /// 完整构造函数。
        /// </summary>
        /// <param name="msg">消息内容。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="source">消息来源。</param>
        /// <param name="code">错误码。</param>
        /// <param name="description">错误说明。</param>
        /// <param name="suggestion">处理建议。</param>
        /// <param name="cardId">控制卡编号。</param>
        public SystemMessage(
            string msg,
            SystemMessageType type,
            string source,
            string code,
            string description,
            string suggestion,
            short? cardId)
        {
            Message = msg ?? string.Empty;
            Type = type;
            Source = source;
            Code = code;
            Description = description;
            Suggestion = suggestion;
            CardId = cardId;
            Time = DateTime.Now;
        }
    }
}
