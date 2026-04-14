using AM.Model.Common;
using AM.Model.Entity.Motion.Topology;

namespace AM.Model.Interfaces.DB.Motion.Topology
{
    /// <summary>
    /// IO 接线信息 CRUD 服务接口。
    /// 对应数据库表：`motion_io_wiring`
    /// 负责维护端子、线号、对端设备与核对状态等装配接线信息。
    /// </summary>
    public interface IMotionIoWiringCrudService
    {
        /// <summary>
        /// 查询全部 IO 接线信息。
        /// </summary>
        Result<MotionIoWiringEntity> QueryAll();

        /// <summary>
        /// 按控制卡 CardId 查询接线信息。
        /// </summary>
        /// <param name="cardId">控制卡硬件通道号。</param>
        Result<MotionIoWiringEntity> QueryByCardId(short cardId);

        /// <summary>
        /// 按 IO 映射主键查询接线信息。
        /// </summary>
        /// <param name="ioMapId">IO 映射主键，对应 motion_io_map.Id。</param>
        Result<MotionIoWiringEntity> QueryByIoMapId(int ioMapId);

        /// <summary>
        /// 按逻辑位与 IO 类型查询接线信息。
        /// </summary>
        /// <param name="logicalBit">逻辑位号。</param>
        /// <param name="ioType">IO 类型，仅支持 DI 或 DO。</param>
        Result<MotionIoWiringEntity> QueryByLogicalBit(short logicalBit, string ioType);

        /// <summary>
        /// 保存 IO 接线信息。
        /// 若主键已存在则更新，否则新增。
        /// </summary>
        /// <param name="entity">IO 接线信息实体。</param>
        Result Save(MotionIoWiringEntity entity);

        /// <summary>
        /// 按 IO 映射主键删除接线信息。
        /// </summary>
        /// <param name="ioMapId">IO 映射主键，对应 motion_io_map.Id。</param>
        Result DeleteByIoMapId(int ioMapId);
    }
}
