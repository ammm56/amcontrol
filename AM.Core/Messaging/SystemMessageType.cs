using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Messaging
{
    /// <summary>
    /// 系统消息类型。
    /// 用于区分消息在日志、UI 通知和消息中心中的展示语义。
    /// </summary>
    public enum SystemMessageType
    {
        /// <summary>
        /// 普通信息。
        /// </summary>
        Info,

        /// <summary>
        /// 警告信息。
        /// </summary>
        Warning,

        /// <summary>
        /// 错误信息。
        /// </summary>
        Error,

        /// <summary>
        /// 报警信息。
        /// 属于运行时报警状态，而不只是普通消息。
        /// </summary>
        Alarm,

        /// <summary>
        /// 状态信息。
        /// 通常用于提示流程进度、连接成功、初始化完成等状态变化。
        /// </summary>
        Status
    }
}
