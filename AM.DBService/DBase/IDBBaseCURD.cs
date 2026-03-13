using AM.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.DBase
{
    public interface IDBBaseCURD<T>
    {
        Result Add(T obj);
        Result Delete(T obj);
        Result Edit(T obj);
        Result<T> QueryAll();
    }
}
