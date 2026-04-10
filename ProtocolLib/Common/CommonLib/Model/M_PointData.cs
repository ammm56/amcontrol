namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位读写结果。
    /// 当前版本直接使用 address 表示完整协议地址。
    /// </summary>
    public class M_PointData
    {
        public string address { get; set; } = string.Empty;

        public string dataType { get; set; } = string.Empty;

        public string value { get; set; } = string.Empty;

        public byte[] rawBuffer { get; set; } = new byte[0];

        public string quality { get; set; } = string.Empty;
    }
}