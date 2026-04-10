namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 协议连接配置参数。
    /// </summary>
    public class M_ProtocolOptions
    {
        public string protocolType { get; set; } = string.Empty;
        public string connectionType { get; set; } = string.Empty;
        public string ip { get; set; } = string.Empty;
        public int port { get; set; }
        public short? stationNo { get; set; }
        public short? rack { get; set; }
        public short? slot { get; set; }
        public int timeoutMs { get; set; }
        public string byteOrder { get; set; } = string.Empty;
        public string wordOrder { get; set; } = string.Empty;
        public string stringEncoding { get; set; } = string.Empty;
    }
}