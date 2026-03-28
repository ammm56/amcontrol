using AM.Model.Common;
using AM.Model.Plc;

namespace AM.Model.Interfaces.Plc
{
    /// <summary>
    /// PLC 通用客户端接口。
    /// 屏蔽不同厂商、协议与连接方式的底层差异。
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
        /// 建立连接。
        /// </summary>
        Result Connect();

        /// <summary>
        /// 断开连接。
        /// </summary>
        Result Disconnect();

        /// <summary>
        /// 查询当前是否已连接。
        /// </summary>
        Result<bool> IsConnected();

        /// <summary>
        /// 按地址块读取原始数据。
        /// </summary>
        Result<PlcRawDataBlock> ReadBlock(
            string areaType,
            string startAddress,
            int length,
            string dataType);

        /// <summary>
        /// 单值写入。
        /// </summary>
        Result Write(
            string areaType,
            string address,
            string dataType,
            object value,
            short? bitIndex = null,
            int stringLength = 0,
            int arrayLength = 0);

        /// <summary>
        /// 块写入。
        /// </summary>
        Result WriteBlock(
            string areaType,
            string startAddress,
            byte[] buffer);
    }
}