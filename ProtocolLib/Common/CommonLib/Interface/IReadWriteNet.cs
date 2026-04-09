using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 交互类统一读写标准
    /// </summary>
    public interface IReadWriteNet
    {

        /// <summary>
        /// 当前连接的唯一ID号
        /// </summary>
        string ConnectionId { get; set; }

        /// <summary>
        /// 读取字节数组，返回原始字节数组
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        M_OperateResult<byte[]> Read(string address, ushort length);
        /// <summary>
        /// 写入字节数组到指定地址，返回是否写入成功
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        M_OperateResult Write(string address, byte[] value);

        #region 读取
        M_OperateResult<bool> ReadBool(string address);
        M_OperateResult<bool[]> ReadBool(string address, ushort length);
        M_OperateResult<short> ReadInt16(string address);
        M_OperateResult<short[]> ReadInt16(string address, ushort length);
        M_OperateResult<ushort> ReadUInt16(string address);
        M_OperateResult<ushort[]> ReadUInt16(string address, ushort length);
        M_OperateResult<int> ReadInt32(string address);
        M_OperateResult<int[]> ReadInt32(string address, ushort length);
        M_OperateResult<uint> ReadUInt32(string address);
        M_OperateResult<uint[]> ReadUInt32(string address, ushort length);  
        M_OperateResult<long> ReadInt64(string address);
        M_OperateResult<long[]> ReadInt64(string address, ushort length);
        M_OperateResult<ulong> ReadUInt64(string address);
        M_OperateResult<ulong[]> ReadUInt64(string address, ushort length);
        M_OperateResult<float> ReadFloat(string address);
        M_OperateResult<float[]> ReadFloat(string address, ushort length);
        M_OperateResult<double> ReadDouble(string address);
        M_OperateResult<double[]> ReadDouble(string address, ushort length);
        M_OperateResult<string> ReadString(string address, ushort length);

        #endregion

        #region 写入

        M_OperateResult Write(string address, bool value);
        M_OperateResult Write(string address, bool[] value);
        M_OperateResult Write(string address, short value);
        M_OperateResult Write(string address, short[] value);
        M_OperateResult Write(string address, ushort value);
        M_OperateResult Write(string address, ushort[] value);
        M_OperateResult Write(string address, int value);
        M_OperateResult Write(string address, int[] value);
        M_OperateResult Write(string address, uint value);
        M_OperateResult Write(string address, uint[] value);
        M_OperateResult Write(string address, long value);
        M_OperateResult Write(string address, long[] value);
        M_OperateResult Write(string address, ulong value);
        M_OperateResult Write(string address, ulong[] value);
        M_OperateResult Write(string address, float value);
        M_OperateResult Write(string address, float[] value);
        M_OperateResult Write(string address, double value);
        M_OperateResult Write(string address, double[] value);
        M_OperateResult Write(string address, string value);
        M_OperateResult Write(string address, string value, Encoding encoding);
        M_OperateResult Write(string address, string value, int length);
        M_OperateResult Write(string address, string value, int length, Encoding encoding);


        #endregion

        #region 等待
        /// <summary>
        /// 等待指定地址的值为指定值
        /// </summary>
        /// <param name="address">等待地址</param>
        /// <param name="waitValue">指定值</param>
        /// <param name="readInterval">读取频率</param>
        /// <param name="waitTimeOut">等待超时时间，-1为一直等</param>
        /// <returns>是否等待成功的结果对象</returns>
        M_OperateResult<TimeSpan> Wait(string address, bool waitValue, int readInterval, int waitTimeOut);
        M_OperateResult<TimeSpan> Wait(string address, short waitValue, int readInterval, int waitTimeOut);
        M_OperateResult<TimeSpan> Wait(string address, ushort waitValue, int readInterval, int waitTimeOut);
        M_OperateResult<TimeSpan> Wait(string address, int waitValue, int readInterval, int waitTimeOut);
        M_OperateResult<TimeSpan> Wait(string address, uint watiValue, int readInterval, int waitTimeOut);
        M_OperateResult<TimeSpan> Wait(string address, long waitValue, int readInterval, int waitTimeOut);
        M_OperateResult<TimeSpan> Wait(string address, ulong waitValue, int readInterval, int waitTimeOut);

        #endregion

        #region 读写自定义数据
        /// <summary>
        /// 读取自定义数据类型
        /// </summary>
        /// <typeparam name="T">自定义数据类型</typeparam>
        /// <param name="address">其实地址</param>
        /// <returns></returns>
        M_OperateResult<T> ReadCustomer<T>(string address) where T : IDataTransfer, new();

        /// <summary>
        /// 写入自定义数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        M_OperateResult WriteCustomer<T>(string address,T value)where T:IDataTransfer, new();


        #endregion

    }
}
