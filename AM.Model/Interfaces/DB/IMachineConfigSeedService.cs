using AM.Model.Common;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// 运动设备配置种子服务接口。
    /// </summary>
    public interface IMachineConfigSeedService
    {
        /// <summary>
        /// 确保默认运动配置种子存在。
        /// </summary>
        Result EnsureSeedData();
    }
}