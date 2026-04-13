using AM.PageModel.Plc;
using AntdUI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Plc
{
    /// <summary>
    /// PLC 点位详情控件。
    /// 仅负责显示当前选中点位的详细信息，不承担查询和筛选逻辑。
    /// </summary>
    public partial class PlcPointDetailControl : UserControl
    {
        private string _lastPointName = string.Empty;
        private string _lastSnapshotKey = string.Empty;

        public PlcPointDetailControl()
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

        public void Bind(PlcMonitorPageModel.PointMonitorItem item)
        {
            if (item == null)
            {
                _lastPointName = string.Empty;
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

            string snapshotKey = BuildSnapshotKey(item);
            if (string.Equals(_lastPointName, item.PointName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
            {
                return;
            }

            _lastPointName = item.PointName ?? string.Empty;
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
                labelSubTitle.Text = string.IsNullOrWhiteSpace(item.PointName) ? "—" : item.PointName;

                SetTagRow(labelTagPlcKey, labelTagPlcValue, "PLC", item.PlcName);
                SetTagRow(labelTagGroupKey, labelTagGroupValue, "分组", item.GroupName);
                SetTagRow(labelTagAddressKey, labelTagAddressValue, "地址", item.AddressText);
                SetTagRow(labelTagDataTypeKey, labelTagDataTypeValue, "数据类型", item.DataType);
                SetTagRow(labelTagValueKey, labelTagValueValue, "当前值", item.ValueText);
                SetTagRow(labelTagRawValueKey, labelTagRawValueValue, "原始值", item.RawValue);
                SetTagRow(labelTagQualityKey, labelTagQualityValue, "质量", item.QualityText);
                SetTagRow(labelTagConnectionKey, labelTagConnectionValue, "连接状态", item.IsConnected ? "在线" : "离线");
                SetTagRow(labelTagUpdateTimeKey, labelTagUpdateTimeValue, "更新时间", item.UpdateTimeText);

                labelErrorValue.Text = string.IsNullOrWhiteSpace(item.ErrorMessage) ? "—" : item.ErrorMessage;
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

        private static string BuildSnapshotKey(PlcMonitorPageModel.PointMonitorItem item)
        {
            return string.Join("|", new[]
            {
                item.DisplayTitle ?? string.Empty,
                item.PointName ?? string.Empty,
                item.PlcName ?? string.Empty,
                item.GroupName ?? string.Empty,
                item.AddressText ?? string.Empty,
                item.DataType ?? string.Empty,
                item.ValueText ?? string.Empty,
                item.RawValue ?? string.Empty,
                item.QualityText ?? string.Empty,
                item.IsConnected ? "1" : "0",
                item.UpdateTimeText ?? string.Empty,
                item.ErrorMessage ?? string.Empty
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