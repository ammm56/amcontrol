using AM.Model.Common;
using AM.Model.MotionCard;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// 设备配置应用服务接口。
    /// 负责从数据库装配完整的运动控制运行时配置。
    /// </summary>
    public interface IMachineConfigAppService
    {
        /// <summary>
        /// 初始化设备配置相关表。
        /// </summary>
        Result EnsureTables();

        /// <summary>
        /// 查询数据库中的完整运动控制配置。
        /// </summary>
        Result<MotionCardConfig> QueryAll();

        /// <summary>
        /// 从数据库重新加载配置到运行时上下文。
        /// </summary>
        Result ReloadFromDatabase();
    }
}