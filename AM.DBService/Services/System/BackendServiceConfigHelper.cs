using AM.Core.Context;
using AM.Model.Common;
using AM.Model.License;
using System;
using System.IO;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 统一后端服务配置与授权密钥文件路径读取帮助类。
    /// 授权申请、设备注册、心跳、结构化上报和使用信息上报统一走同一个后端地址。
    /// 授权相关密钥固定从 AM.Tools/Configuration 目录读取，且“许可验签公钥”和“申请签名私钥”属于不同链路，不按一对公私钥理解。
    /// </summary>
    public static class BackendServiceConfigHelper
    {
        private static readonly Setting DefaultSetting = new Setting();

        private static Setting GetSetting()
        {
            try
            {
                return ConfigContext.Instance.Config.Setting ?? new Setting();
            }
            catch
            {
                return new Setting();
            }
        }

        public static string GetBackendServiceUrl()
        {
            try
            {
                return GetSetting().BackendServiceUrl ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(GetBackendServiceUrl());
        }

        public static string GetDesktopAppCode()
        {
            string value = GetSetting().DesktopAppCode;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppCode : value.Trim();
        }

        public static string GetDesktopAppName()
        {
            string value = GetSetting().DesktopAppName;
            return string.IsNullOrWhiteSpace(value) ? GetDesktopAppCode() : value.Trim();
        }

        public static string GetDesktopAppCategory()
        {
            string value = GetSetting().DesktopAppCategory;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppCategory : value.Trim();
        }

        public static string GetDesktopAppEdition()
        {
            return (GetSetting().DesktopAppEdition ?? string.Empty).Trim();
        }

        public static string GetDesktopAppVendor()
        {
            string value = GetSetting().DesktopAppVendor;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppVendor : value.Trim();
        }

        public static string GetDesktopAppTargetFramework()
        {
            string value = GetSetting().DesktopAppTargetFramework;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppTargetFramework : value.Trim();
        }

        public static string GetDesktopAppUiPlatform()
        {
            string value = GetSetting().DesktopAppUiPlatform;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppUiPlatform : value.Trim();
        }

        public static string GetLicenseValidationPublicKeyFilePath()
        {
            return ResolveConfigurationFilePath(LicenseConstants.LicenseValidationPublicKeyFileName);
        }

        /// <summary>
        /// 获取授权申请签名私钥文件路径。
        /// 该私钥只用于设备向后端发送授权申请时做请求签名，不参与本地 license.lic 验签。
        /// </summary>
        public static string GetLicenseRequestSigningPrivateKeyFilePath()
        {
            return ResolveConfigurationFilePath(LicenseConstants.LicenseRequestSigningPrivateKeyFileName);
        }

        /// <summary>
        /// 判断授权申请签名私钥是否已配置。
        /// </summary>
        public static bool HasLicenseRequestSigningPrivateKeyConfigured()
        {
            return File.Exists(GetLicenseRequestSigningPrivateKeyFilePath());
        }

        /// <summary>
        /// 判断授权许可验签公钥是否已配置。
        /// 该公钥只用于本地 license.lic 验签，不用于签名授权申请消息。
        /// </summary>
        public static bool HasLicenseValidationPublicKeyConfigured()
        {
            return File.Exists(GetLicenseValidationPublicKeyFilePath());
        }

        private static string ResolveConfigurationFilePath(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
            string configurationPath = Path.Combine(baseDirectory, "Configuration", fileName ?? string.Empty);
            if (File.Exists(configurationPath))
            {
                return configurationPath;
            }

            return Path.Combine(baseDirectory, fileName ?? string.Empty);
        }
    }
}