using AM.Core.Context;
using AM.Model.Common;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 统一后端服务配置读取帮助类。
    /// 授权申请、设备注册、心跳、结构化上报和使用信息上报统一走同一个后端地址。
    /// </summary>
    internal static class BackendServiceConfigHelper
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
    }
}