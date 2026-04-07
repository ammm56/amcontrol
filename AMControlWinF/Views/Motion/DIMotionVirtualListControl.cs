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
    /// 使用 AntdUI 的 VirtualPanel 渲染大量 IO 卡片，减少实际控件数量。
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
        /// </summary>
        public event EventHandler<DIMotionItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前页数据。
        /// 说明：
        /// 1. 定时刷新时尽量原地更新，避免 VirtualPanel 重建导致滚动位置回顶；
        /// 2. 仅当分页/筛选/数据结构变化时，才执行整批重建。
        /// </summary>
        public void BindItems(IList<DIMotionPageModel.DIMotionIoViewItem> items, DIMotionPageModel.DIMotionIoViewItem selectedItem)
        {
            var sourceItems = items ?? new List<DIMotionPageModel.DIMotionIoViewItem>();
            var selectedLogicalBit = selectedItem == null
                ? (short?)null
                : selectedItem.LogicalBit;

            virtualPanelInputs.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有 DI 输入点"
                : null;

            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedLogicalBit);
                return;
            }

            RebuildItems(sourceItems, selectedLogicalBit);
        }

        private void BindEvents()
        {
            virtualPanelInputs.ItemClick += VirtualPanelInputs_ItemClick;
        }

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
        /// 判断当前 VirtualPanel 中的项是否可以原地更新。
        /// 仅比较当前页逻辑位顺序即可：
        /// - 一致：说明只是状态值变化或选中变化，可直接更新；
        /// - 不一致：说明分页/筛选/排序发生变化，需要重建。
        /// </summary>
        private bool CanUpdateInPlace(IList<DIMotionPageModel.DIMotionIoViewItem> items)
        {
            if (items == null)
                return virtualPanelInputs.Items.Count == 0;

            if (virtualPanelInputs.Items.Count != items.Count)
                return false;

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
        /// 原地更新，保留滚动位置。
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

            virtualPanelInputs.Invalidate();
        }

        /// <summary>
        /// 分页或筛选发生变化时重建当前页卡片。
        /// </summary>
        private void RebuildItems(IList<DIMotionPageModel.DIMotionIoViewItem> items, short? selectedLogicalBit)
        {
            virtualPanelInputs.PauseLayout = true;
            try
            {
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

                    virtualPanelInputs.Items.AddRange(virtualItems);
                }
            }
            finally
            {
                virtualPanelInputs.PauseLayout = false;
            }
        }

        /// <summary>
        /// 列表项选中事件参数。
        /// </summary>
        public sealed class DIMotionItemSelectedEventArgs : EventArgs
        {
            public DIMotionItemSelectedEventArgs(short logicalBit)
            {
                LogicalBit = logicalBit;
            }

            public short LogicalBit { get; private set; }
        }

        /// <summary>
        /// VirtualPanel 中的自绘 DI 卡片。
        /// </summary>
        private sealed class DIMotionVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public DIMotionVirtualCardItem(DIMotionPageModel.DIMotionIoViewItem item, bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public DIMotionPageModel.DIMotionIoViewItem Item { get; private set; }

            /// <summary>
            /// 原地更新卡片数据。
            /// </summary>
            public void Bind(DIMotionPageModel.DIMotionIoViewItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(224 * dpi),
                    (int)Math.Round(126 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            public override void Paint(Canvas g, VirtualPanelArgs e)
            {
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

                var onColor = Color.FromArgb(82, 196, 26);
                var offColor = Color.FromArgb(245, 108, 108);
                var primaryColor = Color.FromArgb(22, 119, 255);

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, _selected ? 1.6F : 1F, path);
                }

                DrawBadge(g, new Rectangle(12, 12, 34, 22), primaryColor, "DI");
                DrawBadge(g, new Rectangle(50, 12, 48, 22), Item.CurrentValue ? onColor : offColor, Item.ValueText);

                g.String(
                    "L#" + Item.LogicalBit,
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 56, 16));

                g.String(
                    TrimText(Item.DisplayTitle, 18),
                    FontTitle,
                    textColor,
                    new Point(12, 46));

                g.String(
                    TrimText(Item.TypeText, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 72));

                g.String(
                    TrimText("HW " + Item.HardwareBit + " · " + Item.CoreText + " · " + Item.ModuleText, 28),
                    FontBody,
                    subTextColor,
                    new Point(12, 92));

                g.FillEllipse(
                    Item.CurrentValue ? onColor : offColor,
                    new Rectangle(12, 108, 8, 8));

                g.String(
                    TrimText(Item.CardText, 18),
                    FontBody,
                    subTextColor,
                    new Point(26, 102));
            }

            private static void DrawBadge(Canvas g, Rectangle rect, Color backColor, string text)
            {
                using (var path = rect.RoundPath(6))
                {
                    g.Fill(backColor, path);
                }

                g.String(text, FontBadge, Color.White, rect);
            }

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