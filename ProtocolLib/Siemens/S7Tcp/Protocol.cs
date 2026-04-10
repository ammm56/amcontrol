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
    /// 当前版本直接使用 Address 作为完整协议地址，不再拆分和拼接地址区域。
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

                if (string.IsNullOrWhiteSpace(request.address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                string dataType = NormalizeDataType(request.dataType);
                M_Return<M_GatherData> result = _collectionUtil.ReadData(_siemensS7, string.Empty, request.address.Trim(), dataType);
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

                if (_siemensS7 == null)
                {
                    return M_Return<M_PointData>.Error("协议未连接");
                }

                if (string.IsNullOrWhiteSpace(request.address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                string dataType = NormalizeDataType(request.dataType);
                M_Return<M_GatherData> result = _collectionUtil.WriteData(_siemensS7, request.address.Trim(), request.value, dataType);
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

                if (_siemensS7 == null)
                {
                    return M_Return<M_BlockData>.Error("协议未连接");
                }

                if (string.IsNullOrWhiteSpace(request.startAddress))
                {
                    return M_Return<M_BlockData>.Error("块起始地址不能为空");
                }

                string dataType = NormalizeDataType(request.dataType);
                int byteLength = ResolveReadByteLength(dataType, request.length, request.stringLength, request.arrayLength);

                M_OperateResult<byte[]> read = _siemensS7.Read(request.startAddress.Trim(), (ushort)Math.Max(1, byteLength));
                if (!read.IsSuccess)
                {
                    return M_Return<M_BlockData>.Error(read.Message);
                }

                byte[] buffer = TrimBlockBuffer(read.Content, dataType, request.length, request.stringLength, request.arrayLength);

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

                if (_siemensS7 == null)
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

                M_OperateResult writeResult = _siemensS7.Write(request.startAddress.Trim(), request.buffer);
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