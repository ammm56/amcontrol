using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model.Enum
{
    /// <summary>
    /// 日志存储方式
    /// </summary>
    public enum E_LogSaveMode
    {
        /// <summary>
        /// 存储到单个文件
        /// </summary>
        singlefile,
        /// <summary>
        /// 到最大文件大小时，创建新文件存储
        /// </summary>
        fixedsize,
        /// <summary>
        /// 按时间创建文件存储 年 月 日 时 分
        /// </summary>
        time
    }

    /// <summary>
    /// 按时间存储时 判断创建方式
    /// </summary>
    public enum E_TimeSaveGenerateMode
    {
        /// <summary>
        /// 每分钟
        /// </summary>
        minute,
        /// <summary>
        /// 每小时
        /// </summary>
        hour,
        /// <summary>
        /// 每天
        /// </summary>
        day,
        /// <summary>
        /// 每月
        /// </summary>
        month,
        /// <summary>
        /// 每年
        /// </summary>
        year
    }

    /// <summary>
    /// 记录日志等级
    /// </summary>
    public enum E_LogMsgLevel
    {
        /// <summary>
        /// 不记录
        /// </summary>
        none,
        /// <summary>
        /// 记录调试和以上
        /// </summary>
        debug,
        /// <summary>
        /// 记录一般信息及以上
        /// </summary>
        info,
        /// <summary>
        /// 记录警告信息及以上
        /// </summary>
        warn,
        /// <summary>
        /// 记录错误信息及以上
        /// </summary>
        error,
        /// <summary>
        /// 记录致命错误信息
        /// </summary>
        fatal
    }
}
