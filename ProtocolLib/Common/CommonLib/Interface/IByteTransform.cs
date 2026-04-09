using ProtocolLib.CommonLib.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 采集数据转换方式
    /// 常规 字节顺序和电脑一致 小端表示，例如MC
    /// 反转 字节顺序和电脑相反 大端表示，例如S7
    /// 2字节为单位转换，例如Modbus
    /// </summary>
    public interface IByteTransform
    {
        /// <summary>
        /// 数据解析格式
        /// </summary>
        E_DataFormat DataFormat { get; set; }

        /// <summary>
        /// 解析时是否转换字节顺序
        /// </summary>
        bool IsStringReverseByteWord { get; set; }

        /// <summary>
        /// 根据指定的格式来实例化一个新的对象
        /// </summary>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        IByteTransform CreateByDataFormat(E_DataFormat dataFormat);

        #region 把字节数组转换成具体的类型数据

        bool bytes2Bool(byte[] buffer, int index);
        bool[] bytes2Bool(byte[] buffer, int index, int length);

        byte bytes2Byte(byte[] buffer, int index);
        byte[] bytes2Byte(byte[] buffer, int index, int length);

        short bytes2Int16(byte[] buffer, int index);
        short[] bytes2Int16(byte[] buffer, int index, int length);

        ushort bytes2UInt16(byte[] buffer, int index);
        ushort[] bytes2UInt16(byte[] buffer, int index, int length);

        int bytes2Int32(byte[] buffer, int index);
        int[] bytes2Int32(byte[] buffer, int index, int length);

        uint bytes2UInt32(byte[] buffer, int index);
        uint[] bytes2UInt32(byte[] buffer, int index,int length);

        long bytes2Int64(byte[] buffer, int index);
        long[] bytes2Int64(byte[] buffer, int index, int length);

        ulong bytes2UInt64(byte[] buffer, int index);
        ulong[] bytes2UInt64(byte[] buffer, int index, int length);

        float bytes2Single(byte[] buffer, int index);
        float[] bytes2Single(byte[] buffer,int index,int length);

        double bytes2Double(byte[] buffer, int index);
        double[] bytes2Double(byte[] buffer, int index,int length);

        string bytes2String(byte[] buffer, Encoding encoding);
        string bytes2String(byte[] buffer, int index, int length,Encoding encoding);

        #endregion

        #region 数据转为字节数组
        byte[] data2Bytes(bool value);
        byte[] data2Bytes(bool[] value);

        byte[] data2Bytes(byte value);

        byte[] data2Bytes(short value);
        byte[] data2Bytes(short[] value);

        byte[] data2Bytes(ushort value);
        byte[] data2Bytes(ushort[] value);

        byte[] data2Bytes(int value);
        byte[] data2Bytes(int[] value);

        byte[] data2Bytes(uint value);
        byte[] data2Bytes(uint[] value);

        byte[] data2Bytes(long value);
        byte[] data2Bytes(long[] value);

        byte[] data2Bytes(ulong value);
        byte[] data2Bytes(ulong[] value);

        byte[] data2Bytes(float value);
        byte[] data2Bytes(float[] value);

        byte[] data2Bytes(double value);
        byte[] data2Bytes(double[] value);

        byte[] data2Bytes(string value,Encoding encoding);
        byte[] data2Bytes(string value,int length,Encoding encoding);



        #endregion
    }
}
