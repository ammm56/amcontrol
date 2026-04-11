using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.S7Tcp.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ProtocolLib.S7Tcp.Common
{
    /// <summary>
    /// SiemensS7 点位读写工具。
    /// 当前实现只负责：
    /// 1. 标量类型读写；
    /// 2. string 类型读写；
    /// 3. 返回统一的 <see cref="M_GatherData"/> 结构。
    ///
    /// 设计约束：
    /// 1. 数组读写不在本类处理，由上层 Protocol 负责；
    /// 2. 不承载寄存器长度换算逻辑，因为 S7 当前按字节读取模型处理；
    /// 3. 统一使用规范化后的基础类型名：
    ///    bool、uint8、int8、uint16、int16、uint32、int32、uint64、int64、float、double、string。
    /// </summary>
    public class CollectionUtil
    {
        /// <summary>
        /// 读取单值或字符串点位。
        /// 标量类型按传入基础类型读取；
        /// string 类型按地址附加长度或默认长度读取。
        /// </summary>
        /// <param name="siemensS7">SiemensS7 客户端。</param>
        /// <param name="functioncode">功能码文本，当前仅透传到返回结构。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="type">点位基础类型。</param>
        /// <param name="len">读取长度，string 时表示字符长度，其余标量通常为 1。</param>
        /// <returns>统一读取结果。</returns>
        public M_Return<M_GatherData> ReadData(SiemensS7 siemensS7, string functioncode, string address, string type, ushort len = 1)
        {
            try
            {
                if (siemensS7 == null)
                {
                    return M_Return<M_GatherData>.Error("SiemensS7 客户端不能为空");
                }

                string normalizedType = ToolBasic.NormalizeProtocolDataType(type);
                string actualAddress = address ?? string.Empty;
                string valueText;
                bool success;
                string errorMessage;

                if (string.Equals(normalizedType, "string", StringComparison.OrdinalIgnoreCase))
                {
                    success = TryReadString(siemensS7, ref actualAddress, len, out valueText, out errorMessage);
                }
                else
                {
                    success = TryReadScalar(siemensS7, actualAddress, normalizedType, len, out valueText, out errorMessage);
                }

                return new M_Return<M_GatherData>
                {
                    Status = success,
                    DescMsg = errorMessage ?? string.Empty,
                    Result = new M_GatherData
                    {
                        functioncode = functioncode,
                        point = address,
                        value = valueText ?? string.Empty,
                        type = normalizedType
                    }
                };
            }
            catch (Exception ex)
            {
                return M_Return<M_GatherData>.Error("读取错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 写入单值或字符串点位。
        /// 标量类型直接按目标类型写入；
        /// string 类型按解析出的字符长度截断后写入。
        /// </summary>
        /// <param name="siemensS7">SiemensS7 客户端。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="value">待写入值。</param>
        /// <param name="type">点位基础类型。</param>
        /// <param name="len">写入长度，string 时表示字符长度，其余标量通常为 1。</param>
        /// <returns>统一写入结果。</returns>
        public M_Return<M_GatherData> WriteData(SiemensS7 siemensS7, string address, object value, string type, ushort len = 1)
        {
            try
            {
                if (siemensS7 == null)
                {
                    return M_Return<M_GatherData>.Error("SiemensS7 客户端不能为空");
                }

                string normalizedType = ToolBasic.NormalizeProtocolDataType(type);
                string actualAddress = address ?? string.Empty;
                object actualValue = UnwrapValue(value, normalizedType);

                M_OperateResult writeResult;
                string valueText;

                if (string.Equals(normalizedType, "string", StringComparison.OrdinalIgnoreCase))
                {
                    int stringLength = ExtractStringLength(ref actualAddress, len);
                    string text = Convert.ToString(actualValue, CultureInfo.InvariantCulture) ?? string.Empty;
                    if (text.Length > stringLength)
                    {
                        text = text.Substring(0, stringLength);
                    }

                    writeResult = siemensS7.Write(actualAddress, text, stringLength, Encoding.ASCII);
                    valueText = text;
                }
                else
                {
                    writeResult = WriteScalar(siemensS7, actualAddress, actualValue, normalizedType);
                    valueText = BuildScalarValueText(actualValue, normalizedType);
                }

                if (writeResult == null || !writeResult.IsSuccess)
                {
                    return M_Return<M_GatherData>.Error("写入错误:" + (writeResult == null ? "未知错误" : writeResult.Message));
                }

                return M_Return<M_GatherData>.OK(
                    new M_GatherData
                    {
                        point = address,
                        value = valueText,
                        type = normalizedType
                    });
            }
            catch (Exception ex)
            {
                return M_Return<M_GatherData>.Error("写入错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 从采集协议配置中解析规则地址。
        /// 当前保留最小空实现，后续若恢复规则引擎，可在此继续扩展。
        /// </summary>
        /// <param name="protocolConfig">协议配置对象。</param>
        /// <returns>规则点位集合。</returns>
        public M_Return<List<Point>> DecodePoints4Rule(ref M_ProtocolConfig protocolConfig)
        {
            try
            {
                return M_Return<List<Point>>.OK(new List<Point>());
            }
            catch (Exception ex)
            {
                return M_Return<List<Point>>.Error(string.Format("解析规则地址错误，异常={0}", ex.Message));
            }
        }

        /// <summary>
        /// 尝试读取标量类型。
        /// </summary>
        /// <param name="siemensS7">SiemensS7 客户端。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="type">规范化后的基础类型。</param>
        /// <param name="len">读取数量。</param>
        /// <param name="valueText">返回的值文本。</param>
        /// <param name="errorMessage">失败时的错误信息。</param>
        /// <returns>是否成功。</returns>
        private static bool TryReadScalar(SiemensS7 siemensS7, string address, string type, ushort len, out string valueText, out string errorMessage)
        {
            valueText = string.Empty;
            errorMessage = string.Empty;

            switch (type)
            {
                case "bool":
                    {
                        M_OperateResult<bool[]> result = siemensS7.ReadBool(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content).Replace("False", "0").Replace("True", "1");
                        return true;
                    }

                case "uint8":
                    {
                        M_OperateResult<byte[]> result = siemensS7.Read(address, 1);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        byte[] content = result.Content ?? new byte[0];
                        valueText = content.Length <= 0 ? "0" : content[0].ToString(CultureInfo.InvariantCulture);
                        return true;
                    }

                case "int8":
                    {
                        M_OperateResult<byte[]> result = siemensS7.Read(address, 1);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        byte[] content = result.Content ?? new byte[0];
                        sbyte value = content.Length <= 0 ? (sbyte)0 : unchecked((sbyte)content[0]);
                        valueText = value.ToString(CultureInfo.InvariantCulture);
                        return true;
                    }

                case "int16":
                    {
                        M_OperateResult<short[]> result = siemensS7.ReadInt16(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                case "uint16":
                    {
                        M_OperateResult<ushort[]> result = siemensS7.ReadUInt16(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                case "int32":
                    {
                        M_OperateResult<int[]> result = siemensS7.ReadInt32(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                case "uint32":
                    {
                        M_OperateResult<uint[]> result = siemensS7.ReadUInt32(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                case "int64":
                    {
                        M_OperateResult<long[]> result = siemensS7.ReadInt64(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                case "uint64":
                    {
                        M_OperateResult<ulong[]> result = siemensS7.ReadUInt64(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                case "float":
                    {
                        M_OperateResult<float[]> result = siemensS7.ReadFloat(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                case "double":
                    {
                        M_OperateResult<double[]> result = siemensS7.ReadDouble(address, len);
                        if (!result.IsSuccess)
                        {
                            errorMessage = result.Message;
                            return false;
                        }

                        valueText = ToolBasic.ArrayFormatValue(result.Content);
                        return true;
                    }

                default:
                    errorMessage = "不支持的数据类型:" + type;
                    return false;
            }
        }

        /// <summary>
        /// 尝试读取字符串类型。
        /// 先解析字符串长度，再按字符长度读取。
        /// </summary>
        /// <param name="siemensS7">SiemensS7 客户端。</param>
        /// <param name="address">点位地址，内部可能会去掉尾部 [n]。</param>
        /// <param name="len">默认字符串长度。</param>
        /// <param name="valueText">读取到的字符串值。</param>
        /// <param name="errorMessage">失败时的错误信息。</param>
        /// <returns>是否成功。</returns>
        private static bool TryReadString(SiemensS7 siemensS7, ref string address, ushort len, out string valueText, out string errorMessage)
        {
            valueText = string.Empty;
            errorMessage = string.Empty;

            int stringLength = ExtractStringLength(ref address, len);

            M_OperateResult<string> result = siemensS7.ReadString(address, (ushort)stringLength, Encoding.ASCII);
            if (!result.IsSuccess)
            {
                errorMessage = result.Message;
                return false;
            }

            valueText = (result.Content ?? string.Empty).TrimEnd('\0');
            if (valueText.Length > stringLength)
            {
                valueText = valueText.Substring(0, stringLength);
            }

            return true;
        }

        /// <summary>
        /// 写入标量类型。
        /// </summary>
        /// <param name="siemensS7">SiemensS7 客户端。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="value">待写入值。</param>
        /// <param name="type">规范化后的基础类型。</param>
        /// <returns>底层写入结果。</returns>
        private static M_OperateResult WriteScalar(SiemensS7 siemensS7, string address, object value, string type)
        {
            switch (type)
            {
                case "bool":
                    return siemensS7.Write(address, Convert.ToBoolean(value, CultureInfo.InvariantCulture));

                case "uint8":
                    return siemensS7.Write(address, new byte[] { Convert.ToByte(value, CultureInfo.InvariantCulture) });

                case "int8":
                    return siemensS7.Write(address, new byte[] { unchecked((byte)Convert.ToSByte(value, CultureInfo.InvariantCulture)) });

                case "int16":
                    return siemensS7.Write(address, Convert.ToInt16(value, CultureInfo.InvariantCulture));

                case "uint16":
                    return siemensS7.Write(address, Convert.ToUInt16(value, CultureInfo.InvariantCulture));

                case "int32":
                    return siemensS7.Write(address, Convert.ToInt32(value, CultureInfo.InvariantCulture));

                case "uint32":
                    return siemensS7.Write(address, Convert.ToUInt32(value, CultureInfo.InvariantCulture));

                case "int64":
                    return siemensS7.Write(address, Convert.ToInt64(value, CultureInfo.InvariantCulture));

                case "uint64":
                    return siemensS7.Write(address, Convert.ToUInt64(value, CultureInfo.InvariantCulture));

                case "float":
                    return siemensS7.Write(address, Convert.ToSingle(value, CultureInfo.InvariantCulture));

                case "double":
                    return siemensS7.Write(address, Convert.ToDouble(value, CultureInfo.InvariantCulture));

                default:
                    return new M_OperateResult("不支持的数据类型:" + type);
            }
        }

        /// <summary>
        /// 递归展开包装值。
        /// 支持从 <see cref="M_GatherData"/> 或 <see cref="M_TypedValue"/> 中提取实际写入值。
        /// </summary>
        /// <param name="value">原始输入值。</param>
        /// <param name="type">目标类型。</param>
        /// <returns>展开后的实际值。</returns>
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

        /// <summary>
        /// 将标量值格式化为统一字符串文本。
        /// 用于写入成功后构造返回值。
        /// </summary>
        /// <param name="value">实际值。</param>
        /// <param name="type">规范化后的基础类型。</param>
        /// <returns>字符串文本。</returns>
        private static string BuildScalarValueText(object value, string type)
        {
            switch (type)
            {
                case "bool":
                    return Convert.ToBoolean(value, CultureInfo.InvariantCulture) ? "1" : "0";
                case "uint8":
                    return Convert.ToByte(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "int8":
                    return Convert.ToSByte(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "int16":
                    return Convert.ToInt16(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "uint16":
                    return Convert.ToUInt16(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "int32":
                    return Convert.ToInt32(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "uint32":
                    return Convert.ToUInt32(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "int64":
                    return Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "uint64":
                    return Convert.ToUInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "float":
                    return Convert.ToSingle(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                case "double":
                    return Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                default:
                    return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
            }
        }

        /// <summary>
        /// 提取字符串长度。
        /// 若地址尾部带有 [n]，优先使用该长度；
        /// 否则使用传入默认长度；
        /// 若默认长度无效，则回退为 1。
        /// </summary>
        /// <param name="address">点位地址，若解析成功会移除尾部 [n]。</param>
        /// <param name="defaultLength">默认字符串长度。</param>
        /// <returns>最终字符串长度。</returns>
        private static int ExtractStringLength(ref string address, ushort defaultLength)
        {
            int length = ToolBasic.ExtractStartIndex(ref address);
            return length > 0 ? length : (defaultLength <= 0 ? 1 : defaultLength);
        }
    }
}