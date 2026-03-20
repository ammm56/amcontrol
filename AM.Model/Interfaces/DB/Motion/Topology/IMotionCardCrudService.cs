using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;

namespace AM.Model.Interfaces.DB.Motion.Topology
{
    /// <summary>
    /// 控制卡配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionCardCrudService
    {
        Result<MotionCardEntity> QueryAll();

        Result<MotionCardEntity> QueryByCardId(short cardId);

        Result Save(MotionCardEntity entity);

        Result DeleteByCardId(short cardId);
    }
}