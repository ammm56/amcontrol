using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using AM.Model.Entity.System;
using AM.Model.License;
using Newtonsoft.Json;
using System;
using System.IO;
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
        private readonly LicenseFileService _licenseFileService;
        private readonly LicenseRuntimeLoader _licenseRuntimeLoader;
        private readonly DeviceReportBufferService _deviceReportBufferService;
        private readonly HttpClient _httpClient;
        private readonly string _serviceUrl;
        private string _cachedRequestKey;
        private string _cachedRequestId;
        private string _cachedRequestTime;

        protected override string MessageSourceName
        {
            get { return "DeviceLicenseApplyClient"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceLicenseApplyClient()
            : this(
                new ClientIdentityService(),
                new HardwareInfoCollector(),
                new LicenseFileService(),
                new LicenseRuntimeLoader(),
                new DeviceReportBufferService(),
                SystemContext.Instance.Reporter)
        {
        }

        public DeviceLicenseApplyClient(IAppReporter reporter)
            : this(
                new ClientIdentityService(reporter),
                new HardwareInfoCollector(reporter),
                new LicenseFileService(reporter),
                new LicenseRuntimeLoader(reporter),
                new DeviceReportBufferService(reporter),
                reporter)
        {
        }

        public DeviceLicenseApplyClient(
            ClientIdentityService clientIdentityService,
            HardwareInfoCollector hardwareInfoCollector,
            LicenseFileService licenseFileService,
            LicenseRuntimeLoader licenseRuntimeLoader,
            DeviceReportBufferService deviceReportBufferService,
            IAppReporter reporter)
            : base(reporter)
        {
            _clientIdentityService = clientIdentityService;
            _hardwareInfoCollector = hardwareInfoCollector;
            _licenseFileService = licenseFileService;
            _licenseRuntimeLoader = licenseRuntimeLoader;
            _deviceReportBufferService = deviceReportBufferService;
            _serviceUrl = GetLicenseServiceUrlFromConfig();
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
            _cachedRequestKey = string.Empty;
            _cachedRequestId = string.Empty;
            _cachedRequestTime = string.Empty;
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

        public Result<LicenseApplyRequest> BuildCurrentRequest(string networkMode = "Online")
        {
            return CreateCurrentRequest(null, null, networkMode);
        }

        public Result<string> ExportCurrentRequestToFile(string filePath, string networkMode = "Offline")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return Fail<string>(-1, "导出文件路径不能为空");
                }

                Result<LicenseApplyRequest> requestResult = CreateCurrentRequest(null, null, networkMode);
                if (!requestResult.Success || requestResult.Item == null)
                {
                    return Fail<string>(requestResult.Code == 0 ? -1 : requestResult.Code, requestResult.Message);
                }

                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string requestJson = JsonConvert.SerializeObject(requestResult.Item, Formatting.Indented);
                File.WriteAllText(filePath, requestJson, Encoding.UTF8);
                return OkSilent(filePath, "授权申请信息导出成功");
            }
            catch (Exception ex)
            {
                return FailLogOnly<string>(-1, "授权申请信息导出失败", ex);
            }
        }

        public async Task<Result<LicenseApplyResponse>> ApplyAsync(LicenseApplyRequest request)
        {
            try
            {
                // 授权申请链路和设备管理链路共用 BackendRequestFailureHelper，
                // 但这里选择 FailLogOnly 而不是 FailSilent，因为授权申请更接近用户主动操作，需要保留明确日志痕迹。
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return FailLogOnly<LicenseApplyResponse>(-1, "未配置后端服务地址");
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
                        if (IsPendingResponse(apiResponse))
                        {
                            return WarnLogOnly<LicenseApplyResponse>(-3, BuildPendingMessage(request, httpResponse.StatusCode, apiResponse));
                        }

                        if (IsPendingPayload(apiResponse == null ? null : apiResponse.Data))
                        {
                            return WarnLogOnly<LicenseApplyResponse>(-3, BuildPendingPayloadMessage(request, apiResponse.Data, apiResponse));
                        }

                        // 真正的 HTTP/业务失败消息统一交给 BackendRequestFailureHelper 生成，
                        // 这样授权申请页、日志和后台排查拿到的是同一格式的失败描述。
                        if (!httpResponse.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
                        {
                            return FailLogOnly<LicenseApplyResponse>(
                                (int)httpResponse.StatusCode,
                                BackendRequestFailureHelper.BuildApiFailureMessage("授权申请", httpResponse.StatusCode, apiResponse));
                        }

                        Result persistResult = PersistLicenseAndReload(apiResponse.Data);
                        if (!persistResult.Success)
                        {
                            return FailLogOnly<LicenseApplyResponse>(persistResult.Code, persistResult.Message);
                        }

                        Result queueResult = _deviceReportBufferService.EnqueueLicenseApplied(apiResponse.Data);
                        if (!queueResult.Success)
                        {
                            WarnLogOnly(queueResult.Code, "授权申请成功，但设备 report 入队失败");
                        }

                        ClearCachedRequest();

                        return OkSilent(apiResponse.Data, "授权申请成功并已写入本地授权文件");
                    }
                }
            }
            catch (Exception ex)
            {
                // 异常失败继续复用统一消息格式，但这里保留 LogOnly 语义，避免用户主动点击后完全静默。
                return FailLogOnly<LicenseApplyResponse>(-1, BackendRequestFailureHelper.BuildExceptionMessage("授权申请", ex));
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

                string normalizedSiteCode = string.IsNullOrWhiteSpace(siteCode)
                    ? BackendServiceConfigHelper.GetLicenseSiteCode()
                    : siteCode.Trim();
                string normalizedCustomerCode = string.IsNullOrWhiteSpace(customerCode)
                    ? BackendServiceConfigHelper.GetLicenseCustomerCode()
                    : customerCode.Trim();
                string normalizedNetworkMode = string.IsNullOrWhiteSpace(networkMode) ? "Online" : networkMode.Trim();
                string requestKey = BuildRequestCacheKey(identityResult.Item, hardwareResult.Item, normalizedSiteCode, normalizedCustomerCode, normalizedNetworkMode);
                string requestId = requestKey == _cachedRequestKey && !string.IsNullOrWhiteSpace(_cachedRequestId)
                    ? _cachedRequestId
                    : BuildRequestId();
                string requestTime = requestKey == _cachedRequestKey && !string.IsNullOrWhiteSpace(_cachedRequestTime)
                    ? _cachedRequestTime
                    : BuildRequestTime();

                DeviceHardwareInfo deviceInfo = hardwareResult.Item;
                string configuredMachineModel = BackendServiceConfigHelper.GetLicenseMachineModel();
                if (!string.IsNullOrWhiteSpace(configuredMachineModel))
                {
                    deviceInfo.MachineModel = configuredMachineModel;
                }

                var request = new LicenseApplyRequest
                {
                    RequestId = requestId,
                    RequestTime = requestTime,
                    LicenseProtocolVersion = LicenseConstants.LicenseProtocolVersion,
                    Software = new LicenseApplySoftware
                    {
                        AppCategory = BackendServiceConfigHelper.GetDesktopAppCategory(),
                        AppCode = string.IsNullOrWhiteSpace(identityResult.Item.AppCode) ? BackendServiceConfigHelper.GetDesktopAppCode() : identityResult.Item.AppCode,
                        AppName = BackendServiceConfigHelper.GetDesktopAppName(),
                        AppEdition = BackendServiceConfigHelper.GetDesktopAppEdition(),
                        AppVersion = AM.Tools.Tools.GetAppVersionText(),
                        TargetFramework = BackendServiceConfigHelper.GetDesktopAppTargetFramework(),
                        UiPlatform = BackendServiceConfigHelper.GetDesktopAppUiPlatform(),
                        Vendor = BackendServiceConfigHelper.GetDesktopAppVendor()
                    },
                    Device = deviceInfo,
                    Environment = new LicenseApplyEnvironment
                    {
                        SiteCode = normalizedSiteCode,
                        CustomerCode = normalizedCustomerCode,
                        NetworkMode = normalizedNetworkMode
                    }
                };

                request.Signature = new LicenseApplyRequestSignature
                {
                    Algorithm = LicenseConstants.RequestSignatureAlgorithm,
                    ContentSha256 = ComputeRequestSha256(request)
                };

                Result<string> signResult = CreateRequestSigningSignatureText(request);
                if (!signResult.Success || string.IsNullOrWhiteSpace(signResult.Item))
                {
                    return Fail<LicenseApplyRequest>(signResult.Code == 0 ? -1 : signResult.Code, signResult.Message);
                }

                request.Signature.SignText = signResult.Item;
                CacheRequestForRetry(requestKey, request.RequestId, request.RequestTime);

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

        private static string BuildRequestTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static string ComputeRequestSha256(LicenseApplyRequest request)
        {
            string signingPayloadJson = BuildRequestSigningPayloadJson(request);
            return ComputeSha256Hex(signingPayloadJson);
        }

        /// <summary>
        /// 生成授权申请消息的签名文本。
        /// 命名显式使用 RequestSigning 口径，避免与 license.lic 的本地验签链路混淆。
        /// </summary>
        private Result<string> CreateRequestSigningSignatureText(LicenseApplyRequest request)
        {
            try
            {
                Result<string> privateKeyResult = LoadRequestSigningPrivateKeyText();
                if (!privateKeyResult.Success || string.IsNullOrWhiteSpace(privateKeyResult.Item))
                {
                    return Fail<string>(privateKeyResult.Code == 0 ? -1 : privateKeyResult.Code, privateKeyResult.Message);
                }

                string signingPayloadJson = BuildRequestSigningPayloadJson(request);
                byte[] dataBytes = Encoding.UTF8.GetBytes(signingPayloadJson);

                using (RSACryptoServiceProvider rsa = CreateRsaFromRequestSigningPrivateKeyText(privateKeyResult.Item))
                {
                    byte[] signatureBytes = rsa.SignData(dataBytes, CryptoConfig.MapNameToOID("SHA256"));
                    return OkSilent(Convert.ToBase64String(signatureBytes), "授权申请签名生成成功");
                }
            }
            catch (Exception ex)
            {
                return Fail<string>(-1, "生成授权申请签名失败", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 读取授权申请签名私钥文本。
        /// 该私钥仅用于设备侧授权申请消息签名，不参与本地 license.lic 验签。
        /// 它与“授权许可验签公钥”不是一对，两者分别属于不同业务链路。
        /// </summary>
        private Result<string> LoadRequestSigningPrivateKeyText()
        {
            try
            {
                string privateKeyFilePath = BackendServiceConfigHelper.GetLicenseRequestSigningPrivateKeyFilePath();
                if (string.IsNullOrWhiteSpace(privateKeyFilePath))
                {
                    return Fail<string>(-1, "授权请求私钥文件路径为空，无法生成签名请求");
                }

                if (!File.Exists(privateKeyFilePath))
                {
                    return Fail<string>(-2, "授权请求私钥文件不存在: " + privateKeyFilePath);
                }

                string fileText = File.ReadAllText(privateKeyFilePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(fileText))
                {
                    return Fail<string>(-3, "授权请求私钥文件内容为空");
                }

                return OkSilent(fileText.Trim(), "已读取授权请求私钥文件");
            }
            catch (Exception ex)
            {
                return Fail<string>(-1, "读取授权请求私钥失败", ReportChannels.Log, ex);
            }
        }

        private static string BuildRequestSigningPayloadJson(LicenseApplyRequest request)
        {
            LicenseApplyRequest normalizedRequest = request ?? new LicenseApplyRequest();
            var signingPayload = new
            {
                requestId = (normalizedRequest.RequestId ?? string.Empty).Trim(),
                requestTime = (normalizedRequest.RequestTime ?? string.Empty).Trim(),
                licenseProtocolVersion = (normalizedRequest.LicenseProtocolVersion ?? string.Empty).Trim(),
                software = normalizedRequest.Software ?? new LicenseApplySoftware(),
                device = normalizedRequest.Device ?? new DeviceHardwareInfo(),
                environment = normalizedRequest.Environment ?? new LicenseApplyEnvironment()
            };

            return JsonConvert.SerializeObject(signingPayload, Formatting.None);
        }

        private static string ComputeSha256Hex(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(text ?? string.Empty));
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

        private static string GetLicenseServiceUrlFromConfig()
        {
            return BackendServiceConfigHelper.GetBackendServiceUrl();
        }

        private static bool IsPendingResponse(DeviceApiResponse<LicenseApplyResponse> apiResponse)
        {
            return apiResponse != null &&
                   !apiResponse.Success &&
                   string.Equals(apiResponse.ErrorCode, "LicensePending", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPendingPayload(LicenseApplyResponse response)
        {
            if (response == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(response.LicenseText) &&
                (string.Equals(response.Status, "Pending", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(response.Status, "PendingReview", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        private static string BuildPendingMessage(LicenseApplyRequest request, HttpStatusCode statusCode, DeviceApiResponse<LicenseApplyResponse> apiResponse)
        {
            string baseMessage = BackendRequestFailureHelper.BuildApiFailureMessage("授权模板未命中，等待管理员处理", statusCode, apiResponse);
            return string.Format(
                "{0} requestId={1}; traceId={2}",
                baseMessage,
                request == null ? string.Empty : request.RequestId ?? string.Empty,
                apiResponse == null ? string.Empty : apiResponse.TraceId ?? string.Empty);
        }

        private static string BuildPendingPayloadMessage(LicenseApplyRequest request, LicenseApplyResponse response, DeviceApiResponse<LicenseApplyResponse> apiResponse)
        {
            string message = apiResponse == null || string.IsNullOrWhiteSpace(apiResponse.Message)
                ? "授权申请已受理，等待管理员处理"
                : apiResponse.Message;

            return string.Format(
                "{0} requestId={1}; status={2}; traceId={3}",
                message,
                request == null ? string.Empty : request.RequestId ?? string.Empty,
                response == null ? string.Empty : response.Status ?? string.Empty,
                apiResponse == null ? string.Empty : apiResponse.TraceId ?? string.Empty);
        }

        private void CacheRequestForRetry(string requestKey, string requestId, string requestTime)
        {
            _cachedRequestKey = requestKey ?? string.Empty;
            _cachedRequestId = requestId ?? string.Empty;
            _cachedRequestTime = requestTime ?? string.Empty;
        }

        private void ClearCachedRequest()
        {
            _cachedRequestKey = string.Empty;
            _cachedRequestId = string.Empty;
            _cachedRequestTime = string.Empty;
        }

        private static string BuildRequestCacheKey(SysClientIdentityEntity identity, DeviceHardwareInfo hardware, string siteCode, string customerCode, string networkMode)
        {
            return string.Join("|", new[]
            {
                identity == null ? string.Empty : identity.ClientId ?? string.Empty,
                identity == null ? string.Empty : identity.MachineCode ?? string.Empty,
                identity == null ? string.Empty : identity.AppCode ?? string.Empty,
                hardware == null ? string.Empty : hardware.CpuId ?? string.Empty,
                hardware == null ? string.Empty : hardware.MainboardSerialNumber ?? string.Empty,
                siteCode ?? string.Empty,
                customerCode ?? string.Empty,
                networkMode ?? string.Empty
            });
        }

        /// <summary>
        /// 从授权申请签名私钥文本创建 RSA 提供程序。
        /// 命名显式使用 RequestSigning 口径，避免与本地 license.lic 验签使用的公钥链路混淆。
        /// </summary>
        private static RSACryptoServiceProvider CreateRsaFromRequestSigningPrivateKeyText(string privateKeyText)
        {
            string trimmed = (privateKeyText ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                throw new CryptographicException("授权请求私钥内容为空。");
            }

            var rsa = new RSACryptoServiceProvider();
            if (trimmed.StartsWith("<RSAKeyValue>", StringComparison.OrdinalIgnoreCase))
            {
                rsa.FromXmlString(trimmed);
                return rsa;
            }

            if (trimmed.Contains("BEGIN PRIVATE KEY"))
            {
                rsa.ImportParameters(DecodePkcs8PrivateKey(GetPemBytes(trimmed, "PRIVATE KEY")));
                return rsa;
            }

            if (trimmed.Contains("BEGIN RSA PRIVATE KEY"))
            {
                rsa.ImportParameters(DecodeRsaPrivateKey(GetPemBytes(trimmed, "RSA PRIVATE KEY")));
                return rsa;
            }

            throw new CryptographicException("授权请求私钥格式不受支持。");
        }

        private static byte[] GetPemBytes(string pemText, string sectionName)
        {
            string normalized = (pemText ?? string.Empty)
                .Replace("-----BEGIN " + sectionName + "-----", string.Empty)
                .Replace("-----END " + sectionName + "-----", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Trim();

            return Convert.FromBase64String(normalized);
        }

        private static RSAParameters DecodePkcs8PrivateKey(byte[] privateKey)
        {
            using (var memoryStream = new MemoryStream(privateKey))
            using (var reader = new BinaryReader(memoryStream))
            {
                ReadAsnSequence(reader);
                ReadAsnInteger(reader);
                ReadAsnSequence(reader);
                reader.ReadBytes(9);
                ReadAsnNull(reader);

                if (reader.ReadByte() != 0x04)
                {
                    throw new CryptographicException("无效的 PKCS#8 私钥格式。");
                }

                int privateKeyLength = ReadAsnLength(reader);
                byte[] rsaPrivateKey = reader.ReadBytes(privateKeyLength);
                return DecodeRsaPrivateKey(rsaPrivateKey);
            }
        }

        private static RSAParameters DecodeRsaPrivateKey(byte[] privateKey)
        {
            using (var memoryStream = new MemoryStream(privateKey))
            using (var reader = new BinaryReader(memoryStream))
            {
                ReadAsnSequence(reader);
                ReadAsnInteger(reader);

                return new RSAParameters
                {
                    Modulus = ReadAsnInteger(reader),
                    Exponent = ReadAsnInteger(reader),
                    D = ReadAsnInteger(reader),
                    P = ReadAsnInteger(reader),
                    Q = ReadAsnInteger(reader),
                    DP = ReadAsnInteger(reader),
                    DQ = ReadAsnInteger(reader),
                    InverseQ = ReadAsnInteger(reader)
                };
            }
        }

        private static void ReadAsnSequence(BinaryReader reader)
        {
            byte tag = reader.ReadByte();
            if (tag != 0x30)
            {
                throw new CryptographicException("无效的 ASN.1 SEQUENCE 标签。");
            }

            ReadAsnLength(reader);
        }

        private static byte[] ReadAsnInteger(BinaryReader reader)
        {
            byte tag = reader.ReadByte();
            if (tag != 0x02)
            {
                throw new CryptographicException("无效的 ASN.1 INTEGER 标签。");
            }

            int length = ReadAsnLength(reader);
            byte[] value = reader.ReadBytes(length);
            if (value.Length > 1 && value[0] == 0x00)
            {
                byte[] trimmed = new byte[value.Length - 1];
                Buffer.BlockCopy(value, 1, trimmed, 0, trimmed.Length);
                return trimmed;
            }

            return value;
        }

        private static int ReadAsnLength(BinaryReader reader)
        {
            int length = reader.ReadByte();
            if ((length & 0x80) == 0)
            {
                return length;
            }

            int bytesCount = length & 0x7F;
            if (bytesCount == 0 || bytesCount > 4)
            {
                throw new CryptographicException("无效的 ASN.1 长度编码。");
            }

            byte[] bytes = reader.ReadBytes(bytesCount);
            int result = 0;
            for (int index = 0; index < bytes.Length; index++)
            {
                result = (result << 8) | bytes[index];
            }

            return result;
        }

        private static void ReadAsnNull(BinaryReader reader)
        {
            byte tag = reader.ReadByte();
            if (tag != 0x05)
            {
                throw new CryptographicException("无效的 ASN.1 NULL 标签。");
            }

            int length = ReadAsnLength(reader);
            if (length > 0)
            {
                reader.ReadBytes(length);
            }
        }

        private Result PersistLicenseAndReload(LicenseApplyResponse response)
        {
            if (response == null)
            {
                return FailLogOnly(-1, "授权申请失败，本地授权结果为空");
            }

            if (string.IsNullOrWhiteSpace(response.LicenseText))
            {
                return FailLogOnly(-2, "授权申请失败，未返回 licenseText");
            }

            Result writeResult = _licenseFileService.WriteLicenseText(response.LicenseText);
            if (!writeResult.Success)
            {
                return FailLogOnly(writeResult.Code, "授权申请成功，但本地授权文件写入失败");
            }

            Result<DeviceLicenseState> loadResult = _licenseRuntimeLoader.Load();
            if (!loadResult.Success || loadResult.Item == null)
            {
                return FailLogOnly(loadResult.Code == 0 ? -1 : loadResult.Code, "授权申请成功，但本地授权重载失败");
            }

            if (!loadResult.Item.IsValid)
            {
                return FailLogOnly(-3, "授权申请成功，但本地授权校验未通过");
            }

            return OkSilent("授权文件落盘并重载成功");
        }
    }
}