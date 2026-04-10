using ProtocolLib.ModbusTcp.Common;
using ProtocolLib.ModbusTcp.Core;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using System;
using System.Collections.Generic;

namespace ProtocolLib.ModbusTcp
{
    /// <summary>
    /// Modbus TCP 协议统一入口。
    /// 对外暴露结构化点位/块读写接口，协议细节在内部完成转换。
    /// </summary>
    public class Protocol : IProtocol
    {
        /// <summary>
        /// 协议名
        /// </summary>
        public static readonly string ProtocolName = "modbustcp";

        /// <summary>
        /// 协议客户端
        /// </summary>
        private ModbusTCP _modbusTCPClient = null;

        /// <summary>
        /// 连接结果
        /// </summary>
        private M_OperateResult _connect = null;

        /// <summary>
        /// 协议配置
        /// </summary>
        private M_ProtocolConfig _protocolConfig = null;

        /// <summary>
        /// 条件采集点表
        /// </summary>
        private List<Point> _firstPoints = new List<Point>();

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

                _protocolConfig = new M_ProtocolConfig
                {
                    equipmentid = string.Empty,
                    protocoltype = string.IsNullOrWhiteSpace(options.protocolType) ? ProtocolName : options.protocolType.Trim(),
                    ip = options.ip ?? string.Empty,
                    port = options.port <= 0 ? 502 : options.port,
                    byteorder = 0,
                    pointinfo = new List<Point>()
                };

                if (_modbusTCPClient != null)
                {
                    _modbusTCPClient.UpdateConnectionInfo(
                        _protocolConfig.ip,
                        _protocolConfig.port,
                        GetStationNo(options));
                }

                M_Return<List<Point>> res = _collectionUtil.DecodePoints4Rule(ref _protocolConfig);
                if (res.Status)
                {
                    _firstPoints = res.Result ?? new List<Point>();
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
                if (_protocolConfig == null)
                {
                    return M_Return<bool>.Error("协议未配置");
                }

                if (_modbusTCPClient == null)
                {
                    _modbusTCPClient = new ModbusTCP(_protocolConfig.ip, _protocolConfig.port, 1);
                }
                else
                {
                    _modbusTCPClient.UpdateConnectionInfo(_protocolConfig.ip, _protocolConfig.port, 1);
                }

                _connect = _modbusTCPClient.Connection();
                if (!_connect.IsSuccess)
                {
                    return M_Return<bool>.Error("连接错误 " + _connect.Message);
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

                M_OperateResult res = _modbusTCPClient.ConnectClose();
                if (!res.IsSuccess)
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
                M_Return<bool> closeRes = Disconnect();
                if (!closeRes.Status)
                {
                    return closeRes;
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
            bool isConnected = _connect != null && _connect.IsSuccess;
            return M_Return<bool>.OK(isConnected);
        }

        public M_Return<M_PointData> ReadPoint(M_PointReadRequest request)
        {
            try
            {
                if (request == null)
                {
                    return M_Return<M_PointData>.Error("点位读取请求不能为空");
                }

                string mappedAddress = BuildPointAddress(request);
                string mappedType = NormalizeDataType(request.dataType);

                M_Return<M_GatherData> res = _collectionUtil.ReadData(_modbusTCPClient, string.Empty, mappedAddress, mappedType);
                if (!res.Status)
                {
                    return M_Return<M_PointData>.Error(res.DescMsg);
                }

                M_GatherData gatherData = res.Result ?? new M_GatherData();
                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        areaType = request.areaType ?? string.Empty,
                        address = request.address ?? string.Empty,
                        dataType = request.dataType ?? string.Empty,
                        value = gatherData.value ?? string.Empty,
                        rawBuffer = new byte[0],
                        quality = res.Status ? "Good" : "Error"
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

                string mappedAddress = BuildPointAddress(request);
                string mappedType = NormalizeDataType(request.dataType);

                M_Return<M_GatherData> res = _collectionUtil.WriteData(_modbusTCPClient, mappedAddress, request.value, mappedType);
                if (!res.Status)
                {
                    return M_Return<M_PointData>.Error(res.DescMsg);
                }

                M_GatherData gatherData = res.Result ?? new M_GatherData();
                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        areaType = request.areaType ?? string.Empty,
                        address = request.address ?? string.Empty,
                        dataType = request.dataType ?? string.Empty,
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

        public M_Return<M_BlockData> ReadBlock(M_BlockReadRequest request)
        {
            try
            {
                if (request == null)
                {
                    return M_Return<M_BlockData>.Error("块读取请求不能为空");
                }

                string normalizedAreaType = NormalizeAreaType(request.areaType);
                string normalizedDataType = NormalizeDataType(request.dataType);
                string mappedAddress = BuildBlockAddress(request);

                byte[] buffer;

                if (IsBitArea(normalizedAreaType))
                {
                    M_OperateResult<bool[]> readBool = _modbusTCPClient.ReadBool(mappedAddress, (ushort)Math.Max(1, request.length));
                    if (!readBool.IsSuccess)
                    {
                        return M_Return<M_BlockData>.Error(readBool.Message);
                    }

                    buffer = BoolArrayToByte(readBool.Content ?? new bool[0]);
                }
                else
                {
                    int registerLength = ResolveRegisterLength(normalizedDataType, request.length, request.stringLength, request.arrayLength);
                    M_OperateResult<byte[]> read = _modbusTCPClient.Read(mappedAddress, (ushort)Math.Max(1, registerLength));
                    if (!read.IsSuccess)
                    {
                        return M_Return<M_BlockData>.Error(read.Message);
                    }

                    buffer = TrimBlockBuffer(read.Content, normalizedDataType, request.length, request.stringLength, request.arrayLength);
                }

                return M_Return<M_BlockData>.OK(
                    new M_BlockData
                    {
                        areaType = request.areaType ?? string.Empty,
                        startAddress = request.startAddress ?? string.Empty,
                        length = request.length,
                        dataType = request.dataType ?? string.Empty,
                        buffer = buffer ?? new byte[0],
                        valueText = BuildBlockValueText(buffer)
                    });
            }
            catch (Exception ex)
            {
                return M_Return<M_BlockData>.Error("块读取异常: " + ex.Message);
            }
        }

        public M_Return<M_BlockData> WriteBlock(M_BlockWriteRequest request)
        {
            try
            {
                if (request == null)
                {
                    return M_Return<M_BlockData>.Error("块写入请求不能为空");
                }

                if (request.buffer == null || request.buffer.Length == 0)
                {
                    return M_Return<M_BlockData>.Error("块写入缓冲区不能为空");
                }

                string normalizedAreaType = NormalizeAreaType(request.areaType);
                string mappedAddress = BuildBlockAddress(request);

                M_OperateResult writeRes;

                if (IsBitArea(normalizedAreaType))
                {
                    bool[] bools = ByteToBoolArray(request.buffer);
                    writeRes = _modbusTCPClient.Write(mappedAddress, bools);
                }
                else
                {
                    byte[] writeBuffer = EnsureEvenLength(request.buffer);
                    writeRes = _modbusTCPClient.Write(mappedAddress, writeBuffer);
                }

                if (!writeRes.IsSuccess)
                {
                    return M_Return<M_BlockData>.Error(writeRes.Message);
                }

                return M_Return<M_BlockData>.OK(
                    new M_BlockData
                    {
                        areaType = request.areaType ?? string.Empty,
                        startAddress = request.startAddress ?? string.Empty,
                        length = request.buffer.Length,
                        dataType = request.dataType ?? string.Empty,
                        buffer = request.buffer,
                        valueText = BuildBlockValueText(request.buffer)
                    });
            }
            catch (Exception ex)
            {
                return M_Return<M_BlockData>.Error("块写入异常: " + ex.Message);
            }
        }

        private static byte GetStationNo(M_ProtocolOptions options)
        {
            if (options == null || !options.stationNo.HasValue || options.stationNo.Value <= 0)
            {
                return 1;
            }

            return (byte)options.stationNo.Value;
        }

        private static string NormalizeAreaType(string areaType)
        {
            return (areaType ?? string.Empty).Trim().Replace(" ", string.Empty).ToLowerInvariant();
        }

        private static string NormalizeDataType(string dataType)
        {
            string type = (dataType ?? string.Empty).Trim().Replace(" ", string.Empty).ToLowerInvariant();

            switch (type)
            {
                case "bit":
                    return "bool";
                case "short":
                    return "int16";
                case "ushort":
                    return "uint16";
                case "int":
                    return "int32";
                case "uint":
                    return "uint32";
                case "float":
                    return "single";
                case "long":
                    return "int64";
                case "ulong":
                    return "uint64";
                default:
                    return type;
            }
        }

        private static bool IsBitArea(string areaType)
        {
            return string.Equals(areaType, "coil", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(areaType, "discreteinput", StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildPointAddress(M_PointReadRequest request)
        {
            return BuildAddress(request.areaType, request.address, request.bitIndex, request.dataType, request.stringLength);
        }

        private static string BuildPointAddress(M_PointWriteRequest request)
        {
            return BuildAddress(request.areaType, request.address, request.bitIndex, request.dataType, request.stringLength);
        }

        private static string BuildBlockAddress(M_BlockReadRequest request)
        {
            return BuildAddress(request.areaType, request.startAddress, null, request.dataType, request.stringLength);
        }

        private static string BuildBlockAddress(M_BlockWriteRequest request)
        {
            return BuildAddress(request.areaType, request.startAddress, null, request.dataType, request.stringLength);
        }

        private static string BuildAddress(string areaType, string address, short? bitIndex, string dataType, int stringLength)
        {
            string normalizedAreaType = NormalizeAreaType(areaType);
            string normalizedDataType = NormalizeDataType(dataType);
            string rawAddress = (address ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(rawAddress))
            {
                throw new InvalidOperationException("地址不能为空");
            }

            if (rawAddress.IndexOf(';') >= 0)
            {
                return AppendStringLength(rawAddress, normalizedDataType, stringLength);
            }

            string mappedAddress;

            switch (normalizedAreaType)
            {
                case "coil":
                    mappedAddress = NormalizePrefixedAddress(rawAddress, '0');
                    break;
                case "discreteinput":
                    mappedAddress = NormalizePrefixedAddress(rawAddress, '1');
                    break;
                case "inputregister":
                    mappedAddress = NormalizePrefixedAddress(rawAddress, '3');
                    break;
                case "holdingregister":
                default:
                    mappedAddress = NormalizePrefixedAddress(rawAddress, '4');
                    break;
            }

            if (bitIndex.HasValue &&
                bitIndex.Value >= 0 &&
                !IsBitArea(normalizedAreaType) &&
                string.Equals(normalizedDataType, "bool", StringComparison.OrdinalIgnoreCase) &&
                mappedAddress.IndexOf('.') < 0)
            {
                mappedAddress = mappedAddress + "." + bitIndex.Value;
            }

            return AppendStringLength(mappedAddress, normalizedDataType, stringLength);
        }

        private static string NormalizePrefixedAddress(string rawAddress, char areaPrefix)
        {
            string text = (rawAddress ?? string.Empty).Trim();

            if (text.IndexOf('[') > 0 || text.IndexOf('.') > 0)
            {
                return text;
            }

            if (text.Length == 5 && text[0] == areaPrefix)
            {
                return text;
            }

            int number;
            if (int.TryParse(text, out number))
            {
                return areaPrefix + number.ToString("0000");
            }

            return text;
        }

        private static string AppendStringLength(string address, string dataType, int stringLength)
        {
            if (!string.Equals(dataType, "string", StringComparison.OrdinalIgnoreCase))
            {
                return address;
            }

            if (stringLength <= 0)
            {
                return address;
            }

            if (address.IndexOf('[') >= 0)
            {
                return address;
            }

            return address + "[" + stringLength + "]";
        }

        private static int ResolveRegisterLength(string dataType, int length, int stringLength, int arrayLength)
        {
            switch (dataType)
            {
                case "string":
                    return Math.Max(1, (stringLength > 0 ? stringLength : Math.Max(1, length) + 1) / 2);
                case "bytearray":
                    return Math.Max(1, (arrayLength > 0 ? arrayLength : Math.Max(1, length) + 1) / 2);
                default:
                    return Math.Max(1, length);
            }
        }

        private static byte[] TrimBlockBuffer(byte[] buffer, string dataType, int length, int stringLength, int arrayLength)
        {
            if (buffer == null)
            {
                return new byte[0];
            }

            if (string.Equals(dataType, "string", StringComparison.OrdinalIgnoreCase))
            {
                int targetLength = stringLength > 0 ? stringLength : length;
                return TrimToLength(buffer, targetLength);
            }

            if (string.Equals(dataType, "bytearray", StringComparison.OrdinalIgnoreCase))
            {
                int targetLength = arrayLength > 0 ? arrayLength : length;
                return TrimToLength(buffer, targetLength);
            }

            return buffer;
        }

        private static byte[] TrimToLength(byte[] buffer, int length)
        {
            int actualLength = Math.Min(buffer.Length, Math.Max(0, length));
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

        private static bool[] ByteToBoolArray(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return new bool[0];
            }

            bool[] values = new bool[buffer.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = (buffer[i / 8] & (1 << (i % 8))) != 0;
            }

            return values;
        }

        private static string BuildBlockValueText(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return string.Empty;
            }

            return BitConverter.ToString(buffer).Replace("-", " ");
        }
    }
}