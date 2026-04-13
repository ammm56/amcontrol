using AM.PageModel.Plc;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Plc
{
    /// <summary>
    /// PLC 点位虚拟卡片列表。
    ///
    /// 设计说明：
    /// 1. 点位数量可能较多，不适合为每个点位创建独立 WinForms 控件；
    /// 2. 使用 AntdUI 的 `VirtualPanel` 承载虚拟项，只绘制可见卡片；
    /// 3. 普通刷新优先做原位更新，避免刷新时滚动位置回到顶部；
    /// 4. 页面层只关心“当前选中了哪个点位”，不需要感知内部绘制细节。
    /// </summary>
    public partial class PlcPointVirtualListControl : UserControl
    {
        public PlcPointVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 点位选中事件。
        /// 参数中只传 PointName，页面层据此更新选中项。
        /// </summary>
        public event EventHandler<PlcPointSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前点位列表。
        ///
        /// 刷新策略：
        /// 1. 若点位数量和顺序未变，则只更新卡片内容，保留滚动位置；
        /// 2. 若筛选条件变化、顺序变化、首次加载，则重建整个虚拟列表。
        /// </summary>
        public void BindItems(
            IList<PlcMonitorPageModel.PointMonitorItem> items,
            PlcMonitorPageModel.PointMonitorItem selectedItem)
        {
            IList<PlcMonitorPageModel.PointMonitorItem> sourceItems =
                items ?? new List<PlcMonitorPageModel.PointMonitorItem>();

            string selectedPointName = selectedItem == null
                ? string.Empty
                : (selectedItem.PointName ?? string.Empty);

            virtualPanelPoints.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有 PLC 点位"
                : null;

            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedPointName);
                return;
            }

            RebuildItems(sourceItems, selectedPointName);
        }

        /// <summary>
        /// 绑定内部事件。
        /// </summary>
        private void BindEvents()
        {
            virtualPanelPoints.ItemClick += VirtualPanelPoints_ItemClick;
        }

        /// <summary>
        /// 卡片点击后，将内部虚拟项转换成页面可直接使用的点位名称事件。
        /// </summary>
        private void VirtualPanelPoints_ItemClick(object sender, VirtualItemEventArgs e)
        {
            PlcPointVirtualCardItem cardItem = e == null ? null : e.Item as PlcPointVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
            {
                return;
            }

            EventHandler<PlcPointSelectedEventArgs> handler = ItemSelected;
            if (handler != null)
            {
                handler(this, new PlcPointSelectedEventArgs(cardItem.Item.PointName));
            }
        }

        /// <summary>
        /// 判断是否可以原位更新。
        /// 只比较数量和当前顺序上的 PointName。
        /// </summary>
        private bool CanUpdateInPlace(IList<PlcMonitorPageModel.PointMonitorItem> items)
        {
            if (items == null)
            {
                return virtualPanelPoints.Items.Count == 0;
            }

            if (virtualPanelPoints.Items.Count != items.Count)
            {
                return false;
            }

            for (int i = 0; i < items.Count; i++)
            {
                PlcPointVirtualCardItem virtualItem = virtualPanelPoints.Items[i] as PlcPointVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                {
                    return false;
                }

                if (!string.Equals(
                    virtualItem.Item.PointName,
                    items[i].PointName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 原位更新卡片数据。
        /// </summary>
        private void UpdateItemsInPlace(IList<PlcMonitorPageModel.PointMonitorItem> items, string selectedPointName)
        {
            for (int i = 0; i < items.Count; i++)
            {
                PlcPointVirtualCardItem virtualItem = virtualPanelPoints.Items[i] as PlcPointVirtualCardItem;
                if (virtualItem == null)
                {
                    continue;
                }

                virtualItem.Bind(
                    items[i],
                    string.Equals(items[i].PointName, selectedPointName, StringComparison.OrdinalIgnoreCase));
            }

            virtualPanelPoints.Invalidate();
        }

        /// <summary>
        /// 重建虚拟卡片列表。
        /// </summary>
        private void RebuildItems(IList<PlcMonitorPageModel.PointMonitorItem> items, string selectedPointName)
        {
            virtualPanelPoints.PauseLayout = true;
            try
            {
                virtualPanelPoints.Items.Clear();

                if (items != null && items.Count > 0)
                {
                    List<VirtualItem> virtualItems = new List<VirtualItem>(items.Count);

                    foreach (PlcMonitorPageModel.PointMonitorItem item in items)
                    {
                        virtualItems.Add(new PlcPointVirtualCardItem(
                            item,
                            string.Equals(item.PointName, selectedPointName, StringComparison.OrdinalIgnoreCase)));
                    }

                    virtualPanelPoints.Items.AddRange(virtualItems.ToArray());
                }
            }
            finally
            {
                virtualPanelPoints.PauseLayout = false;
            }
        }

        /// <summary>
        /// 点位选中事件参数。
        /// </summary>
        public sealed class PlcPointSelectedEventArgs : EventArgs
        {
            public PlcPointSelectedEventArgs(string pointName)
            {
                PointName = pointName ?? string.Empty;
            }

            public string PointName { get; private set; }
        }

        /// <summary>
        /// 单个 PLC 点位卡片。
        /// </summary>
        private sealed class PlcPointVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public PlcPointVirtualCardItem(PlcMonitorPageModel.PointMonitorItem item, bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public PlcMonitorPageModel.PointMonitorItem Item { get; private set; }

            public void Bind(PlcMonitorPageModel.PointMonitorItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            /// <summary>
            /// 卡片尺寸与 `PlcStatusVirtualListControl` 对齐。
            /// </summary>
            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                float dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(172 * dpi),
                    (int)Math.Round(140 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            /// <summary>
            /// 绘制点位卡片。
            ///
            /// 布局节奏对齐 `PlcStatusVirtualListControl`：
            /// 1. 顶部两枚状态标签；
            /// 2. 中间三行紧凑信息；
            /// 3. 底部一行显示错误或更新时间；
            /// 4. 卡片高度、留白、文字密度尽量保持一致。
            /// </summary>
            public override void Paint(Canvas g, VirtualPanelArgs e)
            {
                if (Item == null)
                {
                    return;
                }

                bool isDark = AntdUI.Config.IsDark;
                Rectangle rect = new Rectangle(0, 0, e.Rect.Width, e.Rect.Height);

                Color backColor = _selected
                    ? (isDark ? Color.FromArgb(29, 58, 97) : Color.FromArgb(237, 246, 255))
                    : (isDark ? Color.FromArgb(39, 44, 52) : Color.FromArgb(255, 255, 255));

                Color borderColor = _selected
                    ? Color.FromArgb(22, 119, 255)
                    : (isDark ? Color.FromArgb(72, 79, 92) : Color.FromArgb(225, 229, 235));

                Color textColor = isDark
                    ? Color.FromArgb(235, 235, 235)
                    : Color.FromArgb(38, 38, 38);

                Color subTextColor = isDark
                    ? Color.FromArgb(170, 176, 186)
                    : Color.FromArgb(120, 120, 120);

                Color readableColor = Color.FromArgb(82, 196, 26);
                Color unreadableColor = Color.FromArgb(250, 173, 20);
                Color errorColor = Color.FromArgb(245, 108, 108);
                Color readOnlyColor = Color.FromArgb(140, 140, 140);
                Color readWriteColor = Color.FromArgb(22, 119, 255);
                Color writeOnlyColor = Color.FromArgb(250, 173, 20);

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, _selected ? 1.6F : 1F, path);
                }

                DrawBadge(
                    g,
                    new Rectangle(12, 12, 58, 22),
                    ResolveStateColor(Item, readableColor, unreadableColor, errorColor),
                    ResolveStateText(Item));

                DrawBadge(
                    g,
                    new Rectangle(76, 12, 58, 22),
                    ResolveAccessModeColor(Item, readOnlyColor, readWriteColor, writeOnlyColor),
                    TrimText(Item.AccessModeText, 4));

                g.String(
                    TrimText(Item.DisplayTitle, 16),
                    FontTitle,
                    textColor,
                    new Point(12, 42));

                g.String(
                    TrimText(string.IsNullOrWhiteSpace(Item.AddressText) ? "-" : Item.AddressText, 22),
                    FontBody,
                    subTextColor,
                    new Point(12, 64));

                g.String(
                    TrimText(string.IsNullOrWhiteSpace(Item.DataType) ? "-" : Item.DataType, 12),
                    FontBody,
                    subTextColor,
                    new Point(106, 64));

                g.String(
                    TrimText(Item.ValueBriefText, 12),
                    FontBody,
                    subTextColor,
                    new Point(12, 84));

                if (Item.HasError)
                {
                    g.String(
                        "异常：" + Item.ErrorBriefText,
                        FontBody,
                        errorColor,
                        new Rectangle(12, 102, rect.Width - 24, 18),
                        FormatFlags.Left | FormatFlags.Top | FormatFlags.NoWrapEllipsis);
                }
                else
                {
                    g.String(
                        TrimText(Item.UpdateTimeText, 22),
                        FontBody,
                        subTextColor,
                        new Point(12, 102));
                }
            }

            private static string ResolveStateText(PlcMonitorPageModel.PointMonitorItem item)
            {
                if (item == null)
                {
                    return "不可读";
                }

                if (item.HasError)
                {
                    return "异常";
                }

                return item.IsConnected ? "可读" : "不可读";
            }

            private static Color ResolveStateColor(
                PlcMonitorPageModel.PointMonitorItem item,
                Color readableColor,
                Color unreadableColor,
                Color errorColor)
            {
                if (item == null)
                {
                    return unreadableColor;
                }

                if (item.HasError)
                {
                    return errorColor;
                }

                return item.IsConnected ? readableColor : unreadableColor;
            }

            private static Color ResolveAccessModeColor(
                PlcMonitorPageModel.PointMonitorItem item,
                Color readOnlyColor,
                Color readWriteColor,
                Color writeOnlyColor)
            {
                if (item == null)
                {
                    return readOnlyColor;
                }

                if (string.Equals(item.AccessMode, "ReadWrite", StringComparison.OrdinalIgnoreCase))
                {
                    return readWriteColor;
                }

                if (string.Equals(item.AccessMode, "WriteOnly", StringComparison.OrdinalIgnoreCase))
                {
                    return writeOnlyColor;
                }

                return readOnlyColor;
            }

            private static void DrawBadge(Canvas g, Rectangle rect, Color backColor, string text)
            {
                using (var path = rect.RoundPath(6))
                {
                    g.Fill(backColor, path);
                }

                g.String(
                    string.IsNullOrWhiteSpace(text) ? "-" : text,
                    FontBadge,
                    Color.White,
                    rect);
            }

            private static string TrimText(string text, int maxLength)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return "-";
                }

                if (text.Length <= maxLength)
                {
                    return text;
                }

                return text.Substring(0, maxLength - 1) + "…";
            }
        }
    }
}