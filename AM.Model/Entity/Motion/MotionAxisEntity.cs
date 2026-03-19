using SqlSugar;

namespace AM.Model.Entity.Motion
{
    /// <summary>
    /// 运动轴拓扑表。
    /// 仅保存轴归属、逻辑轴与物理轴映射。
    /// </summary>
    [SugarTable("motion_axis")]
    public class MotionAxisEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 所属控制卡 CardId。
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// 轴编号。
        /// </summary>
        public short AxisId { get; set; }

        /// <summary>
        /// 逻辑轴号。
        /// </summary>
        public short LogicalAxis { get; set; }

        /// <summary>
        /// 轴名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 物理内核号。
        /// </summary>
        public short PhysicalCore { get; set; }

        /// <summary>
        /// 物理轴号。
        /// </summary>
        public short PhysicalAxis { get; set; }

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
        public string Remark { get; set; }
    }
}