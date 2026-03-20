using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.App;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.MotionService.Factory;
using AM.MotionService.Hub;
using System.Linq;

namespace AM.DBService.Services.Motion.App
{
    /// <summary>
    /// 设备配置热重载服务。
    /// 负责刷新 ConfigContext 并重建 MachineContext。
    /// </summary>
    public class MachineConfigReloadService : ServiceBase, IMachineConfigReloadService
    {
        private readonly IMachineConfigAppService _machineConfigAppService;

        protected override string MessageSourceName
        {
            get { return "MachineConfigReload"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MachineConfigReloadService()
            : this(new MachineConfigAppService(), SystemContext.Instance.Reporter)
        {
        }

        public MachineConfigReloadService(
            IMachineConfigAppService machineConfigAppService,
            IAppReporter reporter)
            : base(reporter)
        {
            _machineConfigAppService = machineConfigAppService;
        }

        public Result ReloadFromDatabase()
        {
            return _machineConfigAppService.ReloadFromDatabase();
        }

        public Result RebuildMachineContext()
        {
            var machine = MachineContext.Instance;
            var cardConfigs = ConfigContext.Instance.Config.MotionCardsConfig;

            ResetMachineContext(machine);

            if (cardConfigs == null || cardConfigs.Count == 0)
            {
                machine.MotionHub = new MotionServiceHub();
                return Ok("当前无运动控制卡配置，设备上下文已清空");
            }

            foreach (var motionCfg in cardConfigs.Where(p => p != null))
            {
                var buildResult = RegisterMotionCard(machine, motionCfg);
                if (!buildResult.Success)
                {
                    ResetMachineContext(machine);
                    machine.MotionHub = new MotionServiceHub();
                    return buildResult;
                }
            }

            machine.MotionHub = new MotionServiceHub();
            return Ok("设备上下文重建成功");
        }

        public Result ReloadAndRebuild()
        {
            var reloadResult = ReloadFromDatabase();
            if (!reloadResult.Success)
            {
                return reloadResult;
            }

            return RebuildMachineContext();
        }

        private static void ResetMachineContext(MachineContext machine)
        {
            machine.MotionCards.Clear();
            machine.AxisMotionCards.Clear();
            machine.DICards.Clear();
            machine.DOCards.Clear();
            machine.MotionHub = null;
        }

        private Result RegisterMotionCard(MachineContext machine, MotionCardConfig motionCfg)
        {
            if (motionCfg == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "存在空的运动控制卡配置");
            }

            if (machine.MotionCards.ContainsKey(motionCfg.CardId))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "控制卡 CardId 重复: " + motionCfg.CardId);
            }

            IMotionCardService motion = MotionCardFactory.Create(motionCfg);
            motion.LoadAxisConfig(motionCfg.AxisConfigs);

            machine.MotionCards[motionCfg.CardId] = motion;

            var axisResult = RegisterAxisMappings(machine, motion, motionCfg);
            if (!axisResult.Success)
            {
                return axisResult;
            }

            var diResult = RegisterDIMappings(machine, motion, motionCfg);
            if (!diResult.Success)
            {
                return diResult;
            }

            var doResult = RegisterDOMappings(machine, motion, motionCfg);
            if (!doResult.Success)
            {
                return doResult;
            }

            return Ok("控制卡注册成功");
        }

        private Result RegisterAxisMappings(MachineContext machine, IMotionCardService motion, MotionCardConfig motionCfg)
        {
            if (motionCfg.AxisConfigs == null)
            {
                return Ok("无轴映射");
            }

            foreach (var axisCfg in motionCfg.AxisConfigs.Where(p => p != null))
            {
                if (machine.AxisMotionCards.ContainsKey(axisCfg.LogicalAxis))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "逻辑轴重复映射: " + axisCfg.LogicalAxis);
                }

                machine.AxisMotionCards[axisCfg.LogicalAxis] = motion;
            }

            return Ok("轴映射注册成功");
        }

        private Result RegisterDIMappings(MachineContext machine, IMotionCardService motion, MotionCardConfig motionCfg)
        {
            if (motionCfg.DIBitMaps == null)
            {
                return Ok("无DI映射");
            }

            foreach (var di in motionCfg.DIBitMaps.Where(p => p != null))
            {
                if (machine.DICards.ContainsKey(di.LogicalBit))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "逻辑DI重复映射: " + di.LogicalBit);
                }

                machine.DICards[di.LogicalBit] = motion;
            }

            return Ok("DI映射注册成功");
        }

        private Result RegisterDOMappings(MachineContext machine, IMotionCardService motion, MotionCardConfig motionCfg)
        {
            if (motionCfg.DOBitMaps == null)
            {
                return Ok("无DO映射");
            }

            foreach (var dob in motionCfg.DOBitMaps.Where(p => p != null))
            {
                if (machine.DOCards.ContainsKey(dob.LogicalBit))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "逻辑DO重复映射: " + dob.LogicalBit);
                }

                machine.DOCards[dob.LogicalBit] = motion;
            }

            return Ok("DO映射注册成功");
        }
    }
}