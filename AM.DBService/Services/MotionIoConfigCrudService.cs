using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.DB;
using AM.Model.Entity.Motion;
using AM.Model.Interfaces.DB;
using System;

namespace AM.DBService.Services
{
    /// <summary>
    /// IO 配置统一 CRUD 聚合服务。
    /// 聚合 motion_io_map 与 motion_io_point_config 两层配置。
    /// </summary>
    public class MotionIoConfigCrudService : ServiceBase, IMotionIoConfigCrudService
    {
        private readonly IMotionIoMapCrudService _motionIoMapCrudService;
        private readonly IMotionIoPointConfigCrudService _motionIoPointConfigCrudService;

        protected override string MessageSourceName
        {
            get { return "MotionIoConfigCrud"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public MotionIoConfigCrudService()
            : this(
                new MotionIoMapCrudService(),
                new MotionIoPointConfigCrudService(),
                SystemContext.Instance.Reporter)
        {
        }

        public MotionIoConfigCrudService(
            IMotionIoMapCrudService motionIoMapCrudService,
            IMotionIoPointConfigCrudService motionIoPointConfigCrudService,
            IAppReporter reporter)
            : base(reporter)
        {
            _motionIoMapCrudService = motionIoMapCrudService;
            _motionIoPointConfigCrudService = motionIoPointConfigCrudService;
        }

        public Result<MotionIoMapEntity> QueryAllMaps()
        {
            return _motionIoMapCrudService.QueryAll();
        }

        public Result<MotionIoPointConfigEntity> QueryAllPointConfigs()
        {
            return _motionIoPointConfigCrudService.QueryAll();
        }

        public Result<MotionIoMapEntity> QueryMapByLogicalBit(short logicalBit, string ioType)
        {
            return _motionIoMapCrudService.QueryByLogicalBit(logicalBit, ioType);
        }

        public Result<MotionIoPointConfigEntity> QueryPointConfigByLogicalBit(short logicalBit, string ioType)
        {
            return _motionIoPointConfigCrudService.QueryByLogicalBit(logicalBit, ioType);
        }

        public Result Save(MotionIoMapEntity ioMapEntity, MotionIoPointConfigEntity pointConfigEntity)
        {
            if (ioMapEntity == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "IO映射不能为空");
            }

            if (pointConfigEntity != null)
            {
                pointConfigEntity.IoType = ioMapEntity.IoType;
                pointConfigEntity.LogicalBit = ioMapEntity.LogicalBit;
            }

            var saveMapResult = _motionIoMapCrudService.Save(ioMapEntity);
            if (!saveMapResult.Success)
            {
                return Fail(saveMapResult.Code, saveMapResult.Message);
            }

            if (pointConfigEntity == null)
            {
                return Ok("IO配置保存成功");
            }

            var savePointResult = _motionIoPointConfigCrudService.Save(pointConfigEntity);
            if (!savePointResult.Success)
            {
                return Fail(savePointResult.Code, "IO映射已保存，但点位公共配置保存失败");
            }

            return Ok("IO配置保存成功");
        }

        public Result DeleteByLogicalBit(short logicalBit, string ioType)
        {
            var deletePointResult = _motionIoPointConfigCrudService.DeleteByLogicalBit(logicalBit, ioType);
            if (!deletePointResult.Success && deletePointResult.Code != (int)DbErrorCode.NotFound)
            {
                return Fail(deletePointResult.Code, deletePointResult.Message);
            }

            var deleteMapResult = _motionIoMapCrudService.DeleteByLogicalBit(logicalBit, ioType);
            if (!deleteMapResult.Success)
            {
                return Fail(deleteMapResult.Code, deleteMapResult.Message);
            }

            return Ok("IO配置删除成功");
        }
    }
}