using AM.Model.Common;
using AM.Model.Plc;
using ProtocolLib.CommonLib.Model;

namespace AM.Model.Interfaces.Plc
{
    /// <summary>
    /// AM 侧统一 PLC 客户端门面。
    /// 负责统一上层调用入口，不负责协议行为实现。
    /// 当前版本直接复用 ProtocolLib 的点位请求/结果模型。
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
        Result Configure(M_ProtocolOptions options);

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
        /// 单值、字符串、数组统一使用该接口。
        /// </summary>
        Result<M_PointData> ReadPoint(M_PointReadRequest request);

        /// <summary>
        /// 点位写入。
        /// 单值、字符串、数组统一使用该接口。
        /// </summary>
        Result<M_PointData> WritePoint(M_PointWriteRequest request);
    }
}