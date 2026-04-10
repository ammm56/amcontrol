namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位读写结果。
    /// </summary>
    public class M_PointData
    {
        public string areaType { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string dataType { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;
        public byte[] rawBuffer { get; set; } = new byte[0];
        public string quality { get; set; } = string.Empty;
    }
}