using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Net.NetworkBase
{
    /// <summary>
    /// 设备读写基类
    /// </summary>
    public class NetworkDeviceBase:NetworkSecBase,IReadWriteNet
    {

        /// <summary>
        /// 一个字单位的数据表示的地址长度
        /// 西门子 2
        /// 三菱 欧姆龙 modbus 1
        /// </summary>
        protected ushort WordLength { get; set; } = 1;
        public string ConnectionId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #region 读取写入 byte bool

        public virtual M_OperateResult<byte[]> Read(string address, ushort length) =>
            new M_OperateResult<byte[]>(CommonResources.Get.Language.Language.notsupportedfunction);
        public virtual M_OperateResult Write(string address, byte[] value) =>
            new M_OperateResult(CommonResources.Get.Language.Language.notsupportedfunction);
        public virtual M_OperateResult<bool> ReadBool(string address) => ToolBasic.GetResultFromArray(ReadBool(address, 1));
        public virtual M_OperateResult<bool[]> ReadBool(string address, ushort length) =>
            new M_OperateResult<bool[]>(CommonResources.Get.Language.Language.notsupportedfunction);
        public virtual M_OperateResult Write(string address, bool value)
            => Write(address, new bool[] { value });
        public virtual M_OperateResult Write(string address, bool[] value)
            => new M_OperateResult(CommonResources.Get.Language.Language.notsupportedfunction);





        #endregion

        #region 自定义读写

        
        public M_OperateResult<T> ReadCustomer<T>(string address)where T : IDataTransfer, new()
        {
            M_OperateResult<T> result = new M_OperateResult<T>();
            T Content = new T();
            M_OperateResult<byte[]> read = Read(address, Content.ReadCount);
            if (read.IsSuccess)
            {
                Content.ParseSource(read.Content);
                result.Content = Content;
                result.IsSuccess = true;
            }
            else
            {
                result.ErrorCode = read.ErrorCode;
                result.Message = read.Message;
            }
            return result;
        }
        public M_OperateResult WriteCustomer<T>(string address, T data) where T : IDataTransfer, new()
            => Write(address, data.ToSource());
        #endregion

        #region 读取
        public M_OperateResult<short> ReadInt16(string address) => ToolBasic.GetResultFromArray(ReadInt16(address, 1));
        public virtual M_OperateResult<short[]> ReadInt16(string address, ushort length)
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength)), m => byteTransform.bytes2Int16(m, 0, length));
        public M_OperateResult<ushort> ReadUInt16(string address) => ToolBasic.GetResultFromArray(ReadUInt16(address, 1));
        public virtual M_OperateResult<ushort[]> ReadUInt16(string address, ushort length) 
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength)), m => byteTransform.bytes2UInt16(m, 0, length));
        public M_OperateResult<int> ReadInt32(string address) => ToolBasic.GetResultFromArray(ReadInt32(address, 1));
        public virtual M_OperateResult<int[]> ReadInt32(string address, ushort length)
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength *2)), m => byteTransform.bytes2Int32(m, 0, length));
        public M_OperateResult<uint> ReadUInt32(string address) => ToolBasic.GetResultFromArray(ReadUInt32(address, 1));
        public virtual M_OperateResult<uint[]> ReadUInt32(string address, ushort length)
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength *2)), m => byteTransform.bytes2UInt32(m, 0, length));
        public M_OperateResult<long> ReadInt64(string address) => ToolBasic.GetResultFromArray(ReadInt64(address, 1));
        public virtual M_OperateResult<long[]> ReadInt64(string address, ushort length)
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength *4)), m => byteTransform.bytes2Int64(m, 0, length));
        public M_OperateResult<ulong> ReadUInt64(string address) => ToolBasic.GetResultFromArray(ReadUInt64(address, 1));
        public virtual M_OperateResult<ulong[]> ReadUInt64(string address, ushort length)
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 4)), m => byteTransform.bytes2UInt64(m, 0, length));
        public M_OperateResult<float> ReadFloat(string address) => ToolBasic.GetResultFromArray(ReadFloat(address, 1));
        public virtual M_OperateResult<float[]> ReadFloat(string address, ushort length)
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 2)), m => byteTransform.bytes2Single(m, 0, length));
        public M_OperateResult<double> ReadDouble(string address) => ToolBasic.GetResultFromArray(ReadDouble(address, 1));
        public virtual M_OperateResult<double[]> ReadDouble(string address, ushort length)
            => ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 4)), m => byteTransform.bytes2Double(m, 0, length));
        public M_OperateResult<string> ReadString(string address,ushort length)
            => ReadString(address, length, Encoding.ASCII);
        public virtual M_OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
            => ToolBasic.GetResultFromBytes(Read(address, length), m => byteTransform.bytes2String(m, 0, m.Length, encoding));




        #endregion

        #region 写入

        public virtual M_OperateResult Write(string address, short value) => Write(address, new short[] { value });
        public virtual M_OperateResult Write(string address, short[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, ushort value) => Write(address, new ushort[] { value });
        public virtual M_OperateResult Write(string address, ushort[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, int value) => Write(address, new int[] { value });
        public virtual M_OperateResult Write(string address, int[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, uint value) => Write(address, new uint[] { value });
        public virtual M_OperateResult Write(string address, uint[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, long value) => Write(address, new long[] { value });
        public virtual M_OperateResult Write(string address, long[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, ulong value) => Write(address, new ulong[] { value });
        public virtual M_OperateResult Write(string address, ulong[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, float value) => Write(address, new float[] { value });
        public virtual M_OperateResult Write(string address, float[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, double value) => Write(address, new double[] { value });
        public virtual M_OperateResult Write(string address, double[] value) => Write(address, byteTransform.data2Bytes(value));
        public virtual M_OperateResult Write(string address, string value) => Write(address, value, Encoding.ASCII);
        public virtual M_OperateResult Write(string address,string value,Encoding encoding)
        {
            byte[] temp = byteTransform.data2Bytes(value, encoding);
            if(WordLength == 1)temp = ToolBasic.ArrayExpandToLengthEven(temp);
            return Write(address, temp);
        }
        public virtual M_OperateResult Write(string address, string value, int length) => Write(address, value, length, Encoding.ASCII);
        public virtual M_OperateResult Write(string address,string value, int length,Encoding encoding)
        {
            byte[] temp = byteTransform.data2Bytes(value, encoding);
            if (WordLength == 1) temp = ToolBasic.ArrayExpandToLengthEven(temp);
            temp = ToolBasic.ArrayExpandToLength(temp, length);
            return Write(address,temp);
        }

        #endregion

        #region 等待
        public M_OperateResult<TimeSpan> Wait(string address, bool waitValue, int readInterval = 100, int waitTimeout = 3000)
            => ReadWriteWaitHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
        public M_OperateResult<TimeSpan> Wait(string address, short waitValue, int readInterval = 100, int waitTimeout = 3000)
            => ReadWriteWaitHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
        public M_OperateResult<TimeSpan> Wait(string address, ushort waitValue, int readInterval = 100, int waitTimeout = 3000)
            => ReadWriteWaitHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
        public M_OperateResult<TimeSpan> Wait(string address, int waitValue, int readInterval = 100, int waitTimeout = 3000)
            => ReadWriteWaitHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
        public M_OperateResult<TimeSpan> Wait(string address, uint waitValue, int readInterval = 100, int waitTimeout = 3000)
            => ReadWriteWaitHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
        public M_OperateResult<TimeSpan> Wait(string address, long waitValue, int readInterval = 100, int waitTimeout = 3000)
            => ReadWriteWaitHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
        public M_OperateResult<TimeSpan> Wait(string address, ulong waitValue, int readInterval = 100, int waitTimeout = 3000)
            => ReadWriteWaitHelper.Wait(this, address, waitValue, readInterval, waitTimeout);



        #endregion

        public override string ToString()
        {
            return $"NetworkDeviceBase<{GetNewNetMessage().GetType()}, {byteTransform.GetType()}>[{ip}:{port}]";
        }

        
    }
}
