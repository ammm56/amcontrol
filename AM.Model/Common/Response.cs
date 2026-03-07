using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Common
{
    /// <summary>
    /// 返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
    }

    /// <summary>
    /// 数据库返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DBResponse<T>:Response<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool status { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string message { get; set; }

    }
}
