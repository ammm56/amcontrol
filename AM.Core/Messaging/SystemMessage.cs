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

        public SystemMessage(string msg, SystemMessageType type)
        {
            Message = msg;
            Type = type;
            Time = DateTime.Now;
        }
    }
}
