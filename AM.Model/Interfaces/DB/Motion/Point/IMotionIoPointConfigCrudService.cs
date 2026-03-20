using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Point;

namespace AM.Model.Interfaces.DB.Motion.Point
{
    /// <summary>
    /// IO 点位公共配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionIoPointConfigCrudService
    {
        Result<MotionIoPointConfigEntity> QueryAll();

        Result<MotionIoPointConfigEntity> QueryByLogicalBit(short logicalBit, string ioType);

        Result Save(MotionIoPointConfigEntity entity);

        Result DeleteByLogicalBit(short logicalBit, string ioType);
    }
}