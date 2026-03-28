using AM.Model.Plc;

namespace AM.Model.Interfaces.Plc
{
    /// <summary>
    /// PLC 客户端工厂接口。
    /// 按 PLC 站配置创建具体协议客户端。
    /// </summary>
    public interface IPlcClientFactory
    {
        /// <summary>
        /// 根据 PLC 站配置创建客户端。
        /// </summary>
        IPlcClient Create(PlcStationConfig stationConfig);
    }
}