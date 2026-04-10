using AM.Model.Common;
using AM.Model.Plc;

namespace AM.Model.Interfaces.Plc
{
    /// <summary>
    /// AM 侧统一 PLC 客户端门面。
    /// 负责统一上层调用入口，不负责协议行为实现。
    /// </summary>
    public interface IPlcClient
    {
        /// <summary>
        /// PLC 站名称。
        /// </summary>
        string PlcName { get; }

        /// <summary>
        /// 当前协议类型。
        /// </summary>
        string ProtocolType { get; }

        /// <summary>
        /// 当前连接方式。
        /// </summary>
        string ConnectionType { get; }

        /// <summary>
        /// 配置客户端。
        /// </summary>
        Result Configure(PlcProtocolClientOptions options);

        /// <summary>
        /// 建立连接。
        /// </summary>
        Result Connect();

        /// <summary>
        /// 断开连接。
        /// </summary>
        Result Disconnect();

        /// <summary>
        /// 重连。
        /// </summary>
        Result Reconnect();

        /// <summary>
        /// 查询当前是否已连接。
        /// </summary>
        Result<bool> IsConnected();

        /// <summary>
        /// 点位读取。
        /// </summary>
        Result<PlcPointReadResult> ReadPoint(PlcPointReadRequest request);

        /// <summary>
        /// 点位写入。
        /// </summary>
        Result<PlcPointReadResult> WritePoint(PlcPointWriteRequest request);

        /// <summary>
        /// 块读取。
        /// </summary>
        Result<PlcRawDataBlock> ReadBlock(PlcBlockReadRequest request);

        /// <summary>
        /// 块写入。
        /// </summary>
        Result<PlcRawDataBlock> WriteBlock(PlcBlockWriteRequest request);
    }
}