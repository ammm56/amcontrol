using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Actuator;
using AM.Model.Entity.Motion.Point;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.Topology;
using SqlSugar;
using System;
using System.Collections.Generic;
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

                var db = CreateDb();
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
            SqlSugarClient db = null;

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

                db = CreateDb();
                EnsureTables(db);

                // 删除前检查第三层对象引用
                // 若被引用，拒绝删除并列出引用来源，防止误操作和悬空引用
                var refCheck = CheckActuatorReferences(db, logicalBit, normalizedIoType);
                if (!refCheck.Success)
                {
                    return refCheck;
                }

                db.Ado.BeginTran();

                var count = db.Deleteable<MotionIoMapEntity>()
                    .Where(p => p.LogicalBit == logicalBit && p.IoType == normalizedIoType)
                    .ExecuteCommand();

                if (count <= 0)
                {
                    db.Ado.RollbackTran();
                    return Warn((int)DbErrorCode.NotFound, "未找到要删除的 IO 映射");
                }

                // 级联删除对应的 IO 点位公共配置（避免留下孤立记录导致重载校验失败）
                db.Deleteable<MotionIoPointConfigEntity>()
                    .Where(p => p.LogicalBit == logicalBit && p.IoType == normalizedIoType)
                    .ExecuteCommand();

                db.Ado.CommitTran();

                return Ok("IO映射删除成功");
            }
            catch (Exception ex)
            {
                if (db != null)
                {
                    try { db.Ado.RollbackTran(); } catch { }
                }

                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "IO映射删除失败");
            }
        }

        /// <summary>
        /// 检查指定逻辑 IO 位是否被第三层对象（气缸/真空/灯塔/夹爪）引用。
        /// 有引用则返回 Fail 并列出全部引用来源，阻止删除操作。
        /// </summary>
        private Result CheckActuatorReferences(SqlSugarClient db, short logicalBit, string ioType)
        {
            var refs = new List<string>();

            if (ioType == "DO")
            {
                // 气缸：伸出输出位 / 缩回输出位
                db.Queryable<CylinderConfigEntity>()
                    .Where(p => p.ExtendOutputBit == logicalBit || p.RetractOutputBit == logicalBit)
                    .ToList()
                    .ForEach(p => refs.Add(string.Format("气缸[{0}]", p.DisplayName)));

                // 真空：吸真空输出位 / 破真空输出位
                db.Queryable<VacuumConfigEntity>()
                    .Where(p => p.VacuumOnOutputBit == logicalBit || p.BlowOffOutputBit == logicalBit)
                    .ToList()
                    .ForEach(p => refs.Add(string.Format("真空[{0}]", p.DisplayName)));

                // 灯塔：红/黄/绿/蓝/蜂鸣器输出位
                db.Queryable<StackLightConfigEntity>()
                    .Where(p => p.RedOutputBit == logicalBit
                             || p.YellowOutputBit == logicalBit
                             || p.GreenOutputBit == logicalBit
                             || p.BlueOutputBit == logicalBit
                             || p.BuzzerOutputBit == logicalBit)
                    .ToList()
                    .ForEach(p => refs.Add(string.Format("灯塔[{0}]", p.DisplayName)));

                // 夹爪：夹紧输出位 / 打开输出位
                db.Queryable<GripperConfigEntity>()
                    .Where(p => p.CloseOutputBit == logicalBit || p.OpenOutputBit == logicalBit)
                    .ToList()
                    .ForEach(p => refs.Add(string.Format("夹爪[{0}]", p.DisplayName)));
            }
            else // DI
            {
                // 气缸：伸出反馈位 / 缩回反馈位
                db.Queryable<CylinderConfigEntity>()
                    .Where(p => p.ExtendFeedbackBit == logicalBit || p.RetractFeedbackBit == logicalBit)
                    .ToList()
                    .ForEach(p => refs.Add(string.Format("气缸[{0}]", p.DisplayName)));

                // 真空：真空建立反馈 / 释放反馈 / 工件检测位
                db.Queryable<VacuumConfigEntity>()
                    .Where(p => p.VacuumFeedbackBit == logicalBit
                             || p.ReleaseFeedbackBit == logicalBit
                             || p.WorkpiecePresentBit == logicalBit)
                    .ToList()
                    .ForEach(p => refs.Add(string.Format("真空[{0}]", p.DisplayName)));

                // 夹爪：夹紧反馈 / 打开反馈 / 工件检测位
                db.Queryable<GripperConfigEntity>()
                    .Where(p => p.CloseFeedbackBit == logicalBit
                             || p.OpenFeedbackBit == logicalBit
                             || p.WorkpiecePresentBit == logicalBit)
                    .ToList()
                    .ForEach(p => refs.Add(string.Format("夹爪[{0}]", p.DisplayName)));
            }

            if (refs.Count == 0)
            {
                return OkSilent("未发现第三层对象引用");
            }

            return Fail(
                (int)DbErrorCode.InvalidArgument,
                string.Format(
                    "{0} {1} 已被以下对象引用，请先在对应配置中取消引用后再删除：{2}",
                    ioType, logicalBit, string.Join("、", refs)));
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void EnsureTables(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(
                typeof(MotionCardEntity),
                typeof(MotionIoMapEntity),
                typeof(MotionIoPointConfigEntity),
                typeof(CylinderConfigEntity),
                typeof(VacuumConfigEntity),
                typeof(StackLightConfigEntity),
                typeof(GripperConfigEntity));
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

            return OkSilent("IO映射校验通过");
        }
    }
}