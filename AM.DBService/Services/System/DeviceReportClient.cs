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
        /// <summary>
        /// report 发送专用 HTTP 客户端。
        /// 使用事件上报和结构化 report 上报都复用该客户端。
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 后端统一服务地址。
        /// report 接口路径在此基础上拼接 `/api/devices/{id}/report`。
        /// </summary>
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

        /// <summary>
        /// 发送一条设备 report。
        /// 调用方需保证 reportType 与 payload 已准备完整，当前方法只负责鉴权、序列化和结果解析。
        /// </summary>
        public async Task<Result> ReportAsync(DeviceReportRequest request)
        {
            try
            {
                // report 的错误处理与 heartbeat 保持一致：
                // 本地参数校验、本地配置缺失和后端失败分别在不同层处理，最终统一回到 Result.Message。
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

                        // 一旦后端返回 HTTP 非成功、统一包装解析失败，或 success=false，
                        // 都统一压成单行失败描述，供 UsageUploadWorker 直接记录与回滚队列。
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
                // 异常消息链与注册/心跳保持一致，便于后台统一分析网络、超时和通用异常。
                return FailSilent(-1, BackendRequestFailureHelper.BuildExceptionMessage("设备结构化上报", ex));
            }
        }

        /// <summary>
        /// 反序列化设备 report 接口的统一响应包装。
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