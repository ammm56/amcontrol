using System;

namespace AM.Model.Update
{
    /// <summary>
    /// 更新结果上报
    /// 用于上报检查、下载、安装、回滚等动作结果。
    /// </summary>
    public class UpdateReport
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
        /// 当前版本号。
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 目标版本号。
        /// </summary>
        public string TargetVersion { get; set; }

        /// <summary>
        /// 发布通道。
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 更新动作。
        /// 建议值：Check / Download / Install / Rollback。
        /// </summary>
        public string UpdateAction { get; set; }

        /// <summary>
        /// 执行结果。
        /// 建议值：Success / Failed / Canceled。
        /// </summary>
        public string UpdateStatus { get; set; }

        /// <summary>
        /// 结果消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 跟踪标识。
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// 发生时间。
        /// </summary>
        public DateTime OccurredTime { get; set; }
    }
}