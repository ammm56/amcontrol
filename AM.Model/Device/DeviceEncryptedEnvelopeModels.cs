using Newtonsoft.Json;

namespace AM.Model.Device
{
    /// <summary>
    /// 设备 AES-GCM 请求信封。
    /// 当前 register、heartbeat、report 三个设备写接口都以该 JSON 结构作为 HTTP body 外层承载。
    /// </summary>
    public class DeviceEncryptedEnvelope
    {
        /// <summary>
        /// AES-GCM 加密后的密文。
        /// 使用 Base64Url 文本传输，避免直接传输二进制。
        /// </summary>
        [JsonProperty("ciphertext")]
        public string Ciphertext { get; set; } = string.Empty;

        /// <summary>
        /// AES-GCM 认证标签。
        /// 使用 Base64Url 文本传输，对应 16 字节认证 tag。
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; } = string.Empty;
    }
}