using AM.Core.Alarm;
using AM.DBService.DBase;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Entity.Dev;
using System;
using System.Collections.Generic;

namespace AM.DBService.Services.Dev
{
    /// <summary>
    /// 报警持久化与查询服务。
    /// 实现 IDevAlarmRecord，将报警发生/清除记录写入 SQLite 并提供分页查询。
    ///
    /// 重要：写入方法（SaveRaised/SaveCleared）不使用 IAppReporter,
    ///   避免 AlarmManager → IDevAlarmRecord → Reporter → AlarmManager 的循环调用。
    /// 写入异常一律静默，持久化失败不影响主报警流程。
    /// 查询方法通过 Result 返回错误，由调用方决定是否展示。
    /// </summary>
    public class DevAlarmRecordService : IDevAlarmRecord
    {
        private readonly DBCommon<DevAlarmRecordEntity> _db;

        public DevAlarmRecordService()
        {
            _db = new DBCommon<DevAlarmRecordEntity>();
            try
            {
                _db._sqlSugarClient.CodeFirst.InitTables<DevAlarmRecordEntity>();
            }
            catch { /* 表初始化失败不影响应用启动 */ }
        }

        // ── 写入（IAlarmRecord） ──────────────────────────────────────────

        /// <summary>写入报警触发记录。</summary>
        public void SaveRaised(
            AlarmCode code, AlarmLevel level,
            string message, string source,
            short? cardId, DateTime raisedTime)
        {
            try
            {
                _db.Add(new DevAlarmRecordEntity
                {
                    AlarmCode  = (int)code,
                    AlarmLevel = level.ToString(),
                    Message    = message,
                    Source     = source,
                    CardId     = cardId,
                    RaisedTime = raisedTime,
                    IsCleared  = false
                });
            }
            catch { }
        }

        /// <summary>将指定报警码所有未清除记录标记为已清除。</summary>
        public void SaveCleared(AlarmCode code, DateTime clearedTime)
        {
            try
            {
                var client = _db._sqlSugarClient;
                var records = client.Queryable<DevAlarmRecordEntity>()
                    .Where(x => x.AlarmCode == (int)code && !x.IsCleared)
                    .ToList();

                foreach (var rec in records)
                {
                    rec.IsCleared   = true;
                    rec.ClearedTime = clearedTime;
                    client.Updateable(rec).ExecuteCommand();
                }
            }
            catch { }
        }

        // ── 查询（IDevAlarmRecord 扩展）───────────────────────────────────

        /// <summary>分页查询报警记录，按触发时间倒序。</summary>
        public Result<DevAlarmRecordEntity> QueryPage(
            int page, int pageSize,
            string levelFilter = null,
            bool? isCleared    = null,
            DateTime? from     = null,
            DateTime? to       = null)
        {
            try
            {
                if (page < 1)     page     = 1;
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

                // SqlSugar ToPageList：page 从 1 开始
                var items = query
                    .OrderByDescending(x => x.RaisedTime)
                    .ToPageList(page, pageSize);

                return Result<DevAlarmRecordEntity>.OkList(items, "报警记录查询成功")
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<DevAlarmRecordEntity>.Fail(-1, "报警记录分页查询失败: " + ex.Message);
            }
        }

        /// <summary>查询满足过滤条件的总条数，供分页控件计算页数。</summary>
        public int QueryTotalCount(
            string levelFilter = null,
            bool? isCleared    = null,
            DateTime? from     = null,
            DateTime? to       = null)
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
            catch
            {
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
                return Result<DevAlarmRecordEntity>.Fail(-1, "报警记录查询失败: " + ex.Message);
            }
        }
    }
}
