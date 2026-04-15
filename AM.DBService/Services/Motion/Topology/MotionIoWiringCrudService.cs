using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB.Motion.Topology;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services.Motion.Topology
{
    /// <summary>
    /// IO 接线信息 CRUD 服务。
    /// </summary>
    public class MotionIoWiringCrudService : ServiceBase, IMotionIoWiringCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionIoWiringCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionIoWiringCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionIoWiringCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// 查询全部 IO 接线信息。
        /// </summary>
        public Result<MotionIoWiringEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<MotionIoWiringEntity>()
                    .ToList()
                    .OrderBy(p => p.CardId)
                    .ThenBy(p => p.IoType)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();

                return OkListLogOnly(items, "IO接线信息查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoWiringEntity>(ex, (int)DbErrorCode.QueryFailed, "IO接线信息查询失败");
            }
        }

        /// <summary>
        /// 按控制卡查询 IO 接线信息。
        /// </summary>
        public Result<MotionIoWiringEntity> QueryByCardId(short cardId)
        {
            try
            {
                if (cardId < 0)
                {
                    return Fail<MotionIoWiringEntity>((int)DbErrorCode.InvalidArgument, "控制卡 CardId 无效");
                }

                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<MotionIoWiringEntity>()
                    .Where(p => p.CardId == cardId)
                    .ToList()
                    .OrderBy(p => p.IoType)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();

                return OkListLogOnly(items, "按控制卡查询 IO 接线信息成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoWiringEntity>(ex, (int)DbErrorCode.QueryFailed, "按控制卡查询 IO 接线信息失败");
            }
        }

        /// <summary>
        /// 按 IO 映射主键查询接线信息。
        /// </summary>
        public Result<MotionIoWiringEntity> QueryByIoMapId(int ioMapId)
        {
            try
            {
                if (ioMapId <= 0)
                {
                    return Fail<MotionIoWiringEntity>((int)DbErrorCode.InvalidArgument, "IO 映射主键无效");
                }

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<MotionIoWiringEntity>()
                    .First(p => p.IoMapId == ioMapId);

                if (item == null)
                {
                    return WarnSilent<MotionIoWiringEntity>((int)DbErrorCode.NotFound, "未找到对应 IO 接线信息");
                }

                return OkLogOnly(item, "IO接线信息查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoWiringEntity>(ex, (int)DbErrorCode.QueryFailed, "按 IO 映射主键查询接线信息失败");
            }
        }

        /// <summary>
        /// 按逻辑位与 IO 类型查询接线信息。
        /// </summary>
        public Result<MotionIoWiringEntity> QueryByLogicalBit(short logicalBit, string ioType)
        {
            try
            {
                if (logicalBit <= 0 || string.IsNullOrWhiteSpace(ioType))
                {
                    return Fail<MotionIoWiringEntity>((int)DbErrorCode.InvalidArgument, "逻辑位号或 IO 类型无效");
                }

                var normalizedIoType = NormalizeIoType(ioType);
                if (normalizedIoType == null)
                {
                    return Fail<MotionIoWiringEntity>((int)DbErrorCode.InvalidArgument, "IO 类型仅支持 DI 或 DO");
                }

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<MotionIoWiringEntity>()
                    .First(p => p.LogicalBit == logicalBit && p.IoType == normalizedIoType);

                if (item == null)
                {
                    return WarnSilent<MotionIoWiringEntity>((int)DbErrorCode.NotFound, "未找到对应 IO 接线信息");
                }

                return OkLogOnly(item, "IO接线信息查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoWiringEntity>(ex, (int)DbErrorCode.QueryFailed, "按逻辑位查询 IO 接线信息失败");
            }
        }

        /// <summary>
        /// 保存 IO 接线信息。
        /// </summary>
        public Result Save(MotionIoWiringEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "IO接线信息不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                var db = CreateDb();
                EnsureTables(db);

                var ioMapExists = db.Queryable<MotionIoMapEntity>()
                    .Any(p => p.Id == entity.IoMapId);
                if (!ioMapExists)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "对应的 IO 映射不存在: " + entity.IoMapId);
                }

                var existing = db.Queryable<MotionIoWiringEntity>()
                    .First(p => p.IoMapId == entity.IoMapId);

                if (entity.Id > 0)
                {
                    db.Updateable(entity).ExecuteCommand();
                }
                else if (existing != null)
                {
                    entity.Id = existing.Id;
                    db.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    db.Insertable(entity).ExecuteCommand();
                }

                return Ok("IO接线信息保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "IO接线信息保存失败");
            }
        }

        /// <summary>
        /// 按 IO 映射主键删除接线信息。
        /// </summary>
        public Result DeleteByIoMapId(int ioMapId)
        {
            try
            {
                if (ioMapId <= 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "IO 映射主键无效");
                }

                var db = CreateDb();
                EnsureTables(db);

                var count = db.Deleteable<MotionIoWiringEntity>()
                    .Where(p => p.IoMapId == ioMapId)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的 IO 接线信息");
                }

                return Ok("IO接线信息删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "IO接线信息删除失败");
            }
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void EnsureTables(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(
                typeof(MotionIoMapEntity),
                typeof(MotionIoWiringEntity));
        }

        private static void Normalize(MotionIoWiringEntity entity)
        {
            entity.IoType = NormalizeIoType(entity.IoType);
            entity.TerminalBlock = TrimOrNull(entity.TerminalBlock);
            entity.TerminalNo = TrimOrNull(entity.TerminalNo);
            entity.ConnectorNo = TrimOrNull(entity.ConnectorNo);
            entity.PinNo = TrimOrNull(entity.PinNo);
            entity.WireNo = TrimOrNull(entity.WireNo);
            entity.DeviceName = TrimOrNull(entity.DeviceName);
            entity.DeviceModel = TrimOrNull(entity.DeviceModel);
            entity.DeviceTerminal = TrimOrNull(entity.DeviceTerminal);
            entity.CabinetArea = TrimOrNull(entity.CabinetArea);
            entity.SignalType = TrimOrNull(entity.SignalType);
            entity.ExpectedNormalState = TrimOrNull(entity.ExpectedNormalState);
            entity.CheckMethod = TrimOrNull(entity.CheckMethod);
            entity.VerifiedBy = TrimOrNull(entity.VerifiedBy);
            entity.Remark = TrimOrNull(entity.Remark);
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

        private static string TrimOrNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private Result Validate(MotionIoWiringEntity entity)
        {
            if (entity.IoMapId <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "IO 映射主键必须大于 0");
            }

            if (entity.CardId < 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "控制卡 CardId 无效");
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

            return OkSilent("IO接线信息校验通过");
        }
    }
}
