using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Globalization;
using System.Text;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 设备请求加密服务。
    /// 负责把 register、heartbeat、report 的明文 DTO 按最新后端协议封装成 `AES-GCM envelope + 安全头`。
    ///
    /// 当前实现说明：
    /// 1. HKDF-SHA256 与 AES-GCM 已统一切换到 BouncyCastle.Cryptography.dll；
    /// 2. 该服务只负责“明文 DTO -> 安全头 + 密文 envelope”的封装，不负责 HTTP 发送；
    /// 3. refresh-token 当前不走该服务，因为后端首版仍只要求 `X-Device-Token`；
    /// 4. 授权申请 `/api/license/apply` 也不走该服务，仍保持原有 RSA-SHA256 请求签名链路。
    /// </summary>
    internal class DeviceRequestCryptoService : ServiceBase
    {
        /// <summary>
        /// 设备写接口约定的算法标识。
        /// 当前固定写入 `X-Device-Alg=A256GCM`，同时参与 AAD 拼接。
        /// </summary>
        private const string AesAlgorithmName = "A256GCM";

        /// <summary>
        /// 设备请求 nonce 长度，单位字节。
        /// 按后端协议固定为 12 字节，对应 AES-GCM 推荐 IV 长度。
        /// </summary>
        private const int NonceLength = 12;

        /// <summary>
        /// AES-GCM 认证标签长度，单位字节。
        /// 当前固定为 16 字节，对应 128 bit tag。
        /// </summary>
        private const int TagLength = 16;

        /// <summary>
        /// HKDF 派生后设备请求密钥长度，单位字节。
        /// 当前固定为 32 字节，对应 AES-256-GCM。
        /// </summary>
        private const int DerivedKeyLength = 32;

        /// <summary>
        /// HKDF info 前缀。
        /// 实际派生时会拼上 `deviceId`，形成后端文档约定的 `autoinboomgate:device-request:v1:{deviceId}`。
        /// </summary>
        private const string HkdfInfoPrefix = "autoinboomgate:device-request:v1:";

        /// <summary>
        /// BouncyCastle 安全随机数生成器。
        /// 当前用于生成每次请求独立的 12 字节 nonce。
        /// </summary>
        private static readonly SecureRandom Random = new SecureRandom();

        protected override string MessageSourceName
        {
            get { return "DeviceRequestCryptoService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceRequestCryptoService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public DeviceRequestCryptoService(IAppReporter reporter)
            : base(reporter)
        {
        }

        /// <summary>
        /// 构建设备注册密文请求包。
        /// register 不携带 `X-Device-Token`，仅依赖 appSecret 派生的请求密钥完成 AES-GCM 封装。
        /// </summary>
        public Result<DeviceEncryptedRequestPackage> BuildRegisterPackage(DeviceRegisterRequest request)
        {
            if (request == null)
            {
                return Fail<DeviceEncryptedRequestPackage>(-1, "设备注册请求不能为空");
            }

            string appCode = ResolveRegisterAppCode(request);
            return BuildEncryptedPackage("设备注册", appCode, request.DeviceId, request);
        }

        /// <summary>
        /// 构建设备心跳密文请求包。
        /// 当前心跳明文 DTO 只包含 `statusJson`，但真正发送前会被统一封装成 `DeviceEncryptedEnvelope`。
        /// </summary>
        public Result<DeviceEncryptedRequestPackage> BuildHeartbeatPackage(string deviceId, DeviceHeartbeatRequest request)
        {
            if (request == null)
            {
                return Fail<DeviceEncryptedRequestPackage>(-1, "设备心跳请求不能为空");
            }

            return BuildEncryptedPackage("设备心跳", BackendServiceConfigHelper.GetDesktopAppCode(), deviceId, request);
        }

        /// <summary>
        /// 构建设备上报密文请求包。
        /// report 允许调用方传入 `request.AppCode`；若为空，则统一回退到配置中的 `DesktopAppCode`。
        /// </summary>
        public Result<DeviceEncryptedRequestPackage> BuildReportPackage(string deviceId, DeviceReportRequest request)
        {
            if (request == null)
            {
                return Fail<DeviceEncryptedRequestPackage>(-1, "设备上报请求不能为空");
            }

            string appCode = string.IsNullOrWhiteSpace(request.AppCode)
                ? BackendServiceConfigHelper.GetDesktopAppCode()
                : request.AppCode.Trim();

            request.AppCode = appCode;
            return BuildEncryptedPackage("设备结构化上报", appCode, deviceId, request);
        }

        /// <summary>
        /// 按 appCode、deviceId 和共享 appSecret 构建一条通用密文请求包。
        ///
        /// 当前关键步骤如下：
        /// 1. 读取 `DeviceAppSecret` 与 `DeviceKeyVersion`；
        /// 2. 生成 12 字节随机 nonce，并转成 Base64Url 文本；
        /// 3. 按后端约定拼接 AAD：`appCode + "\n" + deviceId + "\n" + nonce + "\n" + alg + "\n" + keyVersion`；
        /// 4. 将明文 DTO 序列化为 UTF-8 JSON；
        /// 5. 通过 HKDF-SHA256 派生 32 字节请求密钥；
        /// 6. 使用 AES-256-GCM 得到 ciphertext 与 tag；
        /// 7. 组装成 `DeviceEncryptedRequestPackage`，供 Client 填充请求头与 body。
        /// </summary>
        private Result<DeviceEncryptedRequestPackage> BuildEncryptedPackage(string action, string appCode, string deviceId, object payload)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appCode))
                {
                    return Fail<DeviceEncryptedRequestPackage>(-2, action + "失败，未配置桌面应用 AppCode");
                }

                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    return Fail<DeviceEncryptedRequestPackage>(-3, action + "失败，缺少 DeviceId");
                }

                string appSecret = BackendServiceConfigHelper.GetDeviceAppSecret();
                if (string.IsNullOrWhiteSpace(appSecret))
                {
                    return Fail<DeviceEncryptedRequestPackage>(-4, action + "失败，未配置 DeviceAppSecret");
                }

                int keyVersion = BackendServiceConfigHelper.GetDeviceKeyVersion();
                string keyVersionText = keyVersion.ToString(CultureInfo.InvariantCulture);

                byte[] nonceBytes = GenerateRandomBytes(NonceLength);
                string nonceText = ToBase64Url(nonceBytes);

                // AAD 必须与后端完全一致，否则服务端会返回 DEVICE_ENVELOPE_DECRYPT_FAILED。
                string aadText = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}\n{1}\n{2}\n{3}\n{4}",
                    appCode.Trim(),
                    deviceId.Trim(),
                    nonceText,
                    AesAlgorithmName,
                    keyVersionText);

                // 明文 DTO 统一序列化成 UTF-8 JSON，再执行 HKDF + AES-GCM。
                string plaintextJson = JsonConvert.SerializeObject(payload);
                byte[] requestKey = DeriveRequestKey(appSecret, appCode, deviceId);
                byte[] tag;
                byte[] ciphertext = EncryptAesGcm(
                    requestKey,
                    nonceBytes,
                    Encoding.UTF8.GetBytes(plaintextJson),
                    Encoding.UTF8.GetBytes(aadText),
                    out tag);

                var package = new DeviceEncryptedRequestPackage
                {
                    AppCode = appCode.Trim(),
                    DeviceId = deviceId.Trim(),
                    Nonce = nonceText,
                    Algorithm = AesAlgorithmName,
                    KeyVersion = keyVersionText,
                    Envelope = new DeviceEncryptedEnvelope
                    {
                        Ciphertext = ToBase64Url(ciphertext),
                        Tag = ToBase64Url(tag)
                    }
                };

                return OkSilent(package, action + "加密封装成功");
            }
            catch (Exception ex)
            {
                return Fail<DeviceEncryptedRequestPackage>(-1, action + "加密封装异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 解析注册请求应使用的 appCode。
        /// 优先级：`request.AppCode` > `request.Extra["appCode"]` > `DesktopAppCode`。
        /// </summary>
        private static string ResolveRegisterAppCode(DeviceRegisterRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.AppCode))
            {
                return request.AppCode.Trim();
            }

            string extraAppCode;
            if (request.Extra != null && request.Extra.TryGetValue("appCode", out extraAppCode) && !string.IsNullOrWhiteSpace(extraAppCode))
            {
                return extraAppCode.Trim();
            }

            return BackendServiceConfigHelper.GetDesktopAppCode();
        }

        /// <summary>
        /// 按后端协议派生设备请求密钥。
        ///
        /// 当前口径：
        /// 1. `ikm = UTF8(appSecret)`；
        /// 2. `salt = UTF8(appCode)`；
        /// 3. `info = UTF8("autoinboomgate:device-request:v1:" + deviceId)`；
        /// 4. 输出 32 字节结果，供 AES-256-GCM 使用。
        /// </summary>
        private static byte[] DeriveRequestKey(string appSecret, string appCode, string deviceId)
        {
            byte[] ikm = Encoding.UTF8.GetBytes(appSecret ?? string.Empty);
            byte[] salt = Encoding.UTF8.GetBytes(appCode ?? string.Empty);
            byte[] info = Encoding.UTF8.GetBytes(HkdfInfoPrefix + (deviceId ?? string.Empty));
            return HkdfSha256(ikm, salt, info, DerivedKeyLength);
        }

        /// <summary>
        /// 使用 BouncyCastle 实现 HKDF-SHA256。
        /// 这里不再手写 HMAC 提取与展开逻辑，统一复用 `HkdfBytesGenerator` 保持实现简洁且更贴近标准算法组件。
        /// </summary>
        private static byte[] HkdfSha256(byte[] ikm, byte[] salt, byte[] info, int length)
        {
            byte[] normalizedSalt = salt == null || salt.Length <= 0 ? new byte[0] : salt;
            byte[] normalizedIkm = ikm ?? new byte[0];
            byte[] normalizedInfo = info ?? new byte[0];

            // HKDF 提取 + 展开由 BouncyCastle 内部处理，当前只负责提供输入材料与目标长度。
            var generator = new HkdfBytesGenerator(new Sha256Digest());
            generator.Init(new HkdfParameters(normalizedIkm, normalizedSalt, normalizedInfo));

            byte[] output = new byte[length];
            generator.GenerateBytes(output, 0, output.Length);
            return output;
        }

        /// <summary>
        /// 使用 BouncyCastle 实现 AES-GCM 加密。
        ///
        /// 当前关键步骤：
        /// 1. 将 key、nonce、plaintext、aad 都规范化为非 null 字节数组；
        /// 2. 使用 `AeadParameters` 绑定密钥、nonce、tag 位数和 AAD；
        /// 3. 使用 `GcmBlockCipher(new AesEngine())` 执行加密；
        /// 4. BouncyCastle 输出结果为 `ciphertext || tag`，这里再拆分成两段返回。
        /// </summary>
        private static byte[] EncryptAesGcm(byte[] key, byte[] nonce, byte[] plaintext, byte[] aad, out byte[] tag)
        {
            byte[] normalizedKey = key ?? new byte[0];
            byte[] normalizedNonce = nonce ?? new byte[0];
            byte[] normalizedPlaintext = plaintext ?? new byte[0];
            byte[] normalizedAad = aad ?? new byte[0];

            // AeadParameters 中的 tag 位数使用 bit 单位，因此这里传入 16 * 8 = 128。
            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(normalizedKey), TagLength * 8, normalizedNonce, normalizedAad);
            cipher.Init(true, parameters);

            // BouncyCastle 会把 ciphertext 和 tag 作为连续字节写入 output。
            byte[] output = new byte[cipher.GetOutputSize(normalizedPlaintext.Length)];
            int outputLength = cipher.ProcessBytes(normalizedPlaintext, 0, normalizedPlaintext.Length, output, 0);
            outputLength += cipher.DoFinal(output, outputLength);

            // 最后 16 字节固定是认证 tag，前面的部分是实际密文。
            int cipherLength = outputLength - TagLength;
            byte[] ciphertext = new byte[cipherLength];
            tag = new byte[TagLength];

            Buffer.BlockCopy(output, 0, ciphertext, 0, ciphertext.Length);
            Buffer.BlockCopy(output, ciphertext.Length, tag, 0, tag.Length);

            return ciphertext;
        }

        /// <summary>
        /// 生成指定长度的安全随机字节数组。
        /// 当前只用于 nonce 生成，因此每次请求必须重新调用，禁止缓存复用。
        /// </summary>
        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            Random.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// 将字节数组编码为 Base64Url 文本。
        /// 当前设备安全协议要求 `nonce`、`ciphertext`、`tag` 都采用该编码格式传输。
        /// </summary>
        private static string ToBase64Url(byte[] bytes)
        {
            return Convert.ToBase64String(bytes ?? new byte[0])
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

    }

    /// <summary>
    /// 一次设备加密请求所需的完整发送上下文。
    /// 当前由 Client 用来同时构造安全头和 JSON 信封 body。
    /// </summary>
    internal class DeviceEncryptedRequestPackage
    {
        /// <summary>
        /// 请求头中的 `X-Device-AppCode`。
        /// 当前同时作为 HKDF `salt` 与 AAD 组成部分。
        /// </summary>
        public string AppCode { get; set; } = string.Empty;

        /// <summary>
        /// 请求头中的 `X-Device-Id`。
        /// 当前同时作为 HKDF `info` 尾段与 AAD 组成部分。
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// 当前请求独立生成的 Base64Url nonce。
        /// 将写入 `X-Device-Nonce`，同时作为 AES-GCM IV 使用。
        /// </summary>
        public string Nonce { get; set; } = string.Empty;

        /// <summary>
        /// 请求头中的算法标识。
        /// 当前固定为 `A256GCM`。
        /// </summary>
        public string Algorithm { get; set; } = string.Empty;

        /// <summary>
        /// 请求头中的密钥版本文本。
        /// 当前来源于 `Setting.DeviceKeyVersion`。
        /// </summary>
        public string KeyVersion { get; set; } = string.Empty;

        /// <summary>
        /// 最终发送到 HTTP body 的 AES-GCM 密文信封。
        /// </summary>
        public DeviceEncryptedEnvelope Envelope { get; set; } = new DeviceEncryptedEnvelope();
    }
}