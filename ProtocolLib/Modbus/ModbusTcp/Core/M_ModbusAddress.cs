using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ProtocolLib.ModbusTcp.Core
{
    /// <summary>
    /// Modbus地址格式
    /// </summary>
    public class M_ModbusAddress:M_DeviceAddressBase
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
            Function = function;
            Address = 0;
            Parse(address);
        }

        public M_ModbusAddress(string address, byte station, byte function)
        {
            Station = -1;
            Function = function;
            Station = station;
            Address = 0;
            Parse(address);
        }

        /// <summary>
        /// 站号
        /// </summary>
        public int Station { get; set; }
        /// <summary>
        /// 功能码
        /// </summary>
        public int Function { get; set; }

        public override void Parse(string address)
        {
            if (address.IndexOf(';') < 0)
            {
                // 正常地址，功能码03
                //Address = ushort.Parse(address);
                ClassAddressParse(address);
            }
            else
            {
                // 带功能码的地址
                string[] list = address.Split(';');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i][0] == 's' || list[i][0] == 'S')
                    {
                        // 站号信息
                        this.Station = byte.Parse(list[i].Substring(2));
                    }
                    else if (list[i][0] == 'x' || list[i][0] == 'X')
                    {
                        this.Function = byte.Parse(list[i].Substring(2));
                    }
                    else
                    {
                        this.Address = ushort.Parse(list[i]);
                    }
                }
            }
        }
        /// <summary>
        /// 标准地址转换
        /// 00001 10001 40001 30001
        /// </summary>
        /// <param name="addrss"></param>
        private void ClassAddressParse(string addrss)
        {
            string startValue = addrss.Substring(0, 1);//几区
            string endValue = addrss.Substring(1, addrss.Length - 1);
            switch (Convert.ToInt32(startValue))
            {
                //线圈 0区 01功能码-读取 05功能码-写入
                case 0:
                    //x=1;0000
                    //读
                    this.Function = byte.Parse("1");
                    this.Address = ushort.Parse(endValue);
                    //写
                    break;
                //读取离散量 1区 02功能码-读取
                case 1:
                    //x=2;0000
                    //读
                    this.Function = byte.Parse("2");
                    this.Address = ushort.Parse(endValue);
                    break;
                //读取输入寄存器 3区 04功能码-读取
                case 3:
                    //x=4;0000
                    //读
                    this.Function = byte.Parse("4");
                    this.Address = ushort.Parse(endValue);
                    break;
                //保持寄存器 4区 03功能码-读取 06-功能码-写入
                case 4:
                    //x=3;0000
                    //读
                    this.Function = byte.Parse("3");
                    this.Address = ushort.Parse(endValue);
                    //写
                    break;
            }
        }

        /// <summary>
        /// 地址便宜指定的位置，返回一个新地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
		/// 地址偏移1，返回一个新的地址
		/// </summary>
		/// <returns>新增后的地址信息</returns>
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
