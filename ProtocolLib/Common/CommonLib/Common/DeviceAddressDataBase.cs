using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 设备地址数据的信息，包含起始地址，数据类型，长度
    /// </summary>
    public class DeviceAddressDataBase
    {
        public int AddressStart { get; set; }
        public ushort Length { get; set; }
        public virtual void Parse(string address, ushort length)
        {
            AddressStart = int.Parse(address);
            Length = length;
        }
        public override string ToString() => AddressStart.ToString();
    }

    public class S7AddressData : DeviceAddressDataBase
    {
        /// <summary>
        /// 获取或设置等待读取的数据的代码
        /// </summary>
        public byte DataCode { get; set; }

        /// <summary>
        /// 获取或设置PLC的DB块数据信息
        /// </summary>
        public ushort DbBlock { get; set; }

        /// <summary>
        /// 从指定的地址信息解析成真正的设备地址信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        public override void Parse(string address, ushort length)
        {
            M_OperateResult<S7AddressData> addressData = ParseFrom(address, length);
            if (addressData.IsSuccess)
            {
                AddressStart = addressData.Content.AddressStart;
                Length = addressData.Content.Length;
                DataCode = addressData.Content.DataCode;
                DbBlock = addressData.Content.DbBlock;
            }
        }

        #region Static Method

        /// <summary>
        /// 计算特殊的地址信息
        /// </summary>
        /// <param name="address">字符串地</param>
        /// <param name="isCT">是否是定时器和计数器的地址</param>
        /// <returns>实际值</returns>
        public static int CalculateAddressStarted(string address, bool isCT = false)
        {
            if (address.IndexOf('.') < 0)
            {
                if (isCT)
                    return Convert.ToInt32(address);
                else
                    return Convert.ToInt32(address) * 8;
            }
            else
            {
                string[] temp = address.Split('.');
                return Convert.ToInt32(temp[0]) * 8 + Convert.ToInt32(temp[1]);
            }
        }

        /// <summary>
        /// 从实际的西门子的地址里面解析出地址对象
        /// </summary>
        /// <param name="address">西门子的地址数据信息</param>
        /// <returns>是否成功的结果对象</returns>
        public static M_OperateResult<S7AddressData> ParseFrom(string address)
        {
            return ParseFrom(address, 0);
        }

        /// <summary>
        /// 从实际的西门子的地址里面解析出地址对象
        /// </summary>
        /// <param name="address">西门子的地址数据信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static M_OperateResult<S7AddressData> ParseFrom(string address, ushort length)
        {
            S7AddressData addressData = new S7AddressData();
            try
            {
                addressData.Length = length;
                addressData.DbBlock = 0;
                if (address.StartsWith("AI") || address.StartsWith("ai"))
                {
                    addressData.DataCode = 0x06;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(2));
                }
                else if (address.StartsWith("AQ") || address.StartsWith("aq"))
                {
                    addressData.DataCode = 0x07;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(2));
                }
                else if (address[0] == 'I')
                {
                    addressData.DataCode = 0x81;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(1));
                }
                else if (address[0] == 'Q')
                {
                    addressData.DataCode = 0x82;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(1));
                }
                else if (address[0] == 'M')
                {
                    addressData.DataCode = 0x83;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(1));
                }
                else if (address[0] == 'D' || address.Substring(0, 2) == "DB")
                {
                    addressData.DataCode = 0x84;
                    string[] adds = address.Split('.');
                    if (address[1] == 'B')
                    {
                        addressData.DbBlock = Convert.ToUInt16(adds[0].Substring(2));
                    }
                    else
                    {
                        addressData.DbBlock = Convert.ToUInt16(adds[0].Substring(1));
                    }

                    string addTemp = address.Substring(address.IndexOf('.') + 1);
                    if (addTemp.StartsWith("DBX") || addTemp.StartsWith("DBB") || addTemp.StartsWith("DBW") || addTemp.StartsWith("DBD"))
                        addTemp = addTemp.Substring(3);
                    addressData.AddressStart = CalculateAddressStarted(addTemp);
                }
                else if (address[0] == 'T')
                {
                    addressData.DataCode = 0x1F;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(1), true);
                }
                else if (address[0] == 'C')
                {
                    addressData.DataCode = 0x1E;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(1), true);
                }
                else if (address[0] == 'V')
                {
                    addressData.DataCode = 0x84;
                    addressData.DbBlock = 1;
                    addressData.AddressStart = CalculateAddressStarted(address.Substring(1));
                }
                else
                {
                    return new M_OperateResult<S7AddressData>(CommonResources.Get.Language.Language.notsupportedfunction);
                }
            }
            catch (Exception ex)
            {
                return new M_OperateResult<S7AddressData>(ex.Message);
            }

            return M_OperateResult.CreateSuccessResult(addressData);
        }

        #endregion
    }



}
