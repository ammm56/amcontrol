using System;

namespace AM.PageModel.Motion.DI
{
    /// <summary>
    /// DI 监视页显示项。
    ///
    /// 【当前职责】
    /// 1. 承载 DI 点位的原始快照字段；
    /// 2. 提供列表卡片和右侧详情区共用的派生显示属性；
    /// 3. 只表达界面应该如何显示，不承担查询与执行逻辑。
    ///
    /// 【层级关系】
    /// - 上游：DIMotionPageModel；
    /// - 下游：DIMotionPage、DIMotionVirtualListControl、DIMotionDetailControl。
    /// </summary>
    public sealed class DIMotionIoViewItem
    {
        #region 原始数据属性

        /// <summary>
        /// IO 类型。
        /// 当前页面固定为 `DI`。
        /// </summary>
        public string IoType { get; set; }

        /// <summary>
        /// 逻辑位编号。
        /// </summary>
        public short LogicalBit { get; set; }

        /// <summary>
        /// 内部名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 信号分类显示名。
        /// </summary>
        public string SignalCategoryDisplayName { get; set; }

        /// <summary>
        /// 控制卡编号。
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// 控制卡显示名。
        /// </summary>
        public string CardDisplayName { get; set; }

        /// <summary>
        /// 物理核心编号。
        /// </summary>
        public short Core { get; set; }

        /// <summary>
        /// 硬件位编号。
        /// </summary>
        public short HardwareBit { get; set; }

        /// <summary>
        /// 是否为扩展模块点位。
        /// </summary>
        public bool IsExtModule { get; set; }

        /// <summary>
        /// 当前值。
        /// </summary>
        public bool CurrentValue { get; set; }

        /// <summary>
        /// 最后更新时间。
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 描述信息。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注信息。
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联对象名称。
        /// </summary>
        public string LinkObjectName { get; set; }

        #endregion

        #region 派生显示属性

        /// <summary>
        /// 卡片主标题。
        /// 优先显示显示名，其次显示内部名。
        /// </summary>
        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                    return DisplayName;

                return string.IsNullOrWhiteSpace(Name) ? "未命名输入点" : Name;
            }
        }

        /// <summary>
        /// 当前值显示文本。
        /// </summary>
        public string ValueText
        {
            get { return CurrentValue ? "ON" : "OFF"; }
        }

        /// <summary>
        /// 类型显示文本。
        /// </summary>
        public string TypeText
        {
            get
            {
                return string.IsNullOrWhiteSpace(SignalCategoryDisplayName)
                    ? "数字输入"
                    : SignalCategoryDisplayName;
            }
        }

        /// <summary>
        /// 模块来源显示文本。
        /// </summary>
        public string ModuleText
        {
            get { return IsExtModule ? "扩展" : "板载"; }
        }

        /// <summary>
        /// 控制卡显示文本。
        /// </summary>
        public string CardText
        {
            get
            {
                var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                return "卡#" + CardId + "  " + name;
            }
        }

        /// <summary>
        /// Core 显示文本。
        /// </summary>
        public string CoreText
        {
            get { return "Core " + Core; }
        }

        /// <summary>
        /// 硬件位显示文本。
        /// </summary>
        public string HardwareBitText
        {
            get { return HardwareBit.ToString(); }
        }

        /// <summary>
        /// 硬件地址显示文本。
        /// </summary>
        public string HardwareAddressText
        {
            get { return CoreText + " / Bit " + HardwareBit + " / " + ModuleText; }
        }

        /// <summary>
        /// 最后更新时间显示文本。
        /// </summary>
        public string LastUpdateTimeText
        {
            get
            {
                return LastUpdateTime.HasValue
                    ? LastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : "—";
            }
        }

        /// <summary>
        /// 描述显示文本。
        /// 空值时统一回退到占位符。
        /// </summary>
        public string DescriptionText
        {
            get { return string.IsNullOrWhiteSpace(Description) ? "—" : Description; }
        }

        /// <summary>
        /// 备注显示文本。
        /// 空值时统一回退到占位符。
        /// </summary>
        public string RemarkText
        {
            get { return string.IsNullOrWhiteSpace(Remark) ? "—" : Remark; }
        }

        /// <summary>
        /// 关联对象显示文本。
        /// 空值时统一回退到占位符。
        /// </summary>
        public string LinkObjectDisplayText
        {
            get { return string.IsNullOrWhiteSpace(LinkObjectName) ? "—" : LinkObjectName; }
        }

        #endregion
    }
}
