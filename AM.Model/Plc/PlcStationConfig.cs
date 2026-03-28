namespace AM.Model.Plc
{
    /// <summary>
    /// PLC 站运行时配置对象。
    /// 与数据库实体解耦，供 ConfigContext / MachineContext / 服务层使用。
    /// </summary>
    public class PlcStationConfig
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Vendor { get; set; }

        public string Model { get; set; }

        public string ConnectionType { get; set; }

        public string ProtocolType { get; set; }

        public string IpAddress { get; set; }

        public int? Port { get; set; }

        public string ComPort { get; set; }

        public int? BaudRate { get; set; }

        public int? DataBits { get; set; }

        public string Parity { get; set; }

        public string StopBits { get; set; }

        public short? StationNo { get; set; }

        public short? NetworkNo { get; set; }

        public short? PcNo { get; set; }

        public short? Rack { get; set; }

        public short? Slot { get; set; }

        public int TimeoutMs { get; set; }

        public int ReconnectIntervalMs { get; set; }

        public int ScanIntervalMs { get; set; }

        public bool IsEnabled { get; set; }

        public int SortOrder { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                {
                    return DisplayName;
                }

                return string.IsNullOrWhiteSpace(Name) ? "未命名 PLC" : Name;
            }
        }
    }
}