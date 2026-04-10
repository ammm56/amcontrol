namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位写入请求。
    /// 当前版本直接使用 address 表示完整协议地址。
    /// length 统一表示长度：
    /// - 标量：1
    /// - 字符串：字符长度
    /// - 数组：元素个数
    /// </summary>
    public class M_PointWriteRequest
    {
        /// <summary>
        /// 完整协议地址。
        /// </summary>
        public string address { get; set; } = string.Empty;

        /// <summary>
        /// 数据类型。
        /// 统一使用字符串表示。
        /// </summary>
        public string dataType { get; set; } = string.Empty;

        /// <summary>
        /// 写入值。
        /// 标量、字符串、数组均通过该字段传入。
        /// </summary>
        public object value { get; set; }

        /// <summary>
        /// 长度。
        /// - 标量：1
        /// - 字符串：字符长度
        /// - 数组：元素个数
        /// </summary>
        public int length { get; set; }
    }
}