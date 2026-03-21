using SqlSugar;
using System;

namespace AM.Model.Entity.Motion.Topology
{
    /// <summary>
    /// 运动控制卡主表。
    /// 负责保存控制卡的基础身份、驱动识别、初始化顺序与运行时装配所需的静态配置。
    /// 唯一约束建议：
    /// 1. CardId 唯一
    /// 2. Name 唯一
    /// </summary>
    [SugarTable("motion_card")]
    public class MotionCardEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 控制卡硬件通道号。
        /// 对应实际硬件卡索引或厂商驱动通道号。
        /// 该值在系统内应唯一。
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// 控制卡类型。
        /// 保存 MotionCardType 的枚举数值。
        /// 例如：GOOGO / LEISAI / VIRTUAL。
        /// </summary>
        public int CardType { get; set; }

        /// <summary>
        /// 控制卡内部名称。
        /// 用于系统内部唯一标识、日志输出、配置关联。
        /// 建议唯一。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 控制卡显示名称。
        /// 主要用于界面展示、配置页面和状态页面显示。
        /// 允许为空；为空时可回退显示 Name。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 驱动识别键。
        /// 用于细分同一大类控制卡下的具体驱动实现或型号。
        /// 例如：Googol.GTN / Leisai.Dmc5000 / Virtual.Basic。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DriverKey { get; set; }

        /// <summary>
        /// 控制卡打开模式参数。
        /// 一般对应厂商驱动的 open/init 参数。
        /// 例如：0=仅打开，1=按配置初始化。
        /// </summary>
        public short ModeParam { get; set; }

        /// <summary>
        /// 控制卡扩展打开配置。
        /// 建议保存为 JSON 字符串，用于承载不适合拆成固定列的厂商专属初始化参数。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string OpenConfig { get; set; }

        /// <summary>
        /// 控制卡内核数量。
        /// 多核卡常为 2，入门卡可能为 1。
        /// </summary>
        public int CoreNumber { get; set; }

        /// <summary>
        /// 控制卡支持的轴总数。
        /// 用于拓扑校验、界面约束和初始化检查。
        /// </summary>
        public short AxisCountNumber { get; set; }

        /// <summary>
        /// 是否启用扩展模块。
        /// true 表示该卡存在扩展 IO / 模块配置。
        /// </summary>
        public bool UseExtModule { get; set; }

        /// <summary>
        /// 初始化顺序。
        /// 多卡场景下用于确定启动时的连接、初始化和自检先后顺序。
        /// </summary>
        public int InitOrder { get; set; }

        /// <summary>
        /// 是否启用。
        /// false 时该卡不参与运行时装配与硬件初始化。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序号。
        /// 用于配置页面展示排序。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 正式描述说明。
        /// 用于说明该卡的安装位置、用途、厂商型号或项目约束。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// 用于记录非结构化补充信息。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间。
        /// 用于配置审计与问题追踪。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间。
        /// 每次保存时应同步刷新。
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}