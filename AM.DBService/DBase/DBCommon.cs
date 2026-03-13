using AM.Model.Common;
using AM.Model.DB;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace AM.DBService.DBase
{
    public class DBCommon<T> : DBContext, IDBBaseCURD<T> where T : class, new()
    {
        public SqlSugarClient _sqlSugarClient;

        public DBCommon()
        {
            _sqlSugarClient = GetClient(new MDBaseSet { DBType = "Sqlite" });
        }

        public Result Add(T obj)
        {
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                _sqlSugarClient.Insertable(obj).ExecuteCommand();
                _sqlSugarClient.Ado.CommitTran();
                return Result.Ok($"Add<{typeof(T).Name}> success", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _sqlSugarClient.Ado.RollbackTran();
                return Result.Fail(-1, $"Add<{typeof(T).Name}> failed {ex.Message}", ResultSource.Database);
            }
        }

        public Result Delete(T obj)
        {
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                _sqlSugarClient.Deleteable(obj).ExecuteCommand();
                _sqlSugarClient.Ado.CommitTran();
                return Result.Ok($"Delete<{typeof(T).Name}> success", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _sqlSugarClient.Ado.RollbackTran();
                return Result.Fail(-1, $"Delete<{typeof(T).Name}> failed {ex.Message}", ResultSource.Database);
            }
        }

        public Result Edit(T obj)
        {
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                _sqlSugarClient.Updateable(obj).ExecuteCommand();
                _sqlSugarClient.Ado.CommitTran();
                return Result.Ok($"Edit<{typeof(T).Name}> success", ResultSource.Database);
            }
            catch (Exception ex)
            {
                _sqlSugarClient.Ado.RollbackTran();
                return Result.Fail(-1, $"Edit<{typeof(T).Name}> failed {ex.Message}", ResultSource.Database);
            }
        }

        public Result<T> QueryAll()
        {
            try
            {
                List<T> list = _sqlSugarClient.Queryable<T>().ToList();
                return Result<T>.OkList(list, $"QueryAll<{typeof(T).Name}> success", ResultSource.Database);
            }
            catch (Exception ex)
            {
                return Result<T>.Fail(-1, $"QueryAll<{typeof(T).Name}> failed {ex.Message}", ResultSource.Database);
            }
        }
    }
}
