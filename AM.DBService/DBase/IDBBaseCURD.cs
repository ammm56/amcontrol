using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.DBase
{
    public interface IDBBaseCURD<T>
    {
        bool Add(T obj);
        bool Delete(T obj);
        bool Edit(T obj);
        List<T> QueryAll();
    }
}
