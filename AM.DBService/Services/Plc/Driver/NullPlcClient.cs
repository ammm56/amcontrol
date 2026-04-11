using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;
using ProtocolLib.CommonLib.Model;

namespace AM.DBService.Services.Plc.Driver
{
    /// <summary>
    /// 占位 PLC 客户端。
    /// 在真实协议客户端不可用时，用于保持配置装配与运行时链路可继续工作。
    /// </summary>
    internal class NullPlcClient : IPlcClient
    {
        /// <summary>
        /// PLC 站配置。
        /// </summary>
        private readonly PlcStationConfig _stationConfig;

        /// <summary>
        /// 最近一次配置。
        /// </summary>
        private M_ProtocolOptions _options;

        public NullPlcClient(PlcStationConfig stationConfig)
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
            _options = options;

            return Result.Ok("PLC 占位客户端配置完成", ResultSource.Plc)
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        public Result Connect()
        {
            return Result.Fail(-3201, "PLC 协议客户端尚未实现: " + PlcName, ResultSource.Plc);
        }

        public Result Disconnect()
        {
            return Result.Ok("PLC 占位客户端断开完成", ResultSource.Plc)
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        public Result Reconnect()
        {
            return Result.Fail(-3205, "PLC 协议客户端尚未实现重连: " + PlcName, ResultSource.Plc);
        }

        public Result<bool> IsConnected()
        {
            return Result<bool>.OkItem(false, "PLC 占位客户端未连接", ResultSource.Plc)
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        public Result<M_PointData> ReadPoint(M_PointReadRequest request)
        {
            return Result<M_PointData>.Fail(-3206, "PLC 点位读取功能尚未实现: " + PlcName, ResultSource.Plc);
        }

        public Result<M_PointData> WritePoint(M_PointWriteRequest request)
        {
            return Result<M_PointData>.Fail(-3207, "PLC 点位写入功能尚未实现: " + PlcName, ResultSource.Plc);
        }
    }
}