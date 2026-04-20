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

                bool isDeveloperLicense = IsDeveloperEdition(license);
                if (isDeveloperLicense)
                {
                    validation.IsHardwareMatched = ValidateDeveloperGrantScope(license, out string developerMessage);
                    if (!validation.IsHardwareMatched)
                    {
                        validation.Success = false;
                        validation.ErrorCode = "LICENSE_DEVELOPER_SCOPE_MISMATCH";
                        validation.Message = developerMessage;
                        return OkLogOnly(validation, validation.Message);
                    }
                }
                else
                {
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
                validation.Message = isDeveloperLicense ? "开发版授权范围校验通过" : "授权校验通过";
                return OkLogOnly(validation, validation.Message);
            }
            catch (Exception ex)
            {
                return Fail<LicenseValidationResult>(-1, "授权校验异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 判断当前授权是否为开发版授权。
        /// 开发版设备软件不按常规硬件强绑定校验，而是按软件版本与业务范围字段匹配。
        /// </summary>
        private static bool IsDeveloperEdition(DeviceLicense license)
        {
            string licenseEdition = license == null || license.Software == null
                ? string.Empty
                : license.Software.AppEdition;

            return string.Equals(
                NormalizeText(licenseEdition),
                NormalizeText(LicenseConstants.DeveloperAppEdition),
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 校验开发版授权范围。
        /// 开发版当前不检查 deviceBinding，而是校验运行时版本、Edition 与 grantScope 中的 customer/site/machineModel。
        /// 其中 machineModel 优先取 config 中统一配置，未配置时回退到本机实时采集值。
        /// </summary>
        private bool ValidateDeveloperGrantScope(DeviceLicense license, out string message)
        {
            DeviceLicenseSoftware software = license == null ? null : license.Software;
            DeviceLicenseGrantScope grantScope = license == null ? null : license.GrantScope;

            string runtimeAppVersion = NormalizeText(AM.Tools.Tools.GetAppVersionText());
            string configuredAppEdition = NormalizeText(BackendServiceConfigHelper.GetDesktopAppEdition());
            string configuredCustomerCode = NormalizeText(BackendServiceConfigHelper.GetLicenseCustomerCode());
            string configuredSiteCode = NormalizeText(BackendServiceConfigHelper.GetLicenseSiteCode());

            if (!ValidateDeveloperField("AppVersion", software == null ? string.Empty : software.AppVersion, runtimeAppVersion, out message))
            {
                return false;
            }

            if (!ValidateDeveloperField("AppEdition", software == null ? string.Empty : software.AppEdition, configuredAppEdition, out message))
            {
                return false;
            }

            if (!ValidateDeveloperField("CustomerCode", grantScope == null ? string.Empty : grantScope.CustomerCode, configuredCustomerCode, out message))
            {
                return false;
            }

            if (!ValidateDeveloperField("SiteCode", grantScope == null ? string.Empty : grantScope.SiteCode, configuredSiteCode, out message))
            {
                return false;
            }

            string currentMachineModel = ResolveCurrentDeveloperMachineModel(out string machineModelError);
            if (!string.IsNullOrWhiteSpace(machineModelError))
            {
                message = machineModelError;
                return false;
            }

            if (!ValidateDeveloperField("MachineModel", grantScope == null ? string.Empty : grantScope.MachineModel, currentMachineModel, out message))
            {
                return false;
            }

            message = "开发版授权范围匹配通过";
            return true;
        }

        /// <summary>
        /// 获取开发版授权当前用于匹配的设备型号。
        /// 优先取 config 统一配置；未配置时才回退到硬件采集值，保持与授权申请提交时的 machineModel 口径一致。
        /// </summary>
        private string ResolveCurrentDeveloperMachineModel(out string errorMessage)
        {
            string configuredMachineModel = NormalizeText(BackendServiceConfigHelper.GetLicenseMachineModel());
            if (!string.IsNullOrWhiteSpace(configuredMachineModel))
            {
                errorMessage = string.Empty;
                return configuredMachineModel;
            }

            Result<DeviceHardwareInfo> hardwareResult = _hardwareInfoCollector.CollectCurrent();
            if (!hardwareResult.Success || hardwareResult.Item == null)
            {
                errorMessage = "读取开发版授权匹配所需的设备型号失败";
                return string.Empty;
            }

            errorMessage = string.Empty;
            return NormalizeText(hardwareResult.Item.MachineModel);
        }

        /// <summary>
        /// 校验开发版授权单个业务字段。
        /// Developer 版要求授权中下发值与本机当前运行值逐项一致。
        /// </summary>
        private static bool ValidateDeveloperField(string fieldName, string licenseValue, string currentValue, out string message)
        {
            string normalizedLicense = NormalizeText(licenseValue);
            string normalizedCurrent = NormalizeText(currentValue);

            if (string.IsNullOrWhiteSpace(normalizedLicense))
            {
                message = "开发版授权缺少匹配字段: " + fieldName;
                return false;
            }

            if (string.IsNullOrWhiteSpace(normalizedCurrent))
            {
                message = "程序当前缺少匹配字段: " + fieldName;
                return false;
            }

            if (!string.Equals(normalizedLicense, normalizedCurrent, StringComparison.OrdinalIgnoreCase))
            {
                message = "开发版授权字段不匹配: " + fieldName;
                return false;
            }

            message = string.Empty;
            return true;
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
        /// 归一化开发版授权中的普通业务字段。
        /// 这里不移除中划线等字符，只做去空白，避免 customer/site/model 等字段语义被本地额外篡改。
        /// </summary>
        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();
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