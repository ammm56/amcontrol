using AM.Model.Common;
using AM.Model.Entity.Motion.Actuator;

namespace AM.Model.Interfaces.DB.Motion.Actuator
{
    /// <summary>
    /// 气缸对象配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionCylinderConfigCrudService
    {
        Result<CylinderConfigEntity> QueryAll();

        Result<CylinderConfigEntity> QueryByName(string name);

        Result Save(CylinderConfigEntity entity);

        Result DeleteByName(string name);
    }
}