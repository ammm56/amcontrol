using AM.Model.Common;
using AM.Model.Interfaces.Vision;
using Amvision.Workflows.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.VisionService.Runtime
{
    /// <summary>
    /// 长期持有仓库内 amvision SDK 的 WorkflowOperationRunner。
    /// </summary>
    public sealed class VisionWorkflowRunnerProvider : IVisionWorkflowRunnerProvider
    {
        private static readonly Lazy<VisionWorkflowRunnerProvider> SharedProvider =
            new Lazy<VisionWorkflowRunnerProvider>(() => new VisionWorkflowRunnerProvider());

        private readonly object _syncRoot = new object();
        private WorkflowOperationRunner _runner;
        private bool _disposed;

        public static VisionWorkflowRunnerProvider Shared
        {
            get { return SharedProvider.Value; }
        }

        public IEnumerable<string> RuntimeNames
        {
            get { return GetRunner().RuntimeNames.ToList(); }
        }

        public IEnumerable<string> TriggerSourceNames
        {
            get { return GetRunner().TriggerSourceNames.ToList(); }
        }

        public IEnumerable<string> ModelDeploymentNames
        {
            get { return GetRunner().ModelDeploymentNames.ToList(); }
        }

        public Result EnsureInitialized()
        {
            try
            {
                GetRunner();
                return Result.Ok("视觉 Workflow 调用入口初始化完成", ResultSource.Unknown);
            }
            catch (Exception ex)
            {
                return Result.Fail(-1, "视觉 Workflow 调用入口初始化失败: " + ex.Message, ResultSource.Unknown);
            }
        }

        internal WorkflowOperationRunner GetRunner()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (_runner != null)
            {
                return _runner;
            }

            lock (_syncRoot)
            {
                if (_runner == null)
                {
                    _runner = WorkflowOperationRunner.CreateDefault();
                }

                return _runner;
            }
        }

        public void Dispose()
        {
            lock (_syncRoot)
            {
                if (_disposed)
                {
                    return;
                }

                if (_runner != null)
                {
                    _runner.Dispose();
                    _runner = null;
                }

                _disposed = true;
            }
        }
    }
}
