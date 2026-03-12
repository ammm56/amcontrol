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
        bool Del();
        List<T> Edit();
        List<T> Query();
    }
}
