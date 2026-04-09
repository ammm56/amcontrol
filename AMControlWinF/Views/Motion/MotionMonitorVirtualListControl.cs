using AM.PageModel.Motion;
using AM.PageModel.Motion.Monitor;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 多轴总览虚拟卡片列表。
    ///
    /// 设计目标：
    /// 1. 左侧区域可能有很多轴卡片，不能为每个轴都创建一个真实 WinForms 控件；
    /// 2. 使用 AntdUI 的 `VirtualPanel` 进行“虚拟绘制”，只在可见区域绘制卡片；
    /// 3. 减少控件数量、降低内存占用，并保证滚动时更流畅；
    /// 4. 刷新时优先原地更新，而不是整批清空重建，尽量保留滚动位置。
    ///
    /// 重要说明：
    /// - 这里的卡片不是普通 `UserControl`；
    /// - 而是 `VirtualPanel` 内部的 `VirtualItem`；
    /// - 这些卡片只在运行时通过 `Paint()` 绘制出来；
    /// - 因此在 WinForms 设计器中，看不到真实卡片内容，这是正常现象。
    ///
    /// 也就是说：
    /// Designer 中能看到的只是 `VirtualPanel` 容器本身，
    /// 真正的卡片布局、尺寸、间距、文字和颜色，全部由下面的代码决定。
    /// </summary>
    public partial class MotionMonitorVirtualListControl : UserControl
    {
        public MotionMonitorVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 卡片选中事件。
        ///
        /// 左侧轴卡片本身并不是标准 WinForms 控件，没有传统 Click 事件，
        /// 所以这里把 `VirtualPanel.ItemClick` 再包装为页面层更容易使用的事件。
        ///
        /// 外层页面收到这个事件后：
        /// - 更新当前选中轴；
        /// - 刷新右侧详情区。
        /// </summary>
        public event EventHandler<MotionAxisItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前页轴卡片数据。
        ///
        /// 这是左侧虚拟列表最核心的刷新入口。
        ///
        /// 这里的关键目标不是“能刷新”，而是“刷新时尽量稳定”：
        /// ---------------------------------------------------------
        /// 如果每次刷新都执行：
        ///     Items.Clear() + 重新 AddRange(...)
        /// 那么 `VirtualPanel` 会认为整个列表被重建：
        /// - 滚动条位置可能丢失；
        /// - 布局重新计算；
        /// - 用户正在浏览中部时，刷新后可能跳回顶部；
        /// - 页面打开时更容易出现明显闪动。
        ///
        /// 因此这里采用“两级刷新策略”：
        /// 1. 如果当前页项目结构没有变化（数量和逻辑轴顺序都相同），
        ///    则只做原地更新：更新每张卡片的数据，不重建容器；
        /// 2. 只有在分页切换、筛选变化、搜索变化、排序变化时，
        ///    才执行整批重建。
        ///
        /// 这样做的结果：
        /// - 普通 500ms 定时刷新时，不会轻易回顶；
        /// - 只更新卡片上的状态、颜色、位置值等轻量内容；
        /// - 用户滚动体验更稳定。
        /// </summary>
        /// <param name="items">当前页要显示的轴集合</param>
        /// <param name="selectedItem">当前选中的轴，用于绘制选中态</param>
        public void BindItems(
            IList<MotionAxisViewItem> items,
            MotionAxisViewItem selectedItem)
        {
            // 防御性处理：
            // 页面层即使传 null，这里也统一转为空集合，避免后面反复判空。
            var sourceItems = items ?? new List<MotionAxisViewItem>();

            // 当前选中的逻辑轴号。
            // 这里只比较逻辑轴，不直接比较对象引用，
            // 因为页面每次刷新后，ViewItem 可能是新的实例。
            var selectedLogicalAxis = selectedItem == null
                ? (short?)null
                : selectedItem.LogicalAxis;

            // 空数据时显示占位文案；
            // 有数据时置空，让 VirtualPanel 正常显示列表。
            virtualPanelInputs.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有轴"
                : null;

            // 如果当前 VirtualPanel 内部卡片结构和新数据一致，
            // 则只更新内容，不重建。
            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedLogicalAxis);
                return;
            }

            // 如果结构已经变化，则执行整批重建。
            RebuildItems(sourceItems, selectedLogicalAxis);
        }

        /// <summary>
        /// 绑定内部事件。
        ///
        /// 这里仅绑定一次 `VirtualPanel.ItemClick`。
        /// 因为虚拟卡片不是标准 WinForms 控件，点击逻辑统一由 VirtualPanel 分发。
        /// </summary>
        private void BindEvents()
        {
            virtualPanelInputs.ItemClick += VirtualPanelInputs_ItemClick;
        }

        /// <summary>
        /// VirtualPanel 卡片点击处理。
        ///
        /// 运行机制：
        /// 1. 用户点击的是 `VirtualPanel` 内部绘制出来的某个 `VirtualItem`；
        /// 2. `VirtualPanel` 命中该项后触发 `ItemClick`；
        /// 3. 这里把点击项转换为 `MotionAxisVirtualCardItem`；
        /// 4. 再把对应逻辑轴号抛给外层页面。
        /// </summary>
        private void VirtualPanelInputs_ItemClick(object sender, VirtualItemEventArgs e)
        {
            var cardItem = e == null ? null : e.Item as MotionAxisVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
                return;

            var handler = ItemSelected;
            if (handler != null)
                handler(this, new MotionAxisItemSelectedEventArgs(cardItem.Item.LogicalAxis));
        }

        /// <summary>
        /// 判断当前列表是否可以“原地更新”。
        ///
        /// 这里判断的不是内容是否变化，而是“结构是否变化”。
        ///
        /// 所谓结构一致，是指：
        /// 1. 当前 VirtualPanel 里的卡片数量 == 新数据数量；
        /// 2. 每个位置上的逻辑轴顺序都相同。
        ///
        /// 只要这两个条件成立，就说明：
        /// - 当前仍然是同一页；
        /// - 筛选结果结构没有变；
        /// - 只是卡片上的状态、数值、选中态变了；
        /// 这时就可以安全地原地更新，避免滚动位置丢失。
        ///
        /// 为什么只比较 `LogicalAxis`：
        /// - 它在当前页中可以作为稳定标识；
        /// - 刷新后即使对象实例变了，逻辑轴仍然不变；
        /// - 用它做结构比对最稳定，成本也最低。
        /// </summary>
        private bool CanUpdateInPlace(IList<MotionAxisViewItem> items)
        {
            if (items == null)
                return virtualPanelInputs.Items.Count == 0;

            // 数量变了，说明结构一定变了，需要重建。
            if (virtualPanelInputs.Items.Count != items.Count)
                return false;

            // 逐项比较逻辑轴顺序。
            // 一旦顺序变化，说明分页/搜索/筛选结果已变化，必须重建。
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as MotionAxisVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                    return false;

                if (virtualItem.Item.LogicalAxis != items[i].LogicalAxis)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 原地更新当前页卡片数据。
        ///
        /// 这里不会清空 `VirtualPanel.Items`，
        /// 只会把每个 `VirtualItem` 绑定到新的 ViewItem。
        ///
        /// 这样做的好处：
        /// - 保留滚动位置；
        /// - 保留当前布局结果；
        /// - 只触发一次重绘；
        /// - 更适合 500ms 周期的低频状态刷新。
        ///
        /// 对运行界面的表现：
        /// - 卡片位置不变；
        /// - 只更新 Enable/Disable、Alarm/Moving/Ready、位置值等内容。
        /// </summary>
        private void UpdateItemsInPlace(IList<MotionAxisViewItem> items, short? selectedLogicalAxis)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as MotionAxisVirtualCardItem;
                if (virtualItem == null)
                    continue;

                virtualItem.Bind(
                    items[i],
                    selectedLogicalAxis.HasValue && selectedLogicalAxis.Value == items[i].LogicalAxis);
            }

            // 通知 VirtualPanel 重新绘制。
            // 这里只重绘，不重建布局，因此滚动条位置可保留。
            virtualPanelInputs.Invalidate();
        }

        /// <summary>
        /// 重建当前页卡片。
        ///
        /// 触发场景：
        /// - 首次加载；
        /// - 页码改变；
        /// - 搜索条件变化；
        /// - 控制卡筛选变化；
        /// - 结果顺序变化。
        ///
        /// 这里必须重建，因为原来的虚拟项结构已经不再对应当前页数据。
        ///
        /// `PauseLayout = true` 的目的：
        /// - 暂停 VirtualPanel 的自动布局；
        /// - 避免 Clear/AddRange 过程中重复计算布局；
        /// - 最后一次性恢复布局，提高效率。
        /// </summary>
        private void RebuildItems(IList<MotionAxisViewItem> items, short? selectedLogicalAxis)
        {
            virtualPanelInputs.PauseLayout = true;
            try
            {
                // 先清除旧页数据。
                virtualPanelInputs.Items.Clear();

                if (items != null && items.Count > 0)
                {
                    var virtualItems = new List<VirtualItem>(items.Count);

                    foreach (var item in items)
                    {
                        virtualItems.Add(new MotionAxisVirtualCardItem(
                            item,
                            selectedLogicalAxis.HasValue && selectedLogicalAxis.Value == item.LogicalAxis));
                    }

                    // 一次性加入，减少多次刷新带来的性能损耗。
                    virtualPanelInputs.Items.AddRange(virtualItems);
                }
            }
            finally
            {
                // 恢复布局后，VirtualPanel 会重新计算卡片位置。
                virtualPanelInputs.PauseLayout = false;
            }
        }

        /// <summary>
        /// 列表项选中事件参数。
        ///
        /// 这里只传逻辑轴号，避免页面层直接依赖内部虚拟项实现。
        /// 页面层只需要知道“用户点击了哪一条轴”即可。
        /// </summary>
        public sealed class MotionAxisItemSelectedEventArgs : EventArgs
        {
            public MotionAxisItemSelectedEventArgs(short logicalAxis)
            {
                LogicalAxis = logicalAxis;
            }

            /// <summary>
            /// 被点击卡片对应的逻辑轴号。
            /// 外层页面通过这个值更新 SelectedItem。
            /// </summary>
            public short LogicalAxis { get; private set; }
        }

        /// <summary>
        /// VirtualPanel 中的自绘轴卡片。
        ///
        /// 这是“真正的轴卡片实现”。
        ///
        /// 重要说明：
        /// - 这里不是 WinForms 的 `Control`；
        /// - 而是 AntdUI 的 `VirtualShadowItem`；
        /// - 它只负责尺寸、绘制、鼠标命中等虚拟行为；
        /// - 具体显示什么，完全由 `Size()` 和 `Paint()` 决定。
        ///
        /// 可以把它理解为：
        /// “左侧每一张轴卡片的运行时绘制模板”。
        /// </summary>
        private sealed class MotionAxisVirtualCardItem : VirtualShadowItem
        {
            /// <summary>
            /// 卡片主标题字体：显示轴名称。
            /// </summary>
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);

            /// <summary>
            /// 卡片正文/说明字体：显示类型、位置、物理映射等。
            /// </summary>
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);

            /// <summary>
            /// 左上角小标签字体，例如 L#101、Enable、Alarm。
            /// </summary>
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            /// <summary>
            /// 当前卡片是否处于选中态。
            /// 选中态主要通过背景色和边框色体现。
            /// </summary>
            private bool _selected;

            public MotionAxisVirtualCardItem(MotionAxisViewItem item, bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public MotionAxisViewItem Item { get; private set; }

            /// <summary>
            /// 原地更新卡片数据。
            /// 这里不会重建对象，只更新卡片内容与选中态。
            /// </summary>
            public void Bind(MotionAxisViewItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            /// <summary>
            /// 返回卡片尺寸。
            ///
            /// 这里直接决定：
            /// - 每行能放几张卡；
            /// - 滚动时每项占用的可视高度；
            /// - 卡片内部可用绘制空间。
            ///
            /// 本次细调：
            /// - 宽度从 170 提升到 178，避免标题和位置文本过于局促；
            /// - 高度从 118 提升到 120，让底部两行位置文本更舒展。
            /// </summary>
            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(170 * dpi),
                    (int)Math.Round(130 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            /// <summary>
            /// 绘制单张轴卡片。
            ///
            /// 当前卡片内容：
            /// 1. 左上角逻辑轴标签；
            /// 2. Enable/Disable 状态标签；
            /// 3. 右上角当前运行状态；
            /// 4. 中间显示轴名称；
            /// 5. 下方显示轴类型、物理映射、指令位置、编码器位置。
            ///
            /// 细调点：
            /// - 右上角状态改为小标签，更稳定、更易读；
            /// - 增加物理映射信息，便于总览时识别轴来源；
            /// - 位置文本更紧凑，减少卡片内截断；
            /// - 颜色区分与 DI/DO 页面保持统一风格。
            /// </summary>
            public override void Paint(Canvas g, VirtualPanelArgs e)
            {
                if (Item == null)
                    return;

                var isDark = AntdUI.Config.IsDark;
                var rect = new Rectangle(0, 0, e.Rect.Width, e.Rect.Height);

                var backColor = _selected
                    ? (isDark ? Color.FromArgb(29, 58, 97) : Color.FromArgb(237, 246, 255))
                    : (isDark ? Color.FromArgb(39, 44, 52) : Color.FromArgb(255, 255, 255));

                var borderColor = _selected
                    ? Color.FromArgb(22, 119, 255)
                    : (isDark ? Color.FromArgb(72, 79, 92) : Color.FromArgb(225, 229, 235));

                var textColor = isDark
                    ? Color.FromArgb(235, 235, 235)
                    : Color.FromArgb(38, 38, 38);

                var subTextColor = isDark
                    ? Color.FromArgb(170, 176, 186)
                    : Color.FromArgb(120, 120, 120);

                var logicalColor = Color.FromArgb(22, 119, 255);
                var enableColor = Item.IsEnabled
                    ? Color.FromArgb(82, 196, 26)
                    : Color.FromArgb(160, 160, 160);

                var stateColor = Item.IsAlarm
                    ? Color.FromArgb(245, 108, 108)
                    : (Item.IsMoving
                        ? Color.FromArgb(22, 119, 255)
                        : (Item.IsReady
                            ? Color.FromArgb(82, 196, 26)
                            : Color.FromArgb(160, 160, 160)));

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, _selected ? 1.6F : 1F, path);
                }

                // 左上角：逻辑轴标签
                DrawBadge(g, new Rectangle(12, 12, 46, 22), logicalColor, "L#" + Item.LogicalAxis);

                // 左上角：使能状态标签
                DrawBadge(g, new Rectangle(62, 12, 60, 22), enableColor, Item.EnableText);

                // 右上角：运行状态标签
                //DrawBadge(g, new Rectangle(rect.Width - 70, 12, 58, 22), stateColor, Item.StateText);

                // 标题：轴名称
                g.String(
                    TrimText(Item.DisplayTitle, 18),
                    FontTitle,
                    textColor,
                    new Point(12, 44));

                // 第二行：轴类型
                g.String(
                    TrimText("类型：" + Item.AxisCategoryText, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 60));

                // 第二行右：物理映射
                g.String(
                    TrimText(Item.PhysicalText, 26),
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 40, 60));

                // 第三行：指令位置
                g.String(
                    TrimText("指令：" + Item.CommandPositionMm.ToString("0.###") + " mm", 27),
                    FontBody,
                    subTextColor,
                    new Point(12, 78));

                // 第四行：编码器位置
                g.String(
                    TrimText("编码器：" + Item.EncoderPositionMm.ToString("0.###") + " mm", 27),
                    FontBody,
                    subTextColor,
                    new Point(12, 95));
            }

            /// <summary>
            /// 绘制小标签。
            /// 该方法供逻辑轴、使能状态、运行状态复用。
            /// </summary>
            private static void DrawBadge(Canvas g, Rectangle rect, Color backColor, string text)
            {
                using (var path = rect.RoundPath(6))
                {
                    g.Fill(backColor, path);
                }

                g.String(text, FontBadge, Color.White, rect);
            }

            /// <summary>
            /// 文本裁剪辅助。
            /// 防止名称或类型过长导致卡片排版被挤乱。
            /// </summary>
            private static string TrimText(string text, int maxLength)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return "-";

                if (text.Length <= maxLength)
                    return text;

                return text.Substring(0, maxLength - 1) + "…";
            }
        }
    }
}