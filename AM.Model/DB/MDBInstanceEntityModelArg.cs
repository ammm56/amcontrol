using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.DB
{
    /// <summary>
    /// 数据库生成实体 模型参数
    /// </summary>
    public class MDBInstanceEntityModelArg
    {
        /// <summary>
        /// 生成的实体名 对应的单个表名 或所有表名 *
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 生成的模型名 对应的单个表名 或所有表名 *
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 数据库类型 MySQL SqlServer
        /// </summary>
        public string DBaseType { get; set; }
    }
}
