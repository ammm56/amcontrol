using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using AM.Model.License;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 按设备查询授权状态客户端。
    /// 仅补齐服务层追踪链路，不直接驱动 UI 自动轮询。
    /// </summary>
    public class DeviceLicenseStatusClient : ServiceBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _serviceUrl;

        protected override string MessageSourceName
        {
            get { return "DeviceLicenseStatusClient"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceLicenseStatusClient()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public DeviceLicenseStatusClient(IAppReporter reporter)
            : base(reporter)
        {
            _serviceUrl = BackendServiceConfigHelper.GetBackendServiceUrl();
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_serviceUrl);
        }

        public async Task<Result<LicenseStatusQueryResponse>> QueryCurrentDeviceAsync()
        {
            Setting setting = ConfigContext.Instance.Config.Setting;
            if (string.IsNullOrWhiteSpace(setting.DeviceId))
            {
                return Fail<LicenseStatusQueryResponse>(-1, "未配置设备 DeviceId，无法查询授权状态");
            }

            if (string.IsNullOrWhiteSpace(setting.DeviceToken))
            {
                return Fail<LicenseStatusQueryResponse>(-2, "未配置设备 token，无法查询授权状态");
            }

            return await QueryAsync(setting.DeviceId, setting.DeviceToken).ConfigureAwait(false);
        }

        public async Task<Result<LicenseStatusQueryResponse>> QueryAsync(string deviceId, string deviceToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return FailSilent<LicenseStatusQueryResponse>(-1, "未配置后端服务地址");
                }

                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    return Fail<LicenseStatusQueryResponse>(-2, "设备标识不能为空");
                }

                if (string.IsNullOrWhiteSpace(deviceToken))
                {
                    return Fail<LicenseStatusQueryResponse>(-3, "设备 token 不能为空");
                }

                string appCode = BackendServiceConfigHelper.GetDesktopAppCode();
                if (string.IsNullOrWhiteSpace(appCode))
                {
                    return Fail<LicenseStatusQueryResponse>(-4, "未配置桌面应用 AppCode，无法查询授权状态");
                }

                string requestUrl = string.Format(
                    "{0}/api/license/by-device/{1}?appCode={2}",
                    _serviceUrl.TrimEnd('/'),
                    Uri.EscapeDataString(deviceId),
                    Uri.EscapeDataString(appCode));

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl))
                {
                    httpRequest.Headers.Add("X-Device-Token", deviceToken);

                    using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false))
                    {
                        string responseText = httpResponse.Content == null
                            ? string.Empty
                            : await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        DeviceApiResponse<LicenseStatusQueryResponse> apiResponse = DeserializeApiResponse<LicenseStatusQueryResponse>(responseText);
                        if (!httpResponse.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
                        {
                            return FailSilent<LicenseStatusQueryResponse>(
                                (int)httpResponse.StatusCode,
                                BackendRequestFailureHelper.BuildApiFailureMessage("查询设备授权状态", httpResponse.StatusCode, apiResponse));
                        }

                        LicenseStatusQueryResponse response = apiResponse.Data;
                        string status = response.Status ?? string.Empty;
                        if (string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(status, "PendingReview", StringComparison.OrdinalIgnoreCase))
                        {
                            return OkLogOnly(
                                response,
                                string.Format(
                                    "授权状态待处理: requestId={0}; licenseId={1}; traceId={2}",
                                    response.RequestId ?? string.Empty,
                                    response.LicenseId ?? string.Empty,
                                    apiResponse.TraceId ?? string.Empty));
                        }

                        return OkSilent(response, "查询设备授权状态成功");
                    }
                }
            }
            catch (Exception ex)
            {
                return FailSilent<LicenseStatusQueryResponse>(-1, BackendRequestFailureHelper.BuildExceptionMessage("查询设备授权状态", ex));
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
    }
}