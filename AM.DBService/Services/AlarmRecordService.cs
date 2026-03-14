using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.DBService.DBase;
using AM.Model.Alarm;
using AM.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.Services
{
    /// <summary>
    /// 报警持久化实现。
    /// </summary>
    public class AlarmRecordService : IAlarmRecord
    {
        private readonly DBCommon<AlarmRecord> _db;
        private readonly IAppReporter _reporter;

        public AlarmRecordService() : this(SystemContext.Instance.Reporter)
        {
        }

        public AlarmRecordService(IAppReporter reporter)
        {
            _db = new DBCommon<AlarmRecord>();
            _reporter = reporter;
        }

        public void SaveRaised(AlarmCode code, AlarmLevel level, string message, string source, short? cardId, DateTime raisedTime)
        {
            var entity = new AlarmRecord
            {
                Code = (int)code,
                Level = (int)level,
                Source = string.IsNullOrWhiteSpace(source) ? "Alarm" : source,
                Message = message ?? string.Empty,
                CardId = cardId,
                RaisedTime = raisedTime,
                IsCleared = false,
                ClearedTime = null
            };

            var result = _db.Add(entity);
            if (!result.Success)
            {
                _reporter?.Error("AlarmPersistence", "报警发生记录持久化失败", result.Code);
            }
        }

        public void SaveCleared(AlarmCode code, DateTime clearedTime)
        {
            var queryResult = _db.QueryAll();
            if (!queryResult.Success)
            {
                _reporter?.Error("AlarmPersistence", "报警清除记录查询失败", queryResult.Code);
                return;
            }

            var targets = queryResult.Items
                .Where(a => a.Code == (int)code && !a.IsCleared)
                .ToList();

            foreach (var item in targets)
            {
                item.IsCleared = true;
                item.ClearedTime = clearedTime;

                var editResult = _db.Edit(item);
                if (!editResult.Success)
                {
                    _reporter?.Error("AlarmPersistence", "报警清除记录持久化失败", editResult.Code);
                }
            }
        }
    }
}
