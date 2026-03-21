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
        /// 包含控制卡、轴、IO、点位公共配置及第三层对象默认种子。
        /// </summary>
        Result EnsureSeedData();
    }
}