namespace AM.Model.Usage
{
    /// <summary>
    /// 客户端身份信息
    /// 由客户端本地生成和持久化，用于更新与使用统计共用。
    /// </summary>
    public class ClientIdentity
    {
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
        public string MachineCode { get; set; }

        /// <summary>
        /// 设备名称。
        /// </summary>
        public string MachineName { get; set; }
    }
}