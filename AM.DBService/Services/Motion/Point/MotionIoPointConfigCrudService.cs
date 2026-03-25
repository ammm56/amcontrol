using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Point;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.Point;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services.Motion.Point
{
    /// <summary>
    /// IO 点位公共配置 CRUD 服务。
    /// </summary>
    public class MotionIoPointConfigCrudService : ServiceBase, IMotionIoPointConfigCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionIoPointConfigCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionIoPointConfigCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionIoPointConfigCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<MotionIoPointConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<MotionIoPointConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.IoType)
                    .ThenBy(p => p.LogicalBit)
                    .ToList();

                return OkListLogOnly(items, "IO点位公共配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoPointConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "IO点位公共配置查询失败");
            }
        }

        public Result<MotionIoPointConfigEntity> QueryByLogicalBit(short logicalBit, string ioType)
        {
            try
            {
                if (logicalBit <= 0 || string.IsNullOrWhiteSpace(ioType))
                {
                    return Fail<MotionIoPointConfigEntity>((int)DbErrorCode.InvalidArgument, "逻辑位号或 IO 类型无效");
                }

                var normalizedIoType = NormalizeIoType(ioType);
                if (normalizedIoType == null)
                {
                    return Fail<MotionIoPointConfigEntity>((int)DbErrorCode.InvalidArgument, "IO 类型仅支持 DI 或 DO");
                }

                var db = CreateDb();
                EnsureTables(db);

                var item = db.Queryable<MotionIoPointConfigEntity>()
                    .First(p => p.LogicalBit == logicalBit && p.IoType == normalizedIoType);

                if (item == null)
                {
                    return Warn<MotionIoPointConfigEntity>((int)DbErrorCode.NotFound, "未找到对应 IO 点位公共配置");
                }

                return OkLogOnly(item, "IO点位公共配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionIoPointConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按逻辑位查询 IO 点位公共配置失败");
            }
        }

        public Result Save(MotionIoPointConfigEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "IO点位公共配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                db = CreateDb();
                EnsureTables(db);

                var ioMapExists = db.Queryable<MotionIoMapEntity>()
                    .Any(p => p.LogicalBit == entity.LogicalBit && p.IoType == entity.IoType);

                if (!ioMapExists)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "对应的 IO 映射不存在: " + entity.IoType + " " + entity.LogicalBit);
                }

                var existing = db.Queryable<MotionIoPointConfigEntity>()
                    .First(p => p.LogicalBit == entity.LogicalBit && p.IoType == entity.IoType);

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

                return Ok("IO点位公共配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "IO点位公共配置保存失败");
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

                var count = db.Deleteable<MotionIoPointConfigEntity>()
                    .Where(p => p.LogicalBit == logicalBit && p.IoType == normalizedIoType)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的 IO 点位公共配置");
                }

                return Ok("IO点位公共配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "IO点位公共配置删除失败");
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
                typeof(MotionIoPointConfigEntity));
        }

        private static void Normalize(MotionIoPointConfigEntity entity)
        {
            entity.IoType = NormalizeIoType(entity.IoType);
            entity.DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? null : entity.DisplayName.Trim();
            entity.SignalCategory = string.IsNullOrWhiteSpace(entity.SignalCategory) ? "Other" : entity.SignalCategory.Trim();
            entity.OutputMode = NormalizeOutputMode(entity.OutputMode);
            entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();

            if (entity.DebounceMs < 0) entity.DebounceMs = 0;
            if (entity.FilterMs < 0) entity.FilterMs = 0;
            if (entity.PulseWidthMs < 0) entity.PulseWidthMs = 0;
            if (entity.BlinkOnMs < 0) entity.BlinkOnMs = 0;
            if (entity.BlinkOffMs < 0) entity.BlinkOffMs = 0;
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

        private static string NormalizeOutputMode(string outputMode)
        {
            if (string.IsNullOrWhiteSpace(outputMode))
            {
                return "Keep";
            }

            var value = outputMode.Trim();
            if (string.Equals(value, "Keep", StringComparison.OrdinalIgnoreCase))
            {
                return "Keep";
            }

            if (string.Equals(value, "Pulse", StringComparison.OrdinalIgnoreCase))
            {
                return "Pulse";
            }

            if (string.Equals(value, "Blink", StringComparison.OrdinalIgnoreCase))
            {
                return "Blink";
            }

            return "Keep";
        }

        private Result Validate(MotionIoPointConfigEntity entity)
        {
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

            if (string.IsNullOrWhiteSpace(entity.DisplayName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "显示名称不能为空");
            }

            if (entity.IoType == "DI")
            {
                if (!string.Equals(entity.OutputMode, "Keep", StringComparison.OrdinalIgnoreCase))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "DI 点的 OutputMode 必须为 Keep");
                }

                if (entity.PulseWidthMs > 0 || entity.BlinkOnMs > 0 || entity.BlinkOffMs > 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "DI 点不能配置 Pulse/Blink 输出参数");
                }
            }

            if (entity.IoType == "DO")
            {
                if (string.Equals(entity.OutputMode, "Pulse", StringComparison.OrdinalIgnoreCase) && entity.PulseWidthMs <= 0)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "Pulse 模式必须配置大于 0 的 PulseWidthMs");
                }

                if (string.Equals(entity.OutputMode, "Blink", StringComparison.OrdinalIgnoreCase)
                    && (entity.BlinkOnMs <= 0 || entity.BlinkOffMs <= 0))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "Blink 模式必须配置大于 0 的 BlinkOnMs 和 BlinkOffMs");
                }
            }

            return OkSilent("IO点位公共配置校验通过");
        }
    }
}