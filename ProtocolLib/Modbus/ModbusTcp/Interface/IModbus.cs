using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.ModbusTcp.Interface
{
    /// <summary>
    /// Modbus设备接口
    /// </summary>
    public interface IModbus:IReadWriteNet
    {
        /// <summary>
        /// 将报文发送到设备，然后从设备接收数据
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        M_OperateResult<byte[]> ReadData(byte[] send);

        /// <summary>
        /// 地址从0开始
        /// </summary>
        bool AddressStartWithZero { get; set; }

        /// <summary>
        /// 站号
        /// </summary>
        byte Station { get; set; }

        /// <summary>
        /// 字节顺序
        /// ABCD BADC CDAB DCBA
        /// </summary>
        E_DataFormat DataFormat { get; set; }

        /// <summary>
        /// 字符串是否按字反转
        /// </summary>
        bool IsStringReverse { get; set; }

    }
}
