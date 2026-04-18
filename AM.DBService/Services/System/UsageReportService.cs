using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Entity.System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private readonly string _serviceUrl;
        private readonly HttpClient _httpClient;

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
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public UsageReportService(string serviceUrl, IAppReporter reporter)
            : base(reporter)
        {
            _serviceUrl = serviceUrl == null ? string.Empty : serviceUrl.Trim();
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        /// <summary>
        /// 当前是否已完成服务地址配置。
        /// </summary>
        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_serviceUrl);
        }

        /// <summary>
        /// 批量上报使用事件。
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

                SysUsageEventBufferEntity first = list[0];

                var payload = new
                {
                    AppCode = first.AppCode ?? string.Empty,
                    ClientId = first.ClientId ?? string.Empty,
                    MachineCode = first.MachineCode ?? string.Empty,
                    MachineName = first.MachineName ?? string.Empty,
                    Events = list.Select(x => new
                    {
                        x.EventId,
                        x.EventType,
                        x.AppCode,
                        x.AppVersion,
                        x.ClientId,
                        x.MachineCode,
                        x.MachineName,
                        x.UserId,
                        x.LoginName,
                        x.PageKey,
                        x.IsSuccess,
                        x.FailReasonCode,
                        x.TraceId,
                        x.OccurredTime
                    }).ToList()
                };

                string json = JsonConvert.SerializeObject(payload);
                string requestUrl = BuildBatchUploadUrl();

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUrl))
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            return FailSilent(
                                -1,
                                BackendRequestFailureHelper.BuildHttpFailureMessage(
                                    "使用事件上报",
                                    response.StatusCode,
                                    response.ReasonPhrase,
                                    null,
                                    null));
                        }
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
        /// 构建批量上报地址。
        /// </summary>
        private string BuildBatchUploadUrl()
        {
            return _serviceUrl.TrimEnd('/') + "/api/usage/events/batch";
        }

        /// <summary>
        /// 从配置读取服务地址。
        /// </summary>
        private static string GetServiceUrlFromConfig()
        {
            return BackendServiceConfigHelper.GetBackendServiceUrl();
        }
    }
}