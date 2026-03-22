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
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Motion.Actuator
{
    /// <summary>
    /// 灯塔/声光报警对象配置 CRUD 服务。
    /// 对应数据库表：motion_stacklight_config
    /// </summary>
    public class MotionStackLightConfigCrudService : ServiceBase, IMotionStackLightConfigCrudService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "MotionStackLightConfigCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionStackLightConfigCrudService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionStackLightConfigCrudService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// 查询全部灯塔对象配置。
        /// </summary>
        public Result<StackLightConfigEntity> QueryAll()
        {
            try
            {
                var db = CreateDb();
                EnsureTables(db);

                var items = db.Queryable<StackLightConfigEntity>()
                    .ToList()
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.Name)
                    .ToList();

                return OkList(items, "灯塔对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<StackLightConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "灯塔对象配置查询失败");
            }
        }

        /// <summary>
        /// 按名称查询灯塔对象配置。
        /// </summary>
        public Result<StackLightConfigEntity> QueryByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail<StackLightConfigEntity>((int)DbErrorCode.InvalidArgument, "灯塔对象名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var item = db.Queryable<StackLightConfigEntity>()
                    .First(p => p.Name == normalizedName);

                if (item == null)
                {
                    return Warn<StackLightConfigEntity>((int)DbErrorCode.NotFound, "未找到对应灯塔对象配置");
                }

                return Ok(item, "灯塔对象配置查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<StackLightConfigEntity>(ex, (int)DbErrorCode.QueryFailed, "按名称查询灯塔对象配置失败");
            }
        }

        /// <summary>
        /// 保存灯塔对象配置。
        /// </summary>
        public Result Save(StackLightConfigEntity entity)
        {
            SqlSugarClient db = null;

            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔对象配置不能为空");
                }

                Normalize(entity);

                var validateResult = Validate(entity);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                db = CreateDb();
                EnsureTables(db);

                var existing = db.Queryable<StackLightConfigEntity>()
                    .First(p => p.Name == entity.Name);

                if (existing != null && existing.Id != entity.Id)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔对象名称已存在: " + entity.Name);
                }

                var outputBits = GetConfiguredOutputBits(entity);
                foreach (var logicalBit in outputBits)
                {
                    if (!ExistsIo(db, logicalBit, "DO"))
                    {
                        return Fail((int)DbErrorCode.InvalidArgument, "灯塔输出位不存在或不是 DO: " + logicalBit);
                    }
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

                return Ok("灯塔对象配置保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "灯塔对象配置保存失败");
            }
        }

        /// <summary>
        /// 按名称删除灯塔对象配置。
        /// </summary>
        public Result DeleteByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔对象名称不能为空");
                }

                var db = CreateDb();
                EnsureTables(db);

                var normalizedName = name.Trim();
                var count = db.Deleteable<StackLightConfigEntity>()
                    .Where(p => p.Name == normalizedName)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的灯塔对象配置");
                }

                return Ok("灯塔对象配置删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "灯塔对象配置删除失败");
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
                typeof(StackLightConfigEntity));
        }

        private static bool ExistsIo(SqlSugarClient db, short logicalBit, string ioType)
        {
            return db.Queryable<MotionIoMapEntity>()
                .Any(p => p.LogicalBit == logicalBit && p.IoType == ioType && p.IsEnabled);
        }

        private static void Normalize(StackLightConfigEntity entity)
        {
            entity.Name = string.IsNullOrWhiteSpace(entity.Name) ? null : entity.Name.Trim();
            entity.DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? null : entity.DisplayName.Trim();
            entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description.Trim();
            entity.Remark = string.IsNullOrWhiteSpace(entity.Remark) ? null : entity.Remark.Trim();

            if (string.IsNullOrWhiteSpace(entity.DisplayName) && !string.IsNullOrWhiteSpace(entity.Name))
            {
                entity.DisplayName = entity.Name;
            }
        }

        private Result Validate(StackLightConfigEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "灯塔对象名称不能为空");
            }

            if (!entity.RedOutputBit.HasValue
                && !entity.YellowOutputBit.HasValue
                && !entity.GreenOutputBit.HasValue
                && !entity.BlueOutputBit.HasValue
                && !entity.BuzzerOutputBit.HasValue)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "灯塔对象至少应配置一个输出位");
            }

            var outputBits = GetConfiguredOutputBits(entity);
            var duplicateBit = outputBits
                .GroupBy(p => p)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicateBit != null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "灯塔输出位重复配置: " + duplicateBit.Key);
            }

            return OkSilent("灯塔对象配置校验通过");
        }

        private static List<short> GetConfiguredOutputBits(StackLightConfigEntity entity)
        {
            var result = new List<short>();

            if (entity.RedOutputBit.HasValue) result.Add(entity.RedOutputBit.Value);
            if (entity.YellowOutputBit.HasValue) result.Add(entity.YellowOutputBit.Value);
            if (entity.GreenOutputBit.HasValue) result.Add(entity.GreenOutputBit.Value);
            if (entity.BlueOutputBit.HasValue) result.Add(entity.BlueOutputBit.Value);
            if (entity.BuzzerOutputBit.HasValue) result.Add(entity.BuzzerOutputBit.Value);

            return result;
        }
    }
}