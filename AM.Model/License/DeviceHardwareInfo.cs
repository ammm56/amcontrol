using Newtonsoft.Json;

namespace AM.Model.License
{
    /// <summary>
    /// 设备硬件采集结果。
    /// 同时用于授权申请与本地硬件绑定校验。
    /// </summary>
    public class DeviceHardwareInfo
    {
        /// <summary>
        /// 软件实例唯一标识。
        /// </summary>
        [JsonProperty("clientId")]
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 设备编码。
        /// </summary>
        [JsonProperty("machineCode")]
        public string MachineCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备名称。
        /// </summary>
        [JsonProperty("machineName")]
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// 设备型号。
        /// </summary>
        [JsonProperty("machineModel")]
        public string MachineModel { get; set; } = string.Empty;

        /// <summary>
        /// 操作系统版本。
        /// </summary>
        [JsonProperty("osVersion")]
        public string OsVersion { get; set; } = string.Empty;

        /// <summary>
        /// CPU 标识。
        /// </summary>
        [JsonProperty("cpuId")]
        public string CpuId { get; set; } = string.Empty;

        /// <summary>
        /// BIOS 序列号。
        /// </summary>
        [JsonProperty("biosSerialNumber")]
        public string BiosSerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// 主板序列号。
        /// </summary>
        [JsonProperty("mainboardSerialNumber")]
        public string MainboardSerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// 磁盘序列号。
        /// </summary>
        [JsonProperty("diskSerialNumber")]
        public string DiskSerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// 网卡 MAC 地址。
        /// </summary>
        [JsonProperty("macAddress")]
        public string MacAddress { get; set; } = string.Empty;
    }
}