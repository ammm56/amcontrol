namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧 PLC 客户端配置参数。
    /// 用于将运行时站配置转换成客户端可消费的结构化参数。
    /// </summary>
    public class PlcProtocolClientOptions
    {
        public string PlcName { get; set; }

        public string ProtocolType { get; set; }

        public string ConnectionType { get; set; }

        public string IpAddress { get; set; }

        public int Port { get; set; }

        public short? StationNo { get; set; }

        public short? Rack { get; set; }

        public short? Slot { get; set; }

        public int TimeoutMs { get; set; }

        public string ByteOrder { get; set; }

        public string WordOrder { get; set; }

        public string StringEncoding { get; set; }
    }
}