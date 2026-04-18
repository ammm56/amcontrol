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
    /// 设备结构化上报客户端。
    /// </summary>
    public class DeviceReportClient : ServiceBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _serviceUrl;

        protected override string MessageSourceName
        {
            get { return "DeviceReportClient"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceReportClient()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public DeviceReportClient(IAppReporter reporter)
            : base(reporter)
        {
            _serviceUrl = GetDeviceServiceUrlFromConfig();
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public async Task<Result> ReportAsync(DeviceReportRequest request)
        {
            try
            {
                Setting setting = ConfigContext.Instance.Config.Setting;
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return FailSilent(-1, "未配置后端服务地址");
                }

                if (string.IsNullOrWhiteSpace(setting.DeviceId))
                {
                    return Fail(-2, "未配置设备 DeviceId，无法执行上报");
                }

                if (string.IsNullOrWhiteSpace(setting.DeviceToken))
                {
                    return Fail(-3, "未配置设备 token，无法执行上报");
                }

                if (request == null)
                {
                    return Fail(-4, "设备上报请求不能为空");
                }

                if (string.IsNullOrWhiteSpace(request.ReportType))
                {
                    return Fail(-5, "设备上报请求缺少 reportType");
                }

                if (request.Payload == null)
                {
                    return Fail(-6, "设备上报请求缺少 payload");
                }

                string requestUrl = string.Format(
                    "{0}/api/devices/{1}/report",
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
                            return FailSilent(
                                (int)httpResponse.StatusCode,
                                BackendRequestFailureHelper.BuildApiFailureMessage("设备结构化上报", httpResponse.StatusCode, apiResponse));
                        }

                        return OkSilent("设备结构化上报成功: " + (request.EventId ?? string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return FailSilent(-1, BackendRequestFailureHelper.BuildExceptionMessage("设备结构化上报", ex));
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

        private static string GetDeviceServiceUrlFromConfig()
        {
            return BackendServiceConfigHelper.GetBackendServiceUrl();
        }
    }
}