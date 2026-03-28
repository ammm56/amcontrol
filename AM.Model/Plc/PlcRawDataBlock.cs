using System;

namespace AM.Model.Plc
{
    /// <summary>
    /// PLC 原始读块结果。
    /// 用于承载底层协议返回的原始字节与块元数据。
    /// </summary>
    public class PlcRawDataBlock
    {
        /// <summary>
        /// 所属 PLC 名称。
        /// </summary>
        public string PlcName { get; set; }

        /// <summary>
        /// 区域类型。
        /// </summary>
        public string AreaType { get; set; }

        /// <summary>
        /// 起始地址。
        /// </summary>
        public string StartAddress { get; set; }

        /// <summary>
        /// 读取长度。
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 数据类型说明。
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 原始字节缓冲区。
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 读取时间。
        /// </summary>
        public DateTime ReadTime { get; set; } = DateTime.Now;
    }
}