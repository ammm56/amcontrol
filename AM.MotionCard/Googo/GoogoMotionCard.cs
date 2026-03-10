using AM.Model.Common;
using AM.Model.MotionCard;
using AM.Model.Structs;
using AM.Tools;
using GTN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// 连接打开控制卡
        /// 实现基类的抽象方法（多卡支持）
        /// </summary>
        /// <returns></returns>
        public override short Connect()
        {
            var cfg = ConfigSingle.Instance.Config.MotionCardConfig;
            _cardId = cfg.CardId;

            // 1. 强制关闭并清理旧连接（固高习惯：先关再开）
            mc.GTN_Close();
            Thread.Sleep(300);

            // 2. 打开控制卡
            short rtn = mc.GTN_Open(_cardId, cfg.ModeParam);
            if (rtn != 0)
            {
                return HandleError(rtn, $"GTN_Open 通道 {_cardId} 失败。请检查驱动安装或插槽。");
            }

            // 3. 按配置数量循环复位内核
            for (short core = 1; core <= cfg.CoreNumber; core++)
            {
                Thread.Sleep(200);
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

            // 4. 扩展模块初始化 (如果配置开启)
            if (cfg.UseExtModule)
            {
                Thread.Sleep(200);
                rtn = mc.GTN_ExtModuleInit(1, 1); // 默认地址1
                if (rtn != 0) return HandleError(rtn, "GTN_ExtModuleInit 扩展卡启动失败");
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

        /// <summary>
        /// 清除单个逻辑轴的位置
        /// 规划位置（Command Position）和实际位置（Actual Position/Encoder）同时清零
        /// 禁止运动中清除，清除后检查限位
        /// </summary>
        public override short SetZeroPos(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return -1;

            // 参数：内核, 起始物理轴, 轴数(1)
            short rtn = mc.GTN_ZeroPos(cfg.PhysicalCore, cfg.PhysicalAxis, 1);

            if (rtn != 0)
            {
                HandleError(rtn, $"逻辑轴 {logicalAxis} (核{cfg.PhysicalCore} 轴{cfg.PhysicalAxis}) 位置清零失败");
            }
            return rtn;
        }

        /// <summary>
        /// 根据配置清除所有内核的所有轴位置
        /// 规划位置（Command Position）和实际位置（Actual Position/Encoder）同时清零
        /// </summary>
        public override short SetAllZeroPos()
        {
            var mConfig = ConfigSingle.Instance.Config.MotionCardConfig;
            short lastRtn = 0;

            // 遍历配置文件中定义的所有物理内核
            for (short core = 1; core <= mConfig.CoreNumber; core++)
            {
                // 固高指令：从该核的 1 轴开始，清除该核配置的所有轴
                // 第 3 个参数建议使用该核实际物理轴数
                short rtn = mc.GTN_ZeroPos(core, 1, mConfig.AxisCountNumber);

                if (rtn != 0)
                {
                    HandleError(rtn, $"第 {core} 核位置全清失败");
                    lastRtn = rtn;
                }
            }
            return lastRtn;
        }

        public override short ConfigAxisHardware(AxisConfig cfg)
        {
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            // 1. 关闭使能
            mc.GTN_AxisOff(core, axis);

            // 2. 报警信号设置
            if (cfg.AlarmEnable)
            {
                mc.GTN_AlarmOn(core, axis);
                mc.GTN_SetSense(core, mc.MC_ALARM, axis, cfg.AlarmInvert ? (short)1 : (short)0);
            }
            else
            {
                mc.GTN_AlarmOff(core, axis);
            }

            // 3. 脉冲模式 (0: Step/Dir, 1: CW/CCW)
            if (cfg.PulseMode == 0) mc.GTN_StepDir(core, axis);
            else mc.GTN_StepPulse(core, axis);

            // 4. 编码器设置
            if (cfg.EncoderExternal) mc.GTN_EncOn(core, axis);
            else mc.GTN_EncOff(core, axis);

            mc.GTN_SetSense(core, mc.MC_ENCODER, axis, cfg.EncoderInvert ? (short)1 : (short)0);

            // 5. 限位与原点极性 (0: 正逻辑, 1: 负逻辑)
            short sense = cfg.LimitHomeInvert ? (short)1 : (short)0;
            mc.GTN_SetSense(core, mc.MC_LIMIT_POSITIVE, axis, sense);
            mc.GTN_SetSense(core, mc.MC_LIMIT_NEGATIVE, axis, sense);
            mc.GTN_SetSense(core, mc.MC_HOME, axis, sense);

            // 6. 限位有效性设置 (0:正, 1:负, -1:双向, 10:关闭)
            if (cfg.LimitMode == 10) mc.GTN_LmtsOff(core, axis, -1);
            else mc.GTN_LmtsOn(core, axis, cfg.LimitMode);

            return 1;
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

        /// <summary>
        /// 停止单个逻辑轴
        /// </summary>
        public override short Stop(short logicalAxis, bool isEmergency = false)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return -1;

            // 固高参数2(mask)：1 << (axis-1) 对应具体轴
            // 固高参数3(option)：0为平滑停止，1为急停
            short option = isEmergency ? (short)1 : (short)0;
            int mask = 1 << (cfg.PhysicalAxis - 1);

            short rtn = mc.GTN_Stop(cfg.PhysicalCore, mask, option);

            if (rtn != 0) HandleError(rtn, $"轴 {logicalAxis} 停止失败");
            return rtn;
        }
        /// <summary>
        /// 停止所有配置的内核的所有轴
        /// </summary>
        public override short StopAll(bool isEmergency = false)
        {
            var mConfig = ConfigSingle.Instance.Config.MotionCardConfig;
            short option = isEmergency ? (short)1 : (short)0;
            short lastRtn = 0;

            // 遍历所有物理内核
            for (short core = 1; core <= mConfig.CoreNumber; core++)
            {
                // 0xff 表示停止该核前8个轴，0xffff 表示前16个轴
                // 稳妥起见，可以使用 0xffffffff (全1) 停止所有可能存在的轴
                short rtn = mc.GTN_Stop(core, -1, option);

                if (rtn != 0)
                {
                    HandleError(rtn, $"第 {core} 核全轴停止失败");
                    lastRtn = rtn;
                }
            }
            return lastRtn;
        }

        public override Task<short> HomeAsync(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        public override short Home(short logicalAxis)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 点位运动 相对位移（单位: 脉冲）
        /// </summary>
        /// <param name="logicalAxis">逻辑轴 映射核 轴</param>
        /// <param name="pulseDistance">相对位移脉冲数</param>
        /// <param name="velocity">脉冲 电机 1 秒转 1 圈（假设 10000 脉冲），传参就是 10.0</param>
        /// <param name="acc">加速度 脉冲（默认使用配置参数）</param>
        /// <param name="dec">减速带 脉冲（默认使用配置参数）</param>
        /// <returns></returns>
        public override short MoveRelative(short logicalAxis, double pulseDistance, double velocity, double acc, double dec)
        {
            // 1. 获取轴配置映射 (用于找到物理 Core 和 Axis)
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return -1;

            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            // 2. 设置为点位运动模式 (Trap)
            short rtn = mc.GTN_PrfTrap(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 模式切换失败");

            // 3. 设置点位运动曲线参数 (Acc/Dec/SmoothTime)
            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(core, axis, out trap);
            if (rtn == 0)
            {
                // 直接赋值，不涉及任何 K 值换算
                trap.acc = acc > 0 ? acc : cfg.Acc;
                trap.dec = dec > 0 ? dec : cfg.Dec;
                trap.smoothTime = cfg.SmoothTime;
                rtn = mc.GTN_SetTrapPrm(core, axis, ref trap);
            }

            // 4. 设置目标速度 (单位: Pulse/ms)
            rtn = mc.GTN_SetVel(core, axis, velocity);

            // 5. 获取当前规划位置并计算目标位置 (单位: Pulse)
            uint clock;
            rtn = mc.GTN_GetPrfPos(core, axis, out double currentPrf, 1, out clock);
            int targetPos = (int)(currentPrf + pulseDistance);

            // 6. 设置目标位置
            rtn = mc.GTN_SetPos(core, axis, targetPos);

            // 7. 启动运动 (Update 掩码)
            // 1 << (axis-1) 确保只更新当前轴
            rtn = mc.GTN_Update(core, 1 << (axis - 1));

            if (rtn != 0) HandleError(rtn, $"轴{logicalAxis} 指令下发失败");
            return rtn;
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

        /// <summary>
        /// 相对位移运动 (输入单位: mm, mm/s)
        /// </summary>
        /// <param name="logicalAxis">逻辑轴号</param>
        /// <param name="distanceMm">相对移动距离 (mm)</param>
        /// <param name="velMm">运行速度 (mm/s)</param>
        public override short MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            // 1. 获取轴配置映射
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return -1;

            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;
            double K = cfg.K; // 脉冲/mm 系数

            // 2. 模式切换：设置为点位运动模式 (Trap)
            short rtn = mc.GTN_PrfTrap(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 模式切换失败");

            // 3. 设置运动曲线参数 (Acc/Dec/SmoothTime)
            // 注意：配置里存的就是 Pulse/ms²，直接使用，无需换算
            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(core, axis, out trap);
            if (rtn == 0)
            {
                trap.acc = cfg.Acc;
                trap.dec = cfg.Dec;
                trap.smoothTime = cfg.SmoothTime;
                rtn = mc.GTN_SetTrapPrm(core, axis, ref trap);
            }

            // 4. 设置目标速度 (mm/s -> Pulse/ms)
            // 固高 API 单位是 Pulse/ms，所以除以 1000
            double velPulsePerMs = (velMm * K) / 1000.0;
            rtn = mc.GTN_SetVel(core, axis, velPulsePerMs);

            // 5. 获取当前规划位置并计算目标位置 (单位: Pulse)
            uint clock;
            rtn = mc.GTN_GetPrfPos(core, axis, out double currentPrfPulse, 1, out clock);

            // 计算相对位移的脉冲数
            int deltaPulse = (int)(distanceMm * K);
            int targetPulse = (int)(currentPrfPulse + deltaPulse);

            // 6. 设置目标位置
            rtn = mc.GTN_SetPos(core, axis, targetPulse);

            // 7. 启动运动 (Update 掩码)
            // 1 << (axis - 1) 是固高操作单轴的固定逻辑
            rtn = mc.GTN_Update(core, 1 << (axis - 1));

            if (rtn != 0) HandleError(rtn, $"轴{logicalAxis} 启动毫米相对运动失败");
            return rtn;
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

        public override short JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return -1;

            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;
            double K = cfg.K;

            // 1. 设置为 Jog 模式
            short rtn = mc.GTN_PrfJog(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 进入Jog模式失败");

            // 2. 设置 Jog 参数 (Acc/Dec)
            mc.TJogPrm jogPrm;
            rtn = mc.GTN_GetJogPrm(core, axis, out jogPrm);
            if (rtn == 0)
            {
                // 这里的单位转换逻辑与 Trap 运动一致
                jogPrm.acc = (cfg.Acc * K) / 1000000.0;
                jogPrm.dec = (cfg.Dec * K) / 1000000.0;
                rtn = mc.GTN_SetJogPrm(core, axis, ref jogPrm);
            }

            // 3. 设置目标速度 (带方向)
            double speed = direction ? (velMm * K) / 1000.0 : -(velMm * K) / 1000.0;
            rtn = mc.GTN_SetVel(core, axis, speed);

            // 4. 触发运动
            rtn = mc.GTN_Update(core, 1 << (axis - 1));

            if (rtn != 0) HandleError(rtn, $"轴{logicalAxis} 启动Jog失败");
            return rtn;
        }

        public override short JogStop(short logicalAxis)
        {
            // Jog 停止通常调用通用的平滑停止即可
            return this.Stop(logicalAxis, isEmergency: false);
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
