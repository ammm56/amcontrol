using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// IO 映射配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionIoMapCrudService
    {
        Result<MotionIoMapEntity> QueryAll();

        Result<MotionIoMapEntity> QueryByCardId(short cardId);

        Result<MotionIoMapEntity> QueryByLogicalBit(short logicalBit, string ioType);

        Result Save(MotionIoMapEntity entity);

        Result DeleteByLogicalBit(short logicalBit, string ioType);
    }
}