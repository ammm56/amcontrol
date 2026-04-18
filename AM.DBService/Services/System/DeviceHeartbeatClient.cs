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
        private readonly HttpClient _httpClient;
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
        }

        public async Task<Result> SendHeartbeatAsync(DeviceHeartbeatRequest request)
        {
            try
            {
                Setting setting = ConfigContext.Instance.Config.Setting;
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return Fail(-1, "未配置设备服务地址", ReportChannels.Log);
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

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl))
                {
                    httpRequest.Headers.Add("X-Device-Token", setting.DeviceToken);
                    httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false))
                    {
                        string responseText = httpResponse.Content == null
                            ? string.Empty
                            : await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        DeviceApiResponse<object> apiResponse = DeserializeApiResponse<object>(responseText);
                        if (!httpResponse.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success)
                        {
                            return Fail(
                                (int)httpResponse.StatusCode,
                                BuildApiFailureMessage("发送设备心跳失败", httpResponse.StatusCode, apiResponse, responseText),
                                ReportChannels.Log);
                        }

                        return OkLogOnly("发送设备心跳成功");
                    }
                }
            }
            catch (Exception ex)
            {
                return Fail(-1, "发送设备心跳异常", ReportChannels.Log, ex);
            }
        }

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

        private static string BuildApiFailureMessage<T>(string title, HttpStatusCode statusCode, DeviceApiResponse<T> apiResponse, string responseText)
        {
            return string.Format(
                "{0}，HTTP {1}，ErrorCode={2}，Message={3}，TraceId={4}，Body={5}",
                title,
                (int)statusCode,
                apiResponse == null ? string.Empty : apiResponse.ErrorCode ?? string.Empty,
                apiResponse == null ? string.Empty : apiResponse.Message ?? string.Empty,
                apiResponse == null ? string.Empty : apiResponse.TraceId ?? string.Empty,
                responseText ?? string.Empty);
        }

        private static string GetDeviceServiceUrlFromConfig()
        {
            try
            {
                return ConfigContext.Instance.Config.Setting.DeviceServiceUrl ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}