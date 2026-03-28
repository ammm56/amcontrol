namespace AM.Model.Plc
{
    /// <summary>
    /// PLC 点位运行时配置对象。
    /// </summary>
    public class PlcPointConfig
    {
        public int Id { get; set; }

        public string PlcName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string GroupName { get; set; }

        public string AreaType { get; set; }

        public string Address { get; set; }

        public short? BitIndex { get; set; }

        public string DataType { get; set; }

        public int StringLength { get; set; }

        public int ArrayLength { get; set; }

        public int ReadLength { get; set; }

        public double Scale { get; set; }

        public double Offset { get; set; }

        public string Unit { get; set; }

        public string AccessMode { get; set; }

        public string ReadMode { get; set; }

        public string BatchKey { get; set; }

        public string ByteOrder { get; set; }

        public string WordOrder { get; set; }

        public string StringEncoding { get; set; }

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

                return string.IsNullOrWhiteSpace(Name) ? "未命名点位" : Name;
            }
        }

        public string AddressText
        {
            get
            {
                if (BitIndex.HasValue)
                {
                    return string.Format("{0} {1}.{2}", AreaType, Address, BitIndex.Value);
                }

                return string.Format("{0} {1}", AreaType, Address);
            }
        }
    }
}