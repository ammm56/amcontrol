using AM.Core.Base;
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
    /// </summary>
    public abstract class MotionCardBase : ServiceBase, IMotionCardService
    {
        protected short _cardId;
        private readonly object _axisMapLock = new object();

        /// <summary>
        /// 逻辑轴映射表：Key 是 LogicalAxis，Value 是完整的轴配置。
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
            return base.Fail((int)code, message);
        }

        protected Result<T> Fail<T>(MotionErrorCode code, string message)
        {
            return base.Fail<T>((int)code, message);
        }

        #region 轴配置管理

        public virtual Result LoadAxisConfigResult(List<AxisConfig> configs)
        {
            if (configs == null)
            {
                return Fail(MotionErrorCode.InvalidAxisConfig, "LoadAxisConfig 入参为空");
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

            return Ok("轴映射加载完成");
        }

        public virtual void LoadAxisConfig(List<AxisConfig> configs)
        {
            LoadAxisConfigResult(configs);
        }

        protected Result<AxisConfig> GetLogicalAxisCfgResult(short logicalAxis)
        {
            AxisConfig cfg;
            lock (_axisMapLock)
            {
                if (_axisMap.TryGetValue(logicalAxis, out cfg))
                    return Ok(cfg, "逻辑轴映射获取成功");
            }

            return Fail<AxisConfig>(MotionErrorCode.AxisMapNotFound, "逻辑轴 " + logicalAxis + " 映射未找到");
        }

        #endregion

        #region 脉冲/毫米单位转换

        protected Result<int> MmToPulseResult(short logicalAxis, double mm)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
            {
                return Result<int>.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            }

            var cfg = cfgResult.Item;
            if (cfg.K <= 0)
            {
                return Fail<int>(MotionErrorCode.InvalidK, "轴 " + logicalAxis + " 的K值非法(" + cfg.K + ")，请检查导程或脉冲数设置");
            }

            var pulseCalc = mm * cfg.K;
            if (pulseCalc > int.MaxValue || pulseCalc < int.MinValue)
            {
                return Fail<int>(MotionErrorCode.PulseOverflow, "轴 " + logicalAxis + " 目标脉冲溢出(" + pulseCalc + ")，请检查行程或 K 值");
            }

            var pulse = Convert.ToInt32(Math.Round(pulseCalc, MidpointRounding.AwayFromZero));
            return Ok(pulse, "毫米转脉冲成功");
        }

        protected Result<double> PulseToMmResult(short logicalAxis, double pulse)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
            {
                return Result<double>.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            }

            var cfg = cfgResult.Item;
            if (Math.Abs(cfg.K) < 0.000001)
            {
                return Fail<double>(MotionErrorCode.InvalidK, "轴 " + logicalAxis + " 的脉冲当量(K)为0，无法换算坐标");
            }

            return Ok(pulse / cfg.K, "脉冲转毫米成功");
        }

        #endregion

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
