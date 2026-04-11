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
    /// PLC 站状态虚拟卡片列表。
    /// 使用 AntdUI 的 `VirtualPanel` 实现高性能卡片滚动，
    /// 避免 PLC 站较多时为每个站创建独立 WinForms 控件。
    /// </summary>
    public partial class PlcStatusVirtualListControl : UserControl
    {
        public PlcStatusVirtualListControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 站点选中事件。
        /// </summary>
        public event EventHandler<PlcStationSelectedEventArgs> ItemSelected;

        /// <summary>
        /// 绑定当前站列表。
        /// </summary>
        public void BindItems(
            IList<PlcStatusPageModel.StationStatusItem> items,
            PlcStatusPageModel.StationStatusItem selectedItem)
        {
            var sourceItems = items ?? new List<PlcStatusPageModel.StationStatusItem>();
            var selectedName = selectedItem == null ? string.Empty : (selectedItem.Name ?? string.Empty);

            virtualPanelStations.EmptyText = sourceItems.Count == 0
                ? "当前筛选条件下没有 PLC 站"
                : null;

            if (CanUpdateInPlace(sourceItems))
            {
                UpdateItemsInPlace(sourceItems, selectedName);
                return;
            }

            RebuildItems(sourceItems, selectedName);
        }

        private void BindEvents()
        {
            virtualPanelStations.ItemClick += VirtualPanelStations_ItemClick;
        }

        private void VirtualPanelStations_ItemClick(object sender, VirtualItemEventArgs e)
        {
            var cardItem = e == null ? null : e.Item as PlcStationVirtualCardItem;
            if (cardItem == null || cardItem.Item == null)
            {
                return;
            }

            var handler = ItemSelected;
            if (handler != null)
            {
                handler(this, new PlcStationSelectedEventArgs(cardItem.Item.Name));
            }
        }

        private bool CanUpdateInPlace(IList<PlcStatusPageModel.StationStatusItem> items)
        {
            if (items == null)
            {
                return virtualPanelStations.Items.Count == 0;
            }

            if (virtualPanelStations.Items.Count != items.Count)
            {
                return false;
            }

            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelStations.Items[i] as PlcStationVirtualCardItem;
                if (virtualItem == null || virtualItem.Item == null)
                {
                    return false;
                }

                if (!string.Equals(virtualItem.Item.Name, items[i].Name, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateItemsInPlace(IList<PlcStatusPageModel.StationStatusItem> items, string selectedName)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var virtualItem = virtualPanelStations.Items[i] as PlcStationVirtualCardItem;
                if (virtualItem == null)
                {
                    continue;
                }

                virtualItem.Bind(
                    items[i],
                    string.Equals(items[i].Name, selectedName, StringComparison.OrdinalIgnoreCase));
            }

            virtualPanelStations.Invalidate();
        }

        private void RebuildItems(IList<PlcStatusPageModel.StationStatusItem> items, string selectedName)
        {
            virtualPanelStations.PauseLayout = true;
            try
            {
                virtualPanelStations.Items.Clear();

                if (items != null && items.Count > 0)
                {
                    var virtualItems = new List<VirtualItem>(items.Count);

                    foreach (var item in items)
                    {
                        virtualItems.Add(new PlcStationVirtualCardItem(
                            item,
                            string.Equals(item.Name, selectedName, StringComparison.OrdinalIgnoreCase)));
                    }

                    virtualPanelStations.Items.AddRange(virtualItems);
                }
            }
            finally
            {
                virtualPanelStations.PauseLayout = false;
            }
        }

        public sealed class PlcStationSelectedEventArgs : EventArgs
        {
            public PlcStationSelectedEventArgs(string plcName)
            {
                PlcName = plcName ?? string.Empty;
            }

            public string PlcName { get; private set; }
        }

        /// <summary>
        /// 单个 PLC 站状态卡片。
        /// </summary>
        private sealed class PlcStationVirtualCardItem : VirtualShadowItem
        {
            private static readonly Font FontTitle = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            private static readonly Font FontBody = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Regular);
            private static readonly Font FontBadge = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);

            private bool _selected;

            public PlcStationVirtualCardItem(PlcStatusPageModel.StationStatusItem item, bool selected)
            {
                Bind(item, selected);
                CanClick = true;
            }

            public PlcStatusPageModel.StationStatusItem Item { get; private set; }

            public void Bind(PlcStatusPageModel.StationStatusItem item, bool selected)
            {
                Item = item;
                _selected = selected;
                Tag = item;
            }

            public override Size Size(Canvas g, VirtualPanelArgs e)
            {
                var dpi = g == null ? 1F : g.Dpi;
                return new Size(
                    (int)Math.Round(172 * dpi),
                    (int)Math.Round(120 * dpi));
            }

            public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
            {
                return true;
            }

            public override void Paint(Canvas g, VirtualPanelArgs e)
            {
                if (Item == null)
                {
                    return;
                }

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

                var onlineColor = Color.FromArgb(82, 196, 26);
                var offlineColor = Color.FromArgb(245, 108, 108);
                var scanColor = Item.IsScanRunning
                    ? Color.FromArgb(22, 119, 255)
                    : Color.FromArgb(250, 173, 20);
                var disabledColor = Color.FromArgb(140, 140, 140);

                using (var path = rect.RoundPath(e.Radius))
                {
                    g.Fill(backColor, path);
                    g.Draw(borderColor, _selected ? 1.6F : 1F, path);
                }

                DrawBadge(
                    g,
                    new Rectangle(12, 12, 58, 22),
                    Item.IsConnected ? onlineColor : offlineColor,
                    Item.ConnectionStatusText);

                DrawBadge(
                    g,
                    new Rectangle(76, 12, 58, 22),
                    Item.IsEnabled ? scanColor : disabledColor,
                    Item.ScanStatusText);

                g.String(
                    TrimText(Item.DisplayTitle, 16),
                    FontTitle,
                    textColor,
                    new Point(12, 42));

                g.String(
                    TrimText(Item.Name, 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 64));

                g.String(
                    TrimText("协议：" + (string.IsNullOrWhiteSpace(Item.ProtocolType) ? "-" : Item.ProtocolType), 24),
                    FontBody,
                    subTextColor,
                    new Point(12, 84));

                g.String(
                    TrimText("连接：" + (string.IsNullOrWhiteSpace(Item.ConnectionType) ? "-" : Item.ConnectionType), 24),
                    FontBody,
                    subTextColor,
                    new Point(126, 84));

                g.String(
                    TrimText("平均读取：" + Item.AverageReadMsText, 22),
                    FontBody,
                    subTextColor,
                    new Point(12, 102));

                if (Item.HasError)
                {
                    g.String(
                        TrimText("异常：" + Item.ErrorBriefText, 28),
                        FontBody,
                        offlineColor,
                        new Point(126, 102));
                }
                else
                {
                    g.String(
                        TrimText("更新：" + Item.LastScanTimeText, 22),
                        FontBody,
                        subTextColor,
                        new Point(126, 102));
                }
            }

            private static void DrawBadge(Canvas g, Rectangle rect, Color backColor, string text)
            {
                using (var path = rect.RoundPath(6))
                {
                    g.Fill(backColor, path);
                }

                g.String(
                    text,
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