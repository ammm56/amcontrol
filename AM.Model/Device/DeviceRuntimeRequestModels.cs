using Newtonsoft.Json;
using System;

namespace AM.Model.Device
{
    /// <summary>
    /// 设备心跳请求。
    /// 当前设备端将实时状态先序列化为一段 JSON 字符串，再通过该模型提交到 heartbeat 接口。
    /// </summary>
    public class DeviceHeartbeatRequest
    {
        /// <summary>
        /// 设备当前状态摘要 JSON。
        /// 当前通常包含 appCode、appVersion、clientId、deviceId、machineCode、machineName、licenseValid、licenseId、loggedIn、loginName、sampledAt。
        /// </summary>
        [JsonProperty("statusJson")]
        public string StatusJson { get; set; } = string.Empty;
    }

    /// <summary>
    /// 设备结构化上报请求。
    /// 当前使用事件上报和结构化设备 report 上报都统一落到该模型，再调用 `/api/devices/{id}/report`。
    /// </summary>
    public class DeviceReportRequest
    {
        /// <summary>
        /// 事件唯一标识。
        /// 用于后端幂等判断和事件追踪；结构化 report 若未显式传入，设备端会在入队时自动生成。
        /// </summary>
        [JsonProperty("eventId")]
        public string EventId { get; set; } = string.Empty;

        /// <summary>
        /// 上报类型。
        /// 当前常见值为 `Info`、`Status`、`Metric`，由调用方或映射逻辑决定。
        /// </summary>
        [JsonProperty("reportType")]
        public string ReportType { get; set; } = string.Empty;

        /// <summary>
        /// 应用编码。
        /// 用于标识本条 report 来源于哪个设备软件应用实例。
        /// </summary>
        [JsonProperty("appCode")]
        public string AppCode { get; set; } = string.Empty;

        /// <summary>
        /// 应用版本。
        /// 当前通常取设备端程序集版本字符串。
        /// </summary>
        [JsonProperty("appVersion")]
        public string AppVersion { get; set; } = string.Empty;

        /// <summary>
        /// 客户端身份标识。
        /// 当前来自 ClientIdentityService，用于标识设备软件实例本身。
        /// </summary>
        [JsonProperty("clientId")]
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 设备编码。
        /// 当前来自客户端身份中的 MachineCode，可为空。
        /// </summary>
        [JsonProperty("machineCode")]
        public string MachineCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备名称。
        /// 当前来自客户端身份中的 MachineName。
        /// </summary>
        [JsonProperty("machineName")]
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// 当前操作用户 ID。
        /// 若当前无登录用户，则允许为空。
        /// </summary>
        [JsonProperty("userId")]
        public int? UserId { get; set; }

        /// <summary>
        /// 当前操作用户名。
        /// 用于把设备操作与具体登录账号关联。
        /// </summary>
        [JsonProperty("loginName")]
        public string LoginName { get; set; } = string.Empty;

        /// <summary>
        /// 页面标识。
        /// 当前用于记录该 report 发生时关联的业务页面或功能入口。
        /// </summary>
        [JsonProperty("pageKey")]
        public string PageKey { get; set; } = string.Empty;

        /// <summary>
        /// 业务动作是否成功。
        /// 当前主要用于登录、授权、页面行为等状态类 report。
        /// </summary>
        [JsonProperty("isSuccess")]
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// 失败原因码。
        /// 当前用于记录登录失败、业务执行失败等场景下的统一失败原因。
        /// </summary>
        [JsonProperty("failReasonCode")]
        public string FailReasonCode { get; set; } = string.Empty;

        /// <summary>
        /// 跟踪标识。
        /// 用于把本地日志、后端日志和单条 report 关联起来。
        /// </summary>
        [JsonProperty("traceId")]
        public string TraceId { get; set; } = string.Empty;

        /// <summary>
        /// 事件发生时间。
        /// 使用事件上报通常取本地事件发生时间；结构化 report 未指定时由设备端自动补为当前 UTC 时间。
        /// </summary>
        [JsonProperty("occurredAt")]
        public DateTime OccurredAt { get; set; }

        /// <summary>
        /// 业务载荷。
        /// 当前不做强类型约束，由调用方根据 reportType 和场景写入匿名对象或结构化对象。
        /// </summary>
        [JsonProperty("payload")]
        public object Payload { get; set; }
    }
}