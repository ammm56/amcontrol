namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位读取请求。
    /// 当前版本直接使用 address 表示完整协议地址。
    /// length 统一表示长度：
    /// - 标量：1
    /// - 字符串：字符长度
    /// - 数组：元素个数
    /// </summary>
    public class M_PointReadRequest
    {
        /// <summary>
        /// 完整协议地址。
        /// 例如：
        /// - Modbus: 00001 / 40001 / 40040[20]
        /// - S7: DB1.0 / DB1.20[20]
        /// </summary>
        public string address { get; set; } = string.Empty;

        /// <summary>
        /// 数据类型。
        /// 统一使用字符串表示，例如：
        /// bool、uint8、int8、uint16、int16、uint32、int32、uint64、int64、float、double、string
        /// 数组使用 type[] 表示，例如 uint16[]。
        /// </summary>
        public string dataType { get; set; } = string.Empty;

        /// <summary>
        /// 长度。
        /// - 标量：1
        /// - 字符串：字符长度
        /// - 数组：元素个数
        /// </summary>
        public int length { get; set; }
    }
}