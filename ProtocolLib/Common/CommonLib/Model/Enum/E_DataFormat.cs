using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model.Enum
{
    /// <summary>
    /// 字节数据解析
    /// </summary>
    public enum E_DataFormat
    {
        /// <summary>
		/// 按照顺序排序
		/// </summary>
		ABCD = 0,
		/// <summary>
		/// 按照单字反转
		/// </summary>
		BADC = 1,
		/// <summary>
		/// 按照双字反转
		/// </summary>
		CDAB = 2,
		/// <summary>
		/// 按照倒序排序
		/// </summary>
		DCBA = 3,
    }
}
