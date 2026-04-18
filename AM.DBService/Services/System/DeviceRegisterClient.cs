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
        private readonly ClientIdentityService _clientIdentityService;
        private readonly HttpClient _httpClient;
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
        }

        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_serviceUrl);
        }

        public async Task<Result<DeviceRegisterResponse>> RegisterCurrentDeviceAsync()
        {
            Result<DeviceRegisterRequest> requestResult = CreateCurrentRegisterRequest();
            if (!requestResult.Success || requestResult.Item == null)
            {
                return Fail<DeviceRegisterResponse>(requestResult.Code == 0 ? -1 : requestResult.Code, requestResult.Message);
            }

            return await RegisterAsync(requestResult.Item).ConfigureAwait(false);
        }

        public async Task<Result<DeviceRegisterResponse>> RegisterAsync(DeviceRegisterRequest request)
        {
            try
            {
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

                string requestUrl = _serviceUrl.TrimEnd('/') + "/api/devices/register";
                string requestJson = JsonConvert.SerializeObject(request);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl))
                {
                    httpRequest.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    using (HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false))
                    {
                        string responseText = httpResponse.Content == null
                            ? string.Empty
                            : await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

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
                return FailSilent<DeviceRegisterResponse>(-1, BackendRequestFailureHelper.BuildExceptionMessage("设备注册", ex));
            }
        }

        public async Task<Result<DeviceTokenRefreshResponse>> RefreshTokenAsync()
        {
            try
            {
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
                    IpAddress = GetLocalIpv4Address(),
                    Extra = new Dictionary<string, string>
                    {
                        { "clientId", identity.ClientId ?? string.Empty },
                        { "machineCode", identity.MachineCode ?? string.Empty },
                        { "machineName", machineName },
                        { "appCode", identity.AppCode ?? string.Empty }
                    }
                };

                return OkSilent(request, "生成设备注册请求成功");
            }
            catch (Exception ex)
            {
                return Fail<DeviceRegisterRequest>(-1, "生成设备注册请求异常", ReportChannels.Log, ex);
            }
        }

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