using SqlSugar;
using System;

namespace AM.Model.Entity.System
{
    /// <summary>
    /// 客户端更新记录本地表。
    /// 记录检查、下载、安装、回滚等动作结果，便于审计与问题排查。
    /// </summary>
    [SugarTable("sys_client_update_record")]
    public class SysClientUpdateRecordEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 客户端唯一标识。
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 应用编码。
        /// </summary>
        public string AppCode { get; set; }

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
        /// 当前版本号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 目标版本号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string TargetVersion { get; set; }

        /// <summary>
        /// 发布通道。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Channel { get; set; }

        /// <summary>
        /// 更新动作。
        /// 建议值：Check / Download / Install / Rollback。
        /// </summary>
        public string UpdateAction { get; set; }

        /// <summary>
        /// 更新结果。
        /// 建议值：Success / Failed / Canceled。
        /// </summary>
        public string UpdateStatus { get; set; }

        /// <summary>
        /// 结果消息。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Message { get; set; }

        /// <summary>
        /// 跟踪标识。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string TraceId { get; set; }

        /// <summary>
        /// 是否已成功上报到服务端。
        /// </summary>
        public bool IsReported { get; set; }

        /// <summary>
        /// 上报时间。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ReportTime { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}