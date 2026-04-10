using ProtocolLib.CommonLib.Model;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 协议统一接口。
    /// 当前版本采用最简设计：
    /// - 点位、数组、字符串统一走点位读写接口；
    /// - 不再单独定义块读写接口；
    /// - 请求与结果统一使用 CommonLib 模型。
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// 配置协议实例。
        /// </summary>
        M_Return<bool> Configure(M_ProtocolOptions options);

        /// <summary>
        /// 建立连接。
        /// </summary>
        M_Return<bool> Connect();

        /// <summary>
        /// 断开连接。
        /// </summary>
        M_Return<bool> Disconnect();

        /// <summary>
        /// 重连。
        /// </summary>
        M_Return<bool> Reconnect();

        /// <summary>
        /// 查询连接状态。
        /// </summary>
        M_Return<bool> IsConnected();

        /// <summary>
        /// 点位读取。
        /// 单值、字符串、数组统一使用该接口。
        /// </summary>
        M_Return<M_PointData> ReadPoint(M_PointReadRequest request);

        /// <summary>
        /// 点位写入。
        /// 单值、字符串、数组统一使用该接口。
        /// </summary>
        M_Return<M_PointData> WritePoint(M_PointWriteRequest request);
    }
}