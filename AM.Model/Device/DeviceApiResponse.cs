using Newtonsoft.Json;

namespace AM.Model.Device
{
    /// <summary>
    /// 设备与授权 HTTP 接口的统一响应包装。
    /// 当前设备注册、token 刷新、设备心跳、设备 report、授权申请等接口都按该结构返回。
    /// </summary>
    public class DeviceApiResponse<T>
    {
        /// <summary>
        /// 是否成功。
        /// 当前调用方通常先判断 HTTP 状态，再判断这里的 success 字段。
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// 业务数据。
        /// 具体类型由泛型参数 T 决定，例如设备注册时为 DeviceRegisterResponse，心跳和 report 当前通常不关心 data 载荷。
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// 响应消息。
        /// 成功时可为空，失败时通常作为本地日志与错误提示的直接消息来源。
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 业务错误码。
        /// 当前设备管理链路重点关注 `DEVICE_TOKEN_*`、`DEVICE_ID_MISMATCH` 等错误码。
        /// </summary>
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 跟踪标识。
        /// 用于联调时关联一次客户端请求与后端日志。
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