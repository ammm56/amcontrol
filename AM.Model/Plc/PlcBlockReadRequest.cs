namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧块读取请求。
    /// </summary>
    public class PlcBlockReadRequest
    {
        public string PlcName { get; set; }

        public string AreaType { get; set; }

        public string StartAddress { get; set; }

        public int Length { get; set; }

        public string DataType { get; set; }

        public int StringLength { get; set; }

        public int ArrayLength { get; set; }
    }
}