using AM.Model.Common;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GTN;
using AM.Model.MotionCard;

namespace AM.MotionCard.Googo
{
    public class GoogoMotionCard : MotionCardBase
    {
        public override bool Initialize(string configPath)
        {
            // 调用固高 GT_Open() 等 API
            short res = mc.GT_Open(0, 1);
            return res == 0;
        }

        // 获取当前位置 (mm)
        public override double GetPositionMm(short axis)
        {
            // 固高 Axis 1 对应的 Profile 通常也是 1
            short res = mc.GT_GetPos(axis, out int pulsePos);

            if (res != 0)
            {
                HandleError(res, $"读取轴 {axis} 位置失败");
                return 0;
            }

            // 调用基类的转换逻辑：Pulse -> Mm
            return PulseToMm(axis, pulsePos);
        }
        protected override short RawMoveAbs(short cardId, short axis, int pulse, int vel)
        {
            return 0;
        }

        // 实现基类的抽象方法（多卡支持：_cardId 在基类 Connect 时已赋值）
        public override short Connect(short cardId)
        {
            _cardId = cardId;
            short res = mc.GT_Open(_cardId, 0); // 0 为正常模式
            if (res != 0) HandleError(res, "固高卡打开失败");
            return res;
        }

        public override short Disconnect()
        {
            throw new NotImplementedException();
        }

        public override short Enable(short axis, bool onORoff)
        {
            throw new NotImplementedException();
        }

        public override short Stop(short axis, bool emergency = false)
        {
            throw new NotImplementedException();
        }

        public override Task<short> HomeAsync(short axis)
        {
            throw new NotImplementedException();
        }

        public override short Home(short axis)
        {
            throw new NotImplementedException();
        }

        public override short MoveRelative(short axis, double distance, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override short MoveAbsolute(short axis, double position, double velocity, double acc, double dec)
        {
            // 1. 设置规划参数 2. 设置目标位置 3. 启动更新
            Log($"轴 {axis} 绝对运动至 {position}");

            return 0;
        }

        public override short JogMove(short axis, int direction, double velocity)
        {
            throw new NotImplementedException();
        }

        public override short MoveRelativeMm(short axis, double distanceMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override short MoveAbsoluteMm(short axis, double positionMm, double velMm)
        {
            int pulse = MmToPulse(axis, positionMm);

            // 固高逻辑：1. 设置目标位置 2. 更新运动
            short res = mc.GT_SetPos(axis, pulse);
            // ... 此处省略 GT_Update 等固高必须的后续指令

            if (res != 0) HandleError(res, $"轴 {axis} 运动指令发送失败");
            return res;
        }

        public override short SetDO(short bit, bool status)
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

        public override short SetVel(short axis, double vel)
        {
            throw new NotImplementedException();
        }

        public override short SetAcc(short axis, double acc)
        {
            throw new NotImplementedException();
        }

        public override short SetDec(short axis, double dec)
        {
            throw new NotImplementedException();
        }

        public override AxisStatus GetAxisStatus(short axis)
        {
            throw new NotImplementedException();
        }

        public override double GetCommandPosition(short axis)
        {
            throw new NotImplementedException();
        }

        public override double GetEncoderPosition(short axis)
        {
            throw new NotImplementedException();
        }

        public override bool IsMoving(short axis)
        {
            throw new NotImplementedException();
        }
    }
}
