using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.Entity;
using AM.Model.Interfaces.DB;
using System;
using System.Linq;

namespace AM.DBService.Services
{
    public class ConfigAxisArgService : IConfigAxisArgService
    {
        private readonly IMessageBus _messageBus;
        private readonly IAMLogger _logger;
        private readonly AlarmManager _alarmManager;
        private readonly DBCommon<ConfigAxisArg> _db;

        public ConfigAxisArgService()
            : this(SystemContext.Instance.MessageBus, SystemContext.Instance.Logger, SystemContext.Instance.AlarmManager)
        {
        }

        public ConfigAxisArgService(IMessageBus msgbus, IAMLogger logger, AlarmManager alarmManager)
        {
            _messageBus = msgbus;
            _logger = logger;
            _alarmManager = alarmManager;
            _db = new DBCommon<ConfigAxisArg>();
        }

        public Result<ConfigAxisArg> QueryAll()
        {
            try
            {
                var result = _db.QueryAll();
                return Result<ConfigAxisArg>.OkList(result.Items.ToList(),"轴参数查询成功",ResultSource.Database);
            }
            catch (Exception ex)
            {
                PublishError("QueryAll<ConfigAxisArg> failed", ex, DbErrorCode.QueryFailed);
                return Result<ConfigAxisArg>.Fail((int)DbErrorCode.QueryFailed,"轴参数查询失败",ResultSource.Database);
            }
        }

        public Result<ConfigAxisArg> QueryByAxis(int axis)
        {
            try
            {
                if (axis <= 0)
                {
                    return Result<ConfigAxisArg>.Fail((int)DbErrorCode.InvalidArgument,"轴号参数无效",ResultSource.Database);
                }

                var queryResult = _db.QueryAll();
                if (!queryResult.Success)
                {
                    PublishWarning("QueryByAxis(" + axis + ") query failed: " + queryResult.Message, DbErrorCode.QueryFailed);
                    return Result<ConfigAxisArg>.Fail(queryResult.Code, "按轴查询失败", ResultSource.Database);
                }

                var item = queryResult.Items.FirstOrDefault(a => a.Axis == axis);
                if (item == null)
                {
                    PublishWarning("QueryByAxis(" + axis + ") not found", DbErrorCode.NotFound);
                    return Result<ConfigAxisArg>.Fail((int)DbErrorCode.NotFound,"未找到对应轴参数",ResultSource.Database);
                }

                return Result<ConfigAxisArg>.OkItem(item,"轴参数查询成功",ResultSource.Database);
            }
            catch (Exception ex)
            {
                PublishError("QueryByAxis(" + axis + ") failed", ex, DbErrorCode.QueryFailed);
                return Result<ConfigAxisArg>.Fail((int)DbErrorCode.QueryFailed,"按轴查询失败",ResultSource.Database);
            }
        }

        public Result Save(ConfigAxisArg param)
        {
            try
            {
                if (param == null)
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument,"保存参数不能为空",ResultSource.Database);
                }

                var result = param.Id > 0 ? _db.Edit(param) : _db.Add(param);

                if (!result.Success)
                {
                    PublishError("Save(" + param.Axis + " 轴 " + param.ParamName + ") failed", null, DbErrorCode.SaveFailed);
                    return Result.Fail(result.Code, "轴参数保存失败", ResultSource.Database);
                }

                PublishStatus("Save(" + param.Axis + " 轴 " + param.ParamName + ") success");
                return Result.Ok("轴参数保存成功", ResultSource.Database);
            }
            catch (Exception ex)
            {
                var axisText = param == null ? "null" : param.Axis.ToString();
                var nameText = param == null ? "null" : param.ParamName;
                PublishError("Save(" + axisText + " 轴 " + nameText + ") failed", ex, DbErrorCode.SaveFailed);

                return Result.Fail((int)DbErrorCode.SaveFailed,"轴参数保存异常",ResultSource.Database);
            }
        }

        public Result Delete(int axis, string paramname, string paramname_cn)
        {
            try
            {
                if (axis <= 0 || string.IsNullOrWhiteSpace(paramname) || string.IsNullOrWhiteSpace(paramname_cn))
                {
                    return Result.Fail((int)DbErrorCode.InvalidArgument,"删除参数无效",ResultSource.Database);
                }

                var queryResult = _db.QueryAll();
                if (!queryResult.Success)
                {
                    PublishWarning("Delete(" + axis + " 轴 " + paramname + ") query failed: " + queryResult.Message, DbErrorCode.QueryFailed);
                    return Result.Fail(queryResult.Code, "删除查询失败", ResultSource.Database);
                }

                var item = queryResult.Items.FirstOrDefault(a =>
                    a.Axis == axis &&
                    a.ParamName == paramname &&
                    a.ParamName_Cn == paramname_cn);

                if (item == null)
                {
                    PublishWarning("Delete(" + axis + " 轴 " + paramname + ") target not found", DbErrorCode.NotFound);
                    return Result.Fail((int)DbErrorCode.NotFound, "删除目标不存在", ResultSource.Database);
                }

                var result = _db.Delete(item);
                if (!result.Success)
                {
                    PublishError("Delete(" + axis + " 轴 " + paramname + ") failed", null, DbErrorCode.DeleteFailed);
                    return Result.Fail((int)DbErrorCode.DeleteFailed,"轴参数删除失败",ResultSource.Database);
                }

                PublishStatus("Delete(" + axis + " 轴 " + paramname + ") success");
                return Result.Ok("轴参数删除成功", ResultSource.Database);
            }
            catch (Exception ex)
            {
                PublishError("Delete(" + axis + " 轴 " + paramname + ") failed", ex, DbErrorCode.DeleteFailed);
                return Result.Fail((int)DbErrorCode.DeleteFailed,"轴参数删除异常",ResultSource.Database);
            }
        }

        private void PublishStatus(string message)
        {
            _logger.Info(message);
            _messageBus.Publish(new SystemMessage(
                message,
                SystemMessageType.Status,
                "DB",
                ((int)DbErrorCode.Success).ToString(),
                null));
        }

        private void PublishWarning(string message, DbErrorCode code)
        {
            _logger.Warn(message);
            _messageBus.Publish(new SystemMessage(
                message,
                SystemMessageType.Warning,
                "DB",
                ((int)code).ToString(),
                null));
        }

        private void PublishError(string message, Exception ex, DbErrorCode code)
        {
            if (ex == null)
                _logger.Error(message);
            else
                _logger.Error(ex, message);

            _messageBus.Publish(new SystemMessage(
                message,
                SystemMessageType.Error,
                "DB",
                ((int)code).ToString(),
                null));
        }
    }
}
