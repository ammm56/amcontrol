using ProtocolLib.CommonLib.Model;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 协议库统一接口。
    /// 协议差异应在协议库内部完成屏蔽，AM 上层只依赖该接口。
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// 配置协议连接参数。
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
        /// 查询当前连接状态。
        /// </summary>
        M_Return<bool> IsConnected();

        /// <summary>
        /// 按点位读取。
        /// </summary>
        M_Return<M_PointData> ReadPoint(M_PointReadRequest request);

        /// <summary>
        /// 按点位写入。
        /// </summary>
        M_Return<M_PointData> WritePoint(M_PointWriteRequest request);

        /// <summary>
        /// 按连续地址块读取。
        /// </summary>
        M_Return<M_BlockData> ReadBlock(M_BlockReadRequest request);

        /// <summary>
        /// 按连续地址块写入。
        /// </summary>
        M_Return<M_BlockData> WriteBlock(M_BlockWriteRequest request);
    }
}