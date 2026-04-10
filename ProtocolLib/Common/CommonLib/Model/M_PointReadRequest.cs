namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位读取请求。
    /// 当前版本直接使用 address 表示完整协议地址。
    /// </summary>
    public class M_PointReadRequest
    {
        public string address { get; set; } = string.Empty;

        public string dataType { get; set; } = string.Empty;

        public int stringLength { get; set; }

        public int arrayLength { get; set; }
    }
}