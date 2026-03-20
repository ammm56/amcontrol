using SqlSugar;

namespace AM.Model.Entity.Motion.Topology
{
    /// <summary>
    /// 运动控制卡主表。
    /// 保存控制卡基础信息与拓扑定义。
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
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// 控制卡类型。
        /// 存枚举数值。
        /// </summary>
        public int CardType { get; set; }

        /// <summary>
        /// 控制卡名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 打开模式参数。
        /// </summary>
        public short ModeParam { get; set; }

        /// <summary>
        /// 控制卡内核数量。
        /// </summary>
        public int CoreNumber { get; set; }

        /// <summary>
        /// 控制卡轴总数。
        /// </summary>
        public short AxisCountNumber { get; set; }

        /// <summary>
        /// 是否启用扩展模块。
        /// </summary>
        public bool UseExtModule { get; set; }

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序号。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }
    }
}