using AM.Core.Context;
using AM.Model.Common;
using AM.Model.MotionCard;
using AM.Model.Structs;
using AM.MotionService.Base;
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
    public class GoogoMotionCardService : MotionCardBase
    {
        private MotionCardConfig CurrentConfig
        {
            get { return ConfigContext.Instance.Config.MotionCardConfig; }
        }

        public GoogoMotionCardService(MotionCardConfig config)
        {
        }

        public override MotionResult Initialize(string configPath)
        {
            // 启动阶段仅做连通性预检查，正式连接由 Connect 负责
            short res = mc.GTN_Open(CurrentConfig.CardId, CurrentConfig.ModeParam);
            if (res != 0)
            {
                return HandleError(res, "Initialize: GTN_Open 失败");
            }
            mc.GTN_Close();
            return Ok("控制卡连通性预检查成功");
        }

        /// <summary>
        /// 连接打开控制卡
        /// 实现基类的抽象方法（多卡支持）
        /// </summary>
        /// <returns></returns>
        public override MotionResult Connect()
        {
            _cardId = CurrentConfig.CardId;

            // 1. 强制关闭并清理旧连接（固高习惯：先关再开）
            mc.GTN_Close();
            Thread.Sleep(300);

            // 2. 打开控制卡
            short rtn = mc.GTN_Open(_cardId, CurrentConfig.ModeParam);
            if (rtn != 0)
            {
                return HandleError(rtn, $"GTN_Open 通道 {_cardId} 失败。请检查驱动安装或插槽。");
            }

            // 3. 按配置数量循环复位内核
            for (short core = 1; core <= CurrentConfig.CoreNumber; core++)
            {
                Thread.Sleep(200);
                short resetRtn = mc.GTN_Reset(core);
                if (resetRtn != 0)
                {
                    // 失败必须关闭已打开的通道，否则下次无法再次 Open
                    mc.GTN_Close();
                    return HandleError(resetRtn, $"GTN_Reset 第 {core} 核复位失败 (总配置 {CurrentConfig.CoreNumber} 核)");
                }
            }

            // 4. 扩展模块初始化 (如果配置开启)
            if (CurrentConfig.UseExtModule)
            {
                Thread.Sleep(200);
                rtn = mc.GTN_ExtModuleInit(1, 1); // 默认地址1
                if (rtn != 0)
                {
                    return HandleError(rtn, "GTN_ExtModuleInit 扩展卡启动失败");
                }
            }

            return Ok("控制卡连接成功");
        }
        /// <summary>
        /// 清除指定核轴状态
        /// </summary>
        /// <param name="logicalAxis"></param>
        /// <returns></returns>
        public override MotionResult ClearStatus(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult; // 找不到配置直接退出

            // // 参数 1：内核，参数 2：起始轴，参数 3：清除轴的数量（这里是 1）
            short rtn = mc.GTN_ClrSts(cfg.PhysicalCore, cfg.PhysicalAxis, 1);
            if (rtn != 0)
            {
                return HandleError(rtn, $"GTN_ClrSts 第 {cfg.PhysicalCore} 核 {cfg.PhysicalAxis} 轴 清除状态失败。");
            }

            return Ok($"逻辑轴 {logicalAxis} 清除状态成功");
        }
        /// <summary>
        /// 清除指定核心所有轴状态
        /// </summary>
        /// <returns></returns>
        public override MotionResult ClearAllAxisStatus()
        {
            // 遍历配置文件中定义的所有内核
            for (short core = 1; core <= CurrentConfig.CoreNumber; core++)
            {
                // 固高的习惯通常是从 1 轴开始，连续清除 N 个轴
                // 第 3 个参数应为该核支持的最大轴数（通常是 8 或 16），或者配置的轴数
                short rtn = mc.GTN_ClrSts(core, 1, CurrentConfig.AxisCountNumber);
                if (rtn != 0)
                {
                    return HandleError(rtn, $"GTN_ClrSts 第 {core} 核清除全轴状态失败。");
                }
            }
            return Ok("全轴状态清除成功");
        }

        /// <summary>
        /// 清除单个逻辑轴的位置
        /// 规划位置（Command Position）和实际位置（Actual Position/Encoder）同时清零
        /// 禁止运动中清除，清除后检查限位
        /// </summary>
        public override MotionResult SetZeroPos(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            // 参数：内核, 起始物理轴, 轴数(1)
            short rtn = mc.GTN_ZeroPos(cfg.PhysicalCore, cfg.PhysicalAxis, 1);
            if (rtn != 0)
            {
                return HandleError(rtn, $"逻辑轴 {logicalAxis} (核{cfg.PhysicalCore} 轴{cfg.PhysicalAxis}) 位置清零失败");
            }

            return Ok($"逻辑轴 {logicalAxis} 位置清零成功");
        }

        /// <summary>
        /// 根据配置清除所有内核的所有轴位置
        /// 规划位置（Command Position）和实际位置（Actual Position/Encoder）同时清零
        /// </summary>
        public override MotionResult SetAllZeroPos()
        {
            // 遍历配置文件中定义的所有物理内核
            for (short core = 1; core <= CurrentConfig.CoreNumber; core++)
            {
                // 固高指令：从该核的 1 轴开始，清除该核配置的所有轴
                // 第 3 个参数建议使用该核实际物理轴数
                short rtn = mc.GTN_ZeroPos(core, 1, CurrentConfig.AxisCountNumber);
                if (rtn != 0)
                {
                    return HandleError(rtn, $"第 {core} 核位置全清失败");
                }
            }
            return Ok("全轴位置清零成功");
        }

        public override MotionResult ConfigAxisHardware(AxisConfig cfg)
        {
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;
            short rtn = 0;

            // 1. 关闭使能
            rtn = mc.GTN_AxisOff(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴硬件配置前关闭使能失败，核{core} 轴{axis}");

            // 2. 报警信号设置
            if (cfg.AlarmEnable)
            {
                rtn = mc.GTN_AlarmOn(core, axis);
                if (rtn != 0) return HandleError(rtn, $"报警使能配置失败，核{core} 轴{axis}");

                rtn = mc.GTN_SetSense(core, mc.MC_ALARM, axis, cfg.AlarmInvert ? (short)1 : (short)0);
                if (rtn != 0) return HandleError(rtn, $"报警极性配置失败，核{core} 轴{axis}");
            }
            else
            {
                rtn = mc.GTN_AlarmOff(core, axis);
                if (rtn != 0) return HandleError(rtn, $"报警关闭配置失败，核{core} 轴{axis}");
            }

            // 3. 脉冲模式 (0: Step/Dir, 1: CW/CCW)
            rtn = cfg.PulseMode == 0 ? mc.GTN_StepDir(core, axis) : mc.GTN_StepPulse(core, axis);
            if (rtn != 0) return HandleError(rtn, $"脉冲模式配置失败，核{core} 轴{axis}");


            // 4. 编码器设置
            rtn = cfg.EncoderExternal ? mc.GTN_EncOn(core, axis) : mc.GTN_EncOff(core, axis);
            if (rtn != 0) return HandleError(rtn, $"扩展编码器配置失败，核{core} 轴{axis}");
            
            rtn = mc.GTN_SetSense(core, mc.MC_ENCODER, axis, cfg.EncoderInvert ? (short)1 : (short)0);
            if (rtn != 0) return HandleError(rtn, $"编码器极性配置失败，核{core} 轴{axis}");

            // 5. 限位与原点极性 (0: 正逻辑, 1: 负逻辑)
            short sense = cfg.LimitHomeInvert ? (short)1 : (short)0;
            rtn = mc.GTN_SetSense(core, mc.MC_LIMIT_POSITIVE, axis, sense);
            if (rtn != 0) return HandleError(rtn, $"正限位极性配置失败，核{core} 轴{axis}");
            rtn = mc.GTN_SetSense(core, mc.MC_LIMIT_NEGATIVE, axis, sense);
            if (rtn != 0) return HandleError(rtn, $"负限位极性配置失败，核{core} 轴{axis}");
            rtn = mc.GTN_SetSense(core, mc.MC_HOME, axis, sense);
            if (rtn != 0) return HandleError(rtn, $"原点极性配置失败，核{core} 轴{axis}");

            // 6. 限位有效性设置 (0:正, 1:负, -1:双向, 10:关闭)
            rtn = cfg.LimitMode == 10 ? mc.GTN_LmtsOff(core, axis, -1) : mc.GTN_LmtsOn(core, axis, cfg.LimitMode);
            if (rtn != 0) return HandleError(rtn, $"限位模式配置失败，核{core} 轴{axis}");

            return Ok($"轴硬件参数配置成功，核{core} 轴{axis}");
        }

        public override MotionResult Disconnect()
        {
            short rtn = mc.GTN_Close();
            if (rtn != 0) return HandleError(rtn, "GTN_Close 失败");
            return Ok("控制卡已断开");
        }

        public override MotionResult Enable(short logicalAxis, bool onOff)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            short rtn = onOff
                ? mc.GTN_AxisOn(cfg.PhysicalCore, cfg.PhysicalAxis)
                : mc.GTN_AxisOff(cfg.PhysicalCore, cfg.PhysicalAxis);

            if (rtn != 0) return HandleError(rtn, "轴 " + logicalAxis + " 使能切换失败");
            return Ok("轴使能状态切换成功");
        }

        /// <summary>
        /// 停止单个逻辑轴
        /// </summary>
        public override MotionResult Stop(short logicalAxis, bool isEmergency = false)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            // 固高参数2(mask)：1 << (axis-1) 对应具体轴
            // 固高参数3(option)：0为平滑停止，1为急停
            short option = isEmergency ? (short)1 : (short)0;
            int mask = 1 << (cfg.PhysicalAxis - 1);

            short rtn = mc.GTN_Stop(cfg.PhysicalCore, mask, option);
            if (rtn != 0)
            {
                return HandleError(rtn, $"轴 {logicalAxis} 停止失败");
            }

            return Ok($"轴 {logicalAxis} 停止成功");
        }
        /// <summary>
        /// 停止所有配置的内核的所有轴
        /// </summary>
        public override MotionResult StopAll(bool isEmergency = false)
        {
            short option = isEmergency ? (short)1 : (short)0;
            // 遍历所有物理内核
            for (short core = 1; core <= CurrentConfig.CoreNumber; core++)
            {
                // 0xff 表示停止该核前8个轴，0xffff 表示前16个轴
                // 稳妥起见，可以使用 0xffffffff (全1) 停止所有可能存在的轴
                short rtn = mc.GTN_Stop(core, -1, option);
                if (rtn != 0)
                {
                    return HandleError(rtn, $"第 {core} 核全轴停止失败");
                }
            }

            return Ok("全轴停止成功");
        }

        public override Task<MotionResult> HomeAsync(short logicalAxis)
        {
            return Task.FromResult(Home(logicalAxis));
        }

        public override MotionResult Home(short logicalAxis)
        {
            return Fail(MotionErrorCode.NotImplemented, $"轴 {logicalAxis} 回零功能未实现");
        }

        #region 相对运动实现（Trap模式）
        /// <summary>
        /// 相对位移运动 (输入单位: mm, mm/s)
        /// </summary>
        /// <param name="logicalAxis">逻辑轴号</param>
        /// <param name="distanceMm">相对移动距离 (mm)</param>
        /// <param name="velMm">运行速度 (mm/s)</param>
        public override MotionResult MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            // mm -> pulse, mm/s -> pulse/ms
            int deltaPulse = MmToPulse(logicalAxis, distanceMm);
            double velPulsePerMs = (velMm * cfg.K) / 1000.0;

            return MoveRelative(logicalAxis, deltaPulse, velPulsePerMs, cfg.Acc, cfg.Dec);
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
        public override MotionResult MoveRelative(short logicalAxis, double pulseDistance, double velocity, double acc, double dec)
        {
            // 1. 获取轴配置映射 (用于找到物理 Core 和 Axis)
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            // 2. 设置为点位运动模式 (Trap)
            short rtn = mc.GTN_PrfTrap(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 模式切换失败");

            // 3. 设置点位运动曲线参数 (Acc/Dec/SmoothTime)
            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(core, axis, out trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Trap参数失败");
            // 直接赋值，不涉及任何 K 值换算
            trap.acc = acc > 0 ? acc : cfg.Acc;
            trap.dec = dec > 0 ? dec : cfg.Dec;
            trap.smoothTime = cfg.SmoothTime;
            rtn = mc.GTN_SetTrapPrm(core, axis, ref trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置Trap参数失败");

            // 4. 设置目标速度 (单位: Pulse/ms)
            rtn = mc.GTN_SetVel(core, axis, velocity);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置速度失败");

            // 5. 获取当前规划位置并计算目标位置 (单位: Pulse)
            uint clock;
            double currentPrf;
            rtn = mc.GTN_GetPrfPos(core, axis, out currentPrf, 1, out clock);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取当前规划位置失败");
            int targetPos = (int)Math.Round(currentPrf + pulseDistance, MidpointRounding.AwayFromZero);

            // 6. 设置目标位置
            rtn = mc.GTN_SetPos(core, axis, targetPos);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置目标位置失败");

            // 7. 启动运动 (Update 掩码)
            // 1 << (axis-1) 确保只更新当前轴
            rtn = mc.GTN_Update(core, 1 << (axis - 1));
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 指令下发失败");

            return Ok($"轴{logicalAxis} 相对运动下发成功");
        }


        #endregion

        #region 绝对运动实现（Trap模式）
        /// <summary>
        /// 绝对位移运动 (输入单位: mm, mm/s)
        /// </summary>
        /// <param name="logicalAxis">逻辑轴号</param>
        /// <param name="positionMm">绝对位移位置 (mm)</param>
        /// <param name="velMm">运行速度 (mm/s)</param>
        public override MotionResult MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            // mm -> pulse, mm/s -> pulse/ms
            int targetPulse = MmToPulse(logicalAxis, positionMm);
            double velPulsePerMs = (velMm * cfg.K) / 1000.0;

            return MoveAbsolute(logicalAxis, targetPulse, velPulsePerMs, cfg.Acc, cfg.Dec);
        }

        /// <summary>
        /// 点位运动 绝对位移（单位: 脉冲）
        /// </summary>
        /// <param name="logicalAxis">逻辑轴 映射核 轴</param>
        /// <param name="targetPulse">绝对位移脉冲数</param>
        /// <param name="velocity">脉冲 电机 1 秒转 1 圈（假设 10000 脉冲），传参就是 10.0</param>
        /// <param name="acc">加速度 脉冲（默认使用配置参数）</param>
        /// <param name="dec">减速带 脉冲（默认使用配置参数）</param>
        /// <returns></returns>
        public override MotionResult MoveAbsolute(short logicalAxis, double targetPulse, double velocity, double acc, double dec)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            short rtn = mc.GTN_PrfTrap(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 模式切换失败");

            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(core, axis, out trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Trap参数失败");

            trap.acc = acc > 0 ? acc : cfg.Acc;
            trap.dec = dec > 0 ? dec : cfg.Dec;
            trap.smoothTime = cfg.SmoothTime;

            rtn = mc.GTN_SetTrapPrm(core, axis, ref trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置Trap参数失败");

            rtn = mc.GTN_SetVel(core, axis, velocity);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置速度失败");

            int pos = (int)Math.Round(targetPulse, MidpointRounding.AwayFromZero);
            rtn = mc.GTN_SetPos(core, axis, pos);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置目标位置失败");

            rtn = mc.GTN_Update(core, 1 << (axis - 1));
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 下发绝对运动失败");

            return Ok($"轴{logicalAxis} 绝对运动下发成功");
        }


        #endregion

        public override MotionResult JogMove(short logicalAxis, int direction, double velocity)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            if (direction == 0)
            {
                return JogStop(logicalAxis);
            }

            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            short rtn = mc.GTN_PrfJog(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 进入Jog模式失败");

            mc.TJogPrm jogPrm;
            rtn = mc.GTN_GetJogPrm(core, axis, out jogPrm);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Jog参数失败");

            jogPrm.acc = cfg.Acc;
            jogPrm.dec = cfg.Dec;

            rtn = mc.GTN_SetJogPrm(core, axis, ref jogPrm);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置Jog参数失败");

            double speed = direction > 0 ? Math.Abs(velocity) : -Math.Abs(velocity);
            rtn = mc.GTN_SetVel(core, axis, speed);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置Jog速度失败");

            rtn = mc.GTN_Update(core, 1 << (axis - 1));
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 启动Jog失败");

            return Ok($"轴{logicalAxis} Jog启动成功");
        }

        public override MotionResult JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;
            double K = cfg.K;

            // 1. 设置为 Jog 模式
            short rtn = mc.GTN_PrfJog(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 进入Jog模式失败");

            // 2. 设置 Jog 参数 (Acc/Dec)
            mc.TJogPrm jogPrm;
            rtn = mc.GTN_GetJogPrm(core, axis, out jogPrm);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Jog参数失败");

            // 这里的单位转换逻辑与 Trap 运动一致
            jogPrm.acc = (cfg.Acc * K) / 1000000.0;
            jogPrm.dec = (cfg.Dec * K) / 1000000.0;
            rtn = mc.GTN_SetJogPrm(core, axis, ref jogPrm);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置Jog参数失败");

            // 3. 设置目标速度 (带方向)
            double speed = direction ? (velMm * K) / 1000.0 : -(velMm * K) / 1000.0;
            rtn = mc.GTN_SetVel(core, axis, speed);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置Jog速度失败");

            // 4. 触发运动
            rtn = mc.GTN_Update(core, 1 << (axis - 1));
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 启动Jog失败");

            return Ok($"轴{logicalAxis} Jog启动成功");
        }

        public override MotionResult JogStop(short logicalAxis)
        {
            return Stop(logicalAxis, false);
        }

        public override MotionResult SetDO(short bit, bool status)
        {
            return Fail(MotionErrorCode.NotImplemented, $"DO 输出未实现，Bit={bit}");
        }

        public override bool GetDI(short bit)
        {
            HandleError((short)MotionErrorCode.NotImplemented, $"DI 读取未实现，Bit={bit}");
            return false;
        }

        public override bool GetDO(short bit)
        {
            HandleError((short)MotionErrorCode.NotImplemented, $"DO 读取未实现，Bit={bit}");
            return false;
        }

        public override MotionResult SetVel(short logicalAxis, double vel)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            short rtn = mc.GTN_SetVel(cfg.PhysicalCore, cfg.PhysicalAxis, vel);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置速度失败");

            return Ok($"轴{logicalAxis} 设置速度成功");
        }

        public override MotionResult SetAcc(short logicalAxis, double acc)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            short rtn = mc.GTN_PrfTrap(cfg.PhysicalCore, cfg.PhysicalAxis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 切换Trap模式失败");

            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(cfg.PhysicalCore, cfg.PhysicalAxis, out trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Trap参数失败");

            trap.acc = acc;
            rtn = mc.GTN_SetTrapPrm(cfg.PhysicalCore, cfg.PhysicalAxis, ref trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置加速度失败");

            return Ok($"轴{logicalAxis} 设置加速度成功");
        }

        public override MotionResult SetDec(short logicalAxis, double dec)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return LastResult;

            short rtn = mc.GTN_PrfTrap(cfg.PhysicalCore, cfg.PhysicalAxis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 切换Trap模式失败");

            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(cfg.PhysicalCore, cfg.PhysicalAxis, out trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Trap参数失败");

            trap.dec = dec;
            rtn = mc.GTN_SetTrapPrm(cfg.PhysicalCore, cfg.PhysicalAxis, ref trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置减速度失败");

            return Ok($"轴{logicalAxis} 设置减速度成功");
        }

        public override AxisStatus GetAxisStatus(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return default(AxisStatus);

            var result = new AxisStatus();

            short posLimit;
            short negLimit;
            short rtn = mc.GTN_GetLimitStatus(cfg.PhysicalCore, cfg.PhysicalAxis, out posLimit, out negLimit);
            if (rtn != 0)
            {
                HandleError(rtn, $"读取轴{logicalAxis}限位状态失败");
                return result;
            }

            result.PositiveLimit = posLimit != 0;
            result.NegativeLimit = negLimit != 0;
            result.IsDone = !IsMoving(logicalAxis);

            // 其余状态位后续再结合固高状态字细化
            result.IsEnabled = false;
            result.IsAlarm = false;
            result.IsAtHome = false;

            return result;
        }

        #region 获得规划/实际位置（脉冲/mm）
        /// <summary>
        /// 获得规划位置（毫米）- 基于 GTN_GetAxisPrfPos 获取脉冲后转换为毫米
        /// </summary>
        /// <param name="logicalId"></param>
        /// <returns></returns>
        public override double GetCommandPositionMm(short logicalAxis)
        {
            var pulse = GetCommandPosition(logicalAxis);
            if (double.IsNaN(pulse)) return double.NaN;
            return PulseToMm(logicalAxis, pulse);
        }
        /// <summary>
        /// 获取规划位置（脉冲）- 使用 GTN_GetAxisPrfPos
        /// </summary>
        /// <param name="logicalAxis"></param>
        /// <returns></returns>
        public override double GetCommandPosition(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return double.NaN;

            uint clock;
            double prfPulse;
            short rtn = mc.GTN_GetAxisPrfPos(cfg.PhysicalCore, cfg.PhysicalAxis, out prfPulse, 1, out clock);
            if (rtn != 0)
            {
                HandleError(rtn, $"读取轴{logicalAxis}规划位置失败");
                return double.NaN;
            }

            return prfPulse;
        }
        /// <summary>
        /// 获得实际位置（毫米）- 基于 GTN_GetAxisEncPos 获取脉冲后转换为毫米
        /// </summary>
        /// <param name="logicalId"></param>
        /// <returns></returns>
        public override double GetEncoderPositionMm(short logicalAxis)
        {
            var pulse = GetEncoderPosition(logicalAxis);
            if (double.IsNaN(pulse)) return double.NaN;
            return PulseToMm(logicalAxis, pulse);
        }
        /// <summary>
        /// 获取实际编码器位置（脉冲）- 使用 GTN_GetAxisEncPos
        /// </summary>
        /// <param name="logicalAxis"></param>
        /// <returns></returns>
        public override double GetEncoderPosition(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return double.NaN;

            uint clock;
            double encPulse;
            short rtn = mc.GTN_GetAxisEncPos(cfg.PhysicalCore, cfg.PhysicalAxis, out encPulse, 1, out clock);
            if (rtn != 0)
            {
                HandleError(rtn, $"读取轴{logicalAxis}编码器位置失败");
                return double.NaN;
            }

            return encPulse;
        }

        #endregion

        public override bool IsMoving(short logicalAxis)
        {
            var cfg = GetLogicalAxisCfg(logicalAxis);
            if (cfg == null) return false;

            uint clock;
            double vel;
            short rtn = mc.GTN_GetPrfVel(cfg.PhysicalCore, cfg.PhysicalAxis, out vel, 1, out clock);
            if (rtn != 0)
            {
                HandleError(rtn, $"读取轴{logicalAxis}速度失败");
                return false;
            }

            return Math.Abs(vel) > 0.0001;
        }

    }
}
