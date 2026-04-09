using ProtocolLib.ModbusTcp.Interface;
using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.ModbusTcp.Core
{
    internal class ModbusHelper
    {
        public static M_OperateResult<byte[]> ExtraRtuResponseContent(byte[] send, byte[] response)
        {
            try
            {
                if (response == null) response = new byte[0];

                // 长度校验
                if (response.Length < 5) return new M_OperateResult<byte[]>("接收的数据长度太短");

                // 检查crc
                if (!SoftCRC16.CheckCRC16(response)) return new M_OperateResult<byte[]>("Modbus的CRC校验检查失败" + ToolBasic.Byte2HexString(response, ' '));

                // 发生了错误
                if ((send[1] + 0x80) == response[1]) return new M_OperateResult<byte[]>(response[2], ModbusInfo.GetDescriptionByErrorCode(response[2]));

                if (send[1] != response[1]) return new M_OperateResult<byte[]>(response[1], $"Receive Command Check Failed: ");

                // 移除CRC校验，返回真实数据
                return ModbusInfo.ExtractActualData(ModbusInfo.ExplodeRtuCommandToCore(response));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static M_OperateResult<byte[]> Read(IModbus modbus, string address, ushort length)
        {
            M_OperateResult<byte[][]> command = ModbusInfo.BuildReadModbusCommand(address, length, modbus.Station, modbus.AddressStartWithZero, ModbusInfo.ReadRegister);
            if (!command.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(command);

            List<byte> resultArray = new List<byte>();
            for (int i = 0; i < command.Content.Length; i++)
            {
                M_OperateResult<byte[]> read = modbus.ReadData(command.Content[i]);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(read);

                resultArray.AddRange(read.Content);
            }

            return M_OperateResult.CreateSuccessResult(resultArray.ToArray());
        }

        public static M_OperateResult Write(IModbus modbus, string address, byte[] value)
        {
            M_OperateResult<byte[]> command = ModbusInfo.BuildWriteWordModbusCommand(address, value, modbus.Station, modbus.AddressStartWithZero, ModbusInfo.WriteRegister);
            if (!command.IsSuccess) return command;

            return modbus.ReadData(command.Content);
        }
        public static M_OperateResult Write(IModbus modbus, string address, short value)
        {
            M_OperateResult<byte[]> command = ModbusInfo.BuildWriteWordModbusCommand(address, value, modbus.Station, modbus.AddressStartWithZero, ModbusInfo.WriteOneRegister);
            if (!command.IsSuccess) return command;

            return modbus.ReadData(command.Content);
        }
        public static M_OperateResult Write(IModbus modbus, string address, ushort value)
        {
            M_OperateResult<byte[]> command = ModbusInfo.BuildWriteWordModbusCommand(address, value, modbus.Station, modbus.AddressStartWithZero, ModbusInfo.WriteOneRegister);
            if (!command.IsSuccess) return command;

            return modbus.ReadData(command.Content);
        }
        public static M_OperateResult WriteMask(IModbus modbus, string address, ushort andMask, ushort orMask)
        {
            M_OperateResult<byte[]> command = ModbusInfo.BuildWriteMaskModbusCommand(address, andMask, orMask, modbus.Station, modbus.AddressStartWithZero, ModbusInfo.WriteMaskRegister);
            if (!command.IsSuccess) return command;

            return modbus.ReadData(command.Content);
        }
        public static M_OperateResult<bool[]> ReadBoolHelper(IModbus modbus, string address, ushort length, byte function)
        {
            if (address.IndexOf('.') > 0)
            {
                string[] addressSplits = address.SplitDot();
                int bitIndex = 0;
                try
                {
                    bitIndex = Convert.ToInt32(addressSplits[1]);
                }
                catch (Exception ex)
                {
                    return new M_OperateResult<bool[]>("Bit Index format wrong, " + ex.Message);
                }
                ushort len = (ushort)((length + bitIndex + 15) / 16);

                M_OperateResult<byte[]> read = modbus.Read(addressSplits[0], len);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<bool[]>(read);

                return M_OperateResult.CreateSuccessResult(ToolBasic.BytesReverseByWord(read.Content).ToBoolArray().SelectMiddle(bitIndex, length));
            }
            else
            {
                M_OperateResult<byte[][]> command = ModbusInfo.BuildReadModbusCommand(address, length, modbus.Station, modbus.AddressStartWithZero, function);
                if (!command.IsSuccess) return M_OperateResult.CreateFailedResult<bool[]>(command);

                List<bool> resultArray = new List<bool>();
                for (int i = 0; i < command.Content.Length; i++)
                {
                    M_OperateResult<byte[]> read = modbus.ReadData(command.Content[i]);
                    if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<bool[]>(read);

                    int bitLength = command.Content[i][4] * 256 + command.Content[i][5];
                    resultArray.AddRange(ToolBasic.ByteToBoolArray(read.Content, bitLength));
                }

                return M_OperateResult.CreateSuccessResult(resultArray.ToArray());
            }
        }

        public static M_OperateResult Write(IModbus modbus, string address, bool[] values)
        {
            M_OperateResult<byte[]> command = ModbusInfo.BuildWriteBoolModbusCommand(address, values, modbus.Station, modbus.AddressStartWithZero, ModbusInfo.WriteCoil);
            if (!command.IsSuccess) return command;

            return modbus.ReadData(command.Content);
        }

        public static M_OperateResult Write(IModbus modbus, string address, bool value)
        {
            M_OperateResult<byte[]> command = ModbusInfo.BuildWriteBoolModbusCommand(address, value, modbus.Station, modbus.AddressStartWithZero, ModbusInfo.WriteOneCoil);
            if (!command.IsSuccess) return command;

            return modbus.ReadData(command.Content);
        }



    }
}
