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
    /// PLC 点位配置 CRUD 服务。
    /// 对应数据库表：plc_point
    /// </summary>
    public class PlcPointCrudService : ServiceBase, IPlcPointCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "PlcPointCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public PlcPointCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public PlcPointCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<PlcPointConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<PlcPointConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.PlcName)
                    .ThenBy(p => p.SortOrder)
                    .ThenBy(p => p.Name)
                    .ToList();

                return OkListLogOnly(items, "PLC点位配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "PLC点位配置查询失败");
            }
        }

        public Result<PlcPointConfigEntity> QueryByPlcName(string plcName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName))
                {
                    return Fail<PlcPointConfigEntity>((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
                }

                var normalizedPlcName = NormalizeText(plcName);
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<PlcPointConfigEntity>()
                    .Where(p => p.PlcName == normalizedPlcName)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.Name)
                    .ToList();

                return OkListLogOnly(items, "PLC点位配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按 PLC 查询点位配置失败");
            }
        }

        public Result<PlcPointConfigEntity> QueryByName(string plcName, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName) || string.IsNullOrWhiteSpace(name))
                {
                    return Fail<PlcPointConfigEntity>((int)DbErrorCode.InvalidArgument, "PLC名称或点位名称不能为空");
                }

                var normalizedPlcName = NormalizeText(plcName);
                var normalizedName = NormalizeText(name);

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<PlcPointConfigEntity>()
                    .First(p => p.PlcName == normalizedPlcName && p.Name == normalizedName);

                if (item == null)
                {
                    return Warn<PlcPointConfigEntity>((int)DbErrorCode.NotFound, "未找到对应 PLC 点位配置");
                }

                return OkLogOnly(item, "PLC点位配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按名称查询 PLC 点位配置失败");
            }
        }

        public Result Save(PlcPointConfigEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC点位配置不能为空");
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

                var existing = db.Queryable<PlcPointConfigEntity>()
                    .First(p => p.PlcName == entity.PlcName && p.Name == entity.Name && p.Id != entity.Id);

                if (existing != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC点位名称已存在: " + entity.Name);
                }

                if (entity.Id > 0)
                {
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("PLC点位配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "PLC点位配置保存失败");
            }
        }

        public Result DeleteByName(string plcName, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName) || string.IsNullOrWhiteSpace(name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC名称或点位名称不能为空");
                }

                var normalizedPlcName = NormalizeText(plcName);
                var normalizedName = NormalizeText(name);

                var db = CreateDb();
                EnsureTables(db);

                var count = db.Deleteable<PlcPointConfigEntity>()
                    .Where(p => p.PlcName == normalizedPlcName && p.Name == normalizedName)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的 PLC 点位配置");
                }

                return Ok("PLC点位配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "PLC点位配置删除失败");
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
                typeof(PlcPointConfigEntity));

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_plc_point_plcname_name ON plc_point(PlcName, Name)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_plc_point_plcname_sortorder ON plc_point(PlcName, SortOrder)");
        }

        private static void Normalize(PlcPointConfigEntity entity)
        {
            entity.PlcName = NormalizeText(entity.PlcName);
            entity.Name = NormalizeText(entity.Name);
            entity.DisplayName = NormalizeText(entity.DisplayName);
            entity.GroupName = NormalizeText(entity.GroupName);
            entity.AreaType = NormalizeText(entity.AreaType);
            entity.Address = NormalizeText(entity.Address);
            entity.DataType = NormalizeText(entity.DataType);
            entity.Unit = NormalizeText(entity.Unit);
            entity.AccessMode = NormalizeText(entity.AccessMode);
            entity.ReadMode = NormalizeText(entity.ReadMode);
            entity.BatchKey = NormalizeText(entity.BatchKey);
            entity.ByteOrder = NormalizeText(entity.ByteOrder);
            entity.WordOrder = NormalizeText(entity.WordOrder);
            entity.StringEncoding = NormalizeText(entity.StringEncoding);
            entity.Description = NormalizeText(entity.Description);
            entity.Remark = NormalizeText(entity.Remark);

            if (string.IsNullOrWhiteSpace(entity.DisplayName) && !string.IsNullOrWhiteSpace(entity.Name))
            {
                entity.DisplayName = entity.Name;
            }

            if (entity.StringLength < 0) entity.StringLength = 0;
            if (entity.ArrayLength < 0) entity.ArrayLength = 0;
            if (entity.ReadLength < 0) entity.ReadLength = 0;
            if (entity.SortOrder < 0) entity.SortOrder = 0;

            if (entity.Scale == 0)
            {
                entity.Scale = 1D;
            }
        }

        private Result Validate(PlcPointConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.PlcName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "所属 PLC 名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "点位名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.AreaType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "地址区域不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.Address))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "地址不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.DataType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "数据类型不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.AccessMode))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "访问模式不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.ReadMode))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "读取模式不能为空");
            }

            if (string.Equals(entity.DataType, "String", StringComparison.OrdinalIgnoreCase) && entity.StringLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "String 类型点位必须配置大于 0 的字符串长度");
            }

            if (string.Equals(entity.DataType, "ByteArray", StringComparison.OrdinalIgnoreCase) && entity.ArrayLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "ByteArray 类型点位必须配置大于 0 的数组长度");
            }

            return OkSilent("PLC点位配置校验通过");
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}