using AM.DBService.DBase;
using AM.Model.Entity;
using AM.Model.Interfaces.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.Services
{
    public class ConfigAxisArgService : IConfigAxisArgService
    {
        private readonly DBCommon<ConfigAxisArg> _db;

        public ConfigAxisArgService()
        {
            _db = new DBCommon<ConfigAxisArg>();
        }

        public List<ConfigAxisArg> QueryAll() => _db.QueryAll();

        public ConfigAxisArg QueryByAxis(int axis)
        {
            return _db.QueryAll().FirstOrDefault(a => a.Axis == axis);
        }

        public bool Save(ConfigAxisArg param)
        {
            if (param.Id > 0)
                return _db.Edit(param);
            else
                return _db.Add(param);
        }

        public bool Delete(int axis, string paramname, string paramname_cn)
        {
            ConfigAxisArg item = _db.QueryAll().FirstOrDefault(a => a.Axis == axis && a.ParamName == paramname && a.ParamName_Cn == paramname_cn);
            if (item == null) return false;
            return _db.Delete(item);
        }
    }
}
