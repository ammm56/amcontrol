using System.Collections.Generic;

namespace AM.Model.MotionCard
{
    /// <summary>
    /// 运动控制卡类型枚举。
    /// </summary>
    public enum MotionCardType
    {
        /// <summary>
        /// 固高。
        /// </summary>
        GOOGO = 10,

        /// <summary>
        /// 雷赛。
        /// </summary>
        LEISAI = 20,

        /// <summary>
        /// 虚拟卡。
        /// </summary>
        VIRTUAL = 90,

        /// <summary>
        /// 其它。
        /// </summary>
        Other = 99
    }

    /// <summary>
    /// 运动控制卡运行时完整配置。
    /// 由 `motion_card`、`motion_axis`、`motion_io_map`、`motion_axis_config`
    /// 等数据库表聚合装配而成。
    /// </summary>
    public class MotionCardConfig
    {
        /// <summary>
        /// 控制卡类型。
        /// </summary>
        public MotionCardType CardType { get; set; } = MotionCardType.GOOGO;

        /// <summary>
        /// 控制卡内部名称。
        /// 用于系统标识、日志、配置关联。
        /// </summary>
        public string Name { get; set; } = "MotionCard-0";

        /// <summary>
        /// 控制卡显示名称。
        /// 用于配置页、监视页、报警页展示。
        /// 为空时可回退到 Name。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 驱动识别键。
        /// 用于细分同一控制卡类型下的具体驱动实现。
        /// </summary>
        public string DriverKey { get; set; }

        /// <summary>
        /// 硬件通信通道。
        /// 通常对应物理卡索引或厂商驱动通道号。
        /// </summary>
        public short CardId { get; set; } = 0;

        /// <summary>
        /// 打开模式参数。
        /// 例如：0=仅打开，1=按配置初始化。
        /// </summary>
        public short ModeParam { get; set; } = 0;

        /// <summary>
        /// 控制卡扩展打开配置。
        /// 一般承载厂商专属的 JSON 配置参数。
        /// </summary>
        public string OpenConfig { get; set; }

        /// <summary>
        /// 控制卡内核数量。
        /// </summary>
        public ushort CoreNumber { get; set; } = 2;

        /// <summary>
        /// 控制卡轴总数。
        /// </summary>
        public short AxisCountNumber { get; set; } = 4;

        /// <summary>
        /// 是否启用扩展模块。
        /// </summary>
        public bool UseExtModule { get; set; }

        /// <summary>
        /// 初始化顺序。
        /// 多卡场景下用于统一启动顺序控制。
        /// </summary>
        public int InitOrder { get; set; }

        /// <summary>
        /// 控制卡说明。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 控制卡备注。
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 板载/扩展 DI 映射。
        /// </summary>
        public List<MotionIoBitMap> DIBitMaps { get; set; } = new List<MotionIoBitMap>();

        /// <summary>
        /// 板载/扩展 DO 映射。
        /// </summary>
        public List<MotionIoBitMap> DOBitMaps { get; set; } = new List<MotionIoBitMap>();

        /// <summary>
        /// 该卡下的所有轴配置。
        /// </summary>
        public List<AxisConfig> AxisConfigs { get; set; } = new List<AxisConfig>();
    }
}
