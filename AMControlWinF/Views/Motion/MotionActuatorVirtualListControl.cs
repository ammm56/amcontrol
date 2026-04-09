using AM.PageModel.Motion.Actuator;
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
    /// 【层级定位】
    /// - 所在层：WinForms 显示控件层；
    /// - 上游来源：MotionActuatorPage / MotionActuatorPageModel；
    /// - 下游输出：卡片点击选择事件。
    ///
    /// 【职责】
    /// 1. 显示左侧执行器卡片集合；
    /// 2. 使用 VirtualPanel + VirtualItem 保持大列表滚动性能；
    /// 3. 仅负责“显示”和“选择”，不承担动作执行；
    /// 4. 不再依赖页面原始快照，而只依赖专门的列表显示对象 MotionActuatorListItem。
    ///
    /// 【本轮重构意义】
    /// 旧实现直接依赖 MotionActuatorViewItem，一个对象同时承担：
    /// - 原始状态
    /// - 列表显示
    /// - 详情显示
    /// - 动作面板输入
    ///
    /// 第一轮适配后：
    /// - 本控件只依赖 MotionActuatorListItem；
    /// - 页面层通过 ItemKey 保持选中；
    /// - 左侧列表与右侧详情/动作区的数据对象开始解耦。
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
        /// 页面层收到后根据 ItemKey 切换当前选中对象。
        /// </summary>
        public event EventHandler<MotionActuatorItemSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前显示的执行器卡片集合。
        ///
        /// 说明：
        /// 1. 入参 items 已经是列表专用显示对象；
        /// 2. selectedKey 由页面层传入，不再直接耦合 SelectedSnapshot；
        /// 3. 当卡片结构未变化时，原地更新，避免滚动位置回到顶部。
        /// </summary>
        public void BindItems(
            IList<MotionActuatorListItem> items,
            string selectedKey)
        {
            var sourceItems = items ?? new List<MotionActuatorListItem>();

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

        /// <summary>
        /// 绑定内部事件。
        /// </summary>
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

        /// <summary>
        /// 判断当前虚拟项集合是否可以原地更新。
        ///
        /// 原地更新条件：
        /// - 数量一致；
        /// - 每个位置的 ItemKey 一致；
        /// 这样就可以只刷新内容与选中状态，而不重建 VirtualItem，
        /// 从而尽量保持滚动位置稳定。
        /// </summary>
        private bool CanUpdateInPlace(IList<MotionActuatorListItem> items)
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

        /// <summary>
        /// 原地更新虚拟项内容。
        /// </summary>
        private void UpdateItemsInPlace(
            IList<MotionActuatorListItem> items,
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

        /// <summary>
        /// 重建整个虚拟列表。
        ///
        /// 仅在结构变化时使用：
        /// - 数量变化；
        /// - 顺序变化；
        /// - ItemKey 变化。
        /// </summary>
        private void RebuildItems(
            IList<MotionActuatorListItem> items,
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

        /// <summary>
        /// 列表项选中事件参数。
        /// 页面层只通过 ItemKey 与页面模型交互，避免控件直接持有页面原始状态对象。
        /// </summary>
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
        ///
        /// 【职责】
        /// 1. 只承载 MotionActuatorListItem；
        /// 2. 只绘制列表展示所需字段；
        /// 3. 不依赖详情区和动作区字段。
        /// </summary>
        private sealed class MotionActuatorVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public MotionActuatorVirtualCardItem(
                MotionActuatorListItem item,
                bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            /// <summary>
            /// 当前卡片绑定的列表显示对象。
            /// </summary>
            public MotionActuatorListItem Item { get; private set; }

            public void Bind(MotionActuatorListItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            /// <summary>
            /// 返回每个卡片的固定绘制大小。
            /// 当前使用紧凑卡片风格，与 DI/DO/多轴总览左侧卡片风格保持一致。
            /// </summary>
            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(170 * dpi),
                    (int)Math.Round(135 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            /// <summary>
            /// 绘制单张执行器卡片。
            ///
            /// 显示内容：
            /// - 左上角类型标签
            /// - 状态标签
            /// - 标题
            /// - 内部名称
            /// - 两行摘要
            /// </summary>
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

                DrawBadge(
                    g,
                    new Rectangle(12, 12, 44, 20),
                    ResolveActuatorTypeColor(Item.TypeText),
                    TrimText(Item.TypeText, 4));

                DrawBadge(
                    g,
                    new Rectangle(60, 12, 70, 20),
                    ResolveStateColor(Item.StateLevel),
                    TrimText(Item.StateText, 8));

                g.String(
                    TrimText(Item.TitleText, 14),
                    FontTitle,
                    textColor,
                    new Point(12, 40));

                g.String(
                    TrimText("内部：" + Item.NameText, 22),
                    FontBody,
                    subTextColor,
                    new Point(12, 60));

                g.String(
                    TrimText(Item.Line1Text, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 80));

                g.String(
                    TrimText(Item.Line2Text, 24),
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

            /// <summary>
            /// 根据类型文本解析对应标签色。
            /// 第一轮适配中直接使用 TypeText，避免控件依赖原始快照中的 ActuatorType。
            /// </summary>
            private static Color ResolveActuatorTypeColor(string typeText)
            {
                switch (typeText)
                {
                    case "气缸":
                        return Color.FromArgb(22, 119, 255);

                    case "真空":
                        return Color.FromArgb(3, 169, 244);

                    case "夹爪":
                        return Color.FromArgb(156, 39, 176);

                    case "灯塔":
                        return Color.FromArgb(250, 140, 22);

                    default:
                        return Color.FromArgb(96, 125, 139);
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