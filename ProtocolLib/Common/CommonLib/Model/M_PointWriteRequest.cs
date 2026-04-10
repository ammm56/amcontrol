namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位写入请求。
    /// </summary>
    public class M_PointWriteRequest
    {
        public string areaType { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string dataType { get; set; } = string.Empty;
        public object value { get; set; }
        public short? bitIndex { get; set; }
        public int stringLength { get; set; }
        public int arrayLength { get; set; }
    }
}