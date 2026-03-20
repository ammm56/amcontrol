using AM.Model.Common;
using AM.Model.MotionCard;

namespace AM.Model.Interfaces.DB.Motion.App
{
    /// <summary>
    /// 轴运行时配置应用服务接口。
    /// 负责读取、保存强类型轴配置，并将其同步回数据库与运行时上下文。
    /// </summary>
    public interface IAxisConfigAppService
    {
        /// <summary>
        /// 查询当前所有运行时轴配置。
        /// </summary>
        Result<AxisConfig> QueryAll();

        /// <summary>
        /// 按逻辑轴查询当前运行时轴配置。
        /// </summary>
        Result<AxisConfig> QueryByLogicalAxis(short logicalAxis);

        /// <summary>
        /// 保存指定轴的运行时配置，并同步到数据库和运行时上下文。
        /// </summary>
        Result Save(AxisConfig axisConfig);

        /// <summary>
        /// 从数据库重新覆盖当前运行时轴配置。
        /// </summary>
        Result ReloadFromDatabase();
    }
}