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
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 运动轴参数数据库服务。
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
                var items = db.Queryable<MotionAxisConfigEntity>()
                    .OrderBy(p => p.LogicalAxis)
                    .OrderBy(p => p.ParamName)
                    .ToList();

                return OkList(items, "运动轴参数查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<MotionAxisConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "运动轴参数查询失败");
            }
        }

        public Result<MotionAxisConfigEntity> QueryByLogicalAxis(int logicalAxis)
        {
            try
            {
                if (logicalAxis <= 0)
                {
                    return Fail<MotionAxisConfigEntity>((int)DbErrorCode.InvalidArgument, "逻辑轴参数无效");
                }

                var db = CreateDb();
                var items = db.Queryable<MotionAxisConfigEntity>()
                    .Where(p => p.LogicalAxis == logicalAxis)
                    .OrderBy(p => p.ParamName)
                    .ToList();

                if (items == null || items.Count == 0)
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
            try
            {
                if (entities == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "参数集合不能为空");
                }

                var list = entities
                    .Where(p => p != null && p.LogicalAxis > 0 && !string.IsNullOrWhiteSpace(p.ParamName))
                    .ToList();

                if (list.Count == 0)
                {
                    return Warn((int)DbErrorCode.InvalidArgument, "没有可保存的有效参数");
                }

                var db = CreateDb();
                db.Ado.BeginTran();

                var logicalAxes = list.Select(p => p.LogicalAxis).Distinct().ToList();
                var existingItems = db.Queryable<MotionAxisConfigEntity>()
                    .Where(p => logicalAxes.Contains(p.LogicalAxis))
                    .ToList();

                var existingMap = existingItems.ToDictionary(
                    p => BuildKey(p.LogicalAxis, p.ParamName),
                    p => p,
                    StringComparer.OrdinalIgnoreCase);

                foreach (var item in list)
                {
                    Normalize(item);

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
                try
                {
                    CreateDb().Ado.RollbackTran();
                }
                catch
                {
                }

                return HandleException(ex, (int)DbErrorCode.SaveFailed, "运动轴参数保存失败");
            }
        }

        public Result Delete(int logicalAxis, string paramName)
        {
            try
            {
                if (logicalAxis <= 0 || string.IsNullOrWhiteSpace(paramName))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "删除参数无效");
                }

                var db = CreateDb();
                var count = db.Deleteable<MotionAxisConfigEntity>()
                    .Where(p => p.LogicalAxis == logicalAxis && p.ParamName == paramName)
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

        private static string BuildKey(int logicalAxis, string paramName)
        {
            return logicalAxis.ToString() + "|" + (paramName ?? string.Empty).Trim();
        }

        private static void Normalize(MotionAxisConfigEntity entity)
        {
            if (entity.ParamValueType == null)
            {
                entity.ParamValueType = "Double";
            }

            entity.ParamName = (entity.ParamName ?? string.Empty).Trim();
        }
    }
}