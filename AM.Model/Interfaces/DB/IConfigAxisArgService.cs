using AM.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.DB
{
    public interface IConfigAxisArgService
    {
        List<ConfigAxisArg> QueryAll();
        ConfigAxisArg QueryByAxis(int axis);
        bool Save(ConfigAxisArg param);
        bool Delete(int axis,string paramname,string paramname_cn);
    }
}
