using ProtocolLib.ModbusTcp.Interface;
using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Enum;
using ProtocolLib.CommonLib.Net.NetworkBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.ModbusTcp.Core
{
    public class ModbusTCP : NetworkDeviceBase, IModbus
    {
        public ModbusTCP()
        {
            softIncrementCount = new SoftIncrementCount(ushort.MaxValue);
            WordLength = 1;
            station = 1;
            byteTransform = new ReverseWordTransform();
        }

        public ModbusTCP(string ip,int port = 502,byte station = 0x01)
        {
            this.softIncrementCount = new SoftIncrementCount(ushort.MaxValue);
            this.ip = ip;
            this.port = port;
            this.WordLength = 1;
            this.station = station;
            this.byteTransform = new ReverseWordTransform();
        }

        public void UpdateConnectionInfo(string ip, int port = 502, byte station = 0x01)
        {
            this.ip = ip;
            this.port = port;
            this.station = station;
        }

        /// <summary>
        /// 客户端站号
        /// </summary>
        private byte station { get; set; } = 0x01;
        /// <summary>
        /// 自增消息的对象
        /// </summary>
        private readonly SoftIncrementCount softIncrementCount;
        /// <summary>
        /// 地址是否从0开始
        /// </summary>
        private bool isAddressStartWithZero { get; set; } = true;

        public byte Station
        {
            get { return station; }
            set { station = value; }
        }
        public bool AddressStartWithZero
        {
            get { return isAddressStartWithZero; }
            set { isAddressStartWithZero = value; }
        }
        public E_DataFormat DataFormat
        {
            get { return byteTransform.DataFormat; }
            set { byteTransform.DataFormat = value; }
        }
        /// <summary>
        /// 字符串是否按字反转
        /// 默认False
        /// </summary>
        public bool IsStringReverse
        {
            get { return byteTransform.IsStringReverseByteWord; }   
            set { byteTransform.IsStringReverseByteWord = value; }
        }

        /// <summary>
        /// 获取modbus协议自增的消息号
        /// </summary>
        public SoftIncrementCount MessageId => softIncrementCount;

        protected override M_OperateResult InitOnConnect(Socket socket)
        {
            return base.InitOnConnect(socket);
        }

        public override M_OperateResult<int[]> ReadInt32(string address, ushort length)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 2)), m => transform.bytes2Int32(m, 0, length));
        }
        public override M_OperateResult<uint[]> ReadUInt32(string address, ushort length)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 2)), m => transform.bytes2UInt32(m, 0, length));
        }
        public override M_OperateResult<long[]> ReadInt64(string address, ushort length)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 4)), m => transform.bytes2Int64(m, 0, length));
        }
        public override M_OperateResult<ulong[]> ReadUInt64(string address, ushort length)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 4)), m => transform.bytes2UInt64(m, 0, length));
        }
        public override M_OperateResult<float[]> ReadFloat(string address,ushort length)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 2)), m => transform.bytes2Single(m, 0, length));
        }
        public override M_OperateResult<double[]> ReadDouble(string address, ushort length)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return ToolBasic.GetResultFromBytes(Read(address, (ushort)(length * WordLength * 4)), m => transform.bytes2Double(m, 0, length));
        }


        public override M_OperateResult Write(string address, int[] value)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return Write(address, transform.data2Bytes(value));
        }
        public override M_OperateResult Write(string address, uint[] value)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return Write(address, transform.data2Bytes(value));
        }
        public override M_OperateResult Write(string address, long[] value)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return Write(address, transform.data2Bytes(value));
        }
        public override M_OperateResult Write(string address, ulong[] value)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return Write(address, transform.data2Bytes(value));
        }
        public override M_OperateResult Write(string address, float[] value)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return Write(address, transform.data2Bytes(value));
        }
        public override M_OperateResult Write(string address, double[] value)
        {
            IByteTransform transform = ToolBasic.ExtractTransformParameter(ref address, this.byteTransform);
            return Write(address, transform.data2Bytes(value));
        }


        

        protected override byte[] PackCommandWithHeader(byte[] command)
        {
            return ModbusInfo.PackCommandToTcp(command, (ushort)softIncrementCount.GetCurrentValue());
        }
        protected override M_OperateResult<byte[]> ExtraResponseContent(byte[] send, byte[] response)
        {
            return ModbusInfo.ExtractActualData(ModbusInfo.ExplodeTcpCommandToCore(response));
        }

        #region 读写
        /// <summary>
        /// 读取线圈 默认功能码0x01
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public M_OperateResult<bool> ReadCoil(string address) => ReadBool(address);
        /// <summary>
        /// 批量读取线圈
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public M_OperateResult<bool[]> ReadCoil(string address, ushort length) => ReadBool(address, length);
        /// <summary>
        /// 读取输入线圈 默认0x02
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public M_OperateResult<bool> ReadDiscrete(string address) => ToolBasic.GetResultFromArray(ReadDiscrete(address, 1));
        /// <summary>
        /// 批量读取输入线圈
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public M_OperateResult<bool[]> ReadDiscrete(string address, ushort length) => ModbusHelper.ReadBoolHelper(this, address, length, ModbusInfo.ReadDiscrete);
        /// <summary>
        /// 批量读取寄存器信息 默认0x03
        /// </summary>
        /// <param name="address">起始地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override M_OperateResult<byte[]> Read(string address, ushort length) => ModbusHelper.Read(this, address, length);
        /// <summary>
        /// 数据写入到寄存器 默认0x10
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override M_OperateResult Write(string address, byte[] value) => ModbusHelper.Write(this, address, value);
        /// <summary>
        /// 数据写入单个寄存器 默认0x06
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override M_OperateResult Write(string address, short value) => ModbusHelper.Write(this, address, value);
        /// <summary>
        /// 数据写入单个寄存器 默认0x06
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override M_OperateResult Write(string address, ushort value) => ModbusHelper.Write(this, address, value);
        /// <summary>
        /// 写入掩码 0x16
        /// </summary>
        /// <param name="address"></param>
        /// <param name="andMask"></param>
        /// <param name="orMask"></param>
        /// <returns></returns>
        public M_OperateResult WriteMask(string address, ushort andMask, ushort orMask) => ModbusHelper.WriteMask(this, address, andMask, orMask);
        /// <summary>
        /// 批量读取线圈或是离散的数据信息
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override M_OperateResult<bool[]> ReadBool(string address, ushort length) => ModbusHelper.ReadBoolHelper(this, address, length, ModbusInfo.ReadCoil);
        /// <summary>
        /// 向线圈中写入bool数组，返回是否写入成功
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override M_OperateResult Write(string address, bool[] values) => ModbusHelper.Write(this, address, values);
        /// <summary>
        /// 向线圈中写入bool数值，返回是否写入成功
        /// 地址为字地址，例如100.2，那么将使用0x16的功能码，通过掩码的方式来修改寄存器的某一位
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override M_OperateResult Write(string address, bool value) => ModbusHelper.Write(this, address, value);

        #endregion

        public M_OperateResult WriteOneRegister(string address, short value) => Write(address, value);
        public M_OperateResult WriteOneRegister(string address, ushort value) => Write(address, value);


    }
}
