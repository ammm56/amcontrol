using Newtonsoft.Json;
using System;

namespace AM.Model.Device
{
    /// <summary>
    /// 设备心跳请求。
    /// </summary>
    public class DeviceHeartbeatRequest
    {
        [JsonProperty("statusJson")]
        public string StatusJson { get; set; } = string.Empty;
    }

    /// <summary>
    /// 设备结构化上报请求。
    /// </summary>
    public class DeviceReportRequest
    {
        [JsonProperty("eventId")]
        public string EventId { get; set; } = string.Empty;

        [JsonProperty("reportType")]
        public string ReportType { get; set; } = string.Empty;

        [JsonProperty("appCode")]
        public string AppCode { get; set; } = string.Empty;

        [JsonProperty("appVersion")]
        public string AppVersion { get; set; } = string.Empty;

        [JsonProperty("clientId")]
        public string ClientId { get; set; } = string.Empty;

        [JsonProperty("machineCode")]
        public string MachineCode { get; set; } = string.Empty;

        [JsonProperty("machineName")]
        public string MachineName { get; set; } = string.Empty;

        [JsonProperty("userId")]
        public int? UserId { get; set; }

        [JsonProperty("loginName")]
        public string LoginName { get; set; } = string.Empty;

        [JsonProperty("pageKey")]
        public string PageKey { get; set; } = string.Empty;

        [JsonProperty("isSuccess")]
        public bool? IsSuccess { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; } = string.Empty;

        [JsonProperty("occurredAt")]
        public DateTime OccurredAt { get; set; }

        [JsonProperty("payload")]
        public object Payload { get; set; }
    }
}