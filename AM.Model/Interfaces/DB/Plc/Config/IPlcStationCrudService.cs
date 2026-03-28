using AM.Model.Common;
using AM.Model.Entity.Plc;

namespace AM.Model.Interfaces.DB.Plc.Config
{
    /// <summary>
    /// PLC 站配置 CRUD 服务接口。
    /// 对应数据库表：`plc_station`
    /// </summary>
    public interface IPlcStationCrudService
    {
        /// <summary>
        /// 查询全部 PLC 站配置。
        /// </summary>
        Result<PlcStationConfigEntity> QueryAll();

        /// <summary>
        /// 按 PLC 名称查询单站配置。
        /// </summary>
        Result<PlcStationConfigEntity> QueryByName(string name);

        /// <summary>
        /// 保存 PLC 站配置。
        /// 若主键已存在则更新，否则新增。
        /// </summary>
        Result Save(PlcStationConfigEntity entity);

        /// <summary>
        /// 按 PLC 名称删除配置。
        /// 删除前应保证该站下无点位配置。
        /// </summary>
        Result DeleteByName(string name);
    }
}