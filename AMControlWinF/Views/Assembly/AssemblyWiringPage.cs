using AM.Model.Common;
using AM.PageModel.Assembly;
using AMControlWinF.Views.MotionConfig;
using AntdUI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Assembly
{
    /// <summary>
    /// 装配接线与调试工作台页面。
    /// 被 MainWindow 页面缓存复用：
    /// - 不在离开页面时释放页面模型；
    /// - 首次加载使用布尔标记控制，避免重复初始化；
    /// - 通过顶部筛选与底部调试区在同页完成接线查看和单点调试。
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
                new Column("SoftwareNameText", "软件点名", ColumnAlign.Left)
                {
                    Width = "150",
                    Fixed = true
                },
                new Column("DisplayNameText", "显示名", ColumnAlign.Left)
                {
                    Width = "160"
                },
                new Column("CardText", "控制卡", ColumnAlign.Left)
                {
                    Width = "145"
                },
                new Column("HardwareText", "硬件位置", ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("WiringText", "接线信息", ColumnAlign.Left)
                {
                    Width = "220",
                    LineBreak = true
                },
                new Column("DeviceText", "对端设备", ColumnAlign.Left)
                {
                    Width = "180",
                    LineBreak = true
                },
                new Column("CurrentValueTag", "当前值", ColumnAlign.Center)
                {
                    Width = "88"
                },
                new Column("RuntimeStatusTag", "运行", ColumnAlign.Center)
                {
                    Width = "92"
                },
                new Column("WiringStatusTag", "接线", ColumnAlign.Center)
                {
                    Width = "92"
                }
            };
        }

        private void BindEvents()
        {
            Load += AssemblyWiringPage_Load;

            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonSelectCard.Click += ButtonSelectCard_Click;
            buttonFilterAll.Click += ButtonFilterAll_Click;
            buttonFilterDI.Click += ButtonFilterDI_Click;
            buttonFilterDO.Click += ButtonFilterDO_Click;

            inputSearch.TextChanged += InputSearch_TextChanged;
            checkboxOnlyUnverified.CheckedChanged += CheckboxOnlyUnverified_CheckedChanged;
            checkboxOnlyIssues.CheckedChanged += CheckboxOnlyIssues_CheckedChanged;
            checkboxDebugMode.CheckedChanged += CheckboxDebugMode_CheckedChanged;

            tableWiring.CellClick += TableWiring_CellClick;

            buttonReadDi.Click += async (s, e) => await ExecuteValueAsync(() => _model.ReadSelectedDi());
            buttonDoOn.Click += async (s, e) => await ExecuteAsync(() => _model.SetSelectedDo(true));
            buttonDoOff.Click += async (s, e) => await ExecuteAsync(() => _model.SetSelectedDo(false));
            buttonPulseDo.Click += async (s, e) => await ExecuteAsync(() => _model.PulseSelectedDo(GetPulseWidthMs()));
            buttonTestActuator.Click += async (s, e) => await ExecuteAsync(() => _model.TestSelectedActuator("Test"));
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

                if (!result.Success)
                {
                    return;
                }
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

                if (!result.Success)
                {
                    return;
                }
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

                if (!result.Success)
                {
                    return;
                }
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
                inputSearch.Text = _model.SearchText;
                checkboxOnlyUnverified.Checked = _model.OnlyUnverified;
                checkboxOnlyIssues.Checked = _model.OnlyIssues;
                checkboxDebugMode.Checked = _model.DebugModeEnabled;
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
            RefreshActionButtons();
        }

        private void RefreshStatCards()
        {
            labelTotalCount.Text = _model.TotalCount.ToString();
            labelVerifiedCount.Text = _model.VerifiedCount.ToString();
            labelUnverifiedCount.Text = _model.UnverifiedCount.ToString();
            labelRuntimeCount.Text = _model.RuntimeOkCount.ToString();
            labelIssueCount.Text = _model.IssueCount.ToString();
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
                    SoftwareNameText = new CellText(item.SoftwareName),
                    DisplayNameText = new CellText(string.IsNullOrWhiteSpace(item.DisplayName) ? "-" : item.DisplayName),
                    CardText = new CellText(item.CardDisplayName),
                    HardwareText = new CellText(BuildHardwareText(item)),
                    WiringText = new CellText(BuildWiringText(item)),
                    DeviceText = new CellText(BuildDeviceText(item)),
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
                labelSelectedNameValue.Text = "未选择点位";
                labelSelectedHardwareValue.Text = "-";
                labelSelectedWiringValue.Text = "-";
                labelSelectedRuntimeValue.Text = "-";
                labelDebugHint.Text = string.IsNullOrWhiteSpace(_lastActionMessage)
                    ? "请选择表格中的点位后再执行调试；高风险动作需要先开启调试模式。"
                    : _lastActionMessage;
                return;
            }

            labelSelectedNameValue.Text = BuildSelectedNameText(item);
            labelSelectedHardwareValue.Text = BuildHardwareText(item);
            labelSelectedWiringValue.Text = BuildWiringText(item);
            labelSelectedRuntimeValue.Text = item.CurrentValueText + " / " + item.RuntimeStatusText + " / " + item.LastUpdateTimeText;

            if (!string.IsNullOrWhiteSpace(_lastActionMessage))
            {
                labelDebugHint.Text = _lastActionMessage;
            }
            else if (!_model.DebugModeEnabled)
            {
                labelDebugHint.Text = "已选中点位，开启调试模式后可执行 DO 和执行器动作。";
            }
            else
            {
                labelDebugHint.Text = "已开启调试模式，可在右侧执行单点调试。";
            }
        }

        private void RefreshActionButtons()
        {
            var item = _model.SelectedItem;
            var hasSelection = item != null;
            var isDi = hasSelection && string.Equals(item.IoType, "DI", StringComparison.OrdinalIgnoreCase);
            var isDo = hasSelection && string.Equals(item.IoType, "DO", StringComparison.OrdinalIgnoreCase);
            var debugEnabled = _model.DebugModeEnabled;
            var canManualDo = isDo && item.CanManualOperate;
            var canTestActuator = hasSelection
                && debugEnabled
                && !string.IsNullOrWhiteSpace(item.RelatedActuatorName);

            buttonSelectCard.Enabled = !_isBusy;
            buttonRefresh.Enabled = !_isBusy;
            buttonFilterAll.Enabled = !_isBusy;
            buttonFilterDI.Enabled = !_isBusy;
            buttonFilterDO.Enabled = !_isBusy;
            inputSearch.Enabled = !_isBusy;
            checkboxOnlyUnverified.Enabled = !_isBusy;
            checkboxOnlyIssues.Enabled = !_isBusy;
            checkboxDebugMode.Enabled = !_isBusy;
            tableWiring.Enabled = !_isBusy;
            inputPulseWidth.Enabled = !_isBusy && debugEnabled;

            buttonReadDi.Enabled = !_isBusy && isDi;
            buttonDoOn.Enabled = !_isBusy && debugEnabled && canManualDo;
            buttonDoOff.Enabled = !_isBusy && debugEnabled && canManualDo;
            buttonPulseDo.Enabled = !_isBusy && debugEnabled && canManualDo;
            buttonTestActuator.Enabled = !_isBusy && canTestActuator;
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
            RefreshActionButtons();
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

        private void CheckboxOnlyIssues_CheckedChanged(object sender, BoolEventArgs e)
        {
            if (_isBindingView)
            {
                return;
            }

            _model.SetOnlyIssues(checkboxOnlyIssues.Checked);
            RefreshView();
        }

        private void CheckboxDebugMode_CheckedChanged(object sender, BoolEventArgs e)
        {
            if (_isBindingView)
            {
                return;
            }

            _model.SetDebugModeEnabled(checkboxDebugMode.Checked);
            _lastActionMessage = string.Empty;
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
                ? item.SoftwareName
                : item.SoftwareName + " / " + item.DisplayName;

            return item.IoType + " " + item.LogicalBit + " / " + displayName;
        }

        private static string BuildHardwareText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            return item.CardDisplayName
                + " / Core " + item.Core
                + " / " + (item.IsExtModule ? "扩展" : "板载")
                + " / Bit " + item.HardwareBit;
        }

        private static string BuildWiringText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            var terminalText = string.IsNullOrWhiteSpace(item.TerminalBlock) && string.IsNullOrWhiteSpace(item.TerminalNo)
                ? "端子未定义"
                : (item.TerminalBlock + " " + item.TerminalNo).Trim();

            var wireText = string.IsNullOrWhiteSpace(item.WireNo)
                ? string.Empty
                : " / 线号 " + item.WireNo;

            return terminalText + wireText;
        }

        private static string BuildDeviceText(AssemblyWiringPageModel.AssemblyWiringRowViewItem item)
        {
            if (string.IsNullOrWhiteSpace(item.DeviceName) && string.IsNullOrWhiteSpace(item.DeviceTerminal))
            {
                return "-";
            }

            if (string.IsNullOrWhiteSpace(item.DeviceTerminal))
            {
                return item.DeviceName;
            }

            return item.DeviceName + " / " + item.DeviceTerminal;
        }

        private sealed class WiringTableRow
        {
            public AssemblyWiringPageModel.AssemblyWiringRowViewItem Item { get; set; }

            public CellTag IoTypeTag { get; set; }

            public CellText SoftwareNameText { get; set; }

            public CellText DisplayNameText { get; set; }

            public CellText CardText { get; set; }

            public CellText HardwareText { get; set; }

            public CellText WiringText { get; set; }

            public CellText DeviceText { get; set; }

            public CellTag CurrentValueTag { get; set; }

            public CellTag RuntimeStatusTag { get; set; }

            public CellTag WiringStatusTag { get; set; }
        }
    }
}
