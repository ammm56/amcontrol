using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Device;
using AM.Model.Entity.System;
using AM.Model.Interfaces.Runtime;
using AM.Model.License;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 统一后台上报工作单元。
    /// 
    /// 职责：
    /// 1. 周期性上报本地使用事件；
    /// 2. 静默维护设备注册、token 刷新与心跳；
    /// 3. 周期性发送设备结构化上报；
    /// 4. 所有失败仅记录后台日志，不影响主程序与其他后台任务。
    /// </summary>
    public class UsageUploadWorker : ServiceBase, IRuntimeWorker
    {
        private const int InitialUploadDelayMs = 5000;
        private const int LoopDelayMs = 1000;
        private const int DefaultUploadIntervalMs = 60000;
        private const int DefaultHeartbeatIntervalMs = 60000;
        private const int DefaultBatchSize = 100;
        private const int MinUploadIntervalMs = 5000;
        private const int MinHeartbeatIntervalMs = 5000;
        private const int DeviceReportBatchSize = 20;
        private const int BackgroundLogThrottleIntervalMs = 30000;
        private const int BackendFailureLogThrottleIntervalMs = 300000;

        private readonly object _stateSyncRoot;
        private readonly UsageEventBufferService _usageEventBufferService;
        private readonly UsageReportService _usageReportService;
        private readonly DeviceRegisterClient _deviceRegisterClient;
        private readonly DeviceHeartbeatClient _deviceHeartbeatClient;
        private readonly DeviceReportClient _deviceReportClient;
        private readonly DeviceReportBufferService _deviceReportBufferService;

        private CancellationTokenSource _cancellationTokenSource;
        private Task _workerTask;
        private bool _deviceSessionReady;
        private bool _startupReportQueued;
        private DateTime _nextUsageUploadTime;
        private DateTime _nextDeviceHeartbeatTime;

        protected override string MessageSourceName
        {
            get { return "UsageUploadWorker"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public UsageUploadWorker()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public UsageUploadWorker(IAppReporter reporter)
            : this(
                reporter,
                new UsageEventBufferService(reporter),
                new UsageReportService(reporter),
                new DeviceRegisterClient(reporter),
                new DeviceHeartbeatClient(reporter),
                new DeviceReportClient(reporter),
                new DeviceReportBufferService(reporter),
                GetUploadIntervalFromConfig(),
                GetUploadBatchSizeFromConfig(),
                GetHeartbeatIntervalFromConfig())
        {
        }

        public UsageUploadWorker(
            IAppReporter reporter,
            UsageEventBufferService usageEventBufferService,
            UsageReportService usageReportService,
            DeviceRegisterClient deviceRegisterClient,
            DeviceHeartbeatClient deviceHeartbeatClient,
            DeviceReportClient deviceReportClient,
            DeviceReportBufferService deviceReportBufferService,
            int uploadIntervalMs,
            int batchSize,
            int heartbeatIntervalMs)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _usageEventBufferService = usageEventBufferService ?? new UsageEventBufferService(reporter);
            _usageReportService = usageReportService ?? new UsageReportService(reporter);
            _deviceRegisterClient = deviceRegisterClient ?? new DeviceRegisterClient(reporter);
            _deviceHeartbeatClient = deviceHeartbeatClient ?? new DeviceHeartbeatClient(reporter);
            _deviceReportClient = deviceReportClient ?? new DeviceReportClient(reporter);
            _deviceReportBufferService = deviceReportBufferService ?? new DeviceReportBufferService(reporter);
            UploadIntervalMs = uploadIntervalMs < MinUploadIntervalMs ? MinUploadIntervalMs : uploadIntervalMs;
            BatchSize = batchSize <= 0 ? DefaultBatchSize : batchSize;
            HeartbeatIntervalMs = heartbeatIntervalMs < MinHeartbeatIntervalMs ? MinHeartbeatIntervalMs : heartbeatIntervalMs;
            LastError = string.Empty;
        }

        /// <summary>
        /// 工作单元名称。
        /// </summary>
        public string Name
        {
            get { return "UsageUploadWorker"; }
        }

        /// <summary>
        /// 是否运行中。
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_stateSyncRoot)
                {
                    return _cancellationTokenSource != null && _workerTask != null;
                }
            }
        }

        /// <summary>
        /// 上传周期，单位毫秒。
        /// </summary>
        public int UploadIntervalMs { get; private set; }

        /// <summary>
        /// 单批上报数量。
        /// </summary>
        public int BatchSize { get; private set; }

        /// <summary>
        /// 设备心跳周期，单位毫秒。
        /// </summary>
        public int HeartbeatIntervalMs { get; private set; }

        /// <summary>
        /// 最近一次成功运行时间。
        /// </summary>
        public DateTime? LastRunTime { get; private set; }

        /// <summary>
        /// 最近一次错误信息。
        /// </summary>
        public string LastError { get; private set; }

        /// <summary>
        /// 启动后台上传任务。
        /// </summary>
        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_cancellationTokenSource != null && _workerTask != null)
                {
                    return Warn(-1, "使用信息上传工作单元已在运行");
                }

                if (!BackendServiceConfigHelper.IsConfigured())
                {
                    return WarnLogOnly(-1, "未配置统一后端服务地址，后台上报工作单元跳过启动");
                }

                _deviceSessionReady = false;
                _startupReportQueued = false;
                _nextUsageUploadTime = DateTime.MinValue;
                _nextDeviceHeartbeatTime = DateTime.MinValue;
                _cancellationTokenSource = new CancellationTokenSource();
                _workerTask = Task.Run(() => WorkerLoopAsync(_cancellationTokenSource.Token));
            }

            return OkLogOnly("后台上报工作单元启动成功");
        }

        /// <summary>
        /// 停止后台上传任务。
        /// </summary>
        public async Task<Result> StopAsync()
        {
            CancellationTokenSource cts;
            Task workerTask;

            lock (_stateSyncRoot)
            {
                cts = _cancellationTokenSource;
                workerTask = _workerTask;
                _cancellationTokenSource = null;
                _workerTask = null;
            }

            if (cts == null || workerTask == null)
            {
                return OkLogOnly("后台上报工作单元未启动");
            }

            try
            {
                cts.Cancel();
                await workerTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return HandleException(ex, -1, "停止后台上报工作单元失败");
            }
            finally
            {
                cts.Dispose();
            }

            return OkLogOnly("后台上报工作单元已停止");
        }

        /// <summary>
        /// 后台循环。
        /// </summary>
        private async Task WorkerLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(InitialUploadDelayMs, cancellationToken).ConfigureAwait(false);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await ExecuteCycleAsync().ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        LastError = ex.Message;
                        FailLogOnlyIfRepeated(
                            "UsageUploadWorker.ExecuteCycleAsync",
                            -1,
                            "后台上报工作单元异常: " + ex.Message,
                            BackgroundLogThrottleIntervalMs,
                            ex);
                    }

                    await Task.Delay(LoopDelayMs, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        /// <summary>
        /// 执行单轮后台周期任务。
        /// </summary>
        private async Task ExecuteCycleAsync()
        {
            if (!_startupReportQueued)
            {
                _startupReportQueued = true;
                Result queueResult = _deviceReportBufferService.EnqueueAppStart();
                if (!queueResult.Success)
                {
                    WarnLogOnlyIfRepeated(
                        "UsageUploadWorker.EnqueueAppStart",
                        queueResult.Code,
                        "应用启动设备 report 入队失败: " + queueResult.Message,
                        BackgroundLogThrottleIntervalMs);
                }
            }

            DateTime now = DateTime.Now;

            if (now >= _nextUsageUploadTime)
            {
                await ExecuteUsageUploadOnceAsync().ConfigureAwait(false);
                _nextUsageUploadTime = now.AddMilliseconds(UploadIntervalMs);
            }

            if (now >= _nextDeviceHeartbeatTime)
            {
                await ExecuteDeviceRuntimeOnceAsync().ConfigureAwait(false);
                _nextDeviceHeartbeatTime = now.AddMilliseconds(HeartbeatIntervalMs);
            }
        }

        private async Task ExecuteUsageUploadOnceAsync()
        {
            if (!IsUsageReportEnabled() || !_usageReportService.IsConfigured())
            {
                return;
            }

            Result<SysUsageEventBufferEntity> queryResult = _usageEventBufferService.QueryPending(BatchSize);
            if (!queryResult.Success)
            {
                LastError = queryResult.Message;
                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.QueryPending",
                    queryResult.Code,
                    "查询待上传使用事件失败: " + queryResult.Message,
                    BackgroundLogThrottleIntervalMs);
                return;
            }

            List<SysUsageEventBufferEntity> list = queryResult.Items == null
                ? new List<SysUsageEventBufferEntity>()
                : queryResult.Items.ToList();

            if (list.Count <= 0)
            {
                LastRunTime = DateTime.Now;
                LastError = string.Empty;
                return;
            }

            Result uploadResult = await _usageReportService.UploadBatchAsync(list).ConfigureAwait(false);
            if (!uploadResult.Success)
            {
                List<int> failIds = list.Select(x => x.Id).ToList();
                _usageEventBufferService.MarkUploadFailed(failIds, uploadResult.Message);

                LastError = uploadResult.Message;

                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.UploadBatchAsync",
                    uploadResult.Code,
                    "使用事件上报失败: " + uploadResult.Message,
                    BackendFailureLogThrottleIntervalMs);

                return;
            }

            List<int> uploadedIds = list.Select(x => x.Id).ToList();
            _usageEventBufferService.MarkUploaded(uploadedIds);

            LastRunTime = DateTime.Now;
        }

        private async Task ExecuteDeviceRuntimeOnceAsync()
        {
            Result ensureResult = await EnsureDeviceSessionAsync().ConfigureAwait(false);
            if (!ensureResult.Success)
            {
                LastError = ensureResult.Message;
                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.EnsureDeviceSessionAsync",
                    ensureResult.Code,
                    "设备注册或 token 刷新失败: " + ensureResult.Message,
                    BackendFailureLogThrottleIntervalMs);
                return;
            }

            Result heartbeatResult = await SendHeartbeatAsync().ConfigureAwait(false);
            if (!heartbeatResult.Success)
            {
                LastError = heartbeatResult.Message;
                if (IsTokenRelatedFailure(heartbeatResult.Message))
                {
                    _deviceSessionReady = false;
                }

                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.SendHeartbeatAsync",
                    heartbeatResult.Code,
                    "设备心跳失败: " + heartbeatResult.Message,
                    BackendFailureLogThrottleIntervalMs);
            }

            Result flushResult = await FlushDeviceReportsAsync().ConfigureAwait(false);
            if (!flushResult.Success)
            {
                LastError = flushResult.Message;
                if (IsTokenRelatedFailure(flushResult.Message))
                {
                    _deviceSessionReady = false;
                }

                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.FlushDeviceReportsAsync",
                    flushResult.Code,
                    "设备 report 上传失败: " + flushResult.Message,
                    BackendFailureLogThrottleIntervalMs);
                return;
            }

            LastRunTime = DateTime.Now;
            if (heartbeatResult.Success)
            {
                LastError = string.Empty;
            }
        }

        private static bool IsUsageReportEnabled()
        {
            try
            {
                return ConfigContext.Instance.Config.Setting.EnableUsageReport;
            }
            catch
            {
                return false;
            }
        }

        private static int GetUploadIntervalFromConfig()
        {
            try
            {
                int value = ConfigContext.Instance.Config.Setting.UsageUploadIntervalMs;
                return value <= 0 ? DefaultUploadIntervalMs : value;
            }
            catch
            {
                return DefaultUploadIntervalMs;
            }
        }

        private static int GetUploadBatchSizeFromConfig()
        {
            try
            {
                int value = ConfigContext.Instance.Config.Setting.UsageUploadBatchSize;
                return value <= 0 ? DefaultBatchSize : value;
            }
            catch
            {
                return DefaultBatchSize;
            }
        }

        private static int GetHeartbeatIntervalFromConfig()
        {
            try
            {
                int value = ConfigContext.Instance.Config.Setting.DeviceHeartbeatIntervalMs;
                return value <= 0 ? DefaultHeartbeatIntervalMs : value;
            }
            catch
            {
                return DefaultHeartbeatIntervalMs;
            }
        }

        private async Task<Result> EnsureDeviceSessionAsync()
        {
            if (_deviceSessionReady)
            {
                return OkSilent("设备会话已就绪");
            }

            Setting setting = ConfigContext.Instance.Config.Setting;
            if (!string.IsNullOrWhiteSpace(setting.DeviceId) && !string.IsNullOrWhiteSpace(setting.DeviceToken))
            {
                Result<DeviceTokenRefreshResponse> refreshResult = await _deviceRegisterClient.RefreshTokenAsync().ConfigureAwait(false);
                if (refreshResult.Success)
                {
                    _deviceSessionReady = true;
                    return OkSilent("设备 token 刷新成功");
                }
            }

            Result<DeviceRegisterResponse> registerResult = await _deviceRegisterClient.RegisterCurrentDeviceAsync().ConfigureAwait(false);
            if (!registerResult.Success)
            {
                return FailSilent(registerResult.Code, registerResult.Message);
            }

            _deviceSessionReady = true;
            return OkSilent("设备注册成功");
        }

        private async Task<Result> SendHeartbeatAsync()
        {
            var request = new DeviceHeartbeatRequest
            {
                StatusJson = BuildHeartbeatStatusJson()
            };

            return await _deviceHeartbeatClient.SendHeartbeatAsync(request).ConfigureAwait(false);
        }

        private async Task<Result> FlushDeviceReportsAsync()
        {
            Result<DeviceReportRequest> batchResult = _deviceReportBufferService.DequeueBatch(DeviceReportBatchSize);
            if (!batchResult.Success)
            {
                return FailSilent(batchResult.Code, batchResult.Message);
            }

            List<DeviceReportRequest> list = batchResult.Items == null
                ? new List<DeviceReportRequest>()
                : batchResult.Items.Where(x => x != null).ToList();

            if (list.Count <= 0)
            {
                return OkSilent("当前无待上传设备 report");
            }

            for (int index = 0; index < list.Count; index++)
            {
                Result reportResult = await _deviceReportClient.ReportAsync(list[index]).ConfigureAwait(false);
                if (!reportResult.Success)
                {
                    _deviceReportBufferService.EnqueueMany(list.Skip(index).ToList());
                    return FailSilent(reportResult.Code, reportResult.Message);
                }
            }

            return OkSilent("设备 report 上传成功");
        }

        private static bool IsTokenRelatedFailure(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            string text = message.ToUpperInvariant();
            return text.Contains("DEVICE_TOKEN_") || text.Contains("DEVICE_ID_MISMATCH");
        }

        private static string BuildHeartbeatStatusJson()
        {
            var setting = ConfigContext.Instance.Config.Setting;
            var license = LicenseRuntimeContext.Instance.Current;
            var currentUser = UserContext.Instance.CurrentUser;

            var status = new
            {
                appCode = LicenseConstants.DesktopAppCode,
                appVersion = AM.Tools.Tools.GetAppVersionText(),
                clientId = setting.ClientId ?? string.Empty,
                deviceId = setting.DeviceId ?? string.Empty,
                machineCode = setting.MachineCode ?? string.Empty,
                machineName = string.IsNullOrWhiteSpace(setting.MachineName) ? Environment.MachineName : setting.MachineName,
                licenseValid = license != null && license.IsValid,
                licenseId = license == null ? string.Empty : license.LicenseId ?? string.Empty,
                loggedIn = currentUser != null,
                loginName = currentUser == null ? string.Empty : currentUser.LoginName ?? string.Empty,
                sampledAt = DateTime.UtcNow
            };

            return JsonConvert.SerializeObject(status);
        }
    }
}