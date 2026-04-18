using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using AM.Model.Entity.System;
using AM.Model.License;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 设备授权申请客户端。
    /// </summary>
    public class DeviceLicenseApplyClient : ServiceBase
    {
        private readonly ClientIdentityService _clientIdentityService;
        private readonly HardwareInfoCollector _hardwareInfoCollector;
        private readonly HttpClient _httpClient;
        private readonly string _serviceUrl;

        protected override string MessageSourceName
        {
            get { return "DeviceLicenseApplyClient"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceLicenseApplyClient()
            : this(new ClientIdentityService(), new HardwareInfoCollector(), SystemContext.Instance.Reporter)
        {
        }

        public DeviceLicenseApplyClient(IAppReporter reporter)
            : this(new ClientIdentityService(reporter), new HardwareInfoCollector(reporter), reporter)
        {
        }

        public DeviceLicenseApplyClient(ClientIdentityService clientIdentityService, HardwareInfoCollector hardwareInfoCollector, IAppReporter reporter)
            : base(reporter)
        {
            _clientIdentityService = clientIdentityService;
            _hardwareInfoCollector = hardwareInfoCollector;
            _serviceUrl = GetLicenseServiceUrlFromConfig();
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_serviceUrl);
        }

        public async Task<Result<LicenseApplyResponse>> ApplyCurrentDeviceAsync(string siteCode = null, string customerCode = null, string networkMode = "Online")
        {
            Result<LicenseApplyRequest> requestResult = CreateCurrentRequest(siteCode, customerCode, networkMode);
            if (!requestResult.Success || requestResult.Item == null)
            {
                return Fail<LicenseApplyResponse>(requestResult.Code == 0 ? -1 : requestResult.Code, requestResult.Message);
            }

            return await ApplyAsync(requestResult.Item).ConfigureAwait(false);
        }

        public async Task<Result<LicenseApplyResponse>> ApplyAsync(LicenseApplyRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return Fail<LicenseApplyResponse>(-1, "未配置授权服务地址", ReportChannels.Log);
                }

                if (request == null)
                {
                    return Fail<LicenseApplyResponse>(-2, "授权申请请求不能为空");
                }

                string requestUrl = _serviceUrl.TrimEnd('/') + "/api/license/apply";
                string requestJson = JsonConvert.SerializeObject(request);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl))
                {
                    httpRequest.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false))
                    {
                        string responseText = httpResponse.Content == null
                            ? string.Empty
                            : await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        DeviceApiResponse<LicenseApplyResponse> apiResponse = DeserializeApiResponse<LicenseApplyResponse>(responseText);
                        if (apiResponse != null && !apiResponse.Success && string.Equals(apiResponse.ErrorCode, "LicensePending", StringComparison.OrdinalIgnoreCase))
                        {
                            return Warn<LicenseApplyResponse>(-3, BuildApiFailureMessage("授权模板未命中，等待管理员处理", httpResponse.StatusCode, apiResponse, responseText), ReportChannels.Log);
                        }

                        if (!httpResponse.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
                        {
                            return Fail<LicenseApplyResponse>(
                                (int)httpResponse.StatusCode,
                                BuildApiFailureMessage("授权申请失败", httpResponse.StatusCode, apiResponse, responseText),
                                ReportChannels.Log);
                        }

                        return OkLogOnly(apiResponse.Data, "授权申请成功");
                    }
                }
            }
            catch (Exception ex)
            {
                return Fail<LicenseApplyResponse>(-1, "授权申请异常", ReportChannels.Log, ex);
            }
        }

        private Result<LicenseApplyRequest> CreateCurrentRequest(string siteCode, string customerCode, string networkMode)
        {
            try
            {
                Result<SysClientIdentityEntity> identityResult = _clientIdentityService.GetCurrent();
                if (!identityResult.Success || identityResult.Item == null)
                {
                    return Fail<LicenseApplyRequest>(identityResult.Code == 0 ? -1 : identityResult.Code, "读取客户端身份失败，无法生成授权申请请求");
                }

                Result<DeviceHardwareInfo> hardwareResult = _hardwareInfoCollector.CollectCurrent();
                if (!hardwareResult.Success || hardwareResult.Item == null)
                {
                    return Fail<LicenseApplyRequest>(hardwareResult.Code == 0 ? -1 : hardwareResult.Code, "采集硬件信息失败，无法生成授权申请请求");
                }

                var request = new LicenseApplyRequest
                {
                    RequestId = BuildRequestId(),
                    RequestTime = DateTime.UtcNow,
                    LicenseProtocolVersion = LicenseConstants.LicenseProtocolVersion,
                    Software = new LicenseApplySoftware
                    {
                        AppCategory = "MotionControl",
                        AppCode = identityResult.Item.AppCode ?? "AMControlWinF",
                        AppName = "AM Motion Control",
                        AppEdition = string.Empty,
                        AppVersion = AM.Tools.Tools.GetAppVersionText(),
                        TargetFramework = ".NET Framework 4.6.1",
                        UiPlatform = "WinForms",
                        Vendor = "AM"
                    },
                    Device = hardwareResult.Item,
                    Environment = new LicenseApplyEnvironment
                    {
                        SiteCode = siteCode ?? string.Empty,
                        CustomerCode = customerCode ?? string.Empty,
                        NetworkMode = string.IsNullOrWhiteSpace(networkMode) ? "Online" : networkMode.Trim()
                    }
                };

                request.Signature = new LicenseApplyRequestSignature
                {
                    Algorithm = LicenseConstants.RequestSignatureAlgorithm,
                    ContentSha256 = ComputeRequestSha256(request)
                };

                return OkSilent(request, "生成授权申请请求成功");
            }
            catch (Exception ex)
            {
                return Fail<LicenseApplyRequest>(-1, "生成授权申请请求异常", ReportChannels.Log, ex);
            }
        }

        private static string BuildRequestId()
        {
            return string.Format(
                "{0}-{1}-{2}",
                LicenseConstants.ApplyRequestIdPrefix,
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                AM.Tools.Tools.Guid(8).ToLowerInvariant());
        }

        private static string ComputeRequestSha256(LicenseApplyRequest request)
        {
            var signSource = new
            {
                request.LicenseProtocolVersion,
                request.Software,
                request.Device,
                request.Environment
            };

            string json = JsonConvert.SerializeObject(signSource, Formatting.None);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
                var builder = new StringBuilder(hash.Length * 2);
                for (int index = 0; index < hash.Length; index++)
                {
                    builder.Append(hash[index].ToString("x2"));
                }

                return builder.ToString();
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

        private static string GetLicenseServiceUrlFromConfig()
        {
            try
            {
                return ConfigContext.Instance.Config.Setting.LicenseServiceUrl ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}