using System;

namespace AM.Model.Usage
{
    /// <summary>
    /// 使用信息事件
    /// 仅用于最低限度程序运行信息采集，不包含设备运行数据与生产数据。
    /// </summary>
    public class UsageEvent
    {
        /// <summary>
        /// 事件唯一标识。
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 事件类型。
        /// 建议值：AppStart / AppExit / LoginSuccess / LoginFailed / PageVisit。
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 应用编码。
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 程序版本号。
        /// </summary>
        public string AppVersion { get; set; }

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
        /// 用户 ID。
        /// 未登录时可为空。
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 登录名。
        /// 未登录时可为空。
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 页面键。
        /// 仅 PageVisit 事件使用。
        /// </summary>
        public string PageKey { get; set; }

        /// <summary>
        /// 是否成功。
        /// 仅登录类事件使用。
        /// </summary>
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// 失败原因代码。
        /// 仅失败事件使用。
        /// </summary>
        public string FailReasonCode { get; set; }

        /// <summary>
        /// 发生时间。
        /// </summary>
        public DateTime OccurredTime { get; set; }

        /// <summary>
        /// 跟踪标识。
        /// </summary>
        public string TraceId { get; set; }
    }
}