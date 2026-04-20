using System.Collections.Generic;

namespace AM.Model.License
{
    /// <summary>
    /// 设备授权相关常量。
    /// </summary>
    public static class LicenseConstants
    {
        /// <summary>
        /// 本地授权文件名。
        /// </summary>
        public const string LicenseFileName = "license.lic";

        /// <summary>
        /// 授权协议版本。
        /// </summary>
        public const string LicenseProtocolVersion = "1.0";

        /// <summary>
        /// 开发版设备软件的 Edition 标识。
        /// 当授权中的 software.appEdition 为该值时，设备侧按开发版范围匹配规则校验，不走常规硬件强绑定。
        /// </summary>
        public const string DeveloperAppEdition = "Developer";

        /// <summary>
        /// 授权申请请求前缀。
        /// </summary>
        public const string ApplyRequestIdPrefix = "apply";

        /// <summary>
        /// 授权申请请求摘要算法。
        /// </summary>
        public const string RequestSignatureAlgorithm = "RSA-SHA256";

        /// <summary>
        /// 授权明文验签算法。
        /// </summary>
        public const string LicenseSignatureAlgorithm = "RSA-SHA256";

        /// <summary>
        /// 授权许可签名中使用的固定 KeyId。
        /// 该 KeyId 只对应“许可验签公钥”这条链路，不对应授权申请签名私钥。
        /// </summary>
        public const string BuiltInPublicKeyKeyId = "AMCONTROL_RSA_V1";

        /// <summary>
        /// 授权许可验签公钥文件名。
        /// 该公钥仅用于验证本地 license.lic 的签名。
        /// 它不与“授权申请签名私钥”构成一对，两者分别属于不同业务链路。
        /// </summary>
        public const string LicenseValidationPublicKeyFileName = "license-validation-public-key.pem";

        /// <summary>
        /// 授权申请签名私钥文件名。
        /// 该私钥仅用于签名设备侧授权申请消息。
        /// 它不与“授权许可验签公钥”构成一对，两者分别属于不同业务链路。
        /// </summary>
        public const string LicenseRequestSigningPrivateKeyFileName = "license-request-signing-private-key.pem";

        /// <summary>
        /// 首版强校验硬件字段。
        /// </summary>
        public static readonly IReadOnlyList<string> StrongBindingFieldKeys = new List<string>
        {
            "ClientId",
            "MachineCode",
            "CpuId"
        };

        /// <summary>
        /// 首版仅用于诊断日志的硬件字段。
        /// </summary>
        public static readonly IReadOnlyList<string> DiagnosticBindingFieldKeys = new List<string>
        {
            "MachineName",
            "BiosSerialNumber",
            "MainboardSerialNumber",
            "DiskSerialNumber",
            "MacAddress"
        };

        /// <summary>
        /// 无有效授权时的最小功能页面白名单。
        /// </summary>
        public static readonly IReadOnlyList<string> DefaultOpenPageKeys = new List<string>
        {
            "Home.Overview",
            "Home.SysStatus",
            "Motion.DI",
            "Motion.DO",
            "Motion.Monitor",
            "PLC.Status",
            "PLC.Monitor",
            "AlarmLog.Current",
            "AlarmLog.History",
            "AlarmLog.RunLog",
            "System.LoginLog"
        };
    }
}