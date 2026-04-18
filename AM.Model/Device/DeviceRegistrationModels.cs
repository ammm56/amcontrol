using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AM.Model.Device
{
    /// <summary>
    /// 设备注册请求。
    /// </summary>
    public class DeviceRegisterRequest
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; } = string.Empty;

        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; } = string.Empty;

        [JsonProperty("extra")]
        public Dictionary<string, string> Extra { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 设备注册响应载荷。
    /// </summary>
    public class DeviceRegisterResponse
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; } = string.Empty;

        [JsonProperty("registrationAction")]
        public string RegistrationAction { get; set; } = string.Empty;

        [JsonProperty("registeredAt")]
        public DateTime? RegisteredAt { get; set; }
    }

    /// <summary>
    /// 设备令牌刷新响应载荷。
    /// </summary>
    public class DeviceTokenRefreshResponse
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; } = string.Empty;

        [JsonProperty("refreshedAt")]
        public DateTime? RefreshedAt { get; set; }
    }
}