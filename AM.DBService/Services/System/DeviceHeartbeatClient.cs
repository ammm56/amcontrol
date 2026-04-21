using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 设备心跳客户端。
    /// </summary>
    public class DeviceHeartbeatClient : ServiceBase
    {
        /// <summary>
        /// 心跳发送专用 HTTP 客户端。
        /// 每个实例固定复用，避免频繁创建连接对象。
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 设备请求加密服务。
        /// 负责把明文心跳 DTO 封装成 AES-GCM 信封。
        /// </summary>
        private readonly DeviceRequestCryptoService _deviceRequestCryptoService;

        /// <summary>
        /// 后端统一服务地址。
        /// 心跳接口路径在此基础上拼接 `/api/devices/{id}/heartbeat`。
        /// </summary>
        private readonly string _serviceUrl;

        protected override string MessageSourceName
        {
            get { return "DeviceHeartbeatClient"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceHeartbeatClient()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public DeviceHeartbeatClient(IAppReporter reporter)
            : base(reporter)
        {
            _serviceUrl = GetDeviceServiceUrlFromConfig();
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
            _deviceRequestCryptoService = new DeviceRequestCryptoService(reporter);
        }

        /// <summary>
        /// 发送设备心跳。
        /// 当前要求本地已存在 DeviceId、DeviceToken，且请求体中必须带有 statusJson。
        /// </summary>
        public async Task<Result> SendHeartbeatAsync(DeviceHeartbeatRequest request)
        {
            try
            {
                // 本地前置条件失败时，消息直接在当前方法内生成，
                // 避免把“未配置 DeviceId / DeviceToken”等本地问题错误包装成后端异常。
                Setting setting = ConfigContext.Instance.Config.Setting;
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return FailSilent(-1, "未配置后端服务地址");
                }

                if (string.IsNullOrWhiteSpace(setting.DeviceId))
                {
                    return Fail(-2, "未配置设备 DeviceId，无法发送心跳");
                }

                if (string.IsNullOrWhiteSpace(setting.DeviceToken))
                {
                    return Fail(-3, "未配置设备 token，无法发送心跳");
                }

                if (request == null)
                {
                    return Fail(-4, "设备心跳请求不能为空");
                }

                if (string.IsNullOrWhiteSpace(request.StatusJson))
                {
                    return Fail(-5, "设备心跳请求缺少 statusJson");
                }

                string requestUrl = string.Format(
                    "{0}/api/devices/{1}/heartbeat",
                    _serviceUrl.TrimEnd('/'),
                    Uri.EscapeDataString(setting.DeviceId));

                Result<DeviceEncryptedRequestPackage> encryptedResult = _deviceRequestCryptoService.BuildHeartbeatPackage(setting.DeviceId, request);
                if (!encryptedResult.Success || encryptedResult.Item == null)
                {
                    return Fail(encryptedResult.Code == 0 ? -1 : encryptedResult.Code, encryptedResult.Message);
                }

                DeviceEncryptedRequestPackage encryptedPackage = encryptedResult.Item;

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl))
                {
                    httpRequest.Headers.Add("X-Device-Token", setting.DeviceToken);
                    httpRequest.Headers.Add("X-Device-AppCode", encryptedPackage.AppCode);
                    httpRequest.Headers.Add("X-Device-Id", encryptedPackage.DeviceId);
                    httpRequest.Headers.Add("X-Device-Nonce", encryptedPackage.Nonce);
                    httpRequest.Headers.Add("X-Device-Alg", encryptedPackage.Algorithm);
                    httpRequest.Headers.Add("X-Device-KeyVersion", encryptedPackage.KeyVersion);
                    httpRequest.Content = new StringContent(JsonConvert.SerializeObject(encryptedPackage.Envelope), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false))
                    {
                        string responseText = httpResponse.Content == null
                            ? string.Empty
                            : await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        // 心跳接口的失败消息统一收敛到 BackendRequestFailureHelper，
                        // 这样 UsageUploadWorker 只需要消费一条 Result.Message 并按是否 token 相关做后续处理。
                        DeviceApiResponse<object> apiResponse = DeserializeApiResponse<object>(responseText);
                        if (!httpResponse.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success)
                        {
                            return FailSilent(
                                (int)httpResponse.StatusCode,
                                BackendRequestFailureHelper.BuildApiFailureMessage("发送设备心跳", httpResponse.StatusCode, apiResponse));
                        }

                        return OkSilent("发送设备心跳成功");
                    }
                }
            }
            catch (Exception ex)
            {
                // 心跳异常最终也被压缩成一行消息，供后台节流日志和 token 失效判断复用。
                return FailSilent(-1, BackendRequestFailureHelper.BuildExceptionMessage("发送设备心跳", ex));
            }
        }

        /// <summary>
        /// 反序列化设备心跳接口的统一响应包装。
        /// </summary>
        private static DeviceApiResponse<T> DeserializeApiResponse<T>(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<DeviceApiResponse<T>>(responseText);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从统一配置中获取设备管理接口根地址。
        /// </summary>
        private static string GetDeviceServiceUrlFromConfig()
        {
            return BackendServiceConfigHelper.GetBackendServiceUrl();
        }
    }
}