using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Interfaces.DB;
using AM.Model.MotionCard;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 设备配置仓储。
    /// 负责数据库层设备配置读取。
    /// </summary>
    public class MachineConfigRepository : ServiceBase, IMachineConfigRepository
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MachineConfigRepo"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MachineConfigRepository()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MachineConfigRepository(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result EnsureTables()
        {
            try
            {
                var db = CreateDb();
                db.CodeFirst.InitTables(
                    typeof(MotionCardEntity),
                    typeof(MotionAxisEntity),
                    typeof(MotionIoMapEntity),
                    typeof(MotionAxisConfigEntity));

                return Ok("运动配置表初始化完成");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.QueryFailed, "运动配置表初始化失败");
            }
        }

        public Result<MachineConfigData> LoadAll()
        {
            try
            {
                var ensureResult = EnsureTables();
                if (!ensureResult.Success)
                {
                    return Fail<MachineConfigData>(ensureResult.Code, ensureResult.Message);
                }

                var db = CreateDb();

                var cards = db.Queryable<MotionCardEntity>()
                    .Where(p => p.IsEnabled)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.CardId)
                    .ToList();

                var axes = db.Queryable<MotionAxisEntity>()
                    .Where(p => p.IsEnabled)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalAxis)
                    .ToList();

                var ioMaps = db.Queryable<MotionIoMapEntity>()
                    .Where(p => p.IsEnabled)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();

                var data = new MachineConfigData
                {
                    MotionCards = cards,
                    MotionAxes = axes,
                    MotionIoMaps = ioMaps
                };

                return Ok(data, "读取设备配置原始数据成功");
            }
            catch (Exception ex)
            {
                return HandleException<MachineConfigData>(ex, (int)DbErrorCode.QueryFailed, "读取设备配置原始数据失败");
            }
        }

        public Result<MotionCardEntity> QueryAllCards()
        {
            try
            {
                var db = CreateDb();
                var items = db.Queryable<MotionCardEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.CardId)
                    .ToList();

                return OkList(items, "控制卡查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionCardEntity>(ex, (int)DbErrorCode.QueryFailed, "控制卡查询失败");
            }
        }

        public Result<MotionAxisEntity> QueryAllAxes()
        {
            try
            {
                var db = CreateDb();
                var items = db.Queryable<MotionAxisEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalAxis)
                    .ToList();

                return OkList(items, "轴定义查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisEntity>(ex, (int)DbErrorCode.QueryFailed, "轴定义查询失败");
            }
        }

        public Result<MotionIoMapEntity> QueryAllIoMaps()
        {
            try
            {
                var db = CreateDb();
                var items = db.Queryable<MotionIoMapEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();

                return OkList(items, "IO映射查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoMapEntity>(ex, (int)DbErrorCode.QueryFailed, "IO映射查询失败");
            }
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }
    }
}