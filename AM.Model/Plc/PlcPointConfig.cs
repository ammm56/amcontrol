namespace AM.Model.Plc
{
    /// <summary>
    /// PLC 点位运行时配置对象。
    /// 当前版本采用最简模型，直接使用 Address 表示完整协议地址。
    /// </summary>
    public class PlcPointConfig
    {
        public int Id { get; set; }

        public string PlcName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string GroupName { get; set; }

        public string Address { get; set; }

        public string DataType { get; set; }

        public int StringLength { get; set; }

        public int ArrayLength { get; set; }

        public string Unit { get; set; }

        public string AccessMode { get; set; }

        public string ReadMode { get; set; }

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
            get { return Address ?? string.Empty; }
        }
    }
}