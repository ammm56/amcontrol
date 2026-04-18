using System;
using Newtonsoft.Json;

namespace AM.Model.License
{
    /// <summary>
    /// 授权申请成功后的业务载荷。
    /// 对应统一 API 包装中的 data 节点。
    /// </summary>
    public class LicenseApplyResponse
    {
        /// <summary>
        /// 授权记录 ID。
        /// </summary>
        [JsonProperty("licenseId")]
        public string LicenseId { get; set; } = string.Empty;

        /// <summary>
        /// 当前授权记录状态。
        /// 例如 Issued / Pending。
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 授权文本。
        /// 设备侧固定保存该字段到 license.lic。
        /// </summary>
        [JsonProperty("licenseText")]
        public string LicenseText { get; set; } = string.Empty;

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
    }
}