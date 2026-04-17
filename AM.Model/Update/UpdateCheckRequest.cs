namespace AM.Model.Update
{
    /// <summary>
    /// 检查更新请求
    /// 客户端向更新服务发起版本检查时使用。
    /// </summary>
    public class UpdateCheckRequest
    {
        /// <summary>
        /// 应用编码。
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 当前客户端版本号。
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 发布通道。
        /// </summary>
        public string Channel { get; set; }

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
    }
}