using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 地址
    /// </summary>
    public class M_DeviceAddressBase
    {
        public ushort Address { get; set; }

        /// <summary>
		/// 解析字符串的地址
		/// </summary>
		/// <param name="address">地址信息</param>
		public virtual void Parse(string address)
        {
            Address = ushort.Parse(address);
        }

        public override string ToString() => Address.ToString();
    }
}
