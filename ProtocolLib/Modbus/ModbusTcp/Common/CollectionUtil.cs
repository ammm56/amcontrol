using ProtocolLib.ModbusTcp.Core;
using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.ModbusTcp.Common
{
    /// <summary>
    /// 读取写入设备数据
    /// </summary>
    public class CollectionUtil
    {
        /// <summary>
        /// 规则中表示地址的标记
        /// </summary>
        public static readonly List<string> _addressSign = new List<string> { "last", "now" };

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="modbusTCP"></param>
        /// <param name="functioncode"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public M_Return<M_GatherData> ReadData(ModbusTCP modbusTCP, string functioncode, string address, string type, ushort len = 1)
        {
            string value = string.Empty;
            string descmsg = "";
            try
            {
                switch (type)
                {
                    case "int16":
                        M_OperateResult<short[]> result_short = modbusTCP.ReadInt16(address, len);
                        if (result_short.IsSuccess) value = ToolBasic.ArrayFormatValue(result_short.Content);
                        else descmsg = result_short.Message;
                        break;
                    case "uint16":
                        M_OperateResult<ushort[]> result_ushort = modbusTCP.ReadUInt16(address, len);
                        if (result_ushort.IsSuccess) value = ToolBasic.ArrayFormatValue(result_ushort.Content);
                        else descmsg = result_ushort.Message;
                        break;
                    case "int32":
                        M_OperateResult<int[]> result_int = modbusTCP.ReadInt32(address, len);
                        if (result_int.IsSuccess) value = ToolBasic.ArrayFormatValue(result_int.Content);
                        else descmsg = result_int.Message;
                        break;
                    case "uint32":
                        M_OperateResult<uint[]> result_uint = modbusTCP.ReadUInt32(address, len);
                        if (result_uint.IsSuccess) value = ToolBasic.ArrayFormatValue(result_uint.Content);
                        else descmsg = result_uint.Message;
                        break;
                    case "int64":
                        M_OperateResult<long[]> result_long = modbusTCP.ReadInt64(address, len);
                        if (result_long.IsSuccess) value = ToolBasic.ArrayFormatValue(result_long.Content);
                        else descmsg = result_long.Message;
                        break;
                    case "uint64":
                        M_OperateResult<ulong[]> result_ulong = modbusTCP.ReadUInt64(address, len);
                        if (result_ulong.IsSuccess) value = ToolBasic.ArrayFormatValue(result_ulong.Content);
                        else descmsg = result_ulong.Message;
                        break;
                    case "single":
                        M_OperateResult<float[]> result_single = modbusTCP.ReadFloat(address, len);
                        if (result_single.IsSuccess) value = ToolBasic.ArrayFormatValue(result_single.Content);
                        else descmsg = result_single.Message;
                        break;
                    case "double":
                        M_OperateResult<double[]> result_double = modbusTCP.ReadDouble(address, len);
                        if (result_double.IsSuccess) value = ToolBasic.ArrayFormatValue(result_double.Content);
                        else descmsg = result_double.Message;
                        break;
                    case "bool":
                        M_OperateResult<bool[]> result_bool = modbusTCP.ReadBool(address, len);
                        if (result_bool.IsSuccess) value = ToolBasic.ArrayFormatValue(result_bool.Content);
                        else descmsg = result_bool.Message;
                        value = value.Replace("False", "0");
                        value = value.Replace("True", "1");
                        break;
                    case "byte":
                        M_OperateResult<byte[]> result_byte = modbusTCP.Read(address, len);
                        if (result_byte.IsSuccess) value = ToolBasic.ArrayFormatValue(result_byte.Content);
                        else descmsg = result_byte.Message;
                        break;
                    case "string":
                        M_OperateResult<string> result_string = modbusTCP.ReadString(address, 10, Encoding.UTF8);
                        if (result_string.IsSuccess) value = ToolBasic.ArrayFormat(result_string.Content.Trim().Replace("\u0000", "")); 
                        else descmsg = result_string.Message;
                        break;
                    default:
                        break;
                }

                bool status = value.Equals(string.Empty) ? false : true;
                return new M_Return<M_GatherData>
                {
                    Status = status,
                    DescMsg = descmsg,
                    Result = new M_GatherData
                    {
                        //functioncode = functioncode,
                        point = address,
                        //lastvalue = "",
                        value = value
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CollectionUtil ReadData ex={ex.Message}");
                return M_Return<M_GatherData>.Error();
            }
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="modbusTCP"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public M_Return<M_GatherData> WriteData(ModbusTCP modbusTCP, string address, object value, string type, ushort len = 10)
        {
            try
            {
                M_OperateResult operateResult = null;
                for (int item = 1; item <= 3; item++)
                {
                    try
                    {
                        operateResult = WriteByType(modbusTCP, address, value, type, len);
                        if (operateResult != null && operateResult.IsSuccess)
                        {
                            return M_Return<M_GatherData>.OK(
                                new M_GatherData
                                {
                                    point = address,
                                    value = Convert.ToString(value),
                                });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("第{0}次写入错误 {1}", item, ex.Message));
                        if (item < 3) Console.WriteLine("尝试再次写入！");
                    }
                }

                return M_Return<M_GatherData>.Error("写入错误:" + (operateResult == null ? "未知错误" : operateResult.Message));
            }
            catch (Exception ex)
            {
                return M_Return<M_GatherData>.Error("写入错误:" + ex.Message);
            }
        }

        private M_OperateResult WriteByType(ModbusTCP modbusTCP, string address, object value, string type, ushort len)
        {
            switch (type)
            {
                case "bool":
                    return modbusTCP.Write(address, Convert.ToBoolean(value));
                case "byte":
                    return modbusTCP.Write(address, new byte[] { Convert.ToByte(value) });
                case "int16":
                    return modbusTCP.Write(address, Convert.ToInt16(value));
                case "uint16":
                    return modbusTCP.Write(address, Convert.ToUInt16(value));
                case "int32":
                    return modbusTCP.Write(address, Convert.ToInt32(value));
                case "uint32":
                    return modbusTCP.Write(address, Convert.ToUInt32(value));
                case "int64":
                    return modbusTCP.Write(address, Convert.ToInt64(value));
                case "uint64":
                    return modbusTCP.Write(address, Convert.ToUInt64(value));
                case "single":
                    return modbusTCP.Write(address, Convert.ToSingle(value));
                case "double":
                    return modbusTCP.Write(address, Convert.ToDouble(value));
                case "string":
                    string valuestr = Convert.ToString(value) ?? string.Empty;
                    int templen = Math.Min(valuestr.Length, len);
                    valuestr = valuestr.Substring(0, templen);
                    return modbusTCP.Write(address, valuestr);
                default:
                    return new M_OperateResult("不支持的数据类型:" + type);
            }
        }

        /// <summary>
        /// 从采集协议配置中解析规则地址
        /// </summary>
        /// <param name="protocolConfig"></param>
        /// <returns></returns>
        public M_Return<List<Point>> DecodePoints4Rule(ref M_ProtocolConfig protocolConfig)
        {
            try
            {
                List<Point> points = new List<Point>();
                return M_Return<List<Point>>.OK(points);
            }
            catch (Exception ex)
            {
                return M_Return<List<Point>>.Error($"从采集协议配置中解析规则地址错误，异常={ex.Message}");
            }
        }

    }
}
