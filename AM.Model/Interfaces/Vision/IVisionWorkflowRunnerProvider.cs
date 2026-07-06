using AM.Model.Common;
using System;
using System.Collections.Generic;

namespace AM.Model.Interfaces.Vision
{
    /// <summary>
    /// 视觉 Workflow 调用入口提供者。
    /// </summary>
    public interface IVisionWorkflowRunnerProvider : IDisposable
    {
        IEnumerable<string> RuntimeNames { get; }

        IEnumerable<string> TriggerSourceNames { get; }

        Result EnsureInitialized();
    }
}
