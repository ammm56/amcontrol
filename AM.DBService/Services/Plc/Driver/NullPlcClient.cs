using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;

namespace AM.DBService.Services.Plc.Driver
{
    /// <summary>
    /// 占位 PLC 客户端。
    /// 在真实协议客户端尚未实现前，用于打通配置装配与 MachineContext 注册链路。
    /// </summary>
    internal class NullPlcClient : IPlcClient
    {
        public NullPlcClient(PlcStationConfig stationConfig)
        {
            _stationConfig = stationConfig ?? new PlcStationConfig();
        }

        private readonly PlcStationConfig _stationConfig;

        public string PlcName
        {
            get { return _stationConfig.Name; }
        }

        public string ProtocolType
        {
            get { return _stationConfig.ProtocolType; }
        }

        public string ConnectionType
        {
            get { return _stationConfig.ConnectionType; }
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

        public Result<bool> IsConnected()
        {
            return Result<bool>.OkItem(false, "PLC 占位客户端未连接", ResultSource.Plc)
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        public Result<PlcRawDataBlock> ReadBlock(string areaType, string startAddress, int length, string dataType)
        {
            return Result<PlcRawDataBlock>.Fail(-3202, "PLC 读块功能尚未实现: " + PlcName, ResultSource.Plc);
        }

        public Result Write(string areaType, string address, string dataType, object value, short? bitIndex = null, int stringLength = 0, int arrayLength = 0)
        {
            return Result.Fail(-3203, "PLC 写入功能尚未实现: " + PlcName, ResultSource.Plc);
        }

        public Result WriteBlock(string areaType, string startAddress, byte[] buffer)
        {
            return Result.Fail(-3204, "PLC 块写入功能尚未实现: " + PlcName, ResultSource.Plc);
        }
    }
}