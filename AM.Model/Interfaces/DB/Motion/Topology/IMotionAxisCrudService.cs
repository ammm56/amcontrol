using AM.Model.Common;
using AM.Model.Entity.Motion.Topology;

namespace AM.Model.Interfaces.DB.Motion.Topology
{
    /// <summary>
    /// 运动轴拓扑配置 CRUD 服务接口。
    /// 对应数据库表：`motion_axis`
    /// 负责轴归属关系、逻辑轴号、物理映射和基础展示信息的管理。
    /// </summary>
    public interface IMotionAxisCrudService
    {
        /// <summary>
        /// 查询全部轴拓扑配置。
        /// </summary>
        Result<MotionAxisEntity> QueryAll();

        /// <summary>
        /// 按逻辑轴号查询轴拓扑配置。
        /// </summary>
        /// <param name="logicalAxis">逻辑轴号。</param>
        Result<MotionAxisEntity> QueryByLogicalAxis(short logicalAxis);

        /// <summary>
        /// 按控制卡 CardId 查询该卡下全部轴拓扑配置。
        /// </summary>
        /// <param name="cardId">控制卡硬件通道号。</param>
        Result<MotionAxisEntity> QueryByCardId(short cardId);

        /// <summary>
        /// 保存轴拓扑配置。
        /// 若主键已存在则更新，否则新增。
        /// </summary>
        /// <param name="entity">轴拓扑配置实体。</param>
        Result Save(MotionAxisEntity entity);

        /// <summary>
        /// 按逻辑轴号删除轴拓扑配置。
        /// 删除时应同步删除该轴对应的参数配置记录。
        /// </summary>
        /// <param name="logicalAxis">逻辑轴号。</param>
        Result DeleteByLogicalAxis(short logicalAxis);
    }
}