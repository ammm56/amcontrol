namespace AM.Model.MotionCard
{
    /// <summary>
    /// 逻辑IO位到固高硬件IO位的映射
    /// </summary>
    public class MotionIoBitMap
    {
        /// <summary>
        /// 业务层使用的逻辑位号
        /// </summary>
        public short LogicalBit { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属内核
        /// </summary>
        public short Core { get; set; } = 1;

        /// <summary>
        /// 是否扩展模块IO
        /// </summary>
        public bool IsExtModule { get; set; }

        /// <summary>
        /// 硬件位号
        /// </summary>
        public short HardwareBit { get; set; }
    }
}