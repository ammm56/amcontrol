using AM.Model.Common;
using AM.Model.Entity.Motion;
using System.Collections.Generic;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// 运动轴参数数据库服务接口。
    /// </summary>
    public interface IMotionAxisConfigService
    {
        Result<MotionAxisConfigEntity> QueryAll();
        Result<MotionAxisConfigEntity> QueryByLogicalAxis(int logicalAxis);
        Result Save(MotionAxisConfigEntity entity);
        Result SaveRange(IEnumerable<MotionAxisConfigEntity> entities);
        Result Delete(int logicalAxis, string paramName);
    }
}