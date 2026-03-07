using AM.Model.DB;
using AM.Tools;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace AM.DB.DBase
{
    public class DBCommon<T> : DBContext, IDBBaseCURD<T> where T : class, new()
    {
        public SqlSugarClient _sqlSugarClient;

        public DBCommon()
        {
            _sqlSugarClient = GetClient(new MDBaseSet { DBType = "Sqlite" });
        }
        /// <summary>
        /// 单个添加
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Add(T obj)
        {
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                _sqlSugarClient.Insertable(obj).ExecuteCommand();
                _sqlSugarClient.Ado.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                Tools.Tools.Print($"DBCommon Add EX {ex.Message}");
                return false;
            }
        }

        public bool Del()
        {
            throw new NotImplementedException();
        }

        public List<T> Edit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<T> Query()
        {
            return _sqlSugarClient.Queryable<T>().ToList();
        }
    }
}
