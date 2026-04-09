using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 字节倒序的转换类
    /// </summary>
    public class ReverseBytesTransform: ByteTransformBase
    {
        public ReverseBytesTransform() { }
        public ReverseBytesTransform(E_DataFormat dataFormat) : base(dataFormat) { }

        #region 从字节数组获得值
        public override short bytes2Int16(byte[] buffer, int index)
        {
            byte[] tmp = new byte[2];
            tmp[0] = buffer[1 + index];
            tmp[1] = buffer[0 + index];
            return BitConverter.ToInt16(tmp, 0);
        }

        public override ushort bytes2UInt16(byte[] buffer, int index)
        {
            byte[] tmp = new byte[2];
            tmp[0] = buffer[1 + index];
            tmp[1] = buffer[0 + index];
            return BitConverter.ToUInt16(tmp, 0);
        }

        public override int bytes2Int32(byte[] buffer, int index)
        {
            byte[] tmp = new byte[4];
            tmp[0] = buffer[3 + index];
            tmp[1] = buffer[2 + index];
            tmp[2] = buffer[1 + index];
            tmp[3] = buffer[0 + index];
            return BitConverter.ToInt32(ByteTransDataFormat4(tmp), 0);
        }

        public override uint bytes2UInt32(byte[] buffer, int index)
        {
            byte[] tmp = new byte[4];
            tmp[0] = buffer[3 + index];
            tmp[1] = buffer[2 + index];
            tmp[2] = buffer[1 + index];
            tmp[3] = buffer[0 + index];
            return BitConverter.ToUInt32(ByteTransDataFormat4(tmp), 0);
        }

        public override long bytes2Int64(byte[] buffer, int index)
        {
            byte[] tmp = new byte[8];
            tmp[0] = buffer[7 + index];
            tmp[1] = buffer[6 + index];
            tmp[2] = buffer[5 + index];
            tmp[3] = buffer[4 + index];
            tmp[4] = buffer[3 + index];
            tmp[5] = buffer[2 + index];
            tmp[6] = buffer[1 + index];
            tmp[7] = buffer[0 + index];
            return BitConverter.ToInt64(ByteTransDataFormat8(tmp), 0);
        }

        public override ulong bytes2UInt64(byte[] buffer, int index)
        {
            byte[] tmp = new byte[8];
            tmp[0] = buffer[7 + index];
            tmp[1] = buffer[6 + index];
            tmp[2] = buffer[5 + index];
            tmp[3] = buffer[4 + index];
            tmp[4] = buffer[3 + index];
            tmp[5] = buffer[2 + index];
            tmp[6] = buffer[1 + index];
            tmp[7] = buffer[0 + index];
            return BitConverter.ToUInt64(ByteTransDataFormat8(tmp), 0);
        }

        public override float bytes2Single(byte[] buffer, int index)
        {
            byte[] tmp = new byte[4];
            tmp[0] = buffer[3 + index];
            tmp[1] = buffer[2 + index];
            tmp[2] = buffer[1 + index];
            tmp[3] = buffer[0 + index];
            return BitConverter.ToSingle(ByteTransDataFormat4(tmp), 0);
        }

        public override double bytes2Double(byte[] buffer, int index)
        {
            byte[] tmp = new byte[8];
            tmp[0] = buffer[7 + index];
            tmp[1] = buffer[6 + index];
            tmp[2] = buffer[5 + index];
            tmp[3] = buffer[4 + index];
            tmp[4] = buffer[3 + index];
            tmp[5] = buffer[2 + index];
            tmp[6] = buffer[1 + index];
            tmp[7] = buffer[0 + index];
            return BitConverter.ToDouble(ByteTransDataFormat8(tmp), 0);
        }

        public virtual string bytes2String(byte[] buffer, int index, int length, Encoding encoding)
        {
            byte[] tmp = bytes2Byte(buffer, index, length);
            if (IsStringReverseByteWord)
                return encoding.GetString(ToolBasic.BytesReverseByWord(tmp));
            else
                return encoding.GetString(tmp);
        }

        public virtual string bytes2String(byte[] buffer, Encoding encoding) => encoding.GetString(buffer);


        #endregion

        #region 把值转成字节数组
        public override byte[] data2Bytes(short[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                tmp.CopyTo(buffer, 2 * i);
            }

            return buffer;
        }
        public override byte[] data2Bytes(ushort[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                tmp.CopyTo(buffer, 2 * i);
            }

            return buffer;
        }
        public override byte[] data2Bytes(int[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat4(tmp).CopyTo(buffer, 4 * i);
            }

            return buffer;
        }
        public override byte[] data2Bytes(uint[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat4(tmp).CopyTo(buffer, 4 * i);
            }

            return buffer;
        }
        public override byte[] data2Bytes(long[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat8(tmp).CopyTo(buffer, 8 * i);
            }

            return buffer;
        }
        public override byte[] data2Bytes(ulong[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat8(tmp).CopyTo(buffer, 8 * i);
            }

            return buffer;
        }
        public override byte[] data2Bytes(float[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat4(tmp).CopyTo(buffer, 4 * i);
            }

            return buffer;
        }
        public override byte[] data2Bytes(double[] values)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes(values[i]);
                Array.Reverse(tmp);
                ByteTransDataFormat8(tmp).CopyTo(buffer, 8 * i);
            }

            return buffer;
        }

        #endregion

        public override IByteTransform CreateByDataFormat(E_DataFormat dataFormat) => new ReverseBytesTransform(dataFormat) { IsStringReverseByteWord = this.IsStringReverseByteWord };

        public override string ToString()
        {
            return $"ReverseBytesTransform[{DataFormat}]";
        }
    }
}
