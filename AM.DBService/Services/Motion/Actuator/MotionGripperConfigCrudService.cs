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
    /// 夹爪对象配置 CRUD 服务。
    /// 对应数据库表：motion_gripper_config
    /// </summary>
    public class MotionGripperConfigCrudService : ServiceBase, IMotionGripperConfigCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionGripperConfigCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionGripperConfigCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionGripperConfigCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<GripperConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<GripperConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.Name)
                    .ToList();

                return OkList(items, "夹爪对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<GripperConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "夹爪对象配置查询失败");
            }
        }

        public Result<GripperConfigEntity> QueryByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail<GripperConfigEntity>((int)DbErrorCode.InvalidArgument, "夹爪对象名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var item = db.Queryable<GripperConfigEntity>()
                    .First(p => p.Name == normalizedName);

                if (item == null)
                {
                    return Warn<GripperConfigEntity>((int)DbErrorCode.NotFound, "未找到对应夹爪对象配置");
                }

                return Ok(item, "夹爪对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<GripperConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按名称查询夹爪对象配置失败");
            }
        }

        public Result Save(GripperConfigEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "夹爪对象配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                db = CreateDb();
                EnsureTables(db);

                var existing = db.Queryable<GripperConfigEntity>()
                    .First(p => p.Name == entity.Name);

                if (existing != null && existing.Id != entity.Id)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "夹爪对象名称已存在: " + entity.Name);
                }

                if (!ExistsIo(db, entity.CloseOutputBit, "DO"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "夹紧输出位不存在或不是 DO: " + entity.CloseOutputBit);
                }

                if (entity.OpenOutputBit.HasValue && !ExistsIo(db, entity.OpenOutputBit.Value, "DO"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "打开输出位不存在或不是 DO: " + entity.OpenOutputBit.Value);
                }

                if (entity.CloseFeedbackBit.HasValue && !ExistsIo(db, entity.CloseFeedbackBit.Value, "DI"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "夹紧反馈位不存在或不是 DI: " + entity.CloseFeedbackBit.Value);
                }

                if (entity.OpenFeedbackBit.HasValue && !ExistsIo(db, entity.OpenFeedbackBit.Value, "DI"))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "打开反馈位不存在或不是 DI: " + entity.OpenFeedbackBit.Value);
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

                return Ok("夹爪对象配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "夹爪对象配置保存失败");
            }
        }

        public Result DeleteByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "夹爪对象名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var count = db.Deleteable<GripperConfigEntity>()
                    .Where(p => p.Name == normalizedName)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的夹爪对象配置");
                }

                return Ok("夹爪对象配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "夹爪对象配置删除失败");
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
                typeof(GripperConfigEntity));
        }

        private static bool ExistsIo(SqlSugarClient db, short logicalBit, string ioType)
        {
            return db.Queryable<MotionIoMapEntity>()
                .Any(p => p.LogicalBit == logicalBit && p.IoType == ioType && p.IsEnabled);
        }

        private static void Normalize(GripperConfigEntity entity)
        {
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? null : entity.Name.Trim();
            entity.DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? null : entity.DisplayName.Trim();
            entity.DriveMode = NormalizeDriveMode(entity.DriveMode);
            entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();

            if (entity.CloseTimeoutMs < 0) entity.CloseTimeoutMs = 0;
            if (entity.OpenTimeoutMs < 0) entity.OpenTimeoutMs = 0;
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

        private Result Validate(GripperConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "夹爪对象名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(entity.DisplayName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "夹爪对象显示名称不能为空");
            }

            if (entity.CloseOutputBit <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "夹紧输出位必须大于 0");
            }

            if (string.Equals(entity.DriveMode, "Double", StringComparison.OrdinalIgnoreCase)
                && !entity.OpenOutputBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "双线圈夹爪必须配置打开输出位");
            }

            if (entity.OpenOutputBit.HasValue && entity.OpenOutputBit.Value == entity.CloseOutputBit)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "夹紧输出位与打开输出位不能相同");
            }

            if (entity.UseFeedbackCheck
                && !entity.CloseFeedbackBit.HasValue
                && !entity.OpenFeedbackBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，至少应配置一个反馈位");
            }

            if (entity.UseWorkpieceCheck && !entity.WorkpiecePresentBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用工件检测时，必须配置工件检测位");
            }

            if (entity.UseFeedbackCheck && entity.CloseTimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，夹紧超时时间必须大于 0");
            }

            if (entity.UseFeedbackCheck && entity.OpenTimeoutMs <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "启用反馈校验时，打开超时时间必须大于 0");
            }

            if (entity.AllowBothOn && string.Equals(entity.DriveMode, "Double", StringComparison.OrdinalIgnoreCase))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "双线圈夹爪通常不允许双输出同时导通");
            }

            return OkSilent("夹爪对象配置校验通过");
        }
    }
}