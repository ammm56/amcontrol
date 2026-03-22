using AM.Model.Common;
using AM.Model.Entity.Motion.Actuator;

namespace AM.Model.Interfaces.DB.Motion.Actuator
{
    /// <summary>
    /// 夹爪对象配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionGripperConfigCrudService
    {
        /// <summary>
        /// 查询全部夹爪对象配置。
        /// </summary>
        Result<GripperConfigEntity> QueryAll();

        /// <summary>
        /// 按名称查询夹爪对象配置。
        /// </summary>
        Result<GripperConfigEntity> QueryByName(string name);

        /// <summary>
        /// 保存夹爪对象配置。
        /// </summary>
        Result Save(GripperConfigEntity entity);

        /// <summary>
        /// 按名称删除夹爪对象配置。
        /// </summary>
        Result DeleteByName(string name);
    }
}