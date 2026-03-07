using AM.DB.DBase;
using AM.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DB.Tables
{
    public class DBTable<T> : DBCommon<T> where T : class, new()
    {
    }
}
