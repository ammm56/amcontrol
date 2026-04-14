using SqlSugar;
using System;

namespace AM.Model.Entity.Motion.Topology
{
    /// <summary>
    /// IO 接线信息表。
    /// 用于补充软件 IO 定义到现场电气接线之间的关系描述。
    /// 不替代 motion_io_map，仅承载端子、线号、对端设备等装配信息。
    /// </summary>
    [SugarTable("motion_io_wiring")]
    public class MotionIoWiringEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 关联的 IO 映射主键。
        /// 对应 motion_io_map.Id。
        /// </summary>
        public int IoMapId { get; set; }

        /// <summary>
        /// 所属控制卡 CardId。
        /// 冗余字段，便于按控制卡快速查询。
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// IO 类型：DI / DO。
        /// </summary>
        public string IoType { get; set; }

        /// <summary>
        /// 逻辑位号。
        /// 冗余字段，便于按逻辑位快速查询。
        /// </summary>
        public short LogicalBit { get; set; }

        /// <summary>
        /// 端子排编号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string TerminalBlock { get; set; }

        /// <summary>
        /// 端子号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string TerminalNo { get; set; }

        /// <summary>
        /// 插头编号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ConnectorNo { get; set; }

        /// <summary>
        /// 针脚编号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string PinNo { get; set; }

        /// <summary>
        /// 线号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string WireNo { get; set; }

        /// <summary>
        /// 对端设备名称。
        /// 例如：气缸电磁阀、真空阀、按钮、光电传感器。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DeviceName { get; set; }

        /// <summary>
        /// 对端设备型号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DeviceModel { get; set; }

        /// <summary>
        /// 对端端子。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DeviceTerminal { get; set; }

        /// <summary>
        /// 机柜区域或安装区域。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CabinetArea { get; set; }

        /// <summary>
        /// 信号类型。
        /// 例如：24V / NPN / PNP / Relay。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string SignalType { get; set; }

        /// <summary>
        /// 常态说明。
        /// 例如：常开 / 常闭 / 默认断开 / 默认导通。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ExpectedNormalState { get; set; }

        /// <summary>
        /// 点检方法说明。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CheckMethod { get; set; }

        /// <summary>
        /// 是否已核对接线。
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// 核对人。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string VerifiedBy { get; set; }

        /// <summary>
        /// 核对时间。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? VerifiedTime { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }
    }
}
