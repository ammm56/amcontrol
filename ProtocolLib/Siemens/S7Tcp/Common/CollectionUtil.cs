using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Text;
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
            string descmsg = string.Empty;
            bool status = false;

            try
            {
                string actualAddress = address;

                switch (type)
                {
                    case "int16":
                        M_OperateResult<short[]> result_short = siemensS7.ReadInt16(actualAddress, len);
                        status = result_short.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_short.Content);
                        else descmsg = result_short.Message;
                        break;

                    case "uint16":
                        M_OperateResult<ushort[]> result_ushort = siemensS7.ReadUInt16(actualAddress, len);
                        status = result_ushort.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_ushort.Content);
                        else descmsg = result_ushort.Message;
                        break;

                    case "int32":
                        M_OperateResult<int[]> result_int = siemensS7.ReadInt32(actualAddress, len);
                        status = result_int.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_int.Content);
                        else descmsg = result_int.Message;
                        break;

                    case "uint32":
                        M_OperateResult<uint[]> result_uint = siemensS7.ReadUInt32(actualAddress, len);
                        status = result_uint.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_uint.Content);
                        else descmsg = result_uint.Message;
                        break;

                    case "int64":
                        M_OperateResult<long[]> result_long = siemensS7.ReadInt64(actualAddress, len);
                        status = result_long.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_long.Content);
                        else descmsg = result_long.Message;
                        break;

                    case "uint64":
                        M_OperateResult<ulong[]> result_ulong = siemensS7.ReadUInt64(actualAddress, len);
                        status = result_ulong.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_ulong.Content);
                        else descmsg = result_ulong.Message;
                        break;

                    case "single":
                        M_OperateResult<float[]> result_single = siemensS7.ReadFloat(actualAddress, len);
                        status = result_single.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_single.Content);
                        else descmsg = result_single.Message;
                        break;

                    case "double":
                        M_OperateResult<double[]> result_double = siemensS7.ReadDouble(actualAddress, len);
                        status = result_double.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_double.Content);
                        else descmsg = result_double.Message;
                        break;

                    case "bool":
                        M_OperateResult<bool[]> result_bool = siemensS7.ReadBool(actualAddress, len);
                        status = result_bool.IsSuccess;
                        if (status)
                        {
                            value = ToolBasic.ArrayFormatValue(result_bool.Content);
                            value = value.Replace("False", "0");
                            value = value.Replace("True", "1");
                        }
                        else
                        {
                            descmsg = result_bool.Message;
                        }
                        break;

                    case "byte":
                        M_OperateResult<byte[]> result_byte = siemensS7.Read(actualAddress, len);
                        status = result_byte.IsSuccess;
                        if (status) value = ToolBasic.ArrayFormatValue(result_byte.Content);
                        else descmsg = result_byte.Message;
                        break;

                    case "string":
                        int readStringLength = ResolveStringLength(ref actualAddress, 10);
                        M_OperateResult<string> result_string = siemensS7.ReadString(actualAddress, (ushort)readStringLength, Encoding.ASCII);
                        status = result_string.IsSuccess;
                        if (status)
                        {
                            value = (result_string.Content ?? string.Empty).TrimEnd('\0');
                            if (value.Length > readStringLength)
                            {
                                value = value.Substring(0, readStringLength);
                            }
                        }
                        else
                        {
                            descmsg = result_string.Message;
                        }
                        break;

                    default:
                        descmsg = "不支持的数据类型:" + type;
                        status = false;
                        break;
                }

                return new M_Return<M_GatherData>
                {
                    Status = status,
                    DescMsg = descmsg,
                    Result = new M_GatherData
                    {
                        functioncode = functioncode,
                        point = address,
                        value = value,
                        type = type
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("CollectionUtil ReadData ex={0}", ex.Message));
                return M_Return<M_GatherData>.Error("读取错误:" + ex.Message);
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

                for (int item = 1; item <= 3; item++)
                {
                    try
                    {
                        string currentAddress = address;
                        int resolvedStringLength = len;

                        if (string.Equals(type, "string", StringComparison.OrdinalIgnoreCase))
                        {
                            resolvedStringLength = ResolveStringLength(ref currentAddress, len);
                        }

                        string valueText = GetValueText(value, type, resolvedStringLength);
                        operateResult = WriteByType(siemensS7, currentAddress, value, type, resolvedStringLength);

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

        private M_OperateResult WriteByType(SiemensS7 siemensS7, string address, object value, string type, int len)
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
                    return siemensS7.Write(address, valuestr, len, Encoding.ASCII);
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

        private static string GetValueText(object value, string type, int stringLength)
        {
            string text;

            if (value is M_GatherData gatherData)
            {
                text = gatherData.value ?? string.Empty;
            }
            else if (value is M_TypedValue typedValue)
            {
                text = typedValue.value ?? string.Empty;
            }
            else
            {
                text = Convert.ToString(value) ?? string.Empty;
            }

            if (string.Equals(type, "string", StringComparison.OrdinalIgnoreCase))
            {
                int textLength = Math.Min(text.Length, stringLength);
                return text.Substring(0, textLength);
            }

            return text;
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

        private static int ResolveStringLength(ref string address, int defaultLength)
        {
            int length = ToolBasic.ExtractStartIndex(ref address);
            return length > 0 ? length : defaultLength;
        }
    }
}