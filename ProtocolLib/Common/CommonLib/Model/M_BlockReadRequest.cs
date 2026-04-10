namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 连续地址块读取请求。
    /// </summary>
    public class M_BlockReadRequest
    {
        public string areaType { get; set; } = string.Empty;
        public string startAddress { get; set; } = string.Empty;
        public int length { get; set; }
        public string dataType { get; set; } = string.Empty;
        public int stringLength { get; set; }
        public int arrayLength { get; set; }
    }
}