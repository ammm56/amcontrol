using System;

namespace AM.Core.Messaging
{
    /// <summary>
    /// UI消息通知系统
    /// </summary>
    public class SystemMessage
    {
        public SystemMessageType Type { get; }

        public string Message { get; }

        public DateTime Time { get; }

        /// <summary>
        /// 消息来源，例如 MotionCard / DB / PLC / Vision / Tools
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// 业务或设备错误码，允许为空
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 错误详细说明
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 处理建议
        /// </summary>
        public string Suggestion { get; }

        /// <summary>
        /// 控制卡编号，非控制卡消息可为空
        /// </summary>
        public short? CardId { get; }

        /// <summary>
        /// 兼容旧调用方式，默认 Source/Code/CardId/Description/Suggestion 为空
        /// 默认调用只有类型和消息内容，适用于大多数场景
        /// </summary>
        public SystemMessage(string msg, SystemMessageType type): this(msg, type, null, null, null, null, null)
        {
        }

        /// <summary>
        /// 兼容旧调用方式，默认 Description/Suggestion 为空
        /// 默认调用只有类型、消息内容、来源、错误代码
        /// </summary>
        public SystemMessage(string msg, SystemMessageType type, string source, string code, short? cardId): this(msg, type, source, code, null, null, cardId)
        {
        }

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
