using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.DBService.DBase;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Entity.Dev;
using System;
using System.Collections.Generic;
using System.Data;

namespace AM.DBService.Services.Dev
{
    /// <summary>
    /// 报警持久化与查询服务。
    /// 实现 IDevAlarmRecord，将报警发生/清除记录写入 SQLite 并提供分页查询。
    ///
    /// 设计约束：写入路径只使用 IAMLogger，不使用 IAppReporter,
    ///   规避 AlarmManager → IDevAlarmRecord → Reporter → AlarmManager 的循环调用。
    /// Logger 通过 SystemContext 惰性获取：服务构造时 SystemContext 尚未初始化,
    ///   但 SaveRaised/SaveCleared 被调用时已初始化完成，因此惰性访问是安全的。
    /// </summary>
    public class DevAlarmRecordService : IDevAlarmRecord
    {
        private readonly DBCommon<DevAlarmRecordEntity> _db;

        /// <summary>
        /// 惰性获取日志器，规避构造时 SystemContext 未就绪的问题。
        /// </summary>
        private IAMLogger Logger
        {
            get
            {
                try { return SystemContext.Instance.Logger; }
                catch { return null; }
            }
        }

        public DevAlarmRecordService()
        {
            _db = new DBCommon<DevAlarmRecordEntity>();
            EnsureTableSchema();
        }

        // ── 表结构初始化与自动迁移 ────────────────────────────────────────

        /// <summary>
        /// 确保表结构与当前实体定义一致。
        /// 若表不存在则建表；若检测到任何已知 schema 问题（旧版缺 IsNullable 或缺列），
        /// 则自动重建表（开发阶段旧数据会丢失，生产环境应使用正式迁移脚本）。
        /// </summary>
        private void EnsureTableSchema()
        {
            try
            {
                var client = _db._sqlSugarClient;

                if (!client.DbMaintenance.IsAnyTable("dev_alarm_record", false))
                {
                    client.CodeFirst.InitTables<DevAlarmRecordEntity>();
                    return;
                }

                // CardId NOT NULL（旧版缺 IsNullable 标注） 或 Description 列不存在（新增字段）→ 重建
                var needRebuild = IsColumnNotNull(client, "dev_alarm_record", "CardId")
                               || !HasColumn(client, "dev_alarm_record", "Description");

                if (needRebuild)
                {
                    LogWarn("dev_alarm_record 表 schema 与当前实体不一致，将重建表（开发阶段旧数据会丢失）。");
                    client.DbMaintenance.DropTable("dev_alarm_record");
                    client.CodeFirst.InitTables<DevAlarmRecordEntity>();
                    LogWarn("dev_alarm_record 表已按最新 schema 重建完成。");
                    return;
                }

                client.CodeFirst.InitTables<DevAlarmRecordEntity>();
            }
            catch (Exception ex)
            {
                LogError("dev_alarm_record 表 schema 初始化失败，后续写入可能失败", ex);
            }
        }

        /// <summary>
        /// 通过 SQLite PRAGMA table_info 检测指定列是否有 NOT NULL 约束。
        /// </summary>
        private bool IsColumnNotNull(SqlSugar.SqlSugarClient client, string tableName, string columnName)
        {
            try
            {
                var dt = client.Ado.GetDataTable("PRAGMA table_info('" + tableName + "')");
                foreach (DataRow row in dt.Rows)
                {
                    var col = row["name"]?.ToString() ?? string.Empty;
                    if (string.Equals(col, columnName, StringComparison.OrdinalIgnoreCase))
                        return (row["notnull"]?.ToString() ?? "0") == "1";
                }
            }
            catch (Exception ex)
            {
                LogError("PRAGMA table_info('" + tableName + "') 检查失败", ex);
            }

            return false;
        }

        /// <summary>
        /// 通过 SQLite PRAGMA table_info 检测指定列是否存在。
        /// </summary>
        private bool HasColumn(SqlSugar.SqlSugarClient client, string tableName, string columnName)
        {
            try
            {
                var dt = client.Ado.GetDataTable("PRAGMA table_info('" + tableName + "')");
                foreach (DataRow row in dt.Rows)
                {
                    var col = row["name"]?.ToString() ?? string.Empty;
                    if (string.Equals(col, columnName, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            catch (Exception ex)
            {
                LogError("PRAGMA table_info('" + tableName + "') HasColumn 检查失败", ex);
            }

            return false;
        }

        // ── 写入（IAlarmRecord） ──────────────────────────────────────────

        /// <summary>写入报警触发记录。失败只写日志，不影响主报警流程。</summary>
        public void SaveRaised(
            AlarmCode code, AlarmLevel level,
            string message, string source,
            short? cardId, DateTime raisedTime,
            string description, string suggestion)
        {
            try
            {
                var result = _db.Add(new DevAlarmRecordEntity
                {
                    AlarmCode = (int)code,
                    AlarmLevel = level.ToString(),
                    Message = message,
                    Source = source,
                    CardId = cardId,
                    RaisedTime = raisedTime,
                    IsCleared = false,
                    Description = description,
                    Suggestion = suggestion
                });

                if (!result.Success)
                {
                    LogError("SaveRaised 写入失败 [Code=" + code + "]: " + result.Message);
                }
            }
            catch (Exception ex)
            {
                LogError("SaveRaised 发生异常 [Code=" + code + "]", ex);
            }
        }

        /// <summary>将指定报警码所有未清除记录标记为已清除。失败只写日志。</summary>
        public void SaveCleared(AlarmCode code, DateTime clearedTime)
        {
            try
            {
                var client = _db._sqlSugarClient;

                var records = client.Queryable<DevAlarmRecordEntity>()
                    .Where(x => x.AlarmCode == (int)code && !x.IsCleared)
                    .ToList();

                if (records == null || records.Count == 0)
                    return;

                foreach (var rec in records)
                {
                    rec.IsCleared = true;
                    rec.ClearedTime = clearedTime;

                    int affected = client.Updateable(rec).ExecuteCommand();
                    if (affected <= 0)
                    {
                        LogWarn("SaveCleared 更新无生效行 [Id=" + rec.Id + ", Code=" + code + "]");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("SaveCleared 发生异常 [Code=" + code + "]", ex);
            }
        }

        // ── 查询（IDevAlarmRecord 扩展）───────────────────────────────────

        /// <summary>分页查询报警记录，按触发时间倒序。</summary>
        public Result<DevAlarmRecordEntity> QueryPage(
            int page, int pageSize,
            string levelFilter = null,
            bool? isCleared = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 20;

                var query = _db._sqlSugarClient.Queryable<DevAlarmRecordEntity>();

                if (!string.IsNullOrWhiteSpace(levelFilter))
                    query = query.Where(x => x.AlarmLevel == levelFilter);

                if (isCleared.HasValue)
                    query = query.Where(x => x.IsCleared == isCleared.Value);

                if (from.HasValue)
                    query = query.Where(x => x.RaisedTime >= from.Value);

                if (to.HasValue)
                    query = query.Where(x => x.RaisedTime <= to.Value);

                var items = query
                    .OrderByDescending(x => x.RaisedTime)
                    .ToPageList(page, pageSize);

                return Result<DevAlarmRecordEntity>.OkList(items, "报警记录查询成功")
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                LogError("QueryPage 发生异常", ex);
                return Result<DevAlarmRecordEntity>.Fail(-1, "报警记录分页查询失败: " + ex.Message);
            }
        }

        /// <summary>查询满足过滤条件的总条数，供分页控件计算页数。</summary>
        public int QueryTotalCount(
            string levelFilter = null,
            bool? isCleared = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            try
            {
                var query = _db._sqlSugarClient.Queryable<DevAlarmRecordEntity>();

                if (!string.IsNullOrWhiteSpace(levelFilter))
                    query = query.Where(x => x.AlarmLevel == levelFilter);

                if (isCleared.HasValue)
                    query = query.Where(x => x.IsCleared == isCleared.Value);

                if (from.HasValue)
                    query = query.Where(x => x.RaisedTime >= from.Value);

                if (to.HasValue)
                    query = query.Where(x => x.RaisedTime <= to.Value);

                return query.Count();
            }
            catch (Exception ex)
            {
                LogError("QueryTotalCount 发生异常", ex);
                return 0;
            }
        }

        /// <summary>按主键查询单条报警记录（用于行选中后显示详情）。</summary>
        public Result<DevAlarmRecordEntity> QueryById(int id)
        {
            try
            {
                var item = _db._sqlSugarClient.Queryable<DevAlarmRecordEntity>()
                    .First(x => x.Id == id);

                if (item == null)
                    return Result<DevAlarmRecordEntity>.Fail(-1, "报警记录不存在: Id=" + id);

                return Result<DevAlarmRecordEntity>.OkItem(item)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                LogError("QueryById 发生异常 [Id=" + id + "]", ex);
                return Result<DevAlarmRecordEntity>.Fail(-1, "报警记录查询失败: " + ex.Message);
            }
        }

        // ── 日志辅助（只用 IAMLogger，不走 IAppReporter） ──────────────────

        private void LogWarn(string message)
        {
            try { Logger?.Warn("[DevAlarmRecordService] " + message); }
            catch { }
        }

        private void LogError(string message, Exception ex = null)
        {
            try
            {
                if (ex != null)
                    Logger?.Error(ex, "[DevAlarmRecordService] " + message);
                else
                    Logger?.Error("[DevAlarmRecordService] " + message);
            }
            catch { }
        }
    }
}
