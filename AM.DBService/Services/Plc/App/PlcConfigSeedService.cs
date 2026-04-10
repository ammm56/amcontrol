using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Plc;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace AM.DBService.Services.Plc.App
{
    /// <summary>
    /// PLC 配置种子服务。
    /// 启动时自动补一组最小默认 ModbusTcp 测试数据。
    /// 当前版本点位使用单一 Length 字段表达长度：
    /// - 标量：1
    /// - 字符串：字符长度
    /// - 数组：元素个数
    /// </summary>
    public class PlcConfigSeedService
    {
        private readonly IAppReporter _reporter;
        private readonly DBContext _dbContext;

        public PlcConfigSeedService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public PlcConfigSeedService(IAppReporter reporter)
        {
            _reporter = reporter;
            _dbContext = new DBContext();
        }

        public Result EnsureSeedData()
        {
            SqlSugarClient db = null;

            try
            {
                db = _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });

                db.CodeFirst.InitTables(
                    typeof(PlcStationConfigEntity),
                    typeof(PlcPointConfigEntity));

                EnsureIndexes(db);

                if (HasAnyPlcConfigData(db))
                {
                    _reporter.Info("PlcConfigSeed", "PLC 配置种子已存在，跳过初始化");
                    return Result.Ok("PLC 配置种子已存在", ResultSource.Database);
                }

                db.Ado.BeginTran();

                var stations = CreateDefaultStations();
                var points = CreateDefaultPoints();

                db.Insertable(stations).ExecuteCommand();
                db.Insertable(points).ExecuteCommand();

                db.Ado.CommitTran();

                _reporter.Info("PlcConfigSeed", "默认 PLC 配置种子初始化完成");
                return Result.Ok("默认 PLC 配置种子初始化完成", ResultSource.Database);
            }
            catch (Exception ex)
            {
                if (db != null)
                {
                    try
                    {
                        db.Ado.RollbackTran();
                    }
                    catch
                    {
                    }
                }

                _reporter.Error("PlcConfigSeed", ex, "默认 PLC 配置种子初始化失败");
                return Result.Fail((int)DbErrorCode.SaveFailed, "默认 PLC 配置种子初始化失败", ResultSource.Database);
            }
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

        private static bool HasAnyPlcConfigData(SqlSugarClient db)
        {
            return db.Queryable<PlcStationConfigEntity>().Any()
                || db.Queryable<PlcPointConfigEntity>().Any();
        }

        private static List<PlcStationConfigEntity> CreateDefaultStations()
        {
            var now = DateTime.Now;

            return new List<PlcStationConfigEntity>
            {
                new PlcStationConfigEntity
                {
                    Name = "TestModbusTcp1",
                    DisplayName = "默认测试PLC",
                    Vendor = "ModbusGeneric",
                    Model = "Simulator",
                    ConnectionType = "Tcp",
                    ProtocolType = "ModbusTcp",
                    IpAddress = "127.0.0.1",
                    Port = 502,
                    ComPort = null,
                    BaudRate = null,
                    DataBits = null,
                    Parity = null,
                    StopBits = null,
                    StationNo = 1,
                    NetworkNo = null,
                    PcNo = null,
                    Rack = null,
                    Slot = null,
                    TimeoutMs = 1000,
                    ReconnectIntervalMs = 2000,
                    ScanIntervalMs = 200,
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认 ModbusTcp 本地测试站",
                    Remark = "用于逐点读写链路验证",
                    CreateTime = now,
                    UpdateTime = now
                }
            };
        }

        private static List<PlcPointConfigEntity> CreateDefaultPoints()
        {
            return new List<PlcPointConfigEntity>
            {
                new PlcPointConfigEntity
                {
                    PlcName = "TestModbusTcp1",
                    Name = "PlcReady",
                    DisplayName = "PLC就绪",
                    GroupName = "Default",
                    Address = "00001",
                    DataType = "bool",
                    Length = 1,
                    AccessMode = "ReadWrite",
                    IsEnabled = true,
                    SortOrder = 1,
                    Description = "默认布尔测试点",
                    Remark = "Modbus Coil"
                },
                new PlcPointConfigEntity
                {
                    PlcName = "TestModbusTcp1",
                    Name = "Heartbeat",
                    DisplayName = "心跳计数",
                    GroupName = "Default",
                    Address = "40010",
                    DataType = "uint16",
                    Length = 1,
                    AccessMode = "ReadWrite",
                    IsEnabled = true,
                    SortOrder = 2,
                    Description = "默认无符号整型测试点",
                    Remark = "Modbus Holding Register"
                },
                new PlcPointConfigEntity
                {
                    PlcName = "TestModbusTcp1",
                    Name = "Temperature",
                    DisplayName = "温度值",
                    GroupName = "Default",
                    Address = "40020",
                    DataType = "float",
                    Length = 1,
                    AccessMode = "ReadWrite",
                    IsEnabled = true,
                    SortOrder = 3,
                    Description = "默认浮点测试点",
                    Remark = "Modbus Holding Register"
                },
                new PlcPointConfigEntity
                {
                    PlcName = "TestModbusTcp1",
                    Name = "ProductCode",
                    DisplayName = "产品编码",
                    GroupName = "Default",
                    Address = "40040",
                    DataType = "string",
                    Length = 20,
                    AccessMode = "ReadWrite",
                    IsEnabled = true,
                    SortOrder = 4,
                    Description = "默认字符串测试点",
                    Remark = "Modbus Fixed String"
                }
            };
        }
    }
}