using AM.Core.Base;
using AM.Core.Context;
using AM.Model.Common;
using AM.Model.Interfaces.Motion.Assembly;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using System.Threading;

namespace AM.DBService.Services.Motion.Assembly
{
    /// <summary>
    /// 装配接线工作台调试服务。
    /// 当前阶段负责单点 DI/DO 调试。
    /// </summary>
    public class AssemblyWiringDebugService : ServiceBase, IAssemblyWiringDebugService
    {
        protected override string MessageSourceName
        {
            get { return "AssemblyWiringDebug"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        /// <summary>
        /// 读取单个 DI 当前状态。
        /// </summary>
        public Result<bool> ReadDi(short logicalBit)
        {
            if (logicalBit <= 0)
            {
                return Fail<bool>((int)MotionErrorCode.InvalidIoBit, "逻辑 DI 位号无效");
            }

            var hub = GetMotionHub();
            if (hub == null)
            {
                return Fail<bool>((int)MotionErrorCode.CardConnectFailed, "MotionHub 未初始化");
            }

            return hub.GetDI(logicalBit);
        }

        /// <summary>
        /// 设置单个 DO 输出状态。
        /// </summary>
        public Result SetDo(short logicalBit, bool value)
        {
            if (logicalBit <= 0)
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "逻辑 DO 位号无效");
            }

            var hub = GetMotionHub();
            if (hub == null)
            {
                return Fail((int)MotionErrorCode.CardConnectFailed, "MotionHub 未初始化");
            }

            return hub.SetDO(logicalBit, value);
        }

        /// <summary>
        /// 对单个 DO 执行脉冲输出。
        /// </summary>
        public Result PulseDo(short logicalBit, int pulseMs)
        {
            if (logicalBit <= 0)
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "逻辑 DO 位号无效");
            }

            if (pulseMs <= 0)
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "脉冲宽度必须大于 0");
            }

            var hub = GetMotionHub();
            if (hub == null)
            {
                return Fail((int)MotionErrorCode.CardConnectFailed, "MotionHub 未初始化");
            }

            var turnOnResult = hub.SetDO(logicalBit, true);
            if (!turnOnResult.Success)
            {
                return turnOnResult;
            }

            Thread.Sleep(pulseMs);
            return hub.SetDO(logicalBit, false);
        }

        private static IMotionCardService GetMotionHub()
        {
            return MachineContext.Instance.MotionHub;
        }
    }
}
