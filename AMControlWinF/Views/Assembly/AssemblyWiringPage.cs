using AM.Model.Common;
using AM.PageModel.Assembly;
using AMControlWinF.Views.MotionConfig;
using AntdUI;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Assembly
{
    /// <summary>
    /// 装配接线与手动检查页面。
    /// 面向装配电工，只保留接线对照、单点 IO 检查和导出功能。
    /// </summary>
    public partial class AssemblyWiringPage : UserControl
    {
        private readonly AssemblyWiringPageModel _model;

        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isBindingView;
        private string _lastActionMessage;
        private AntList<WiringTableRow> _tableRows;

        public AssemblyWiringPage()
        {
            InitializeComponent();

            _model = new AssemblyWiringPageModel();
            _tableRows = new AntList<WiringTableRow>();
            _lastActionMessage = string.Empty;

            InitializeTableColumns();
            BindEvents();
            UpdateFilterButtons();
            RefreshActionButtons();
            RefreshDebugPanel();
        }

        private void InitializeTableColumns()
        {
            tableWiring.Columns = new ColumnCollection()
            {
                new Column("IoTypeTag", "类型", ColumnAlign.Center)
                {
                    Width = "72",
                    Fixed = true
                },
                new Column("LogicalBitText", "逻辑IO号", ColumnAlign.Center)
                {
                    Width = "65",
                    Fixed = true
                },
                new Column("HardwareBitText", "硬件位号", ColumnAlign.Center)
                {
                    Width = "65"
                },
                new Column("DisplayNameText", "显示名", ColumnAlign.Left)
                {
                    Width = "135"
                },
                new Column("ModuleTypeText", "板载/扩展", ColumnAlign.Center)
                {
                    Width = "65"
                },
                new Column("CurrentValueTag", "当前值", ColumnAlign.Center)
                {
                    Width = "65"
                },
                new Column("RuntimeStatusTag", "运行状态", ColumnAlign.Center)
                {
                    Width = "65"
                },
                new Column("WiringStatusTag", "接线", ColumnAlign.Center)
                {
                    Width = "65"
                },
                new Column("CardText", "控制卡", ColumnAlign.Left)
                {
                    Width = "100"
                },
                new Column("WiringText", "接线信息", ColumnAlign.Left)
                {
                    Width = "220",
                    LineBreak = true
                },
                new Column("DeviceText", "对端设备", ColumnAlign.Left)
                {
                    Width = "160",
                    LineBreak = true
                },
                new Column("FieldInfoText", "现场信息", ColumnAlign.Left)
                {
                    Width = "200",
                    LineBreak = true
                }
            };
        }

        private void BindEvents()
        {
            Load += AssemblyWiringPage_Load;

            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonExportCsv.Click += ButtonExportCsv_Click;
            buttonSelectCard.Click += ButtonSelectCard_Click;
            buttonFilterAll.Click += ButtonFilterAll_Click;
            buttonFilterDI.Click += ButtonFilterDI_Click;
            buttonFilterDO.Click += ButtonFilterDO_Click;

            inputSearch.TextChanged += InputSearch_TextChanged;
            checkboxOnlyUnverified.CheckedChanged += CheckboxOnlyUnverified_CheckedChanged;

            tableWiring.CellClick += TableWiring_CellClick;

            buttonReadDi.Click += async (s, e) => await ExecuteValueAsync(() => _model.ReadSelectedDi());
            buttonDoOn.Click += async (s, e) => await ExecuteAsync(() => _model.SetSelectedDo(true));
            buttonDoOff.Click += async (s, e) => await ExecuteAsync(() => _model.SetSelectedDo(false));
            buttonPulseDo.Click += async (s, e) => await ExecuteAsync(() => _model.PulseSelectedDo(GetPulseWidthMs()));
            buttonMarkVerified.Click += async (s, e) => await ExecuteAsync(() => _model.MarkSelectedVerified());
            buttonCancelVerified.Click += async (s, e) => await ExecuteAsync(() => _model.CancelSelectedVerified());
        }

        private async void AssemblyWiringPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (_isBusy)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await _model.LoadAsync();
                _lastActionMessage = result.Message;
                RefreshView();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ExecuteAsync(Func<Result> action)
        {
            if (_isBusy || action == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await Task.Run(action);
                _lastActionMessage = result.Message;
                await _model.RefreshAsync();
                RefreshView();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ExecuteValueAsync(Func<Result<bool>> action)
        {
            if (_isBusy || action == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await Task.Run(action);
                _lastActionMessage = result.Success
                    ? result.Message + "，值=" + (result.Item ? "ON" : "OFF")
                    : result.Message;

                await _model.RefreshAsync();
                RefreshView();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void RefreshView()
        {
            _isBindingView = true;
            try
            {
                if (!inputSearch.Focused && !string.Equals(inputSearch.Text, _model.SearchText, StringComparison.Ordinal))
                {
                    inputSearch.Text = _model.SearchText;
                }

                checkboxOnlyUnverified.Checked = _model.OnlyUnverified;
                labelSelectedCard.Text = _model.SelectedCardText;
            }
            finally
            {
                _isBindingView = false;
            }

            UpdateFilterButtons();
            RefreshStatCards();
            RebindTable();
            RefreshDebugPanel();
            RefreshDebugModeState();
            RefreshActionButtons();
        }

        private void RefreshStatCards()
        {
            labelTotalCount.Text = _model.TotalCount.ToString();
            labelVerifiedCount.Text = _model.VerifiedCount.ToString();
            labelUnverifiedCount.Text = _model.UnverifiedCount.ToString();
            labelTableSummary.Text = _model.SummaryText;
        }

        private void RebindTable()
        {
            _tableRows = new AntList<WiringTableRow>();

            foreach (var item in _model.Items)
            {
                _tableRows.Add(new WiringTableRow
                {
                    Item = item,
                    IoTypeTag = BuildIoTypeTag(item),
                    LogicalBitText = new CellText(item.LogicalBit.ToString()),
                    HardwareBitText = new CellText(item.HardwareBit.ToString()),
                    DisplayNameText = new CellText(string.IsNullOrWhiteSpace(item.DisplayName) ? "-" : item.DisplayName),
                    CardText = new CellText(item.CardDisplayName),
                    ModuleTypeText = new CellText(item.IsExtModule ? "扩展" : "板载"),
                    WiringText = new CellText(BuildWiringText(item)),
                    DeviceText = new CellText(BuildDeviceText(item)),
                    FieldInfoText = new CellText(BuildFieldInfoText(item)),
                    CurrentValueTag = BuildCurrentValueTag(item),
                    RuntimeStatusTag = BuildRuntimeStatusTag(item),
                    WiringStatusTag = BuildWiringStatusTag(item)
                });
            }

            tableWiring.Binding(_tableRows);
        }

        private void RefreshDebugPanel()
        {
            var item = _model.SelectedItem;
            if (item == null)
            {
                labelDebugTitle.Text = "点位检查";
                labelSelectedNameValue.Text = "未选择点位";
                labelSelectedHardwareValue.Text = "-";
                labelSelectedWiringValue.Text = "-";
                labelSelectedRuntimeValue.Text = "-";
                labelDebugHint.Text = string.IsNullOrWhiteSpace(_lastActionMessage)
                    ? "请选择表格中的点位后执行检查；本页仅用于接线对照和单点 IO 检查。"
                    : _lastActionMessage;
                return;
            }

            labelDebugTitle.Text = _model.SelectedCheckModeText;
            labelSelectedNameValue.Text = BuildSelectedNameText(item);
            labelSelectedHardwareValue.Text = "逻辑IO " + item.LogicalBit + " / 硬件位 " + item.HardwareBit + " / " + (item.IsExtModule ? "扩展" : "板载");
            labelSelectedWiringValue.Text = BuildWiringText(item) + " / " + BuildFieldInfoText(item) + " / " + BuildVerifyStatusText(item);
            labelSelectedRuntimeValue.Text = item.CurrentValueText + " / " + item.RuntimeStatusText + " / " + item.LastUpdateTimeText;

            if (!string.IsNullOrWhiteSpace(_lastActionMessage))
            {
                labelDebugHint.Text = _lastActionMessage;
            }
            else if (_model.IsSelectedDi)
            {
                labelDebugHint.Text = "先对照逻辑IO号、硬件位号和显示名，再读取 DI 状态确认物理接线是否正确。";
            }
            else if (_model.CanSetSelectedDo)
            {
                labelDebugHint.Text = "先对照逻辑IO号、硬件位号和显示名，再执行 DO 打开、关闭或脉冲输出确认物理接线。";
            }
            else
            {
                labelDebugHint.Text = "当前 DO 点未开放手动操作，仅可查看接线和状态。";
            }
        }

        private void RefreshDebugModeState()
        {
            var isDi = _model.IsSelectedDi;
            var isDo = _model.IsSelectedDo;
            var hasSelection = _model.HasSelection;

            labelPulseWidth.Visible = isDo;
            inputPulseWidth.Visible = isDo;

            buttonReadDi.Visible = isDi;
            buttonDoOn.Visible = isDo;
            buttonDoOff.Visible = isDo;
            buttonPulseDo.Visible = isDo;
            buttonMarkVerified.Visible = hasSelection;
            buttonCancelVerified.Visible = hasSelection;
        }

        private void RefreshActionButtons()
        {
            buttonExportCsv.Enabled = _model.Items.Count > 0;
            inputPulseWidth.Enabled = _model.IsSelectedDo;

            buttonReadDi.Enabled = _model.CanReadSelectedDi;
            buttonDoOn.Enabled = _model.CanSetSelectedDo;
            buttonDoOff.Enabled = _model.CanSetSelectedDo;
            buttonPulseDo.Enabled = _model.CanSetSelectedDo;
            buttonMarkVerified.Enabled = _model.CanMarkSelectedVerified;
            buttonCancelVerified.Enabled = _model.CanCancelSelectedVerified;
        }

        private void UpdateFilterButtons()
        {
            buttonFilterAll.Type = string.Equals(_model.SelectedIoType, "All", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterDI.Type = string.Equals(_model.SelectedIoType, "DI", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterDO.Type = string.Equals(_model.SelectedIoType, "DO", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            UseWaitCursor = isBusy;
        }

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            if (_isBindingView)
            {
                return;
            }

            _model.SetSearchText(inputSearch.Text);
            RefreshView();
        }

        private void CheckboxOnlyUnverified_CheckedChanged(object sender, BoolEventArgs e)
        {
            if (_isBindingView)
            {
                return;
            }

            _model.SetOnlyUnverified(checkboxOnlyUnverified.Checked);
            RefreshView();
        }

        private void ButtonSelectCard_Click(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }

            using (var dialog = new MotionCardSelectDialog(_model.SelectedCardId))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                {
                    return;
                }

                _model.SetCardFilter(dialog.SelectedCardId);
                RefreshView();
            }
        }

        private void ButtonExportCsv_Click(object sender, EventArgs e)
        {
            if (_isBusy || _model.Items.Count <= 0)
            {
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "导出接线表";
                dialog.Filter = "CSV 文件 (*.csv)|*.csv";
                dialog.FileName = "assembly_wiring_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    var csv = _model.ExportCurrentItemsToCsv();
                    File.WriteAllText(dialog.FileName, csv, new UTF8Encoding(true));
                    _lastActionMessage = "接线表导出成功：" + dialog.FileName;
                }
                catch (IOException ex)
                {
                    _lastActionMessage = "接线表导出失败：" + ex.Message;
                }
                catch (UnauthorizedAccessException ex)
                {
                    _lastActionMessage = "接线表导出失败：" + ex.Message;
                }

                RefreshDebugPanel();
            }
        }

        private void ButtonFilterAll_Click(object sender, EventArgs e)
        {
            _model.SetIoTypeFilter("All");
            RefreshView();
        }

        private void ButtonFilterDI_Click(object sender, EventArgs e)
        {
            _model.SetIoTypeFilter("DI");
            RefreshView();
        }

        private void ButtonFilterDO_Click(object sender, EventArgs e)
        {
            _model.SetIoTypeFilter("DO");
            RefreshView();
        }

        private void TableWiring_CellClick(object sender, TableClickEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            var row = e.Record as WiringTableRow;
            if (row == null || row.Item == null)
            {
                return;
            }

            _lastActionMessage = string.Empty;
            _model.SelectRow(row.Item.IoMapId);
            RefreshView();
        }

        private int GetPulseWidthMs()
        {
            int pulseWidth;
            if (!int.TryParse(inputPulseWidth.Text, out pulseWidth) || pulseWidth <= 0)
            {
                pulseWidth = 300;
                inputPulseWidth.Text = pulseWidth.ToString();
            }

            return pulseWidth;
        }

        private static CellTag BuildIoTypeTag(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            return new CellTag(
                string.Equals(item.IoType, "DO", StringComparison.OrdinalIgnoreCase) ? "DO" : "DI",
                string.Equals(item.IoType, "DO", StringComparison.OrdinalIgnoreCase) ? TTypeMini.Warn : TTypeMini.Primary);
        }

        private static CellTag BuildCurrentValueTag(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            return new CellTag(
                item.CurrentValueText,
                string.Equals(item.CurrentValueText, "ON", StringComparison.OrdinalIgnoreCase) ? TTypeMini.Success : TTypeMini.Default);
        }

        private static CellTag BuildRuntimeStatusTag(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            return new CellTag(
                item.RuntimeStatusText,
                string.Equals(item.RuntimeStatusText, "已刷新", StringComparison.OrdinalIgnoreCase) ? TTypeMini.Success : TTypeMini.Warn);
        }

        private static CellTag BuildWiringStatusTag(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            if (string.Equals(item.WiringStatusText, "已核对", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag(item.WiringStatusText, TTypeMini.Success);
            }

            if (string.Equals(item.WiringStatusText, "未核对", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag(item.WiringStatusText, TTypeMini.Warn);
            }

            return new CellTag(item.WiringStatusText, TTypeMini.Error);
        }

        private static string BuildSelectedNameText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            var displayName = string.IsNullOrWhiteSpace(item.DisplayName)
                ? "未命名点位"
                : item.DisplayName;

            return item.IoType + " " + item.LogicalBit + " / " + displayName;
        }

        private static string BuildWiringText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            var terminalText = string.IsNullOrWhiteSpace(item.TerminalBlock) && string.IsNullOrWhiteSpace(item.TerminalNo)
                ? "端子未定义"
                : (item.TerminalBlock + " " + item.TerminalNo).Trim();

            var connectorText = string.IsNullOrWhiteSpace(item.ConnectorNo) && string.IsNullOrWhiteSpace(item.PinNo)
                ? string.Empty
                : " / 插头 " + (item.ConnectorNo + " " + item.PinNo).Trim();

            var wireText = string.IsNullOrWhiteSpace(item.WireNo)
                ? string.Empty
                : " / 线号 " + item.WireNo;

            return terminalText + connectorText + wireText;
        }

        private static string BuildDeviceText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            if (string.IsNullOrWhiteSpace(item.DeviceName)
                && string.IsNullOrWhiteSpace(item.DeviceModel)
                && string.IsNullOrWhiteSpace(item.DeviceTerminal))
            {
                return "-";
            }

            var deviceText = string.IsNullOrWhiteSpace(item.DeviceName) ? string.Empty : item.DeviceName;
            if (!string.IsNullOrWhiteSpace(item.DeviceModel))
            {
                deviceText = string.IsNullOrWhiteSpace(deviceText)
                    ? item.DeviceModel
                    : deviceText + " / " + item.DeviceModel;
            }

            if (!string.IsNullOrWhiteSpace(item.DeviceTerminal))
            {
                deviceText = string.IsNullOrWhiteSpace(deviceText)
                    ? item.DeviceTerminal
                    : deviceText + " / " + item.DeviceTerminal;
            }

            return string.IsNullOrWhiteSpace(deviceText) ? "-" : deviceText;
        }

        private static string BuildFieldInfoText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            var parts = new System.Collections.Generic.List<string>();

            if (!string.IsNullOrWhiteSpace(item.CabinetArea))
            {
                parts.Add("区域 " + item.CabinetArea);
            }

            if (!string.IsNullOrWhiteSpace(item.SignalType))
            {
                parts.Add("信号 " + item.SignalType);
            }

            if (!string.IsNullOrWhiteSpace(item.ExpectedNormalState))
            {
                parts.Add("常态 " + item.ExpectedNormalState);
            }

            if (!string.IsNullOrWhiteSpace(item.CheckMethod))
            {
                parts.Add("点检 " + item.CheckMethod);
            }

            if (!string.IsNullOrWhiteSpace(item.WiringRemark))
            {
                parts.Add("备注 " + item.WiringRemark);
            }

            return parts.Count == 0 ? "-" : string.Join(" / ", parts);
        }

        private static string BuildVerifyStatusText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            if (!item.IsVerified)
            {
                return "未核对";
            }

            return string.IsNullOrWhiteSpace(item.VerifiedBy)
                ? "已核对"
                : "已核对 / " + item.VerifiedBy;
        }

        private sealed class WiringTableRow
        {
            public AssemblyWiringPageModel.AssemblyWiringRowViewItem Item { get; set; }

            public CellTag IoTypeTag { get; set; }

            public CellText LogicalBitText { get; set; }

            public CellText HardwareBitText { get; set; }

            public CellText DisplayNameText { get; set; }

            public CellText CardText { get; set; }

            public CellText ModuleTypeText { get; set; }

            public CellText WiringText { get; set; }

            public CellText DeviceText { get; set; }

            public CellText FieldInfoText { get; set; }

            public CellTag CurrentValueTag { get; set; }

            public CellTag RuntimeStatusTag { get; set; }

            public CellTag WiringStatusTag { get; set; }
        }
    }
}
