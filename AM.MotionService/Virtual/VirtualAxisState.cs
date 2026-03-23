using AM.Model.Structs;
using System.Threading;

namespace AM.MotionService.Virtual
{
    /// <summary>
    /// 单轴虚拟运行时状态。
    /// 供 VirtualMotionCardService 内部使用，同时作为 3D 可视化数据源对外暴露。
    /// </summary>
    public class VirtualAxisState
    {
        private CancellationTokenSource _currentCts;
        private readonly object _ctsLock = new object();

        /// <summary>规划位置（脉冲）。</summary>
        public double CommandPosition { get; set; }

        /// <summary>编码器位置（脉冲，虚拟卡与规划位置相同）。</summary>
        public double EncoderPosition { get; set; }

        /// <summary>是否使能。</summary>
        public bool IsEnabled { get; set; }

        /// <summary>是否报警。</summary>
        public bool IsAlarm { get; set; }

        /// <summary>是否已回零。</summary>
        public bool IsAtHome { get; set; }

        /// <summary>是否运动中。</summary>
        public bool IsMoving { get; set; }

        /// <summary>当前仿真速度（pulse/ms）。</summary>
        public double CurrentVelocity { get; set; }

        /// <summary>
        /// 停止当前运动任务，取消关联的 CancellationToken。
        /// </summary>
        public void CancelMotion()
        {
            CancellationTokenSource old;
            lock (_ctsLock)
            {
                old = _currentCts;
                _currentCts = null;
            }

            if (old != null)
            {
                old.Cancel();
                old.Dispose();
            }

            IsMoving = false;
            CurrentVelocity = 0;
        }

        /// <summary>
        /// 开始新运动任务，同时取消上一次运动。
        /// 返回新的 CancellationTokenSource，调用方使用其 Token 驱动后台仿真任务。
        /// </summary>
        public CancellationTokenSource StartNewMotion()
        {
            var newCts = new CancellationTokenSource();
            CancellationTokenSource old;
            lock (_ctsLock)
            {
                old = _currentCts;
                _currentCts = newCts;
            }

            if (old != null)
            {
                old.Cancel();
                old.Dispose();
            }

            IsMoving = true;
            return newCts;
        }

        /// <summary>
        /// 快照当前状态为 AxisStatus 结构体（供 3D 渲染帧同步）。
        /// </summary>
        public AxisStatus ToAxisStatus()
        {
            return new AxisStatus
            {
                IsEnabled = IsEnabled,
                IsAlarm = IsAlarm,
                IsAtHome = IsAtHome,
                PositiveLimit = false,
                NegativeLimit = false,
                IsDone = !IsMoving
            };
        }
    }
}