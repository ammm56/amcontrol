using AM.Core.Context;
using AM.Core.Messaging;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.MotionService.Base
{
    /// <summary>
    /// 运动控制卡抽象基类，提供公共功能实现（如轴配置管理、单位转换、结果处理、消息发布等），并定义抽象运动接口由具体厂家类实现。
    /// </summary>
    public abstract class MotionCardBase : IMotionCardService
    {
        protected short _cardId;
        private readonly object _axisMapLock = new object();

        /// <summary>
        /// 最近一次执行结果（用于上层诊断/UI）
        /// </summary>
        public MotionResult LastResult { get; private set; } = MotionResult.Ok();

        /// <summary>
        /// 逻辑轴映射表：Key 是 LogicalAxis，Value 是完整的轴配置（含物理核、轴、单位系数）
        /// </summary>
        protected Dictionary<short, AxisConfig> _axisMap = new Dictionary<short, AxisConfig>();

        #region 消息总线
        protected IMessageBus MessageBus
        {
            get { return SystemContext.Instance.MessageBus; }
        }

        protected void PublishMessage(SystemMessageType type, string message, string code = null)
        {
            var bus = MessageBus;
            if (bus == null) return;

            bus.Publish(new SystemMessage(message, type, "MotionCard", code, _cardId));
        }

        /// <summary>
        /// 统一成功结果写入
        /// </summary>
        protected MotionResult Ok(string message = "OK")
        {
            LastResult = MotionResult.Ok(message);
            PublishMessage(SystemMessageType.Status, message);
            return LastResult;
        }
        /// <summary>
        /// 统一失败结果写入并分发报警
        /// </summary>
        protected MotionResult Fail(MotionErrorCode code, string message)
        {
            LastResult = MotionResult.Fail(code, message);
            PublishMessage(SystemMessageType.Error, message, ((short)code).ToString());
            return LastResult;
        }
        /// <summary>
        /// 兼容厂家 SDK 返回码（保留）
        /// </summary>
        protected MotionResult HandleError(short code, string message)
        {
            LastResult = code == 0? MotionResult.Ok(message): MotionResult.Fail(code, message);

            if (code == 0)
                PublishMessage(SystemMessageType.Status, message);
            else
                PublishMessage(SystemMessageType.Error, message, code.ToString());

            return LastResult;
        }

        #endregion

        #region 轴配置管理
        /// <summary>
        /// 加载并初始化轴配置映射表
        /// </summary>
        public virtual void LoadAxisConfig(List<AxisConfig> configs)
        {
            if (configs == null)
            {
                Fail(MotionErrorCode.InvalidAxisConfig, "LoadAxisConfig 入参为空");
                return;
            }

            var tempMap = new Dictionary<short, AxisConfig>();
            foreach (var cfg in configs)
            {
                tempMap[cfg.LogicalAxis] = cfg;
            }

            lock (_axisMapLock)
            {
                _axisMap = tempMap;
            }

            Ok("轴映射加载完成");
        }

        /// <summary>
        /// 安全获取逻辑轴配置，若找不到则触发报警
        /// </summary>
        protected AxisConfig GetLogicalAxisCfg(short logicalAxis)
        {
            AxisConfig cfg;
            lock (_axisMapLock)
            {
                if (_axisMap.TryGetValue(logicalAxis, out cfg))
                    return cfg;
            }

            Fail(MotionErrorCode.AxisMapNotFound, "逻辑轴 " + logicalAxis + " 映射未找到");
            return null;
        }

        #endregion

        #region 脉冲/毫米单位转换

        /// <summary>
        /// 使用配置的 K 值进行毫米到脉冲转换，并增加溢出保护
        /// </summary>
        protected int MmToPulse(short logicalAxis, double mm)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null)
            {
                Fail(MotionErrorCode.AxisMapNotFound, "[致命错误] 毫米到脉冲转换，尝试操作未配置的逻辑轴: " + logicalAxis);
                return 0;
            }

            if (cfg.K <= 0)
            {
                Fail(MotionErrorCode.InvalidK, "轴 " + logicalAxis + " 的K值非法(" + cfg.K + ")，请检查导程或脉冲数设置");
                return 0;
            }

            var pulseCalc = mm * cfg.K;
            if (pulseCalc > int.MaxValue || pulseCalc < int.MinValue)
            {
                Fail(MotionErrorCode.PulseOverflow, "轴 " + logicalAxis + " 目标脉冲溢出(" + pulseCalc + ")，请检查行程或 K 值");
                return 0;
            }

            return Convert.ToInt32(Math.Round(pulseCalc, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// 使用配置的 K 值进行脉冲到毫米转换，并增加有效性检查
        /// </summary>
        protected double PulseToMm(short logicalAxis, double pulse)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null)
            {
                Fail(MotionErrorCode.AxisMapNotFound, "[数据错误] 脉冲到毫米转换，尝试换算未配置逻辑轴 " + logicalAxis + " 的坐标");
                return double.NaN;
            }

            if (Math.Abs(cfg.K) < 0.000001)
            {
                Fail(MotionErrorCode.InvalidK, "[参数错误] 脉冲到毫米转换，轴 " + logicalAxis + " 的脉冲当量(K)为0，无法换算坐标");
                return double.NaN;
            }

            return pulse / cfg.K;
        }

        #endregion

        // --- 抽象运动接口 (由厂家类实现具体协议) ---
        public abstract MotionResult Enable(short logicalAxis, bool onOff);
        public abstract MotionResult Stop(short logicalAxis, bool isEmergency = false);
        public abstract MotionResult MoveRelative(short logicalAxis, double pulse, double velocity, double acc, double dec);
        public abstract MotionResult MoveRelativeMm(short logicalAxis, double distanceMm, double velMm);
        public abstract MotionResult SetZeroPos(short logicalAxis);
        public abstract MotionResult Initialize(string configPath);
        public abstract MotionResult Connect();
        public abstract MotionResult Disconnect();
        public abstract MotionResult ClearStatus(short logicalAxis);
        public abstract MotionResult ClearAllAxisStatus();
        public abstract MotionResult SetAllZeroPos();
        public abstract MotionResult ConfigAxisHardware(AxisConfig cfg);
        public abstract MotionResult StopAll(bool isEmergency = false);
        public abstract Task<MotionResult> HomeAsync(short logicalAxis);
        public abstract MotionResult Home(short logicalAxis);
        public abstract MotionResult MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        public abstract MotionResult JogMove(short logicalAxis, int direction, double velocity);
        public abstract MotionResult MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);
        public abstract MotionResult JogMoveMm(short logicalAxis, bool direction, double velMm);
        public abstract MotionResult JogStop(short logicalAxis);
        public abstract MotionResult SetDO(short bit, bool status);
        public abstract bool GetDI(short bit);
        public abstract bool GetDO(short bit);
        public abstract MotionResult SetVel(short logicalAxis, double vel);
        public abstract MotionResult SetAcc(short logicalAxis, double acc);
        public abstract MotionResult SetDec(short logicalAxis, double dec);
        public abstract AxisStatus GetAxisStatus(short logicalAxis);
        public abstract double GetCommandPosition(short logicalAxis);
        public abstract double GetEncoderPosition(short logicalAxis);
        public abstract double GetCommandPositionMm(short logicalAxis);
        public abstract double GetEncoderPositionMm(short logicalAxis);
        public abstract bool IsMoving(short logicalAxis);

        protected void Log(string message) => Console.WriteLine("[" + _cardId + "] " + message);
    }
}
