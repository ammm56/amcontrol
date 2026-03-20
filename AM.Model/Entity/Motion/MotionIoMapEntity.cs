using SqlSugar;

namespace AM.Model.Entity.Motion
{
    /// <summary>
    /// 运动控制卡 IO 映射表。
    /// </summary>
    [SugarTable("motion_io_map")]
    public class MotionIoMapEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 所属控制卡 CardId。
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// IO 类型：DI / DO。
        /// </summary>
        public string IoType { get; set; }

        /// <summary>
        /// 逻辑位号。
        /// </summary>
        public short LogicalBit { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属内核。
        /// </summary>
        public short Core { get; set; }

        /// <summary>
        /// 是否扩展模块 IO。
        /// </summary>
        public bool IsExtModule { get; set; }

        /// <summary>
        /// 硬件位号。
        /// </summary>
        public short HardwareBit { get; set; }

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