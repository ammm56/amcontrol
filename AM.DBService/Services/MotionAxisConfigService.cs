using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 运动轴参数数据库服务。
    /// 负责 `motion_axis_config` 表的查询、保存、删除与基础边界校验。
    /// </summary>
    public class MotionAxisConfigService : ServiceBase, IMotionAxisConfigService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionAxisConfig"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionAxisConfigService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionAxisConfigService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result<MotionAxisConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTable(db);

                var items = db.Queryable<MotionAxisConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.LogicalAxis)
                    .ThenBy(p => p.ParamGroup)
                    .ThenBy(p => p.ParamName)
                    .ToList();

                return OkList(items, "运动轴参数查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "运动轴参数查询失败");
            }
        }

        public Result<MotionAxisConfigEntity> QueryByLogicalAxis(short logicalAxis)
        {
            try
            {
                if (logicalAxis <= 0)
                {
                    return Fail<MotionAxisConfigEntity>((int)DbErrorCode.InvalidArgument, "逻辑轴参数无效");
                }

                var db = CreateDb();
                EnsureTable(db);

                var items = db.Queryable<MotionAxisConfigEntity>()
                    .Where(p => p.LogicalAxis == logicalAxis)
                    .ToList()
                    .OrderBy(p => p.ParamGroup)
                    .ThenBy(p => p.ParamName)
                    .ToList();

                if (items.Count == 0)
                {
                    return Warn<MotionAxisConfigEntity>((int)DbErrorCode.NotFound, "未找到对应逻辑轴参数");
                }

                return OkList(items, "运动轴参数查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按逻辑轴查询参数失败");
            }
        }

        public Result Save(MotionAxisConfigEntity entity)
        {
            if (entity == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "参数不能为空");
            }

            return SaveRange(new[] { entity });
        }

        public Result SaveRange(IEnumerable<MotionAxisConfigEntity> entities)
        {
            SqlSugarClient db = null;

            try
            {
                if (entities == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "参数集合不能为空");
                }

                var list = entities
                    .Where(p => p != null)
                    .ToList();

                if (list.Count == 0)
                {
                    return Warn((int)DbErrorCode.InvalidArgument, "没有可保存的有效参数");
                }

                foreach (var item in list)
                {
                    Normalize(item);
                }

                db = CreateDb();
                EnsureTable(db);

                var logicalAxes = list
                    .Select(p => p.LogicalAxis)
                    .Distinct()
                    .ToList();

                var axisMap = db.Queryable<MotionAxisEntity>()
                    .Where(p => logicalAxes.Contains(p.LogicalAxis))
                    .ToList()
                    .ToDictionary(p => p.LogicalAxis, p => p);

                var duplicateInRequest = list
                    .GroupBy(p => BuildKey(p.LogicalAxis, p.ParamName), StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault(g => g.Count() > 1);
                if (duplicateInRequest != null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "参数集合存在重复项: " + duplicateInRequest.Key);
                }

                foreach (var item in list)
                {
                    var validateResult = Validate(item, axisMap);
                    if (!validateResult.Success)
                    {
                        return validateResult;
                    }
                }

                db.Ado.BeginTran();

                var existingItems = db.Queryable<MotionAxisConfigEntity>()
                    .Where(p => logicalAxes.Contains(p.LogicalAxis))
                    .ToList();

                var existingMap = existingItems.ToDictionary(
                    p => BuildKey(p.LogicalAxis, p.ParamName),
                    p => p,
                    StringComparer.OrdinalIgnoreCase);

                foreach (var item in list)
                {
                    MotionAxisConfigEntity existing;
                    if (existingMap.TryGetValue(BuildKey(item.LogicalAxis, item.ParamName), out existing))
                    {
                        item.Id = existing.Id;
                        db.Updateable(item).ExecuteCommand();
                    }
                    else
                    {
                        db.Insertable(item).ExecuteCommand();
                    }
                }

                db.Ado.CommitTran();
                return Ok("运动轴参数保存成功");
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

                return HandleException(ex, (int)DbErrorCode.SaveFailed, "运动轴参数保存失败");
            }
        }

        public Result Delete(short logicalAxis, string paramName)
        {
            try
            {
                if (logicalAxis <= 0 || string.IsNullOrWhiteSpace(paramName))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "删除参数无效");
                }

                var db = CreateDb();
                EnsureTable(db);

                var normalizedParamName = paramName.Trim();

                var count = db.Deleteable<MotionAxisConfigEntity>()
                    .Where(p => p.LogicalAxis == logicalAxis && p.ParamName == normalizedParamName)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "删除目标不存在");
                }

                return Ok("运动轴参数删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "运动轴参数删除失败");
            }
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void EnsureTable(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(typeof(MotionAxisConfigEntity));

            db.Ado.ExecuteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS ux_motion_axis_config_axis_param ON motion_axis_config(LogicalAxis, ParamName)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_motion_axis_config_axis_group ON motion_axis_config(LogicalAxis, ParamGroup)");
        }

        private static string BuildKey(short logicalAxis, string paramName)
        {
            return logicalAxis.ToString() + "|" + (paramName ?? string.Empty).Trim();
        }

        private Result Validate(
            MotionAxisConfigEntity entity,
            IDictionary<short, MotionAxisEntity> axisMap)
        {
            if (entity.LogicalAxis <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "逻辑轴号必须大于 0");
            }

            if (string.IsNullOrWhiteSpace(entity.ParamName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "参数名不能为空");
            }

            if (axisMap == null || !axisMap.ContainsKey(entity.LogicalAxis))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "参数所属逻辑轴不存在: " + entity.LogicalAxis);
            }

            if (!IsSupportedValueType(entity.ParamValueType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "不支持的参数值类型: " + entity.ParamValueType);
            }

            ApplyBuiltInRange(entity);

            if (entity.ParamMaxValue < entity.ParamMinValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "参数上下限无效: " + entity.ParamName);
            }

            if (string.Equals(entity.ParamValueType, "Bool", StringComparison.OrdinalIgnoreCase)
                && entity.ParamSetValue != 0D
                && entity.ParamSetValue != 1D)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "布尔参数只允许 0 或 1: " + entity.ParamName);
            }

            if ((string.Equals(entity.ParamValueType, "Int16", StringComparison.OrdinalIgnoreCase)
                || string.Equals(entity.ParamValueType, "Int32", StringComparison.OrdinalIgnoreCase)
                || string.Equals(entity.ParamValueType, "Enum", StringComparison.OrdinalIgnoreCase))
                && !IsIntegerValue(entity.ParamSetValue))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "整型/枚举参数必须为整数值: " + entity.ParamName);
            }

            if (entity.ParamSetValue < entity.ParamMinValue || entity.ParamSetValue > entity.ParamMaxValue)
            {
                return Fail(
                    (int)DbErrorCode.InvalidArgument,
                    "参数值超出允许范围: " + entity.ParamName + "，当前值=" + entity.ParamSetValue);
            }

            return OkSilent("轴参数校验通过");
        }

        private static void Normalize(MotionAxisConfigEntity entity)
        {
            entity.ParamName = (entity.ParamName ?? string.Empty).Trim();
            entity.ParamDisplayName = NormalizeNullableText(entity.ParamDisplayName);
            entity.ParamGroup = NormalizeNullableText(entity.ParamGroup);
            entity.ParamValueType = string.IsNullOrWhiteSpace(entity.ParamValueType)
                ? "Double"
                : entity.ParamValueType.Trim();
            entity.Unit = NormalizeNullableText(entity.Unit);
            entity.Description = NormalizeNullableText(entity.Description);
            entity.ValueDescription = NormalizeNullableText(entity.ValueDescription);
            entity.VendorScope = string.IsNullOrWhiteSpace(entity.VendorScope)
                ? "All"
                : entity.VendorScope.Trim();
            entity.Remark = NormalizeNullableText(entity.Remark);
            entity.AxisDisplayName = NormalizeNullableText(entity.AxisDisplayName);

            if (string.IsNullOrWhiteSpace(entity.ParamGroup))
            {
                entity.ParamGroup = ResolveDefaultGroup(entity.ParamName);
            }
        }

        private static string NormalizeNullableText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static bool IsSupportedValueType(string valueType)
        {
            return string.Equals(valueType, "Bool", StringComparison.OrdinalIgnoreCase)
                || string.Equals(valueType, "Int16", StringComparison.OrdinalIgnoreCase)
                || string.Equals(valueType, "Int32", StringComparison.OrdinalIgnoreCase)
                || string.Equals(valueType, "Double", StringComparison.OrdinalIgnoreCase)
                || string.Equals(valueType, "Enum", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsIntegerValue(double value)
        {
            return Math.Abs(value - Math.Round(value, MidpointRounding.AwayFromZero)) < 0.0000001D;
        }

        private static string ResolveDefaultGroup(string paramName)
        {
            switch (paramName)
            {
                case "AlarmEnabled":
                case "AlarmInvert":
                case "EnableInvert":
                case "PulseMode":
                case "EncoderExternal":
                case "EncoderInvert":
                case "LimitHomeInvert":
                case "LimitMode":
                case "TriggerEdge":
                    return "Hardware";

                case "Lead":
                case "PulsePerRev":
                case "GearRatio":
                    return "Scale";

                case "StandardHomeMode":
                case "ResetDirection":
                case "HomeSearchVelocity":
                case "IndexSearchVelocity":
                case "HomeOffset":
                case "HomeMaxDistance":
                case "IndexMaxDistance":
                case "EscapeStep":
                case "IndexSearchDirection":
                case "HomeCheck":
                case "HomeUseHomeSignal":
                case "HomeUseIndexSignal":
                case "HomeUseLimitSignal":
                case "HomeAutoZeroPos":
                case "HomeTimeoutMs":
                    return "Home";

                case "SoftLimitEnabled":
                case "SoftLimitPositive":
                case "SoftLimitNegative":
                    return "SoftLimit";

                case "EnableDelayMs":
                case "DisableDelayMs":
                    return "Timing";

                case "EStopId":
                case "StopId":
                    return "Safety";

                default:
                    return "Motion";
            }
        }

        private static void ApplyBuiltInRange(MotionAxisConfigEntity entity)
        {
            if (entity == null || string.IsNullOrWhiteSpace(entity.ParamName))
            {
                return;
            }

            if (!(entity.ParamMinValue == 0D && entity.ParamMaxValue == 0D))
            {
                return;
            }

            switch (entity.ParamName)
            {
                case "Lead":
                    entity.ParamMinValue = 0.001D;
                    entity.ParamMaxValue = 1000D;
                    break;
                case "PulsePerRev":
                    entity.ParamMinValue = 1D;
                    entity.ParamMaxValue = 10000000D;
                    break;
                case "GearRatio":
                    entity.ParamMinValue = 0.0001D;
                    entity.ParamMaxValue = 10000D;
                    break;
                case "DefaultVelocity":
                    entity.ParamMinValue = 0.001D;
                    entity.ParamMaxValue = 200D;
                    break;
                case "JogVelocity":
                    entity.ParamMinValue = 0.001D;
                    entity.ParamMaxValue = 100D;
                    break;
                case "Acc":
                case "Dec":
                    entity.ParamMinValue = 0.001D;
                    entity.ParamMaxValue = 1.0D;
                    break;
                case "SmoothTime":
                    entity.ParamMinValue = 0D;
                    entity.ParamMaxValue = 256D;
                    break;
                case "HomeTimeoutMs":
                    entity.ParamMinValue = 1000D;
                    entity.ParamMaxValue = 600000D;
                    break;
                case "EnableDelayMs":
                case "DisableDelayMs":
                    entity.ParamMinValue = 0D;
                    entity.ParamMaxValue = 10000D;
                    break;
                default:
                    break;
            }
        }
    }
}