namespace AM.Model.Plc
{
    /// <summary>
    /// PLC 点位运行时配置对象。
    /// 当前版本采用最简模型，直接使用 Address 表示完整协议地址，
    /// 并使用单一 Length 字段统一表达长度。
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

        /// <summary>
        /// 长度。
        /// - 标量：1
        /// - 字符串：字符长度
        /// - 数组：元素个数
        /// </summary>
        public int Length { get; set; }

        public string AccessMode { get; set; }

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