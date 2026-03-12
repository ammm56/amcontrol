using AM.Model.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.DBase
{
    public interface IDBContext
    {
        SqlSugarClient GetClient(MDBaseSet mDBaseSet);
    }
}
