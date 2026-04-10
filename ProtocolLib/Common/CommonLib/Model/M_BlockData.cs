namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 连续地址块读写结果。
    /// </summary>
    public class M_BlockData
    {
        public string areaType { get; set; } = string.Empty;
        public string startAddress { get; set; } = string.Empty;
        public int length { get; set; }
        public string dataType { get; set; } = string.Empty;
        public byte[] buffer { get; set; } = new byte[0];
        public string valueText { get; set; } = string.Empty;
    }
}