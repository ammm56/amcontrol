using ProtocolLib.CommonLib.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 按字节顺序转换数据
    /// </summary>
    public class ReverseWordTransform:ByteTransformBase
    {
        public ReverseWordTransform()
        {
            DataFormat = E_DataFormat.ABCD;
        }
        public ReverseWordTransform(E_DataFormat dataFormat) : base(dataFormat) { }

        /// <summary>
        /// 按双字节排序
        /// </summary>
        /// <param name="buffer">实际的字节数据</param>
        /// <param name="index">起始字节位置</param>
        /// <param name="length">数据长度</param>
        /// <returns></returns>
        private byte[] ReverseBytesByWord(byte[] buffer,int index,int length)
        {
            if (buffer == null) return null;
            return ToolBasic.BytesReverseByWord(buffer.SelectMiddle(index, length));
        }

        public override short bytes2Int16(byte[] buffer, int index)
        {
            return base.bytes2Int16(ReverseBytesByWord(buffer, index, 2), 0);
        }
        public override ushort bytes2UInt16(byte[] buffer, int index)
        {
            return base.bytes2UInt16(ReverseBytesByWord(buffer, index, 2), 0);
        }

        public override byte[] data2Bytes(short[] value)
        {
            byte[] buffer = base.data2Bytes(value);
            return ToolBasic.BytesReverseByWord(buffer);
        }
        public override byte[] data2Bytes(ushort[] value)
        {
            byte[] buffer = base.data2Bytes(value);
            return ToolBasic.BytesReverseByWord(buffer);
        }

        public override string ToString()
        {
            return $"ReverseWordTransform {DataFormat}";
        }

    }
}
