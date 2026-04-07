using AM.PageModel.Motion;
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
    /// 1. 左侧轴卡片数量可能很多，不能为每个轴都创建真实 WinForms 控件；
    /// 2. 使用 AntdUI 的 VirtualPanel 进行“虚拟绘制”，减少控件数量；
    /// 3. 保证滚动流畅，并尽量保留滚动位置；
    /// 4. 刷新时优先原地更新，避免频繁整页重建。
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
        /// 页面收到逻辑轴号后，再更新右侧详情区。
        /// </summary>
        public event EventHandler<MotionAxisItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前页轴卡片数据。
        /// </summary>
        public void BindItems(
            IList<MotionMonitorPageModel.MotionAxisViewItem> items,
            MotionMonitorPageModel.MotionAxisViewItem selectedItem)
        {
            var sourceItems = items ?? new List<MotionMonitorPageModel.MotionAxisViewItem>();
            var selectedLogicalAxis = selectedItem == null
                ? (short?)null
                : selectedItem.LogicalAxis;

            virtualPanelInputs.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有轴"
                : null;

            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedLogicalAxis);
                return;
            }

            RebuildItems(sourceItems, selectedLogicalAxis);
        }

        /// <summary>
        /// 绑定内部事件。
        /// </summary>
        private void BindEvents()
        {
            virtualPanelInputs.ItemClick += VirtualPanelInputs_ItemClick;
        }

        /// <summary>
        /// VirtualPanel 卡片点击处理。
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
        /// 判断当前页是否可以原地更新。
        /// 如果数量与逻辑轴顺序都不变，则只更新显示内容，不重建列表。
        /// </summary>
        private bool CanUpdateInPlace(IList<MotionMonitorPageModel.MotionAxisViewItem> items)
        {
            if (items == null)
                return virtualPanelInputs.Items.Count == 0;

            if (virtualPanelInputs.Items.Count != items.Count)
                return false;

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
        /// 不清空 Items，从而尽量保留当前滚动位置。
        /// </summary>
        private void UpdateItemsInPlace(IList<MotionMonitorPageModel.MotionAxisViewItem> items, short? selectedLogicalAxis)
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

            virtualPanelInputs.Invalidate();
        }

        /// <summary>
        /// 重建当前页卡片。
        /// 仅在分页、筛选、搜索导致结构变化时执行。
        /// </summary>
        private void RebuildItems(IList<MotionMonitorPageModel.MotionAxisViewItem> items, short? selectedLogicalAxis)
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
                        virtualItems.Add(new MotionAxisVirtualCardItem(
                            item,
                            selectedLogicalAxis.HasValue && selectedLogicalAxis.Value == item.LogicalAxis));
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
        public sealed class MotionAxisItemSelectedEventArgs : EventArgs
        {
            public MotionAxisItemSelectedEventArgs(short logicalAxis)
            {
                LogicalAxis = logicalAxis;
            }

            public short LogicalAxis { get; private set; }
        }

        /// <summary>
        /// VirtualPanel 中的自绘轴卡片。
        /// 真正的布局和绘制都在这里定义。
        /// </summary>
        private sealed class MotionAxisVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public MotionAxisVirtualCardItem(MotionMonitorPageModel.MotionAxisViewItem item, bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public MotionMonitorPageModel.MotionAxisViewItem Item { get; private set; }

            /// <summary>
            /// 原地更新卡片数据。
            /// </summary>
            public void Bind(MotionMonitorPageModel.MotionAxisViewItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            /// <summary>
            /// 卡片尺寸。
            /// 这里决定了每行大致能放下几张卡片。
            /// </summary>
            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(170 * dpi),
                    (int)Math.Round(118 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            /// <summary>
            /// 绘制单张轴卡片。
            /// 第一阶段先按“逻辑轴 / Enable / 名称 / 类型 / 指令位置 / 编码器位置”实现。
            /// </summary>
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

                var primaryColor = Color.FromArgb(22, 119, 255);
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

                DrawBadge(g, new Rectangle(12, 12, 44, 22), primaryColor, "L#" + Item.LogicalAxis);
                DrawBadge(g, new Rectangle(60, 12, 58, 22), enableColor, Item.EnableText);

                g.String(
                    Item.StateText,
                    FontBody,
                    stateColor,
                    new Point(rect.Width - 54, 16));

                g.String(
                    TrimText(Item.DisplayTitle, 18),
                    FontTitle,
                    textColor,
                    new Point(12, 44));

                g.String(
                    TrimText("类型：" + Item.AxisCategoryText, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 68));

                g.String(
                    TrimText("指令：" + Item.CommandPositionMm.ToString("0.###") + " mm", 26),
                    FontBody,
                    subTextColor,
                    new Point(12, 88));

                g.String(
                    TrimText("编码器：" + Item.EncoderPositionMm.ToString("0.###") + " mm", 26),
                    FontBody,
                    subTextColor,
                    new Point(12, 104));
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