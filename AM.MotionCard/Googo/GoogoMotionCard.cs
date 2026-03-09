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
using AM.Tools;

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
        public override void LoadAxisConfig(List<AxisConfig> configs)
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

        /// <summary>
        /// 连接打开控制卡
        /// 实现基类的抽象方法（多卡支持）
        /// </summary>
        /// <returns></returns>
        public override short Connect()
        {
            _cardId = ConfigSingle.Instance.Config.MotionCardConfig.CardId;
            short rtn = mc.GTN_Open(ConfigSingle.Instance.Config.MotionCardConfig.CardId, ConfigSingle.Instance.Config.MotionCardConfig.ModeParam);
            if (rtn != 0)
            {
                HandleError(rtn, $"GTN_Open 通道 {_cardId} 失败。请检查驱动安装或插槽。");
                return rtn;
            }

            // 按配置数量复位内核
            for (short core = 1; core <= ConfigSingle.Instance.Config.MotionCardConfig.CoreNumber; core++)
            {
                short resetRtn = mc.GTN_Reset(core);
                if (resetRtn != 0)
                {
                    // 核心逻辑：任何一个内核失败，都是致命错误
                    HandleError(resetRtn, $"GTN_Reset 第 {core} 核复位失败 (总配置 {ConfigSingle.Instance.Config.MotionCardConfig.CoreNumber} 核)");

                    // 失败必须关闭已打开的通道，否则下次无法再次 Open
                    mc.GTN_Close();
                    return resetRtn;
                }
            }

            return rtn;
        }
        /// <summary>
        /// 清除指定核轴状态
        /// </summary>
        /// <param name="logicalAxis"></param>
        /// <returns></returns>
        public override short ClearStatus(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return -1; // 找不到配置直接退出

            // // 参数 1：内核，参数 2：起始轴，参数 3：清除轴的数量（这里是 1）
            short rtn = mc.GTN_ClrSts(cfg.PhysicalCore,cfg.PhysicalAxis,1);
            if (rtn != 0)
            {
                HandleError(rtn, $"GTN_ClrSts 第 {cfg.PhysicalCore} 核 {cfg.PhysicalAxis} 轴 清除状态失败。");
                return rtn;
            }

            return rtn;
        }
        /// <summary>
        /// 清除指定核心所有轴状态
        /// </summary>
        /// <returns></returns>
        public override short ClearAllAxisStatus()
        {
            short lastRtn = 0;

            // 遍历配置文件中定义的所有内核
            for (short core = 1; core <= ConfigSingle.Instance.Config.MotionCardConfig.CoreNumber; core++)
            {
                // 固高的习惯通常是从 1 轴开始，连续清除 N 个轴
                // 第 3 个参数应为该核支持的最大轴数（通常是 8 或 16），或者配置的轴数
                short rtn = mc.GTN_ClrSts(core, 1, ConfigSingle.Instance.Config.MotionCardConfig.AxisCountNumber);

                if (rtn != 0)
                {
                    HandleError(rtn, $"GTN_ClrSts 第 {core} 核清除全轴状态失败。");
                    lastRtn = rtn;
                }
            }
            return lastRtn;
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
