using ProtocolLib.CommonLib.Model;
using System;
using System.Text;

namespace ProtocolLib.ModbusTcp.Core
{
    /// <summary>
    /// Modbus 地址格式。
    /// </summary>
    public class M_ModbusAddress : M_DeviceAddressBase
    {
        public M_ModbusAddress()
        {
            Station = -1;
            Function = -1;
            Address = 0;
        }

        public M_ModbusAddress(string address)
        {
            Station = -1;
            Function = -1;
            Address = 0;
            Parse(address);
        }

        public M_ModbusAddress(string address, byte function)
        {
            Station = -1;
            Function = -1;
            Address = 0;
            Parse(address);

            if (Function < 0)
            {
                Function = function;
            }
        }

        public M_ModbusAddress(string address, byte station, byte function)
        {
            Station = station;
            Function = -1;
            Address = 0;
            Parse(address);

            if (Function < 0)
            {
                Function = function;
            }
        }

        /// <summary>
        /// 站号。
        /// </summary>
        public int Station { get; set; }

        /// <summary>
        /// 功能码。
        /// </summary>
        public int Function { get; set; }

        public override void Parse(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new Exception("Modbus 地址不能为空");
            }

            if (address.IndexOf(';') < 0)
            {
                ClassAddressParse(address);
                return;
            }

            string[] list = address.Split(';');
            for (int i = 0; i < list.Length; i++)
            {
                string item = list[i];
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                if (item[0] == 's' || item[0] == 'S')
                {
                    Station = byte.Parse(item.Substring(2));
                }
                else if (item[0] == 'x' || item[0] == 'X')
                {
                    Function = byte.Parse(item.Substring(2));
                }
                else
                {
                    ClassAddressParse(item);
                }
            }
        }

        /// <summary>
        /// 标准地址转换：
        /// 00001 / 10001 / 30001 / 40001
        /// 同时兼容带偏移：
        /// 40010.H / 40010.L / 40010.0 / 40010.1
        /// </summary>
        private void ClassAddressParse(string addressText)
        {
            string pureAddress = ExtractBaseAddress(addressText);

            if (string.IsNullOrWhiteSpace(pureAddress) || pureAddress.Length < 2)
            {
                throw new Exception("不支持的Modbus地址格式:" + addressText);
            }

            string startValue = pureAddress.Substring(0, 1);
            string endValue = pureAddress.Substring(1, pureAddress.Length - 1);

            switch (Convert.ToInt32(startValue))
            {
                case 0:
                    if (Function < 0) Function = ModbusInfo.ReadCoil;
                    Address = ushort.Parse(endValue);
                    break;

                case 1:
                    if (Function < 0) Function = ModbusInfo.ReadDiscrete;
                    Address = ushort.Parse(endValue);
                    break;

                case 3:
                    if (Function < 0) Function = ModbusInfo.ReadInputRegister;
                    Address = ushort.Parse(endValue);
                    break;

                case 4:
                    if (Function < 0) Function = ModbusInfo.ReadRegister;
                    Address = ushort.Parse(endValue);
                    break;

                default:
                    throw new Exception("不支持的Modbus地址格式:" + addressText);
            }
        }

        private static string ExtractBaseAddress(string addressText)
        {
            string working = addressText.Trim();

            int dotIndex = working.IndexOf('.');
            if (dotIndex > 0)
            {
                working = working.Substring(0, dotIndex);
            }

            return working;
        }

        /// <summary>
        /// 返回一个新地址。
        /// </summary>
        public M_ModbusAddress AddressAdd(int value)
        {
            return new M_ModbusAddress()
            {
                Station = this.Station,
                Function = this.Function,
                Address = (ushort)(this.Address + value),
            };
        }

        /// <summary>
        /// 地址偏移 1，返回一个新的地址。
        /// </summary>
        public M_ModbusAddress AddressAdd()
        {
            return AddressAdd(1);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Station >= 0) sb.Append("s=" + Station + ";");
            if (Function >= 1) sb.Append("x=" + Function + ";");
            sb.Append(Address.ToString());
            return sb.ToString();
        }
    }
}