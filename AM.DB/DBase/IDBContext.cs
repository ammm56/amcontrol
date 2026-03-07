using AM.Model.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DB.DBase
{
    public interface IDBContext
    {
        SqlSugarClient GetClient(MDBaseSet mDBaseSet);
    }
}
