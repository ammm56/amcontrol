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
        /// <summary>
        /// Key 是 LogicalId
        /// 内部维护一张逻辑轴到物理参数的映射表
        /// </summary>
        private Dictionary<short, AxisConfig> _axisMap;
        /// <summary>
        /// 将通用的配置同步到具体厂家类中
        /// </summary>
        public void LoadAxisConfig(List<AxisConfig> configs)
        {
            // 转换成字典，方便 Move(logicalId) 时快速找到 PhysicalCore 和 PhysicalAxis
            _axisMap = configs.ToDictionary(x => x.LogicalId);

            // 同时给基类设置单位转换参数
            foreach (var cfg in configs)
            {
                this.SetAxisParam(cfg.LogicalId, new AxisParam
                {
                    Lead = cfg.Lead,
                    PulsePerRev = cfg.PulsePerRev
                });
            }
        }
        /// <summary>
        /// 抽取通用的查找逻辑
        /// </summary>
        /// <param name="logicalAxis"></param>
        /// <returns></returns>
        private AxisConfig GetLogicalAxisCfg(short logicalAxis)
        {
            if (_axisMap != null && _axisMap.TryGetValue(logicalAxis, out var cfg))
            {
                return cfg;
            }

            // 如果找不到，统一报一次错
            HandleError(-1, $"逻辑轴 {logicalAxis} 未配置或未加载映射表");
            return null;
        }

        public override bool Initialize(string configPath)
        {
            // 调用固高 GT_Open() 等 API
            short res = mc.GT_Open(0, 1);
            return res == 0;
        }

        // 获取当前位置 (mm)
        public override double GetPositionMm(short logicalAxis)
        {
            // 固高 Axis 1 对应的 Profile 通常也是 1
            short res = mc.GT_GetPos(_axisMap[logicalAxis].PhysicalAxis, out int pulsePos);

            if (res != 0)
            {
                HandleError(res, $"读取轴 {_axisMap[logicalAxis].PhysicalAxis} 位置失败");
                return 0;
            }

            // 调用基类的转换逻辑：Pulse -> Mm
            return PulseToMm(_axisMap[logicalAxis].PhysicalAxis, pulsePos);
        }
        protected override short RawMoveAbs(short cardId, short logicalAxis, int pulse, int vel)
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

        public override short Enable(short logicalAxis, bool onORoff)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return -1; // 找不到配置直接退出

            // 调用固高 API
            short res;
            if (onORoff)
                res = mc.GT_AxisOn(cfg.PhysicalAxis); // 使用查出来的物理参数
            else
                res = mc.GT_AxisOff(cfg.PhysicalAxis);

            if (res != 0) HandleError(res, $"轴 {logicalAxis} 使能失败");
            return res;
        }

        public override short Stop(short logicalAxis, bool emergency = false)
        {
            throw new NotImplementedException();
        }

        public override Task<short> HomeAsync(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override short Home(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override short MoveRelative(short logicalAxis, double distance, double velocity, double acc, double dec)
        {
            throw new NotImplementedException();
        }

        public override short MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec)
        {
            // 1. 设置规划参数 2. 设置目标位置 3. 启动更新
            Log($"轴 {_axisMap[logicalAxis].PhysicalAxis} 绝对运动至 {position}");

            return 0;
        }

        public override short JogMove(short logicalAxis, int direction, double velocity)
        {
            throw new NotImplementedException();
        }

        public override short MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            throw new NotImplementedException();
        }

        public override short MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            int pulse = MmToPulse(logicalAxis, positionMm);

            // 固高逻辑：1. 设置目标位置 2. 更新运动
            short res = mc.GT_SetPos(logicalAxis, pulse);
            // ... 此处省略 GT_Update 等固高必须的后续指令

            if (res != 0) HandleError(res, $"轴 {logicalAxis} 运动指令发送失败");
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

        public override short SetVel(short logicalAxis, double vel)
        {
            throw new NotImplementedException();
        }

        public override short SetAcc(short logicalAxis, double acc)
        {
            throw new NotImplementedException();
        }

        public override short SetDec(short logicalAxis, double dec)
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

        public override double GetEncoderPosition(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override bool IsMoving(short logicalAxis)
        {
            throw new NotImplementedException();
        }
    }
}
