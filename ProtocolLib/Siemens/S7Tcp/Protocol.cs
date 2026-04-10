using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.S7Tcp.Common;
using ProtocolLib.S7Tcp.Core;
using ProtocolLib.S7Tcp.Model;
using System;
using System.Collections.Generic;

namespace ProtocolLib.S7Tcp
{
    /// <summary>
    /// Siemens S7 协议统一入口。
    /// 对外暴露结构化点位/块读写接口，协议细节在内部完成转换。
    /// </summary>
    public class Protocol : IProtocol
    {
        /// <summary>
        /// 协议名
        /// </summary>
        public static readonly string ProtocolName = "s7tcp";

        /// <summary>
        /// 协议客户端
        /// </summary>
        private SiemensS7 _siemensS7 = null;

        /// <summary>
        /// 连接结果
        /// </summary>
        private M_OperateResult _connect = null;

        /// <summary>
        /// 协议配置
        /// </summary>
        private M_ProtocolConfig _protocolConfig = null;

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
                    port = options.port <= 0 ? 102 : options.port,
                    byteorder = 0,
                    pointinfo = new List<Point>()
                };

                if (_siemensS7 != null)
                {
                    _siemensS7.UpdateIPPortInfo(_protocolConfig.ip, _protocolConfig.port);
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

                if (_siemensS7 == null)
                {
                    _siemensS7 = new SiemensS7(ResolvePlcType(), _protocolConfig.ip, _protocolConfig.port);
                }
                else
                {
                    _siemensS7.UpdateIPPortInfo(_protocolConfig.ip, _protocolConfig.port);
                }

                _connect = _siemensS7.Connection();
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
                if (_siemensS7 == null || _connect == null || !_connect.IsSuccess)
                {
                    _connect = null;
                    return M_Return<bool>.OK(true);
                }

                M_OperateResult res = _siemensS7.ConnectClose();
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

                M_Return<M_GatherData> res = _collectionUtil.ReadData(_siemensS7, string.Empty, mappedAddress, mappedType);
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

                M_Return<M_GatherData> res = _collectionUtil.WriteData(_siemensS7, mappedAddress, request.value, mappedType);
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

                string normalizedDataType = NormalizeDataType(request.dataType);
                string mappedAddress = BuildBlockAddress(request);
                int byteLength = ResolveReadByteLength(normalizedDataType, request.length, request.stringLength, request.arrayLength);

                M_OperateResult<byte[]> read = _siemensS7.Read(mappedAddress, (ushort)Math.Max(1, byteLength));
                if (!read.IsSuccess)
                {
                    return M_Return<M_BlockData>.Error(read.Message);
                }

                byte[] buffer = TrimBlockBuffer(read.Content, normalizedDataType, request.length, request.stringLength, request.arrayLength);

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

                string mappedAddress = BuildBlockAddress(request);
                M_OperateResult writeRes = _siemensS7.Write(mappedAddress, request.buffer);
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

        private E_SiemensPLCS ResolvePlcType()
        {
            string protocolType = _protocolConfig == null ? string.Empty : (_protocolConfig.protocoltype ?? string.Empty).Trim().ToUpperInvariant();

            if (protocolType.Contains("1500")) return E_SiemensPLCS.S1500;
            if (protocolType.Contains("300")) return E_SiemensPLCS.S300;
            if (protocolType.Contains("400")) return E_SiemensPLCS.S400;
            if (protocolType.Contains("200SMART")) return E_SiemensPLCS.S200Smart;
            if (protocolType.Contains("200")) return E_SiemensPLCS.S200;

            return E_SiemensPLCS.S1200;
        }

        private static string NormalizeAreaType(string areaType)
        {
            return (areaType ?? string.Empty).Trim().Replace(" ", string.Empty).ToUpperInvariant();
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

            if (StartsWithS7Area(rawAddress))
            {
                return AppendStringLength(rawAddress, normalizedDataType, stringLength);
            }

            string mappedAddress;
            switch (normalizedAreaType)
            {
                case "DB":
                    mappedAddress = rawAddress.StartsWith("DB", StringComparison.OrdinalIgnoreCase) ? rawAddress : "DB" + rawAddress;
                    break;
                case "M":
                case "I":
                case "Q":
                case "V":
                case "T":
                case "C":
                    mappedAddress = normalizedAreaType + rawAddress;
                    break;
                default:
                    mappedAddress = rawAddress;
                    break;
            }

            if (bitIndex.HasValue &&
                bitIndex.Value >= 0 &&
                string.Equals(normalizedDataType, "bool", StringComparison.OrdinalIgnoreCase) &&
                mappedAddress.LastIndexOf('.') < 0)
            {
                mappedAddress = mappedAddress + "." + bitIndex.Value;
            }

            return AppendStringLength(mappedAddress, normalizedDataType, stringLength);
        }

        private static bool StartsWithS7Area(string address)
        {
            string upper = (address ?? string.Empty).Trim().ToUpperInvariant();
            return upper.StartsWith("DB") ||
                   upper.StartsWith("M") ||
                   upper.StartsWith("I") ||
                   upper.StartsWith("Q") ||
                   upper.StartsWith("V") ||
                   upper.StartsWith("T") ||
                   upper.StartsWith("C");
        }

        private static string AppendStringLength(string address, string dataType, int stringLength)
        {
            if (!string.Equals(dataType, "string", StringComparison.OrdinalIgnoreCase))
            {
                return address;
            }

            if (stringLength <= 0 || address.IndexOf('[') >= 0)
            {
                return address;
            }

            return address + "[" + stringLength + "]";
        }

        private static int ResolveReadByteLength(string dataType, int length, int stringLength, int arrayLength)
        {
            switch (dataType)
            {
                case "bool":
                case "byte":
                    return Math.Max(1, length);
                case "int16":
                case "uint16":
                    return Math.Max(1, length) * 2;
                case "int32":
                case "uint32":
                case "single":
                    return Math.Max(1, length) * 4;
                case "int64":
                case "uint64":
                case "double":
                    return Math.Max(1, length) * 8;
                case "string":
                    return Math.Max(1, stringLength > 0 ? stringLength : length);
                case "bytearray":
                    return Math.Max(1, arrayLength > 0 ? arrayLength : length);
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

            int targetLength;

            switch (dataType)
            {
                case "string":
                    targetLength = stringLength > 0 ? stringLength : length;
                    break;
                case "bytearray":
                    targetLength = arrayLength > 0 ? arrayLength : length;
                    break;
                default:
                    return buffer;
            }

            int actualLength = Math.Min(buffer.Length, Math.Max(0, targetLength));
            byte[] result = new byte[actualLength];
            Array.Copy(buffer, 0, result, 0, actualLength);
            return result;
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