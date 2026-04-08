using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器虚拟卡片列表。
    ///
    /// 设计目标：
    /// 1. 执行器数量可能较多，不能为每个对象创建真实 WinForms 控件；
    /// 2. 使用 VirtualPanel + VirtualItem 方式绘制卡片，保证滚动流畅；
    /// 3. 与 DI/DO/多轴总览左侧卡片实现方式保持一致；
    /// 4. 左侧卡片只负责选择，不直接执行动作。
    /// </summary>
    public partial class MotionActuatorVirtualListControl : UserControl
    {
        public MotionActuatorVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 卡片选中事件。
        /// 页面层收到后刷新右侧详情区。
        /// </summary>
        public event EventHandler<MotionActuatorItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前显示的执行器卡片集合。
        /// 结构不变时原地更新，避免滚动位置丢失。
        /// </summary>
        public void BindItems(
            IList<MotionActuatorPageModel.MotionActuatorViewItem> items,
            MotionActuatorPageModel.MotionActuatorViewItem selectedItem)
        {
            var sourceItems = items ?? new List<MotionActuatorPageModel.MotionActuatorViewItem>();
            var selectedKey = selectedItem == null ? null : selectedItem.ItemKey;

            virtualPanelItems.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有执行器对象"
                : null;

            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedKey);
                return;
            }

            RebuildItems(sourceItems, selectedKey);
        }

        private void BindEvents()
        {
            virtualPanelItems.ItemClick += VirtualPanelItems_ItemClick;
        }

        private void VirtualPanelItems_ItemClick(object sender, VirtualItemEventArgs e)
        {
            var cardItem = e == null ? null : e.Item as MotionActuatorVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
                return;

            var handler = ItemSelected;
            if (handler != null)
                handler(this, new MotionActuatorItemSelectedEventArgs(cardItem.Item.ItemKey));
        }

        private bool CanUpdateInPlace(IList<MotionActuatorPageModel.MotionActuatorViewItem> items)
        {
            if (items == null)
                return virtualPanelItems.Items.Count == 0;

            if (virtualPanelItems.Items.Count != items.Count)
                return false;

            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelItems.Items[i] as MotionActuatorVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                    return false;

                if (!string.Equals(virtualItem.Item.ItemKey, items[i].ItemKey, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private void UpdateItemsInPlace(
            IList<MotionActuatorPageModel.MotionActuatorViewItem> items,
            string selectedKey)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelItems.Items[i] as MotionActuatorVirtualCardItem;
                if (virtualItem == null)
                    continue;

                virtualItem.Bind(
                    items[i],
                    !string.IsNullOrWhiteSpace(selectedKey)
                    && string.Equals(selectedKey, items[i].ItemKey, StringComparison.OrdinalIgnoreCase));
            }

            virtualPanelItems.Invalidate();
        }

        private void RebuildItems(
            IList<MotionActuatorPageModel.MotionActuatorViewItem> items,
            string selectedKey)
        {
            virtualPanelItems.PauseLayout = true;
            try
            {
                virtualPanelItems.Items.Clear();

                if (items != null && items.Count > 0)
                {
                    var virtualItems = new List<VirtualItem>(items.Count);
                    foreach (var item in items)
                    {
                        virtualItems.Add(new MotionActuatorVirtualCardItem(
                            item,
                            !string.IsNullOrWhiteSpace(selectedKey)
                            && string.Equals(selectedKey, item.ItemKey, StringComparison.OrdinalIgnoreCase)));
                    }

                    virtualPanelItems.Items.AddRange(virtualItems);
                }
            }
            finally
            {
                virtualPanelItems.PauseLayout = false;
            }
        }

        public sealed class MotionActuatorItemSelectedEventArgs : EventArgs
        {
            public MotionActuatorItemSelectedEventArgs(string itemKey)
            {
                ItemKey = itemKey;
            }

            public string ItemKey { get; private set; }
        }

        /// <summary>
        /// VirtualPanel 中的执行器卡片。
        /// 卡片显示：
        /// - 左上角分类标签
        /// - 状态标签
        /// - 名称
        /// - 内部名称
        /// - 模式
        /// - DO / 红黄 / 绿蓝鸣摘要
        /// </summary>
        private sealed class MotionActuatorVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public MotionActuatorVirtualCardItem(
                MotionActuatorPageModel.MotionActuatorViewItem item,
                bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public MotionActuatorPageModel.MotionActuatorViewItem Item { get; private set; }

            public void Bind(MotionActuatorPageModel.MotionActuatorViewItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(178 * dpi),
                    (int)Math.Round(124 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            public override void Paint(Canvas g, VirtualPanelArgs e)
            {
                if (Item == null)
                    return;

                var isDark = AntdUI.Config.IsDark;
                var rect = new Rectangle(0, 0, e.Rect.Width, e.Rect.Height);

                var backColor = _selected
                    ? (isDark ? Color.FromArgb(33, 46, 62) : Color.FromArgb(240, 247, 255))
                    : (isDark ? Color.FromArgb(39, 44, 52) : Color.White);

                var borderColor = _selected
                    ? Color.FromArgb(22, 119, 255)
                    : (Hover
                        ? (isDark ? Color.FromArgb(88, 96, 108) : Color.FromArgb(225, 229, 235))
                        : (isDark ? Color.FromArgb(64, 72, 84) : Color.FromArgb(236, 239, 244)));

                var borderWidth = _selected ? 1.6F : (Hover ? 1F : 0.8F);

                var textColor = isDark
                    ? Color.FromArgb(235, 235, 235)
                    : Color.FromArgb(38, 38, 38);

                var subTextColor = isDark
                    ? Color.FromArgb(170, 176, 186)
                    : Color.FromArgb(120, 120, 120);

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, borderWidth, path);
                }

                DrawBadge(g, new Rectangle(12, 12, 44, 20), ResolveActuatorTypeColor(Item.ActuatorType), ResolveTypeText(Item.ActuatorType));
                DrawBadge(g, new Rectangle(60, 12, 70, 20), ResolveStateColor(Item.StateLevel), TrimText(Item.StateText, 8));

                g.String(
                    TrimText(Item.DisplayTitle, 14),
                    FontTitle,
                    textColor,
                    new Point(12, 40));

                g.String(
                    TrimText("内部：" + Item.Name, 22),
                    FontBody,
                    subTextColor,
                    new Point(12, 60));

                g.String(
                    TrimText(Item.CardLine1Text, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 80));

                g.String(
                    TrimText(Item.CardLine2Text, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 98));
            }

            private static void DrawBadge(Canvas g, Rectangle rect, Color backColor, string text)
            {
                using (var path = rect.RoundPath(6))
                {
                    g.Fill(backColor, path);
                }

                g.String(text, FontBadge, Color.White, rect);
            }

            private static string ResolveTypeText(string actuatorType)
            {
                switch (actuatorType)
                {
                    case "Cylinder": return "气缸";
                    case "Vacuum": return "真空";
                    case "Gripper": return "夹爪";
                    case "StackLight": return "灯塔";
                    default: return "执行器";
                }
            }

            private static Color ResolveActuatorTypeColor(string actuatorType)
            {
                switch (actuatorType)
                {
                    case "Cylinder": return Color.FromArgb(22, 119, 255);
                    case "Vacuum": return Color.FromArgb(3, 169, 244);
                    case "Gripper": return Color.FromArgb(156, 39, 176);
                    case "StackLight": return Color.FromArgb(250, 140, 22);
                    default: return Color.FromArgb(96, 125, 139);
                }
            }

            private static Color ResolveStateColor(string stateLevel)
            {
                if (string.Equals(stateLevel, "Danger", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(245, 108, 108);

                if (string.Equals(stateLevel, "Warning", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(250, 140, 22);

                if (string.Equals(stateLevel, "Success", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(82, 196, 26);

                if (string.Equals(stateLevel, "Primary", StringComparison.OrdinalIgnoreCase))
                    return Color.FromArgb(22, 119, 255);

                return Color.FromArgb(96, 125, 139);
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