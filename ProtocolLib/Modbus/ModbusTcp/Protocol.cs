using ProtocolLib.CommonLib.Common;
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
    /// 当前实现保持最小职责：
    /// 1. 管理协议连接生命周期；
    /// 2. 统一解析点位请求；
    /// 3. 将请求分发到单值、字符串、数组读写；
    /// 4. 不在此处重复实现底层标量/字符串类型映射逻辑。
    /// </summary>
    public class Protocol : IProtocol
    {
        /// <summary>
        /// 协议名称。
        /// 用于协议注册表按名称发现与创建实现。
        /// </summary>
        public static readonly string ProtocolName = "modbustcp";

        /// <summary>
        /// Modbus TCP 客户端实例。
        /// 在当前协议对象生命周期内复用，避免重复创建底层连接对象。
        /// </summary>
        private ModbusTCP _modbusTCPClient;

        /// <summary>
        /// 最近一次连接结果。
        /// 用于快速判断当前协议对象是否处于连接成功状态。
        /// </summary>
        private M_OperateResult _connect;

        /// <summary>
        /// 当前协议配置。
        /// Configure 成功后缓存，用于 Connect / Reconnect 时复用。
        /// </summary>
        private M_ProtocolOptions _options;

        /// <summary>
        /// 点位读写辅助工具。
        /// 当前仅负责标量与字符串读写，数组读写在 Protocol 中直接处理。
        /// </summary>
        private readonly CollectionUtil _collectionUtil = new CollectionUtil();

        /// <summary>
        /// 初始化协议对象。
        /// </summary>
        public Protocol()
        {
        }

        /// <summary>
        /// 配置协议实例。
        /// 仅缓存配置；若底层客户端已存在，则同步刷新连接参数。
        /// </summary>
        /// <param name="options">协议配置参数。</param>
        /// <returns>配置结果。</returns>
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

        /// <summary>
        /// 建立与 PLC 的连接。
        /// 若底层客户端尚未创建，则按当前配置创建；否则更新连接参数后重连。
        /// </summary>
        /// <returns>连接结果。</returns>
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

        /// <summary>
        /// 断开当前连接。
        /// 若当前未连接，则直接返回成功，避免重复关闭报错。
        /// </summary>
        /// <returns>断开结果。</returns>
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

        /// <summary>
        /// 重新连接 PLC。
        /// 内部先执行 Disconnect，再执行 Connect。
        /// </summary>
        /// <returns>重连结果。</returns>
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

        /// <summary>
        /// 查询当前是否已连接。
        /// </summary>
        /// <returns>连接状态。</returns>
        public M_Return<bool> IsConnected()
        {
            return M_Return<bool>.OK(_connect != null && _connect.IsSuccess);
        }

        /// <summary>
        /// 读取点位。
        /// 统一入口，内部根据解析结果分发到：
        /// 1. 单值/字符串读取；
        /// 2. 数组读取。
        /// </summary>
        /// <param name="request">点位读取请求。</param>
        /// <returns>读取结果。</returns>
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

                string address;
                string dataType;
                int length;
                bool isString;
                bool isArray;
                string elementType;

                ToolBasic.ResolvePointRequest(
                    request.address,
                    request.dataType,
                    request.length,
                    out address,
                    out dataType,
                    out length,
                    out isString,
                    out isArray,
                    out elementType);

                if (string.IsNullOrWhiteSpace(address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                if (string.IsNullOrWhiteSpace(dataType))
                {
                    return M_Return<M_PointData>.Error("点位数据类型不能为空");
                }

                if (!isArray)
                {
                    return ReadSingle(address, dataType, length);
                }

                return ReadArray(address, dataType, elementType, length);
            }
            catch (Exception ex)
            {
                return M_Return<M_PointData>.Error("点位读取异常: " + ex.Message);
            }
        }

        /// <summary>
        /// 写入点位。
        /// 统一入口，内部根据解析结果分发到：
        /// 1. 单值/字符串写入；
        /// 2. 数组写入。
        /// </summary>
        /// <param name="request">点位写入请求。</param>
        /// <returns>写入结果。</returns>
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

                string address;
                string dataType;
                int length;
                bool isString;
                bool isArray;
                string elementType;

                ToolBasic.ResolvePointRequest(
                    request.address,
                    request.dataType,
                    request.length,
                    out address,
                    out dataType,
                    out length,
                    out isString,
                    out isArray,
                    out elementType);

                if (string.IsNullOrWhiteSpace(address))
                {
                    return M_Return<M_PointData>.Error("点位地址不能为空");
                }

                if (string.IsNullOrWhiteSpace(dataType))
                {
                    return M_Return<M_PointData>.Error("点位数据类型不能为空");
                }

                if (!isArray)
                {
                    return WriteSingle(address, dataType, length, request.value);
                }

                return WriteArray(address, dataType, elementType, length, request.value);
            }
            catch (Exception ex)
            {
                return M_Return<M_PointData>.Error("点位写入异常: " + ex.Message);
            }
        }

        /// <summary>
        /// 读取单值或字符串点位。
        /// 标量类型长度固定为 1；字符串长度由上层解析结果决定。
        /// </summary>
        /// <param name="address">已归一化的地址。</param>
        /// <param name="dataType">已归一化的数据类型。</param>
        /// <param name="length">已归一化的长度。</param>
        /// <returns>点位读取结果。</returns>
        private M_Return<M_PointData> ReadSingle(string address, string dataType, int length)
        {
            ushort readLength = (ushort)(string.Equals(dataType, "string", StringComparison.OrdinalIgnoreCase)
                ? (length <= 0 ? 1 : length)
                : 1);

            M_Return<M_GatherData> result = _collectionUtil.ReadData(_modbusTCPClient, string.Empty, address, dataType, readLength);
            if (!result.Status)
            {
                return M_Return<M_PointData>.Error(result.DescMsg);
            }

            return M_Return<M_PointData>.OK(CreatePointData(address, dataType, readLength, result.Result));
        }

        /// <summary>
        /// 写入单值或字符串点位。
        /// 标量类型长度固定为 1；字符串长度由上层解析结果决定。
        /// </summary>
        /// <param name="address">已归一化的地址。</param>
        /// <param name="dataType">已归一化的数据类型。</param>
        /// <param name="length">已归一化的长度。</param>
        /// <param name="value">待写入值。</param>
        /// <returns>点位写入结果。</returns>
        private M_Return<M_PointData> WriteSingle(string address, string dataType, int length, object value)
        {
            ushort writeLength = (ushort)(string.Equals(dataType, "string", StringComparison.OrdinalIgnoreCase)
                ? (length <= 0 ? 1 : length)
                : 1);

            M_Return<M_GatherData> result = _collectionUtil.WriteData(_modbusTCPClient, address, value, dataType, writeLength);
            if (!result.Status)
            {
                return M_Return<M_PointData>.Error(result.DescMsg);
            }

            return M_Return<M_PointData>.OK(CreatePointData(address, dataType, writeLength, result.Result));
        }

        /// <summary>
        /// 读取数组点位。
        /// Modbus 侧对非 bool 数组先换算寄存器数量读取，再按理论字节数裁剪结果。
        /// </summary>
        /// <param name="address">已归一化的地址。</param>
        /// <param name="dataType">完整数组类型名，例如 uint16[]。</param>
        /// <param name="elementType">基础元素类型，例如 uint16。</param>
        /// <param name="length">元素个数。</param>
        /// <returns>数组读取结果。</returns>
        private M_Return<M_PointData> ReadArray(string address, string dataType, string elementType, int length)
        {
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
                        rawBuffer = ToolBasic.BoolArrayToByte(values),
                        quality = "Good"
                    });
            }

            int registerLength = CollectionUtil.ResolveWordRegisterLengthForArray(elementType, length);
            int byteLength = ToolBasic.ResolveByteLengthForArray(elementType, length);

            M_OperateResult<byte[]> read = _modbusTCPClient.Read(address, (ushort)Math.Max(1, registerLength));
            if (!read.IsSuccess)
            {
                return M_Return<M_PointData>.Error(read.Message);
            }

            byte[] buffer = ToolBasic.TrimBuffer(read.Content, byteLength);
            return M_Return<M_PointData>.OK(
                new M_PointData
                {
                    address = address,
                    dataType = dataType,
                    length = length,
                    value = ToolBasic.BuildBufferValueText(buffer),
                    rawBuffer = buffer,
                    quality = "Good"
                });
        }

        /// <summary>
        /// 写入数组点位。
        /// bool[] 直接按布尔数组写入；
        /// 其他数组当前统一要求上层传入 byte[] 原始缓冲区。
        /// </summary>
        /// <param name="address">已归一化的地址。</param>
        /// <param name="dataType">完整数组类型名，例如 uint16[]。</param>
        /// <param name="elementType">基础元素类型，例如 uint16。</param>
        /// <param name="length">元素个数。</param>
        /// <param name="value">待写入值。</param>
        /// <returns>数组写入结果。</returns>
        private M_Return<M_PointData> WriteArray(string address, string dataType, string elementType, int length, object value)
        {
            if (string.Equals(elementType, "bool", StringComparison.OrdinalIgnoreCase))
            {
                bool[] boolValues = value as bool[];
                if (boolValues == null)
                {
                    return M_Return<M_PointData>.Error("bool[] 写入值必须为 bool[]");
                }

                M_OperateResult writeBool = _modbusTCPClient.Write(address, boolValues);
                if (!writeBool.IsSuccess)
                {
                    return M_Return<M_PointData>.Error(writeBool.Message);
                }

                return M_Return<M_PointData>.OK(
                    new M_PointData
                    {
                        address = address,
                        dataType = dataType,
                        length = length,
                        value = string.Join(",", boolValues.Select(p => p ? "1" : "0").ToArray()),
                        rawBuffer = ToolBasic.BoolArrayToByte(boolValues),
                        quality = "Good"
                    });
            }

            byte[] buffer = value as byte[];
            if (buffer == null)
            {
                return M_Return<M_PointData>.Error("数组写入值当前必须为 byte[]");
            }

            M_OperateResult write = _modbusTCPClient.Write(address, ToolBasic.EnsureEvenLength(buffer));
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
                    value = ToolBasic.BuildBufferValueText(buffer),
                    rawBuffer = buffer,
                    quality = "Good"
                });
        }

        /// <summary>
        /// 构造统一点位结果对象。
        /// 单值与字符串路径统一通过该方法组装返回模型。
        /// </summary>
        /// <param name="address">点位地址。</param>
        /// <param name="dataType">点位数据类型。</param>
        /// <param name="length">长度。</param>
        /// <param name="gatherData">底层采集结果。</param>
        /// <returns>统一点位结果对象。</returns>
        private static M_PointData CreatePointData(string address, string dataType, int length, M_GatherData gatherData)
        {
            return new M_PointData
            {
                address = address,
                dataType = dataType,
                length = length <= 0 ? 1 : length,
                value = gatherData == null ? string.Empty : (gatherData.value ?? string.Empty),
                rawBuffer = new byte[0],
                quality = "Good"
            };
        }

        /// <summary>
        /// 解析站号。
        /// 当配置为空或站号无效时，回退为默认站号 1。
        /// </summary>
        /// <param name="options">协议配置。</param>
        /// <returns>站号。</returns>
        private static byte GetStationNo(M_ProtocolOptions options)
        {
            if (options == null || !options.stationNo.HasValue || options.stationNo.Value <= 0)
            {
                return 1;
            }

            return (byte)options.stationNo.Value;
        }
    }
}