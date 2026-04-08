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
    /// 1. 左侧动作区域统一使用 VirtualPanel 虚拟绘制，避免为每张小卡片创建真实控件；
    /// 2. 卡片视觉保持 AntdUI 风格统一，但交互语义按“按钮”处理；
    /// 3. 卡片数量增多时仍能保持滚动流畅和较低资源占用；
    /// 4. 刷新时优先原地更新，避免整批重建造成滚动位置抖动。
    /// </summary>
    public partial class MotionAxisVirtualListControl : UserControl
    {
        public MotionAxisVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 动作执行请求事件。
        /// 左侧卡片单击即执行，因此这里直接把 ActionKey 抛给页面层。
        /// </summary>
        public event EventHandler<MotionAxisActionExecuteRequestedEventArgs> ActionExecuteRequested;

        /// <summary>
        /// 绑定当前要显示的动作卡片集合。
        /// 当前动作集合数量固定不大，但仍统一使用虚拟绘制方案，保持和 DI/DO 页面结构一致。
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
        /// 卡片单击即执行。
        /// 不可执行卡片直接忽略点击，只保留禁用视觉反馈。
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
        /// 动作执行请求事件参数。
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
        /// 卡片只显示左上角分类与中间名称，不额外显示描述和右上角状态。
        /// 是否可执行完全由背景、边框和文字透明度表达。
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
