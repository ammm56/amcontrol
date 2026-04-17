using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.License;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 授权运行时装载器。
    /// 启动时负责读取 license.lic、解码、解析、校验，并将最终状态写入 LicenseRuntimeContext。
    /// </summary>
    public class LicenseRuntimeLoader : ServiceBase
    {
        private readonly LicenseFileService _licenseFileService;
        private readonly LicenseCryptoService _licenseCryptoService;
        private readonly LicenseValidator _licenseValidator;

        protected override string MessageSourceName
        {
            get { return "LicenseRuntimeLoader"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public LicenseRuntimeLoader()
            : this(new LicenseFileService(), new LicenseCryptoService(), new LicenseValidator(), SystemContext.Instance.Reporter)
        {
        }

        public LicenseRuntimeLoader(IAppReporter reporter)
            : this(new LicenseFileService(reporter), new LicenseCryptoService(reporter), new LicenseValidator(reporter), reporter)
        {
        }

        public LicenseRuntimeLoader(LicenseFileService licenseFileService, LicenseCryptoService licenseCryptoService, LicenseValidator licenseValidator, IAppReporter reporter)
            : base(reporter)
        {
            _licenseFileService = licenseFileService;
            _licenseCryptoService = licenseCryptoService;
            _licenseValidator = licenseValidator;
        }

        /// <summary>
        /// 从本地授权文件加载并写入运行时上下文。
        /// </summary>
        public Result<DeviceLicenseState> Load()
        {
            try
            {
                if (!ConfigContext.Instance.Config.Setting.EnableDeviceLicense)
                {
                    DeviceLicenseState disabledState = BuildInvalidState("已关闭设备授权校验", "LICENSE_DISABLED");
                    LicenseRuntimeContext.Instance.Update(disabledState);
                    return OkLogOnly(disabledState, disabledState.Message);
                }

                Result<string> licenseTextResult = _licenseFileService.ReadLicenseText();
                if (!licenseTextResult.Success || string.IsNullOrWhiteSpace(licenseTextResult.Item))
                {
                    DeviceLicenseState missingState = BuildInvalidState("未找到有效的授权文件", "LICENSE_FILE_MISSING");
                    LicenseRuntimeContext.Instance.Update(missingState);
                    return OkLogOnly(missingState, missingState.Message);
                }

                Result<string> decodeResult = _licenseCryptoService.DecodeLicenseText(licenseTextResult.Item);
                if (!decodeResult.Success || string.IsNullOrWhiteSpace(decodeResult.Item))
                {
                    DeviceLicenseState decodeFailedState = BuildInvalidState("授权文件解码失败", "LICENSE_DECODE_FAILED");
                    LicenseRuntimeContext.Instance.Update(decodeFailedState);
                    return OkLogOnly(decodeFailedState, decodeFailedState.Message);
                }

                DeviceLicense license;
                try
                {
                    license = JsonConvert.DeserializeObject<DeviceLicense>(decodeResult.Item);
                }
                catch (Exception ex)
                {
                    DeviceLicenseState parseFailedState = BuildInvalidState("授权 JSON 解析失败", "LICENSE_JSON_INVALID");
                    LicenseRuntimeContext.Instance.Update(parseFailedState);
                    return Fail<DeviceLicenseState>(-1, parseFailedState.Message, ReportChannels.Log, ex);
                }

                Result<LicenseValidationResult> validationResult = _licenseValidator.Validate(license, decodeResult.Item);
                if (!validationResult.Success || validationResult.Item == null)
                {
                    DeviceLicenseState validationFailedState = BuildInvalidState("授权校验执行失败", "LICENSE_VALIDATE_FAILED");
                    LicenseRuntimeContext.Instance.Update(validationFailedState);
                    return OkLogOnly(validationFailedState, validationFailedState.Message);
                }

                DeviceLicenseState state = BuildState(license, validationResult.Item);
                LicenseRuntimeContext.Instance.Update(state);
                return OkLogOnly(state, state.Message);
            }
            catch (Exception ex)
            {
                DeviceLicenseState state = BuildInvalidState("授权运行时装载异常", "LICENSE_RUNTIME_LOAD_FAILED");
                LicenseRuntimeContext.Instance.Update(state);
                return Fail<DeviceLicenseState>(-1, state.Message, ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 根据校验结果构建运行时状态。
        /// </summary>
        private static DeviceLicenseState BuildState(DeviceLicense license, LicenseValidationResult validation)
        {
            license = license ?? new DeviceLicense();
            validation = validation ?? new LicenseValidationResult();

            IReadOnlyList<string> moduleKeys = validation.Success && license.Authorization != null
                ? DistinctKeys(license.Authorization.ModuleKeys)
                : new List<string>();

            IReadOnlyList<string> pageKeys = validation.Success && license.Authorization != null
                ? DistinctKeys(license.Authorization.PageKeys)
                : new List<string>();

            return new DeviceLicenseState
            {
                HasLicense = true,
                IsSignatureValid = validation.IsSignatureValid,
                IsValid = validation.Success,
                IsExpired = validation.IsExpired,
                IsInGracePeriod = validation.IsInGracePeriod,
                LicenseId = license.LicenseId ?? string.Empty,
                ExpiresAt = validation.ExpiresAt,
                Message = string.IsNullOrWhiteSpace(validation.Message) ? "授权装载完成" : validation.Message,
                ModuleKeys = moduleKeys,
                PageKeys = pageKeys,
                License = license,
                ValidationResult = validation
            };
        }

        /// <summary>
        /// 构建无效授权状态。
        /// </summary>
        private static DeviceLicenseState BuildInvalidState(string message, string errorCode)
        {
            return new DeviceLicenseState
            {
                HasLicense = false,
                IsSignatureValid = false,
                IsValid = false,
                IsExpired = false,
                IsInGracePeriod = false,
                Message = message ?? string.Empty,
                ModuleKeys = new List<string>(),
                PageKeys = new List<string>(),
                ValidationResult = new LicenseValidationResult
                {
                    Success = false,
                    ErrorCode = errorCode ?? string.Empty,
                    Message = message ?? string.Empty,
                    HasLicenseFile = false,
                    ValidatedAt = DateTime.Now
                }
            };
        }

        /// <summary>
        /// 对授权键集合去空值、去重并转成只读集合。
        /// </summary>
        private static IReadOnlyList<string> DistinctKeys(IEnumerable<string> keys)
        {
            return keys == null
                ? new List<string>()
                : keys
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
        }
    }
}