using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Entity.System;
using AM.Model.Interfaces.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 使用信息后台上传工作单元。
    /// 
    /// 职责：
    /// 1. 周期性读取本地待上传使用事件；
    /// 2. 批量调用 UsageReportService 上报；
    /// 3. 根据结果更新本地缓冲状态；
    /// 4. 上传失败不影响主程序运行。
    /// </summary>
    public class UsageUploadWorker : ServiceBase, IRuntimeWorker
    {
        private const int InitialUploadDelayMs = 5000;
        private const int DefaultUploadIntervalMs = 60000;
        private const int DefaultBatchSize = 100;
        private const int MinUploadIntervalMs = 5000;
        private const int BackgroundLogThrottleIntervalMs = 30000;

        private readonly object _stateSyncRoot;
        private readonly UsageEventBufferService _usageEventBufferService;
        private readonly UsageReportService _usageReportService;

        private CancellationTokenSource _cancellationTokenSource;
        private Task _workerTask;

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
                GetUploadIntervalFromConfig(),
                GetUploadBatchSizeFromConfig())
        {
        }

        public UsageUploadWorker(
            IAppReporter reporter,
            UsageEventBufferService usageEventBufferService,
            UsageReportService usageReportService,
            int uploadIntervalMs,
            int batchSize)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _usageEventBufferService = usageEventBufferService ?? new UsageEventBufferService(reporter);
            _usageReportService = usageReportService ?? new UsageReportService(reporter);
            UploadIntervalMs = uploadIntervalMs < MinUploadIntervalMs ? MinUploadIntervalMs : uploadIntervalMs;
            BatchSize = batchSize <= 0 ? DefaultBatchSize : batchSize;
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

                if (!IsUsageReportEnabled())
                {
                    return WarnLogOnly(-1, "当前未启用使用信息上报");
                }

                if (!_usageReportService.IsConfigured())
                {
                    return WarnLogOnly(-1, "未配置使用信息上报服务地址");
                }

                _cancellationTokenSource = new CancellationTokenSource();
                _workerTask = Task.Run(() => WorkerLoopAsync(_cancellationTokenSource.Token));
            }

            return OkLogOnly("使用信息上传工作单元启动成功");
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
                return OkLogOnly("使用信息上传工作单元未启动");
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
                return HandleException(ex, -1, "停止使用信息上传工作单元失败");
            }
            finally
            {
                cts.Dispose();
            }

            return OkLogOnly("使用信息上传工作单元已停止");
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
                        await ExecuteUploadOnceAsync().ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        LastError = ex.Message;
                        FailLogOnlyIfRepeated(
                            "UsageUploadWorker.ExecuteUploadOnceAsync",
                            -1,
                            "使用信息后台上传异常: " + ex.Message,
                            BackgroundLogThrottleIntervalMs,
                            ex);
                    }

                    await Task.Delay(UploadIntervalMs, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        /// <summary>
        /// 执行单轮上传。
        /// </summary>
        private async Task ExecuteUploadOnceAsync()
        {
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
                    BackgroundLogThrottleIntervalMs);

                return;
            }

            List<int> uploadedIds = list.Select(x => x.Id).ToList();
            _usageEventBufferService.MarkUploaded(uploadedIds);

            LastRunTime = DateTime.Now;
            LastError = string.Empty;
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
    }
}