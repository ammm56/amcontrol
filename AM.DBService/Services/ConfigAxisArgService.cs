using AM.Core.Alarm;
using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.Entity;
using AM.Model.Interfaces.DB;
using System;
using System.Linq;

namespace AM.DBService.Services
{
    public class ConfigAxisArgService : ServiceBase, IConfigAxisArgService
    {
        private readonly DBCommon<ConfigAxisArg> _db;

        protected override string MessageSourceName
        {
            get { return "DB"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public ConfigAxisArgService(): this(
            SystemContext.Instance.MessageBus,
            SystemContext.Instance.Logger,
            SystemContext.Instance.AlarmManager,
            SystemContext.Instance.Reporter)
        {
        }

        public ConfigAxisArgService(IMessageBus msgbus, IAMLogger logger, AlarmManager alarmManager, IAppReporter reporter): base(msgbus, logger, alarmManager, reporter)
        {
            _db = new DBCommon<ConfigAxisArg>();
        }

        public Result<ConfigAxisArg> QueryAll()
        {
            try
            {
                var result = _db.QueryAll();
                if (!result.Success)
                {
                    return Fail<ConfigAxisArg>(result.Code, "轴参数查询失败");
                }

                return OkList(result.Items, "轴参数查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<ConfigAxisArg>(ex, (int)DbErrorCode.QueryFailed, "轴参数查询失败");
            }
        }

        public Result<ConfigAxisArg> QueryByAxis(int axis)
        {
            try
            {
                if (axis <= 0)
                {
                    return Fail<ConfigAxisArg>((int)DbErrorCode.InvalidArgument, "轴号参数无效");
                }

                var queryResult = _db.QueryAll();
                if (!queryResult.Success)
                {
                    return Fail<ConfigAxisArg>(queryResult.Code, "按轴查询失败");
                }

                var item = queryResult.Items.FirstOrDefault(a => a.Axis == axis);
                if (item == null)
                {
                    return Warn<ConfigAxisArg>((int)DbErrorCode.NotFound, "未找到对应轴参数");
                }

                return Ok(item, "轴参数查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<ConfigAxisArg>(ex, (int)DbErrorCode.QueryFailed, "按轴查询失败");
            }
        }

        public Result Save(ConfigAxisArg param)
        {
            try
            {
                if (param == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "保存参数不能为空");
                }

                var result = param.Id > 0 ? _db.Edit(param) : _db.Add(param);
                if (!result.Success)
                {
                    return Fail(result.Code, "轴参数保存失败");
                }

                return Ok("轴参数保存成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.SaveFailed, "轴参数保存异常");
            }
        }

        public Result Delete(int axis, string paramname, string paramname_cn)
        {
            try
            {
                if (axis <= 0 || string.IsNullOrWhiteSpace(paramname) || string.IsNullOrWhiteSpace(paramname_cn))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "删除参数无效");
                }

                var queryResult = _db.QueryAll();
                if (!queryResult.Success)
                {
                    return Fail(queryResult.Code, "删除查询失败");
                }

                var item = queryResult.Items.FirstOrDefault(a =>
                    a.Axis == axis &&
                    a.ParamName == paramname &&
                    a.ParamName_Cn == paramname_cn);

                if (item == null)
                {
                    return Warn((int)DbErrorCode.NotFound, "删除目标不存在");
                }

                var result = _db.Delete(item);
                if (!result.Success)
                {
                    return Fail(result.Code, "轴参数删除失败");
                }

                return Ok("轴参数删除成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.DeleteFailed, "轴参数删除异常");
            }
        }
    }
}
