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
    /// 当前版本仅负责：
    /// 1. 从协议注册表解析协议实现；
    /// 2. 创建并复用协议实例；
    /// 3. 将 AM 层调用直接转发到 ProtocolLib 模型。
    /// </summary>
    internal class ProtocolPlcClient : IPlcClient
    {
        /// <summary>
        /// PLC 站配置。
        /// </summary>
        private readonly PlcStationConfig _stationConfig;

        /// <summary>
        /// 客户端配置。
        /// </summary>
        private M_ProtocolOptions _options;

        /// <summary>
        /// 协议实例。
        /// 单个客户端生命周期内复用，不重复创建。
        /// </summary>
        private IProtocol _protocol;

        /// <summary>
        /// 协议实例创建同步锁。
        /// </summary>
        private readonly object _protocolSyncRoot = new object();

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

        public Result Configure(M_ProtocolOptions options)
        {
            try
            {
                if (options == null)
                {
                    return Result.Fail(-3601, "PLC 客户端配置不能为空", ResultSource.Plc);
                }

                bool protocolTypeChanged = !string.Equals(
                    NormalizeProtocolType(GetEffectiveProtocolType(_options)),
                    NormalizeProtocolType(options.protocolType),
                    StringComparison.OrdinalIgnoreCase);

                _options = options;

                if (protocolTypeChanged)
                {
                    lock (_protocolSyncRoot)
                    {
                        _protocol = null;
                    }
                }

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return ensureResult;
                }

                M_Return<bool> configureResult = _protocol.Configure(options);
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
                if (result == null || !result.Status)
                {
                    return Result<bool>.Fail(
                        -3610,
                        result == null || string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 连接状态查询失败" : result.DescMsg,
                        ResultSource.Plc);
                }

                return Result<bool>.OkItem(result.Result, "PLC 连接状态查询成功", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(-3611, "PLC 连接状态查询异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result<M_PointData> ReadPoint(M_PointReadRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result<M_PointData>.Fail(-3612, "点位读取请求不能为空", ResultSource.Plc);
                }

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return Result<M_PointData>.Fail(ensureResult.Code, ensureResult.Message, ResultSource.Plc);
                }

                M_Return<M_PointData> result = _protocol.ReadPoint(request);
                if (result == null || !result.Status || result.Result == null)
                {
                    return Result<M_PointData>.Fail(
                        -3613,
                        result == null || string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 点位读取失败" : result.DescMsg,
                        ResultSource.Plc);
                }

                return Result<M_PointData>.OkItem(result.Result, "PLC 点位读取成功", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<M_PointData>.Fail(-3614, "PLC 点位读取异常: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result<M_PointData> WritePoint(M_PointWriteRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result<M_PointData>.Fail(-3615, "点位写入请求不能为空", ResultSource.Plc);
                }

                Result ensureResult = EnsureProtocol();
                if (!ensureResult.Success)
                {
                    return Result<M_PointData>.Fail(ensureResult.Code, ensureResult.Message, ResultSource.Plc);
                }

                M_Return<M_PointData> result = _protocol.WritePoint(request);
                if (result == null || !result.Status || result.Result == null)
                {
                    return Result<M_PointData>.Fail(
                        -3616,
                        result == null || string.IsNullOrWhiteSpace(result.DescMsg) ? "PLC 点位写入失败" : result.DescMsg,
                        ResultSource.Plc);
                }

                return Result<M_PointData>.OkItem(result.Result, "PLC 点位写入成功", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<M_PointData>.Fail(-3617, "PLC 点位写入异常: " + ex.Message, ResultSource.Plc);
            }
        }

        /// <summary>
        /// 确保协议实例已创建。
        /// 协议实例在客户端生命周期内复用。
        /// </summary>
        private Result EnsureProtocol()
        {
            if (_protocol != null)
            {
                return Result.Ok("协议实例已存在", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }

            lock (_protocolSyncRoot)
            {
                if (_protocol != null)
                {
                    return Result.Ok("协议实例已存在", ResultSource.Plc)
                        .WithNotifyMode(ResultNotifyMode.Silent);
                }

                string protocolType = GetEffectiveProtocolType(_options);
                if (string.IsNullOrWhiteSpace(protocolType))
                {
                    return Result.Fail(-3624, "PLC 协议类型不能为空", ResultSource.Plc);
                }

                Type protocolImplType;
                if (!ProtocolAssemblyRegistry.TryResolve(protocolType, out protocolImplType) || protocolImplType == null)
                {
                    string keys = string.Join(", ", ProtocolAssemblyRegistry.GetRegisteredKeys());
                    string message = string.IsNullOrWhiteSpace(keys)
                        ? "未发现任何已注册 PLC 协议实现"
                        : "未找到匹配的 PLC 协议实现，当前已注册: " + keys;

                    return Result.Fail(-3625, message + "；请求协议类型: " + protocolType, ResultSource.Plc);
                }

                object instance;
                try
                {
                    instance = Activator.CreateInstance(protocolImplType);
                }
                catch (Exception ex)
                {
                    return Result.Fail(-3626, "协议实例创建失败: " + protocolImplType.FullName + "，异常: " + ex.Message, ResultSource.Plc);
                }

                IProtocol protocol = instance as IProtocol;
                if (protocol == null)
                {
                    return Result.Fail(-3627, "协议实现未正确实现 IProtocol: " + protocolImplType.FullName, ResultSource.Plc);
                }

                _protocol = protocol;
                return Result.Ok("协议实例创建成功", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
        }

        private string GetEffectiveProtocolType(M_ProtocolOptions options)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.protocolType))
            {
                return options.protocolType;
            }

            return _stationConfig.ProtocolType;
        }

        private static string NormalizeProtocolType(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
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