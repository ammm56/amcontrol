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
        /// <summary>
        /// 工作单元启动后的首轮延迟。
        /// 用于避开程序刚启动时配置装载、设备连接和 UI 初始化的高峰期。
        /// </summary>
        private const int InitialUploadDelayMs = 5000;

        /// <summary>
        /// 后台循环基础轮询间隔。
        /// 每轮只负责判断是否到达上传/心跳时间点，不代表真实业务上报周期。
        /// </summary>
        private const int LoopDelayMs = 1000;

        /// <summary>
        /// 使用事件上传默认周期，单位毫秒。
        /// 当 config 未显式配置或配置非法时使用该值。
        /// </summary>
        private const int DefaultUploadIntervalMs = 60000;

        /// <summary>
        /// 设备心跳默认周期，单位毫秒。
        /// 当 config 未显式配置或配置非法时使用该值。
        /// </summary>
        private const int DefaultHeartbeatIntervalMs = 60000;

        /// <summary>
        /// 使用事件默认单批处理数量。
        /// 仅用于从本地缓冲读取待上传记录时的分页上限。
        /// </summary>
        private const int DefaultBatchSize = 100;

        /// <summary>
        /// 使用事件上传允许的最小周期，单位毫秒。
        /// 避免配置过小导致后台频繁触发上报。
        /// </summary>
        private const int MinUploadIntervalMs = 5000;

        /// <summary>
        /// 设备心跳允许的最小周期，单位毫秒。
        /// 避免配置过小导致后端和本地日志被心跳请求淹没。
        /// </summary>
        private const int MinHeartbeatIntervalMs = 5000;

        /// <summary>
        /// 单轮结构化设备 report 的最大 flush 数量。
        /// 防止大量 report 在一个周期内长时间占用后台线程。
        /// </summary>
        private const int DeviceReportBatchSize = 20;

        /// <summary>
        /// 本地后台异常日志节流间隔，单位毫秒。
        /// 用于控制循环内部重复异常的输出频率。
        /// </summary>
        private const int BackgroundLogThrottleIntervalMs = 30000;

        /// <summary>
        /// 与后端通信失败日志的节流间隔，单位毫秒。
        /// 设备注册、心跳、report 连续失败时按该周期限流输出。
        /// </summary>
        private const int BackendFailureLogThrottleIntervalMs = 300000;

        /// <summary>
        /// 后台工作单元状态锁。
        /// 用于保护启动、停止和运行状态字段的并发访问。
        /// </summary>
        private readonly object _stateSyncRoot;

        /// <summary>
        /// 本地使用事件缓冲服务。
        /// 负责从数据库读取待上传使用事件，并在成功或失败后回写状态。
        /// </summary>
        private readonly UsageEventBufferService _usageEventBufferService;

        /// <summary>
        /// 使用事件上报服务。
        /// 负责把缓冲表中的使用事件映射为设备 report 请求并逐条发送。
        /// </summary>
        private readonly UsageReportService _usageReportService;

        /// <summary>
        /// 设备注册客户端。
        /// 负责设备注册和 token 刷新。
        /// </summary>
        private readonly DeviceRegisterClient _deviceRegisterClient;

        /// <summary>
        /// 设备心跳客户端。
        /// 负责向后端发送当前设备在线状态和运行摘要。
        /// </summary>
        private readonly DeviceHeartbeatClient _deviceHeartbeatClient;

        /// <summary>
        /// 设备 report HTTP 客户端。
        /// 负责发送使用事件映射后的 report 和结构化 report。
        /// </summary>
        private readonly DeviceReportClient _deviceReportClient;

        /// <summary>
        /// 结构化设备 report 本地缓冲服务。
        /// 负责 AppStart、LicenseApplied 等事件的入队、出队和失败重入队。
        /// </summary>
        private readonly DeviceReportBufferService _deviceReportBufferService;

        /// <summary>
        /// 后台工作单元取消源。
        /// 在 StopAsync 时触发取消，通知循环尽快退出。
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 后台工作循环任务句柄。
        /// 用于判断运行状态以及在停止时等待任务收尾。
        /// </summary>
        private Task _workerTask;

        /// <summary>
        /// 当前进程内的设备会话就绪标记。
        /// 为 true 表示本轮无需重复注册或刷新 token；为 false 表示下轮需重新建立会话。
        /// </summary>
        private bool _deviceSessionReady;

        /// <summary>
        /// 应用启动 report 是否已入队。
        /// 用于确保 AppStart 事件只在本次进程生命周期内入队一次。
        /// </summary>
        private bool _startupReportQueued;

        /// <summary>
        /// 下次允许执行使用事件上传的时间点。
        /// </summary>
        private DateTime _nextUsageUploadTime;

        /// <summary>
        /// 下次允许执行设备心跳与结构化 report flush 的时间点。
        /// </summary>
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
        /// 用于 RuntimeTaskManager 注册、日志标识和运行态展示。
        /// </summary>
        public string Name
        {
            get { return "UsageUploadWorker"; }
        }

        /// <summary>
        /// 是否运行中。
        /// 仅表示后台循环任务已创建，不代表本轮一定已经完成设备注册或心跳发送。
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
        /// 使用事件上传周期，单位毫秒。
        /// 由构造函数从 config 读取并做最小值保护后确定。
        /// </summary>
        public int UploadIntervalMs { get; private set; }

        /// <summary>
        /// 使用事件单批处理数量。
        /// 仅用于读取本地缓冲表，不影响结构化 report 的 batch 数量。
        /// </summary>
        public int BatchSize { get; private set; }

        /// <summary>
        /// 设备心跳周期，单位毫秒。
        /// 设备 report flush 也复用该周期一起调度。
        /// </summary>
        public int HeartbeatIntervalMs { get; private set; }

        /// <summary>
        /// 最近一次成功执行后台动作的时间。
        /// 可能来自使用事件上传成功，也可能来自设备心跳/report 成功。
        /// </summary>
        public DateTime? LastRunTime { get; private set; }

        /// <summary>
        /// 最近一次后台失败信息。
        /// 主要用于运行时诊断，不代表当前所有链路都处于失败状态。
        /// </summary>
        public string LastError { get; private set; }

        /// <summary>
        /// 启动后台上传任务。
        /// 若未配置后端服务地址，则直接跳过启动并返回日志型警告。
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
        /// 会先取消后台循环，再等待工作任务自然退出，避免粗暴终止造成中间状态丢失。
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
        /// 负责统一调度使用事件上传、设备会话维护、心跳和 report flush。
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
        /// 本轮内按时间窗口依次判断是否需要执行 AppStart 入队、使用事件上传和设备运行态上报。
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

        /// <summary>
        /// 执行一次使用事件上传流程。
        /// 包括设备会话准备、查询待上传事件、调用 UsageReportService 发送以及回写上传结果。
        /// </summary>
        private async Task ExecuteUsageUploadOnceAsync()
        {
            if (!IsUsageReportEnabled() || !_usageReportService.IsConfigured())
            {
                return;
            }

            // UsageReportService / DeviceReportClient 返回的失败消息最终都通过 Result.Message 回到这里，
            // Worker 不再解析更细的 HTTP/异常上下文，只做节流日志和状态回写控制。
            Result ensureResult = await EnsureDeviceSessionAsync().ConfigureAwait(false);
            if (!ensureResult.Success)
            {
                LastError = ensureResult.Message;
                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.EnsureDeviceSessionBeforeUsageAsync",
                    ensureResult.Code,
                    "使用事件上报前设备注册或 token 刷新失败: " + ensureResult.Message,
                    BackendFailureLogThrottleIntervalMs);
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

        /// <summary>
        /// 执行一次设备运行态后台流程。
        /// 包括确保设备会话、发送心跳以及 flush 结构化 report。
        /// </summary>
        private async Task ExecuteDeviceRuntimeOnceAsync()
        {
            // 设备注册、心跳、report 三条链路的错误消息最终都在这里汇总，
            // 统一进入 LastError、节流日志，以及 token 失效后的 _deviceSessionReady 回滚逻辑。
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
                bool heartbeatNeedsSessionReset = IsTokenRelatedFailure(heartbeatResult.Message);
                if (heartbeatNeedsSessionReset)
                {
                    _deviceSessionReady = false;
                }

                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.SendHeartbeatAsync",
                    heartbeatResult.Code,
                    BuildRuntimeFailureLogMessage("设备心跳失败", heartbeatResult.Message, heartbeatNeedsSessionReset),
                    BackendFailureLogThrottleIntervalMs);
            }

            Result flushResult = await FlushDeviceReportsAsync().ConfigureAwait(false);
            if (!flushResult.Success)
            {
                LastError = flushResult.Message;
                bool reportNeedsSessionReset = IsTokenRelatedFailure(flushResult.Message);
                if (reportNeedsSessionReset)
                {
                    _deviceSessionReady = false;
                }

                WarnLogOnlyIfRepeated(
                    "UsageUploadWorker.FlushDeviceReportsAsync",
                    flushResult.Code,
                    BuildRuntimeFailureLogMessage("设备 report 上传失败", flushResult.Message, reportNeedsSessionReset),
                    BackendFailureLogThrottleIntervalMs);
                return;
            }

            LastRunTime = DateTime.Now;
            if (heartbeatResult.Success)
            {
                LastError = string.Empty;
            }
        }

        /// <summary>
        /// 判断当前是否启用使用事件上报。
        /// </summary>
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

        /// <summary>
        /// 从配置读取使用事件上传周期。
        /// </summary>
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

        /// <summary>
        /// 从配置读取使用事件单批数量。
        /// </summary>
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

        /// <summary>
        /// 从配置读取设备心跳周期。
        /// </summary>
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

        /// <summary>
        /// 确保当前进程具备可用设备会话。
        /// 优先尝试刷新既有 token，失败后再回退到重新注册。
        /// </summary>
        private async Task<Result> EnsureDeviceSessionAsync()
        {
            if (_deviceSessionReady)
            {
                return OkSilent("设备会话已就绪");
            }

            Setting setting = ConfigContext.Instance.Config.Setting;
            if (!string.IsNullOrWhiteSpace(setting.DeviceId) && !string.IsNullOrWhiteSpace(setting.DeviceToken))
            {
                // 配置来源线：这里直接消费当前内存中的 Setting.DeviceId / Setting.DeviceToken，
                // 这两个值由 DeviceRegisterClient 在注册或 refresh-token 成功后回写 config 并同步到运行期配置。
                // 错误传播线：若刷新失败，refreshResult.Message 会保留 BackendRequestFailureHelper 生成的一行失败描述，
                // 上层调用方据此决定是否继续回退到重新注册。
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

        /// <summary>
        /// 构造并发送一条设备心跳。
        /// 心跳体中的状态摘要由 BuildHeartbeatStatusJson 统一生成。
        /// </summary>
        private async Task<Result> SendHeartbeatAsync()
        {
            var request = new DeviceHeartbeatRequest
            {
                // 配置来源线：心跳状态体里的 appCode/deviceId 等字段统一在 BuildHeartbeatStatusJson 中从 ConfigContext 与 BackendServiceConfigHelper 汇总。
                StatusJson = BuildHeartbeatStatusJson()
            };

            return await _deviceHeartbeatClient.SendHeartbeatAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// flush 一批结构化设备 report。
        /// 采用逐条发送、失败后剩余项重新入队的策略。
        /// </summary>
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

        /// <summary>
        /// 判断失败消息是否属于 token 或设备身份相关错误。
        /// 命中后会把 _deviceSessionReady 置为 false，推动下一轮重新建立设备会话。
        ///
        /// 这里本质上连接了“错误传播”与“会话恢复”两条线：
        /// 1. DeviceHeartbeatClient / DeviceReportClient / DeviceRegisterClient 在 HTTP 失败时，会把后端 errorCode 拼进 Result.Message；
        /// 2. UsageUploadWorker 不再关心底层响应对象，只基于这条统一消息判断是否属于 token/设备身份问题；
        /// 3. 当前重点关注 `DEVICE_TOKEN_EXPIRED`、`DEVICE_TOKEN_REVOKED`、`DEVICE_TOKEN_INVALID` 三类明确令牌失效错误，以及其它 `DEVICE_TOKEN_*` 扩展错误；
        /// 4. 一旦命中，当前轮把 _deviceSessionReady 置为 false，下一轮重新走 EnsureDeviceSessionAsync 完成 refresh-token 或重新注册。
        /// </summary>
        private static bool IsTokenRelatedFailure(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            string text = message.ToUpperInvariant();
            return text.Contains("DEVICE_TOKEN_") || text.Contains("DEVICE_ID_MISMATCH");
        }

        /// <summary>
        /// 构造设备运行态失败日志消息。
        /// 当失败已明确属于 token 或设备身份失效时，额外补充“下轮重建设备会话”的语义，避免联调时只看到原始错误码却不知道本地恢复动作。
        /// </summary>
        private static string BuildRuntimeFailureLogMessage(string prefix, string message, bool needsSessionReset)
        {
            string baseMessage = string.Format("{0}: {1}", prefix ?? "后台设备链路失败", message ?? string.Empty);
            if (!needsSessionReset)
            {
                return baseMessage;
            }

            return baseMessage + "；检测到设备 token 或设备身份已失效，当前已标记下轮重建设备会话";
        }

        /// <summary>
        /// 生成设备心跳中的 statusJson。
        /// 当前汇总应用身份、设备标识、授权状态和当前登录用户摘要信息。
        /// 配置字段统一经由 BackendServiceConfigHelper 或 ConfigContext 提供，避免各调用点自行拼装来源。
        /// </summary>
        private static string BuildHeartbeatStatusJson()
        {
            var setting = ConfigContext.Instance.Config.Setting;
            var license = LicenseRuntimeContext.Instance.Current;
            var currentUser = UserContext.Instance.CurrentUser;

            var status = new
            {
                appCode = BackendServiceConfigHelper.GetDesktopAppCode(),
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