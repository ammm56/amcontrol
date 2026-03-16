using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.Mappers;
using AM.Model.Common;
using AM.Model.Entity;
using AM.Model.Interfaces.DB;
using AM.Model.MotionCard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 轴配置覆盖服务。
    /// 负责将数据库中的 ConfigAxisArg 参数覆盖到 MotionCardConfig.AxisConfigs。
    /// </summary>
    public class AxisConfigOverlayService : ServiceBase, IAxisConfigOverlayService
    {
        private readonly IConfigAxisArgService _configAxisArgService;

        /// <summary>
        /// 消息来源名称。
        /// </summary>
        protected override string MessageSourceName
        {
            get { return "AxisConfigOverlay"; }
        }

        /// <summary>
        /// 默认结果来源。
        /// </summary>
        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        /// <summary>
        /// 使用全局上下文初始化。
        /// </summary>
        public AxisConfigOverlayService() : this(new ConfigAxisArgService(), SystemContext.Instance.Reporter)
        {
        }

        /// <summary>
        /// 使用指定依赖初始化。
        /// </summary>
        public AxisConfigOverlayService(IConfigAxisArgService configAxisArgService, IAppReporter reporter) : base(reporter)
        {
            _configAxisArgService = configAxisArgService;
        }

        /// <summary>
        /// 将数据库参数覆盖到多张运动控制卡配置。
        /// </summary>
        public Result ApplyToMotionCards(IList<MotionCardConfig> motionCards)
        {
            if (motionCards == null)
                return Fail((int)DbErrorCode.InvalidArgument, "运动控制卡配置集合不能为空");

            foreach (var motionCard in motionCards)
            {
                if (motionCard == null) continue;

                var result = ApplyToAxisConfigs(motionCard.AxisConfigs);
                if (!result.Success)
                    return result;
            }

            return Ok("数据库轴参数覆盖完成");
        }

        /// <summary>
        /// 将数据库参数覆盖到轴配置集合。
        /// </summary>
        public Result ApplyToAxisConfigs(IList<AxisConfig> axisConfigs)
        {
            if (axisConfigs == null)
                return Fail((int)DbErrorCode.InvalidArgument, "轴配置集合不能为空");

            var queryResult = _configAxisArgService.QueryAll();
            if (!queryResult.Success)
                return Fail(queryResult.Code, "读取数据库轴参数失败");

            var groupedArgs = queryResult.Items
                .GroupBy(p => p.Axis)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var axisConfig in axisConfigs)
            {
                if (axisConfig == null) continue;

                var axisKey = axisConfig.LogicalAxis > 0 ? axisConfig.LogicalAxis : axisConfig.AxisId;

                List<ConfigAxisArg> args;
                if (!groupedArgs.TryGetValue(axisKey, out args))
                {
                    // 兼容：若数据库仍使用 AxisId 存储，则再尝试 AxisId
                    if (axisConfig.AxisId > 0 && groupedArgs.TryGetValue(axisConfig.AxisId, out args))
                    {
                        AxisConfigMapper.Apply(axisConfig, args);
                    }

                    continue;
                }

                AxisConfigMapper.Apply(axisConfig, args);
            }

            return Ok("轴配置覆盖成功");
        }
    }
}