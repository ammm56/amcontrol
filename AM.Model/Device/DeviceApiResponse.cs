using Newtonsoft.Json;

namespace AM.Model.Device
{
    /// <summary>
    /// 设备与授权 HTTP 接口的统一响应包装。
    /// </summary>
    public class DeviceApiResponse<T>
    {
        /// <summary>
        /// 是否成功。
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// 业务数据。
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// 消息。
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 业务错误码。
        /// </summary>
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 跟踪标识。
        /// </summary>
        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        /// <summary>
        /// 扩展错误信息。
        /// 当前阶段不做强类型约束。
        /// </summary>
        [JsonProperty("errors")]
        public object Errors { get; set; }
    }
}