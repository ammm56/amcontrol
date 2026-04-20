using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.License;
using System;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// License 校验服务。
    /// 统一执行验签、硬件绑定和有效期校验。
    /// </summary>
    public class LicenseValidator : ServiceBase
    {
        private readonly HardwareInfoCollector _hardwareInfoCollector;
        private readonly LicenseCryptoService _licenseCryptoService;

        protected override string MessageSourceName
        {
            get { return "LicenseValidator"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public LicenseValidator()
            : this(new HardwareInfoCollector(), new LicenseCryptoService(), SystemContext.Instance.Reporter)
        {
        }

        public LicenseValidator(IAppReporter reporter)
            : this(new HardwareInfoCollector(reporter), new LicenseCryptoService(reporter), reporter)
        {
        }

        /// <summary>
        /// 供本地验证替身使用的无依赖构造。
        /// 仅允许派生类覆写 Validate 时使用，避免触发真实硬件与数据库依赖初始化。
        /// </summary>
        protected LicenseValidator(bool skipDependencyInitialization)
            : base(null)
        {
            if (!skipDependencyInitialization)
            {
                _hardwareInfoCollector = new HardwareInfoCollector();
                _licenseCryptoService = new LicenseCryptoService();
            }
        }

        public LicenseValidator(HardwareInfoCollector hardwareInfoCollector, LicenseCryptoService licenseCryptoService, IAppReporter reporter)
            : base(reporter)
        {
            _hardwareInfoCollector = hardwareInfoCollector;
            _licenseCryptoService = licenseCryptoService;
        }

        /// <summary>
        /// 校验授权明文与当前设备环境。
        /// 有效期比较统一按 UTC 口径执行，避免本地时间与 Z 时间混用时出现歧义。
        /// </summary>
        public virtual Result<LicenseValidationResult> Validate(DeviceLicense license, string licenseJson)
        {
            try
            {
                if (license == null)
                {
                    return Fail<LicenseValidationResult>(-1, "授权对象为空");
                }

                var validation = new LicenseValidationResult
                {
                    HasLicenseFile = true,
                    ValidatedAt = DateTime.UtcNow
                };

                Result signatureResult = _licenseCryptoService.VerifyLicenseSignature(licenseJson, license);
                validation.IsSignatureValid = signatureResult.Success;
                if (!signatureResult.Success)
                {
                    validation.Success = false;
                    validation.ErrorCode = "LICENSE_SIGNATURE_INVALID";
                    validation.Message = signatureResult.Message;
                    return OkLogOnly(validation, validation.Message);
                }

                Result<DeviceHardwareInfo> hardwareResult = _hardwareInfoCollector.CollectCurrent();
                if (!hardwareResult.Success || hardwareResult.Item == null)
                {
                    validation.Success = false;
                    validation.ErrorCode = "LICENSE_HARDWARE_READ_FAILED";
                    validation.Message = "采集本机硬件信息失败";
                    return OkLogOnly(validation, validation.Message);
                }

                validation.IsHardwareMatched = ValidateStrongBindings(license.DeviceBinding, hardwareResult.Item, out string hardwareMessage);
                if (!validation.IsHardwareMatched)
                {
                    validation.Success = false;
                    validation.ErrorCode = "LICENSE_HARDWARE_MISMATCH";
                    validation.Message = hardwareMessage;
                    return OkLogOnly(validation, validation.Message);
                }

                DateTime nowUtc = DateTime.UtcNow;
                DateTime? notBeforeUtc = license.Validity == null ? null : NormalizeToUtc(license.Validity.NotBefore);
                DateTime? expiresAtUtc = license.Validity == null ? null : NormalizeToUtc(license.Validity.ExpiresAt);
                int graceDays = license.Validity == null ? 0 : license.Validity.GraceDays;

                validation.ExpiresAt = expiresAtUtc;

                if (notBeforeUtc.HasValue && nowUtc < notBeforeUtc.Value)
                {
                    validation.Success = false;
                    validation.ErrorCode = "LICENSE_NOT_EFFECTIVE";
                    validation.Message = "授权尚未生效";
                    return OkLogOnly(validation, validation.Message);
                }

                if (expiresAtUtc.HasValue && nowUtc > expiresAtUtc.Value)
                {
                    validation.IsExpired = true;

                    DateTime graceDeadlineUtc = expiresAtUtc.Value.AddDays(graceDays < 0 ? 0 : graceDays);
                    if (nowUtc <= graceDeadlineUtc)
                    {
                        validation.IsInGracePeriod = true;
                        validation.Success = true;
                        validation.Message = "授权已过期，当前处于宽限期";
                        return OkLogOnly(validation, validation.Message);
                    }

                    validation.Success = false;
                    validation.ErrorCode = "LICENSE_EXPIRED";
                    validation.Message = "授权已过期且不在宽限期内";
                    return OkLogOnly(validation, validation.Message);
                }

                validation.Success = true;
                validation.Message = "授权校验通过";
                return OkLogOnly(validation, validation.Message);
            }
            catch (Exception ex)
            {
                return Fail<LicenseValidationResult>(-1, "授权校验异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 校验首版强绑定字段。
        /// 当前口径下 ClientId 作为设备授权主键必须匹配；
        /// 其他绑定字段仅在授权中实际下发了非空值时才参与强校验，避免服务端未下发该字段时被本地误判失败。
        /// </summary>
        private static bool ValidateStrongBindings(DeviceLicenseBinding binding, DeviceHardwareInfo hardware, out string message)
        {
            binding = binding ?? new DeviceLicenseBinding();
            hardware = hardware ?? new DeviceHardwareInfo();

            if (!ValidateRequiredBinding("ClientId", binding.ClientId, hardware.ClientId, out message))
            {
                return false;
            }

            if (!ValidateIssuedBinding("MachineCode", binding.MachineCode, hardware.MachineCode, out message))
            {
                return false;
            }

            if (!ValidateIssuedBinding("CpuId", binding.CpuId, hardware.CpuId, out message))
            {
                return false;
            }

            message = "硬件强绑定校验通过";
            return true;
        }

        /// <summary>
        /// 校验单个强绑定字段。
        /// </summary>
        private static bool ValidateRequiredBinding(string fieldName, string licenseValue, string currentValue, out string message)
        {
            string normalizedLicense = HardwareInfoCollector.NormalizeBindingValue(licenseValue);
            string normalizedCurrent = HardwareInfoCollector.NormalizeBindingValue(currentValue);

            if (string.IsNullOrWhiteSpace(normalizedLicense))
            {
                message = "授权缺少强绑定字段: " + fieldName;
                return false;
            }

            if (string.IsNullOrWhiteSpace(normalizedCurrent))
            {
                message = "本机缺少强绑定字段: " + fieldName;
                return false;
            }

            if (!string.Equals(normalizedLicense, normalizedCurrent, StringComparison.OrdinalIgnoreCase))
            {
                message = "硬件强绑定字段不匹配: " + fieldName;
                return false;
            }

            message = string.Empty;
            return true;
        }

        /// <summary>
        /// 校验授权中已实际下发的绑定字段。
        /// 若授权未下发该字段，则视为当前授权链路未启用该字段做强绑定，不在本地额外判失败。
        /// </summary>
        private static bool ValidateIssuedBinding(string fieldName, string licenseValue, string currentValue, out string message)
        {
            string normalizedLicense = HardwareInfoCollector.NormalizeBindingValue(licenseValue);
            if (string.IsNullOrWhiteSpace(normalizedLicense))
            {
                message = string.Empty;
                return true;
            }

            return ValidateRequiredBinding(fieldName, licenseValue, currentValue, out message);
        }

        /// <summary>
        /// 将授权时间统一归一到 UTC。
        /// 设备侧当前既可能收到带 Z 的 UTC 时间，也可能遇到未显式标注 Kind 的时间值，这里统一转换后再比较。
        /// </summary>
        private static DateTime? NormalizeToUtc(DateTime? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            DateTime time = value.Value;
            if (time.Kind == DateTimeKind.Utc)
            {
                return time;
            }

            if (time.Kind == DateTimeKind.Local)
            {
                return time.ToUniversalTime();
            }

            return DateTime.SpecifyKind(time, DateTimeKind.Local).ToUniversalTime();
        }
    }
}