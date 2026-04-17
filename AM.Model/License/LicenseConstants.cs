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
        /// 授权申请请求前缀。
        /// </summary>
        public const string ApplyRequestIdPrefix = "apply";

        /// <summary>
        /// 授权申请请求摘要算法。
        /// </summary>
        public const string RequestSignatureAlgorithm = "SHA256";

        /// <summary>
        /// 授权明文验签算法。
        /// </summary>
        public const string LicenseSignatureAlgorithm = "RSA-SHA256";

        /// <summary>
        /// 首版固定内置公钥 KeyId。
        /// </summary>
        public const string BuiltInPublicKeyKeyId = "AMCONTROL_RSA_V1";

        /// <summary>
        /// 内置公钥资源名。
        /// 具体资源内容在后续 Phase B 接入。
        /// </summary>
        public const string EmbeddedPublicKeyResourceName = "LicensePublicKey.pem";

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