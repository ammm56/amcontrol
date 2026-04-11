using AM.PageModel.Plc;
using AntdUI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Plc
{
    /// <summary>
    /// PLC 站状态详情控件。
    /// 负责显示当前选中 PLC 站的详细运行信息。
    /// </summary>
    public partial class PlcStatusDetailControl : UserControl
    {
        private string _lastSnapshotKey = string.Empty;
        private string _lastStationName = string.Empty;

        public PlcStatusDetailControl()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            UpdateStyles();

            EnableDoubleBuffer(panelScroll);

            panelDetail.Visible = false;
            panelEmpty.Visible = true;
        }

        public void Bind(PlcStatusPageModel.StationStatusItem item)
        {
            if (item == null)
            {
                _lastStationName = string.Empty;
                _lastSnapshotKey = string.Empty;

                if (!panelEmpty.Visible)
                {
                    SuspendLayout();
                    panelDetail.SuspendLayout();
                    try
                    {
                        panelDetail.Visible = false;
                        panelEmpty.Visible = true;
                    }
                    finally
                    {
                        panelDetail.ResumeLayout(false);
                        ResumeLayout(false);
                    }
                }

                return;
            }

            var snapshotKey = BuildSnapshotKey(item);
            if (string.Equals(_lastStationName, item.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
            {
                return;
            }

            _lastStationName = item.Name ?? string.Empty;
            _lastSnapshotKey = snapshotKey;

            SuspendLayout();
            panelDetail.SuspendLayout();
            panelHeader.SuspendLayout();
            panelScroll.SuspendLayout();
            try
            {
                if (!panelDetail.Visible)
                {
                    panelEmpty.Visible = false;
                    panelDetail.Visible = true;
                }

                labelTitle.Text = item.DisplayTitle;
                labelSubTitle.Text = string.IsNullOrWhiteSpace(item.Name) ? "—" : item.Name;

                SetTagRow(labelTagProtocolKey, labelTagProtocolValue, "协议", item.ProtocolType);
                SetTagRow(labelTagConnectionKey, labelTagConnectionValue, "连接", item.ConnectionType);
                SetTagRow(labelTagEndpointKey, labelTagEndpointValue, "端点", item.EndpointText);
                SetTagRow(labelTagConfigKey, labelTagConfigValue, "配置", item.ConfigStatusText);
                SetTagRow(labelTagRuntimeKey, labelTagRuntimeValue, "通讯", item.ConnectionStatusText);
                SetTagRow(labelTagScanKey, labelTagScanValue, "扫描", item.ScanStatusText);
                SetTagRow(labelTagLastScanKey, labelTagLastScanValue, "扫描完成时间", item.LastScanTimeText);
                SetTagRow(labelTagLastConnectKey, labelTagLastConnectValue, "连接时间", item.LastConnectTimeText);
                SetTagRow(labelTagAverageReadKey, labelTagAverageReadValue, "整轮读取均值", item.AverageReadMsText);
                SetTagRow(labelTagAverageWriteKey, labelTagAverageWriteValue, "平均写入", item.AverageWriteMsText);
                SetTagRow(labelTagSuccessKey, labelTagSuccessValue, "成功轮次", item.SuccessReadCountText);
                SetTagRow(labelTagFailedKey, labelTagFailedValue, "失败轮次", item.FailedReadCountText);

                labelErrorValue.Text = item.LastErrorText;
            }
            finally
            {
                panelScroll.ResumeLayout(false);
                panelHeader.ResumeLayout(false);
                panelDetail.ResumeLayout(false);
                ResumeLayout(false);
                Invalidate();
            }
        }

        private static void SetTagRow(
            AntdUI.Label keyLabel,
            AntdUI.Label valueLabel,
            string keyText,
            string valueText)
        {
            keyLabel.Text = string.IsNullOrWhiteSpace(keyText) ? "-" : keyText;
            valueLabel.Text = string.IsNullOrWhiteSpace(valueText) ? "—" : valueText;
        }

        private static string BuildSnapshotKey(PlcStatusPageModel.StationStatusItem item)
        {
            return string.Join("|", new[]
            {
                item.DisplayTitle ?? string.Empty,
                item.Name ?? string.Empty,
                item.ProtocolType ?? string.Empty,
                item.ConnectionType ?? string.Empty,
                item.EndpointText ?? string.Empty,
                item.ConfigStatusText ?? string.Empty,
                item.ConnectionStatusText ?? string.Empty,
                item.ScanStatusText ?? string.Empty,
                item.LastScanTimeText ?? string.Empty,
                item.LastConnectTimeText ?? string.Empty,
                item.AverageReadMsText ?? string.Empty,
                item.AverageWriteMsText ?? string.Empty,
                item.SuccessReadCountText ?? string.Empty,
                item.FailedReadCountText ?? string.Empty,
                item.LastErrorText ?? string.Empty
            });
        }

        private static void EnableDoubleBuffer(Control control)
        {
            if (control == null)
            {
                return;
            }

            var property = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (property != null && property.CanWrite)
            {
                property.SetValue(control, true, null);
            }
        }
    }
}