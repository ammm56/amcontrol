using SqlSugar;
using System;

namespace AM.Model.Entity.System
{
    /// <summary>
    /// 使用信息本地缓冲表。
    /// 用于客户端先落库再批量上传，避免网络问题影响主流程。
    /// </summary>
    [SugarTable("sys_usage_event_buffer")]
    public class SysUsageEventBufferEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 事件唯一标识。
        /// 用于服务端去重。
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 事件类型。
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
        [SugarColumn(IsNullable = true)]
        public string MachineCode { get; set; }

        /// <summary>
        /// 设备名称。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string MachineName { get; set; }

        /// <summary>
        /// 用户 ID。
        /// 未登录时为空。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? UserId { get; set; }

        /// <summary>
        /// 登录名。
        /// 未登录时为空。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string LoginName { get; set; }

        /// <summary>
        /// 页面键。
        /// 仅 PageVisit 事件使用。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string PageKey { get; set; }

        /// <summary>
        /// 是否成功。
        /// 仅登录类事件使用。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// 失败原因代码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string FailReasonCode { get; set; }

        /// <summary>
        /// 跟踪标识。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string TraceId { get; set; }

        /// <summary>
        /// 事件发生时间。
        /// </summary>
        public DateTime OccurredTime { get; set; }

        /// <summary>
        /// 上传状态。
        /// 建议值：Pending / Uploaded / Failed。
        /// </summary>
        public string UploadStatus { get; set; }

        /// <summary>
        /// 上传重试次数。
        /// </summary>
        public int UploadRetryCount { get; set; }

        /// <summary>
        /// 最近上传时间。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? UploadTime { get; set; }

        /// <summary>
        /// 最近一次上传结果消息。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string UploadMessage { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}