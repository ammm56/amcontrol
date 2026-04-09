using AM.PageModel.Motion;
using AM.PageModel.Motion.DO;
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
    /// DO 虚拟卡片列表。
    /// 使用 AntdUI 的 VirtualPanel 渲染大量 IO 卡片，减少实际控件数量。
    /// </summary>
    public partial class DOMotionVirtualListControl : UserControl
    {
        public DOMotionVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 卡片选中事件。
        /// </summary>
        public event EventHandler<DOMotionItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前页数据。
        /// 1. 定时刷新时尽量原地更新，避免 VirtualPanel 重建导致滚动位置回顶；
        /// 2. 仅当分页/筛选/数据结构变化时，才执行整批重建。
        /// </summary>
        public void BindItems(IList<DOMotionIoViewItem> items, DOMotionIoViewItem selectedItem)
        {
            var sourceItems = items ?? new List<DOMotionIoViewItem>();
            var selectedLogicalBit = selectedItem == null
                ? (short?)null
                : selectedItem.LogicalBit;

            virtualPanelInputs.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有 DO 输出点"
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
            var cardItem = e == null ? null : e.Item as DOMotionVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
                return;

            var handler = ItemSelected;
            if (handler != null)
                handler(this, new DOMotionItemSelectedEventArgs(cardItem.Item.LogicalBit));
        }

        private bool CanUpdateInPlace(IList<DOMotionIoViewItem> items)
        {
            if (items == null)
                return virtualPanelInputs.Items.Count == 0;

            if (virtualPanelInputs.Items.Count != items.Count)
                return false;

            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as DOMotionVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                    return false;

                if (virtualItem.Item.LogicalBit != items[i].LogicalBit)
                    return false;
            }

            return true;
        }

        private void UpdateItemsInPlace(IList<DOMotionIoViewItem> items, short? selectedLogicalBit)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as DOMotionVirtualCardItem;
                if (virtualItem == null)
                    continue;

                virtualItem.Bind(
                    items[i],
                    selectedLogicalBit.HasValue && selectedLogicalBit.Value == items[i].LogicalBit);
            }

            virtualPanelInputs.Invalidate();
        }

        private void RebuildItems(IList<DOMotionIoViewItem> items, short? selectedLogicalBit)
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
                        virtualItems.Add(new DOMotionVirtualCardItem(
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

        public sealed class DOMotionItemSelectedEventArgs : EventArgs
        {
            public DOMotionItemSelectedEventArgs(short logicalBit)
            {
                LogicalBit = logicalBit;
            }

            public short LogicalBit { get; private set; }
        }

        private sealed class DOMotionVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public DOMotionVirtualCardItem(DOMotionIoViewItem item, bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public DOMotionIoViewItem Item { get; private set; }

            public void Bind(DOMotionIoViewItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(170 * dpi),
                    (int)Math.Round(100 * dpi));
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
                    ? (isDark ? Color.FromArgb(88, 52, 20) : Color.FromArgb(255, 247, 237))
                    : (isDark ? Color.FromArgb(39, 44, 52) : Color.FromArgb(255, 255, 255));

                var borderColor = _selected
                    ? Color.FromArgb(250, 140, 22)
                    : (isDark ? Color.FromArgb(72, 79, 92) : Color.FromArgb(225, 229, 235));

                var textColor = isDark
                    ? Color.FromArgb(235, 235, 235)
                    : Color.FromArgb(38, 38, 38);

                var subTextColor = isDark
                    ? Color.FromArgb(170, 176, 186)
                    : Color.FromArgb(120, 120, 120);

                var onColor = Color.FromArgb(82, 196, 26);
                var offColor = Color.FromArgb(245, 108, 108);
                var primaryColor = Color.FromArgb(250, 140, 22);

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, _selected ? 1.6F : 1F, path);
                }

                DrawBadge(g, new Rectangle(12, 12, 34, 22), primaryColor, "DO");
                DrawBadge(g, new Rectangle(50, 12, 48, 22), Item.CurrentValue ? onColor : offColor, Item.ValueText);

                g.String(
                    "L#" + Item.LogicalBit,
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 56, 12));

                g.String(
                    "H#" + Item.HardwareBit,
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 40, 32));

                g.String(
                    TrimText(Item.DisplayTitle, 18),
                    FontTitle,
                    textColor,
                    new Point(12, 46));

                g.String(
                    TrimText(Item.TypeText, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 68));

                g.String(
                    Item.ModuleText,
                    FontBody,
                    subTextColor,
                    new Point(rect.Width - 40, 68));
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