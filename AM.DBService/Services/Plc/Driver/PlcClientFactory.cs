using AM.Model.Interfaces.Plc;
using AM.Model.Plc;

namespace AM.DBService.Services.Plc.Driver
{
    /// <summary>
    /// PLC 客户端工厂。
    /// 首版先返回占位客户端，后续逐步切换到真实协议实现。
    /// </summary>
    public class PlcClientFactory : IPlcClientFactory
    {
        public IPlcClient Create(PlcStationConfig stationConfig)
        {
            return new NullPlcClient(stationConfig);
        }
    }
}