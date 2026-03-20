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
    /// 气缸对象配置 CRUD 服务。
    /// </summary>
    public class MotionCylinderConfigCrudService : ServiceBase, IMotionCylinderConfigCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionCylinderConfigCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionCylinderConfigCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionCylinderConfigCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<CylinderConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<CylinderConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.Name)
                    .ToList();

                return OkList(items, "气缸对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<CylinderConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "气缸对象配置查询失败");
            }
        }

        public Result<CylinderConfigEntity> QueryByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail<CylinderConfigEntity>((int)DbErrorCode.InvalidArgument, "气缸名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var item = db.Queryable<CylinderConfigEntity>()
                    .First(p => p.Name == normalizedName);

                if (item == null)
                {
                    return Warn<CylinderConfigEntity>((int)DbErrorCode.NotFound, "未找到对应气缸对象配置");
                }

                return Ok(item, "气缸对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<CylinderConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按名称查询气缸对象配置失败");
            }
        }

        public Result Save(CylinderConfigEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸对象配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                db = CreateDb();
                EnsureTables(db);

                var existing = db.Queryable<CylinderConfigEntity>()
                    .First(p => p.Name == entity.Name);

                if (existing != null && existing.Id != entity.Id)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸名称已存在: " + entity.Name);
                }

                var extendOutputExists = ExistsIo(db, entity.ExtendOutputBit, "DO");
                if (!extendOutputExists)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "伸出输出位不存在或不是 DO: " + entity.ExtendOutputBit);
                }

                if (entity.RetractOutputBit.HasValue && !ExistsIo(db, entity.RetractOutputBit.Value, "DO"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "缩回输出位不存在或不是 DO: " + entity.RetractOutputBit.Value);
                }

                if (entity.ExtendFeedbackBit.HasValue && !ExistsIo(db, entity.ExtendFeedbackBit.Value, "DI"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "伸出反馈位不存在或不是 DI: " + entity.ExtendFeedbackBit.Value);
                }

                if (entity.RetractFeedbackBit.HasValue && !ExistsIo(db, entity.RetractFeedbackBit.Value, "DI"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "缩回反馈位不存在或不是 DI: " + entity.RetractFeedbackBit.Value);
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

                return Ok("气缸对象配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "气缸对象配置保存失败");
            }
        }

        public Result DeleteByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var count = db.Deleteable<CylinderConfigEntity>()
                    .Where(p => p.Name == normalizedName)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的气缸对象配置");
                }

                return Ok("气缸对象配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "气缸对象配置删除失败");
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
                typeof(CylinderConfigEntity));
        }

        private static bool ExistsIo(SqlSugarClient db, short logicalBit, string ioType)
        {
            return db.Queryable<MotionIoMapEntity>()
                .Any(p => p.LogicalBit == logicalBit && p.IoType == ioType && p.IsEnabled);
        }

        private static void Normalize(CylinderConfigEntity entity)
        {
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? null : entity.Name.Trim();
            entity.DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? null : entity.DisplayName.Trim();
            entity.DriveMode = NormalizeDriveMode(entity.DriveMode);
            entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();

            if (entity.ExtendTimeoutMs < 0) entity.ExtendTimeoutMs = 0;
            if (entity.RetractTimeoutMs < 0) entity.RetractTimeoutMs = 0;
        }

        private static string NormalizeDriveMode(string driveMode)
        {
            if (string.IsNullOrWhiteSpace(driveMode))
            {
                return "Double";
            }

            var value = driveMode.Trim();
            if (string.Equals(value, "Single", StringComparison.OrdinalIgnoreCase))
            {
                return "Single";
            }

            if (string.Equals(value, "Double", StringComparison.OrdinalIgnoreCase))
            {
                return "Double";
            }

            return "Double";
        }

        private Result Validate(CylinderConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "气缸名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.DisplayName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "气缸显示名称不能为空");
            }

            if (entity.ExtendOutputBit <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "伸出输出位必须大于 0");
            }

            if (string.Equals(entity.DriveMode, "Double", StringComparison.OrdinalIgnoreCase)
                && !entity.RetractOutputBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "双线圈气缸必须配置缩回输出位");
            }

            if (entity.RetractOutputBit.HasValue && entity.RetractOutputBit.Value == entity.ExtendOutputBit)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "伸出输出位与缩回输出位不能相同");
            }

            if (entity.UseFeedbackCheck
                && !entity.ExtendFeedbackBit.HasValue
                && !entity.RetractFeedbackBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，至少应配置一个反馈位");
            }

            if (entity.UseFeedbackCheck && entity.ExtendTimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，伸出超时时间必须大于 0");
            }

            if (entity.UseFeedbackCheck && entity.RetractTimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，缩回超时时间必须大于 0");
            }

            if (entity.AllowBothOn && string.Equals(entity.DriveMode, "Double", StringComparison.OrdinalIgnoreCase))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "双线圈气缸通常不允许双输出同时导通");
            }

            return Ok("气缸对象配置校验通过");
        }
    }
}