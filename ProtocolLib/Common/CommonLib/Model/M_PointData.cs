namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位读写结果。
    /// 当前版本统一承载单值、字符串与数组读写结果。
    /// </summary>
    public class M_PointData
    {
        /// <summary>
        /// 完整协议地址。
        /// </summary>
        public string address { get; set; } = string.Empty;

        /// <summary>
        /// 数据类型。
        /// 使用统一字符串类型名。
        /// </summary>
        public string dataType { get; set; } = string.Empty;

        /// <summary>
        /// 长度。
        /// 与请求中的 length 语义保持一致。
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// 结果值文本。
        /// 当前版本统一以文本形式承载。
        /// 数组可由协议层定义统一序列化格式。
        /// </summary>
        public string value { get; set; } = string.Empty;

        /// <summary>
        /// 原始缓冲区。
        /// 用于需要原始字节结果的场景。
        /// </summary>
        public byte[] rawBuffer { get; set; } = new byte[0];

        /// <summary>
        /// 质量标记。
        /// 例如：Good / Error / Disconnected。
        /// </summary>
        public string quality { get; set; } = string.Empty;
    }
}