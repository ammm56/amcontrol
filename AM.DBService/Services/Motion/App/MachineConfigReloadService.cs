using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.App;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.Model.MotionCard.Actuator;
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
            var actuatorConfig = ConfigContext.Instance.Config.ActuatorConfig;

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

            var actuatorResult = RegisterActuatorConfig(machine, actuatorConfig);
            if (!actuatorResult.Success)
            {
                ResetMachineContext(machine);
                machine.MotionHub = new MotionServiceHub();
                return actuatorResult;
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
            machine.Cylinders.Clear();
            machine.Vacuums.Clear();
            machine.StackLights.Clear();
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

        private Result RegisterActuatorConfig(MachineContext machine, ActuatorConfig actuatorConfig)
        {
            if (actuatorConfig == null)
            {
                return Ok("无执行器对象配置");
            }

            var cylinderResult = RegisterCylinderMappings(machine, actuatorConfig);
            if (!cylinderResult.Success)
            {
                return cylinderResult;
            }

            var vacuumResult = RegisterVacuumMappings(machine, actuatorConfig);
            if (!vacuumResult.Success)
            {
                return vacuumResult;
            }

            var stackLightResult = RegisterStackLightMappings(machine, actuatorConfig);
            if (!stackLightResult.Success)
            {
                return stackLightResult;
            }

            return Ok("执行器对象配置注册成功");
        }

        private Result RegisterCylinderMappings(MachineContext machine, ActuatorConfig actuatorConfig)
        {
            if (actuatorConfig.Cylinders == null)
            {
                return Ok("无气缸对象配置");
            }

            foreach (var cylinder in actuatorConfig.Cylinders.Where(p => p != null && p.IsEnabled))
            {
                if (string.IsNullOrWhiteSpace(cylinder.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "存在未配置名称的气缸对象");
                }

                if (machine.Cylinders.ContainsKey(cylinder.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸名称重复: " + cylinder.Name);
                }

                if (!machine.DOCards.ContainsKey(cylinder.ExtendOutputBit))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸伸出输出位未注册到 DO 路由: " + cylinder.ExtendOutputBit);
                }

                if (cylinder.RetractOutputBit.HasValue && !machine.DOCards.ContainsKey(cylinder.RetractOutputBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸缩回输出位未注册到 DO 路由: " + cylinder.RetractOutputBit.Value);
                }

                if (cylinder.ExtendFeedbackBit.HasValue && !machine.DICards.ContainsKey(cylinder.ExtendFeedbackBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸伸出反馈位未注册到 DI 路由: " + cylinder.ExtendFeedbackBit.Value);
                }

                if (cylinder.RetractFeedbackBit.HasValue && !machine.DICards.ContainsKey(cylinder.RetractFeedbackBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "气缸缩回反馈位未注册到 DI 路由: " + cylinder.RetractFeedbackBit.Value);
                }

                machine.Cylinders[cylinder.Name] = cylinder;
            }

            return Ok("气缸对象配置注册成功");
        }

        private Result RegisterVacuumMappings(MachineContext machine, ActuatorConfig actuatorConfig)
        {
            if (actuatorConfig.Vacuums == null)
            {
                return Ok("无真空对象配置");
            }

            foreach (var vacuum in actuatorConfig.Vacuums.Where(p => p != null && p.IsEnabled))
            {
                if (string.IsNullOrWhiteSpace(vacuum.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "存在未配置名称的真空对象");
                }

                if (machine.Vacuums.ContainsKey(vacuum.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空名称重复: " + vacuum.Name);
                }

                if (!machine.DOCards.ContainsKey(vacuum.VacuumOnOutputBit))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空吸附输出位未注册到 DO 路由: " + vacuum.VacuumOnOutputBit);
                }

                if (vacuum.BlowOffOutputBit.HasValue && !machine.DOCards.ContainsKey(vacuum.BlowOffOutputBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空破真空输出位未注册到 DO 路由: " + vacuum.BlowOffOutputBit.Value);
                }

                if (vacuum.VacuumFeedbackBit.HasValue && !machine.DICards.ContainsKey(vacuum.VacuumFeedbackBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空建立反馈位未注册到 DI 路由: " + vacuum.VacuumFeedbackBit.Value);
                }

                if (vacuum.ReleaseFeedbackBit.HasValue && !machine.DICards.ContainsKey(vacuum.ReleaseFeedbackBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空释放反馈位未注册到 DI 路由: " + vacuum.ReleaseFeedbackBit.Value);
                }

                if (vacuum.WorkpiecePresentBit.HasValue && !machine.DICards.ContainsKey(vacuum.WorkpiecePresentBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "真空工件检测位未注册到 DI 路由: " + vacuum.WorkpiecePresentBit.Value);
                }

                machine.Vacuums[vacuum.Name] = vacuum;
            }

            return Ok("真空对象配置注册成功");
        }

        private Result RegisterStackLightMappings(MachineContext machine, ActuatorConfig actuatorConfig)
        {
            if (actuatorConfig.StackLights == null)
            {
                return Ok("无灯塔对象配置");
            }

            foreach (var stackLight in actuatorConfig.StackLights.Where(p => p != null && p.IsEnabled))
            {
                if (string.IsNullOrWhiteSpace(stackLight.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "存在未配置名称的灯塔对象");
                }

                if (machine.StackLights.ContainsKey(stackLight.Name))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔名称重复: " + stackLight.Name);
                }

                if (stackLight.RedOutputBit.HasValue && !machine.DOCards.ContainsKey(stackLight.RedOutputBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔红灯输出位未注册到 DO 路由: " + stackLight.RedOutputBit.Value);
                }

                if (stackLight.YellowOutputBit.HasValue && !machine.DOCards.ContainsKey(stackLight.YellowOutputBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔黄灯输出位未注册到 DO 路由: " + stackLight.YellowOutputBit.Value);
                }

                if (stackLight.GreenOutputBit.HasValue && !machine.DOCards.ContainsKey(stackLight.GreenOutputBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔绿灯输出位未注册到 DO 路由: " + stackLight.GreenOutputBit.Value);
                }

                if (stackLight.BlueOutputBit.HasValue && !machine.DOCards.ContainsKey(stackLight.BlueOutputBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔蓝灯输出位未注册到 DO 路由: " + stackLight.BlueOutputBit.Value);
                }

                if (stackLight.BuzzerOutputBit.HasValue && !machine.DOCards.ContainsKey(stackLight.BuzzerOutputBit.Value))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "灯塔蜂鸣器输出位未注册到 DO 路由: " + stackLight.BuzzerOutputBit.Value);
                }

                machine.StackLights[stackLight.Name] = stackLight;
            }

            return Ok("灯塔对象配置注册成功");
        }


    }
}