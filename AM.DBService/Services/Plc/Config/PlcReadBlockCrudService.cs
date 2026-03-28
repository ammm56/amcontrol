using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Plc;
using AM.Model.Interfaces.Plc.Config;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services.Plc.Config
{
    /// <summary>
    /// PLC 批量读取块配置 CRUD 服务。
    /// 对应数据库表：plc_read_block。
    /// 用于维护扫描分块策略，避免高频场景逐点离散读取。
    /// </summary>
    public class PlcReadBlockCrudService : ServiceBase, IPlcReadBlockCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "PlcReadBlockCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public PlcReadBlockCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public PlcReadBlockCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// 查询全部批量读取块配置。
        /// </summary>
        public Result<PlcReadBlockConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<PlcReadBlockConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.PlcName)
                    .ThenBy(p => p.Priority)
                    .ThenBy(p => p.SortOrder)
                    .ThenBy(p => p.BlockName)
                    .ToList();

                return OkListLogOnly(items, "PLC读块配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcReadBlockConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "PLC读块配置查询失败");
            }
        }

        /// <summary>
        /// 按 PLC 名称查询全部读块配置。
        /// </summary>
        public Result<PlcReadBlockConfigEntity> QueryByPlcName(string plcName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName))
                {
                    return Fail<PlcReadBlockConfigEntity>((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
                }

                var normalizedPlcName = NormalizeText(plcName);
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<PlcReadBlockConfigEntity>()
                    .Where(p => p.PlcName == normalizedPlcName)
                    .ToList()
                    .OrderBy(p => p.Priority)
                    .ThenBy(p => p.SortOrder)
                    .ThenBy(p => p.BlockName)
                    .ToList();

                return OkListLogOnly(items, "PLC读块配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcReadBlockConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按 PLC 查询读块配置失败");
            }
        }

        /// <summary>
        /// 按 PLC 名称与读块名称查询单个读块。
        /// </summary>
        public Result<PlcReadBlockConfigEntity> QueryByName(string plcName, string blockName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName) || string.IsNullOrWhiteSpace(blockName))
                {
                    return Fail<PlcReadBlockConfigEntity>((int)DbErrorCode.InvalidArgument, "PLC名称或读块名称不能为空");
                }

                var normalizedPlcName = NormalizeText(plcName);
                var normalizedBlockName = NormalizeText(blockName);
                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<PlcReadBlockConfigEntity>()
                    .First(p => p.PlcName == normalizedPlcName && p.BlockName == normalizedBlockName);

                if (item == null)
                {
                    return Warn<PlcReadBlockConfigEntity>((int)DbErrorCode.NotFound, "未找到对应 PLC 读块配置");
                }

                return OkLogOnly(item, "PLC读块配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcReadBlockConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按名称查询 PLC 读块配置失败");
            }
        }

        /// <summary>
        /// 保存批量读取块配置。
        /// </summary>
        public Result Save(PlcReadBlockConfigEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC读块配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                var db = CreateDb();
                EnsureTables(db);

                var stationExists = db.Queryable<PlcStationConfigEntity>()
                    .Any(p => p.Name == entity.PlcName);

                if (!stationExists)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "所属 PLC 站不存在: " + entity.PlcName);
                }

                var existing = db.Queryable<PlcReadBlockConfigEntity>()
                    .First(p => p.PlcName == entity.PlcName && p.BlockName == entity.BlockName && p.Id != entity.Id);

                if (existing != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC读块名称已存在: " + entity.BlockName);
                }

                if (entity.Id > 0)
                {
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("PLC读块配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "PLC读块配置保存失败");
            }
        }

        /// <summary>
        /// 按 PLC 名称与读块名称删除读块配置。
        /// </summary>
        public Result DeleteByName(string plcName, string blockName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName) || string.IsNullOrWhiteSpace(blockName))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC名称或读块名称不能为空");
                }

                var normalizedPlcName = NormalizeText(plcName);
                var normalizedBlockName = NormalizeText(blockName);
                var db = CreateDb();
                EnsureTables(db);

                var count = db.Deleteable<PlcReadBlockConfigEntity>()
                    .Where(p => p.PlcName == normalizedPlcName && p.BlockName == normalizedBlockName)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的 PLC 读块配置");
                }

                return Ok("PLC读块配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "PLC读块配置删除失败");
            }
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void EnsureTables(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(
                typeof(PlcStationConfigEntity),
                typeof(PlcReadBlockConfigEntity));

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_plc_read_block_plcname_blockname ON plc_read_block(PlcName, BlockName)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_plc_read_block_plcname_priority_sortorder ON plc_read_block(PlcName, Priority, SortOrder)");
        }

        private static void Normalize(PlcReadBlockConfigEntity entity)
        {
            entity.PlcName = NormalizeText(entity.PlcName);
            entity.BlockName = NormalizeText(entity.BlockName);
            entity.AreaType = NormalizeText(entity.AreaType);
            entity.StartAddress = NormalizeText(entity.StartAddress);
            entity.ReadUnit = NormalizeText(entity.ReadUnit);
            entity.DataType = NormalizeText(entity.DataType);
            entity.ReadMode = NormalizeText(entity.ReadMode);
            entity.Description = NormalizeText(entity.Description);
            entity.Remark = NormalizeText(entity.Remark);

            if (entity.Length < 0) entity.Length = 0;
            if (entity.Priority < 0) entity.Priority = 0;
            if (entity.SortOrder < 0) entity.SortOrder = 0;

            if (string.IsNullOrWhiteSpace(entity.ReadUnit))
            {
                entity.ReadUnit = "Word";
            }

            if (string.IsNullOrWhiteSpace(entity.DataType))
            {
                entity.DataType = "Mixed";
            }
        }

        private Result Validate(PlcReadBlockConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.PlcName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "所属 PLC 名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.BlockName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "读块名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.AreaType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "读块区域不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.StartAddress))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "起始地址不能为空");
            }

            if (entity.Length <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "读取长度必须大于 0");
            }

            if (string.IsNullOrWhiteSpace(entity.ReadMode))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "读取模式不能为空");
            }

            return OkSilent("PLC读块配置校验通过");
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
