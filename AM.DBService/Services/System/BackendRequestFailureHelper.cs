using AM.Model.Device;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 后端请求失败帮助类。
    /// 统一将网络异常、超时和后端错误压缩为简短的一行描述。
    ///
    /// 当前设备端所有后端客户端都遵循同一条失败消息链：
    /// 1. 若本地前置条件缺失，如未配置 BackendServiceUrl、DeviceId、DeviceToken，则由各 Client 直接返回本地 Fail/FailSilent；
    /// 2. 若 HTTP 已成功发出但返回码或业务 success 不符合预期，则调用 BuildApiFailureMessage/BuildHttpFailureMessage 生成统一消息；
    /// 3. 若 SendAsync 过程中抛出超时、连接不可达或其他异常，则调用 BuildExceptionMessage 统一归类；
    /// 4. 生成出的消息最终作为 Result.Message 回到 UsageUploadWorker、UI 或日志链路中。
    /// </summary>
    internal static class BackendRequestFailureHelper
    {
        private const string BackendTimeoutPrefix = "[BackendTimeout] ";
        private const string BackendUnavailablePrefix = "[BackendUnavailable] ";

        /// <summary>
        /// 构造“后端不可用”消息。
        /// 用于连接失败、域名不可解析、服务未启动等不可达场景。
        /// </summary>
        public static string BuildUnavailableMessage(string action)
        {
            return BackendUnavailablePrefix + string.Format("{0}失败，后端服务不可用", action ?? "后端请求");
        }

        /// <summary>
        /// 构造“请求超时”消息。
        /// 用于 HttpClient 超时或任务取消但语义上属于等待超时的场景。
        /// </summary>
        public static string BuildTimeoutMessage(string action)
        {
            return BackendTimeoutPrefix + string.Format("{0}失败，后端请求超时", action ?? "后端请求");
        }

        /// <summary>
        /// 按 HTTP 状态码、业务错误码和业务消息拼接统一失败描述。
        /// 这是“请求已到达后端，但返回不是成功结果”时的标准消息格式。
        /// </summary>
        public static string BuildHttpFailureMessage(string action, HttpStatusCode statusCode, string reasonPhrase, string errorCode, string message)
        {
            string normalizedErrorCode = string.IsNullOrWhiteSpace(errorCode) ? string.Empty : errorCode.Trim();
            string businessCode = string.IsNullOrWhiteSpace(normalizedErrorCode) ? string.Empty : string.Format("，ErrorCode={0}", normalizedErrorCode);
            string businessMessage = string.IsNullOrWhiteSpace(message) ? string.Empty : string.Format("，Message={0}", message.Trim());
            string localHint = ResolveLocalHint(normalizedErrorCode);
            string localHintMessage = string.IsNullOrWhiteSpace(localHint) ? string.Empty : string.Format("，Hint={0}", localHint);

            return string.Format(
                "{0}失败，HTTP {1} {2}{3}{4}{5}",
                action ?? "后端请求",
                (int)statusCode,
                string.IsNullOrWhiteSpace(reasonPhrase) ? string.Empty : reasonPhrase.Trim(),
                businessCode,
                businessMessage,
                localHintMessage);
        }

        /// <summary>
        /// 从统一 API 包装中提取错误码和消息，再生成标准失败描述。
        /// 当前设备注册、心跳、report、授权申请、按设备查询授权状态等接口都复用该入口。
        /// </summary>
        public static string BuildApiFailureMessage<T>(string action, HttpStatusCode statusCode, DeviceApiResponse<T> apiResponse)
        {
            return BuildHttpFailureMessage(
                action,
                statusCode,
                string.Empty,
                apiResponse == null ? string.Empty : apiResponse.ErrorCode,
                apiResponse == null ? string.Empty : apiResponse.Message);
        }

        /// <summary>
        /// 从异常对象构造统一失败描述。
        /// 当前优先识别“超时”和“后端不可用”两类高频场景，其余异常统一归并为“后端请求异常”。
        /// </summary>
        public static string BuildExceptionMessage(string action, Exception ex)
        {
            if (IsTimeout(ex))
            {
                return BuildTimeoutMessage(action);
            }

            if (IsBackendUnavailable(ex))
            {
                return BuildUnavailableMessage(action);
            }

            return string.Format("{0}失败，后端请求异常", action ?? "后端请求");
        }

        /// <summary>
        /// 判断异常链中是否存在“后端不可用”语义。
        /// 当前把 HttpRequestException、WebException、SocketException 统一视为连接层不可达。
        /// </summary>
        private static bool IsBackendUnavailable(Exception ex)
        {
            while (ex != null)
            {
                if (ex is HttpRequestException || ex is WebException || ex is SocketException)
                {
                    return true;
                }

                ex = ex.InnerException;
            }

            return false;
        }

        /// <summary>
        /// 判断异常链中是否存在超时语义。
        /// 当前把 TimeoutException 和 TaskCanceledException 统一视为请求超时。
        /// </summary>
        private static bool IsTimeout(Exception ex)
        {
            while (ex != null)
            {
                if (ex is TimeoutException || ex is TaskCanceledException)
                {
                    return true;
                }

                ex = ex.InnerException;
            }

            return false;
        }

        /// <summary>
        /// 按后端错误码补充更细的本地联调提示。
        /// 当前主要覆盖设备 AES-GCM 接入后最常见的三类对接错误。
        /// </summary>
        private static string ResolveLocalHint(string errorCode)
        {
            if (string.IsNullOrWhiteSpace(errorCode))
            {
                return string.Empty;
            }

            switch (errorCode.Trim().ToUpperInvariant())
            {
                case "DEVICE_ENVELOPE_DECRYPT_FAILED":
                    return "设备密文信封解密失败，请优先核对 DeviceAppSecret、DeviceKeyVersion、AAD(appCode/deviceId/nonce/alg/keyVersion) 和 Base64Url 编码是否与后端完全一致";

                case "DEVICE_NONCE_REPLAYED":
                    return "设备请求 nonce 被判定重复，请确保每次 register、heartbeat、report 都重新生成 12 字节随机 nonce，不要重发旧密文信封";

                case "DEVICE_APP_CODE_MISMATCH":
                    return "设备请求 appCode 不一致，请统一 X-Device-AppCode、注册上下文和明文业务 DTO 中的 appCode 来源";

                default:
                    return string.Empty;
            }
        }
    }
}