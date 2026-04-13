using AM.Model.Common;
using AM.Model.MotionCard;
using AM.Model.Structs;
using AM.MotionService.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AM.MotionService.Virtual
{
    /// <summary>
    /// 虚拟运动控制卡服务。
    /// 在内存中完整模拟梯形速度曲线运动过程与 IO 状态，无需真实硬件即可运行。
    ///
    /// 3D 虚拟设备集成设计：
    ///   1. 通过 <see cref="AxisPositionChanged"/> / <see cref="AxisStatusChanged"/> /
    ///      <see cref="DOChanged"/> / <see cref="DIChanged"/> 事件向 3D 渲染层推送实时状态；
    ///   2. 通过 <see cref="InjectDI"/> 接收 3D 模拟器注入的传感器反馈信号；
    ///   3. 通过 <see cref="GetAllAxisStates"/> / <see cref="GetAllDoValues"/> /
    ///      <see cref="GetAllDiValues"/> 供 3D 渲染层按帧主动拉取全量状态。
    /// </summary>
    public class VirtualMotionCardService : MotionCardBase
    {
        // 仿真步长（ms）：轴运动每步推进的时间粒度
        private const int SimStepMs = 10;

        // Jog 模式的最大行程（脉冲），足够大的单向行程上限
        private const double JogMaxTravelPulse = 5e8;

        private volatile bool _isConnected;
        private readonly MotionCardConfig _config;

        private readonly ConcurrentDictionary<short, VirtualAxisState> _axisStates
            = new ConcurrentDictionary<short, VirtualAxisState>();

        private readonly ConcurrentDictionary<short, bool> _diValues
            = new ConcurrentDictionary<short, bool>();

        private readonly ConcurrentDictionary<short, bool> _doValues
            = new ConcurrentDictionary<short, bool>();

        protected override string MessageSourceName
        {
            get { return "VirtualCard[" + _cardId + "]"; }
        }

        // =====================================================================
        // 3D 虚拟设备集成事件
        // =====================================================================

        /// <summary>
        /// 轴位置更新事件。
        /// 参数：逻辑轴号、规划位置(pulse)、编码器位置(pulse)。
        /// 3D 渲染层订阅此事件以同步轴运动动画。
        /// </summary>
        public event Action<short, double, double> AxisPositionChanged;

        /// <summary>
        /// 轴状态更新事件。
        /// 参数：逻辑轴号、状态快照。
        /// 3D 渲染层订阅此事件以同步使能、报警、回零等状态显示。
        /// </summary>
        public event Action<short, AxisStatus> AxisStatusChanged;

        /// <summary>
        /// DO 输出状态变化事件。
        /// 参数：逻辑位号、输出值。
        /// 3D 渲染层订阅此事件以驱动气缸、灯塔等执行器动画。
        /// </summary>
        public event Action<short, bool> DOChanged;

        /// <summary>
        /// DI 输入状态变化事件。
        /// 参数：逻辑位号、输入值。
        /// 3D 渲染层订阅此事件以同步传感器状态显示。
        /// </summary>
        public event Action<short, bool> DIChanged;

        // =====================================================================
        // 构造函数
        // =====================================================================

        public VirtualMotionCardService(MotionCardConfig config)
        {
            _config = config;
            _cardId = config != null ? config.CardId : (short)0;
        }

        // =====================================================================
        // 3D 虚拟设备集成接口：主动注入 / 全量拉取
        // =====================================================================

        /// <summary>
        /// 由外部 3D 模拟器注入 DI 信号（模拟传感器触发或到位反馈）。
        /// 例如：气缸伸出到位后，3D 模拟器调用此接口注入对应 DI 反馈。
        /// </summary>
        public void InjectDI(short bit, bool value)
        {
            _diValues[bit] = value;
            DIChanged?.Invoke(bit, value);
        }

        /// <summary>
        /// 获取所有轴当前运行时状态快照（供 3D 渲染帧同步或调试监视）。
        /// </summary>
        public IReadOnlyDictionary<short, VirtualAxisState> GetAllAxisStates()
        {
            return _axisStates;
        }

        /// <summary>获取所有 DO 当前状态（供 3D 渲染帧同步）。</summary>
        public IReadOnlyDictionary<short, bool> GetAllDoValues()
        {
            return _doValues;
        }

        /// <summary>获取所有 DI 当前状态（供 3D 渲染帧同步）。</summary>
        public IReadOnlyDictionary<short, bool> GetAllDiValues()
        {
            return _diValues;
        }

        // =====================================================================
        // 连接生命周期
        // =====================================================================

        public override Result Initialize(string configPath)
        {
            return OkSilent("虚拟卡初始化完成，无需配置文件");
        }

        public override Result Connect()
        {
            _isConnected = true;

            foreach (KeyValuePair<short, AxisConfig> kv in _axisMap)
            {
                _axisStates.TryAdd(kv.Key, new VirtualAxisState());
            }

            return OkLogOnly("虚拟卡连接成功，CardId=" + _cardId);
        }

        public override Result Disconnect()
        {
            _isConnected = false;
            StopAllMotionInternal();
            return OkLogOnly("虚拟卡断开成功，CardId=" + _cardId);
        }

        /// <summary>
        /// 返回虚拟卡当前连接标记。
        /// 该标记在 Connect/Disconnect 中维护，用于上层统一显示连接状态。
        /// </summary>
        public override Result<bool> IsConnected()
        {
            return OkSilent(_isConnected, "虚拟卡连接状态查询成功");
        }

        // =====================================================================
        // 轴维护
        // =====================================================================

        public override Result ClearStatus(short logicalAxis)
        {
            var state = GetOrCreateAxisState(logicalAxis);
            state.IsAlarm = false;
            FireAxisStatusChanged(logicalAxis, state);
            return OkSilent("虚拟轴状态已清除: " + logicalAxis);
        }

        public override Result ClearAllAxisStatus()
        {
            foreach (KeyValuePair<short, VirtualAxisState> kv in _axisStates)
            {
                kv.Value.IsAlarm = false;
                FireAxisStatusChanged(kv.Key, kv.Value);
            }
            return OkSilent("所有虚拟轴状态已清除");
        }

        public override Result SetZeroPos(short logicalAxis)
        {
            var state = GetOrCreateAxisState(logicalAxis);
            state.CommandPosition = 0;
            state.EncoderPosition = 0;
            FireAxisPositionChanged(logicalAxis, state);
            return OkSilent("虚拟轴零点已设置: " + logicalAxis);
        }

        public override Result SetAllZeroPos()
        {
            foreach (KeyValuePair<short, VirtualAxisState> kv in _axisStates)
            {
                kv.Value.CommandPosition = 0;
                kv.Value.EncoderPosition = 0;
                FireAxisPositionChanged(kv.Key, kv.Value);
            }
            return OkSilent("所有虚拟轴零点已设置");
        }

        public override Result ConfigAxisHardware(AxisConfig cfg)
        {
            return OkSilent("虚拟轴硬件配置完成: " + cfg?.LogicalAxis);
        }

        // =====================================================================
        // 使能控制
        // =====================================================================

        public override Result Enable(short logicalAxis, bool onOff)
        {
            var state = GetOrCreateAxisState(logicalAxis);
            state.IsEnabled = onOff;
            FireAxisStatusChanged(logicalAxis, state);
            return OkSilent((onOff ? "使能" : "失能") + " 虚拟轴: " + logicalAxis);
        }

        // =====================================================================
        // 停止
        // =====================================================================

        public override Result Stop(short logicalAxis, bool isEmergency = false)
        {
            var state = GetOrCreateAxisState(logicalAxis);
            state.CancelMotion();
            state.IsMoving = false;
            FireAxisStatusChanged(logicalAxis, state);
            return OkSilent("虚拟轴停止成功: " + logicalAxis);
        }

        public override Result StopAll(bool isEmergency = false)
        {
            StopAllMotionInternal();
            return OkSilent(isEmergency ? "急停所有虚拟轴" : "停止所有虚拟轴");
        }

        // =====================================================================
        // 回零
        // =====================================================================

        public override Result Home(short logicalAxis)
        {
            return HomeAsync(logicalAxis).GetAwaiter().GetResult();
        }

        public override Task<Result> HomeAsync(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
            {
                return Task.FromResult(Fail(cfgResult.Code, cfgResult.Message));
            }

            var state = GetOrCreateAxisState(logicalAxis);
            state.IsAtHome = false;
            var cts = state.StartNewMotion();

            return SimulateHomeInternalAsync(logicalAxis, cfgResult.Item, state, cts.Token);
        }

        // =====================================================================
        // 轴运动（脉冲单位）
        // =====================================================================

        public override Result MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec)
        {
            var state = GetOrCreateAxisState(logicalAxis);
            var cts = state.StartNewMotion();
            Task.Run(() => SimulateMoveInternalAsync(logicalAxis, state, position, velocity, acc, dec, cts.Token));
            return OkSilent("虚拟绝对运动启动: 轴" + logicalAxis + " pos=" + position);
        }

        public override Result MoveRelative(short logicalAxis, double pulse, double velocity, double acc, double dec)
        {
            var state = GetOrCreateAxisState(logicalAxis);
            double target = state.CommandPosition + pulse;
            var cts = state.StartNewMotion();
            Task.Run(() => SimulateMoveInternalAsync(logicalAxis, state, target, velocity, acc, dec, cts.Token));
            return OkSilent("虚拟相对运动启动: 轴" + logicalAxis + " dist=" + pulse);
        }

        public override Result JogMove(short logicalAxis, int direction, double velocity)
        {
            if (Math.Abs(velocity) < 0.0001) velocity = 0.1;

            var state = GetOrCreateAxisState(logicalAxis);
            double target = direction > 0 ? JogMaxTravelPulse : -JogMaxTravelPulse;

            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            double acc = cfgResult.Success ? cfgResult.Item.Acc : 0.1;
            double dec = cfgResult.Success ? cfgResult.Item.Dec : 0.1;

            var cts = state.StartNewMotion();
            Task.Run(() => SimulateMoveInternalAsync(logicalAxis, state, target, Math.Abs(velocity), acc, dec, cts.Token));
            return OkSilent("虚拟Jog启动: 轴" + logicalAxis + (direction > 0 ? " +" : " -"));
        }

        public override Result JogStop(short logicalAxis)
        {
            return Stop(logicalAxis);
        }

        // =====================================================================
        // 轴运动（毫米单位）
        // 速度单位：mm/s；转换：velPulse(pulse/ms) = velMm(mm/s) * K(pulse/mm) / 1000
        // =====================================================================

        public override Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Fail(MotionErrorCode.AxisMapNotFound, cfgResult.Message);

            var cfg = cfgResult.Item;
            if (cfg.K <= 0)
                return Fail(MotionErrorCode.InvalidK, "轴 " + logicalAxis + " K 值非法");

            return MoveAbsolute(logicalAxis,
                positionMm * cfg.K,
                velMm * cfg.K / 1000.0,
                cfg.Acc,
                cfg.Dec);
        }

        public override Result MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Fail(MotionErrorCode.AxisMapNotFound, cfgResult.Message);

            var cfg = cfgResult.Item;
            if (cfg.K <= 0)
                return Fail(MotionErrorCode.InvalidK, "轴 " + logicalAxis + " K 值非法");

            return MoveRelative(logicalAxis,
                distanceMm * cfg.K,
                velMm * cfg.K / 1000.0,
                cfg.Acc,
                cfg.Dec);
        }

        public override Result JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            double velPulse = cfgResult.Success && cfgResult.Item.K > 0
                ? velMm * cfgResult.Item.K / 1000.0
                : velMm;

            return JogMove(logicalAxis, direction ? 1 : -1, velPulse);
        }

        // =====================================================================
        // 参数设置
        // =====================================================================

        public override Result SetVel(short logicalAxis, double vel)
        {
            return OkSilent("虚拟轴速度设置: 轴" + logicalAxis + " v=" + vel);
        }

        public override Result SetAcc(short logicalAxis, double acc)
        {
            return OkSilent("虚拟轴加速度设置: 轴" + logicalAxis + " acc=" + acc);
        }

        public override Result SetDec(short logicalAxis, double dec)
        {
            return OkSilent("虚拟轴减速度设置: 轴" + logicalAxis + " dec=" + dec);
        }

        // =====================================================================
        // 状态读取
        // =====================================================================

        public override Result<AxisStatus> GetAxisStatus(short logicalAxis)
        {
            return OkSilent(GetOrCreateAxisState(logicalAxis).ToAxisStatus());
        }

        public override Result<double> GetCommandPosition(short logicalAxis)
        {
            return OkSilent(GetOrCreateAxisState(logicalAxis).CommandPosition);
        }

        public override Result<double> GetEncoderPosition(short logicalAxis)
        {
            return OkSilent(GetOrCreateAxisState(logicalAxis).EncoderPosition);
        }

        public override Result<double> GetCommandPositionMm(short logicalAxis)
        {
            double pos = GetOrCreateAxisState(logicalAxis).CommandPosition;
            double k = GetKSafe(logicalAxis);
            return OkSilent(k > 0 ? pos / k : 0.0);
        }

        public override Result<double> GetEncoderPositionMm(short logicalAxis)
        {
            double pos = GetOrCreateAxisState(logicalAxis).EncoderPosition;
            double k = GetKSafe(logicalAxis);
            return OkSilent(k > 0 ? pos / k : 0.0);
        }

        public override Result<bool> IsMoving(short logicalAxis)
        {
            return OkSilent(GetOrCreateAxisState(logicalAxis).IsMoving);
        }

        // =====================================================================
        // IO
        // =====================================================================

        public override Result<bool> GetDI(short bit)
        {
            bool value;
            _diValues.TryGetValue(bit, out value);
            return OkSilent(value);
        }

        public override Result<bool> GetDO(short bit)
        {
            bool value;
            _doValues.TryGetValue(bit, out value);
            return OkSilent(value);
        }

        public override Result SetDO(short bit, bool status)
        {
            _doValues[bit] = status;
            DOChanged?.Invoke(bit, status);
            return OkSilent("虚拟DO: bit=" + bit + " → " + status);
        }

        // =====================================================================
        // 内部仿真：梯形速度曲线（Trapezoidal Profile）
        // 单位：position/distance=pulse；velocity=pulse/ms；acc/dec=pulse/ms²
        // =====================================================================

        private async Task SimulateMoveInternalAsync(
            short logicalAxis,
            VirtualAxisState state,
            double targetPosition,
            double velocity,
            double acc,
            double dec,
            CancellationToken token)
        {
            // 参数保护
            if (velocity < 0.001) velocity = 0.1;
            if (acc < 0.001) acc = 0.01;
            if (dec < 0.001) dec = 0.01;

            double startPos = state.CommandPosition;
            double distance = targetPosition - startPos;

            // 小于半脉冲认为已到位
            if (Math.Abs(distance) < 0.5)
            {
                state.IsMoving = false;
                state.CurrentVelocity = 0;
                FireAxisStatusChanged(logicalAxis, state);
                return;
            }

            int sign = distance > 0 ? 1 : -1;
            double absDistance = Math.Abs(distance);
            double currentVel = 0;
            double traveled = 0;

            state.IsMoving = true;
            FireAxisStatusChanged(logicalAxis, state);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    double remaining = absDistance - traveled;
                    if (remaining <= 0.5) break;

                    // 制动距离：v² / (2 × dec)（单位：pulse）
                    double brakingDist = (currentVel * currentVel) / (2.0 * dec);

                    if (remaining <= brakingDist + dec * SimStepMs)
                    {
                        // 减速阶段
                        currentVel = Math.Max(0, currentVel - dec * SimStepMs);
                    }
                    else if (currentVel < velocity)
                    {
                        // 加速阶段
                        currentVel = Math.Min(velocity, currentVel + acc * SimStepMs);
                    }
                    // else：匀速阶段

                    double step = Math.Min(currentVel * SimStepMs, remaining);
                    traveled += step;

                    state.CommandPosition = startPos + sign * traveled;
                    state.EncoderPosition = state.CommandPosition;
                    state.CurrentVelocity = currentVel;

                    FireAxisPositionChanged(logicalAxis, state);

                    await Task.Delay(SimStepMs, token);
                }

                if (!token.IsCancellationRequested)
                {
                    // 精确到位
                    state.CommandPosition = targetPosition;
                    state.EncoderPosition = targetPosition;
                    FireAxisPositionChanged(logicalAxis, state);
                }
            }
            catch (OperationCanceledException)
            {
                // 被主动停止，保留当前中间位置
            }
            finally
            {
                state.IsMoving = false;
                state.CurrentVelocity = 0;
                FireAxisStatusChanged(logicalAxis, state);
            }
        }

        // =====================================================================
        // 内部仿真：回零（两阶段：高速粗搜索 + 低速精定位）
        // =====================================================================

        private async Task<Result> SimulateHomeInternalAsync(
            short logicalAxis,
            AxisConfig cfg,
            VirtualAxisState state,
            CancellationToken token)
        {
            double searchVel = cfg.HomeSearchVelocity > 0 ? cfg.HomeSearchVelocity : 1.0;
            double indexVel = cfg.IndexSearchVelocity > 0 ? cfg.IndexSearchVelocity : searchVel * 0.2;

            // 向负方向搜索（反向对应正回零方向），符合多数工艺习惯
            int searchDir = cfg.ResetDirection > 0 ? -1 : 1;
            int timeoutMs = cfg.HomeTimeoutMs > 0 ? cfg.HomeTimeoutMs : 30000;
            var deadline = DateTime.Now.AddMilliseconds(timeoutMs);

            state.IsMoving = true;
            FireAxisStatusChanged(logicalAxis, state);

            try
            {
                // --- 阶段1：高速粗搜索（模拟 1500ms 后触发原点信号）---
                int phase1StepMs = SimStepMs * 2;
                int phase1Steps = 1500 / phase1StepMs;
                double phase1DistPerStep = searchVel * phase1StepMs;

                for (int i = 0; i < phase1Steps; i++)
                {
                    if (DateTime.Now > deadline)
                    {
                        state.IsMoving = false;
                        FireAxisStatusChanged(logicalAxis, state);
                        return Fail(MotionErrorCode.HomeTimeout, "虚拟轴回零超时: 轴" + logicalAxis);
                    }

                    state.CommandPosition += searchDir * phase1DistPerStep;
                    state.EncoderPosition = state.CommandPosition;
                    state.CurrentVelocity = searchVel;
                    FireAxisPositionChanged(logicalAxis, state);

                    await Task.Delay(phase1StepMs, token);
                }

                // --- 阶段2：低速精定位（反向 500ms，模拟 INDEX 精确定位）---
                int phase2StepMs = SimStepMs;
                int phase2Steps = 500 / phase2StepMs;
                double phase2DistPerStep = indexVel * phase2StepMs;

                for (int i = 0; i < phase2Steps; i++)
                {
                    state.CommandPosition -= searchDir * phase2DistPerStep;
                    state.EncoderPosition = state.CommandPosition;
                    state.CurrentVelocity = indexVel;
                    FireAxisPositionChanged(logicalAxis, state);

                    await Task.Delay(phase2StepMs, token);
                }

                // --- 回零完成 ---
                state.CommandPosition = cfg.HomeAutoZeroPos ? cfg.HomeOffset : state.CommandPosition;
                state.EncoderPosition = state.CommandPosition;
                state.IsAtHome = true;
                state.IsMoving = false;
                state.CurrentVelocity = 0;

                FireAxisPositionChanged(logicalAxis, state);
                FireAxisStatusChanged(logicalAxis, state);

                return OkSilent("虚拟轴回零完成: 轴" + logicalAxis);
            }
            catch (OperationCanceledException)
            {
                state.IsMoving = false;
                state.CurrentVelocity = 0;
                FireAxisStatusChanged(logicalAxis, state);
                return Fail(MotionErrorCode.HomeFailed, "虚拟轴回零被取消: 轴" + logicalAxis);
            }
        }

        // =====================================================================
        // 内部辅助
        // =====================================================================

        private void StopAllMotionInternal()
        {
            foreach (var kv in _axisStates)
            {
                kv.Value.CancelMotion();
                FireAxisStatusChanged(kv.Key, kv.Value);
            }
        }

        private VirtualAxisState GetOrCreateAxisState(short logicalAxis)
        {
            return _axisStates.GetOrAdd(logicalAxis, _ => new VirtualAxisState());
        }

        private double GetKSafe(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            return cfgResult.Success && cfgResult.Item.K > 0 ? cfgResult.Item.K : 1.0;
        }

        private void FireAxisPositionChanged(short logicalAxis, VirtualAxisState state)
        {
            AxisPositionChanged?.Invoke(logicalAxis, state.CommandPosition, state.EncoderPosition);
        }

        private void FireAxisStatusChanged(short logicalAxis, VirtualAxisState state)
        {
            AxisStatusChanged?.Invoke(logicalAxis, state.ToAxisStatus());
        }
    }
}
