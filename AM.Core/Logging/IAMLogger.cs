using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Logging
{
    /// <summary>
    /// 日志记录器接口
    /// 职责：持久化记录、排障、审计
    /// 特点：面向开发/运维、不一定直接给用户
    /// </summary>
    public interface IAMLogger
    {
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Error(string message);
        void Error(Exception ex, string message);
    }
}
