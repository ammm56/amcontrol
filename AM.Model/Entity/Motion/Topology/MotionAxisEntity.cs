using SqlSugar;
using System;

namespace AM.Model.Entity.Motion.Topology
{
    /// <summary>
    /// 运动轴拓扑表。
    /// 只保存轴的归属关系、逻辑编号、物理映射和基础展示信息，
    /// 不承载可频繁调整的运动参数。
    /// 唯一约束建议：
    /// 1. LogicalAxis 唯一
    /// 2. Name 唯一
    /// 3. (CardId, AxisId) 唯一
    /// 4. (CardId, PhysicalCore, PhysicalAxis) 唯一
    /// </summary>
    [SugarTable("motion_axis")]
    public class MotionAxisEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 所属控制卡 CardId。
        /// 对应 motion_card.CardId。
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// 轴编号。
        /// 一般表示该卡内部的轴序号，用于配置管理和兼容部分厂商接口。
        /// 在同一控制卡内应唯一。
        /// </summary>
        public short AxisId { get; set; }

        /// <summary>
        /// 逻辑轴号。
        /// 业务层统一访问编号，系统内应唯一。
        /// </summary>
        public short LogicalAxis { get; set; }

        /// <summary>
        /// 轴内部名称。
        /// 用于系统标识、日志、流程引用。
        /// 建议全局唯一。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 轴显示名称。
        /// 用于界面展示，允许为空；为空时可回退为 Name。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 轴分类。
        /// 用于区分直线轴、旋转轴、龙门主轴、龙门从轴、虚拟轴等稳定类别。
        /// 推荐值：Linear / Rotary / GantryMaster / GantrySlave / Virtual / Other。
        /// </summary>
        public string AxisCategory { get; set; }

        /// <summary>
        /// 物理内核号。
        /// 多核控制卡使用。
        /// </summary>
        public short PhysicalCore { get; set; }

        /// <summary>
        /// 物理轴号。
        /// 控制卡上的实际轴编号。
        /// 在同一卡同一内核下应唯一。
        /// </summary>
        public short PhysicalAxis { get; set; }

        /// <summary>
        /// 是否启用。
        /// false 时该轴不参与运行时装配。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序号。
        /// 用于页面显示顺序与配置列表排序。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 轴说明。
        /// 用于描述该轴在整机中的工艺角色、安装位置和用途。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// 记录临时说明或补充信息。
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