using AM.Model.Common;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// 设备配置热重载服务接口。
    /// </summary>
    public interface IMachineConfigReloadService
    {
        /// <summary>
        /// 从数据库重新加载运行时配置。
        /// </summary>
        Result ReloadFromDatabase();

        /// <summary>
        /// 根据当前运行时配置重建设备上下文。
        /// </summary>
        Result RebuildMachineContext();

        /// <summary>
        /// 先从数据库重新加载，再重建设备上下文。
        /// </summary>
        Result ReloadAndRebuild();
    }
}