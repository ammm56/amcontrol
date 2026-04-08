using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制左侧简单动作虚拟卡片列表。
    ///
    /// 职责说明：
    /// 1. 只承载简单动作卡片；
    /// 2. 不承载输入框、确认按钮等复杂参数布局；
    /// 3. 继续使用 VirtualPanel，保证滚动流畅和资源占用低；
    /// 4. 参数动作（应用速度 / 绝对定位 / 相对移动）由独立 UserControl 实现。
    /// </summary>
    public partial class MotionAxisVirtualListControl : UserControl
    {
        public MotionAxisVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 简单动作执行请求事件。
        /// </summary>
        public event EventHandler<MotionAxisActionExecuteRequestedEventArgs> ActionExecuteRequested;

        /// <summary>
        /// 绑定当前要显示的简单动作卡片集合。
        /// </summary>
        public void BindItems(IList<MotionAxisPageModel.MotionAxisActionViewItem> items)
        {
            var sourceItems = items ?? new List<MotionAxisPageModel.MotionAxisActionViewItem>();

            virtualPanelActions.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有动作卡片"
                : null;

            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems);
                return;
            }

            RebuildItems(sourceItems);
        }

        private void BindEvents()
        {
            virtualPanelActions.ItemClick += VirtualPanelActions_ItemClick;
        }

        /// <summary>
        /// 简单动作卡片单击即执行。
        /// </summary>
        private void VirtualPanelActions_ItemClick(object sender, VirtualItemEventArgs e)
        {
            var cardItem = e == null ? null : e.Item as MotionAxisActionVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
                return;

            if (!cardItem.Item.CanExecute)
                return;

            var handler = ActionExecuteRequested;
            if (handler != null)
                handler(this, new MotionAxisActionExecuteRequestedEventArgs(cardItem.Item.ActionKey));
        }

        private bool CanUpdateInPlace(IList<MotionAxisPageModel.MotionAxisActionViewItem> items)
        {
            if (items == null)
                return virtualPanelActions.Items.Count == 0;

            if (virtualPanelActions.Items.Count != items.Count)
                return false;

            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelActions.Items[i] as MotionAxisActionVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                    return false;

                if (!string.Equals(virtualItem.Item.ActionKey, items[i].ActionKey, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private void UpdateItemsInPlace(IList<MotionAxisPageModel.MotionAxisActionViewItem> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelActions.Items[i] as MotionAxisActionVirtualCardItem;
                if (virtualItem == null)
                    continue;

                virtualItem.Bind(items[i]);
            }

            virtualPanelActions.Invalidate();
        }

        private void RebuildItems(IList<MotionAxisPageModel.MotionAxisActionViewItem> items)
        {
            virtualPanelActions.PauseLayout = true;
            try
            {
                virtualPanelActions.Items.Clear();

                if (items != null && items.Count > 0)
                {
                    var virtualItems = new List<VirtualItem>(items.Count);
                    foreach (var item in items)
                    {
                        virtualItems.Add(new MotionAxisActionVirtualCardItem(item));
                    }

                    virtualPanelActions.Items.AddRange(virtualItems);
                }
            }
            finally
            {
                virtualPanelActions.PauseLayout = false;
            }
        }

        /// <summary>
        /// 简单动作执行请求事件参数。
        /// </summary>
        public sealed class MotionAxisActionExecuteRequestedEventArgs : EventArgs
        {
            public MotionAxisActionExecuteRequestedEventArgs(string actionKey)
            {
                ActionKey = actionKey;
            }

            public string ActionKey { get; private set; }
        }

        /// <summary>
        /// VirtualPanel 内部的按钮式动作小卡片。
        /// 卡片只显示左上角分类与中间名称。
        /// </summary>
        private sealed class MotionAxisActionVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            public MotionAxisActionVirtualCardItem(MotionAxisPageModel.MotionAxisActionViewItem item)
            {
                Bind(item);
            }

            public MotionAxisPageModel.MotionAxisActionViewItem Item { get; private set; }

            public void Bind(MotionAxisPageModel.MotionAxisActionViewItem item)
            {
                Item = item;
                Tag = item;
                CanClick = item != null && item.CanExecute;
            }

            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(152 * dpi),
                    (int)Math.Round(82 * dpi));
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

                var enabled = Item.CanExecute;
                var backColor = enabled
                    ? (isDark ? Color.FromArgb(39, 44, 52) : Color.White)
                    : (isDark ? Color.FromArgb(49, 53, 60) : Color.FromArgb(245, 245, 245));

                var borderColor = enabled
                    ? ResolveAccentColor(Item.AccentType)
                    : (isDark ? Color.FromArgb(88, 92, 100) : Color.FromArgb(225, 229, 235));

                var textColor = enabled
                    ? (isDark ? Color.FromArgb(235, 235, 235) : Color.FromArgb(38, 38, 38))
                    : (isDark ? Color.FromArgb(150, 150, 150) : Color.FromArgb(160, 160, 160));

                var badgeColor = enabled
                    ? ResolveAccentColor(Item.AccentType)
                    : Color.FromArgb(160, 160, 160);

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, enabled ? 1.2F : 1F, path);
                }

                DrawBadge(g, new Rectangle(10, 10, 52, 20), badgeColor, TrimText(Item.CategoryText, 6));

                g.String(
                    TrimText(Item.DisplayText, 8),
                    FontTitle,
                    textColor,
                    new Rectangle(12, 30, rect.Width - 24, 34));
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