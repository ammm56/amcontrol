using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.ModbusTcp.Common;
using ProtocolLib.ModbusTcp.Core;
using System;
using System.Linq;

namespace ProtocolLib.ModbusTcp
{
    /// <summary>
    /// Modbus TCP 协议统一入口。
    /// 当前版本直接使用 Address 作为完整协议地址。
    /// 点位、字符串、同类型连续数组统一走点位读写接口。
    /// </summary>
    public class Protocol : IProtocol
    {
        /// <summary>
        /// 协议名。
        /// </summary>
        public static readonly string ProtocolName = "modbustcp";

        /// <summary>
        /// 协议客户端。
        /// </summary>
        private ModbusTCP _modbusTCPClient;

        /// <summary>
        /// 连接结果。
        /// </summary>
        private M_OperateResult _connect;

        /// <summary>
        /// 协议配置。
        /// </summary>
        private M_ProtocolOptions _options;

        /// <summary>
        /// 点位读写辅助工具。
        /// </summary>
        private readonly CollectionUtil _collectionUtil = new CollectionUtil();

        public Protocol()
        {
        }

        public M_Return<bool> Configure(M_ProtocolOptions options)
        {
            try
            {
                if (options == null)
                {
                    return M_Return<bool>.Error("协议配置不能为空");
                }

                _options = options;

                if (_modbusTCPClient != null)
                {
                    _modbusTCPClient.UpdateConnectionInfo(
                        _options.ip ?? string.Empty,
                        _options.port <= 0 ? 502 : _options.port,
                        GetStationNo(_options));
                }

                return M_Return<bool>.OK(true);
            }
            catch (Exception ex)
            {
                return M_Return<bool>.Error("配置协议失败: " + ex.Message);
            }
        }

        public M_Return<bool> Connect()
        {
            try
            {
                if (_options == null)
                {
                    return M_Return<bool>.Error("协议未配置");
                }

                string ip = _options.ip ?? string.Empty;
                int port = _options.port <= 0 ? 502 : _options.port;
                byte stationNo = GetStationNo(_options);

                if (_modbusTCPClient == null)
                {
                    _modbusTCPClient = new ModbusTCP(ip, port, stationNo);
                }
                else
                {
                    _modbusTCPClient.UpdateConnectionInfo(ip, port, stationNo);
                }

                _connect = _modbusTCPClient.Connection();
                if (_connect == null || !_connect.IsSuccess)
                {
                    return M_Return<bool>.Error("连接错误 " + (_connect == null ? string.Empty : _connect.Message));
                }

                return M_Return<bool>.OK(true);
            }
            catch (Exception ex)
            {
                return M_Return<bool>.Error("连接异常: " + ex.Message);
            }
        }

        public M_Return<bool> Disconnect()
        {
            try
            {
                if (_modbusTCPClient == null || _connect == null || !_connect.IsSuccess)
                {
                    _connect = null;
                    return M_Return<bool>.OK(true);
                }

                M_OperateResult result = _modbusTCPClient.ConnectClose();
                if (!result.IsSuccess)
                {
                    return M_Return<bool>.Error("关闭连接错误");
                }

                _connect = null;
                return M_Return<bool>.OK(true);
            }
            catch (Exception ex)
            {
                return M_Return<bool>.Error("关闭连接异常: " + ex.Message);
            }
        }

        public M_Return<bool> Reconnect()
        {
            try
            {
                M_Return<bool> closeResult = Disconnect();
                if (!closeResult.Status)
                {
                    return closeResult;
                }

                return Connect();
            }
            catch (Exception ex)
            {
                return M_Return<bool>.Error("重新连接异常: " + ex.Message);
            }
        }

        public M_Return<bool> IsConnected()
        {
            return M_Return<bool>.OK(_connect != null && _connect.IsSuccess);
        }

        public M_Return<M_PointData> ReadPoint(M_PointReadRequest request)
        {
            try
            {
                if (request == null)
                {
                    return M_Return<M_PointData>.Error("点位读取请求不能为空");
                }

                if (_modbusTCPClient == null)
                {
                    return M_Return<M_PointData>.Error("协议未连接");
                }

                string address = NormalizeAddress(request.address);
                string dataType = NormalizeDataType(request.dataType);
                int length = NormalizeLength(request.length);

                if (string.IsNullOrWhiteSpace(address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                if (IsStringType(dataType))
                {
                    return ReadStringPoint(address, length);
                }

                if (IsArrayType(dataType))
                {
                    return ReadArrayPoint(address, dataType, length);
                }

                M_Return<M_GatherData> result = _collectionUtil.ReadData(_modbusTCPClient, string.Empty, address, dataType);
                if (!result.Status)
                {
                    return M_Return<M_PointData>.Error(result.DescMsg);
                }

                M_GatherData gatherData = result.Result ?? new M_GatherData();
                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        address = address,
                        dataType = dataType,
                        length = length,
                        value = gatherData.value ?? string.Empty,
                        rawBuffer = new byte[0],
                        quality = "Good"
                    });
            }
            catch (Exception ex)
            {
                return M_Return<M_PointData>.Error("点位读取异常: " + ex.Message);
            }
        }

        public M_Return<M_PointData> WritePoint(M_PointWriteRequest request)
        {
            try
            {
                if (request == null)
                {
                    return M_Return<M_PointData>.Error("点位写入请求不能为空");
                }

                if (_modbusTCPClient == null)
                {
                    return M_Return<M_PointData>.Error("协议未连接");
                }

                string address = NormalizeAddress(request.address);
                string dataType = NormalizeDataType(request.dataType);
                int length = NormalizeLength(request.length);

                if (string.IsNullOrWhiteSpace(address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                if (IsStringType(dataType))
                {
                    return WriteStringPoint(address, length, request.value);
                }

                if (IsArrayType(dataType))
                {
                    return WriteArrayPoint(address, dataType, length, request.value);
                }

                M_Return<M_GatherData> result = _collectionUtil.WriteData(_modbusTCPClient, address, request.value, dataType);
                if (!result.Status)
                {
                    return M_Return<M_PointData>.Error(result.DescMsg);
                }

                M_GatherData gatherData = result.Result ?? new M_GatherData();
                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        address = address,
                        dataType = dataType,
                        length = length,
                        value = gatherData.value ?? string.Empty,
                        rawBuffer = new byte[0],
                        quality = "Good"
                    });
            }
            catch (Exception ex)
            {
                return M_Return<M_PointData>.Error("点位写入异常: " + ex.Message);
            }
        }

        private M_Return<M_PointData> ReadStringPoint(string address, int length)
        {
            string actualAddress = AppendStringLength(address, length);
            M_Return<M_GatherData> result = _collectionUtil.ReadData(_modbusTCPClient, string.Empty, actualAddress, "string");
            if (!result.Status)
            {
                return M_Return<M_PointData>.Error(result.DescMsg);
            }

            M_GatherData gatherData = result.Result ?? new M_GatherData();
            return M_Return<M_PointData>.OK(
                new M_PointData
                {
                    address = address,
                    dataType = "string",
                    length = length,
                    value = gatherData.value ?? string.Empty,
                    rawBuffer = new byte[0],
                    quality = "Good"
                });
        }

        private M_Return<M_PointData> WriteStringPoint(string address, int length, object value)
        {
            string actualAddress = AppendStringLength(address, length);
            M_Return<M_GatherData> result = _collectionUtil.WriteData(_modbusTCPClient, actualAddress, value, "string");
            if (!result.Status)
            {
                return M_Return<M_PointData>.Error(result.DescMsg);
            }

            M_GatherData gatherData = result.Result ?? new M_GatherData();
            return M_Return<M_PointData>.OK(
                new M_PointData
                {
                    address = address,
                    dataType = "string",
                    length = length,
                    value = gatherData.value ?? string.Empty,
                    rawBuffer = new byte[0],
                    quality = "Good"
                });
        }

        private M_Return<M_PointData> ReadArrayPoint(string address, string dataType, int length)
        {
            string elementType = GetElementType(dataType);

            if (string.Equals(elementType, "bool", StringComparison.OrdinalIgnoreCase))
            {
                M_OperateResult<bool[]> readBool = _modbusTCPClient.ReadBool(address, (ushort)Math.Max(1, length));
                if (!readBool.IsSuccess)
                {
                    return M_Return<M_PointData>.Error(readBool.Message);
                }

                bool[] values = readBool.Content ?? new bool[0];
                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        address = address,
                        dataType = dataType,
                        length = length,
                        value = string.Join(",", values.Select(p => p ? "1" : "0").ToArray()),
                        rawBuffer = BoolArrayToByte(values),
                        quality = "Good"
                    });
            }

            int registerLength = ResolveRegisterLengthForArray(elementType, length);
            M_OperateResult<byte[]> read = _modbusTCPClient.Read(address, (ushort)Math.Max(1, registerLength));
            if (!read.IsSuccess)
            {
                return M_Return<M_PointData>.Error(read.Message);
            }

            byte[] buffer = TrimArrayBuffer(read.Content, elementType, length);
            return M_Return<M_PointData>.OK(
                new M_PointData
                {
                    address = address,
                    dataType = dataType,
                    length = length,
                    value = BuildBufferValueText(buffer),
                    rawBuffer = buffer,
                    quality = "Good"
                });
        }

        private M_Return<M_PointData> WriteArrayPoint(string address, string dataType, int length, object value)
        {
            string elementType = GetElementType(dataType);

            if (string.Equals(elementType, "bool", StringComparison.OrdinalIgnoreCase))
            {
                bool[] boolValues = value as bool[];
                if (boolValues == null)
                {
                    return M_Return<M_PointData>.Error("bool[] 写入值必须为 bool[]");
                }

                M_OperateResult writeResult = _modbusTCPClient.Write(address, boolValues);
                if (!writeResult.IsSuccess)
                {
                    return M_Return<M_PointData>.Error(writeResult.Message);
                }

                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        address = address,
                        dataType = dataType,
                        length = length,
                        value = string.Join(",", boolValues.Select(p => p ? "1" : "0").ToArray()),
                        rawBuffer = BoolArrayToByte(boolValues),
                        quality = "Good"
                    });
            }

            byte[] buffer = value as byte[];
            if (buffer == null)
            {
                return M_Return<M_PointData>.Error("数组写入值当前必须为 byte[]");
            }

            M_OperateResult write = _modbusTCPClient.Write(address, EnsureEvenLength(buffer));
            if (!write.IsSuccess)
            {
                return M_Return<M_PointData>.Error(write.Message);
            }

            return M_Return<M_PointData>.OK(
                new M_PointData
                {
                    address = address,
                    dataType = dataType,
                    length = length,
                    value = BuildBufferValueText(buffer),
                    rawBuffer = buffer,
                    quality = "Good"
                });
        }

        private static byte GetStationNo(M_ProtocolOptions options)
        {
            if (options == null || !options.stationNo.HasValue || options.stationNo.Value <= 0)
            {
                return 1;
            }

            return (byte)options.stationNo.Value;
        }

        private static string NormalizeAddress(string address)
        {
            return string.IsNullOrWhiteSpace(address) ? string.Empty : address.Trim();
        }

        private static string NormalizeDataType(string dataType)
        {
            return string.IsNullOrWhiteSpace(dataType)
                ? string.Empty
                : dataType.Trim().Replace(" ", string.Empty).ToLowerInvariant();
        }

        private static int NormalizeLength(int length)
        {
            return length <= 0 ? 1 : length;
        }

        private static bool IsStringType(string dataType)
        {
            return string.Equals(dataType, "string", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsArrayType(string dataType)
        {
            return !string.IsNullOrWhiteSpace(dataType) && dataType.EndsWith("[]", StringComparison.Ordinal);
        }

        private static string GetElementType(string dataType)
        {
            return IsArrayType(dataType) ? dataType.Substring(0, dataType.Length - 2) : dataType;
        }

        private static string AppendStringLength(string address, int length)
        {
            if (string.IsNullOrWhiteSpace(address) || length <= 0)
            {
                return address;
            }

            if (address.IndexOf('[') >= 0)
            {
                return address;
            }

            return address + "[" + length + "]";
        }

        private static int ResolveRegisterLengthForArray(string elementType, int length)
        {
            switch (elementType)
            {
                case "uint8":
                case "int8":
                    return Math.Max(1, (length + 1) / 2);
                case "uint16":
                case "int16":
                    return Math.Max(1, length);
                case "uint32":
                case "int32":
                case "float":
                    return Math.Max(1, length * 2);
                case "uint64":
                case "int64":
                case "double":
                    return Math.Max(1, length * 4);
                default:
                    return Math.Max(1, length);
            }
        }

        private static int ResolveByteLengthForArray(string elementType, int length)
        {
            switch (elementType)
            {
                case "uint8":
                case "int8":
                    return Math.Max(1, length);
                case "uint16":
                case "int16":
                    return Math.Max(1, length) * 2;
                case "uint32":
                case "int32":
                case "float":
                    return Math.Max(1, length) * 4;
                case "uint64":
                case "int64":
                case "double":
                    return Math.Max(1, length) * 8;
                default:
                    return Math.Max(1, length);
            }
        }

        private static byte[] TrimArrayBuffer(byte[] buffer, string elementType, int length)
        {
            if (buffer == null)
            {
                return new byte[0];
            }

            int byteLength = ResolveByteLengthForArray(elementType, length);
            int actualLength = Math.Min(buffer.Length, Math.Max(0, byteLength));
            byte[] result = new byte[actualLength];
            Array.Copy(buffer, 0, result, 0, actualLength);
            return result;
        }

        private static byte[] EnsureEvenLength(byte[] buffer)
        {
            if (buffer == null)
            {
                return new byte[0];
            }

            if (buffer.Length % 2 == 0)
            {
                return buffer;
            }

            byte[] result = new byte[buffer.Length + 1];
            Array.Copy(buffer, result, buffer.Length);
            return result;
        }

        private static byte[] BoolArrayToByte(bool[] values)
        {
            if (values == null || values.Length == 0)
            {
                return new byte[0];
            }

            int byteLength = (values.Length + 7) / 8;
            byte[] buffer = new byte[byteLength];

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i])
                {
                    buffer[i / 8] |= (byte)(1 << (i % 8));
                }
            }

            return buffer;
        }

        private static string BuildBufferValueText(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return string.Empty;
            }

            return BitConverter.ToString(buffer).Replace("-", " ");
        }
    }
}