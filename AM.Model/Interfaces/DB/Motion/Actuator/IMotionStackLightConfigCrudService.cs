using AM.Model.Common;
using AM.Model.Entity.Motion.Actuator;

namespace AM.Model.Interfaces.DB.Motion.Actuator
{
    /// <summary>
    /// 灯塔/声光报警对象配置 CRUD 服务接口。
    /// </summary>
    public interface IMotionStackLightConfigCrudService
    {
        /// <summary>
        /// 查询全部灯塔对象配置。
        /// </summary>
        Result<StackLightConfigEntity> QueryAll();

        /// <summary>
        /// 按名称查询灯塔对象配置。
        /// </summary>
        Result<StackLightConfigEntity> QueryByName(string name);

        /// <summary>
        /// 保存灯塔对象配置。
        /// </summary>
        Result Save(StackLightConfigEntity entity);

        /// <summary>
        /// 按名称删除灯塔对象配置。
        /// </summary>
        Result DeleteByName(string name);
    }
}