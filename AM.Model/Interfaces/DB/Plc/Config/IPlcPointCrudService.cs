using AM.Model.Common;
using AM.Model.Entity.Plc;

namespace AM.Model.Interfaces.DB.Plc.Config
{
    /// <summary>
    /// PLC 点位配置 CRUD 服务接口。
    /// 对应数据库表：`plc_point`
    /// </summary>
    public interface IPlcPointCrudService
    {
        /// <summary>
        /// 查询全部 PLC 点位配置。
        /// </summary>
        Result<PlcPointConfigEntity> QueryAll();

        /// <summary>
        /// 按 PLC 名称查询该站下全部点位。
        /// </summary>
        Result<PlcPointConfigEntity> QueryByPlcName(string plcName);

        /// <summary>
        /// 按 PLC 名称与点位名称查询单个点位。
        /// </summary>
        Result<PlcPointConfigEntity> QueryByName(string plcName, string name);

        /// <summary>
        /// 保存 PLC 点位配置。
        /// 若主键已存在则更新，否则新增。
        /// </summary>
        Result Save(PlcPointConfigEntity entity);

        /// <summary>
        /// 按 PLC 名称与点位名称删除点位。
        /// </summary>
        Result DeleteByName(string plcName, string name);
    }
}