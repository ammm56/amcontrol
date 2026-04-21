using AM.Core.Base;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.License;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 授权加解密与验签服务。
    /// 首版实现以下能力：
    /// 1. 从 AM.Tools/Configuration 目录读取“授权许可验签公钥”；
    /// 2. 兼容明文 JSON 和 Base64 文本两种 licenseText 载荷；
    /// 3. 按固定 KeyId 和 RSA-SHA256 规则校验签名。
    /// 注意：这里读取的公钥只服务于本地 license.lic 验签链路，不与授权申请签名私钥构成一对。
    /// </summary>
    public class LicenseCryptoService : ServiceBase
    {
        protected override string MessageSourceName
        {
            get { return "LicenseCryptoService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public LicenseCryptoService()
            : base()
        {
        }

        public LicenseCryptoService(IAppReporter reporter)
            : base(reporter)
        {
        }

        /// <summary>
        /// 将 licenseText 解码为授权明文 JSON。
        /// 当前联调阶段优先支持两种输入：
        /// 1. 已经是 JSON 明文；
        /// 2. Base64 编码后的 UTF8 JSON 文本。
        /// </summary>
        public virtual Result<string> DecodeLicenseText(string licenseText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(licenseText))
                {
                    return Fail<string>(-1, "授权文本为空");
                }

                string trimmed = licenseText.Trim();
                if (LooksLikeJson(trimmed))
                {
                    return OkSilent(trimmed, "授权文本已是 JSON 明文");
                }

                try
                {
                    byte[] bytes = Convert.FromBase64String(trimmed);
                    string decoded = Encoding.UTF8.GetString(bytes).Trim();
                    if (LooksLikeJson(decoded))
                    {
                        return OkSilent(decoded, "授权文本 Base64 解码成功");
                    }
                }
                catch
                {
                }

                return Fail<string>(-2, "授权文本无法解析为 JSON 明文");
            }
            catch (Exception ex)
            {
                return Fail<string>(-1, "授权文本解码异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 验证授权明文 JSON 与签名信息是否一致。
        /// 固定规则：
        /// 1. 使用 Configuration 目录中的“授权许可验签公钥”；
        /// 2. 校验 KeyId；
        /// 3. 校验 contentSha256；
        /// 4. 对去除整个 signature 对象后的最小化 JSON 执行 RSA-SHA256 验签。
        /// 注意：此处不读取也不依赖“授权申请签名私钥”，两把 key 分属不同业务链路。
        /// </summary>
        public virtual Result VerifyLicenseSignature(string licenseJson, DeviceLicense license)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(licenseJson) || license == null)
                {
                    return Fail(-1, "授权内容为空，无法验签");
                }

                DeviceLicenseSignature signature = license.Signature ?? new DeviceLicenseSignature();
                if (!string.Equals(signature.Algorithm, LicenseConstants.LicenseSignatureAlgorithm, StringComparison.OrdinalIgnoreCase))
                {
                    return Fail(-2, "授权签名算法不匹配");
                }

                if (!string.IsNullOrWhiteSpace(signature.KeyId) &&
                    !string.Equals(signature.KeyId, LicenseConstants.BuiltInPublicKeyKeyId, StringComparison.OrdinalIgnoreCase))
                {
                    return Fail(-3, "授权 KeyId 与内置公钥不匹配");
                }

                if (string.IsNullOrWhiteSpace(signature.SignText))
                {
                    return Fail(-4, "授权签名内容为空");
                }

                string canonicalJson = BuildCanonicalLicenseJson(licenseJson);
                string actualSha256 = ComputeSha256Hex(canonicalJson);

                if (!string.IsNullOrWhiteSpace(signature.ContentSha256) &&
                    !string.Equals(actualSha256, signature.ContentSha256.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return Fail(-5, "授权摘要校验失败");
                }

                Result<string> publicKeyResult = LoadLicenseValidationPublicKeyPem();
                if (!publicKeyResult.Success || string.IsNullOrWhiteSpace(publicKeyResult.Item))
                {
                    return Fail(publicKeyResult.Code == 0 ? -6 : publicKeyResult.Code, "读取授权公钥失败");
                }

                byte[] signatureBytes;
                try
                {
                    signatureBytes = Convert.FromBase64String(signature.SignText.Trim());
                }
                catch (Exception ex)
                {
                    return Fail(-7, "授权签名文本不是有效 Base64", ReportChannels.Log, ex);
                }

                using (RSACryptoServiceProvider rsa = CreateRsaFromPem(publicKeyResult.Item))
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(canonicalJson);
                    bool verified = rsa.VerifyData(dataBytes, CryptoConfig.MapNameToOID("SHA256"), signatureBytes);
                    if (!verified)
                    {
                        return Fail(-8, "RSA 验签失败");
                    }
                }

                return OkLogOnly("RSA 验签成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "授权验签异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 从 AM.Tools/Configuration 目录读取授权许可验签公钥文本。
        /// 该公钥仅用于本地 license.lic 验签，不用于设备侧授权申请签名。
        /// </summary>
        public virtual Result<string> LoadLicenseValidationPublicKeyPem()
        {
            try
            {
                string pem = BackendServiceConfigHelper.GetLicenseValidationPublicKeyPem();
                if (string.IsNullOrWhiteSpace(pem))
                {
                    return Fail<string>(-1, "授权公钥为空");
                }

                return OkSilent(pem.Trim(), "读取授权公钥成功");
            }
            catch (Exception ex)
            {
                return Fail<string>(-1, "读取授权公钥异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 构建用于摘要和验签的规范化授权 JSON。
        /// 当前固定移除整个 signature 对象，再输出为单行最小化 JSON。
        /// 这样与服务端签发 license 时写入 contentSha256 和 signText 的规则保持一致。
        /// </summary>
        private static string BuildCanonicalLicenseJson(string licenseJson)
        {
            JObject root = JObject.Parse(licenseJson);
            JProperty signatureProperty = root.Property("signature", StringComparison.OrdinalIgnoreCase);
            if (signatureProperty != null)
            {
                signatureProperty.Remove();
            }

            return root.ToString(Formatting.None);
        }

        /// <summary>
        /// 计算 UTF8 文本的 SHA256 十六进制字符串。
        /// </summary>
        private static string ComputeSha256Hex(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(text ?? string.Empty));
                StringBuilder builder = new StringBuilder(hash.Length * 2);
                for (int index = 0; index < hash.Length; index++)
                {
                    builder.Append(hash[index].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// 判断字符串是否看起来像 JSON 对象。
        /// </summary>
        private static bool LooksLikeJson(string text)
        {
            return !string.IsNullOrWhiteSpace(text) &&
                   text.StartsWith("{", StringComparison.Ordinal) &&
                   text.EndsWith("}", StringComparison.Ordinal);
        }

        /// <summary>
        /// 从 PEM 公钥创建 RSA 提供程序。
        /// .NET Framework 4.6.1 不支持直接导入 PEM，因此这里手动解析 SubjectPublicKeyInfo 或 PKCS#1 RSA PUBLIC KEY。
        /// </summary>
        private static RSACryptoServiceProvider CreateRsaFromPem(string pem)
        {
            string normalizedPem = (pem ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedPem))
            {
                throw new CryptographicException("授权公钥内容为空。");
            }

            RSAParameters parameters;
            if (normalizedPem.Contains("BEGIN RSA PUBLIC KEY"))
            {
                byte[] keyData = GetPemBytes(normalizedPem, "RSA PUBLIC KEY");
                parameters = DecodeRsaPublicKey(keyData);
            }
            else if (normalizedPem.Contains("BEGIN PUBLIC KEY"))
            {
                byte[] keyData = GetPemBytes(normalizedPem, "PUBLIC KEY");
                parameters = DecodeSubjectPublicKeyInfo(keyData);
            }
            else
            {
                throw new CryptographicException("授权公钥 PEM 格式不受支持。");
            }

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);
            return rsa;
        }

        /// <summary>
        /// 从 PEM 段中提取 Base64 正文。
        /// </summary>
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

        /// <summary>
        /// 解码 X509 SubjectPublicKeyInfo 结构中的 RSA 公钥参数。
        /// </summary>
        private static RSAParameters DecodeSubjectPublicKeyInfo(byte[] x509Key)
        {
            using (var memoryStream = new MemoryStream(x509Key))
            using (var reader = new BinaryReader(memoryStream))
            {
                ReadAsnSequence(reader);
                ReadAsnSequence(reader);

                byte[] rsaOid = reader.ReadBytes(9);
                byte[] expectedOid = new byte[] { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };
                if (!rsaOid.SequenceEqual(expectedOid))
                {
                    throw new CryptographicException("公钥 OID 不是 RSA。");
                }

                ReadAsnNull(reader);

                byte bitStringTag = reader.ReadByte();
                if (bitStringTag != 0x03)
                {
                    throw new CryptographicException("无效的 BIT STRING 标签。");
                }

                int bitStringLength = ReadAsnLength(reader);
                reader.ReadByte();
                byte[] publicKeyBytes = reader.ReadBytes(bitStringLength - 1);

                using (var rsaStream = new MemoryStream(publicKeyBytes))
                using (var rsaReader = new BinaryReader(rsaStream))
                {
                    ReadAsnSequence(rsaReader);
                    byte[] modulus = ReadAsnInteger(rsaReader);
                    byte[] exponent = ReadAsnInteger(rsaReader);

                    return new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                }
            }
        }

        /// <summary>
        /// 解码 PKCS#1 RSA PUBLIC KEY 结构中的公钥参数。
        /// </summary>
        private static RSAParameters DecodeRsaPublicKey(byte[] rsaKey)
        {
            using (var memoryStream = new MemoryStream(rsaKey))
            using (var reader = new BinaryReader(memoryStream))
            {
                ReadAsnSequence(reader);
                byte[] modulus = ReadAsnInteger(reader);
                byte[] exponent = ReadAsnInteger(reader);

                return new RSAParameters
                {
                    Modulus = modulus,
                    Exponent = exponent
                };
            }
        }

        /// <summary>
        /// 读取 ASN.1 SEQUENCE。
        /// </summary>
        private static void ReadAsnSequence(BinaryReader reader)
        {
            byte tag = reader.ReadByte();
            if (tag != 0x30)
            {
                throw new CryptographicException("无效的 ASN.1 Sequence 标签。");
            }

            ReadAsnLength(reader);
        }

        /// <summary>
        /// 读取 ASN.1 NULL。
        /// </summary>
        private static void ReadAsnNull(BinaryReader reader)
        {
            byte tag = reader.ReadByte();
            if (tag != 0x05)
            {
                throw new CryptographicException("无效的 ASN.1 Null 标签。");
            }

            int length = ReadAsnLength(reader);
            if (length > 0)
            {
                reader.ReadBytes(length);
            }
        }

        /// <summary>
        /// 读取 ASN.1 INTEGER，自动去掉正号填充字节。
        /// </summary>
        private static byte[] ReadAsnInteger(BinaryReader reader)
        {
            byte tag = reader.ReadByte();
            if (tag != 0x02)
            {
                throw new CryptographicException("无效的 ASN.1 Integer 标签。");
            }

            int length = ReadAsnLength(reader);
            byte[] value = reader.ReadBytes(length);
            if (value.Length > 1 && value[0] == 0x00)
            {
                return value.Skip(1).ToArray();
            }

            return value;
        }

        /// <summary>
        /// 读取 ASN.1 长度字段。
        /// </summary>
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
                throw new CryptographicException("无效的 ASN.1 长度字段。");
            }

            byte[] bytes = reader.ReadBytes(bytesCount);
            int value = 0;
            for (int index = 0; index < bytes.Length; index++)
            {
                value = (value << 8) | bytes[index];
            }

            return value;
        }
    }
}