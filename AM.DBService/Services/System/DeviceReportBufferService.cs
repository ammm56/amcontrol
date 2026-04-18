using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using AM.Model.Entity.Auth;
using AM.Model.Entity.System;
using AM.Model.License;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 设备结构化上报本地缓冲服务。
    /// 当前阶段仅做进程内轻量缓冲，不落数据库。
    /// </summary>
    public class DeviceReportBufferService : ServiceBase
    {
        private static readonly object QueueSyncRoot = new object();
        private static readonly Queue<DeviceReportRequest> PendingQueue = new Queue<DeviceReportRequest>();

        private readonly ClientIdentityService _clientIdentityService;

        protected override string MessageSourceName
        {
            get { return "DeviceReportBufferService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.System; }
        }

        public DeviceReportBufferService()
            : this(new ClientIdentityService(), SystemContext.Instance.Reporter)
        {
        }

        public DeviceReportBufferService(IAppReporter reporter)
            : this(new ClientIdentityService(reporter), reporter)
        {
        }

        public DeviceReportBufferService(ClientIdentityService clientIdentityService, IAppReporter reporter)
            : base(reporter)
        {
            _clientIdentityService = clientIdentityService;
        }

        /// <summary>
        /// 写入通用上报请求。
        /// </summary>
        public Result Enqueue(DeviceReportRequest request)
        {
            if (request == null)
            {
                return Fail(-1, "设备上报请求不能为空");
            }

            if (string.IsNullOrWhiteSpace(request.ReportType))
            {
                return Fail(-2, "设备上报请求缺少 reportType");
            }

            if (request.Payload == null)
            {
                return Fail(-3, "设备上报请求缺少 payload");
            }

            request.EventId = string.IsNullOrWhiteSpace(request.EventId) ? BuildEventId() : request.EventId.Trim();
            request.TraceId = string.IsNullOrWhiteSpace(request.TraceId) ? AM.Tools.Tools.Guid(16) : request.TraceId.Trim();
            if (request.OccurredAt == default(DateTime))
            {
                request.OccurredAt = DateTime.UtcNow;
            }

            lock (QueueSyncRoot)
            {
                PendingQueue.Enqueue(request);
            }

            return OkSilent("设备上报请求入队成功");
        }

        /// <summary>
        /// 批量重新入队。
        /// 用于上传失败后的重试。
        /// </summary>
        public Result EnqueueMany(IEnumerable<DeviceReportRequest> requests)
        {
            if (requests == null)
            {
                return OkSilent("无设备上报请求需要重新入队");
            }

            List<DeviceReportRequest> list = requests.Where(x => x != null).ToList();
            if (list.Count <= 0)
            {
                return OkSilent("无设备上报请求需要重新入队");
            }

            lock (QueueSyncRoot)
            {
                foreach (DeviceReportRequest request in list)
                {
                    PendingQueue.Enqueue(request);
                }
            }

            return OkSilent("设备上报请求批量入队成功");
        }

        /// <summary>
        /// 取出一批待上报请求。
        /// </summary>
        public Result<DeviceReportRequest> DequeueBatch(int takeCount)
        {
            if (takeCount <= 0)
            {
                takeCount = 20;
            }

            List<DeviceReportRequest> list = new List<DeviceReportRequest>();

            lock (QueueSyncRoot)
            {
                while (PendingQueue.Count > 0 && list.Count < takeCount)
                {
                    list.Add(PendingQueue.Dequeue());
                }
            }

            return OkListSilent(list, "获取待上报设备请求成功");
        }

        /// <summary>
        /// 写入应用启动上报请求。
        /// </summary>
        public Result EnqueueAppStart()
        {
            DeviceLicenseState state = LicenseRuntimeContext.Instance.Current;
            Result<DeviceReportRequest> buildResult = BuildRequest(
                ResolveSystemEventReportType("AppStart"),
                new
                {
                    eventName = "AppStart",
                    licenseValid = state != null && state.IsValid,
                    licenseId = state == null ? string.Empty : state.LicenseId ?? string.Empty,
                    licenseMessage = state == null ? string.Empty : state.Message ?? string.Empty,
                    occurredAt = DateTime.UtcNow
                },
                null,
                true);

            if (!buildResult.Success || buildResult.Item == null)
            {
                return Fail(buildResult.Code == 0 ? -1 : buildResult.Code, buildResult.Message);
            }

            return Enqueue(buildResult.Item);
        }

        /// <summary>
        /// 写入授权申请成功后的上报请求。
        /// </summary>
        public Result EnqueueLicenseApplied(LicenseApplyResponse response)
        {
            if (response == null)
            {
                return Fail(-1, "授权申请结果不能为空");
            }

            Result<DeviceReportRequest> buildResult = BuildRequest(
                ResolveSystemEventReportType("LicenseApplied"),
                new
                {
                    eventName = "LicenseApplied",
                    licenseId = response.LicenseId ?? string.Empty,
                    status = response.Status ?? string.Empty,
                    issuedAt = response.IssuedAt,
                    expiresAt = response.ExpiresAt,
                    hasLicenseText = !string.IsNullOrWhiteSpace(response.LicenseText)
                },
                null,
                true);

            if (!buildResult.Success || buildResult.Item == null)
            {
                return Fail(buildResult.Code == 0 ? -1 : buildResult.Code, buildResult.Message);
            }

            return Enqueue(buildResult.Item);
        }

        private Result<DeviceReportRequest> BuildRequest(string reportType, object payload, string pageKey, bool? isSuccess)
        {
            try
            {
                Result<SysClientIdentityEntity> identityResult = _clientIdentityService.GetCurrent();
                if (!identityResult.Success || identityResult.Item == null)
                {
                    return Fail<DeviceReportRequest>(identityResult.Code == 0 ? -1 : identityResult.Code, "获取客户端身份失败，无法生成设备上报请求");
                }

                SysClientIdentityEntity identity = identityResult.Item;
                SysUserEntity currentUser = null;

                try
                {
                    currentUser = UserContext.Instance.CurrentUser;
                }
                catch
                {
                }

                var request = new DeviceReportRequest
                {
                    EventId = BuildEventId(),
                    ReportType = reportType ?? string.Empty,
                    AppCode = string.IsNullOrWhiteSpace(identity.AppCode) ? BackendServiceConfigHelper.GetDesktopAppCode() : identity.AppCode,
                    AppVersion = AM.Tools.Tools.GetAppVersionText(),
                    ClientId = identity.ClientId ?? string.Empty,
                    MachineCode = identity.MachineCode ?? string.Empty,
                    MachineName = identity.MachineName ?? string.Empty,
                    UserId = currentUser == null ? null : (int?)currentUser.Id,
                    LoginName = currentUser == null ? string.Empty : currentUser.LoginName ?? string.Empty,
                    PageKey = pageKey ?? string.Empty,
                    IsSuccess = isSuccess,
                    TraceId = AM.Tools.Tools.Guid(16),
                    OccurredAt = DateTime.UtcNow,
                    Payload = payload
                };

                return OkSilent(request, "生成设备上报请求成功");
            }
            catch (Exception ex)
            {
                return Fail<DeviceReportRequest>(-1, "生成设备上报请求异常", ReportChannels.Log, ex);
            }
        }

        private static string ResolveSystemEventReportType(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return "Info";
            }

            switch (eventName.Trim())
            {
                case "LicenseApplied":
                    return "Status";

                case "AppStart":
                default:
                    return "Info";
            }
        }

        private static string BuildEventId()
        {
            return string.Format("evt-{0}-{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), AM.Tools.Tools.Guid(8).ToLowerInvariant());
        }
    }
}