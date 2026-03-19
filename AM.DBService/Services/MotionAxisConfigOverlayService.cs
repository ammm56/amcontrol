using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.Mappers;
using AM.Model.Common;
using AM.Model.Entity.Motion;
using AM.Model.Interfaces.DB;
using AM.Model.MotionCard;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services
{
    /// <summary>
    /// 将数据库中的 motion_axis_config 覆盖到运行时 AxisConfig。
    /// </summary>
    public class MotionAxisConfigOverlayService : ServiceBase, IMotionAxisConfigOverlayService
    {
        private readonly IMotionAxisConfigService _motionAxisConfigService;

        protected override string MessageSourceName
        {
            get { return "MotionAxisConfigOverlay"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionAxisConfigOverlayService()
            : this(new MotionAxisConfigService(), SystemContext.Instance.Reporter)
        {
        }

        public MotionAxisConfigOverlayService(
            IMotionAxisConfigService motionAxisConfigService,
            IAppReporter reporter)
            : base(reporter)
        {
            _motionAxisConfigService = motionAxisConfigService;
        }

        public Result ApplyToMotionCards(IList<MotionCardConfig> motionCards)
        {
            if (motionCards == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "运动控制卡配置集合不能为空");
            }

            var axisConfigs = motionCards
                .Where(p => p != null && p.AxisConfigs != null)
                .SelectMany(p => p.AxisConfigs)
                .Where(p => p != null)
                .ToList();

            return ApplyToAxisConfigs(axisConfigs);
        }

        public Result ApplyToAxisConfigs(IList<AxisConfig> axisConfigs)
        {
            if (axisConfigs == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "轴配置集合不能为空");
            }

            var queryResult = _motionAxisConfigService.QueryAll();
            if (!queryResult.Success)
            {
                return Fail(queryResult.Code, "读取数据库轴参数失败");
            }

            var groupedRows = queryResult.Items
                .GroupBy(p => p.LogicalAxis)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var axisConfig in axisConfigs)
            {
                List<MotionAxisConfigEntity> rows;
                if (!groupedRows.TryGetValue(axisConfig.LogicalAxis, out rows))
                {
                    continue;
                }

                MotionAxisConfigMapper.Apply(axisConfig, rows);
            }

            return Ok("数据库轴参数覆盖成功");
        }
    }
}