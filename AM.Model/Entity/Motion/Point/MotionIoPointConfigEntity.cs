using SqlSugar;

namespace AM.Model.Entity.Motion.Point
{
    /// <summary>
    /// IO 点位公共配置表。
    /// 负责描述逻辑点位本身的解释方式与通用控制参数，
    /// 不承载控制卡拓扑映射，也不承载执行器级联动配置。
    /// 用 IoType + LogicalBit 做业务关联键
    /// </summary>
    [SugarTable("motion_io_point_config")]
    public class MotionIoPointConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// IO 类型：DI / DO。
        /// 用于与 motion_io_map 形成稳定的一对一关系。
        /// </summary>
        public string IoType { get; set; }

        /// <summary>
        /// 逻辑位号。
        /// </summary>
        public short LogicalBit { get; set; }

        /// <summary>
        /// 显示名称。
        /// 用于 UI 与业务层展示。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 信号分类。
        /// 例如：Sensor / Button / Safety / Cylinder / AlarmLamp / Buzzer / Valve / Other。
        /// 例如：传感器/按钮/安全装置/气缸/报警灯/蜂鸣器/阀门/其他。
        /// </summary>
        public string SignalCategory { get; set; }

        /// <summary>
        /// 逻辑是否取反。
        /// true=读取或输出时做逻辑翻转。
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// 是否常闭。
        /// true=NC（常闭），false=NO（常开）。
        /// </summary>
        public bool IsNormallyClosed { get; set; }

        /// <summary>
        /// 去抖时间，单位 ms。
        /// 主要用于 DI。
        /// </summary>
        public int DebounceMs { get; set; }

        /// <summary>
        /// 滤波时间，单位 ms。
        /// 主要用于 DI。
        /// </summary>
        public int FilterMs { get; set; }

        /// <summary>
        /// 是否允许手动操作。
        /// 主要用于 DO，也可用于 DI 调试模拟。
        /// </summary>
        public bool CanManualOperate { get; set; }

        /// <summary>
        /// 默认输出状态。
        /// 主要用于 DO。false=默认断开，true=默认导通。
        /// </summary>
        public bool DefaultOutputState { get; set; }

        /// <summary>
        /// 输出模式。
        /// Keep / Pulse / Blink。
        /// 保持/脉冲/闪烁。
        /// DI 点可固定为 Keep。
        /// </summary>
        public string OutputMode { get; set; }

        /// <summary>
        /// 脉冲宽度，单位 ms。
        /// OutputMode=Pulse 时使用。
        /// </summary>
        public int PulseWidthMs { get; set; }

        /// <summary>
        /// 闪烁亮时长，单位 ms。
        /// OutputMode=Blink 时使用。
        /// </summary>
        public int BlinkOnMs { get; set; }

        /// <summary>
        /// 闪烁灭时长，单位 ms。
        /// OutputMode=Blink 时使用。
        /// </summary>
        public int BlinkOffMs { get; set; }

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
    }
}