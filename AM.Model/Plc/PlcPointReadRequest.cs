namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧点位读取请求。
    /// </summary>
    public class PlcPointReadRequest
    {
        public string PlcName { get; set; }

        public string AreaType { get; set; }

        public string Address { get; set; }

        public string DataType { get; set; }

        public short? BitIndex { get; set; }

        public int StringLength { get; set; }

        public int ArrayLength { get; set; }
    }
}