using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.DBService.Services.Plc.Config;
using AM.DBService.Services.Plc.Driver;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Plc;
using AM.Model.Interfaces.DB.Plc.App;
using AM.Model.Interfaces.DB.Plc.Config;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Plc.App
{
    /// <summary>
    /// PLC 配置应用服务。
    /// 负责将数据库中的 PLC 站/点位配置装配成运行时配置，并同步到全局上下文。
    /// </summary>
    public class PlcConfigAppService : ServiceBase, IPlcConfigAppService
    {
        private readonly DBContext _dbContext;
        private readonly IPlcStationCrudService _plcStationCrudService;
        private readonly IPlcPointCrudService _plcPointCrudService;
        private readonly IPlcClientFactory _plcClientFactory;

        protected override string MessageSourceName
        {
            get { return "PlcConfigApp"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public PlcConfigAppService()
            : this(
                new PlcStationCrudService(),
                new PlcPointCrudService(),
                new PlcClientFactory(),
                SystemContext.Instance.Reporter)
        {
        }

        public PlcConfigAppService(
            IPlcStationCrudService plcStationCrudService,
            IPlcPointCrudService plcPointCrudService,
            IPlcClientFactory plcClientFactory,
            IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
            _plcStationCrudService = plcStationCrudService;
            _plcPointCrudService = plcPointCrudService;
            _plcClientFactory = plcClientFactory;
        }

        public Result EnsureTables()
        {
            try
            {
                var db = CreateDb();

                db.CodeFirst.InitTables(
                    typeof(PlcStationConfigEntity),
                    typeof(PlcPointConfigEntity));

                EnsureIndexes(db);

                return OkLogOnly("PLC 配置表初始化完成");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.QueryFailed, "PLC 配置表初始化失败");
            }
        }

        public Result<PlcConfig> QueryAll()
        {
            try
            {
                var ensureResult = EnsureTables();
                if (!ensureResult.Success)
                {
                    return Fail<PlcConfig>(ensureResult.Code, ensureResult.Message);
                }

                var stationResult = _plcStationCrudService.QueryAll();
                if (!stationResult.Success && stationResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<PlcConfig>(stationResult.Code, "读取 PLC 站配置失败");
                }

                var pointResult = _plcPointCrudService.QueryAll();
                if (!pointResult.Success && pointResult.Code != (int)DbErrorCode.NotFound)
                {
                    return Fail<PlcConfig>(pointResult.Code, "读取 PLC 点位配置失败");
                }

                var stationEntities = stationResult.Success
                    ? stationResult.Items.Where(p => p != null && p.IsEnabled).ToList()
                    : new List<PlcStationConfigEntity>();

                var pointEntities = pointResult.Success
                    ? pointResult.Items.Where(p => p != null && p.IsEnabled).ToList()
                    : new List<PlcPointConfigEntity>();

                var validateResult = ValidateEntities(stationEntities, pointEntities);
                if (!validateResult.Success)
                {
                    return Fail<PlcConfig>(validateResult.Code, validateResult.Message);
                }

                var plcConfig = BuildPlcConfig(stationEntities, pointEntities);

                return OkLogOnly(plcConfig, "读取数据库 PLC 配置成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcConfig>(ex, (int)DbErrorCode.QueryFailed, "读取数据库 PLC 配置失败");
            }
        }

        public Result ReloadFromDatabase()
        {
            var queryResult = QueryAll();
            if (!queryResult.Success)
            {
                return Fail(queryResult.Code, "PLC 配置重载失败");
            }

            var plcConfig = queryResult.Item ?? new PlcConfig();

            ConfigContext.Instance.Config.PlcConfig = plcConfig;

            RebuildPlcs(plcConfig);

            RuntimeContext.Instance.Plc.Clear();

            return OkLogOnly("PLC 运行时配置重载成功");
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void EnsureIndexes(SqlSugarClient db)
        {
            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_plc_station_name ON plc_station(Name)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_plc_station_sortorder ON plc_station(SortOrder)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_plc_point_plcname_name ON plc_point(PlcName, Name)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_plc_point_plcname_sortorder ON plc_point(PlcName, SortOrder)");
        }

        private Result ValidateEntities(
            IList<PlcStationConfigEntity> stationEntities,
            IList<PlcPointConfigEntity> pointEntities)
        {
            var stationNameSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var station in stationEntities)
            {
                if (string.IsNullOrWhiteSpace(station.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "存在 PLC 站名称为空的配置");
                }

                if (!stationNameSet.Add(station.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "存在重复 PLC 站名称: " + station.Name);
                }
            }

            foreach (var point in pointEntities)
            {
                if (string.IsNullOrWhiteSpace(point.PlcName))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "存在未关联 PLC 的点位配置: " + point.Name);
                }

                if (!stationNameSet.Contains(point.PlcName))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "点位所属 PLC 不存在或未启用: " + point.PlcName + " / " + point.Name);
                }

                if (string.Equals(point.DataType, "String", StringComparison.OrdinalIgnoreCase) && point.StringLength <= 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "String 类型点位必须配置大于 0 的字符串长度: " + point.Name);
                }

                if (string.Equals(point.DataType, "ByteArray", StringComparison.OrdinalIgnoreCase) && point.ArrayLength <= 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "ByteArray 类型点位必须配置大于 0 的数组长度: " + point.Name);
                }
            }

            return OkSilent("PLC 配置校验通过");
        }

        private static PlcConfig BuildPlcConfig(
            IList<PlcStationConfigEntity> stationEntities,
            IList<PlcPointConfigEntity> pointEntities)
        {
            var stationList = stationEntities
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .Select(ToStationConfig)
                .ToList();

            var stationNameSet = new HashSet<string>(stationList.Select(p => p.Name), StringComparer.OrdinalIgnoreCase);

            var pointList = pointEntities
                .Where(p => stationNameSet.Contains(p.PlcName))
                .OrderBy(p => p.PlcName)
                .ThenBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .Select(ToPointConfig)
                .ToList();

            return new PlcConfig
            {
                Stations = stationList,
                Points = pointList,
                ReadBlocks = new List<PlcReadBlockConfig>()
            };
        }

        private void RebuildPlcs(PlcConfig plcConfig)
        {
            var machine = MachineContext.Instance;
            machine.Plcs.Clear();

            if (plcConfig == null || plcConfig.Stations == null || plcConfig.Stations.Count == 0)
            {
                return;
            }

            foreach (var station in plcConfig.Stations
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name))
            {
                if (string.IsNullOrWhiteSpace(station.Name))
                {
                    continue;
                }

                var client = _plcClientFactory.Create(station);
                if (client == null)
                {
                    continue;
                }

                machine.Plcs[station.Name] = client;
            }
        }

        private static PlcStationConfig ToStationConfig(PlcStationConfigEntity entity)
        {
            return new PlcStationConfig
            {
                Id = entity.Id,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Vendor = entity.Vendor,
                Model = entity.Model,
                ConnectionType = entity.ConnectionType,
                ProtocolType = entity.ProtocolType,
                IpAddress = entity.IpAddress,
                Port = entity.Port,
                ComPort = entity.ComPort,
                BaudRate = entity.BaudRate,
                DataBits = entity.DataBits,
                Parity = entity.Parity,
                StopBits = entity.StopBits,
                StationNo = entity.StationNo,
                NetworkNo = entity.NetworkNo,
                PcNo = entity.PcNo,
                Rack = entity.Rack,
                Slot = entity.Slot,
                TimeoutMs = entity.TimeoutMs,
                ReconnectIntervalMs = entity.ReconnectIntervalMs,
                ScanIntervalMs = entity.ScanIntervalMs,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description,
                Remark = entity.Remark
            };
        }

        private static PlcPointConfig ToPointConfig(PlcPointConfigEntity entity)
        {
            return new PlcPointConfig
            {
                Id = entity.Id,
                PlcName = entity.PlcName,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                GroupName = entity.GroupName,
                AreaType = entity.AreaType,
                Address = entity.Address,
                BitIndex = entity.BitIndex,
                DataType = entity.DataType,
                StringLength = entity.StringLength,
                ArrayLength = entity.ArrayLength,
                ReadLength = entity.ReadLength,
                Scale = entity.Scale,
                Offset = entity.Offset,
                Unit = entity.Unit,
                AccessMode = entity.AccessMode,
                ReadMode = entity.ReadMode,
                BatchKey = entity.BatchKey,
                ByteOrder = entity.ByteOrder,
                WordOrder = entity.WordOrder,
                StringEncoding = entity.StringEncoding,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description,
                Remark = entity.Remark
            };
        }
    }
}