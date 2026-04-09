using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.ModbusTcp.Core
{
    /// <summary>
    /// Modbus协议相关的一些信息
    /// </summary>
    public class ModbusInfo
    {
        #region 功能码
        /// <summary>
		/// 读取线圈
		/// </summary>
		public const byte ReadCoil = 0x01;

        /// <summary>
        /// 读取离散量
        /// </summary>
        public const byte ReadDiscrete = 0x02;

        /// <summary>
        /// 读取寄存器
        /// </summary>
        public const byte ReadRegister = 0x03;

        /// <summary>
        /// 读取输入寄存器
        /// </summary>
        public const byte ReadInputRegister = 0x04;

        /// <summary>
        /// 写单个线圈
        /// </summary>
        public const byte WriteOneCoil = 0x05;

        /// <summary>
        /// 写单个寄存器
        /// </summary>
        public const byte WriteOneRegister = 0x06;

        /// <summary>
        /// 写多个线圈
        /// </summary>
        public const byte WriteCoil = 0x0F;

        /// <summary>
        /// 写多个寄存器
        /// </summary>
        public const byte WriteRegister = 0x10;

        /// <summary>
        /// 使用掩码的方式写入寄存器
        /// </summary>
        public const byte WriteMaskRegister = 0x16;




        #endregion

        #region 错误码
        /// <summary>
		/// 不支持该功能码
		/// </summary>
		public const byte FunctionCodeNotSupport = 0x01;
        /// <summary>
        /// 该地址越界
        /// </summary>
        public const byte FunctionCodeOverBound = 0x02;
        /// <summary>
        /// 读取长度超过最大值
        /// </summary>
        public const byte FunctionCodeQuantityOver = 0x03;
        /// <summary>
        /// 读写异常
        /// </summary>
        public const byte FunctionCodeReadWriteException = 0x04;


        #endregion

        private static void CheckModbusAddressStart(M_ModbusAddress mAddress, bool isStartWithZero)
        {
            if (!isStartWithZero)
            {
                if (mAddress.Address < 1) throw new Exception("地址值在起始地址为1的情况下，必须大于1");
                mAddress.Address = (ushort)(mAddress.Address - 1);
            }
        }

        /// <summary>
        /// 构建Modbus读取数据的核心报文，需要指定地址，长度，站号，是否起始地址0
        /// </summary>
        /// <param name="address">Modbus地址</param>
        /// <param name="length">读取的数据长度</param>
        /// <param name="station">默认的站号</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static M_OperateResult<byte[][]> BuildReadModbusCommand(string address, ushort length, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                M_ModbusAddress mAddress = new M_ModbusAddress(address, station, defaultFunction);
                CheckModbusAddressStart(mAddress, isStartWithZero);

                return BuildReadModbusCommand(mAddress, length);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[][]>(ex.Message);
            }
        }
        /// <summary>
        /// 构建Modbus读取数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码应该根据bool或是字来区分
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[][]> BuildReadModbusCommand(M_ModbusAddress mAddress, ushort length)
        {
            List<byte[]> commands = new List<byte[]>();
            if (mAddress.Function == ReadCoil ||
                mAddress.Function == ReadDiscrete ||
                mAddress.Function == ReadRegister ||
                mAddress.Function == ReadInputRegister)
            {
                // 支持自动切割读取，字读取 120 个字，位读取 2000 个位
                M_OperateResult<int[], int[]> bytes = ToolBasic.SplitReadLength(mAddress.Address, length,
                    (mAddress.Function == ReadCoil || mAddress.Function == ReadDiscrete) ? (ushort)2000 : (ushort)120);
                for (int i = 0; i < bytes.Content1.Length; i++)
                {
                    byte[] buffer = new byte[6];
                    buffer[0] = (byte)mAddress.Station;
                    buffer[1] = (byte)mAddress.Function;
                    buffer[2] = BitConverter.GetBytes(bytes.Content1[i])[1];
                    buffer[3] = BitConverter.GetBytes(bytes.Content1[i])[0];
                    buffer[4] = BitConverter.GetBytes(bytes.Content2[i])[1];
                    buffer[5] = BitConverter.GetBytes(bytes.Content2[i])[0];
                    commands.Add(buffer);
                }
            }
            else
            {
                byte[] buffer = new byte[6];
                buffer[0] = (byte)mAddress.Station;
                buffer[1] = (byte)mAddress.Function;
                buffer[2] = BitConverter.GetBytes(mAddress.Address)[1];
                buffer[3] = BitConverter.GetBytes(mAddress.Address)[0];
                buffer[4] = BitConverter.GetBytes(length)[1];
                buffer[5] = BitConverter.GetBytes(length)[0];
                commands.Add(buffer);
            }
            return M_OperateResult.CreateSuccessResult(commands.ToArray());
        }

        #region 写入bool
        /// <summary>
        /// 构建Modbus写入bool数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码
        /// </summary>
        /// <param name="address">Modbus地址</param>
        /// <param name="values">bool数组</param>
        /// <param name="station">默认站号</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static M_OperateResult<byte[]> BuildWriteBoolModbusCommand(string address, bool[] values, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                M_ModbusAddress mAddress = new M_ModbusAddress(address, station, defaultFunction);
                CheckModbusAddressStart(mAddress, isStartWithZero);

                return BuildWriteBoolModbusCommand(mAddress, values);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// 构建Modbus写入bool数据的核心报文，需要指定地址，长度，站号，是否起始地址0
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="station"></param>
        /// <param name="isStartWithZero"></param>
        /// <param name="defaultFunction"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteBoolModbusCommand(string address, bool value, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                if (address.IndexOf('.') <= 0)
                {
                    M_ModbusAddress mAddress = new M_ModbusAddress(address, station, defaultFunction);
                    CheckModbusAddressStart(mAddress, isStartWithZero);

                    return BuildWriteBoolModbusCommand(mAddress, value);
                }
                else
                {
                    int bitIndex = Convert.ToInt32(address.Substring(address.IndexOf('.') + 1));
                    if (bitIndex < 0 || bitIndex > 15) return new M_OperateResult<byte[]>("位访问的索引越界，应该在0-15之间");

                    int orMask = 1 << bitIndex;
                    int andMask = ~orMask;
                    if (!value) orMask = 0;

                    return BuildWriteMaskModbusCommand(address.Substring(0, address.IndexOf('.')), (ushort)andMask, (ushort)orMask, station, isStartWithZero, ModbusInfo.WriteMaskRegister);
                }
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// 构建Modbus写入bool数组的核心报文
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteBoolModbusCommand(M_ModbusAddress mAddress, bool[] values)
        {
            try
            {
                byte[] data = ToolBasic.BoolArrayToByte(values);
                byte[] content = new byte[7 + data.Length];
                content[0] = (byte)mAddress.Station;
                content[1] = (byte)mAddress.Function;
                content[2] = BitConverter.GetBytes(mAddress.Address)[1];
                content[3] = BitConverter.GetBytes(mAddress.Address)[0];
                content[4] = (byte)(values.Length / 256);
                content[5] = (byte)(values.Length % 256);
                content[6] = (byte)(data.Length);
                data.CopyTo(content, 7);
                return M_OperateResult.CreateSuccessResult(content);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// 构建Modbus写入bool数据的核心报文
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteBoolModbusCommand(M_ModbusAddress mAddress, bool value)
        {
            byte[] content = new byte[6];
            content[0] = (byte)mAddress.Station;
            content[1] = (byte)mAddress.Function;
            content[2] = BitConverter.GetBytes(mAddress.Address)[1];
            content[3] = BitConverter.GetBytes(mAddress.Address)[0];
            if (value)
            {
                content[4] = 0xFF;
                content[5] = 0x00;
            }
            else
            {
                content[4] = 0x00;
                content[5] = 0x00;
            }
            return M_OperateResult.CreateSuccessResult(content);
        }

        #endregion

        #region 写入字节数组 byte[]
        /// <summary>
        /// 构建Modbus写入字数据的核心报文，需要指定地址，长度，站号，是否起始地址0
        /// byte[]
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="station"></param>
        /// <param name="isStartWithZero"></param>
        /// <param name="defaultFunction"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteWordModbusCommand(string address, byte[] values, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                M_ModbusAddress mAddress = new M_ModbusAddress(address, station, defaultFunction);
                if (mAddress.Function == ModbusInfo.ReadRegister) mAddress.Function = defaultFunction;
                CheckModbusAddressStart(mAddress, isStartWithZero);

                return BuildWriteWordModbusCommand(mAddress, values);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// 写入byte[]
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteWordModbusCommand(M_ModbusAddress mAddress, byte[] values)
        {
            byte[] content = new byte[7 + values.Length];
            content[0] = (byte)mAddress.Station;
            content[1] = (byte)mAddress.Function;
            content[2] = BitConverter.GetBytes(mAddress.Address)[1];
            content[3] = BitConverter.GetBytes(mAddress.Address)[0];
            content[4] = (byte)(values.Length / 2 / 256);
            content[5] = (byte)(values.Length / 2 % 256);
            content[6] = (byte)(values.Length);
            values.CopyTo(content, 7);
            return M_OperateResult.CreateSuccessResult(content);
        }

        #endregion

        #region 写入int16
        /// <summary>
        /// 写入short
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="station"></param>
        /// <param name="isStartWithZero"></param>
        /// <param name="defaultFunction"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteWordModbusCommand(string address, short value, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                M_ModbusAddress mAddress = new M_ModbusAddress(address, station, defaultFunction);
                if (mAddress.Function == ModbusInfo.ReadRegister) mAddress.Function = defaultFunction;
                CheckModbusAddressStart(mAddress, isStartWithZero);

                return BuildWriteOneRegisterModbusCommand(mAddress, value);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// 写入short
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteOneRegisterModbusCommand(M_ModbusAddress mAddress, short value)
        {
            byte[] content = new byte[6];
            content[0] = (byte)mAddress.Station;
            content[1] = (byte)mAddress.Function;
            content[2] = BitConverter.GetBytes(mAddress.Address)[1];
            content[3] = BitConverter.GetBytes(mAddress.Address)[0];
            content[4] = BitConverter.GetBytes(value)[1];
            content[5] = BitConverter.GetBytes(value)[0];
            return M_OperateResult.CreateSuccessResult(content);
        }


        #endregion

        #region 写入uint16
        /// <summary>
        /// 写入ushort
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="station"></param>
        /// <param name="isStartWithZero"></param>
        /// <param name="defaultFunction"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteWordModbusCommand(string address, ushort value, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                M_ModbusAddress mAddress = new M_ModbusAddress(address, station, defaultFunction);
                if (mAddress.Function == ModbusInfo.ReadRegister) mAddress.Function = defaultFunction;
                CheckModbusAddressStart(mAddress, isStartWithZero);

                return BuildWriteOneRegisterModbusCommand(mAddress, value);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// 写入ushort
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteOneRegisterModbusCommand(M_ModbusAddress mAddress, ushort value)
        {
            byte[] content = new byte[6];
            content[0] = (byte)mAddress.Station;
            content[1] = (byte)mAddress.Function;
            content[2] = BitConverter.GetBytes(mAddress.Address)[1];
            content[3] = BitConverter.GetBytes(mAddress.Address)[0];
            content[4] = BitConverter.GetBytes(value)[1];
            content[5] = BitConverter.GetBytes(value)[0];
            return M_OperateResult.CreateSuccessResult(content);
        }



        #endregion

        #region 写入掩码
        /// <summary>
        /// 构建Modbus写入掩码数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码
        /// </summary>
        /// <param name="address"></param>
        /// <param name="andMask"></param>
        /// <param name="orMask"></param>
        /// <param name="station"></param>
        /// <param name="isStartWithZero"></param>
        /// <param name="defaultFunction"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteMaskModbusCommand(string address, ushort andMask, ushort orMask, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                M_ModbusAddress mAddress = new M_ModbusAddress(address, station, defaultFunction);
                if (mAddress.Function == ModbusInfo.ReadRegister) mAddress.Function = defaultFunction;
                CheckModbusAddressStart(mAddress, isStartWithZero);

                return BuildWriteMaskModbusCommand(mAddress, andMask, orMask);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// 构建Modbus写入掩码数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="andMask">等待进行与操作的掩码</param>
        /// <param name="orMask">等待进行或操作的掩码</param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> BuildWriteMaskModbusCommand(M_ModbusAddress mAddress, ushort andMask, ushort orMask)
        {
            byte[] content = new byte[8];
            content[0] = (byte)mAddress.Station;
            content[1] = (byte)mAddress.Function;
            content[2] = BitConverter.GetBytes(mAddress.Address)[1];
            content[3] = BitConverter.GetBytes(mAddress.Address)[0];
            content[4] = BitConverter.GetBytes(andMask)[1];
            content[5] = BitConverter.GetBytes(andMask)[0];
            content[6] = BitConverter.GetBytes(orMask)[1];
            content[7] = BitConverter.GetBytes(orMask)[0];
            return M_OperateResult.CreateSuccessResult(content);
        }

        #endregion

        /// <summary>
        /// 从返回的内容中取出数据
        /// 用于读取和写入操作
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> ExtractActualData(byte[] response)
        {
            try
            {
                if (response[1] >= 0x80)
                    return new M_OperateResult<byte[]>(ModbusInfo.GetDescriptionByErrorCode(response[2]));
                else if (response.Length > 3)
                    return M_OperateResult.CreateSuccessResult(ToolBasic.ArrayRemoveBegin(response, 3));
                else
                    return M_OperateResult.CreateSuccessResult(new byte[0]);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>(ex.Message);
            }
        }
        /// <summary>
        /// modbus指令打包成modbustcp指令
        /// 需要指定ID信息来添加6个字节的报文头
        /// </summary>
        /// <param name="modbus"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static byte[] PackCommandToTcp(byte[] modbus, ushort id)
        {
            byte[] buffer = new byte[modbus.Length + 6];
            buffer[0] = BitConverter.GetBytes(id)[1];
            buffer[1] = BitConverter.GetBytes(id)[0];

            buffer[4] = BitConverter.GetBytes(modbus.Length)[1];
            buffer[5] = BitConverter.GetBytes(modbus.Length)[0];

            modbus.CopyTo(buffer, 6);
            return buffer;
        }
        /// <summary>
        /// modbus指令打包成modbusrtu指令
        /// 末尾添加CRC16校验码
        /// </summary>
        /// <param name="modbus"></param>
        /// <returns></returns>
        public static byte[] PackCommandToRtu(byte[] modbus) => SoftCRC16.CRC16(modbus);
        /// <summary>
        /// modbustcp数据还原成modbus数据
        /// 移除6个字节的报文头数据
        /// </summary>
        /// <param name="modbusTcp"></param>
        /// <returns></returns>
        public static byte[] ExplodeTcpCommandToCore(byte[] modbusTcp) => modbusTcp.RemoveBegin(6);
        /// <summary>
        /// 将modbusrtu数据还原成modbus数据
        /// 移除CRC校验的内容
        /// </summary>
        /// <param name="modbusRtu"></param>
        /// <returns></returns>
        public static byte[] ExplodeRtuCommandToCore(byte[] modbusRtu) => modbusRtu.RemoveLast(2);
        /// <summary>
        /// 将modbus数据报文转换成modbus ascii数据报文
        /// 添加LRC校验，添加首尾标记数据
        /// </summary>
        /// <param name="modbus">modbusrtu报文，携带相关校验码</param>
        /// <returns>modbus ascii报文</returns>
        public static byte[] TransModbusCoreToAsciiPackCommand(byte[] modbus)
        {
            //添加LRC校验
            byte[] modbus_lrc = SoftLRC.LRC(modbus);

            //转换ascii信息
            byte[] modbus_ascii = ToolBasic.Bytes2AsciiBytes(modbus_lrc);

            //添加首尾信息
            return ToolBasic.SpliceArray(new byte[] { 0x3A }, modbus_ascii, new byte[] { 0x0D, 0x0A });
        }
        /// <summary>
        /// 将modbus ascii报文转换成modbus核心数据报文，移除首尾标记，移除LRC校验
        /// </summary>
        /// <param name="modbusAscii"></param>
        /// <returns></returns>
        public static M_OperateResult<byte[]> TransAsciiPackCommandToCore(byte[] modbusAscii)
        {
            try
            {
                //检查是不是modbus ascii指令
                if (modbusAscii[0] != 0x3A || modbusAscii[modbusAscii.Length - 2] != 0x0D || modbusAscii[modbusAscii.Length - 1] != 0x0A)
                {
                    return new M_OperateResult<byte[]>()
                    {
                        Message = $"Modbus的ascii指令检查失败，不是modbus-ascii报文 {modbusAscii.ToHexString(' ')}"
                    };
                }
                //移除前后指定字节数
                byte[] modbus_core = ToolBasic.AsciiBytes2Bytes(modbusAscii.RemoveDouble(1, 2));

                if (!SoftLRC.CheckLRC(modbus_core))
                {
                    return new M_OperateResult<byte[]>()
                    {
                        Message = $"Modbus的LRC校验检查失败 {modbus_core.ToHexString(' ')}"
                    };
                }

                return M_OperateResult.CreateSuccessResult(modbus_core.RemoveLast(1));
            }
            catch (Exception ex)
            {
                return new M_OperateResult<byte[]>()
                {
                    Message = $"{ex.Message} {modbusAscii.ToHexString(' ')}"
                };
            }
        }
        /// <summary>
        /// 分析Modbus协议的地址信息，该地址适应于tcp及rtu模式
        /// </summary>
        /// <param name="address">带格式的地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="defaultStation">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码信息</param>
        /// <returns></returns>
        public static M_OperateResult<M_ModbusAddress> AnalysisAddress(string address,byte defaultStation,bool isStartWithZero,byte defaultFunction)
        {
            try
            {
                M_ModbusAddress modbusAddress = new M_ModbusAddress(address,defaultStation,defaultFunction);
                if (!isStartWithZero)
                {
                    if(modbusAddress.Address < 1)
                    {
                        throw new Exception("地址值在起始地址为1的情况下，必须大于1");
                    }
                    modbusAddress.Address = (ushort)(modbusAddress.Address - 1);
                }
                return M_OperateResult.CreateSuccessResult(modbusAddress);

            }
            catch (Exception ex)
            {
                return new M_OperateResult<M_ModbusAddress>()
                {
                    Message = ex.Message
                };
            }
        }



        /// <summary>
        /// 错误码->文本消息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetDescriptionByErrorCode(byte code)
        {
            switch (code)
            {
                case ModbusInfo.FunctionCodeNotSupport: return "不支持这个功能码";
                case ModbusInfo.FunctionCodeOverBound: return "地址越界";
                case ModbusInfo.FunctionCodeQuantityOver: return "读取长度超过最大值";
                case ModbusInfo.FunctionCodeReadWriteException: return "读写异常";
                default: return CommonResources.Get.Language.Language.unknownerror;
            }
        }

    }
}
