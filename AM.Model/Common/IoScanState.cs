namespace AM.Model.Common
{
    /// <summary>
    /// IO 后台扫描服务运行状态。
    /// 工业设备要求：扫描出错后服务自动停止，需手动重启。
    /// </summary>
    public enum IoScanState
    {
        /// <summary>
        /// 空闲/已停止（默认状态）。
        /// </summary>
        Idle,

        /// <summary>
        /// 扫描运行中。
        /// </summary>
        Running,

        /// <summary>
        /// 因扫描错误已自动停止，需手动重启。
        /// </summary>
        Error
    }
}