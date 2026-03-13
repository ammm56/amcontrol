using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Messaging
{
    /// <summary>
    /// UI消息通知系统
    /// 
    /// 提示信息
    /// 状态信息
    /// 普通错误
    /// 
    /// 系统启动完成
    /// 参数加载成功
    /// 数据库连接成功
    /// 程序已连接PLC
    /// </summary>
    public class SystemMessage
    {
        public SystemMessageType Type { get; }

        public string Message { get; }

        public DateTime Time { get; }

        /// <summary>
        /// 消息来源，例如 MotionCard / DB / PLC / Vision
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// 业务或设备错误码，允许为空
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 控制卡编号，非控制卡消息可为空
        /// </summary>
        public short? CardId { get; }

        /// <summary>
        /// 兼容旧调用方式，默认 Source/Code/CardId 为空
        /// 默认调用只有类型和消息内容，适用于大多数场景
        /// </summary>
        public SystemMessage(string msg, SystemMessageType type): this(msg, type, null, null, null)
        {
        }

        public SystemMessage(string msg,SystemMessageType type,string source,string code,short? cardId)
        {
            Message = msg ?? string.Empty;
            Type = type;
            Source = source;
            Code = code;
            CardId = cardId;
            Time = DateTime.Now;
        }

    }
}
