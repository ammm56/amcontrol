using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;

namespace AM.Model.Interfaces.DB.Motion.Topology
{
    /// <summary>
    /// 轴拓扑配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionAxisCrudService
    {
        Result<MotionAxisEntity> QueryAll();

        Result<MotionAxisEntity> QueryByLogicalAxis(short logicalAxis);

        Result<MotionAxisEntity> QueryByCardId(short cardId);

        Result Save(MotionAxisEntity entity);

        Result DeleteByLogicalAxis(short logicalAxis);
    }
}