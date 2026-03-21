using AM.Model.Common;
using AM.Model.Entity.Motion.Topology;

namespace AM.Model.Interfaces.DB.Motion.Topology
{
    /// <summary>
    /// 运动控制卡配置 CRUD 服务接口。
    /// 对应数据库表：`motion_card`
    /// 负责控制卡静态拓扑、驱动识别、初始化顺序等配置的查询、保存、删除。
    /// </summary>
    public interface IMotionCardCrudService
    {
        /// <summary>
        /// 查询全部控制卡配置。
        /// </summary>
        Result<MotionCardEntity> QueryAll();

        /// <summary>
        /// 按控制卡硬件通道号查询单张控制卡配置。
        /// </summary>
        /// <param name="cardId">控制卡硬件通道号。</param>
        Result<MotionCardEntity> QueryByCardId(short cardId);

        /// <summary>
        /// 保存控制卡配置。
        /// 若主键已存在则更新，否则新增。
        /// </summary>
        /// <param name="entity">控制卡配置实体。</param>
        Result Save(MotionCardEntity entity);

        /// <summary>
        /// 按控制卡硬件通道号删除控制卡配置。
        /// 删除前应保证该卡下无轴定义、无 IO 映射。
        /// </summary>
        /// <param name="cardId">控制卡硬件通道号。</param>
        Result DeleteByCardId(short cardId);
    }
}