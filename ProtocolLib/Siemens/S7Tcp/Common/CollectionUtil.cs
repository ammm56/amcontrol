using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtocolLib.S7Tcp.Core;

namespace ProtocolLib.S7Tcp.Common
{
    /// <summary>
    /// 读取写入设备数据
    /// </summary>
    public class CollectionUtil
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        public M_Return<M_GatherData> ReadData(SiemensS7 siemensS7, string functioncode, string address, string type, ushort len = 1)
        {
            string value = string.Empty;
            string descmsg = "";
            try
            {
                switch (type)
                {
                    case "int16":
                        M_OperateResult<short[]> result_short = siemensS7.ReadInt16(address, len);
                        if (result_short.IsSuccess) value = ToolBasic.ArrayFormatValue(result_short.Content);
                        else descmsg = result_short.Message;
                        break;
                    case "uint16":
                        M_OperateResult<ushort[]> result_ushort = siemensS7.ReadUInt16(address, len);
                        if (result_ushort.IsSuccess) value = ToolBasic.ArrayFormatValue(result_ushort.Content);
                        else descmsg = result_ushort.Message;
                        break;
                    case "int32":
                        M_OperateResult<int[]> result_int = siemensS7.ReadInt32(address, len);
                        if (result_int.IsSuccess) value = ToolBasic.ArrayFormatValue(result_int.Content);
                        else descmsg = result_int.Message;
                        break;
                    case "uint32":
                        M_OperateResult<uint[]> result_uint = siemensS7.ReadUInt32(address, len);
                        if (result_uint.IsSuccess) value = ToolBasic.ArrayFormatValue(result_uint.Content);
                        else descmsg = result_uint.Message;
                        break;
                    case "int64":
                        M_OperateResult<long[]> result_long = siemensS7.ReadInt64(address, len);
                        if (result_long.IsSuccess) value = ToolBasic.ArrayFormatValue(result_long.Content);
                        else descmsg = result_long.Message;
                        break;
                    case "uint64":
                        M_OperateResult<ulong[]> result_ulong = siemensS7.ReadUInt64(address, len);
                        if (result_ulong.IsSuccess) value = ToolBasic.ArrayFormatValue(result_ulong.Content);
                        else descmsg = result_ulong.Message;
                        break;
                    case "single":
                        M_OperateResult<float[]> result_single = siemensS7.ReadFloat(address, len);
                        if (result_single.IsSuccess) value = ToolBasic.ArrayFormatValue(result_single.Content);
                        else descmsg = result_single.Message;
                        break;
                    case "double":
                        M_OperateResult<double[]> result_double = siemensS7.ReadDouble(address, len);
                        if (result_double.IsSuccess) value = ToolBasic.ArrayFormatValue(result_double.Content);
                        else descmsg = result_double.Message;
                        break;
                    case "bool":
                        M_OperateResult<bool[]> result_bool = siemensS7.ReadBool(address, len);
                        if (result_bool.IsSuccess) value = ToolBasic.ArrayFormatValue(result_bool.Content);
                        else descmsg = result_bool.Message;
                        value = value.Replace("False", "0");
                        value = value.Replace("True", "1");
                        break;
                    case "byte":
                        M_OperateResult<byte[]> result_byte = siemensS7.Read(address, len);
                        if (result_byte.IsSuccess) value = ToolBasic.ArrayFormatValue(result_byte.Content);
                        else descmsg = result_byte.Message;
                        break;
                    case "string":
                        M_OperateResult<string> result_string = siemensS7.ReadString(address, 10, Encoding.UTF8);
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
                        point = address,
                        value = value,
                        type = type
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("CollectionUtil ReadData ex={0}", ex.Message));
                return M_Return<M_GatherData>.Error();
            }
        }

        /// <summary>
        /// 写入
        /// </summary>
        public M_Return<M_GatherData> WriteData(SiemensS7 siemensS7, string address, object value, string type, ushort len = 10)
        {
            try
            {
                M_OperateResult operateResult = null;
                string valueText = GetValueText(value);

                for (int item = 1; item <= 3; item++)
                {
                    try
                    {
                        operateResult = WriteByType(siemensS7, address, value, type, len);
                        if (operateResult != null && operateResult.IsSuccess)
                        {
                            return M_Return<M_GatherData>.OK(
                                new M_GatherData
                                {
                                    point = address,
                                    value = valueText,
                                    type = type
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

        private M_OperateResult WriteByType(SiemensS7 siemensS7, string address, object value, string type, ushort len)
        {
            object actualValue = UnwrapValue(value, type);

            switch (type)
            {
                case "bool":
                    return siemensS7.Write(address, Convert.ToBoolean(actualValue));
                case "byte":
                    return siemensS7.Write(address, new byte[] { Convert.ToByte(actualValue) });
                case "int16":
                    return siemensS7.Write(address, Convert.ToInt16(actualValue));
                case "uint16":
                    return siemensS7.Write(address, Convert.ToUInt16(actualValue));
                case "int32":
                    return siemensS7.Write(address, Convert.ToInt32(actualValue));
                case "uint32":
                    return siemensS7.Write(address, Convert.ToUInt32(actualValue));
                case "int64":
                    return siemensS7.Write(address, Convert.ToInt64(actualValue));
                case "uint64":
                    return siemensS7.Write(address, Convert.ToUInt64(actualValue));
                case "single":
                    return siemensS7.Write(address, Convert.ToSingle(actualValue));
                case "double":
                    return siemensS7.Write(address, Convert.ToDouble(actualValue));
                case "string":
                    string valuestr = Convert.ToString(actualValue) ?? string.Empty;
                    int templen = Math.Min(valuestr.Length, len);
                    valuestr = valuestr.Substring(0, templen);
                    return siemensS7.Write(address, valuestr);
                default:
                    return new M_OperateResult("不支持的数据类型:" + type);
            }
        }

        private static object UnwrapValue(object value, string type)
        {
            if (value is M_GatherData gatherData)
            {
                return UnwrapValue(gatherData.value, string.IsNullOrEmpty(type) ? gatherData.type : type);
            }

            if (value is M_TypedValue typedValue)
            {
                if (!string.IsNullOrEmpty(typedValue.type) &&
                    !string.IsNullOrEmpty(type) &&
                    !string.Equals(typedValue.type, type, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("写入值类型与目标类型不一致");
                }

                return UnwrapValue(typedValue.value, string.IsNullOrEmpty(type) ? typedValue.type : type);
            }

            return value;
        }

        private static string GetValueText(object value)
        {
            if (value is M_GatherData gatherData)
            {
                return gatherData.value ?? string.Empty;
            }

            if (value is M_TypedValue typedValue)
            {
                return typedValue.value ?? string.Empty;
            }

            return Convert.ToString(value) ?? string.Empty;
        }

        public M_Return<List<Point>> DecodePoints4Rule(ref M_ProtocolConfig protocolConfig)
        {
            try
            {
                List<Point> points = new List<Point>();
                return M_Return<List<Point>>.OK(points);
            }
            catch (Exception ex)
            {
                return M_Return<List<Point>>.Error(string.Format("解析规则地址错误，异常={0}", ex.Message));
            }
        }
    }
}