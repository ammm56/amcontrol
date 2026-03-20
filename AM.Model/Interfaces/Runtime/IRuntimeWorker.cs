using AM.Model.Common;
using System;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Runtime
{
    /// <summary>
    /// 统一后台运行工作单元接口。
    /// 所有需要后台线程/循环执行的任务统一实现该接口。
    /// </summary>
    public interface IRuntimeWorker
    {
        /// <summary>
        /// 工作单元名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否运行中。
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 最近一次成功运行时间。
        /// </summary>
        DateTime? LastRunTime { get; }

        /// <summary>
        /// 最近一次错误信息。
        /// </summary>
        string LastError { get; }

        /// <summary>
        /// 启动后台任务。
        /// </summary>
        Result Start();

        /// <summary>
        /// 停止后台任务。
        /// </summary>
        Task<Result> StopAsync();
    }
}