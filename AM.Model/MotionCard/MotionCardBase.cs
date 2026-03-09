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
        protected short _cardId;
        public event Action<short, string> OnError;

        // 存储每个轴的转换参数
        protected Dictionary<short, AxisParam> _axisParams = new Dictionary<short, AxisParam>();

        public virtual short Connect(short cardId)
        {
            _cardId = cardId;
            return 0;
        }

        public void SetAxisParam(short axis, AxisParam param) => _axisParams[axis] = param;

        // --- 核心单位转换逻辑 ---
        protected int MmToPulse(short axis, double mm)
        {
            if (!_axisParams.ContainsKey(axis)) return (int)mm; // 默认 1:1
            return (int)(mm * _axisParams[axis].PulsePerMm);
        }

        protected double PulseToMm(short axis, int pulse)
        {
            if (!_axisParams.ContainsKey(axis)) return pulse;
            return pulse / _axisParams[axis].PulsePerMm;
        }

        // --- 实现 mm 运动指令 ---
        public short MoveAbsMm(short axis, double positionMm, double velMm)
        {
            int pulsePos = MmToPulse(axis, positionMm);
            int pulseVel = MmToPulse(axis, velMm);
            return RawMoveAbs(_cardId, axis, pulsePos, pulseVel); // 调用厂家的原始脉冲接口
        }

        // 触发错误事件
        protected void HandleError(short code, string msg)
        {
            if (code != 0) OnError?.Invoke(_cardId, $"Error {code}: {msg}");
        }

        // 厂家具体的脉冲接口（由具体厂家实现）
        protected abstract short RawMoveAbs(short cardId, short axis, int pulse, int vel);

        // 统一封装日志记录、重试机制或异常处理
        protected void Log(string message) => Console.WriteLine($"[{_cardId}] {message}");
        public abstract bool Initialize(string configPath);
        public abstract short Disconnect();
        public abstract short Enable(short axis, bool onORoff);
        public abstract short Stop(short axis, bool emergency = false);
        public abstract Task<short> HomeAsync(short axis);
        public abstract short Home(short axis);
        public abstract short MoveRelative(short axis, double distance, double velocity, double acc, double dec);
        public abstract short MoveAbsolute(short axis, double position, double velocity, double acc, double dec);
        public abstract short JogMove(short axis, int direction, double velocity);
        public abstract short MoveRelativeMm(short axis, double distanceMm, double velMm);
        public abstract short MoveAbsoluteMm(short axis, double positionMm, double velMm);
        public abstract short SetDO(short bit, bool status);
        public abstract bool GetDI(short bit);
        public abstract bool GetDO(short bit);
        public abstract short SetVel(short axis, double vel);
        public abstract short SetAcc(short axis, double acc);
        public abstract short SetDec(short axis, double dec);
        public abstract AxisStatus GetAxisStatus(short axis);
        public abstract double GetCommandPosition(short axis);
        public abstract double GetEncoderPosition(short axis);
        public abstract double GetPositionMm(short axis);
        public abstract bool IsMoving(short axis);
    }
}
