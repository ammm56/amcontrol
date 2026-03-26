using AM.Core.Base;
using AM.Core.Context;
using AM.Model.Common;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using System.Linq;
using System.Threading.Tasks;

namespace AM.DBService.Services.Motion.Runtime
{
    /// <summary>
    /// 单轴操作服务。
    /// UI 层统一通过此服务调用 MotionHub，避免页面直接拼装底层调用。
    /// </summary>
    public class MotionAxisOperationService : ServiceBase
    {
        protected override string MessageSourceName
        {
            get { return "MotionAxisOperation"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public Result Enable(short logicalAxis, bool onOff)
        {
            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return hub.Item.Enable(logicalAxis, onOff);
        }

        public Result Stop(short logicalAxis, bool isEmergency)
        {
            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return hub.Item.Stop(logicalAxis, isEmergency);
        }

        public async Task<Result> HomeAsync(short logicalAxis)
        {
            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return await hub.Item.HomeAsync(logicalAxis);
        }

        public Result ClearStatus(short logicalAxis)
        {
            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return hub.Item.ClearStatus(logicalAxis);
        }

        public Result JogMove(short logicalAxis, bool positiveDirection, double velocityMm)
        {
            if (velocityMm <= 0)
            {
                return Fail(-1101, "Jog 速度必须大于 0");
            }

            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return hub.Item.JogMoveMm(logicalAxis, positiveDirection, velocityMm);
        }

        public Result JogStop(short logicalAxis)
        {
            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return hub.Item.JogStop(logicalAxis);
        }

        public Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velocityMm)
        {
            if (velocityMm <= 0)
            {
                return Fail(-1102, "定位速度必须大于 0");
            }

            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return hub.Item.MoveAbsoluteMm(logicalAxis, positionMm, velocityMm);
        }

        public Result MoveRelativeMm(short logicalAxis, double distanceMm, double velocityMm)
        {
            if (velocityMm <= 0)
            {
                return Fail(-1103, "相对运动速度必须大于 0");
            }

            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            return hub.Item.MoveRelativeMm(logicalAxis, distanceMm, velocityMm);
        }

        public Result ApplyVelocityMm(short logicalAxis, double velocityMm)
        {
            if (velocityMm <= 0)
            {
                return Fail(-1104, "速度必须大于 0");
            }

            var axisConfig = FindAxisConfig(logicalAxis);
            if (axisConfig == null)
            {
                return Fail(-1105, "未找到逻辑轴运行时配置: " + logicalAxis);
            }

            if (axisConfig.K <= 0)
            {
                return Fail(-1106, "轴脉冲当量 K 非法，无法应用速度");
            }

            var hub = GetMotionHub();
            if (!hub.Success)
            {
                return Fail(hub.Code, hub.Message);
            }

            var pulsePerMs = velocityMm * axisConfig.K / 1000D;
            return hub.Item.SetVel(logicalAxis, pulsePerMs);
        }

        private Result<IMotionCardService> GetMotionHub()
        {
            var motionHub = MachineContext.Instance.MotionHub;
            if (motionHub == null)
            {
                return Fail<IMotionCardService>(-1100, "MotionHub 未初始化");
            }

            return OkSilent(motionHub, "MotionHub 获取成功");
        }

        private static AxisConfig FindAxisConfig(short logicalAxis)
        {
            var cards = ConfigContext.Instance.Config.MotionCardsConfig;
            if (cards == null)
            {
                return null;
            }

            foreach (var card in cards.Where(x => x != null))
            {
                if (card.AxisConfigs == null)
                {
                    continue;
                }

                var axis = card.AxisConfigs.FirstOrDefault(x => x != null && x.LogicalAxis == logicalAxis);
                if (axis != null)
                {
                    return axis;
                }
            }

            return null;
        }
    }
}