using AM.Model.Common;
using AM.Model.MotionCard;
using AM.Model.Structs;
using AM.MotionService.Base;
using GTN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.MotionCard.Googo
{
    public class GoogoMotionCardService : MotionCardBase
    {
        private readonly MotionCardConfig _config;

        private MotionCardConfig CurrentConfig
        {
            get { return _config; }
        }

        public GoogoMotionCardService(MotionCardConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _config = config;
        }

        public override Result Initialize(string configPath)
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
        public override Result Connect()
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
        public override Result ClearStatus(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            short rtn = mc.GTN_ClrSts(core, axis, 1);
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
        public override Result ClearAllAxisStatus()
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
        public override Result SetZeroPos(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            short rtn = mc.GTN_ZeroPos(core, axis, 1);
            if (rtn != 0)
            {
                return HandleError(rtn, $"逻辑轴 {logicalAxis} (核{core} 轴{axis}) 位置清零失败");
            }

            return Ok($"逻辑轴 {logicalAxis} 位置清零成功");
        }

        /// <summary>
        /// 根据配置清除所有内核的所有轴位置
        /// 规划位置（Command Position）和实际位置（Actual Position/Encoder）同时清零
        /// </summary>
        public override Result SetAllZeroPos()
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

        public override Result ConfigAxisHardware(AxisConfig cfg)
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

        public override Result Disconnect()
        {
            short rtn = mc.GTN_Close();
            if (rtn != 0) return HandleError(rtn, "GTN_Close 失败");
            return Ok("控制卡已断开");
        }

        public override Result Enable(short logicalAxis, bool onOff)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            short rtn = onOff
                ? mc.GTN_AxisOn(core, axis)
                : mc.GTN_AxisOff(core, axis);

            if (rtn != 0) return HandleError(rtn, "轴 " + logicalAxis + " 使能切换失败");

            cfg.IsServoOn = onOff;
            return Ok("轴使能状态切换成功");
        }

        /// <summary>
        /// 停止单个逻辑轴
        /// </summary>
        public override Result Stop(short logicalAxis, bool isEmergency = false)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            short option = isEmergency ? (short)1 : (short)0;
            int mask = 1 << (axis - 1);

            short rtn = mc.GTN_Stop(core, mask, option);
            if (rtn != 0)
            {
                return HandleError(rtn, $"轴 {logicalAxis} 停止失败");
            }

            return Ok($"轴 {logicalAxis} 停止成功");
        }
        /// <summary>
        /// 停止所有配置的内核的所有轴
        /// </summary>
        public override Result StopAll(bool isEmergency = false)
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

        #region 回零
        public override Task<Result> HomeAsync(short logicalAxis)
        {
            return Task.Run(() => Home(logicalAxis));
        }

        public override Result Home(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            if (cfg.StandardHomeMode <= 0)
            {
                return Fail(MotionErrorCode.HomeConfigInvalid, "轴 " + logicalAxis + " 回零模式未配置");
            }

            if (cfg.HomeHighSpeed <= 0 || cfg.HomeLowSpeed <= 0)
            {
                return Fail(MotionErrorCode.HomeConfigInvalid, "轴 " + logicalAxis + " 回零速度配置无效");
            }

            if (cfg.HomeTimeoutMs <= 0)
            {
                return Fail(MotionErrorCode.HomeConfigInvalid, "轴 " + logicalAxis + " 回零超时配置无效");
            }

            mc.TStandardHomePrm prm = new mc.TStandardHomePrm
            {
                mode = cfg.StandardHomeMode,
                highSpeed = cfg.HomeHighSpeed,
                lowSpeed = cfg.HomeLowSpeed,
                acc = cfg.Acc,
                offset = cfg.HomeOffset,
                check = cfg.HomeCheck ? (short)1 : (short)0,
                autoZeroPos = cfg.HomeAutoZeroPos ? (short)1 : (short)0,
                motorStopDelay = 0
            };

            short rtn = mc.GTN_ExecuteStandardHome(cfg.PhysicalCore, cfg.PhysicalAxis, ref prm);
            if (rtn != 0)
            {
                return HandleError(rtn, "轴 " + logicalAxis + " 启动标准回零失败");
            }

            return WaitStandardHomeDone(cfg);
        }

        #endregion

        #region 相对运动实现（Trap模式）
        /// <summary>
        /// 相对位移运动 (输入单位: mm, mm/s)
        /// </summary>
        /// <param name="logicalAxis">逻辑轴号</param>
        /// <param name="distanceMm">相对移动距离 (mm)</param>
        /// <param name="velMm">运行速度 (mm/s)</param>
        public override Result MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            var pulseResult = MmToPulseResult(logicalAxis, distanceMm);
            if (!pulseResult.Success)
                return Result.Fail(pulseResult.Code, pulseResult.Message, ResultSource.Motion);

            double velPulsePerMs = (velMm * cfg.K) / 1000.0;

            return MoveRelative(logicalAxis, pulseResult.Item, velPulsePerMs, cfg.Acc, cfg.Dec);
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
        public override Result MoveRelative(short logicalAxis, double pulseDistance, double velocity, double acc, double dec)
        {
            // 1. 获取轴配置映射 (用于找到物理 Core 和 Axis)
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

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
            if (rtn != 0)
            {
                return HandleError(rtn, $"轴{logicalAxis} 设置目标位置失败");
            }

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
        public override Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfg.PhysicalCore;
            short axis = cfg.PhysicalAxis;

            var pulseResult = MmToPulseResult(logicalAxis, positionMm);
            if (!pulseResult.Success)
                return Result.Fail(pulseResult.Code, pulseResult.Message, ResultSource.Motion);

            double velPulsePerMs = (velMm * cfg.K) / 1000.0;

            return MoveAbsolute(logicalAxis, pulseResult.Item, velPulsePerMs, cfg.Acc, cfg.Dec);
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
        public override Result MoveAbsolute(short logicalAxis, double targetPulse, double velocity, double acc, double dec)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

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

        public override Result JogMove(short logicalAxis, int direction, double velocity)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            if (direction == 0)
            {
                return JogStop(logicalAxis);
            }

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

        public override Result JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;
            double K = cfgResult.Item.K;

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

        public override Result JogStop(short logicalAxis)
        {
            return Stop(logicalAxis, false);
        }

        #region 数字输入/输出接口

        public override Result SetDO(short bit, bool status)
        {
            var map = GetDOBitMap(bit);
            if (map == null)
            {
                return Fail(MotionErrorCode.IoMapNotFound, "DO 映射未找到，Bit=" + bit);
            }

            short value = status ? (short)1 : (short)0;
            short rtn;

            if (map.IsExtModule)
            {
                rtn = mc.GTN_SetExtDoBit(map.Core, map.HardwareBit, value);
                if (rtn != 0)
                {
                    return HandleError(rtn, "扩展DO输出失败，Bit=" + bit);
                }

                return Ok("扩展DO输出成功，Bit=" + bit + " Value=" + value);
            }

            rtn = mc.GTN_SetDoBit(map.Core, mc.MC_GPO, map.HardwareBit, value);
            if (rtn != 0)
            {
                return HandleError(rtn, "板载DO输出失败，Bit=" + bit);
            }

            return Ok("板载DO输出成功，Bit=" + bit + " Value=" + value);
        }

        public override Result<bool> GetDI(short bit)
        {
            var map = GetDIBitMap(bit);
            if (map == null)
            {
                return Fail<bool>(MotionErrorCode.IoMapNotFound, "DI 映射未找到，Bit=" + bit);
            }

            short rtn;

            if (map.IsExtModule)
            {
                short extValue;
                rtn = mc.GTN_GetExtDiBit(map.Core, map.HardwareBit, out extValue);
                if (rtn != 0)
                {
                    return HandleError<bool>(rtn, "读取扩展DI失败，Bit=" + bit);
                }

                return Ok(extValue != 0, "读取扩展DI成功，Bit=" + bit);
            }

            short value;
            rtn = mc.GTN_GetDiBit(map.Core, mc.MC_GPI, map.HardwareBit, out value);
            if (rtn != 0)
            {
                return HandleError<bool>(rtn, "读取板载DI失败，Bit=" + bit);
            }

            return Ok(value != 0, "读取板载DI成功，Bit=" + bit);
        }

        public override Result<bool> GetDO(short bit)
        {
            var map = GetDOBitMap(bit);
            if (map == null)
            {
                return Fail<bool>(MotionErrorCode.IoMapNotFound, "DO 映射未找到，Bit=" + bit);
            }

            short rtn;

            if (map.IsExtModule)
            {
                short extValue;
                rtn = mc.GTN_GetExtDoBit(map.Core, map.HardwareBit, out extValue);
                if (rtn != 0)
                {
                    return HandleError<bool>(rtn, "读取扩展DO失败，Bit=" + bit);
                }

                return Ok(extValue != 0, "读取扩展DO成功，Bit=" + bit);
            }

            int doValue;
            rtn = mc.GTN_GetDo(map.Core, mc.MC_GPO, out doValue);
            if (rtn != 0)
            {
                return HandleError<bool>(rtn, "读取板载DO失败，Bit=" + bit);
            }

            int mask = 1 << (map.HardwareBit - 1);
            bool value2 = (doValue & mask) != 0;
            return Ok(value2, "读取板载DO成功，Bit=" + bit);
        }

        #endregion

        public override Result SetVel(short logicalAxis, double vel)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            short rtn = mc.GTN_SetVel(core, axis, vel);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置速度失败");

            return Ok($"轴{logicalAxis} 设置速度成功");
        }

        public override Result SetAcc(short logicalAxis, double acc)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            short rtn = mc.GTN_PrfTrap(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 切换Trap模式失败");

            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(core, axis, out trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Trap参数失败");

            trap.acc = acc;
            rtn = mc.GTN_SetTrapPrm(core, axis, ref trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置加速度失败");

            return Ok($"轴{logicalAxis} 设置加速度成功");
        }

        public override Result SetDec(short logicalAxis, double dec)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Result.Fail(cfgResult.Code, cfgResult.Message, ResultSource.Motion);
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            short rtn = mc.GTN_PrfTrap(core, axis);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 切换Trap模式失败");

            mc.TTrapPrm trap;
            rtn = mc.GTN_GetTrapPrm(core, axis, out trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 读取Trap参数失败");

            trap.dec = dec;
            rtn = mc.GTN_SetTrapPrm(core, axis, ref trap);
            if (rtn != 0) return HandleError(rtn, $"轴{logicalAxis} 设置减速度失败");

            return Ok($"轴{logicalAxis} 设置减速度成功");
        }

        public override Result<AxisStatus> GetAxisStatus(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Fail<AxisStatus>(MotionErrorCode.AxisMapNotFound, "逻辑轴 " + logicalAxis + " 映射未找到");
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            uint clock;
            int rawStatus;
            short rtn = mc.GTN_GetStsEx(core, axis, out rawStatus, 1, out clock);
            if (rtn != 0)
            {
                return HandleError<AxisStatus>(rtn, "读取轴" + logicalAxis + "状态字失败");
            }

            short posLimit;
            short negLimit;
            rtn = mc.GTN_GetLimitStatus(core, axis, out posLimit, out negLimit);
            if (rtn != 0)
            {
                return HandleError<AxisStatus>(rtn, "读取轴" + logicalAxis + "限位状态失败");
            }

            var alarmResult = ReadAxisSignal(cfg, mc.MC_ALARM, "报警");
            if (!alarmResult.Success) return Result<AxisStatus>.Fail(alarmResult.Code, alarmResult.Message, ResultSource.Motion);

            var homeResult = ReadAxisSignal(cfg, mc.MC_HOME, "原点");
            if (!homeResult.Success) return Result<AxisStatus>.Fail(homeResult.Code, homeResult.Message, ResultSource.Motion);

            var movingResult = IsMoving(logicalAxis);
            if (!movingResult.Success) return Result<AxisStatus>.Fail(movingResult.Code, movingResult.Message, ResultSource.Motion);

            var status = new AxisStatus
            {
                IsEnabled = cfg.IsServoOn,
                IsAlarm = alarmResult.Item,
                IsAtHome = homeResult.Item,
                PositiveLimit = posLimit != 0,
                NegativeLimit = negLimit != 0,
                IsDone = !movingResult.Item
            };

            return Ok(status, "读取轴" + logicalAxis + "状态成功");
        }

        public override Result<double> GetCommandPosition(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Fail<double>(MotionErrorCode.AxisMapNotFound, $"逻辑轴 {logicalAxis} 映射未找到");
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            uint clock;
            double prfPulse;
            short rtn = mc.GTN_GetAxisPrfPos(core, axis, out prfPulse, 1, out clock);
            if (rtn != 0)
            {
                return HandleError<double>(rtn, $"读取轴{logicalAxis}规划位置失败");
            }

            return Ok(prfPulse, $"读取轴{logicalAxis}规划位置成功");
        }

        public override Result<double> GetEncoderPosition(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Fail<double>(MotionErrorCode.AxisMapNotFound, $"逻辑轴 {logicalAxis} 映射未找到");
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            uint clock;
            double encPulse;
            short rtn = mc.GTN_GetAxisEncPos(core, axis, out encPulse, 1, out clock);
            if (rtn != 0)
            {
                return HandleError<double>(rtn, $"读取轴{logicalAxis}编码器位置失败");
            }

            return Ok(encPulse, $"读取轴{logicalAxis}编码器位置成功");
        }

        public override Result<double> GetCommandPositionMm(short logicalAxis)
        {
            var pulseResult = GetCommandPosition(logicalAxis);
            if (!pulseResult.Success)
            {
                return Result<double>.Fail(pulseResult.Code, pulseResult.Message, ResultSource.Motion);
            }

            var mmResult = PulseToMmResult(logicalAxis, pulseResult.Item);
            if (!mmResult.Success)
            {
                return Result<double>.Fail(mmResult.Code, mmResult.Message, ResultSource.Motion);
            }

            return Ok(mmResult.Item, $"读取轴{logicalAxis}规划位置(mm)成功");
        }

        public override Result<double> GetEncoderPositionMm(short logicalAxis)
        {
            var pulseResult = GetEncoderPosition(logicalAxis);
            if (!pulseResult.Success)
            {
                return Result<double>.Fail(pulseResult.Code, pulseResult.Message, ResultSource.Motion);
            }

            var mmResult = PulseToMmResult(logicalAxis, pulseResult.Item);
            if (!mmResult.Success)
            {
                return Result<double>.Fail(mmResult.Code, mmResult.Message, ResultSource.Motion);
            }

            return Ok(mmResult.Item, $"读取轴{logicalAxis}编码器位置(mm)成功");
        }

        public override Result<bool> IsMoving(short logicalAxis)
        {
            var cfgResult = GetLogicalAxisCfgResult(logicalAxis);
            if (!cfgResult.Success)
                return Fail<bool>(MotionErrorCode.AxisMapNotFound, $"逻辑轴 {logicalAxis} 映射未找到");
            var cfg = cfgResult.Item;
            short core = cfgResult.Item.PhysicalCore;
            short axis = cfgResult.Item.PhysicalAxis;

            uint clock;
            double vel;
            short rtn = mc.GTN_GetPrfVel(core, axis, out vel, 1, out clock);
            if (rtn != 0)
            {
                return HandleError<bool>(rtn, $"读取轴{logicalAxis}速度失败");
            }

            return Ok(Math.Abs(vel) > 0.0001, $"读取轴{logicalAxis}运动状态成功");
        }

        #region 私有方法

        private short GetIoCore()
        {
            return 1;
        }

        private bool IsExtIoBit(short bit)
        {
            return CurrentConfig.UseExtModule && bit > 16;
        }

        private Result WaitStandardHomeDone(AxisConfig cfg)
        {
            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < cfg.HomeTimeoutMs)
            {
                mc.TStandardHomeStatus status;
                short rtn = mc.GTN_GetStandardHomeStatus(cfg.PhysicalCore, cfg.PhysicalAxis, out status);
                if (rtn != 0)
                {
                    return HandleError(rtn, "轴 " + cfg.LogicalAxis + " 读取回零状态失败");
                }

                if (status.run == 0)
                {
                    if (status.error != mc.STANDARD_HOME_ERROR_NONE)
                    {
                        return Fail(MotionErrorCode.HomeFailed,
                            "轴 " + cfg.LogicalAxis + " 回零失败，Stage=" + status.stage + " Error=" + status.error);
                    }

                    if (status.stage == mc.STANDARD_HOME_STAGE_END)
                    {
                        return Ok("轴 " + cfg.LogicalAxis + " 回零成功");
                    }

                    return Fail(MotionErrorCode.HomeFailed,
                        "轴 " + cfg.LogicalAxis + " 回零结束但未到完成阶段，Stage=" + status.stage);
                }

                Thread.Sleep(50);
            }

            Stop(cfg.LogicalAxis, true);
            return Fail(MotionErrorCode.HomeTimeout, "轴 " + cfg.LogicalAxis + " 回零超时");
        }

        private Result<bool> ReadAxisSignal(AxisConfig cfg, short signalType, string signalName)
        {
            short value;
            short rtn = mc.GTN_GetDiBit(cfg.PhysicalCore, signalType, cfg.PhysicalAxis, out value);
            if (rtn != 0)
            {
                return HandleError<bool>(rtn, "读取轴" + cfg.LogicalAxis + signalName + "信号失败");
            }

            return Ok(value != 0, "读取轴" + cfg.LogicalAxis + signalName + "信号成功");
        }

        private MotionIoBitMap GetDIBitMap(short logicalBit)
        {
            return GetIoBitMap(CurrentConfig.DIBitMaps, logicalBit);
        }

        private MotionIoBitMap GetDOBitMap(short logicalBit)
        {
            return GetIoBitMap(CurrentConfig.DOBitMaps, logicalBit);
        }

        private MotionIoBitMap GetIoBitMap(List<MotionIoBitMap> maps, short logicalBit)
        {
            if (logicalBit <= 0)
            {
                return null;
            }

            if (maps != null && maps.Count > 0)
            {
                return maps.FirstOrDefault(p => p.LogicalBit == logicalBit);
            }

            // 兼容旧逻辑：未配置映射时仍按 1~16 板载，17+ 扩展处理
            return new MotionIoBitMap
            {
                LogicalBit = logicalBit,
                Name = "IO" + logicalBit,
                Core = GetIoCore(),
                IsExtModule = IsExtIoBit(logicalBit),
                HardwareBit = (short)(IsExtIoBit(logicalBit) ? logicalBit - 16 : logicalBit)
            };
        }

        #endregion

    }
}
