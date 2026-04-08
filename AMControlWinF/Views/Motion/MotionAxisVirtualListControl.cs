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
    /// 单轴控制动作虚拟卡片列表。
    ///
    /// 设计目标：
    /// 1. 左侧区域可能承载较多动作卡片，不能为每张卡片都创建真实 WinForms 控件；
    /// 2. 使用 AntdUI 的 `VirtualPanel` 进行“虚拟绘制”，只在可见区域绘制卡片；
    /// 3. 减少控件数量、降低内存占用，并保证滚动时更流畅；
    /// 4. 刷新时优先原地更新，而不是整批清空重建，尽量保留滚动位置。
    ///
    /// 重要说明：
    /// - 这里的动作卡片不是普通 `UserControl`；
    /// - 而是 `VirtualPanel` 内部的 `VirtualItem`；
    /// - Designer 中能看到的只是 VirtualPanel 容器本身；
    /// - 真正的卡片内容、尺寸、颜色、间距全部由下面的代码控制。
    /// </summary>
    public partial class MotionAxisVirtualListControl : UserControl
    {
        public MotionAxisVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 动作卡片选中事件。
        /// 页面层收到动作键后，再刷新右侧详情区。
        /// </summary>
        public event EventHandler<MotionAxisActionItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前页动作卡片数据。
        ///
        /// 刷新策略：
        /// 1. 普通定时刷新优先原地更新；
        /// 2. 仅当分页/搜索导致结构变化时，才整批重建。
        /// </summary>
        public void BindItems(
            IList<MotionAxisPageModel.MotionAxisActionViewItem> items,
            MotionAxisPageModel.MotionAxisActionViewItem selectedItem)
        {
            var sourceItems = items ?? new List<MotionAxisPageModel.MotionAxisActionViewItem>();
            var selectedActionKey = selectedItem == null ? null : selectedItem.ActionKey;

            virtualPanelInputs.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有动作卡片"
                : null;

            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedActionKey);
                return;
            }

            RebuildItems(sourceItems, selectedActionKey);
        }

        /// <summary>
        /// 绑定内部事件。
        /// 虚拟卡片的点击统一由 VirtualPanel 分发。
        /// </summary>
        private void BindEvents()
        {
            virtualPanelInputs.ItemClick += VirtualPanelInputs_ItemClick;
        }

        /// <summary>
        /// VirtualPanel 卡片点击处理。
        /// 将点击动作项转换为页面层可直接使用的 ActionKey。
        /// </summary>
        private void VirtualPanelInputs_ItemClick(object sender, VirtualItemEventArgs e)
        {
            var cardItem = e == null ? null : e.Item as MotionAxisActionVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
                return;

            var handler = ItemSelected;
            if (handler != null)
                handler(this, new MotionAxisActionItemSelectedEventArgs(cardItem.Item.ActionKey));
        }

        /// <summary>
        /// 判断当前列表是否可以原地更新。
        /// 条件：
        /// 1. 数量一致；
        /// 2. 同位置上的 ActionKey 顺序一致。
        /// </summary>
        private bool CanUpdateInPlace(IList<MotionAxisPageModel.MotionAxisActionViewItem> items)
        {
            if (items == null)
                return virtualPanelInputs.Items.Count == 0;

            if (virtualPanelInputs.Items.Count != items.Count)
                return false;

            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as MotionAxisActionVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                    return false;

                if (!string.Equals(virtualItem.Item.ActionKey, items[i].ActionKey, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 原地更新当前页卡片数据。
        /// 不清空 Items，从而尽量保留滚动位置。
        /// </summary>
        private void UpdateItemsInPlace(IList<MotionAxisPageModel.MotionAxisActionViewItem> items, string selectedActionKey)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelInputs.Items[i] as MotionAxisActionVirtualCardItem;
                if (virtualItem == null)
                    continue;

                virtualItem.Bind(
                    items[i],
                    string.Equals(selectedActionKey, items[i].ActionKey, StringComparison.OrdinalIgnoreCase));
            }

            virtualPanelInputs.Invalidate();
        }

        /// <summary>
        /// 重建当前页卡片。
        /// 仅在分页、搜索等造成结构变化时执行。
        /// </summary>
        private void RebuildItems(IList<MotionAxisPageModel.MotionAxisActionViewItem> items, string selectedActionKey)
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
                        virtualItems.Add(new MotionAxisActionVirtualCardItem(
                            item,
                            string.Equals(selectedActionKey, item.ActionKey, StringComparison.OrdinalIgnoreCase)));
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
        /// 动作项选中事件参数。
        /// </summary>
        public sealed class MotionAxisActionItemSelectedEventArgs : EventArgs
        {
            public MotionAxisActionItemSelectedEventArgs(string actionKey)
            {
                ActionKey = actionKey;
            }

            public string ActionKey { get; private set; }
        }

        /// <summary>
        /// VirtualPanel 中的自绘动作卡片。
        /// 真正的卡片布局和绘制都在这里定义。
        /// </summary>
        private sealed class MotionAxisActionVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public MotionAxisActionVirtualCardItem(MotionAxisPageModel.MotionAxisActionViewItem item, bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public MotionAxisPageModel.MotionAxisActionViewItem Item { get; private set; }

            /// <summary>
            /// 原地更新卡片数据。
            /// </summary>
            public void Bind(MotionAxisPageModel.MotionAxisActionViewItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            /// <summary>
            /// 卡片尺寸。
            /// 这里决定了每行卡片数和滚动时的占位高度。
            /// </summary>
            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(176 * dpi),
                    (int)Math.Round(108 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            /// <summary>
            /// 绘制单张动作卡片。
            ///
            /// 内容结构：
            /// 1. 左上角显示动作分类；
            /// 2. 右上角显示“已选择轴 / 需先选轴”；
            /// 3. 中间显示动作名称；
            /// 4. 底部显示参数要求说明。
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

                var accentColor = ResolveAccentColor(Item.AccentType);
                var stateColor = Item.HasSelectedAxis
                    ? Color.FromArgb(82, 196, 26)
                    : Color.FromArgb(160, 160, 160);

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, _selected ? 1.6F : 1F, path);
                }

                DrawBadge(g, new Rectangle(12, 12, 54, 22), accentColor, TrimText(Item.CategoryText, 6));
                DrawBadge(g, new Rectangle(rect.Width - 74, 12, 62, 22), stateColor, Item.HasSelectedAxis ? "已选轴" : "待选轴");

                g.String(
                    TrimText(Item.DisplayText, 10),
                    FontTitle,
                    textColor,
                    new Point(12, 46));

                g.String(
                    TrimText(Item.DescriptionText, 22),
                    FontBody,
                    subTextColor,
                    new Point(12, 70));

                g.String(
                    TrimText(Item.ParameterHintText, 22),
                    FontBody,
                    subTextColor,
                    new Point(12, 90));
            }

            private static Color ResolveAccentColor(string accentType)
            {
                if (string.Equals(accentType, "Danger", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(245, 108, 108);

                if (string.Equals(accentType, "Warning", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(250, 140, 22);

                if (string.Equals(accentType, "Success", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(82, 196, 26);

                if (string.Equals(accentType, "Default", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(96, 125, 139);

                return Color.FromArgb(22, 119, 255);
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