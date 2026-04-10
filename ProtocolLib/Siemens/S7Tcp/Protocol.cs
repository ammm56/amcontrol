using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using ProtocolLib.S7Tcp.Common;
using ProtocolLib.S7Tcp.Core;
using ProtocolLib.S7Tcp.Model;
using System;

namespace ProtocolLib.S7Tcp
{
    /// <summary>
    /// Siemens S7 协议统一入口。
    /// 当前版本直接使用 Address 作为完整协议地址。
    /// 点位、字符串、同类型连续数组统一走点位读写接口。
    /// </summary>
    public class Protocol : IProtocol
    {
        /// <summary>
        /// 协议名。
        /// </summary>
        public static readonly string ProtocolName = "s7tcp";

        /// <summary>
        /// 协议客户端。
        /// </summary>
        private SiemensS7 _siemensS7;

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

                if (_siemensS7 != null)
                {
                    _siemensS7.UpdateIPPortInfo(_options.ip ?? string.Empty, _options.port <= 0 ? 102 : _options.port);
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
                int port = _options.port <= 0 ? 102 : _options.port;

                if (_siemensS7 == null)
                {
                    _siemensS7 = new SiemensS7(ResolvePlcType(_options), ip, port);
                }
                else
                {
                    _siemensS7.UpdateIPPortInfo(ip, port);
                }

                _connect = _siemensS7.Connection();
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
                if (_siemensS7 == null || _connect == null || !_connect.IsSuccess)
                {
                    _connect = null;
                    return M_Return<bool>.OK(true);
                }

                M_OperateResult result = _siemensS7.ConnectClose();
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

                if (_siemensS7 == null)
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

                M_Return<M_GatherData> result = _collectionUtil.ReadData(_siemensS7, string.Empty, address, dataType);
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

                if (_siemensS7 == null)
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

                M_Return<M_GatherData> result = _collectionUtil.WriteData(_siemensS7, address, request.value, dataType);
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
            M_Return<M_GatherData> result = _collectionUtil.ReadData(_siemensS7, string.Empty, actualAddress, "string");
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
            M_Return<M_GatherData> result = _collectionUtil.WriteData(_siemensS7, actualAddress, value, "string");
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
            int byteLength = ResolveReadByteLengthForArray(elementType, length);

            M_OperateResult<byte[]> read = _siemensS7.Read(address, (ushort)Math.Max(1, byteLength));
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
            byte[] buffer = value as byte[];
            if (buffer == null)
            {
                return M_Return<M_PointData>.Error("数组写入值当前必须为 byte[]");
            }

            M_OperateResult writeResult = _siemensS7.Write(address, buffer);
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
                    value = BuildBufferValueText(buffer),
                    rawBuffer = buffer,
                    quality = "Good"
                });
        }

        private static E_SiemensPLCS ResolvePlcType(M_ProtocolOptions options)
        {
            string protocolType = options == null ? string.Empty : (options.protocolType ?? string.Empty).Trim().ToUpperInvariant();

            if (protocolType.Contains("1500")) return E_SiemensPLCS.S1500;
            if (protocolType.Contains("300")) return E_SiemensPLCS.S300;
            if (protocolType.Contains("400")) return E_SiemensPLCS.S400;
            if (protocolType.Contains("200SMART")) return E_SiemensPLCS.S200Smart;
            if (protocolType.Contains("200")) return E_SiemensPLCS.S200;

            return E_SiemensPLCS.S1200;
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

        private static int ResolveReadByteLengthForArray(string dataType, int length)
        {
            switch (dataType)
            {
                case "bool":
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
                case "string":
                    return Math.Max(1, length);
                default:
                    return Math.Max(1, length);
            }
        }

        private static byte[] TrimArrayBuffer(byte[] buffer, string dataType, int length)
        {
            if (buffer == null)
            {
                return new byte[0];
            }

            int byteLength = ResolveReadByteLengthForArray(dataType, length);
            int actualLength = Math.Min(buffer.Length, Math.Max(0, byteLength));
            byte[] result = new byte[actualLength];
            Array.Copy(buffer, 0, result, 0, actualLength);
            return result;
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