using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion.Actuator;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.Actuator;
using SqlSugar;
using System;
using System.Linq;

namespace AM.DBService.Services.Motion.Actuator
{
    /// <summary>
    /// 真空对象配置 CRUD 服务。
    /// 对应数据库表：motion_vacuum_config
    /// </summary>
    public class MotionVacuumConfigCrudService : ServiceBase, IMotionVacuumConfigCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionVacuumConfigCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionVacuumConfigCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionVacuumConfigCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// 查询全部真空对象配置。
        /// </summary>
        public Result<VacuumConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<VacuumConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.Name)
                    .ToList();

                return OkListLogOnly(items, "真空对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<VacuumConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "真空对象配置查询失败");
            }
        }

        /// <summary>
        /// 按名称查询真空对象配置。
        /// </summary>
        public Result<VacuumConfigEntity> QueryByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail<VacuumConfigEntity>((int)DbErrorCode.InvalidArgument, "真空对象名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var item = db.Queryable<VacuumConfigEntity>()
                    .First(p => p.Name == normalizedName);

                if (item == null)
                {
                    return Warn<VacuumConfigEntity>((int)DbErrorCode.NotFound, "未找到对应真空对象配置");
                }

                return OkLogOnly(item, "真空对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<VacuumConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按名称查询真空对象配置失败");
            }
        }

        /// <summary>
        /// 保存真空对象配置。
        /// </summary>
        public Result Save(VacuumConfigEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空对象配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                db = CreateDb();
                EnsureTables(db);

                var existing = db.Queryable<VacuumConfigEntity>()
                    .First(p => p.Name == entity.Name);

                if (existing != null && existing.Id != entity.Id)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空对象名称已存在: " + entity.Name);
                }

                if (!ExistsIo(db, entity.VacuumOnOutputBit, "DO"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "吸真空输出位不存在或不是 DO: " + entity.VacuumOnOutputBit);
                }

                if (entity.BlowOffOutputBit.HasValue && !ExistsIo(db, entity.BlowOffOutputBit.Value, "DO"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "破真空输出位不存在或不是 DO: " + entity.BlowOffOutputBit.Value);
                }

                if (entity.VacuumFeedbackBit.HasValue && !ExistsIo(db, entity.VacuumFeedbackBit.Value, "DI"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空建立反馈位不存在或不是 DI: " + entity.VacuumFeedbackBit.Value);
                }

                if (entity.ReleaseFeedbackBit.HasValue && !ExistsIo(db, entity.ReleaseFeedbackBit.Value, "DI"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空释放反馈位不存在或不是 DI: " + entity.ReleaseFeedbackBit.Value);
                }

                if (entity.WorkpiecePresentBit.HasValue && !ExistsIo(db, entity.WorkpiecePresentBit.Value, "DI"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "工件检测位不存在或不是 DI: " + entity.WorkpiecePresentBit.Value);
                }

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

                return Ok("真空对象配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "真空对象配置保存失败");
            }
        }

        /// <summary>
        /// 按名称删除真空对象配置。
        /// </summary>
        public Result DeleteByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空对象名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var count = db.Deleteable<VacuumConfigEntity>()
                    .Where(p => p.Name == normalizedName)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的真空对象配置");
                }

                return Ok("真空对象配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "真空对象配置删除失败");
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
                typeof(VacuumConfigEntity));
        }

        private static bool ExistsIo(SqlSugarClient db, short logicalBit, string ioType)
        {
            return db.Queryable<MotionIoMapEntity>()
                .Any(p => p.LogicalBit == logicalBit && p.IoType == ioType && p.IsEnabled);
        }

        private static void Normalize(VacuumConfigEntity entity)
        {
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? null : entity.Name.Trim();
            entity.DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? null : entity.DisplayName.Trim();
            entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();

            if (entity.VacuumBuildTimeoutMs < 0) entity.VacuumBuildTimeoutMs = 0;
            if (entity.ReleaseTimeoutMs < 0) entity.ReleaseTimeoutMs = 0;
        }

        private Result Validate(VacuumConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "真空对象名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.DisplayName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "真空对象显示名称不能为空");
            }

            if (entity.VacuumOnOutputBit <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "吸真空输出位必须大于 0");
            }

            if (entity.BlowOffOutputBit.HasValue && entity.BlowOffOutputBit.Value == entity.VacuumOnOutputBit)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "吸真空输出位与破真空输出位不能相同");
            }

            if (entity.UseFeedbackCheck
                && !entity.VacuumFeedbackBit.HasValue
                && !entity.ReleaseFeedbackBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，至少应配置一个反馈位");
            }

            if (entity.UseWorkpieceCheck && !entity.WorkpiecePresentBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用工件检测时，必须配置工件检测位");
            }

            if (entity.UseFeedbackCheck && entity.VacuumBuildTimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，建立真空超时时间必须大于 0");
            }

            if (entity.UseFeedbackCheck && entity.ReleaseTimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，释放真空超时时间必须大于 0");
            }

            return OkSilent("真空对象配置校验通过");
        }
    }
}
