namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 连续地址块写入请求。
    /// </summary>
    public class M_BlockWriteRequest
    {
        public string areaType { get; set; } = string.Empty;
        public string startAddress { get; set; } = string.Empty;
        public string dataType { get; set; } = string.Empty;
        public byte[] buffer { get; set; } = new byte[0];
        public int stringLength { get; set; }
        public int arrayLength { get; set; }
    }
}