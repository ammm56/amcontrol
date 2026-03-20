using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Point;
using AM.Model.Entity.Motion.Topology;

namespace AM.Model.Interfaces.DB.Motion.Point
{
    /// <summary>
    /// IO 配置统一 CRUD 聚合服务接口。
    /// 聚合拓扑映射与点位公共配置两层数据。
    /// </summary>
    public interface IMotionIoConfigCrudService
    {
        Result<MotionIoMapEntity> QueryAllMaps();

        Result<MotionIoPointConfigEntity> QueryAllPointConfigs();

        Result<MotionIoMapEntity> QueryMapByLogicalBit(short logicalBit, string ioType);

        Result<MotionIoPointConfigEntity> QueryPointConfigByLogicalBit(short logicalBit, string ioType);

        Result Save(MotionIoMapEntity ioMapEntity, MotionIoPointConfigEntity pointConfigEntity);

        Result DeleteByLogicalBit(short logicalBit, string ioType);
    }
}