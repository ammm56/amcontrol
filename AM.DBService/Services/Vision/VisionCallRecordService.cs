using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Vision;
using AM.Model.Interfaces.Vision;
using SqlSugar;
using System;

namespace AM.DBService.Services.Vision
{
    /// <summary>
    /// 视觉 SDK 调用记录服务。
    /// 对应数据库表：vision_call_record。
    /// </summary>
    public class VisionCallRecordService : ServiceBase, IVisionCallRecordService
    {
        private readonly DBContext _dbContext;

        protected override string MessageSourceName
        {
            get { return "VisionCallRecord"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public VisionCallRecordService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public VisionCallRecordService(IAppReporter reporter)
            : base(reporter)
        {
            _dbContext = new DBContext();
        }

        public Result Save(VisionCallRecordEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "视觉调用记录不能为空");
                }

                Normalize(entity);

                var db = CreateDb();
                EnsureTables(db);

                db.Insertable(entity).ExecuteCommand();
                return OkSilent("视觉调用记录保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "视觉调用记录保存失败");
            }
        }

        public Result<VisionCallRecordEntity> QueryPage(
            int pageIndex,
            int pageSize,
            string cameraCode = null,
            string callMode = null,
            bool? isSuccess = null,
            DateTime? startTime = null,
            DateTime? endTime = null)
        {
            try
            {
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }

                if (pageSize < 1)
                {
                    pageSize = 20;
                }

                var db = CreateDb();
                EnsureTables(db);

                var query = db.Queryable<VisionCallRecordEntity>();

                if (!string.IsNullOrWhiteSpace(cameraCode))
                {
                    var normalizedCode = cameraCode.Trim();
                    query = query.Where(x => x.CameraCode == normalizedCode);
                }

                if (!string.IsNullOrWhiteSpace(callMode))
                {
                    var normalizedMode = callMode.Trim();
                    query = query.Where(x => x.CallMode == normalizedMode);
                }

                if (isSuccess.HasValue)
                {
                    query = query.Where(x => x.IsSuccess == isSuccess.Value);
                }

                if (startTime.HasValue)
                {
                    query = query.Where(x => x.RequestTime >= startTime.Value);
                }

                if (endTime.HasValue)
                {
                    query = query.Where(x => x.RequestTime <= endTime.Value);
                }

                var items = query
                    .OrderBy(x => x.RequestTime, OrderByType.Desc)
                    .ToPageList(pageIndex, pageSize);

                return OkListSilent(items, "视觉调用记录查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<VisionCallRecordEntity>(ex, (int)DbErrorCode.QueryFailed, "视觉调用记录查询失败");
            }
        }

        internal static void EnsureTables(SqlSugarClient db)
        {
            db.CodeFirst.InitTables(typeof(VisionCallRecordEntity));

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_vision_call_record_request_time ON vision_call_record(RequestTime)");

            db.Ado.ExecuteCommand(
                "CREATE INDEX IF NOT EXISTS ix_vision_call_record_camera_code ON vision_call_record(CameraCode)");
        }

        private SqlSugarClient CreateDb()
        {
            return _dbContext.GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        private static void Normalize(VisionCallRecordEntity entity)
        {
            entity.CameraCode = NormalizeText(entity.CameraCode);
            entity.CallMode = NormalizeText(entity.CallMode);
            entity.RuntimeName = NormalizeText(entity.RuntimeName);
            entity.TriggerSourceName = NormalizeText(entity.TriggerSourceName);
            entity.ModelDeploymentName = NormalizeText(entity.ModelDeploymentName);
            entity.ImagePath = NormalizeText(entity.ImagePath);
            entity.MediaType = NormalizeText(entity.MediaType);
            entity.State = NormalizeText(entity.State);
            entity.WorkflowRunId = NormalizeText(entity.WorkflowRunId);
            entity.ResponseJson = NormalizeText(entity.ResponseJson);
            entity.ErrorMessage = NormalizeText(entity.ErrorMessage);
            entity.StationCode = NormalizeText(entity.StationCode);
            entity.ProductCode = NormalizeText(entity.ProductCode);

            if (string.IsNullOrWhiteSpace(entity.CallMode))
            {
                entity.CallMode = "ZeroMqTrigger";
            }

            if (entity.RequestTime == default(DateTime))
            {
                entity.RequestTime = DateTime.Now;
            }

            if (entity.CreateTime == default(DateTime))
            {
                entity.CreateTime = DateTime.Now;
            }
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
