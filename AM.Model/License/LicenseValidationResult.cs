using System;

namespace AM.Model.License
{
    /// <summary>
    /// License 校验结果。
    /// 由文件读取、验签、硬件绑定和有效期校验共同产出。
    /// </summary>
    public class LicenseValidationResult
    {
        /// <summary>
        /// 是否校验通过。
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 结果码。
        /// 用于后续映射统一 Result/日志语义。
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// 校验消息。
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 是否存在授权文件。
        /// </summary>
        public bool HasLicenseFile { get; set; }

        /// <summary>
        /// 是否通过签名校验。
        /// </summary>
        public bool IsSignatureValid { get; set; }

        /// <summary>
        /// 是否通过硬件绑定校验。
        /// </summary>
        public bool IsHardwareMatched { get; set; }

        /// <summary>
        /// 是否已过期。
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// 是否处于宽限期。
        /// </summary>
        public bool IsInGracePeriod { get; set; }

        /// <summary>
        /// 到期时间。
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 校验完成时间。
        /// </summary>
        public DateTime ValidatedAt { get; set; } = DateTime.Now;
    }
}