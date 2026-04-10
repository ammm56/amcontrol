using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.ModbusTcp.Common;
using ProtocolLib.ModbusTcp.Core;
using System;

namespace ProtocolLib.ModbusTcp
{
    /// <summary>
    /// Modbus TCP 协议统一入口。
    /// 当前版本直接使用 Address 作为完整协议地址，不再拆分和拼接地址区域。
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

                if (string.IsNullOrWhiteSpace(request.address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                string dataType = NormalizeDataType(request.dataType);
                M_Return<M_GatherData> result = _collectionUtil.ReadData(_modbusTCPClient, string.Empty, request.address.Trim(), dataType);
                if (!result.Status)
                {
                    return M_Return<M_PointData>.Error(result.DescMsg);
                }

                M_GatherData gatherData = result.Result ?? new M_GatherData();
                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        address = request.address ?? string.Empty,
                        dataType = request.dataType ?? string.Empty,
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

                if (string.IsNullOrWhiteSpace(request.address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                string dataType = NormalizeDataType(request.dataType);
                M_Return<M_GatherData> result = _collectionUtil.WriteData(_modbusTCPClient, request.address.Trim(), request.value, dataType);
                if (!result.Status)
                {
                    return M_Return<M_PointData>.Error(result.DescMsg);
                }

                M_GatherData gatherData = result.Result ?? new M_GatherData();
                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
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

                if (_modbusTCPClient == null)
                {
                    return M_Return<M_BlockData>.Error("协议未连接");
                }

                if (string.IsNullOrWhiteSpace(request.startAddress))
                {
                    return M_Return<M_BlockData>.Error("块起始地址不能为空");
                }

                string startAddress = request.startAddress.Trim();
                string dataType = NormalizeDataType(request.dataType);
                byte[] buffer;

                if (IsBitAddress(startAddress))
                {
                    M_OperateResult<bool[]> readBool = _modbusTCPClient.ReadBool(startAddress, (ushort)Math.Max(1, request.length));
                    if (!readBool.IsSuccess)
                    {
                        return M_Return<M_BlockData>.Error(readBool.Message);
                    }

                    buffer = BoolArrayToByte(readBool.Content ?? new bool[0]);
                }
                else
                {
                    int registerLength = ResolveRegisterLength(dataType, request.length, request.stringLength, request.arrayLength);
                    M_OperateResult<byte[]> read = _modbusTCPClient.Read(startAddress, (ushort)Math.Max(1, registerLength));
                    if (!read.IsSuccess)
                    {
                        return M_Return<M_BlockData>.Error(read.Message);
                    }

                    buffer = TrimBlockBuffer(read.Content, dataType, request.length, request.stringLength, request.arrayLength);
                }

                return M_Return<M_BlockData>.OK(
                    new M_BlockData
                    {
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

                if (_modbusTCPClient == null)
                {
                    return M_Return<M_BlockData>.Error("协议未连接");
                }

                if (string.IsNullOrWhiteSpace(request.startAddress))
                {
                    return M_Return<M_BlockData>.Error("块起始地址不能为空");
                }

                if (request.buffer == null || request.buffer.Length == 0)
                {
                    return M_Return<M_BlockData>.Error("块写入缓冲区不能为空");
                }

                string startAddress = request.startAddress.Trim();
                M_OperateResult writeResult;

                if (IsBitAddress(startAddress))
                {
                    bool[] bools = ByteToBoolArray(request.buffer);
                    writeResult = _modbusTCPClient.Write(startAddress, bools);
                }
                else
                {
                    byte[] writeBuffer = EnsureEvenLength(request.buffer);
                    writeResult = _modbusTCPClient.Write(startAddress, writeBuffer);
                }

                if (!writeResult.IsSuccess)
                {
                    return M_Return<M_BlockData>.Error(writeResult.Message);
                }

                return M_Return<M_BlockData>.OK(
                    new M_BlockData
                    {
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

        private static bool IsBitAddress(string address)
        {
            string text = (address ?? string.Empty).Trim();
            if (text.Length >= 5 && char.IsDigit(text[0]))
            {
                return text[0] == '0' || text[0] == '1';
            }

            return false;
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