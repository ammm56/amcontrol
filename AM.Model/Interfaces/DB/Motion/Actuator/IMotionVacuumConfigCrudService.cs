using AM.Model.Common;
using AM.Model.Entity.Motion.Actuator;

namespace AM.Model.Interfaces.DB.Motion.Actuator
{
    /// <summary>
    /// 真空对象配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionVacuumConfigCrudService
    {
        /// <summary>
        /// 查询全部真空对象配置。
        /// </summary>
        Result<VacuumConfigEntity> QueryAll();

        /// <summary>
        /// 按名称查询真空对象配置。
        /// </summary>
        Result<VacuumConfigEntity> QueryByName(string name);

        /// <summary>
        /// 保存真空对象配置。
        /// </summary>
        Result Save(VacuumConfigEntity entity);

        /// <summary>
        /// 按名称删除真空对象配置。
        /// </summary>
        Result DeleteByName(string name);
    }
}
