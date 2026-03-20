using System;

namespace AM.Core.Reporter
{
    /// <summary>
    /// 报告输出通道。
    /// 用于控制日志与消息通知是否输出。
    /// </summary>
    [Flags]
    public enum ReportChannels
    {
        None = 0,
        Log = 1,
        Message = 2,
        All = Log | Message
    }
}