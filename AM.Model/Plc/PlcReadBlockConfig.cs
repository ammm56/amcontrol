namespace AM.Model.Plc
{
    /// <summary>
    /// PLC 批量读取块配置。
    /// 首版先作为运行时模型预留。
    /// </summary>
    public class PlcReadBlockConfig
    {
        public int Id { get; set; }

        public string PlcName { get; set; }

        public string BlockName { get; set; }

        public string AreaType { get; set; }

        public string StartAddress { get; set; }

        public int Length { get; set; }

        public string ReadUnit { get; set; }

        public string DataType { get; set; }

        public string ReadMode { get; set; }

        public int Priority { get; set; }

        public bool IsEnabled { get; set; }

        public int SortOrder { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }
    }
}