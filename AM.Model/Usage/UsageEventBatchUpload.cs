using System.Collections.Generic;

namespace AM.Model.Usage
{
    /// <summary>
    /// 使用信息批量上报
    /// 用于客户端批量提交本地缓冲事件。
    /// </summary>
    public class UsageEventBatchUpload
    {
        /// <summary>
        /// 应用编码。
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 客户端唯一标识。
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 设备编码。
        /// </summary>
        public string MachineCode { get; set; }

        /// <summary>
        /// 设备名称。
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 事件集合。
        /// </summary>
        public List<UsageEvent> Events { get; set; } = new List<UsageEvent>();
    }
}