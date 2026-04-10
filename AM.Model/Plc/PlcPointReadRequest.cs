namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧点位读取请求。
    /// 当前版本直接使用 Address 表示完整协议地址。
    /// </summary>
    public class PlcPointReadRequest
    {
        public string PlcName { get; set; }

        public string Address { get; set; }

        public string DataType { get; set; }

        public int StringLength { get; set; }

        public int ArrayLength { get; set; }
    }
}