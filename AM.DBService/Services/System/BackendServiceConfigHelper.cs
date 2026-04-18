using AM.Core.Context;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 统一后端服务配置读取帮助类。
    /// 授权申请、设备注册、心跳、结构化上报和使用信息上报统一走同一个后端地址。
    /// </summary>
    internal static class BackendServiceConfigHelper
    {
        public static string GetBackendServiceUrl()
        {
            try
            {
                return ConfigContext.Instance.Config.Setting.BackendServiceUrl ?? string.Empty;
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
    }
}