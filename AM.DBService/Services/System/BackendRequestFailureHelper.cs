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
    /// </summary>
    internal static class BackendRequestFailureHelper
    {
        public static string BuildUnavailableMessage(string action)
        {
            return string.Format("{0}失败，后端服务不可用", action ?? "后端请求");
        }

        public static string BuildTimeoutMessage(string action)
        {
            return string.Format("{0}失败，后端请求超时", action ?? "后端请求");
        }

        public static string BuildHttpFailureMessage(string action, HttpStatusCode statusCode, string reasonPhrase, string errorCode, string message)
        {
            string businessCode = string.IsNullOrWhiteSpace(errorCode) ? string.Empty : string.Format("，ErrorCode={0}", errorCode.Trim());
            string businessMessage = string.IsNullOrWhiteSpace(message) ? string.Empty : string.Format("，Message={0}", message.Trim());

            return string.Format(
                "{0}失败，HTTP {1} {2}{3}{4}",
                action ?? "后端请求",
                (int)statusCode,
                string.IsNullOrWhiteSpace(reasonPhrase) ? string.Empty : reasonPhrase.Trim(),
                businessCode,
                businessMessage);
        }

        public static string BuildApiFailureMessage<T>(string action, HttpStatusCode statusCode, DeviceApiResponse<T> apiResponse)
        {
            return BuildHttpFailureMessage(
                action,
                statusCode,
                string.Empty,
                apiResponse == null ? string.Empty : apiResponse.ErrorCode,
                apiResponse == null ? string.Empty : apiResponse.Message);
        }

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
    }
}