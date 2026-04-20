using Newtonsoft.Json;

namespace AM.Model.License
{
    /// <summary>
    /// 设备侧授权申请请求。
    /// 对应 POST /api/license/apply 的请求体。
    /// </summary>
    public class LicenseApplyRequest
    {
        /// <summary>
        /// 本次授权申请唯一标识。
        /// 同一次重试应保持不变。
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// 请求时间。
        /// </summary>
        [JsonProperty("requestTime")]
        public string RequestTime { get; set; } = string.Empty;

        /// <summary>
        /// 授权协议版本。
        /// </summary>
        [JsonProperty("licenseProtocolVersion")]
        public string LicenseProtocolVersion { get; set; } = string.Empty;

        /// <summary>
        /// 软件信息。
        /// </summary>
        [JsonProperty("software")]
        public LicenseApplySoftware Software { get; set; } = new LicenseApplySoftware();

        /// <summary>
        /// 当前设备硬件信息。
        /// </summary>
        [JsonProperty("device")]
        public DeviceHardwareInfo Device { get; set; } = new DeviceHardwareInfo();

        /// <summary>
        /// 环境信息。
        /// </summary>
        [JsonProperty("environment")]
        public LicenseApplyEnvironment Environment { get; set; } = new LicenseApplyEnvironment();

        /// <summary>
        /// 请求签名摘要信息。
        /// </summary>
        [JsonProperty("signature")]
        public LicenseApplyRequestSignature Signature { get; set; } = new LicenseApplyRequestSignature();
    }

    /// <summary>
    /// 授权申请中的软件信息。
    /// </summary>
    public class LicenseApplySoftware
    {
        [JsonProperty("appCategory")]
        public string AppCategory { get; set; } = string.Empty;

        [JsonProperty("appCode")]
        public string AppCode { get; set; } = string.Empty;

        [JsonProperty("appName")]
        public string AppName { get; set; } = string.Empty;

        [JsonProperty("appEdition")]
        public string AppEdition { get; set; } = string.Empty;

        [JsonProperty("appVersion")]
        public string AppVersion { get; set; } = string.Empty;

        [JsonProperty("targetFramework")]
        public string TargetFramework { get; set; } = string.Empty;

        [JsonProperty("uiPlatform")]
        public string UiPlatform { get; set; } = string.Empty;

        [JsonProperty("vendor")]
        public string Vendor { get; set; } = string.Empty;
    }

    /// <summary>
    /// 授权申请中的部署环境信息。
    /// </summary>
    public class LicenseApplyEnvironment
    {
        [JsonProperty("siteCode")]
        public string SiteCode { get; set; } = string.Empty;

        [JsonProperty("customerCode")]
        public string CustomerCode { get; set; } = string.Empty;

        [JsonProperty("networkMode")]
        public string NetworkMode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 授权申请中的请求摘要信息。
    /// </summary>
    public class LicenseApplyRequestSignature
    {
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; } = string.Empty;

        [JsonProperty("contentSha256")]
        public string ContentSha256 { get; set; } = string.Empty;

        [JsonProperty("signText")]
        public string SignText { get; set; } = string.Empty;
    }
}