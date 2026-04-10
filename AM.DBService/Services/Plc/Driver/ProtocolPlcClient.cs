using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using System;

namespace AM.DBService.Services.Plc.Driver
{
    /// <summary>
    /// PLC 协议统一门面客户端。
    /// 负责反射创建协议库实现，并完成 AM 与 ProtocolLib 之间的请求/结果转换。
    /// </summary>
    internal class ProtocolPlcClient : IPlcClient
    {
        private readonly PlcStationConfig _stationConfig;
        private PlcProtocolClientOptions _options;
        private IProtocol _protocol;

        public ProtocolPlcClient(PlcStationConfig stationConfig)
        {
            _stationConfig = stationConfig ?? new PlcStationConfig();
        }

        public string PlcName
        {
            get { return _stationConfig.Name ?? string.Empty; }
        }

        public string ProtocolType
        {
            get { return _stationConfig.ProtocolType ?? string.Empty; }
        }

        public string ConnectionType
        {
            get { return _stationConfig.ConnectionType ?? string.Empty; }
        }

        public Result Configure(PlcProtocolClientOptions options)
        {
            try
            {
                if (options == null)
                {
                    return Result.Fail(-3601, "PLC 客户端配置不能为空", ResultSource.Plc);
                }

                _options = options;

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return ensureResult;
                }

                M_Return<bool> configureResult = _protocol.Configure(ToProtocolOptions(options));
                return ToResult(configureResult, "PLC 客户端配置成功", -3602, "PLC 客户端配置失败");
            }
            catch (Exception ex)
            {
                return Result.Fail(-3603, "PLC 客户端配置异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result Connect()
        {
            try
            {
                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return ensureResult;
                }

                M_Return<bool> result = _protocol.Connect();
                return ToResult(result, "PLC 连接成功", -3604, "PLC 连接失败");
            }
            catch (Exception ex)
            {
                return Result.Fail(-3605, "PLC 连接异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result Disconnect()
        {
            try
            {
                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return ensureResult;
                }

                M_Return<bool> result = _protocol.Disconnect();
                return ToResult(result, "PLC 断开成功", -3606, "PLC 断开失败");
            }
            catch (Exception ex)
            {
                return Result.Fail(-3607, "PLC 断开异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result Reconnect()
        {
            try
            {
                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return ensureResult;
                }

                M_Return<bool> result = _protocol.Reconnect();
                return ToResult(result, "PLC 重连成功", -3608, "PLC 重连失败");
            }
            catch (Exception ex)
            {
                return Result.Fail(-3609, "PLC 重连异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result<bool> IsConnected()
        {
            try
            {
                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return Result<bool>.Fail(ensureResult.Code, ensureResult.Message, ResultSource.Plc);
                }

                M_Return<bool> result = _protocol.IsConnected();
                if (!result.Status)
                {
                    return Result<bool>.Fail(-3610, string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 连接状态查询失败" : result.DescMsg, ResultSource.Plc);
                }

                return Result<bool>.OkItem(result.Result, "PLC 连接状态查询成功", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(-3611, "PLC 连接状态查询异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result<PlcPointReadResult> ReadPoint(PlcPointReadRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result<PlcPointReadResult>.Fail(-3612, "点位读取请求不能为空", ResultSource.Plc);
                }

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return Result<PlcPointReadResult>.Fail(ensureResult.Code, ensureResult.Message, ResultSource.Plc);
                }

                M_Return<M_PointData> result = _protocol.ReadPoint(ToProtocolPointReadRequest(request));
                if (!result.Status || result.Result == null)
                {
                    return Result<PlcPointReadResult>.Fail(-3613, string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 点位读取失败" : result.DescMsg, ResultSource.Plc);
                }

                return Result<PlcPointReadResult>.OkItem(
                        new PlcPointReadResult
                        {
                            PlcName = request.PlcName ?? PlcName,
                            AreaType = request.AreaType,
                            Address = request.Address,
                            DataType = request.DataType,
                            ValueText = result.Result.value ?? string.Empty,
                            RawBuffer = result.Result.rawBuffer ?? new byte[0]
                        },
                        "PLC 点位读取成功",
                        ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<PlcPointReadResult>.Fail(-3614, "PLC 点位读取异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result<PlcPointReadResult> WritePoint(PlcPointWriteRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result<PlcPointReadResult>.Fail(-3615, "点位写入请求不能为空", ResultSource.Plc);
                }

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return Result<PlcPointReadResult>.Fail(ensureResult.Code, ensureResult.Message, ResultSource.Plc);
                }

                M_Return<M_PointData> result = _protocol.WritePoint(ToProtocolPointWriteRequest(request));
                if (!result.Status || result.Result == null)
                {
                    return Result<PlcPointReadResult>.Fail(-3616, string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 点位写入失败" : result.DescMsg, ResultSource.Plc);
                }

                return Result<PlcPointReadResult>.OkItem(
                        new PlcPointReadResult
                        {
                            PlcName = request.PlcName ?? PlcName,
                            AreaType = request.AreaType,
                            Address = request.Address,
                            DataType = request.DataType,
                            ValueText = result.Result.value ?? string.Empty,
                            RawBuffer = result.Result.rawBuffer ?? new byte[0]
                        },
                        "PLC 点位写入成功",
                        ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<PlcPointReadResult>.Fail(-3617, "PLC 点位写入异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result<PlcRawDataBlock> ReadBlock(PlcBlockReadRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result<PlcRawDataBlock>.Fail(-3618, "块读取请求不能为空", ResultSource.Plc);
                }

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return Result<PlcRawDataBlock>.Fail(ensureResult.Code, ensureResult.Message, ResultSource.Plc);
                }

                M_Return<M_BlockData> result = _protocol.ReadBlock(ToProtocolBlockReadRequest(request));
                if (!result.Status || result.Result == null)
                {
                    return Result<PlcRawDataBlock>.Fail(-3619, string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 块读取失败" : result.DescMsg, ResultSource.Plc);
                }

                return Result<PlcRawDataBlock>.OkItem(
                        new PlcRawDataBlock
                        {
                            PlcName = request.PlcName ?? PlcName,
                            AreaType = request.AreaType,
                            StartAddress = request.StartAddress,
                            Length = request.Length,
                            DataType = request.DataType,
                            Buffer = result.Result.buffer ?? new byte[0],
                            ReadTime = DateTime.Now
                        },
                        "PLC 块读取成功",
                        ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<PlcRawDataBlock>.Fail(-3620, "PLC 块读取异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result<PlcRawDataBlock> WriteBlock(PlcBlockWriteRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result<PlcRawDataBlock>.Fail(-3621, "块写入请求不能为空", ResultSource.Plc);
                }

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return Result<PlcRawDataBlock>.Fail(ensureResult.Code, ensureResult.Message, ResultSource.Plc);
                }

                M_Return<M_BlockData> result = _protocol.WriteBlock(ToProtocolBlockWriteRequest(request));
                if (!result.Status || result.Result == null)
                {
                    return Result<PlcRawDataBlock>.Fail(-3622, string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 块写入失败" : result.DescMsg, ResultSource.Plc);
                }

                return Result<PlcRawDataBlock>.OkItem(
                        new PlcRawDataBlock
                        {
                            PlcName = request.PlcName ?? PlcName,
                            AreaType = request.AreaType,
                            StartAddress = request.StartAddress,
                            Length = request.Buffer == null ? 0 : request.Buffer.Length,
                            DataType = request.DataType,
                            Buffer = result.Result.buffer ?? new byte[0],
                            ReadTime = DateTime.Now
                        },
                        "PLC 块写入成功",
                        ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<PlcRawDataBlock>.Fail(-3623, "PLC 块写入异常: " + ex.Message, ResultSource.Plc);
            }
        }

        private Result EnsureProtocol()
        {
            if (_protocol != null)
            {
                return Result.Ok("协议实例已存在", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }

            string assemblyQualifiedName = ResolveProtocolTypeName(_options == null ? _stationConfig.ProtocolType : _options.ProtocolType);
            if (string.IsNullOrWhiteSpace(assemblyQualifiedName))
            {
                return Result.Fail(-3624, "不支持的 PLC 协议类型: " + (_stationConfig.ProtocolType ?? string.Empty), ResultSource.Plc);
            }

            Type protocolType = Type.GetType(assemblyQualifiedName, false);
            if (protocolType == null)
            {
                return Result.Fail(-3625, "未找到协议实现类型: " + assemblyQualifiedName, ResultSource.Plc);
            }

            object instance = Activator.CreateInstance(protocolType);
            IProtocol protocol = instance as IProtocol;
            if (protocol == null)
            {
                return Result.Fail(-3626, "协议实现未正确实现 IProtocol: " + protocolType.FullName, ResultSource.Plc);
            }

            _protocol = protocol;
            return Result.Ok("协议实例创建成功", ResultSource.Plc)
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        private static string ResolveProtocolTypeName(string protocolType)
        {
            string value = (protocolType ?? string.Empty).Trim();

            if (value.IndexOf("modbus", StringComparison.OrdinalIgnoreCase) >= 0 &&
                value.IndexOf("tcp", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "ProtocolLib.ModbusTcp.Protocol, ProtocolLib.ModbusTcp";
            }

            if (value.IndexOf("s7", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "ProtocolLib.S7Tcp.Protocol, ProtocolLib.S7Tcp";
            }

            return null;
        }

        private static M_ProtocolOptions ToProtocolOptions(PlcProtocolClientOptions options)
        {
            return new M_ProtocolOptions
            {
                protocolType = options.ProtocolType ?? string.Empty,
                connectionType = options.ConnectionType ?? string.Empty,
                ip = options.IpAddress ?? string.Empty,
                port = options.Port,
                stationNo = options.StationNo,
                rack = options.Rack,
                slot = options.Slot,
                timeoutMs = options.TimeoutMs,
                byteOrder = options.ByteOrder ?? string.Empty,
                wordOrder = options.WordOrder ?? string.Empty,
                stringEncoding = options.StringEncoding ?? string.Empty
            };
        }

        private static M_PointReadRequest ToProtocolPointReadRequest(PlcPointReadRequest request)
        {
            return new M_PointReadRequest
            {
                areaType = request.AreaType ?? string.Empty,
                address = request.Address ?? string.Empty,
                dataType = request.DataType ?? string.Empty,
                bitIndex = request.BitIndex,
                stringLength = request.StringLength,
                arrayLength = request.ArrayLength
            };
        }

        private static M_PointWriteRequest ToProtocolPointWriteRequest(PlcPointWriteRequest request)
        {
            return new M_PointWriteRequest
            {
                areaType = request.AreaType ?? string.Empty,
                address = request.Address ?? string.Empty,
                dataType = request.DataType ?? string.Empty,
                value = request.Value,
                bitIndex = request.BitIndex,
                stringLength = request.StringLength,
                arrayLength = request.ArrayLength
            };
        }

        private static M_BlockReadRequest ToProtocolBlockReadRequest(PlcBlockReadRequest request)
        {
            return new M_BlockReadRequest
            {
                areaType = request.AreaType ?? string.Empty,
                startAddress = request.StartAddress ?? string.Empty,
                length = request.Length,
                dataType = request.DataType ?? string.Empty,
                stringLength = request.StringLength,
                arrayLength = request.ArrayLength
            };
        }

        private static M_BlockWriteRequest ToProtocolBlockWriteRequest(PlcBlockWriteRequest request)
        {
            return new M_BlockWriteRequest
            {
                areaType = request.AreaType ?? string.Empty,
                startAddress = request.StartAddress ?? string.Empty,
                dataType = request.DataType ?? string.Empty,
                buffer = request.Buffer ?? new byte[0],
                stringLength = request.StringLength,
                arrayLength = request.ArrayLength
            };
        }

        private static Result ToResult(M_Return<bool> result, string successMessage, int failCode, string failMessage)
        {
            if (result != null && result.Status)
            {
                return Result.Ok(successMessage, ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }

            return Result.Fail(
                failCode,
                result == null || string.IsNullOrWhiteSpace(result.DescMsg) ? failMessage : result.DescMsg,
                ResultSource.Plc);
        }
    }
}