using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using AM.Model.Entity.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 使用信息上报服务。
    /// 
    /// 职责：
    /// 1. 将本地缓冲的使用事件批量上报到服务端；
    /// 2. 仅负责 HTTP 上报，不负责本地缓冲读取与状态更新；
    /// 3. 上报协议仅面向客户端最低限度运行信息。
    /// </summary>
    public class UsageReportService : ServiceBase
    {
        /// <summary>
        /// 后端统一服务地址。
        /// 当前仅用于判断是否允许执行使用事件上报。
        /// </summary>
        private readonly string _serviceUrl;

        /// <summary>
        /// 设备 report 客户端。
        /// 使用事件上报当前统一映射为设备 report 请求后发送。
        /// </summary>
        private readonly DeviceReportClient _deviceReportClient;

        protected override string MessageSourceName
        {
            get { return "UsageReportService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public UsageReportService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public UsageReportService(IAppReporter reporter)
            : base(reporter)
        {
            _serviceUrl = GetServiceUrlFromConfig();
            _deviceReportClient = new DeviceReportClient(reporter);
        }

        public UsageReportService(string serviceUrl, IAppReporter reporter)
            : base(reporter)
        {
            _serviceUrl = serviceUrl == null ? string.Empty : serviceUrl.Trim();
            _deviceReportClient = new DeviceReportClient(reporter);
        }

        /// <summary>
        /// 当前是否已完成服务地址配置。
        /// 只有配置了统一后端地址，使用事件上报链路才允许执行。
        /// </summary>
        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_serviceUrl);
        }

        /// <summary>
        /// 批量上报使用事件。
        /// 当前实现按“逐条转换为 DeviceReportRequest 并逐条调用 report 接口”的方式工作。
        /// </summary>
        public async Task<Result> UploadBatchAsync(IList<SysUsageEventBufferEntity> events)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_serviceUrl))
                {
                    return FailSilent(-1, "未配置后端服务地址");
                }

                List<SysUsageEventBufferEntity> list = events == null
                    ? new List<SysUsageEventBufferEntity>()
                    : events.Where(x => x != null).ToList();

                if (list.Count <= 0)
                {
                    return OkSilent("没有可上报的使用事件");
                }

                foreach (SysUsageEventBufferEntity item in list)
                {
                    Result<DeviceReportRequest> requestResult = BuildDeviceReportRequest(item);
                    if (!requestResult.Success || requestResult.Item == null)
                    {
                        return FailSilent(requestResult.Code == 0 ? -1 : requestResult.Code, requestResult.Message);
                    }

                    Result reportResult = await _deviceReportClient.ReportAsync(requestResult.Item).ConfigureAwait(false);
                    if (!reportResult.Success)
                    {
                        return FailSilent(reportResult.Code, reportResult.Message);
                    }
                }

                return OkSilent("使用事件上报成功，数量: " + list.Count);
            }
            catch (Exception ex)
            {
                return FailSilent(-1, BackendRequestFailureHelper.BuildExceptionMessage("使用事件上报", ex));
            }
        }

        /// <summary>
        /// 将本地使用事件映射为设备 report 请求。
        /// 该方法同时决定 reportType、payload 结构以及各字段值从本地缓冲实体的映射关系。
        /// </summary>
        private Result<DeviceReportRequest> BuildDeviceReportRequest(SysUsageEventBufferEntity entity)
        {
            if (entity == null)
            {
                return Fail<DeviceReportRequest>(-2, "使用事件不能为空");
            }

            string eventType = entity.EventType ?? string.Empty;
            var payload = new
            {
                source = "UsageEventBuffer",
                eventType = eventType,
                uploadCategory = "DesktopUsage",
                localOccurredTime = entity.OccurredTime,
                localBufferId = entity.Id
            };

            var request = new DeviceReportRequest
            {
                EventId = entity.EventId ?? string.Empty,
                ReportType = ResolveReportType(eventType),
                AppCode = entity.AppCode ?? string.Empty,
                AppVersion = entity.AppVersion ?? string.Empty,
                ClientId = entity.ClientId ?? string.Empty,
                MachineCode = entity.MachineCode ?? string.Empty,
                MachineName = entity.MachineName ?? string.Empty,
                UserId = entity.UserId,
                LoginName = entity.LoginName ?? string.Empty,
                PageKey = entity.PageKey ?? string.Empty,
                IsSuccess = entity.IsSuccess,
                FailReasonCode = entity.FailReasonCode ?? string.Empty,
                TraceId = entity.TraceId ?? string.Empty,
                OccurredAt = entity.OccurredTime == default(DateTime) ? DateTime.UtcNow : entity.OccurredTime,
                Payload = payload
            };

            return OkSilent(request, "生成使用事件设备上报请求成功");
        }

        /// <summary>
        /// 根据本地使用事件类型映射后端设备 reportType。
        /// 当前仅区分 Info 与 Status 两类。
        /// </summary>
        private static string ResolveReportType(string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventType))
            {
                return "Info";
            }

            switch (eventType.Trim())
            {
                case "LoginSuccess":
                case "LoginFailed":
                    return "Status";

                case "AppStart":
                case "AppExit":
                case "PageVisit":
                default:
                    return "Info";
            }
        }

        /// <summary>
        /// 从配置读取后端服务地址。
        /// </summary>
        private static string GetServiceUrlFromConfig()
        {
            return BackendServiceConfigHelper.GetBackendServiceUrl();
        }
    }
}