using Newtonsoft.Json;
using System;

namespace AM.Model.License
{
    /// <summary>
    /// 按设备查询授权状态的响应载荷。
    /// 当前用于补齐 Pending 之后的服务层追踪能力，不直接驱动 UI 自动流程。
    /// </summary>
    public class LicenseStatusQueryResponse
    {
        /// <summary>
        /// 当前设备标识。
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// 当前客户端标识。
        /// </summary>
        [JsonProperty("clientId")]
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 当前设备编码。
        /// </summary>
        [JsonProperty("machineCode")]
        public string MachineCode { get; set; } = string.Empty;

        /// <summary>
        /// 最近一次授权申请标识。
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// 当前授权记录标识。
        /// </summary>
        [JsonProperty("licenseId")]
        public string LicenseId { get; set; } = string.Empty;

        /// <summary>
        /// 当前授权状态。
        /// 常见值：Pending / Issued / Rejected。
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 返回的授权文本。
        /// 已签发时可落盘到本地 license.lic。
        /// </summary>
        [JsonProperty("licenseText")]
        public string LicenseText { get; set; } = string.Empty;

        /// <summary>
        /// 状态说明。
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 签发时间。
        /// </summary>
        [JsonProperty("issuedAt")]
        public DateTime? IssuedAt { get; set; }

        /// <summary>
        /// 到期时间。
        /// </summary>
        [JsonProperty("expiresAt")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 最近状态刷新时间。
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}