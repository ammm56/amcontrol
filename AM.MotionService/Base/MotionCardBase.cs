using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Messaging;
using AM.Model.Common;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AM.MotionService.Base
{
    /// <summary>
    /// 运动控制卡抽象基类，提供公共功能实现（如轴配置管理、单位转换等），并定义抽象运动接口由具体厂家类实现。
    /// 消息通知、日志、结果处理、报警在ServiceBase中统一处理
    /// </summary>
    public abstract class MotionCardBase : ServiceBase, IMotionCardService
    {
        protected short _cardId;
        private readonly object _axisMapLock = new object();
        /// <summary>
        /// 逻辑轴映射表：Key 是 LogicalAxis，Value 是完整的轴配置（含物理核、轴、单位系数）
        /// </summary>
        protected Dictionary<short, AxisConfig> _axisMap = new Dictionary<short, AxisConfig>();

        protected override string MessageSourceName
        {
            get { return "MotionCard"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        protected override short? MessageCardId
        {
            get { return _cardId; }
        }

        protected Result Fail(MotionErrorCode code, string message)
        {
            return base.Fail((short)code, message);
        }

        protected Result<T> Fail<T>(MotionErrorCode code, string message)
        {
            return base.Fail<T>((short)code, message);
        }

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
        public abstract Result Enable(short logicalAxis, bool onOff);
        public abstract Result Stop(short logicalAxis, bool isEmergency = false);
        public abstract Result MoveRelative(short logicalAxis, double pulse, double velocity, double acc, double dec);
        public abstract Result MoveRelativeMm(short logicalAxis, double distanceMm, double velMm);
        public abstract Result SetZeroPos(short logicalAxis);
        public abstract Result Initialize(string configPath);
        public abstract Result Connect();
        public abstract Result Disconnect();
        public abstract Result ClearStatus(short logicalAxis);
        public abstract Result ClearAllAxisStatus();
        public abstract Result SetAllZeroPos();
        public abstract Result ConfigAxisHardware(AxisConfig cfg);
        public abstract Result StopAll(bool isEmergency = false);
        public abstract Task<Result> HomeAsync(short logicalAxis);
        public abstract Result Home(short logicalAxis);
        public abstract Result MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        public abstract Result JogMove(short logicalAxis, int direction, double velocity);
        public abstract Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);
        public abstract Result JogMoveMm(short logicalAxis, bool direction, double velMm);
        public abstract Result JogStop(short logicalAxis);
        public abstract Result SetDO(short bit, bool status);
        public abstract Result<bool> GetDI(short bit);
        public abstract Result<bool> GetDO(short bit);
        public abstract Result SetVel(short logicalAxis, double vel);
        public abstract Result SetAcc(short logicalAxis, double acc);
        public abstract Result SetDec(short logicalAxis, double dec);
        public abstract Result<AxisStatus> GetAxisStatus(short logicalAxis);
        public abstract Result<double> GetCommandPosition(short logicalAxis);
        public abstract Result<double> GetEncoderPosition(short logicalAxis);
        public abstract Result<double> GetCommandPositionMm(short logicalAxis);
        public abstract Result<double> GetEncoderPositionMm(short logicalAxis);
        public abstract Result<bool> IsMoving(short logicalAxis);
    }
}
