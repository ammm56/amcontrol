using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Plc;
using AM.Model.Interfaces.DB.Plc.Config;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services.Plc.Config
{
    /// <summary>
    /// PLC 站配置 CRUD 服务。
    /// 对应数据库表：plc_station
    /// </summary>
    public class PlcStationCrudService : ServiceBase, IPlcStationCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "PlcStationCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public PlcStationCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public PlcStationCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<PlcStationConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<PlcStationConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.Name)
                    .ToList();

                return OkListLogOnly(items, "PLC站配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcStationConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "PLC站配置查询失败");
            }
        }

        public Result<PlcStationConfigEntity> QueryByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail<PlcStationConfigEntity>((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
                }

                var normalizedName = NormalizeName(name);
                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<PlcStationConfigEntity>()
                    .First(p => p.Name == normalizedName);

                if (item == null)
                {
                    return Warn<PlcStationConfigEntity>((int)DbErrorCode.NotFound, "未找到对应 PLC 站配置");
                }

                return OkLogOnly(item, "PLC站配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcStationConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按名称查询 PLC 站配置失败");
            }
        }

        public Result Save(PlcStationConfigEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC站配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                var db = CreateDb();
                EnsureTables(db);

                var existingByName = db.Queryable<PlcStationConfigEntity>()
                    .First(p => p.Name == entity.Name && p.Id != entity.Id);

                if (existingByName != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC名称已存在: " + entity.Name);
                }

                var now = DateTime.Now;
                if (entity.Id <= 0 && entity.CreateTime == default(DateTime))
                {
                    entity.CreateTime = now;
                }

                entity.UpdateTime = now;

                if (entity.Id > 0)
                {
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("PLC站配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "PLC站配置保存失败");
            }
        }

        public Result DeleteByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
                }

                var normalizedName = NormalizeName(name);
                var db = CreateDb();
                EnsureTables(db);

                var station = db.Queryable<PlcStationConfigEntity>()
                    .First(p => p.Name == normalizedName);

                if (station == null)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的 PLC 站配置");
                }

                var hasPoint = db.Queryable<PlcPointConfigEntity>()
                    .Any(p => p.PlcName == normalizedName);

                if (hasPoint)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "该 PLC 下仍存在点位配置，禁止删除");
                }

                db.Deleteable<PlcStationConfigEntity>()
                    .Where(p => p.Name == normalizedName)
                    .ExecuteCommand();

                return Ok("PLC站配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "PLC站配置删除失败");
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
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_plc_station_name ON plc_station(Name)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_plc_station_sortorder ON plc_station(SortOrder)");
        }

        private static void Normalize(PlcStationConfigEntity entity)
        {
            entity.Name = NormalizeName(entity.Name);
            entity.DisplayName = NormalizeNullableText(entity.DisplayName);
            entity.Vendor = NormalizeNullableText(entity.Vendor);
            entity.Model = NormalizeNullableText(entity.Model);
            entity.ConnectionType = NormalizeNullableText(entity.ConnectionType);
            entity.ProtocolType = NormalizeNullableText(entity.ProtocolType);
            entity.IpAddress = NormalizeNullableText(entity.IpAddress);
            entity.ComPort = NormalizeNullableText(entity.ComPort);
            entity.Parity = NormalizeNullableText(entity.Parity);
            entity.StopBits = NormalizeNullableText(entity.StopBits);
            entity.Description = NormalizeNullableText(entity.Description);
            entity.Remark = NormalizeNullableText(entity.Remark);

            if (string.IsNullOrWhiteSpace(entity.DisplayName) && !string.IsNullOrWhiteSpace(entity.Name))
            {
                entity.DisplayName = entity.Name;
            }

            if (entity.TimeoutMs <= 0)
            {
                entity.TimeoutMs = 3000;
            }

            if (entity.ReconnectIntervalMs <= 0)
            {
                entity.ReconnectIntervalMs = 2000;
            }

            if (entity.ScanIntervalMs <= 0)
            {
                entity.ScanIntervalMs = 200;
            }

            if (entity.SortOrder < 0)
            {
                entity.SortOrder = 0;
            }
        }

        private Result Validate(PlcStationConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.ConnectionType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "PLC连接方式不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.ProtocolType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "PLC通讯协议不能为空");
            }

            if (entity.TimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "通讯超时必须大于 0");
            }

            if (entity.ReconnectIntervalMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "重连周期必须大于 0");
            }

            if (entity.ScanIntervalMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "采样周期必须大于 0");
            }

            if (string.Equals(entity.ConnectionType, "Tcp", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(entity.IpAddress))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "TCP 连接方式下 IP 地址不能为空");
                }

                if (!entity.Port.HasValue || entity.Port.Value <= 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "TCP 连接方式下端口必须大于 0");
                }
            }

            if (string.Equals(entity.ConnectionType, "Serial", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(entity.ComPort))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "串口连接方式下串口号不能为空");
                }

                if (!entity.BaudRate.HasValue || entity.BaudRate.Value <= 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "串口连接方式下波特率必须大于 0");
                }
            }

            return OkSilent("PLC站配置校验通过");
        }

        private static string NormalizeName(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string NormalizeNullableText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}