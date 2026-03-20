using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Interfaces.DB;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 轴拓扑配置 CRUD 服务。
    /// </summary>
    public class MotionAxisCrudService : ServiceBase, IMotionAxisCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionAxisCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionAxisCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionAxisCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<MotionAxisEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

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

        public Result<MotionAxisEntity> QueryByLogicalAxis(short logicalAxis)
        {
            try
            {
                if (logicalAxis <= 0)
                {
                    return Fail<MotionAxisEntity>((int)DbErrorCode.InvalidArgument, "逻辑轴号无效");
                }

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<MotionAxisEntity>()
                    .First(p => p.LogicalAxis == logicalAxis);

                if (item == null)
                {
                    return Warn<MotionAxisEntity>((int)DbErrorCode.NotFound, "未找到对应逻辑轴");
                }

                return Ok(item, "轴定义查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisEntity>(ex, (int)DbErrorCode.QueryFailed, "按逻辑轴查询失败");
            }
        }

        public Result<MotionAxisEntity> QueryByCardId(short cardId)
        {
            try
            {
                if (cardId < 0)
                {
                    return Fail<MotionAxisEntity>((int)DbErrorCode.InvalidArgument, "控制卡 CardId 无效");
                }

                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<MotionAxisEntity>()
                    .Where(p => p.CardId == cardId)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalAxis)
                    .ToList();

                return OkList(items, "按控制卡查询轴定义成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisEntity>(ex, (int)DbErrorCode.QueryFailed, "按控制卡查询轴定义失败");
            }
        }

        public Result Save(MotionAxisEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "轴定义不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                db = CreateDb();
                EnsureTables(db);

                var cardExists = db.Queryable<MotionCardEntity>().Any(p => p.CardId == entity.CardId);
                if (!cardExists)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "所属控制卡不存在: " + entity.CardId);
                }

                var existingByLogicalAxis = db.Queryable<MotionAxisEntity>()
                    .First(p => p.LogicalAxis == entity.LogicalAxis);

                if (existingByLogicalAxis != null && existingByLogicalAxis.Id != entity.Id)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "逻辑轴已存在: " + entity.LogicalAxis);
                }

                var duplicateAxisId = db.Queryable<MotionAxisEntity>()
                    .First(p => p.CardId == entity.CardId && p.AxisId == entity.AxisId && p.Id != entity.Id);

                if (duplicateAxisId != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "同一卡下 AxisId 已存在: " + entity.AxisId);
                }

                var duplicatePhysicalAxis = db.Queryable<MotionAxisEntity>()
                    .First(p => p.CardId == entity.CardId && p.PhysicalAxis == entity.PhysicalAxis && p.Id != entity.Id);

                if (duplicatePhysicalAxis != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "同一卡下 PhysicalAxis 已存在: " + entity.PhysicalAxis);
                }

                if (entity.Id > 0)
                {
                    db.Updateable(entity).ExecuteCommand();
                }
                else if (existingByLogicalAxis != null)
                {
                    entity.Id = existingByLogicalAxis.Id;
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("轴定义保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "轴定义保存失败");
            }
        }

        public Result DeleteByLogicalAxis(short logicalAxis)
        {
            SqlSugarClient db = null;

            try
            {
                if (logicalAxis <= 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "逻辑轴号无效");
                }

                db = CreateDb();
                EnsureTables(db);

                var axis = db.Queryable<MotionAxisEntity>()
                    .First(p => p.LogicalAxis == logicalAxis);

                if (axis == null)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的轴定义");
                }

                db.Ado.BeginTran();

                db.Deleteable<MotionAxisConfigEntity>()
                    .Where(p => p.LogicalAxis == logicalAxis)
                    .ExecuteCommand();

                db.Deleteable<MotionAxisEntity>()
                    .Where(p => p.LogicalAxis == logicalAxis)
                    .ExecuteCommand();

                db.Ado.CommitTran();

                return Ok("轴定义删除成功");
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

                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "轴定义删除失败");
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
                typeof(MotionAxisConfigEntity));
        }

        private static void Normalize(MotionAxisEntity entity)
        {
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? null : entity.Name.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();
        }

        private Result Validate(MotionAxisEntity entity)
        {
            if (entity.CardId < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "所属控制卡 CardId 无效");
            }

            if (entity.LogicalAxis <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "逻辑轴号必须大于 0");
            }

            if (entity.AxisId < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "AxisId 不能小于 0");
            }

            if (entity.PhysicalCore <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "物理内核号必须大于 0");
            }

            if (entity.PhysicalAxis < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "物理轴号不能小于 0");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "轴名称不能为空");
            }

            return Ok("轴定义校验通过");
        }
    }
}