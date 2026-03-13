using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Model.Common;
using AM.MotionService;
using AM.MotionService.Factory;
using AM.Tools;
using AM.Tools.Logging;
using AM.Tools.Messaging;
using AM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.App.Bootstrap
{
    /// <summary>
    /// 系统初始化
    /// 系统生命周期
    /// 服务注册
    /// </summary>
    public static class AppBootstrap
    {
        public static void Initialize()
        {
            // 全局单例-配置上下文初始化
            var result = Tools.Tools.ReadConfig<Config>("config.json");
            Config config = result.Item1 ? result.Item2 : new Config();
            ConfigContext.Instance.Initialize(config);

            // 全局单例-系统上下文初始化
            IAMLogger logger = new NLogLogger("System");
            IMessageBus msgbus = new MessageBusToolkit();
            SystemContext.Instance.Initialize(logger, msgbus);

            // 初始化硬件
            InitializeMachine();
        }

        private static void InitializeMachine()
        {
            var motionCfg = ConfigContext.Instance.Config.MotionCardConfig;
            var motion = MotionCardFactory.Create(motionCfg);
            // 启动阶段统一加载轴映射配置
            motion.LoadAxisConfig(motionCfg.AxisConfigs);

            MachineContext.Instance.MotionCard = motion;
        }
    }

}
