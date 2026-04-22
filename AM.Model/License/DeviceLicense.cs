using System;
using System.Collections.Generic;

namespace AM.Model.License
{
    /// <summary>
    /// license.lic 解密后的授权明文模型。
    /// </summary>
    public class DeviceLicense
    {
        /// <summary>
        /// 授权 ID。
        /// </summary>
        public string LicenseId { get; set; } = string.Empty;

        /// <summary>
        /// 授权协议版本。
        /// </summary>
        public string LicenseProtocolVersion { get; set; } = string.Empty;

        /// <summary>
        /// 签发时间。
        /// </summary>
        public DateTime? IssueTime { get; set; }

        /// <summary>
        /// 签发方。
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// 软件信息。
        /// </summary>
        public DeviceLicenseSoftware Software { get; set; } = new DeviceLicenseSoftware();

        /// <summary>
        /// 开发版等按范围匹配的授权范围信息。
        /// 当前用于 customerCode、siteCode、machineModel 的本地匹配。
        /// </summary>
        public DeviceLicenseGrantScope GrantScope { get; set; } = new DeviceLicenseGrantScope();

        /// <summary>
        /// 设备绑定信息。
        /// </summary>
        public DeviceLicenseBinding DeviceBinding { get; set; } = new DeviceLicenseBinding();

        /// <summary>
        /// 有效期信息。
        /// </summary>
        public DeviceLicenseValidity Validity { get; set; } = new DeviceLicenseValidity();

        /// <summary>
        /// 功能授权信息。
        /// </summary>
        public DeviceLicenseAuthorization Authorization { get; set; } = new DeviceLicenseAuthorization();

        /// <summary>
        /// 授权签名信息。
        /// </summary>
        public DeviceLicenseSignature Signature { get; set; } = new DeviceLicenseSignature();
    }

    /// <summary>
    /// License 中的软件信息。
    /// </summary>
    public class DeviceLicenseSoftware
    {
        public string AppCategory { get; set; } = string.Empty;

        public string AppCode { get; set; } = string.Empty;

        public string AppName { get; set; } = string.Empty;

        public string AppEdition { get; set; } = string.Empty;

        public string AppVersion { get; set; } = string.Empty;

        /// <summary>
        /// 允许的最小软件版本。
        /// Developer 版授权当前优先按 [MinAppVersion, MaxAppVersion] 范围校验；为空时再回退旧的 AppVersion 单值匹配。
        /// </summary>
        public string MinAppVersion { get; set; } = string.Empty;

        /// <summary>
        /// 允许的最大软件版本。
        /// 许可允许的最小软件版本。
        /// 当前所有授权版型统一按 [MinAppVersion, MaxAppVersion] 闭区间校验运行时版本。
        /// </summary>
        public string MaxAppVersion { get; set; } = string.Empty;
    }

        /// 许可允许的最大软件版本。
        /// 当前所有授权版型统一按 [MinAppVersion, MaxAppVersion] 闭区间校验运行时版本。
    /// License 中按业务范围控制的授权范围。
    /// 开发版当前不再按硬件强绑定校验，而是匹配这里的客户、站点和设备型号。
    /// </summary>
    public class DeviceLicenseGrantScope
    {
        public string CustomerCode { get; set; } = string.Empty;

        public string SiteCode { get; set; } = string.Empty;

        public string MachineModel { get; set; } = string.Empty;
    }

    /// <summary>
    /// License 中的设备绑定信息。
    /// </summary>
    public class DeviceLicenseBinding
    {
        public string ClientId { get; set; } = string.Empty;

        public string MachineCode { get; set; } = string.Empty;

        public string MachineName { get; set; } = string.Empty;

        public string CpuId { get; set; } = string.Empty;

        public string BiosSerialNumber { get; set; } = string.Empty;

        public string MainboardSerialNumber { get; set; } = string.Empty;

        public string DiskSerialNumber { get; set; } = string.Empty;

        public string MacAddress { get; set; } = string.Empty;
    }

    /// <summary>
    /// License 有效期信息。
    /// </summary>
    public class DeviceLicenseValidity
    {
        public string LicenseType { get; set; } = string.Empty;

        public DateTime? NotBefore { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int GraceDays { get; set; }
    }

    /// <summary>
    /// License 中的授权范围。
    /// </summary>
    public class DeviceLicenseAuthorization
    {
        public List<string> ModuleKeys { get; set; } = new List<string>();

        public List<string> PageKeys { get; set; } = new List<string>();
    }

    /// <summary>
    /// License 签名信息。
    /// </summary>
    public class DeviceLicenseSignature
    {
        public string Algorithm { get; set; } = string.Empty;

        public string ContentSha256 { get; set; } = string.Empty;

        public string SignText { get; set; } = string.Empty;

        public string KeyId { get; set; } = string.Empty;
    }
}