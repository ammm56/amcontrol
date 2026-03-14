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
    /// <summary>
    /// 轴参数配置服务。
    /// 负责轴参数的查询、保存、删除等业务操作。
    /// </summary>
    public class ConfigAxisArgService : ServiceBase, IConfigAxisArgService
    {
        /// <summary>
        /// 通用数据库访问对象。
        /// </summary>
        private readonly DBCommon<ConfigAxisArg> _db;

        /// <summary>
        /// 消息来源名称。
        /// </summary>
        protected override string MessageSourceName
        {
            get { return "DB"; }
        }

        /// <summary>
        /// 默认结果来源。
        /// </summary>
        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        /// <summary>
        /// 使用全局系统上下文中的统一报告器初始化服务。
        /// </summary>
        public ConfigAxisArgService(): this(SystemContext.Instance.Reporter)
        {
        }

        /// <summary>
        /// 使用指定统一报告器初始化服务。
        /// </summary>
        /// <param name="reporter">统一报告器。</param>
        public ConfigAxisArgService(IAppReporter reporter): base(reporter)
        {
            _db = new DBCommon<ConfigAxisArg>();
        }


        /// <summary>
        /// 查询全部轴参数。
        /// </summary>
        /// <returns>带轴参数集合的统一结果对象。</returns>
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

        /// <summary>
        /// 按轴号查询参数。
        /// </summary>
        /// <param name="axis">轴号。</param>
        /// <returns>带单项轴参数的统一结果对象。</returns>
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

        /// <summary>
        /// 保存轴参数。
        /// </summary>
        /// <param name="param">待保存参数。</param>
        /// <returns>统一结果对象。</returns>
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

        /// <summary>
        /// 删除轴参数。
        /// </summary>
        /// <param name="axis">轴号。</param>
        /// <param name="paramname">参数英文名称。</param>
        /// <param name="paramname_cn">参数中文名称。</param>
        /// <returns>统一结果对象。</returns>
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
