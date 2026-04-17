using AM.Core.Base;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Common;
using AM.Model.Entity.System;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 客户端更新记录服务。
    /// 
    /// 负责：
    /// 1. 将检查、下载、安装、回滚等动作写入本地表；
    /// 2. 为后续上报更新结果提供本地审计记录。
    /// </summary>
    public class ClientUpdateRecordService : ServiceBase
    {
        private readonly DBCommon<SysClientUpdateRecordEntity> _recordDb;
        private readonly ClientIdentityService _clientIdentityService;

        protected override string MessageSourceName
        {
            get { return "ClientUpdateRecordService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Database; }
        }

        public ClientUpdateRecordService()
            : base()
        {
            _recordDb = new DBCommon<SysClientUpdateRecordEntity>();
            _clientIdentityService = new ClientIdentityService();
            EnsureTableSchema();
        }

        public ClientUpdateRecordService(IAppReporter reporter)
            : base(reporter)
        {
            _recordDb = new DBCommon<SysClientUpdateRecordEntity>();
            _clientIdentityService = new ClientIdentityService(reporter);
            EnsureTableSchema();
        }

        /// <summary>
        /// 确保本地更新记录表存在。
        /// 初始化失败仅记录日志，不阻断主流程。
        /// </summary>
        private void EnsureTableSchema()
        {
            try
            {
                _recordDb._sqlSugarClient.CodeFirst.InitTables<SysClientUpdateRecordEntity>();
            }
            catch (Exception ex)
            {
                _reporter?.Error(MessageSourceName, ex, "初始化 sys_client_update_record 表失败", -1, null, ReportChannels.Log);
            }
        }

        /// <summary>
        /// 保存检查更新结果。
        /// </summary>
        public Result SaveCheckResult(string currentVersion, string targetVersion, string channel, string updateStatus, string message)
        {
            return SaveRecord(currentVersion, targetVersion, channel, "Check", updateStatus, message);
        }

        /// <summary>
        /// 保存下载更新包结果。
        /// </summary>
        public Result SaveDownloadResult(string currentVersion, string targetVersion, string channel, string updateStatus, string message)
        {
            return SaveRecord(currentVersion, targetVersion, channel, "Download", updateStatus, message);
        }

        /// <summary>
        /// 保存安装更新结果。
        /// </summary>
        public Result SaveInstallResult(string currentVersion, string targetVersion, string channel, string updateStatus, string message)
        {
            return SaveRecord(currentVersion, targetVersion, channel, "Install", updateStatus, message);
        }

        /// <summary>
        /// 保存回滚结果。
        /// </summary>
        public Result SaveRollbackResult(string currentVersion, string targetVersion, string channel, string updateStatus, string message)
        {
            return SaveRecord(currentVersion, targetVersion, channel, "Rollback", updateStatus, message);
        }

        /// <summary>
        /// 查询未上报记录。
        /// </summary>
        public Result<SysClientUpdateRecordEntity> QueryUnreported(int takeCount)
        {
            try
            {
                if (takeCount <= 0)
                {
                    takeCount = 50;
                }

                List<SysClientUpdateRecordEntity> list = _recordDb._sqlSugarClient
                    .Queryable<SysClientUpdateRecordEntity>()
                    .Where(x => !x.IsReported)
                    .OrderBy(x => x.CreateTime, OrderByType.Asc)
                    .Take(takeCount)
                    .ToList();

                return OkListSilent(list, "查询未上报更新记录成功");
            }
            catch (Exception ex)
            {
                return Fail<SysClientUpdateRecordEntity>(-1, "查询未上报更新记录失败", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 批量标记为已上报。
        /// </summary>
        public Result MarkReported(IEnumerable<int> ids)
        {
            try
            {
                List<int> idList = ids == null
                    ? new List<int>()
                    : ids.Where(x => x > 0).Distinct().ToList();

                if (idList.Count <= 0)
                {
                    return WarnSilent(-1, "没有可更新的更新记录");
                }

                DateTime now = DateTime.Now;

                _recordDb._sqlSugarClient.Ado.BeginTran();

                _recordDb._sqlSugarClient.Updateable<SysClientUpdateRecordEntity>()
                    .Where(x => idList.Contains(x.Id))
                    .SetColumns(x => new SysClientUpdateRecordEntity
                    {
                        IsReported = true,
                        ReportTime = now
                    })
                    .ExecuteCommand();

                _recordDb._sqlSugarClient.Ado.CommitTran();
                return OkSilent("标记更新记录已上报成功");
            }
            catch (Exception ex)
            {
                _recordDb._sqlSugarClient.Ado.RollbackTran();
                return Fail(-1, "标记更新记录已上报失败", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 保存通用更新记录。
        /// </summary>
        public Result SaveRecord(
            string currentVersion,
            string targetVersion,
            string channel,
            string updateAction,
            string updateStatus,
            string message)
        {
            try
            {
                Result<SysClientIdentityEntity> identityResult = _clientIdentityService.GetCurrent();
                if (!identityResult.Success || identityResult.Item == null)
                {
                    return Fail(identityResult.Code == 0 ? -1 : identityResult.Code, "获取客户端身份失败");
                }

                SysClientIdentityEntity identity = identityResult.Item;

                SysClientUpdateRecordEntity entity = new SysClientUpdateRecordEntity
                {
                    ClientId = identity.ClientId ?? string.Empty,
                    AppCode = identity.AppCode ?? string.Empty,
                    MachineCode = identity.MachineCode ?? string.Empty,
                    MachineName = identity.MachineName ?? string.Empty,
                    CurrentVersion = currentVersion ?? string.Empty,
                    TargetVersion = targetVersion ?? string.Empty,
                    Channel = channel ?? string.Empty,
                    UpdateAction = updateAction ?? string.Empty,
                    UpdateStatus = updateStatus ?? string.Empty,
                    Message = message ?? string.Empty,
                    TraceId = AM.Tools.Tools.Guid(16),
                    IsReported = false,
                    ReportTime = null,
                    CreateTime = DateTime.Now
                };

                Result result = _recordDb.Add(entity);
                if (!result.Success)
                {
                    return WarnLogOnly(result.Code, "写入客户端更新记录失败");
                }

                return OkSilent("写入客户端更新记录成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "写入客户端更新记录异常", ReportChannels.Log, ex);
            }
        }
    }
}