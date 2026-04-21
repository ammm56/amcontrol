using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AM.Model.Device
{
    /// <summary>
    /// 设备注册请求。
    /// 设备软件首次接入设备管理接口时，使用该模型向后端申请 DeviceId 与 DeviceToken。
    /// </summary>
    public class DeviceRegisterRequest
    {
        /// <summary>
        /// 设备标识。
        /// 当前由设备端按 `Setting.DeviceId > MachineCode > ClientId` 的优先级生成，后续将作为设备管理接口路径参数使用。
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// 设备名称。
        /// 当前通常取本机设备名称或配置中的 MachineName，主要用于后端展示与检索。
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 设备类型。
        /// 当前客户端固定写入 `amcontrol`，用于区分接入来源。
        /// </summary>
        [JsonProperty("deviceType")]
        public string DeviceType { get; set; } = string.Empty;

        /// <summary>
        /// 设备当前 IPv4 地址。
        /// 主要用于诊断与平台侧展示，采集失败时允许为空。
        /// </summary>
        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// 扩展附加信息。
        /// 当前设备端会写入 clientId、machineCode、machineName、appCode 等辅助定位字段。
        /// </summary>
        [JsonProperty("extra")]
        public Dictionary<string, string> Extra { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 设备注册响应载荷。
    /// 当 `/api/devices/register` 调用成功时，后端会在统一响应包装的 data 中返回该模型。
    /// </summary>
    public class DeviceRegisterResponse
    {
        /// <summary>
        /// 后端确认后的设备标识。
        /// 当前设备端会将其保存到 config.json，后续心跳、report、refresh-token 都使用该值作为路径参数。
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// 后端签发的设备 token。
        /// 当前设备端会持久化到 config.json，并在心跳与 report 请求头中以 `X-Device-Token` 形式携带。
        /// </summary>
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; } = string.Empty;

        /// <summary>
        /// 注册动作语义。
        /// 常见值例如 `Registered`、`Reconnected`，用于区分是首次注册还是已有设备重连。
        /// </summary>
        [JsonProperty("registrationAction")]
        public string RegistrationAction { get; set; } = string.Empty;

        /// <summary>
        /// 后端记录的注册完成时间。
        /// 当前主要用于日志、界面展示与联调诊断。
        /// </summary>
        [JsonProperty("registeredAt")]
        public DateTime? RegisteredAt { get; set; }
    }

    /// <summary>
    /// 设备令牌刷新响应载荷。
    /// 当 `/api/devices/{id}/refresh-token` 调用成功时，后端会在统一响应包装的 data 中返回该模型。
    /// </summary>
    public class DeviceTokenRefreshResponse
    {
        /// <summary>
        /// 刷新后对应的设备标识。
        /// 正常情况下应与本地当前 DeviceId 保持一致。
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// 新签发的设备 token。
        /// 设备端收到后应立即覆盖本地旧 token。
        /// </summary>
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; } = string.Empty;

        /// <summary>
        /// 后端记录的 token 刷新时间。
        /// 当前主要用于联调与日志定位。
        /// </summary>
        [JsonProperty("refreshedAt")]
        public DateTime? RefreshedAt { get; set; }
    }
}