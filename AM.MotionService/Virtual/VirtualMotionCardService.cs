using AM.Model.MotionCard;
using AM.Model.Structs;
using AM.MotionService.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.MotionService.Virtual
{
    public class VirtualMotionCardService : MotionCardBase
    {
        private readonly MotionCardConfig _config;

        public VirtualMotionCardService(MotionCardConfig config)
        {
            _config = config;
        }

        public override MotionResult ClearAllAxisStatus()
        {
            throw new NotImplementedException();
        }

        public override MotionResult ClearStatus(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override MotionResult ConfigAxisHardware(AxisConfig cfg)
        {
            throw new NotImplementedException();
        }

        public override MotionResult Connect()
        {
            throw new NotImplementedException();
        }

        public override MotionResult Disconnect()
        {
            throw new NotImplementedException();
        }

        public override MotionResult Enable(short logicalAxis, bool onOff)
        {
            throw new NotImplementedException();
        }

        public override AxisStatus GetAxisStatus(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override double GetCommandPosition(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override double GetCommandPositionMm(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override bool GetDI(short bit)
        {
            throw new NotImplementedException();
        }

        public override bool GetDO(short bit)
        {
            throw new NotImplementedException();
        }

        public override double GetEncoderPosition(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override double GetEncoderPositionMm(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override MotionResult Home(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Task<MotionResult> HomeAsync(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override MotionResult Initialize(string configPath)
        {
            throw new NotImplementedException();
        }

        public override bool IsMoving(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override MotionResult JogMove(short logicalAxis, int direction, double velocity)
        {
            throw new NotImplementedException();
        }

        public override MotionResult JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            throw new NotImplementedException();
        }

        public override MotionResult JogStop(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override MotionResult MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override MotionResult MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override MotionResult MoveRelative(short logicalAxis, double pulse, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override MotionResult MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override MotionResult SetAcc(short logicalAxis, double acc)
        {
            throw new NotImplementedException();
        }

        public override MotionResult SetAllZeroPos()
        {
            throw new NotImplementedException();
        }

        public override MotionResult SetDec(short logicalAxis, double dec)
        {
            throw new NotImplementedException();
        }

        public override MotionResult SetDO(short bit, bool status)
        {
            throw new NotImplementedException();
        }

        public override MotionResult SetVel(short logicalAxis, double vel)
        {
            throw new NotImplementedException();
        }

        public override MotionResult SetZeroPos(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override MotionResult Stop(short logicalAxis, bool isEmergency = false)
        {
            throw new NotImplementedException();
        }

        public override MotionResult StopAll(bool isEmergency = false)
        {
            throw new NotImplementedException();
        }
    }
}
