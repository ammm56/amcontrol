using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.MotionCard;
using System.Collections.Generic;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// 设备配置仓储接口。
    /// 负责数据库层设备配置读取与基础持久化。
    /// </summary>
    public interface IMachineConfigRepository
    {
        /// <summary>
        /// 初始化运动配置相关表。
        /// </summary>
        Result EnsureTables();

        /// <summary>
        /// 读取全部设备配置原始数据。
        /// </summary>
        Result<MachineConfigData> LoadAll();

        /// <summary>
        /// 查询全部控制卡。
        /// </summary>
        Result<MotionCardEntity> QueryAllCards();

        /// <summary>
        /// 查询全部轴定义。
        /// </summary>
        Result<MotionAxisEntity> QueryAllAxes();

        /// <summary>
        /// 查询全部 IO 映射。
        /// </summary>
        Result<MotionIoMapEntity> QueryAllIoMaps();
    }
}