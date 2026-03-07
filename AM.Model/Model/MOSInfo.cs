using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Model
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class MOSInfo
    {
        /// <summary>
        /// 内存
        /// </summary>
        public long Mem { get; set; }
        /// <summary>
        /// CPU 运行时间
        /// </summary>
        public TimeSpan CPU { get; set; }
        /// <summary>
        /// 运行框架
        /// </summary>
        public string FrameworkDesc { get; set; }
        /// <summary>
        /// 系统
        /// </summary>
        public string OSDesc { get; set; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string OSVersion { get; set; }
        /// <summary>
        /// 系统架构
        /// </summary>
        public string OSArch { get; set; }
        /// <summary>
        /// 机器名
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// 用户域名
        /// </summary>
        public string UserDomainName { get; set; }
        /// <summary>
        /// 磁盘分区
        /// </summary>
        public List<string> Drives { get; set; }
        /// <summary>
        /// 系统目录
        /// </summary>
        public string SystemDirect { get; set; }
        /// <summary>
        /// 已运行时间 毫秒
        /// </summary>
        public int TickCount { get; set; }
        /// <summary>
        /// 是否交互模式运行
        /// </summary>
        public bool UserInter { get; set; }
        /// <summary>
        /// 关联用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 框架版本
        /// </summary>
        public string Version { get; set; }

    }
}
