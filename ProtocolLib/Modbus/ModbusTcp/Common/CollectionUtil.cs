using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.ModbusTcp.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ProtocolLib.ModbusTcp.Common
{
    /// <summary>
    /// ModbusTCP 点位读写工具。
    /// 当前实现只负责：
    /// 1. 标量类型读写；
    /// 2. string 类型读写；
    /// 3. 返回统一的 <see cref="M_GatherData"/> 结构。
    ///
    /// 设计约束：
    /// 1. 数组读写不在本类处理，由上层 Protocol 负责；
    /// 2. Modbus 专属的“16位寄存器数量换算”放在本类中，不放到 ToolBasic；
    /// 3. 统一使用规范化后的基础类型名：
    ///    bool、uint8、int8、uint16、int16、uint32、int32、uint64、int64、float、double、string。
    /// </summary>
    public class CollectionUtil
    {
        /// <summary>
        /// 规则中可用于表示地址的保留标记。
        /// 当前仍保留该字段，便于兼容后续规则解析逻辑。
        /// </summary>
        public static readonly List<string> _addressSign = new List<string> { "last", "now" };

        /// <summary>
        /// 读取单值或字符串点位。
        /// 标量类型按传入基础类型读取；
        /// string 类型按地址附加长度或默认长度读取。
        /// </summary>
        /// <param name="modbusTCP">ModbusTCP 客户端。</param>
        /// <param name="functioncode">功能码文本，当前仅透传到返回结构。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="type">点位基础类型。</param>
        /// <param name="len">读取长度，string 时表示字符长度，其余标量通常为 1。</param>
        /// <returns>统一读取结果。</returns>
        public M_Return<M_GatherData> ReadData(ModbusTCP modbusTCP, string functioncode, string address, string type, ushort len = 1)
        {
            try
            {
                if (modbusTCP == null)
                {
                    return M_Return<M_GatherData>.Error("ModbusTCP 客户端不能为空");
                }

                string normalizedType = ToolBasic.NormalizeProtocolDataType(type);
                string actualAddress = address ?? string.Empty;
                string valueText;
                bool success;
                string errorMessage;

                if (string.Equals(normalizedType, "string", StringComparison.OrdinalIgnoreCase))
                {
                    success = TryReadString(modbusTCP, ref actualAddress, len, out valueText, out errorMessage);
                }
                else
                {
                    success = TryReadScalar(modbusTCP, actualAddress, normalizedType, len, out valueText, out errorMessage);
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
        /// <param name="modbusTCP">ModbusTCP 客户端。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="value">待写入值。</param>
        /// <param name="type">点位基础类型。</param>
        /// <param name="len">写入长度，string 时表示字符长度，其余标量通常为 1。</param>
        /// <returns>统一写入结果。</returns>
        public M_Return<M_GatherData> WriteData(ModbusTCP modbusTCP, string address, object value, string type, ushort len = 1)
        {
            try
            {
                if (modbusTCP == null)
                {
                    return M_Return<M_GatherData>.Error("ModbusTCP 客户端不能为空");
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

                    writeResult = modbusTCP.Write(actualAddress, text, stringLength, Encoding.ASCII);
                    valueText = text;
                }
                else
                {
                    writeResult = WriteScalar(modbusTCP, actualAddress, actualValue, normalizedType);
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
                return M_Return<List<Point>>.Error(string.Format("从采集协议配置中解析规则地址错误，异常={0}", ex.Message));
            }
        }

        /// <summary>
        /// 计算指定基础类型数组在 Modbus 16 位寄存器读取模型下所需的寄存器数量。
        /// 该方法仅适用于以 16 位寄存器为基本读取单位的协议场景，
        /// 例如 Modbus Holding Register / Input Register。
        /// 不适用于按字节寻址读取的协议，例如 S7 原始字节读取。
        /// </summary>
        /// <param name="elementType">数组元素基础类型，例如 uint16、float。</param>
        /// <param name="length">元素个数，小于等于 0 时按 1 处理。</param>
        /// <returns>所需的 16 位寄存器数量。</returns>
        public static int ResolveWordRegisterLengthForArray(string elementType, int length)
        {
            int actualLength = length <= 0 ? 1 : length;

            switch (elementType)
            {
                case "uint8":
                case "int8":
                    return (actualLength + 1) / 2;

                case "uint16":
                case "int16":
                    return actualLength;

                case "uint32":
                case "int32":
                case "float":
                    return actualLength * 2;

                case "uint64":
                case "int64":
                case "double":
                    return actualLength * 4;

                default:
                    return actualLength;
            }
        }

        /// <summary>
        /// 尝试读取标量类型。
        /// </summary>
        /// <param name="modbusTCP">ModbusTCP 客户端。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="type">规范化后的基础类型。</param>
        /// <param name="len">读取数量。</param>
        /// <param name="valueText">返回的值文本。</param>
        /// <param name="errorMessage">失败时的错误信息。</param>
        /// <returns>是否成功。</returns>
        private static bool TryReadScalar(ModbusTCP modbusTCP, string address, string type, ushort len, out string valueText, out string errorMessage)
        {
            valueText = string.Empty;
            errorMessage = string.Empty;

            switch (type)
            {
                case "bool":
                    {
                        M_OperateResult<bool[]> result = modbusTCP.ReadBool(address, len);
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
                        M_OperateResult<byte[]> result = modbusTCP.Read(address, 1);
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
                        M_OperateResult<byte[]> result = modbusTCP.Read(address, 1);
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
                        M_OperateResult<short[]> result = modbusTCP.ReadInt16(address, len);
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
                        M_OperateResult<ushort[]> result = modbusTCP.ReadUInt16(address, len);
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
                        M_OperateResult<int[]> result = modbusTCP.ReadInt32(address, len);
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
                        M_OperateResult<uint[]> result = modbusTCP.ReadUInt32(address, len);
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
                        M_OperateResult<long[]> result = modbusTCP.ReadInt64(address, len);
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
                        M_OperateResult<ulong[]> result = modbusTCP.ReadUInt64(address, len);
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
                        M_OperateResult<float[]> result = modbusTCP.ReadFloat(address, len);
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
                        M_OperateResult<double[]> result = modbusTCP.ReadDouble(address, len);
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
        /// 先解析字符串长度，再换算为 Modbus 寄存器数读取。
        /// </summary>
        /// <param name="modbusTCP">ModbusTCP 客户端。</param>
        /// <param name="address">点位地址，内部可能会去掉尾部 [n]。</param>
        /// <param name="len">默认字符串长度。</param>
        /// <param name="valueText">读取到的字符串值。</param>
        /// <param name="errorMessage">失败时的错误信息。</param>
        /// <returns>是否成功。</returns>
        private static bool TryReadString(ModbusTCP modbusTCP, ref string address, ushort len, out string valueText, out string errorMessage)
        {
            valueText = string.Empty;
            errorMessage = string.Empty;

            int stringLength = ExtractStringLength(ref address, len);
            ushort registerLength = (ushort)((stringLength + 1) / 2);

            M_OperateResult<string> result = modbusTCP.ReadString(address, registerLength, Encoding.ASCII);
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
        /// <param name="modbusTCP">ModbusTCP 客户端。</param>
        /// <param name="address">点位地址。</param>
        /// <param name="value">待写入值。</param>
        /// <param name="type">规范化后的基础类型。</param>
        /// <returns>底层写入结果。</returns>
        private static M_OperateResult WriteScalar(ModbusTCP modbusTCP, string address, object value, string type)
        {
            switch (type)
            {
                case "bool":
                    return modbusTCP.Write(address, Convert.ToBoolean(value, CultureInfo.InvariantCulture));

                case "uint8":
                    return modbusTCP.Write(address, new byte[] { Convert.ToByte(value, CultureInfo.InvariantCulture) });

                case "int8":
                    return modbusTCP.Write(address, new byte[] { unchecked((byte)Convert.ToSByte(value, CultureInfo.InvariantCulture)) });

                case "int16":
                    return modbusTCP.Write(address, Convert.ToInt16(value, CultureInfo.InvariantCulture));

                case "uint16":
                    return modbusTCP.Write(address, Convert.ToUInt16(value, CultureInfo.InvariantCulture));

                case "int32":
                    return modbusTCP.Write(address, Convert.ToInt32(value, CultureInfo.InvariantCulture));

                case "uint32":
                    return modbusTCP.Write(address, Convert.ToUInt32(value, CultureInfo.InvariantCulture));

                case "int64":
                    return modbusTCP.Write(address, Convert.ToInt64(value, CultureInfo.InvariantCulture));

                case "uint64":
                    return modbusTCP.Write(address, Convert.ToUInt64(value, CultureInfo.InvariantCulture));

                case "float":
                    return modbusTCP.Write(address, Convert.ToSingle(value, CultureInfo.InvariantCulture));

                case "double":
                    return modbusTCP.Write(address, Convert.ToDouble(value, CultureInfo.InvariantCulture));

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