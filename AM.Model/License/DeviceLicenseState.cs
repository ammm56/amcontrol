using System;
using System.Collections.Generic;

namespace AM.Model.License
{
    /// <summary>
    /// 授权运行时状态快照。
    /// </summary>
    public class DeviceLicenseState
    {
        /// <summary>
        /// 是否存在授权数据。
        /// </summary>
        public bool HasLicense { get; set; }

        /// <summary>
        /// 是否通过签名校验。
        /// </summary>
        public bool IsSignatureValid { get; set; }

        /// <summary>
        /// 是否最终有效。
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 是否已过期。
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// 是否处于宽限期。
        /// </summary>
        public bool IsInGracePeriod { get; set; }

        /// <summary>
        /// 当前授权 ID。
        /// </summary>
        public string LicenseId { get; set; } = string.Empty;

        /// <summary>
        /// 到期时间。
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 当前状态说明。
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 授权模块集合。
        /// </summary>
        public IReadOnlyList<string> ModuleKeys { get; set; } = new List<string>();

        /// <summary>
        /// 授权页面集合。
        /// </summary>
        public IReadOnlyList<string> PageKeys { get; set; } = new List<string>();

        /// <summary>
        /// 授权明文对象。
        /// </summary>
        public DeviceLicense License { get; set; }

        /// <summary>
        /// 最近一次校验结果。
        /// </summary>
        public LicenseValidationResult ValidationResult { get; set; } = new LicenseValidationResult();
    }
}