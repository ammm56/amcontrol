namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧块写入请求。
    /// </summary>
    public class PlcBlockWriteRequest
    {
        public string PlcName { get; set; }

        public string AreaType { get; set; }

        public string StartAddress { get; set; }

        public string DataType { get; set; }

        public byte[] Buffer { get; set; }

        public int StringLength { get; set; }

        public int ArrayLength { get; set; }
    }
}