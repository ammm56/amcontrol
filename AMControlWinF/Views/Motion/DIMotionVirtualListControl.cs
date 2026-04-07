using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// DI 虚拟卡片列表。
    ///
    /// 设计目标：
    /// 1. 左侧区域可能有很多 DI 点位，不能为每个点位都创建一个 WinForms Control；
    /// 2. 使用 AntdUI 的 `VirtualPanel` 进行“虚拟绘制”，只在可见区域绘制卡片；
    /// 3. 这样可以减少控件数量、降低内存占用，并保证滚动时更流畅。
    ///
    /// 重要说明：
    /// - 这里的卡片不是普通 `UserControl`；
    /// - 而是 `VirtualPanel` 内部的 `VirtualItem`；
    /// - 这些卡片只在运行时通过 `Paint()` 绘制出来；
    /// - 所以在 WinForms 设计器中，看不到真实卡片内容，这是正常现象。
    ///
    /// 也就是说：
    /// Designer 里能看到的只是 `VirtualPanel` 容器本身，
    /// 真正的卡片布局、尺寸、间距、内容，都是运行时由下面的代码决定的。
    /// </summary>
    public partial class DIMotionVirtualListControl : UserControl
    {
        public DIMotionVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 卡片选中事件。
        ///
        /// 左侧卡片本身并不是标准控件，没有 Click 事件，
        /// 所以这里把 `VirtualPanel.ItemClick` 再包装成页面层可直接使用的事件。
        /// 外层页面收到这个事件后，更新右侧详情区。
        /// </summary>
        public event EventHandler<DIMotionItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前页数据。
        ///
        /// 这是左侧虚拟列表最核心的刷新入口。
        ///
        /// 这里要特别注意性能和滚动位置：
        /// ---------------------------------------------------------
        /// 如果每次刷新都执行：
        ///     Items.Clear() + 重新 AddRange(...)
        /// 那么 `VirtualPanel` 会认为整个列表被重建：
        /// - 滚动条位置可能丢失；
        /// - 布局重新计算；
        /// - 用户拖动到中间时，刷新后可能回到顶部；
        /// - 打开页面时也更容易闪烁。
        ///
        /// 所以这里采用“两级刷新策略”：
        /// 1. 如果当前页项目的“结构”没有变（数量和逻辑位顺序都一样），
        ///    只做原地更新：更新每张卡片的数据，不重建容器；
        /// 2. 只有在分页切换、筛选变化、排序变化等导致卡片结构变化时，
        ///    才执行整批重建。
        ///
        /// 这样做的结果：
        /// - 普通 500ms 定时刷新时，不会跳回顶部；
        /// - 只更新颜色、状态、文字等轻量内容；
        /// - 用户滚动体验会稳定很多。
        /// </summary>
        /// <param name="items">当前页要显示的 DI 点位集合</param>
        /// <param name="selectedItem">当前选中的 DI 点位，用于绘制选中态</param>
        public void BindItems(IList<DIMotionPageModel.DIMotionIoViewItem> items, DIMotionPageModel.DIMotionIoViewItem selectedItem)
        {
            // 防御性处理：
            // 页面层即使传 null，这里也统一转成空集合，避免后面反复判空。
            var sourceItems = items ?? new List<DIMotionPageModel.DIMotionIoViewItem>();

            // 当前选中的逻辑位号。
            // 这里只保存逻辑位，不直接比较对象引用，
            // 因为页面每次刷新时，ViewItem 很可能是新的实例。
            var selectedLogicalBit = selectedItem == null
                ? (short?)null
                : selectedItem.LogicalBit;

            // 空数据时显示占位文案。
            // 有数据时置空，让 VirtualPanel 正常显示列表。
            virtualPanelInputs.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有 DI 输入点"
                : null;

            // 如果当前 VirtualPanel 里的卡片结构与传入数据一致，
            // 则只更新每张卡片的数据，不重建列表。
            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedLogicalBit);
                return;
            }

            // 如果结构发生变化，则重建。
            RebuildItems(sourceItems, selectedLogicalBit);
        }

        /// <summary>
        /// 绑定内部事件。
        ///
        /// 这里仅绑定一次 `VirtualPanel.ItemClick`。
        /// 因为卡片不是 WinForms 控件，点击逻辑统一由 VirtualPanel 分发。
        /// </summary>
        private void BindEvents()
        {
            virtualPanelInputs.ItemClick += VirtualPanelInputs_ItemClick;
        }

        /// <summary>
        /// VirtualPanel 卡片点击处理。
        ///
        /// 运行机制：
        /// 1. 用户点击的是 `VirtualPanel` 内部绘制出的某个 `VirtualItem`；
        /// 2. `VirtualPanel` 命中该项后触发 `ItemClick`；
        /// 3. 这里把点击项转换为 `DIMotionVirtualCardItem`；
        /// 4. 再把对应的逻辑位号抛给外层页面。
        /// </summary>
        private void VirtualPanelInputs_ItemClick(object sender, VirtualItemEventArgs e)
        {
            var cardItem = e == null ? null : e.Item as DIMotionVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
                return;

            var handler = ItemSelected;
            if (handler != null)
                handler(this, new DIMotionItemSelectedEventArgs(cardItem.Item.LogicalBit));
        }

        /// <summary>
        /// 判断当前列表是否可以“原地更新”。
        ///
        /// 这里判断的不是内容是否变化，而是“结构是否变化”。
        ///
        /// 所谓结构一致，指：
        /// 1. 当前 VirtualPanel 里的卡片数量 == 新数据数量；
        /// 2. 每个位置上的逻辑位号顺序都相同。
        ///
        /// 只要这两个条件成立，就说明：
        /// - 页面仍然是同一页；
        /// - 筛选条件没有导致项目顺序变化；
        /// - 只是状态值、名称、选中态等内容变了；
        /// 这时就可以安全地原地更新，避免滚动位置丢失。
        ///
        /// 为什么只比 `LogicalBit`：
        /// - `LogicalBit` 在当前页中可视作卡片稳定标识；
        /// - 刷新后同一张卡片即使对象实例变了，逻辑位仍然不变；
        /// - 用它判断比对最稳定、成本也最低。
        /// </summary>
        private bool CanUpdateInPlace(IList<DIMotionPageModel.DIMotionIoViewItem> items)
        {
            if (items == null)
                return virtualPanelInputs.Items.Count == 0;

            // 数量变了，直接说明结构变了，需要重建。
            if (virtualPanelInputs.Items.Count != items.Count)
                return false;

            // 逐项比较逻辑位顺序。
            // 顺序一旦不同，说明分页/筛选/排序已变化，需要重建。
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as DIMotionVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                    return false;

                if (virtualItem.Item.LogicalBit != items[i].LogicalBit)
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
        /// - 适合高频状态刷新。
        ///
        /// 对运行界面的影响：
        /// - 卡片位置不变；
        /// - 只更新卡片里面的文字、颜色、ON/OFF 状态、选中边框。
        /// </summary>
        private void UpdateItemsInPlace(IList<DIMotionPageModel.DIMotionIoViewItem> items, short? selectedLogicalBit)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as DIMotionVirtualCardItem;
                if (virtualItem == null)
                    continue;

                virtualItem.Bind(
                    items[i],
                    selectedLogicalBit.HasValue && selectedLogicalBit.Value == items[i].LogicalBit);
            }

            // 通知 VirtualPanel 重新绘制。
            // 这里只重绘，不重建布局，因此滚动条位置能保留。
            virtualPanelInputs.Invalidate();
        }

        /// <summary>
        /// 重建当前页卡片。
        ///
        /// 触发场景：
        /// - 页码改变；
        /// - 搜索条件变化；
        /// - 控制卡筛选变化；
        /// - 卡片集合顺序变化；
        /// - 首次加载。
        ///
        /// 这里必须重建，因为原来的卡片结构已经不再对应当前页数据。
        ///
        /// `PauseLayout = true` 的目的：
        /// - 暂停 VirtualPanel 的自动布局；
        /// - 避免 Clear/AddRange 过程中重复计算布局；
        /// - 最后一次性恢复布局，提高效率。
        /// </summary>
        private void RebuildItems(IList<DIMotionPageModel.DIMotionIoViewItem> items, short? selectedLogicalBit)
        {
            virtualPanelInputs.PauseLayout = true;
            try
            {
                // 先清掉旧页数据。
                virtualPanelInputs.Items.Clear();

                if (items != null && items.Count > 0)
                {
                    var virtualItems = new List<VirtualItem>(items.Count);

                    foreach (var item in items)
                    {
                        virtualItems.Add(new DIMotionVirtualCardItem(
                            item,
                            selectedLogicalBit.HasValue && selectedLogicalBit.Value == item.LogicalBit));
                    }

                    // 一次性添加，减少多次刷新带来的性能损耗。
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
        /// 这里只传逻辑位号，避免直接暴露内部虚拟项实现。
        /// 页面层只需要知道“用户选中了哪个逻辑位”即可。
        /// </summary>
        public sealed class DIMotionItemSelectedEventArgs : EventArgs
        {
            public DIMotionItemSelectedEventArgs(short logicalBit)
            {
                LogicalBit = logicalBit;
            }

            /// <summary>
            /// 被点击卡片对应的逻辑位号。
            /// 外层页面通过这个值更新 SelectedItem。
            /// </summary>
            public short LogicalBit { get; private set; }
        }

        /// <summary>
        /// VirtualPanel 中的自绘 DI 卡片。
        ///
        /// 这是“真正的卡片实现”。
        ///
        /// 重要：
        /// - 这里不是 WinForms 的 `Control`；
        /// - 而是 AntdUI 的 `VirtualShadowItem`；
        /// - 它只提供尺寸计算、绘制、鼠标命中等虚拟行为；
        /// - 具体显示什么，完全由 `Size()` 和 `Paint()` 决定。
        ///
        /// 可以把它理解为：
        /// “左侧每一张 DI 卡片的绘制模板”。
        /// </summary>
        private sealed class DIMotionVirtualCardItem : VirtualShadowItem
        {
            /// <summary>
            /// 卡片主标题字体：显示 DI 名称。
            /// </summary>
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);

            /// <summary>
            /// 卡片正文/说明字体：显示类型、位号、卡信息等。
            /// </summary>
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);

            /// <summary>
            /// 左上角小标签字体，例如 DI / ON / OFF。
            /// </summary>
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            /// <summary>
            /// 当前卡片是否为选中态。
            /// 选中态主要体现在背景色和边框色上。
            /// </summary>
            private bool _selected;

            public DIMotionVirtualCardItem(DIMotionPageModel.DIMotionIoViewItem item, bool selected)
            {
                Bind(item, selected);

                // 允许点击，否则 VirtualPanel 命中后不会把它作为可点击项处理。
                CanClick = true;
            }

            /// <summary>
            /// 当前卡片绑定的数据项。
            /// Paint() 中显示的所有内容都从这里读取。
            /// </summary>
            public DIMotionPageModel.DIMotionIoViewItem Item { get; private set; }

            /// <summary>
            /// 原地更新卡片数据。
            ///
            /// 这个方法只负责替换数据，不负责布局。
            /// 也就是说：
            /// - 卡片还是原来的卡片；
            /// - 只是把里面显示的数据换掉。
            ///
            /// 这也是保留滚动位置的关键之一。
            /// </summary>
            public void Bind(DIMotionPageModel.DIMotionIoViewItem item, bool selected)
            {
                Item = item;
                _selected = selected;

                // Tag 主要给调试或事件处理中透传原始数据。
                Tag = item;
            }

            /// <summary>
            /// 返回卡片尺寸。
            ///
            /// `VirtualPanel` 会先调用这个方法，知道每张卡片多大，
            /// 然后再决定整个虚拟列表的排布方式。
            ///
            /// 运行界面中与这两个数值对应：
            /// - 224：卡片宽度
            /// - 126：卡片高度
            ///
            /// 如果运行时要改“每行显示几张卡片”，
            /// 通常优先从这里的宽度入手，再配合 `VirtualPanel.Gap` 调整。
            ///
            /// 这里乘以 DPI 的原因：
            /// - 支持高分屏；
            /// - 避免缩放后卡片过小或过大。
            /// </summary>
            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(170 * dpi),
                    (int)Math.Round(100 * dpi));
            }

            /// <summary>
            /// 鼠标移动时是否认为当前项可交互。
            ///
            /// 返回 true 即可，让 VirtualPanel 继续处理 hover / hand cursor 等行为。
            /// 这里不做额外命中细分，整个卡片区域都允许交互。
            /// </summary>
            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            /// <summary>
            /// 绘制卡片内容。
            ///
            /// 这个方法决定了运行界面中每一张 DI 卡片的最终样子。
            ///
            /// 布局坐标与运行界面对应关系：
            /// -------------------------------------------------
            /// (12,12)  左上角 DI 标签
            /// (50,12)  左上角 ON/OFF 标签
            /// 右上角   逻辑位号 L#xxxx
            /// (12,46)  主标题（显示名称）
            /// (12,72)  分类/类型
            /// (12,92)  硬件位、Core、板载/扩展
            /// (12,108) 底部状态小圆点
            /// (26,102) 底部控制卡文字
            ///
            /// 如果运行时看到文字偏了、挤了、遮挡了，
            /// 基本就是来改这些坐标。
            /// </summary>
            public override void Paint(Canvas g, VirtualPanelArgs e)
            {
                var isDark = AntdUI.Config.IsDark;

                // 当前卡片的绘制区域。
                // 这里的 (0,0) 是“单张卡片内部坐标系”，不是整个页面坐标。
                var rect = new Rectangle(0, 0, e.Rect.Width, e.Rect.Height);

                // 根据主题和选中态，计算卡片底色。
                var backColor = _selected
                    ? (isDark ? Color.FromArgb(29, 58, 97) : Color.FromArgb(237, 246, 255))
                    : (isDark ? Color.FromArgb(39, 44, 52) : Color.FromArgb(255, 255, 255));

                // 根据主题和选中态，计算边框色。
                var borderColor = _selected
                    ? Color.FromArgb(22, 119, 255)
                    : (isDark ? Color.FromArgb(72, 79, 92) : Color.FromArgb(225, 229, 235));

                var textColor = isDark
                    ? Color.FromArgb(235, 235, 235)
                    : Color.FromArgb(38, 38, 38);

                var subTextColor = isDark
                    ? Color.FromArgb(170, 176, 186)
                    : Color.FromArgb(120, 120, 120);

                // 语义色：
                // ON / OFF / 主标签等都通过这里统一控制。
                var onColor = Color.FromArgb(82, 196, 26);
                var offColor = Color.FromArgb(245, 108, 108);
                var primaryColor = Color.FromArgb(22, 119, 255);

                // 绘制整张卡片的圆角背景和边框。
                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, _selected ? 1.6F : 1F, path);
                }

                // 左上角 "DI" 类型标签。
                DrawBadge(
                    g,
                    new Rectangle(12, 12, 34, 22),
                    primaryColor,
                    "DI");

                // 左上角 ON/OFF 值标签。
                DrawBadge(
                    g,
                    new Rectangle(50, 12, 48, 22),
                    Item.CurrentValue ? onColor : offColor,
                    Item.ValueText);

                // 右上角逻辑位号。
                g.String(
                    "L#" + Item.LogicalBit,
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 56, 12));

                // 右上角逻辑位号下方 硬件位号。
                g.String(
                    "H#" + Item.HardwareBit,
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 40, 32));

                // 主标题：DI 显示名称。
                g.String(
                    TrimText(Item.DisplayTitle, 18),
                    FontTitle,
                    textColor,
                    new Point(12, 46));

                // 第二行：分类/类型信息。
                g.String(
                    TrimText(Item.TypeText, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 68));

                // 第二行： 板载/扩展。
                g.String(
                    Item.ModuleText,
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 40, 68));

                // 第三行：硬件位 + Core + 板载/扩展。
                //g.String(
                //    TrimText("HW " + Item.HardwareBit + " · " + Item.CoreText + " · " + Item.ModuleText, 28),
                //    FontBody,
                //    subTextColor,
                //    new Point(12, 92));

                // 左下角状态小圆点。
                //g.FillEllipse(
                //    Item.CurrentValue ? onColor : offColor,
                //    new Rectangle(12, 108, 8, 8));

                // 底部控制卡信息。
                //g.String(
                //    TrimText(Item.CardText, 18),
                //    FontBody,
                //    subTextColor,
                //    new Point(26, 102));
            }

            /// <summary>
            /// 绘制一个小标签。
            ///
            /// 这里主要用于：
            /// - DI 标签
            /// - ON / OFF 状态标签
            ///
            /// 如果运行时觉得标签宽度不够、圆角太大/太小，
            /// 就优先修改传入的 Rectangle 或 RoundPath 半径。
            /// </summary>
            private static void DrawBadge(Canvas g, Rectangle rect, Color backColor, string text)
            {
                using (var path = rect.RoundPath(6))
                {
                    g.Fill(backColor, path);
                }

                g.String(
                    text,
                    FontBadge,
                    Color.White,
                    rect);
            }

            /// <summary>
            /// 文字裁剪。
            ///
            /// 由于卡片尺寸固定，而名称、控制卡信息、类型文字长度可能不固定，
            /// 所以这里先做一个轻量级字符串裁剪，避免内容溢出太多。
            ///
            /// 注意：
            /// 这里是“字符数裁剪”，不是像素级测量。
            /// 优点是快；
            /// 缺点是不同字符宽度不完全一致。
            ///
            /// 如果后续要更精细，可以改成 `g.MeasureString(...)` 方式做像素裁剪。
            /// 当前阶段这个实现更简单、性能也更稳定。
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