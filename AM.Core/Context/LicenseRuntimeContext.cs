using AM.Model.License;

namespace AM.Core.Context
{
    /// <summary>
    /// 授权运行时上下文。
    /// 独立保存授权文件装载与校验后的最终状态，不与用户上下文混用。
    /// </summary>
    public sealed class LicenseRuntimeContext
    {
        /// <summary>
        /// 全局唯一实例。
        /// </summary>
        public static LicenseRuntimeContext Instance { get; } = new LicenseRuntimeContext();

        /// <summary>
        /// 私有构造。
        /// </summary>
        private LicenseRuntimeContext()
        {
            Current = CreateEmptyState();
        }

        /// <summary>
        /// 当前授权状态快照。
        /// </summary>
        public DeviceLicenseState Current { get; private set; }

        /// <summary>
        /// 当前是否已有授权数据。
        /// </summary>
        public bool HasLicense
        {
            get { return Current != null && Current.HasLicense; }
        }

        /// <summary>
        /// 当前授权是否有效。
        /// </summary>
        public bool IsValid
        {
            get { return Current != null && Current.IsValid; }
        }

        /// <summary>
        /// 写入新的授权状态。
        /// </summary>
        public void Update(DeviceLicenseState state)
        {
            Current = state ?? CreateEmptyState();
        }

        /// <summary>
        /// 重置到无授权状态。
        /// </summary>
        public void Reset()
        {
            Current = CreateEmptyState();
        }

        /// <summary>
        /// 创建默认空状态。
        /// </summary>
        private static DeviceLicenseState CreateEmptyState()
        {
            return new DeviceLicenseState
            {
                HasLicense = false,
                IsSignatureValid = false,
                IsValid = false,
                IsExpired = false,
                IsInGracePeriod = false,
                Message = "尚未加载授权",
                ValidationResult = new LicenseValidationResult
                {
                    Success = false,
                    ErrorCode = string.Empty,
                    Message = "尚未加载授权"
                }
            };
        }
    }
}