using AM.Model.Common;
using AM.Model.ICommon;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.MotionCard
{
    public abstract class MotionCardBase : ObservableObject, IMotionCard
    {
        // --- 字段 ---
        protected short _cardId;

        // --- 事件通知 ---
        public event Action<short, string> OnError;

        /// <summary>
        /// 逻辑轴映射表：Key 是 LogicalId，Value 是完整的轴配置（含物理核、轴、单位系数）
        /// 内部维护一张逻辑轴到物理参数的映射表
        /// </summary>
        protected Dictionary<short, AxisConfig> _axisMap = new Dictionary<short, AxisConfig>();

        /// <summary>
        /// 加载并初始化轴配置映射表
        /// </summary>
        public virtual void LoadAxisConfig(List<AxisConfig> configs)
        {
            // 转换成字典，方便 Move(logicalId) 时快速找到 PhysicalCore 和 PhysicalAxis
            _axisMap.Clear();
            foreach (var cfg in configs)
            {
                _axisMap[cfg.LogicalId] = cfg;
            }
        }

        /// <summary>
        /// 安全获取逻辑轴配置，若找不到则触发报警
        /// </summary>
        protected AxisConfig GetLogicalAxisCfg(short logicalId)
        {
            if (_axisMap.TryGetValue(logicalId, out var cfg)) return cfg;

            HandleError(-1, $"未找到逻辑轴 {logicalId} 的硬件映射配置");
            return null;
        }

        /// <summary>
        /// 统一错误处理分发
        /// </summary>
        protected short HandleError(short code, string message)
        {
            if (code != 0) OnError?.Invoke(_cardId, message);
            return code;
        }

        // --- 统一单位转换 (直接引用 AxisConfig 的 K 值) ---
        /// <summary>
        /// 使用配置的 K 值进行毫米到脉冲的转换，并增加数据溢出保护
        /// K值的计算方式为：K = (PulsePerRev * GearRatio) / Lead
        /// </summary>
        /// <param name="logicalId"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected int MmToPulse(short logicalId, double mm)
        {
            var cfg = GetLogicalAxisCfg(logicalId);
            if (cfg == null)
            {
                throw new InvalidOperationException($"[致命错误] 毫米到脉冲转换，尝试操作未配置的逻辑轴: {logicalId}");
            }
            // 增加数据溢出保护：防止计算出的脉冲数超过 int 范围
            double pulseCalc = mm * cfg.K;
            if (pulseCalc > int.MaxValue || pulseCalc < int.MinValue)
            {
                HandleError(-2, $"轴 {logicalId} 目标脉冲数溢出，请检查行程或 K 值。");
                return 0; // 返回 0 脉冲，防止飞车
            }
            return (int)pulseCalc;
        }
        /// <summary>
        /// 使用配置的 K 值进行脉冲到毫米的转换，并增加数据有效性检查
        /// K值的计算方式为：K = (PulsePerRev * GearRatio) / Lead
        /// </summary>
        /// <param name="logicalId"></param>
        /// <param name="pulse"></param>
        /// <returns></returns>
        protected double PulseToMm(short logicalId, double pulse)
        {
            var cfg = GetLogicalAxisCfg(logicalId);
            if (cfg == null)
            {
                // 记录错误但不崩溃，返回非数字 (NaN) 让 UI 显示为空或异常
                HandleError(-3, $"[数据错误] 脉冲到毫米转化，尝试换算未配置逻辑轴 {logicalId} 的坐标");
                return double.NaN;
            }
            // K 值是否合法（防止除以 0）
            // 如果 Lead 设为 0，K 会变成无穷大或 0
            if (Math.Abs(cfg.K) < 0.000001)
            {
                HandleError(-4, $"[参数错误] 脉冲到毫米转化，轴 {logicalId} 的脉冲当量(K)为0，无法换算坐标");
                return double.NaN;
            }
            return pulse / cfg.K;
        }

        // --- 抽象运动接口 (由厂家类实现具体协议) ---
        public abstract short Enable(short logicalId, bool onOff);
        public abstract short Stop(short logicalId, bool isEmergency = false);
        public abstract short MoveRelative(short logicalId, double pulse, double velocity, double acc, double dec);
        public abstract short MoveRelativeMm(short logicalId, double distanceMm, double velMm);
        public abstract short SetZeroPos(short logicalId);
        public abstract bool Initialize(string configPath);
        public abstract short Connect();
        public abstract short Disconnect();
        public abstract short ClearStatus(short logicalAxis);
        public abstract short ClearAllAxisStatus();
        public abstract short SetAllZeroPos();
        public abstract short ConfigAxisHardware(AxisConfig cfg);
        public abstract short StopAll(bool isEmergency = false);
        public abstract Task<short> HomeAsync(short logicalAxis);
        public abstract short Home(short logicalAxis);
        public abstract short MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        public abstract short JogMove(short logicalAxis, int direction, double velocity);
        public abstract short MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);
        public abstract short JogMoveMm(short logicalAxis, bool direction, double velMm);
        public abstract short JogStop(short logicalAxis);
        public abstract short SetDO(short bit, bool status);
        public abstract bool GetDI(short bit);
        public abstract bool GetDO(short bit);
        public abstract short SetVel(short logicalAxis, double vel);
        public abstract short SetAcc(short logicalAxis, double acc);
        public abstract short SetDec(short logicalAxis, double dec);
        public abstract AxisStatus GetAxisStatus(short logicalAxis);
        public abstract double GetCommandPosition(short logicalAxis);
        public abstract double GetEncoderPosition(short logicalAxis);

        // --- 状态查询 ---
        public abstract bool IsMoving(short logicalId);
        public abstract double GetPositionMm(short logicalId);


        // 统一封装日志记录、重试机制或异常处理
        protected void Log(string message) => Console.WriteLine($"[{_cardId}] {message}");
        
    }
}
