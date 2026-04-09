using AM.PageModel.Motion;
using AM.PageModel.Motion.Axis;
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
        public void BindItems(IList<MotionAxisActionViewItem> items)
        {
            var sourceItems = items ?? new List<MotionAxisActionViewItem>();

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

        private bool CanUpdateInPlace(IList<MotionAxisActionViewItem> items)
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

        private void UpdateItemsInPlace(IList<MotionAxisActionViewItem> items)
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

        private void RebuildItems(IList<MotionAxisActionViewItem> items)
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
        ///
        /// 交互语义说明：
        /// 1. 这类卡片是“点击即执行”，不是“点击后保持选中”；
        /// 2. 因此默认态不显示明显边框，只保留阴影层次；
        /// 3. 鼠标移入时显示轻微边框，并利用 VirtualShadowItem 的 Hover 阴影动画增强悬停感；
        /// 4. 鼠标点击成功时，短暂显示强调边框，作为“点击成功已触发”的即时反馈；
        /// 5. 点击反馈结束后边框自动恢复消失，不保留选中态。
        /// </summary>
        private sealed class MotionAxisActionVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            /// <summary>
            /// 点击反馈是否处于显示中。
            /// true 表示当前应绘制强调边框。
            /// </summary>
            private bool _clickFeedbackVisible;

            /// <summary>
            /// 用于控制点击反馈的短暂显示时长。
            /// 点击后启动，超时自动关闭强调边框。
            /// </summary>
            private Timer _clickFeedbackTimer;

            public MotionAxisActionVirtualCardItem(MotionAxisActionViewItem item)
            {
                Bind(item);
            }

            public MotionAxisActionViewItem Item { get; private set; }

            /// <summary>
            /// 绑定动作项，并同步是否允许点击。
            /// </summary>
            public void Bind(MotionAxisActionViewItem item)
            {
                Item = item;
                Tag = item;
                CanClick = item != null && item.CanExecute;
            }

            /// <summary>
            /// 定义卡片尺寸。
            /// </summary>
            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(152 * dpi),
                    (int)Math.Round(82 * dpi));
            }

            /// <summary>
            /// 鼠标移动时返回 true，让 VirtualPanel 维持 Hover 感知。
            /// 配合 VirtualShadowItem，可触发阴影悬停动画。
            /// </summary>
            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            /// <summary>
            /// 鼠标点击时触发一次短暂的强调边框反馈。
            /// 这里不是“选中态”，只是“点击已触发”的瞬时反馈。
            /// </summary>
            public override void MouseClick(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                base.MouseClick(sender, e);

                if (Item == null || !Item.CanExecute)
                    return;

                StartClickFeedback();
            }

            /// <summary>
            /// 释放虚拟项资源，避免定时器残留。
            /// </summary>
            public override void Dispose(VirtualPanel sender, bool disposed)
            {
                if (_clickFeedbackTimer != null)
                {
                    _clickFeedbackTimer.Stop();
                    _clickFeedbackTimer.Dispose();
                    _clickFeedbackTimer = null;
                }

                base.Dispose(sender, disposed);
            }

            /// <summary>
            /// 绘制单张动作卡片。
            ///
            /// 边框策略：
            /// - 默认态：无明显边框；
            /// - Hover：轻边框；
            /// - Click：强调边框；
            ///
            /// 阴影策略：
            /// - 默认态依赖 VirtualPanel 配置的阴影；
            /// - Hover 时由 VirtualShadowItem 自带动画增强阴影反馈。
            /// </summary>
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

                var textColor = enabled
                    ? (isDark ? Color.FromArgb(235, 235, 235) : Color.FromArgb(38, 38, 38))
                    : (isDark ? Color.FromArgb(150, 150, 150) : Color.FromArgb(160, 160, 160));

                var badgeColor = enabled
                    ? ResolveAccentColor(Item.AccentType)
                    : Color.FromArgb(160, 160, 160);

                // 默认态：不显示明显边框。
                // Hover：显示轻微边框。
                // Click：显示强调边框。
                var showClickBorder = enabled && _clickFeedbackVisible;
                var showHoverBorder = !showClickBorder && Hover;

                var borderColor = isDark 
                    ? Color.FromArgb(64, 72, 84) 
                    : Color.FromArgb(236, 239, 244);
                var borderWidth = 1.0F;

                if (showClickBorder)
                {
                    borderColor = ResolveAccentColor(Item.AccentType);
                    borderWidth = 2.0F;
                }
                else if (showHoverBorder)
                {
                    borderColor = isDark
                        ? Color.FromArgb(92, 100, 112)
                        : Color.FromArgb(225, 229, 235);
                    borderWidth = 1.5F;
                }

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);

                    if (borderWidth > 0F)
                        g.Draw(borderColor, borderWidth, path);
                }

                // 左上角分类徽标。
                DrawBadge(g, new Rectangle(10, 10, 52, 20), badgeColor, TrimText(Item.CategoryText, 6));

                // 中间动作主标题。
                g.String(
                    TrimText(Item.DisplayText, 8),
                    FontTitle,
                    textColor,
                    new Rectangle(12, 30, rect.Width - 24, 34));
            }

            /// <summary>
            /// 启动点击反馈。
            /// 点击后短暂显示强调边框，然后自动恢复默认态。
            /// </summary>
            private void StartClickFeedback()
            {
                _clickFeedbackVisible = true;
                Invalidate();

                if (_clickFeedbackTimer == null)
                {
                    _clickFeedbackTimer = new Timer();
                    _clickFeedbackTimer.Interval = 140;
                    _clickFeedbackTimer.Tick += ClickFeedbackTimer_Tick;
                }

                _clickFeedbackTimer.Stop();
                _clickFeedbackTimer.Start();
            }

            /// <summary>
            /// 点击反馈结束，恢复默认显示。
            /// </summary>
            private void ClickFeedbackTimer_Tick(object sender, EventArgs e)
            {
                if (_clickFeedbackTimer != null)
                    _clickFeedbackTimer.Stop();

                _clickFeedbackVisible = false;
                Invalidate();
            }

            /// <summary>
            /// 根据动作强调类型映射颜色。
            /// </summary>
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

            /// <summary>
            /// 绘制左上角分类徽标。
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
            /// 文本截断，避免卡片内容溢出。
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