using AM.Model.DB;
using AM.Tools;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DB.DBase
{
    public class DBContext : IDBContext
    {
        private MDBaseSet _mDBaseSet;
        private string _connectionStr;
        private readonly string _conStr;
        private readonly string _conMySQLStr;
        private DbType _dbType;

        public DBContext()
        {
            _conStr = ConfigSingle.Instance.Config.Sqlite.Connection;
            _conMySQLStr = ConfigSingle.Instance.Config.Sqlite.Connection;
        }

        public SqlSugarClient GetClient(MDBaseSet mDBaseSet)
        {
            _mDBaseSet = mDBaseSet;
            switch (_mDBaseSet.DBType)
            {
                case "Sqlite":
                    _connectionStr = _conStr;
                    _dbType = DbType.Sqlite;
                    break;
                case "MySQL":
                    _connectionStr = _conMySQLStr;
                    _dbType = DbType.MySql;
                    break;
                default:
                    _connectionStr = "";
                    _dbType = DbType.Sqlite;
                    break;
            }
            SqlSugarClient _sqlSugarClient = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = _connectionStr,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            if (ConfigSingle.Instance.Config.Setting.DBLogExec)
            {
                _sqlSugarClient.Aop.OnLogExecuting = (sql, pars) =>
                {
                    string execinfo = $"SQL执行语句 {sql} 参数 {_sqlSugarClient.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value))} !";
                };
                _sqlSugarClient.Aop.OnLogExecuted = (sql, pars) =>
                {
                };
                DBContextLog(_mDBaseSet.DBType);
            }

            return _sqlSugarClient;
        }

        private void DBContextLog(string connectionType)
        {
            string dbexeinfo = $"{connectionType} 连接字符串 {_connectionStr} !";
        }
    }
}
