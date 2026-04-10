namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧点位读写结果。
    /// 当前版本直接使用 Address 表示完整协议地址。
    /// </summary>
    public class PlcPointReadResult
    {
        public string PlcName { get; set; }

        public string Address { get; set; }

        public string DataType { get; set; }

        public string ValueText { get; set; }

        public byte[] RawBuffer { get; set; }
    }
}