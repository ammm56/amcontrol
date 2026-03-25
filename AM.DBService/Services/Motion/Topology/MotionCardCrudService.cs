using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.Topology;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services.Motion.Topology
{
    /// <summary>
    /// 控制卡配置 CRUD 服务。
    /// 对应数据库表：motion_card
    /// 负责控制卡静态拓扑、驱动识别、初始化顺序等配置的查询、保存与删除。
    /// </summary>
    public class MotionCardCrudService : ServiceBase, IMotionCardCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionCardCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionCardCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionCardCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<MotionCardEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<MotionCardEntity>()
                    .ToList()
                    .OrderBy(p => p.InitOrder)
                    .ThenBy(p => p.SortOrder)
                    .ThenBy(p => p.CardId)
                    .ToList();

                return OkListLogOnly(items, "控制卡查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionCardEntity>(ex, (int)DbErrorCode.QueryFailed, "控制卡查询失败");
            }
        }

        public Result<MotionCardEntity> QueryByCardId(short cardId)
        {
            try
            {
                if (cardId < 0)
                {
                    return Fail<MotionCardEntity>((int)DbErrorCode.InvalidArgument, "控制卡 CardId 无效");
                }

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<MotionCardEntity>()
                    .First(p => p.CardId == cardId);

                if (item == null)
                {
                    return Warn<MotionCardEntity>((int)DbErrorCode.NotFound, "未找到对应控制卡");
                }

                return OkLogOnly(item, "控制卡查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionCardEntity>(ex, (int)DbErrorCode.QueryFailed, "按 CardId 查询控制卡失败");
            }
        }

        public Result Save(MotionCardEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "控制卡不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                db = CreateDb();
                EnsureTables(db);

                var existingByCardId = db.Queryable<MotionCardEntity>()
                    .First(p => p.CardId == entity.CardId);

                if (existingByCardId != null && existingByCardId.Id != entity.Id)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "控制卡 CardId 已存在: " + entity.CardId);
                }

                var existingByName = db.Queryable<MotionCardEntity>()
                    .First(p => p.Name == entity.Name && p.Id != entity.Id);

                if (existingByName != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "控制卡名称已存在: " + entity.Name);
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
                else if (existingByCardId != null)
                {
                    entity.Id = existingByCardId.Id;
                    entity.CreateTime = existingByCardId.CreateTime;
                    entity.UpdateTime = now;
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("控制卡保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "控制卡保存失败");
            }
        }

        public Result DeleteByCardId(short cardId)
        {
            SqlSugarClient db = null;

            try
            {
                if (cardId < 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "控制卡 CardId 无效");
                }

                db = CreateDb();
                EnsureTables(db);

                var card = db.Queryable<MotionCardEntity>()
                    .First(p => p.CardId == cardId);

                if (card == null)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的控制卡");
                }

                var hasAxis = db.Queryable<MotionAxisEntity>().Any(p => p.CardId == cardId);
                if (hasAxis)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "该控制卡下仍存在轴定义，禁止删除");
                }

                var hasIo = db.Queryable<MotionIoMapEntity>().Any(p => p.CardId == cardId);
                if (hasIo)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "该控制卡下仍存在 IO 映射，禁止删除");
                }

                db.Deleteable<MotionCardEntity>()
                    .Where(p => p.CardId == cardId)
                    .ExecuteCommand();

                return Ok("控制卡删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "控制卡删除失败");
            }
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void EnsureTables(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(
                typeof(MotionCardEntity),
                typeof(MotionAxisEntity),
                typeof(MotionIoMapEntity),
                typeof(MotionAxisConfigEntity));

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_card_cardid ON motion_card(CardId)");

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_card_name ON motion_card(Name)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_motion_card_initorder ON motion_card(InitOrder)");
        }

        private static void Normalize(MotionCardEntity entity)
        {
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? null : entity.Name.Trim();
            entity.DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? null : entity.DisplayName.Trim();
            entity.DriverKey = string.IsNullOrWhiteSpace(entity.DriverKey) ? null : entity.DriverKey.Trim();
            entity.OpenConfig = string.IsNullOrWhiteSpace(entity.OpenConfig) ? null : entity.OpenConfig.Trim();
            entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();

            if (string.IsNullOrWhiteSpace(entity.DisplayName) && !string.IsNullOrWhiteSpace(entity.Name))
            {
                entity.DisplayName = entity.Name;
            }
        }

        private Result Validate(MotionCardEntity entity)
        {
            if (entity.CardId < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "控制卡 CardId 不能小于 0");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "控制卡名称不能为空");
            }

            if (entity.CoreNumber <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "控制卡内核数量必须大于 0");
            }

            if (entity.AxisCountNumber < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "控制卡轴总数不能小于 0");
            }

            if (entity.InitOrder < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "初始化顺序不能小于 0");
            }

            return OkSilent("控制卡校验通过");
        }
    }
}