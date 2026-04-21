using AM.Core.Context;
using AM.Model.Common;
using System;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 统一后端服务配置与授权密钥文件路径读取帮助类。
    /// 授权申请、设备注册、心跳、结构化上报和使用信息上报统一走同一个后端地址。
    /// 授权相关密钥与设备密钥当前固定直接从代码内置默认值读取，且“许可验签公钥”和“申请签名私钥”属于不同链路，不按一对公私钥理解。
    ///
    /// 当前这条“配置来源”链路刻意做得很收敛：
    /// 1. 运行期业务配置统一从 ConfigContext.Instance.Config.Setting 读取；
    /// 2. 这里不再主动回读磁盘 config.json，也不负责合并多份配置来源；
    /// 3. 各 Client、Worker、UI 通过本帮助类拿到后端地址、AppCode、授权范围等字段；
    /// 4. 一旦这些字段缺失，请求入口会直接返回本地 Fail/FailSilent，随后沿 Result.Message 进入错误传播链。
    /// </summary>
    public static class BackendServiceConfigHelper
    {
        private static readonly Setting DefaultSetting = new Setting();

        /// <summary>
        /// 获取当前运行期 Setting。
        /// 当前唯一可信来源是 ConfigContext 中已经装载到内存的配置对象；若上下文尚未初始化，则回退为空 Setting。
        /// </summary>
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

        /// <summary>
        /// 获取统一后端服务地址。
        /// 设备注册、token 刷新、心跳、report、授权申请、按设备查询授权状态当前都依赖该地址拼接请求 URL。
        /// </summary>
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

        /// <summary>
        /// 判断统一后端服务地址是否已配置。
        /// 这是后台上传工作单元和各后端 Client 允许继续发起请求的最前置开关。
        /// </summary>
        public static bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(GetBackendServiceUrl());
        }

        /// <summary>
        /// 获取桌面应用编码。
        /// 该值会进入授权申请、授权状态查询、设备 report 与心跳摘要，是后端识别当前软件产品线的核心字段。
        /// </summary>
        public static string GetDesktopAppCode()
        {
            string value = GetSetting().DesktopAppCode;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppCode : value.Trim();
        }

        /// <summary>
        /// 获取桌面应用名称。
        /// 主要用于展示与报文补充说明；未配置时回退到 AppCode。
        /// </summary>
        public static string GetDesktopAppName()
        {
            string value = GetSetting().DesktopAppName;
            return string.IsNullOrWhiteSpace(value) ? GetDesktopAppCode() : value.Trim();
        }

        /// <summary>
        /// 获取桌面应用分类。
        /// 当前主要用于授权申请中的软件元信息补充。
        /// </summary>
        public static string GetDesktopAppCategory()
        {
            string value = GetSetting().DesktopAppCategory;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppCategory : value.Trim();
        }

        /// <summary>
        /// 获取桌面应用版本类型。
        /// 当前 Developer 版授权校验、授权页显示和授权申请请求都会使用该字段。
        /// </summary>
        public static string GetDesktopAppEdition()
        {
            return (GetSetting().DesktopAppEdition ?? string.Empty).Trim();
        }

        /// <summary>
        /// 获取授权客户编码。
        /// 当前用于授权申请请求、授权页展示以及 Developer 授权范围匹配。
        /// </summary>
        public static string GetLicenseCustomerCode()
        {
            return (GetSetting().LicenseCustomerCode ?? string.Empty).Trim();
        }

        /// <summary>
        /// 获取授权站点编码。
        /// 当前用于授权申请请求、授权页展示以及 Developer 授权范围匹配。
        /// </summary>
        public static string GetLicenseSiteCode()
        {
            return (GetSetting().LicenseSiteCode ?? string.Empty).Trim();
        }

        /// <summary>
        /// 获取授权机型标识。
        /// 当前优先作为授权申请中的 machineModel 使用，也参与 Developer 授权范围匹配。
        /// </summary>
        public static string GetLicenseMachineModel()
        {
            return (GetSetting().LicenseMachineModel ?? string.Empty).Trim();
        }

        /// <summary>
        /// 获取应用供应商。
        /// 未配置时回退到 Setting 默认值。
        /// </summary>
        public static string GetDesktopAppVendor()
        {
            string value = GetSetting().DesktopAppVendor;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppVendor : value.Trim();
        }

        /// <summary>
        /// 获取目标框架描述。
        /// 当前主要用于授权申请的软件元信息。
        /// </summary>
        public static string GetDesktopAppTargetFramework()
        {
            string value = GetSetting().DesktopAppTargetFramework;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppTargetFramework : value.Trim();
        }

        /// <summary>
        /// 获取 UI 平台描述。
        /// 当前主要用于授权申请的软件元信息。
        /// </summary>
        public static string GetDesktopAppUiPlatform()
        {
            string value = GetSetting().DesktopAppUiPlatform;
            return string.IsNullOrWhiteSpace(value) ? DefaultSetting.DesktopAppUiPlatform : value.Trim();
        }

        /// <summary>
        /// 获取设备接入共享应用密钥。
        /// 当前用于 register、heartbeat、report 三个设备写接口的 HKDF 输入材料。
        /// </summary>
        public static string GetDeviceAppSecret()
        {
            return (GetSetting().DeviceAppSecret ?? string.Empty).Trim();
        }

        /// <summary>
        /// 获取本地 license 验签公钥 PEM 文本。
        /// 该公钥只服务于本地 license.lic 验签，不参与任何设备管理接口请求。
        /// </summary>
        public static string GetLicenseValidationPublicKeyPem()
        {
            return (GetSetting().LicenseValidationPublicKeyPem ?? string.Empty).Trim();
        }

        /// <summary>
        /// 获取授权申请签名私钥 PEM 文本。
        /// 该私钥只用于设备向后端发送授权申请时做请求签名，不参与本地 license.lic 验签。
        /// </summary>
        public static string GetLicenseRequestSigningPrivateKeyPem()
        {
            return (GetSetting().LicenseRequestSigningPrivateKeyPem ?? string.Empty).Trim();
        }

        /// <summary>
        /// 获取设备接入密钥版本。
        /// 当前会写入 `X-Device-KeyVersion` 请求头，并参与 AAD 拼接。
        /// </summary>
        public static int GetDeviceKeyVersion()
        {
            int value = GetSetting().DeviceKeyVersion;
            return value <= 0 ? 1 : value;
        }

        /// <summary>
        /// 判断授权申请签名私钥是否已配置。
        /// </summary>
        public static bool HasLicenseRequestSigningPrivateKeyConfigured()
        {
            return !string.IsNullOrWhiteSpace(GetLicenseRequestSigningPrivateKeyPem());
        }

        /// <summary>
        /// 判断授权许可验签公钥是否已配置。
        /// 该公钥只用于本地 license.lic 验签，不用于签名授权申请消息。
        /// </summary>
        public static bool HasLicenseValidationPublicKeyConfigured()
        {
            return !string.IsNullOrWhiteSpace(GetLicenseValidationPublicKeyPem());
        }
    }
}