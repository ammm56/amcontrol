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
    /// IO 映射配置 CRUD 服务。
    /// </summary>
    public class MotionIoMapCrudService : ServiceBase, IMotionIoMapCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionIoMapCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionIoMapCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionIoMapCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<MotionIoMapEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

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

        public Result<MotionIoMapEntity> QueryByCardId(short cardId)
        {
            try
            {
                if (cardId < 0)
                {
                    return Fail<MotionIoMapEntity>((int)DbErrorCode.InvalidArgument, "控制卡 CardId 无效");
                }

                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<MotionIoMapEntity>()
                    .Where(p => p.CardId == cardId)
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();

                return OkList(items, "按控制卡查询 IO 映射成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoMapEntity>(ex, (int)DbErrorCode.QueryFailed, "按控制卡查询 IO 映射失败");
            }
        }

        public Result<MotionIoMapEntity> QueryByLogicalBit(short logicalBit, string ioType)
        {
            try
            {
                if (logicalBit <= 0 || string.IsNullOrWhiteSpace(ioType))
                {
                    return Fail<MotionIoMapEntity>((int)DbErrorCode.InvalidArgument, "逻辑位号或 IO 类型无效");
                }

                var normalizedIoType = NormalizeIoType(ioType);
                if (normalizedIoType == null)
                {
                    return Fail<MotionIoMapEntity>((int)DbErrorCode.InvalidArgument, "IO 类型仅支持 DI 或 DO");
                }

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<MotionIoMapEntity>()
                    .First(p => p.LogicalBit == logicalBit && p.IoType == normalizedIoType);

                if (item == null)
                {
                    return Warn<MotionIoMapEntity>((int)DbErrorCode.NotFound, "未找到对应 IO 映射");
                }

                return Ok(item, "IO映射查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoMapEntity>(ex, (int)DbErrorCode.QueryFailed, "按逻辑位查询 IO 映射失败");
            }
        }

        public Result Save(MotionIoMapEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "IO映射不能为空");
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

                var existingByLogicalBit = db.Queryable<MotionIoMapEntity>()
                    .First(p => p.LogicalBit == entity.LogicalBit && p.IoType == entity.IoType);

                if (existingByLogicalBit != null && existingByLogicalBit.Id != entity.Id)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "相同 IO 类型下逻辑位已存在: " + entity.LogicalBit);
                }

                var duplicateHardwareBit = db.Queryable<MotionIoMapEntity>()
                    .First(p => p.CardId == entity.CardId
                             && p.IoType == entity.IoType
                             && p.HardwareBit == entity.HardwareBit
                             && p.Id != entity.Id);

                if (duplicateHardwareBit != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "同一卡同类型下 HardwareBit 已存在: " + entity.HardwareBit);
                }

                if (entity.Id > 0)
                {
                    db.Updateable(entity).ExecuteCommand();
                }
                else if (existingByLogicalBit != null)
                {
                    entity.Id = existingByLogicalBit.Id;
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("IO映射保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "IO映射保存失败");
            }
        }

        public Result DeleteByLogicalBit(short logicalBit, string ioType)
        {
            try
            {
                if (logicalBit <= 0 || string.IsNullOrWhiteSpace(ioType))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "逻辑位号或 IO 类型无效");
                }

                var normalizedIoType = NormalizeIoType(ioType);
                if (normalizedIoType == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "IO 类型仅支持 DI 或 DO");
                }

                var db = CreateDb();
                EnsureTables(db);

                var count = db.Deleteable<MotionIoMapEntity>()
                    .Where(p => p.LogicalBit == logicalBit && p.IoType == normalizedIoType)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的 IO 映射");
                }

                return Ok("IO映射删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "IO映射删除失败");
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
                typeof(MotionIoMapEntity));
        }

        private static void Normalize(MotionIoMapEntity entity)
        {
            entity.IoType = NormalizeIoType(entity.IoType);
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? null : entity.Name.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();
        }

        private static string NormalizeIoType(string ioType)
        {
            if (string.IsNullOrWhiteSpace(ioType))
            {
                return null;
            }

            var value = ioType.Trim().ToUpperInvariant();
            if (value == "DI" || value == "DO")
            {
                return value;
            }

            return null;
        }

        private Result Validate(MotionIoMapEntity entity)
        {
            if (entity.CardId < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "所属控制卡 CardId 无效");
            }

            if (entity.LogicalBit <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "逻辑位号必须大于 0");
            }

            if (string.IsNullOrWhiteSpace(entity.IoType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "IO 类型不能为空");
            }

            if (entity.IoType != "DI" && entity.IoType != "DO")
            {
                return Fail((int)DbErrorCode.InvalidArgument, "IO 类型仅支持 DI 或 DO");
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "IO 名称不能为空");
            }

            if (entity.Core <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "所属内核号必须大于 0");
            }

            if (entity.HardwareBit < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "硬件位号不能小于 0");
            }

            return Ok("IO映射校验通过");
        }
    }
}