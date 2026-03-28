using SqlSugar;
using System;

namespace AM.Model.Entity.Plc
{
    /// <summary>
    /// PLC 站配置表。
    /// 用于描述一个 PLC 连接对象的厂商、型号、连接方式、协议与通讯参数。
    /// </summary>
    [SugarTable("plc_station")]
    public class PlcStationConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 内部唯一名称。
        /// 作为运行时索引键与业务关联键。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// PLC 厂商。
        /// 例如：Siemens / Mitsubishi / Omron / ModbusGeneric。
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// PLC 型号。
        /// 例如：S7-1200 / FX5U / Q03UDE。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Model { get; set; }

        /// <summary>
        /// 连接方式。
        /// 例如：Tcp / Serial / Usb / Virtual。
        /// </summary>
        public string ConnectionType { get; set; }

        /// <summary>
        /// 通讯协议。
        /// 例如：ModbusTcp / ModbusRtu / S7 / Mc3E。
        /// </summary>
        public string ProtocolType { get; set; }

        /// <summary>
        /// TCP IP 地址。
        /// ConnectionType=Tcp 时使用。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string IpAddress { get; set; }

        /// <summary>
        /// TCP 端口。
        /// ConnectionType=Tcp 时使用。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Port { get; set; }

        /// <summary>
        /// 串口号。
        /// ConnectionType=Serial 时使用。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ComPort { get; set; }

        /// <summary>
        /// 串口波特率。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? BaudRate { get; set; }

        /// <summary>
        /// 串口数据位。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? DataBits { get; set; }

        /// <summary>
        /// 串口校验位。
        /// 例如：None / Odd / Even。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Parity { get; set; }

        /// <summary>
        /// 串口停止位。
        /// 例如：1 / 1.5 / 2。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string StopBits { get; set; }

        /// <summary>
        /// 站号。
        /// 常用于 Modbus RTU/TCP 等协议。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? StationNo { get; set; }

        /// <summary>
        /// 网络号。
        /// 常用于 MC/FINS 等协议。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? NetworkNo { get; set; }

        /// <summary>
        /// PLC PC 号。
        /// 常用于 MC/FINS 等协议。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? PcNo { get; set; }

        /// <summary>
        /// S7 机架号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? Rack { get; set; }

        /// <summary>
        /// S7 插槽号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? Slot { get; set; }

        /// <summary>
        /// 通讯超时，单位 ms。
        /// </summary>
        public int TimeoutMs { get; set; }

        /// <summary>
        /// 重连周期，单位 ms。
        /// </summary>
        public int ReconnectIntervalMs { get; set; }

        /// <summary>
        /// 扫描周期，单位 ms。
        /// </summary>
        public int ScanIntervalMs { get; set; }

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序号。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 描述说明。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}