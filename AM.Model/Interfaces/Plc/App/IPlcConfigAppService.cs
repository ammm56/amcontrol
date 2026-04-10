using AM.Model.Common;
using AM.Model.Plc;

namespace AM.Model.Interfaces.Plc.App
{
    /// <summary>
    /// PLC 配置应用服务接口。
    /// 负责将数据库中的 PLC 配置装配到 ConfigContext 与 MachineContext。
    /// 当前版本 PLC 配置仅包含：
    /// - 站配置
    /// - 点位配置
    /// </summary>
    public interface IPlcConfigAppService
    {
        /// <summary>
        /// 初始化 PLC 配置相关表。
        /// </summary>
        Result EnsureTables();

        /// <summary>
        /// 从数据库读取并装配 PLC 配置。
        /// </summary>
        Result<PlcConfig> QueryAll();

        /// <summary>
        /// 从数据库重载 PLC 配置并同步到全局上下文。
        /// </summary>
        Result ReloadFromDatabase();
    }
}