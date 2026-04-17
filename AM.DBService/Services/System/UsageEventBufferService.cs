using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.Entity.System;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 使用信息本地缓冲服务。
    /// 
    /// 设计目标：
    /// 1. 将最低限度程序运行信息先写入本地数据库；
    /// 2. 不阻断主业务流程；
    /// 3. 为后续后台批量上传提供待发送数据源。
    /// </summary>
    public class UsageEventBufferService : ServiceBase
    {
        private const int MaxBufferCount = 100;

        private readonly DBCommon<SysUsageEventBufferEntity> _bufferDb;
        private readonly ClientIdentityService _clientIdentityService;

        protected override string MessageSourceName
        {
            get { return "UsageEventBufferService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public UsageEventBufferService()
            : base()
        {
            _bufferDb = new DBCommon<SysUsageEventBufferEntity>();
            _clientIdentityService = new ClientIdentityService();
            EnsureTableSchema();
        }

        public UsageEventBufferService(IAppReporter reporter)
            : base(reporter)
        {
            _bufferDb = new DBCommon<SysUsageEventBufferEntity>();
            _clientIdentityService = new ClientIdentityService(reporter);
            EnsureTableSchema();
        }

        /// <summary>
        /// 确保本地缓冲表存在。
        /// 初始化失败仅记录日志，不阻断主流程。
        /// </summary>
        private void EnsureTableSchema()
        {
            try
            {
                _bufferDb._sqlSugarClient.CodeFirst.InitTables<SysUsageEventBufferEntity>();
            }
            catch (Exception ex)
            {
                _reporter?.Error(MessageSourceName, ex, "初始化 sys_usage_event_buffer 表失败", -1, null, ReportChannels.Log);
            }
        }

        /// <summary>
        /// 记录应用启动事件。
        /// </summary>
        public Result SaveAppStart()
        {
            return SaveEvent("AppStart", null, null, null, null, null);
        }

        /// <summary>
        /// 记录应用退出事件。
        /// </summary>
        public Result SaveAppExit()
        {
            return SaveEvent("AppExit", null, null, null, null, null);
        }

        /// <summary>
        /// 记录登录成功事件。
        /// </summary>
        public Result SaveLoginSuccess(int? userId, string loginName)
        {
            return SaveEvent("LoginSuccess", userId, loginName, null, true, null);
        }

        /// <summary>
        /// 记录登录失败事件。
        /// </summary>
        public Result SaveLoginFailed(string loginName, string failReasonCode)
        {
            return SaveEvent("LoginFailed", null, loginName, failReasonCode, false, null);
        }

        /// <summary>
        /// 记录页面访问事件。
        /// 页面键应来自 NavigationCatalog 的 PageKey。
        /// </summary>
        public Result SavePageVisit(string pageKey)
        {
            if (string.IsNullOrWhiteSpace(pageKey))
            {
                return WarnSilent(-1, "页面键不能为空");
            }

            int? userId = null;
            string loginName = null;

            try
            {
                var user = UserContext.Instance.CurrentUser;
                if (user != null)
                {
                    userId = user.Id;
                    loginName = user.LoginName;
                }
            }
            catch
            {
            }

            return SaveEvent("PageVisit", userId, loginName, null, null, pageKey);
        }

        /// <summary>
        /// 查询待上传事件。
        /// </summary>
        public Result<SysUsageEventBufferEntity> QueryPending(int takeCount)
        {
            try
            {
                if (takeCount <= 0)
                {
                    takeCount = 100;
                }

                List<SysUsageEventBufferEntity> list = _bufferDb._sqlSugarClient
                    .Queryable<SysUsageEventBufferEntity>()
                    .Where(x => x.UploadStatus == "Pending" || x.UploadStatus == "Failed")
                    .OrderBy(x => x.OccurredTime, OrderByType.Asc)
                    .Take(takeCount)
                    .ToList();

                return OkListSilent(list, "查询待上传使用事件成功");
            }
            catch (Exception ex)
            {
                return Fail<SysUsageEventBufferEntity>(-1, "查询待上传使用事件失败", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 批量标记为已上传。
        /// </summary>
        public Result MarkUploaded(IEnumerable<int> ids)
        {
            return UpdateUploadStatus(ids, "Uploaded", "OK");
        }

        /// <summary>
        /// 批量标记为上传失败。
        /// </summary>
        public Result MarkUploadFailed(IEnumerable<int> ids, string message)
        {
            return UpdateUploadStatus(ids, "Failed", message);
        }

        /// <summary>
        /// 保存通用事件。
        /// </summary>
        private Result SaveEvent(
            string eventType,
            int? userId,
            string loginName,
            string failReasonCode,
            bool? isSuccess,
            string pageKey)
        {
            try
            {
                Result<SysClientIdentityEntity> identityResult = _clientIdentityService.GetCurrent();
                if (!identityResult.Success || identityResult.Item == null)
                {
                    return Fail(identityResult.Code == 0 ? -1 : identityResult.Code, "获取客户端身份失败");
                }

                SysClientIdentityEntity identity = identityResult.Item;

                Result capacityResult = EnsureBufferCapacity();
                if (!capacityResult.Success)
                {
                    return capacityResult;
                }

                SysUsageEventBufferEntity entity = new SysUsageEventBufferEntity
                {
                    EventId = AM.Tools.Tools.Guid(32),
                    EventType = eventType ?? string.Empty,
                    AppCode = identity.AppCode ?? string.Empty,
                    AppVersion = AM.Tools.Tools.GetAppVersionText(),
                    ClientId = identity.ClientId ?? string.Empty,
                    MachineCode = identity.MachineCode ?? string.Empty,
                    MachineName = identity.MachineName ?? string.Empty,
                    UserId = userId,
                    LoginName = loginName ?? string.Empty,
                    PageKey = pageKey ?? string.Empty,
                    IsSuccess = isSuccess,
                    FailReasonCode = failReasonCode ?? string.Empty,
                    TraceId = AM.Tools.Tools.Guid(16),
                    OccurredTime = DateTime.Now,
                    UploadStatus = "Pending",
                    UploadRetryCount = 0,
                    UploadTime = null,
                    UploadMessage = string.Empty,
                    CreateTime = DateTime.Now
                };

                Result result = _bufferDb.Add(entity);
                if (!result.Success)
                {
                    return WarnLogOnly(result.Code, "写入使用事件缓冲失败");
                }

                return OkSilent("写入使用事件缓冲成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "写入使用事件缓冲异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 确保缓冲表总量不超过上限。
        /// 达到上限时直接清空旧数据，再重新开始暂存。
        /// </summary>
        private Result EnsureBufferCapacity()
        {
            try
            {
                int totalCount = _bufferDb._sqlSugarClient
                    .Queryable<SysUsageEventBufferEntity>()
                    .Count();

                if (totalCount < MaxBufferCount)
                {
                    return OkSilent("使用事件缓冲容量检查成功");
                }

                _bufferDb._sqlSugarClient
                    .Deleteable<SysUsageEventBufferEntity>()
                    .ExecuteCommand();

                return OkLogOnly("使用事件缓冲已达到上限，已清空旧数据重新开始暂存");
            }
            catch (Exception ex)
            {
                return Fail(-1, "检查使用事件缓冲容量失败", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 更新上传状态。
        /// </summary>
        private Result UpdateUploadStatus(IEnumerable<int> ids, string status, string message)
        {
            try
            {
                List<int> idList = ids == null
                    ? new List<int>()
                    : ids.Where(x => x > 0).Distinct().ToList();

                if (idList.Count <= 0)
                {
                    return WarnSilent(-1, "没有可更新的使用事件记录");
                }

                DateTime now = DateTime.Now;

                _bufferDb._sqlSugarClient.Ado.BeginTran();

                _bufferDb._sqlSugarClient.Updateable<SysUsageEventBufferEntity>()
                    .Where(x => idList.Contains(x.Id))
                    .SetColumns(x => new SysUsageEventBufferEntity
                    {
                        UploadStatus = status,
                        UploadTime = now,
                        UploadMessage = message,
                        UploadRetryCount = x.UploadRetryCount + 1
                    })
                    .ExecuteCommand();

                _bufferDb._sqlSugarClient.Ado.CommitTran();
                return OkSilent("更新使用事件上传状态成功");
            }
            catch (Exception ex)
            {
                _bufferDb._sqlSugarClient.Ado.RollbackTran();
                return Fail(-1, "更新使用事件上传状态失败", ReportChannels.Log, ex);
            }
        }
    }
}