using AM.Model.Common;
using AM.Model.Entity.Plc;

namespace AM.Model.Interfaces.Plc.Config
{
    /// <summary>
    /// PLC 批量读取块配置 CRUD 服务接口。
    /// 对应数据库表：`plc_read_block`
    /// 接口放在 PLC 领域目录下，数据库实现由 DBService 承载。
    /// </summary>
    public interface IPlcReadBlockCrudService
    {
        /// <summary>
        /// 查询全部批量读取块配置。
        /// </summary>
        Result<PlcReadBlockConfigEntity> QueryAll();

        /// <summary>
        /// 按 PLC 名称查询全部读块配置。
        /// </summary>
        Result<PlcReadBlockConfigEntity> QueryByPlcName(string plcName);

        /// <summary>
        /// 按 PLC 名称与读块名称查询单个读块。
        /// </summary>
        Result<PlcReadBlockConfigEntity> QueryByName(string plcName, string blockName);

        /// <summary>
        /// 保存批量读取块配置。
        /// </summary>
        Result Save(PlcReadBlockConfigEntity entity);

        /// <summary>
        /// 按 PLC 名称与读块名称删除读块配置。
        /// </summary>
        Result DeleteByName(string plcName, string blockName);
    }
}
