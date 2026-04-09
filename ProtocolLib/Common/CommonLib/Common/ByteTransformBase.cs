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
    /// 基础数据转换类
    /// </summary>
    public class ByteTransformBase:IByteTransform
    {
        public ByteTransformBase()
        {
            DataFormat = E_DataFormat.DCBA;
        }
        public ByteTransformBase(E_DataFormat dataFormat)
        {
            DataFormat = dataFormat;
        }

        /// <summary>
        /// 重排字节顺序
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected byte[] ByteTransDataFormat4(byte[] value,int index = 0)
        {
            byte[] buffer = new byte[4];
            switch (DataFormat)
            {
                case E_DataFormat.ABCD:
                    {
                        buffer[0] = value[index + 3];
                        buffer[1] = value[index + 2];
                        buffer[2] = value[index + 1];
                        buffer[3] = value[index + 0];
                        break;
                    }
                case E_DataFormat.BADC:
                    {
                        buffer[0] = value[index + 2];
                        buffer[1] = value[index + 3];
                        buffer[2] = value[index + 0];
                        buffer[3] = value[index + 1];
                        break;
                    }
                case E_DataFormat.CDAB:
                    {
                        buffer[0] = value[index + 1];
                        buffer[1] = value[index + 0];
                        buffer[2] = value[index + 3];
                        buffer[3] = value[index + 2];
                        break;
                    }
                case E_DataFormat.DCBA:
                    {
                        buffer[0] = value[index + 0];
                        buffer[1] = value[index + 1];
                        buffer[2] = value[index + 2];
                        buffer[3] = value[index + 3];
                        break;
                    }
            }
            return buffer;
        }
        /// <summary>
        /// 重排字节顺序
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected byte[] ByteTransDataFormat8(byte[] value, int index = 0)
        {
            byte[] buffer = new byte[8];
            switch (DataFormat)
            {
                case E_DataFormat.ABCD:
                    {
                        buffer[0] = value[index + 7];
                        buffer[1] = value[index + 6];
                        buffer[2] = value[index + 5];
                        buffer[3] = value[index + 4];
                        buffer[4] = value[index + 3];
                        buffer[5] = value[index + 2];
                        buffer[6] = value[index + 1];
                        buffer[7] = value[index + 0];
                        break;
                    }
                case E_DataFormat.BADC:
                    {
                        buffer[0] = value[index + 6];
                        buffer[1] = value[index + 7];
                        buffer[2] = value[index + 4];
                        buffer[3] = value[index + 5];
                        buffer[4] = value[index + 2];
                        buffer[5] = value[index + 3];
                        buffer[6] = value[index + 0];
                        buffer[7] = value[index + 1];
                        break;
                    }

                case E_DataFormat.CDAB:
                    {
                        buffer[0] = value[index + 1];
                        buffer[1] = value[index + 0];
                        buffer[2] = value[index + 3];
                        buffer[3] = value[index + 2];
                        buffer[4] = value[index + 5];
                        buffer[5] = value[index + 4];
                        buffer[6] = value[index + 7];
                        buffer[7] = value[index + 6];
                        break;
                    }
                case E_DataFormat.DCBA:
                    {
                        buffer[0] = value[index + 0];
                        buffer[1] = value[index + 1];
                        buffer[2] = value[index + 2];
                        buffer[3] = value[index + 3];
                        buffer[4] = value[index + 4];
                        buffer[5] = value[index + 5];
                        buffer[6] = value[index + 6];
                        buffer[7] = value[index + 7];
                        break;
                    }
            }
            return buffer;
        }

        #region 从字节数组获得值

        public virtual bool bytes2Bool(byte[] buffer, int index) => (buffer[index] & 0x01) == 0x01;
        public bool[] bytes2Bool(byte[] buffer,int index,int length)
        {
            byte[] temp = new byte[length];
            Array.Copy(buffer, index, temp, 0, length);
            return ToolBasic.ByteToBoolArray(temp, length * 8);
        }
        public virtual byte bytes2Byte(byte[] buffer, int index) => buffer[index];
        public virtual byte[] bytes2Byte(byte[] buffer,int index,int length)
        {
            byte[] temp = new byte[length];
            Array.Copy(buffer, index, temp, 0, length);
            return temp;
        }
        public virtual short bytes2Int16(byte[] buffer, int index) => BitConverter.ToInt16(buffer, index);
        public virtual short[] bytes2Int16(byte[] buffer,int index,int length)
        {
            short[] temp = new short[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2Int16(buffer, index + 2 * i);
            }
            return temp;
        }
        public virtual ushort bytes2UInt16(byte[] buffer, int index) => BitConverter.ToUInt16(buffer, index);
        public virtual ushort[] bytes2UInt16(byte[] buffer, int index, int length)
        {
            ushort[] temp = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2UInt16(buffer, index + 2 * i);
            }
            return temp;
        }
        public virtual int bytes2Int32(byte[] buffer, int index) => BitConverter.ToInt32(ByteTransDataFormat4(buffer, index), 0);
        public virtual int[] bytes2Int32(byte[] buffer, int index, int length)
        {
            int[] temp = new int[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2Int32(buffer, index + 4 * i);
            }
            return temp;
        }
        public virtual uint bytes2UInt32(byte[] buffer, int index) => BitConverter.ToUInt32(ByteTransDataFormat4(buffer, index), 0);
        public virtual uint[] bytes2UInt32(byte[] buffer, int index, int length)
        {
            uint[] temp = new uint[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2UInt32(buffer, index + 4 * i);
            }
            return temp;
        }
        public virtual long bytes2Int64(byte[] buffer, int index) => BitConverter.ToInt64(ByteTransDataFormat8(buffer, index), 0);
        public virtual long[] bytes2Int64(byte[] buffer, int index, int length)
        {
            long[] temp = new long[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2Int64(buffer, index + 8 * i);
            }
            return temp;
        }
        public virtual ulong bytes2UInt64(byte[] buffer, int index) => BitConverter.ToUInt64(ByteTransDataFormat8(buffer, index), 0);
        public virtual ulong[] bytes2UInt64(byte[] buffer, int index, int length)
        {
            ulong[] temp = new ulong[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2UInt64(buffer, index + 8 * i);
            }
            return temp;
        }
        public virtual float bytes2Single(byte[] buffer, int index) => BitConverter.ToSingle(ByteTransDataFormat4(buffer, index), 0);
        public virtual float[] bytes2Single(byte[] buffer, int index, int length)
        {
            float[] temp = new float[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2Single(buffer, index + 4 * i);
            }
            return temp;
        }
        public virtual double bytes2Double(byte[] buffer, int index) => BitConverter.ToDouble(ByteTransDataFormat8(buffer, index), 0);
        public virtual double[] bytes2Double(byte[] buffer, int index, int length)
        {
            double[] temp = new double[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = bytes2Double(buffer, index + 8 * i);
            }
            return temp;
        }
        public virtual string bytes2String(byte[] buffer, Encoding encoding) => encoding.GetString(buffer);
        public virtual string bytes2String(byte[] buffer,int index,int length,Encoding encoding)
        {
            byte[] temp = bytes2Byte(buffer, index, length);
            if (IsStringReverseByteWord)
            {
                return encoding.GetString(ToolBasic.BytesReverseByWord(temp));
            }
            else
            {
                return encoding.GetString(temp);
            }
        }

        #endregion

        #region 把值转为字节数组

        public virtual byte[] data2Bytes(bool value) => data2Bytes(new bool[] { value });
        public virtual byte[] data2Bytes(bool[] value) => value == null ? null : ToolBasic.BoolArrayToByte(value);
        public virtual byte[] data2Bytes(byte value) => new byte[] { value };
        public virtual byte[] data2Bytes(short value) => data2Bytes(new short[] { value });
        public virtual byte[] data2Bytes(short[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 2];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, 2 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(ushort value) => data2Bytes(new ushort[] { value });
        public virtual byte[] data2Bytes(ushort[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 2];
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i]).CopyTo(buffer, 2 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(int value) => data2Bytes(new int[] { value });
        public virtual byte[] data2Bytes(int[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 4];
            for (int i = 0; i < value.Length; i++)
            {
                ByteTransDataFormat4(BitConverter.GetBytes(value[i])).CopyTo(buffer, 4 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(uint value) => data2Bytes(new uint[] { value });
        public virtual byte[] data2Bytes(uint[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 4];
            for (int i = 0; i < value.Length; i++)
            {
                ByteTransDataFormat4(BitConverter.GetBytes(value[i])).CopyTo(buffer, 4 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(long value) => data2Bytes(new long[] { value });
        public virtual byte[] data2Bytes(long[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 8];
            for (int i = 0; i < value.Length; i++)
            {
                ByteTransDataFormat8(BitConverter.GetBytes(value[i])).CopyTo(buffer, 8 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(ulong value) => data2Bytes(new ulong[] { value });
        public virtual byte[] data2Bytes(ulong[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 8];
            for (int i = 0; i < value.Length; i++)
            {
                ByteTransDataFormat8(BitConverter.GetBytes(value[i])).CopyTo(buffer, 8 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(float value) => data2Bytes(new float[] { value });
        public virtual byte[] data2Bytes(float[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 4];
            for (int i = 0; i < value.Length; i++)
            {
                ByteTransDataFormat4(BitConverter.GetBytes(value[i])).CopyTo(buffer, 4 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(double value) => data2Bytes(new double[] { value });
        public virtual byte[] data2Bytes(double[] value)
        {
            if (value == null) return null;
            byte[] buffer = new byte[value.Length * 8];
            for (int i = 0; i < value.Length; i++)
            {
                ByteTransDataFormat8(BitConverter.GetBytes(value[i])).CopyTo(buffer, 8 * i);
            }
            return buffer;
        }
        public virtual byte[] data2Bytes(string value,Encoding encoding)
        {
            if (value == null) return null;
            byte[] buffer = encoding.GetBytes(value);
            return IsStringReverseByteWord ? ToolBasic.BytesReverseByWord(buffer) : buffer;
        }
        public virtual byte[] data2Bytes(string value,int length,Encoding encoding)
        {
            if (value == null) return null;
            byte[] buffer = encoding.GetBytes(value);
            if (IsStringReverseByteWord)
            {
                return ToolBasic.ArrayExpandToLength(ToolBasic.BytesReverseByWord(buffer), length);
            }
            else
            {
                return ToolBasic.ArrayExpandToLength(buffer, length);
            }
        }



        #endregion

        public E_DataFormat DataFormat { get; set; }
        public bool IsStringReverseByteWord { get; set; }

        public virtual IByteTransform CreateByDataFormat(E_DataFormat dataFormat)
        {
            return this;
        }

        public override string ToString()
        {
            return $"ByteTransformBase {DataFormat}";
        }

    }
}
