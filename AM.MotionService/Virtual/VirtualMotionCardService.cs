using AM.Model.Common;
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

        public override Result ClearAllAxisStatus()
        {
            throw new NotImplementedException();
        }

        public override Result ClearStatus(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result ConfigAxisHardware(AxisConfig cfg)
        {
            throw new NotImplementedException();
        }

        public override Result Connect()
        {
            throw new NotImplementedException();
        }

        public override Result Disconnect()
        {
            throw new NotImplementedException();
        }

        public override Result Enable(short logicalAxis, bool onOff)
        {
            throw new NotImplementedException();
        }

        public override Result<AxisStatus> GetAxisStatus(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result<double> GetCommandPosition(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result<double> GetCommandPositionMm(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result<bool> GetDI(short bit)
        {
            throw new NotImplementedException();
        }

        public override Result<bool> GetDO(short bit)
        {
            throw new NotImplementedException();
        }

        public override Result<double> GetEncoderPosition(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result<double> GetEncoderPositionMm(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result Home(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Task<Result> HomeAsync(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result Initialize(string configPath)
        {
            throw new NotImplementedException();
        }

        public override Result<bool> IsMoving(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result JogMove(short logicalAxis, int direction, double velocity)
        {
            throw new NotImplementedException();
        }

        public override Result JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            throw new NotImplementedException();
        }

        public override Result JogStop(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override Result MoveRelative(short logicalAxis, double pulse, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override Result MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override Result SetAcc(short logicalAxis, double acc)
        {
            throw new NotImplementedException();
        }

        public override Result SetAllZeroPos()
        {
            throw new NotImplementedException();
        }

        public override Result SetDec(short logicalAxis, double dec)
        {
            throw new NotImplementedException();
        }

        public override Result SetDO(short bit, bool status)
        {
            throw new NotImplementedException();
        }

        public override Result SetVel(short logicalAxis, double vel)
        {
            throw new NotImplementedException();
        }

        public override Result SetZeroPos(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Result Stop(short logicalAxis, bool isEmergency = false)
        {
            throw new NotImplementedException();
        }

        public override Result StopAll(bool isEmergency = false)
        {
            throw new NotImplementedException();
        }
    }
}
