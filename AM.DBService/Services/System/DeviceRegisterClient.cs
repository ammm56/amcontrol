using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using AM.Model.Entity.System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 设备注册与 token 刷新客户端。
    /// </summary>
    public class DeviceRegisterClient : ServiceBase
    {
        /// <summary>
        /// 客户端身份服务。
        /// 用于生成设备注册请求中的 clientId、machineCode、machineName 和 appCode。
        /// </summary>
        private readonly ClientIdentityService _clientIdentityService;

        /// <summary>
        /// 注册与 token 刷新共用的 HTTP 客户端。
        /// 当前为每个 DeviceRegisterClient 实例持有一个固定 HttpClient。
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 设备请求加密服务。
        /// 负责把明文注册请求封装为 AES-GCM 信封与安全头。
        /// </summary>
        private readonly DeviceRequestCryptoService _deviceRequestCryptoService;

        /// <summary>
        /// 后端统一服务地址。
        /// 由 config 中的 BackendServiceUrl 解析得到。
        /// </summary>
        private readonly string _serviceUrl;

        protected override string MessageSourceName
        {
            get { return "DeviceRegisterClient"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceRegisterClient()
            : this(new ClientIdentityService(), SystemContext.Instance.Reporter)
        {
        }

        public DeviceRegisterClient(IAppReporter reporter)
            : this(new ClientIdentityService(reporter), reporter)
        {
        }

        public DeviceRegisterClient(ClientIdentityService clientIdentityService, IAppReporter reporter)
            : base(reporter)
        {
            _clientIdentityService = clientIdentityService;
            _serviceUrl = GetDeviceServiceUrlFromConfig();
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
            _deviceRequestCryptoService = new DeviceRequestCryptoService(reporter);
        }

        /// <summary>
        /// 当前是否已配置设备管理服务地址。
        /// 仅用于判断是否允许发起注册与 token 刷新请求。
        /// </summary>
        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_serviceUrl);
        }

        /// <summary>
        /// 为当前设备构造注册请求并发起注册。
        /// 这是后台工作单元建立设备会话时最常用的入口。
        /// </summary>
        public async Task<Result<DeviceRegisterResponse>> RegisterCurrentDeviceAsync()
        {
            Result<DeviceRegisterRequest> requestResult = CreateCurrentRegisterRequest();
            if (!requestResult.Success || requestResult.Item == null)
            {
                return Fail<DeviceRegisterResponse>(requestResult.Code == 0 ? -1 : requestResult.Code, requestResult.Message);
            }

            return await RegisterAsync(requestResult.Item).ConfigureAwait(false);
        }

        /// <summary>
        /// 发送设备注册请求。
        /// 成功后会把返回的 DeviceId 和 DeviceToken 回写到 config.json。
        /// </summary>
        public async Task<Result<DeviceRegisterResponse>> RegisterAsync(DeviceRegisterRequest request)
        {
            try
            {
                // 第一层失败消息：本地前置条件校验失败，直接返回本地 Fail/FailSilent，
                // 不进入 HTTP 调用，也不会经过 BackendRequestFailureHelper。
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return FailSilent<DeviceRegisterResponse>(-1, "未配置后端服务地址");
                }

                if (request == null)
                {
                    return Fail<DeviceRegisterResponse>(-2, "设备注册请求不能为空");
                }

                if (string.IsNullOrWhiteSpace(request.DeviceId))
                {
                    return Fail<DeviceRegisterResponse>(-3, "设备注册请求缺少 DeviceId");
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Fail<DeviceRegisterResponse>(-4, "设备注册请求缺少设备名称");
                }

                if (string.IsNullOrWhiteSpace(request.DeviceType))
                {
                    return Fail<DeviceRegisterResponse>(-5, "设备注册请求缺少设备类型");
                }

                Result<DeviceEncryptedRequestPackage> encryptedResult = _deviceRequestCryptoService.BuildRegisterPackage(request);
                if (!encryptedResult.Success || encryptedResult.Item == null)
                {
                    return Fail<DeviceRegisterResponse>(encryptedResult.Code == 0 ? -1 : encryptedResult.Code, encryptedResult.Message);
                }

                DeviceEncryptedRequestPackage encryptedPackage = encryptedResult.Item;
                string requestUrl = _serviceUrl.TrimEnd('/') + "/api/devices/register";

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl))
                {
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

                        // 第二层失败消息：HTTP 已返回，但状态码/统一包装/data 不符合预期。
                        // 此时统一由 BackendRequestFailureHelper 拼出一行标准描述，最终成为 Result.Message。
                        DeviceApiResponse<DeviceRegisterResponse> apiResponse = DeserializeApiResponse<DeviceRegisterResponse>(responseText);
                        if (!httpResponse.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
                        {
                            return FailSilent<DeviceRegisterResponse>(
                                (int)httpResponse.StatusCode,
                                BackendRequestFailureHelper.BuildApiFailureMessage("设备注册", httpResponse.StatusCode, apiResponse));
                        }

                        Result saveResult = SaveDeviceRegistrationToConfig(apiResponse.Data.DeviceId, apiResponse.Data.DeviceToken);
                        if (!saveResult.Success)
                        {
                            return Fail<DeviceRegisterResponse>(saveResult.Code, saveResult.Message);
                        }

                        return OkSilent(apiResponse.Data, string.Format("设备注册成功，Action={0}", apiResponse.Data.RegistrationAction));
                    }
                }
            }
            catch (Exception ex)
            {
                // 第三层失败消息：请求过程中抛出异常。
                // 统一归类为超时、后端不可用或通用异常，再返回给上层 worker 做节流日志处理。
                return FailSilent<DeviceRegisterResponse>(-1, BackendRequestFailureHelper.BuildExceptionMessage("设备注册", ex));
            }
        }

        /// <summary>
        /// 使用本地已保存的 DeviceId 与 DeviceToken 向后端刷新 token。
        /// 刷新成功后同样回写最新 token 到 config.json。
        /// </summary>
        public async Task<Result<DeviceTokenRefreshResponse>> RefreshTokenAsync()
        {
            try
            {
                // token 刷新与注册共用同一条消息链：
                // 本地校验失败 -> 直接 Fail；
                // HTTP/业务失败 -> BackendRequestFailureHelper；
                // 异常失败 -> BackendRequestFailureHelper.BuildExceptionMessage。
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return FailSilent<DeviceTokenRefreshResponse>(-1, "未配置后端服务地址");
                }

                Setting setting = ConfigContext.Instance.Config.Setting;
                if (string.IsNullOrWhiteSpace(setting.DeviceId))
                {
                    return Fail<DeviceTokenRefreshResponse>(-2, "未配置设备 DeviceId，无法刷新 token");
                }

                if (string.IsNullOrWhiteSpace(setting.DeviceToken))
                {
                    return Fail<DeviceTokenRefreshResponse>(-3, "未配置设备 token，无法刷新 token");
                }

                string requestUrl = string.Format(
                    "{0}/api/devices/{1}/refresh-token",
                    _serviceUrl.TrimEnd('/'),
                    Uri.EscapeDataString(setting.DeviceId));

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl))
                {
                    httpRequest.Headers.Add("X-Device-Token", setting.DeviceToken);

                    using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false))
                    {
                        string responseText = httpResponse.Content == null
                            ? string.Empty
                            : await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        DeviceApiResponse<DeviceTokenRefreshResponse> apiResponse = DeserializeApiResponse<DeviceTokenRefreshResponse>(responseText);
                        if (!httpResponse.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
                        {
                            return FailSilent<DeviceTokenRefreshResponse>(
                                (int)httpResponse.StatusCode,
                                BackendRequestFailureHelper.BuildApiFailureMessage("刷新设备 token", httpResponse.StatusCode, apiResponse));
                        }

                        Result saveResult = SaveDeviceRegistrationToConfig(apiResponse.Data.DeviceId, apiResponse.Data.DeviceToken);
                        if (!saveResult.Success)
                        {
                            return Fail<DeviceTokenRefreshResponse>(saveResult.Code, saveResult.Message);
                        }

                        return OkSilent(apiResponse.Data, "刷新设备 token 成功");
                    }
                }
            }
            catch (Exception ex)
            {
                return FailSilent<DeviceTokenRefreshResponse>(-1, BackendRequestFailureHelper.BuildExceptionMessage("刷新设备 token", ex));
            }
        }

        /// <summary>
        /// 根据当前客户端身份构建设备注册请求。
        /// DeviceId 的优先级为 config 中现有值，其次 MachineCode，最后回退到 ClientId。
        /// </summary>
        private Result<DeviceRegisterRequest> CreateCurrentRegisterRequest()
        {
            try
            {
                Result<SysClientIdentityEntity> identityResult = _clientIdentityService.GetCurrent();
                if (!identityResult.Success || identityResult.Item == null)
                {
                    return Fail<DeviceRegisterRequest>(identityResult.Code == 0 ? -1 : identityResult.Code, "获取客户端身份失败，无法生成设备注册请求");
                }

                SysClientIdentityEntity identity = identityResult.Item;
                string deviceId = ResolveDeviceId(identity);
                string machineName = string.IsNullOrWhiteSpace(identity.MachineName) ? Environment.MachineName : identity.MachineName.Trim();

                var request = new DeviceRegisterRequest
                {
                    DeviceId = deviceId,
                    Name = machineName,
                    DeviceType = "amcontrol",
                    AppCode = string.IsNullOrWhiteSpace(identity.AppCode) ? BackendServiceConfigHelper.GetDesktopAppCode() : identity.AppCode.Trim(),
                    MachineCode = identity.MachineCode ?? string.Empty,
                    IpAddress = GetLocalIpv4Address(),
                    Extra = new Dictionary<string, string>
                    {
                        { "clientId", identity.ClientId ?? string.Empty },
                        { "machineCode", identity.MachineCode ?? string.Empty },
                        { "machineName", machineName },
                        { "appCode", string.IsNullOrWhiteSpace(identity.AppCode) ? BackendServiceConfigHelper.GetDesktopAppCode() : identity.AppCode },
                        { "siteCode", BackendServiceConfigHelper.GetLicenseSiteCode() }
                    }
                };

                return OkSilent(request, "生成设备注册请求成功");
            }
            catch (Exception ex)
            {
                return Fail<DeviceRegisterRequest>(-1, "生成设备注册请求异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 解析本次设备注册应使用的 DeviceId。
        /// 该值一旦注册成功，会成为后续 heartbeat、report、refresh-token 的路径标识。
        /// </summary>
        private static string ResolveDeviceId(SysClientIdentityEntity identity)
        {
            Setting setting = ConfigContext.Instance.Config.Setting;
            if (!string.IsNullOrWhiteSpace(setting.DeviceId))
            {
                return setting.DeviceId.Trim();
            }

            if (!string.IsNullOrWhiteSpace(identity.MachineCode))
            {
                return identity.MachineCode.Trim();
            }

            return identity.ClientId ?? string.Empty;
        }

        /// <summary>
        /// 将设备注册结果保存到当前配置与 config.json。
        /// 这是设备管理链路持久化 DeviceId / DeviceToken 的唯一收口点。
        /// </summary>
        private Result SaveDeviceRegistrationToConfig(string deviceId, string deviceToken)
        {
            try
            {
                Setting setting = ConfigContext.Instance.Config.Setting;
                setting.DeviceId = deviceId ?? string.Empty;
                setting.DeviceToken = deviceToken ?? string.Empty;

                Result saveResult = AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
                if (!saveResult.Success)
                {
                    return Fail(saveResult.Code, "保存设备注册配置失败");
                }

                return OkSilent("保存设备注册配置成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "保存设备注册配置异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 反序列化设备管理接口的统一响应包装。
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

        /// <summary>
        /// 获取当前机器首个可用 IPv4 地址。
        /// 注册接口仅用于诊断展示，因此采集失败时回退为空字符串。
        /// </summary>
        private static string GetLocalIpv4Address()
        {
            try
            {
                UnicastIPAddressInformation ip = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(x => x != null)
                    .Where(x => x.OperationalStatus == OperationalStatus.Up)
                    .Where(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .SelectMany(x => x.GetIPProperties().UnicastAddresses)
                    .FirstOrDefault(x => x.Address != null && x.Address.AddressFamily == global::System.Net.Sockets.AddressFamily.InterNetwork);

                return ip == null ? string.Empty : ip.Address.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}