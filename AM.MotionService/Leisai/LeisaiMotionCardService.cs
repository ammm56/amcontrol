using AM.Model.MotionCard;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.MotionService.Leisai
{
    public class LeisaiMotionCardService : MotionCardBase
    {
        private readonly MotionCardConfig _config;

        public LeisaiMotionCardService(MotionCardConfig config)
        {
            _config = config;
        }

        public override short ClearAllAxisStatus()
        {
            throw new NotImplementedException();
        }

        public override short ClearStatus(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override short ConfigAxisHardware(AxisConfig cfg)
        {
            throw new NotImplementedException();
        }

        public override short Connect()
        {
            throw new NotImplementedException();
        }

        public override short Disconnect()
        {
            throw new NotImplementedException();
        }

        public override short Enable(short logicalId, bool onOff)
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

        public override double GetPositionMm(short logicalId)
        {
            throw new NotImplementedException();
        }

        public override short Home(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override Task<short> HomeAsync(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override bool Initialize(string configPath)
        {
            throw new NotImplementedException();
        }

        public override bool IsMoving(short logicalId)
        {
            throw new NotImplementedException();
        }

        public override short JogMove(short logicalAxis, int direction, double velocity)
        {
            throw new NotImplementedException();
        }

        public override short JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            throw new NotImplementedException();
        }

        public override short JogStop(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override short MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override short MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override short MoveRelative(short logicalId, double pulse, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override short MoveRelativeMm(short logicalId, double distanceMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override short SetAcc(short logicalAxis, double acc)
        {
            throw new NotImplementedException();
        }

        public override short SetAllZeroPos()
        {
            throw new NotImplementedException();
        }

        public override short SetDec(short logicalAxis, double dec)
        {
            throw new NotImplementedException();
        }

        public override short SetDO(short bit, bool status)
        {
            throw new NotImplementedException();
        }

        public override short SetVel(short logicalAxis, double vel)
        {
            throw new NotImplementedException();
        }

        public override short SetZeroPos(short logicalId)
        {
            throw new NotImplementedException();
        }

        public override short Stop(short logicalId, bool isEmergency = false)
        {
            throw new NotImplementedException();
        }

        public override short StopAll(bool isEmergency = false)
        {
            throw new NotImplementedException();
        }
    }
}
