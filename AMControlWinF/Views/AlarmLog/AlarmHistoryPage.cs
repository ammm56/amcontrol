using AM.Model.Common;
using AM.Model.Entity.Dev;
using AM.PageModel.AlarmLog;
using AntdUI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.AlarmLog
{
    /// <summary>
    /// 报警历史页面。
    /// 布局与 LoginLogPage 保持一致：
    /// 1. 顶部工具栏；
    /// 2. 第二行统计卡片；
    /// 3. 第三行整页 Table；
    /// 4. 底部分页栏。
    /// </summary>
    public partial class AlarmHistoryPage : UserControl
    {
        private readonly AlarmHistoryPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isUpdatingPagination;
        private AntList<AlarmHistoryTableRow> _tableRows;

        public AlarmHistoryPage()
        {
            InitializeComponent();

            _model = new AlarmHistoryPageModel();
            _tableRows = new AntList<AlarmHistoryTableRow>();

            InitializeTableColumns();
            BindEvents();
            InitializePagination();
            UpdateFilterButtons();
        }

        private void BindEvents()
        {
            Load += AlarmHistoryPage_Load;

            inputSearch.TextChanged += InputSearch_TextChanged;
            inputSearch.KeyDown += InputSearch_KeyDown;

            buttonAll.Click += async (s, e) => await ShowAllAsync();
            buttonCritical.Click += async (s, e) => await ShowCriticalAsync();
            buttonAlarm.Click += async (s, e) => await ShowAlarmAsync();
            buttonWarning.Click += async (s, e) => await ShowWarningAsync();
            buttonUncleared.Click += async (s, e) => await ShowUnclearedAsync();
            buttonQuery.Click += async (s, e) => await QueryAsync(true);

            paginationHistory.ValueChanged += PaginationHistory_ValueChanged;
        }

        private void InitializePagination()
        {
            paginationHistory.Current = 1;
            paginationHistory.Total = 0;
            paginationHistory.PageSize = 50;
            paginationHistory.PageSizeOptions = new int[] { 10, 20, 50, 100, 200 };
            paginationHistory.ShowSizeChanger = true;
            paginationHistory.SizeChangerWidth = 72;
            paginationHistory.RightToLeft = RightToLeft.Yes;
            paginationHistory.Gap = 8;
            paginationHistory.Radius = 8;
        }

        private void InitializeTableColumns()
        {
            tableHistory.Columns = new ColumnCollection()
            {
                new Column("", "序号", ColumnAlign.Center)
                {
                    Width = "60",
                    Fixed = true,
                    Render = (value, record, rowindex) =>
                    {
                        return ((_model.PageIndex - 1) * _model.PageSize) + rowindex + 1;
                    }
                },
                new Column("AlarmCodeText", "报警代码", ColumnAlign.Center)
                {
                    Width = "90",
                    Fixed = true
                },
                new Column("LevelTag", "报警等级", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("RaisedTimeText", "触发时间", ColumnAlign.Center)
                {
                    Width = "180"
                },
                new Column("SourceText", "报警来源", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("ClearedTag", "清除状态", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("ClearedTimeText", "清除时间", ColumnAlign.Center)
                {
                    Width = "180"
                },
                new Column("MessageText", "报警消息", ColumnAlign.Left)
                {
                    LineBreak = true
                }
            };

            tableHistory.StackedHeaderRows = new StackedHeaderRow[]
            {
                new StackedHeaderRow(
                    new StackedColumn("AlarmCodeText,LevelTag,RaisedTimeText,SourceText", "报警信息"),
                    new StackedColumn("ClearedTag,ClearedTimeText", "处理信息"),
                    new StackedColumn("MessageText", "报警内容"))
            };
        }

        private async void AlarmHistoryPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            _model.SetAllFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SearchText = inputSearch.Text;
        }

        private async void InputSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            await QueryAsync(true);
        }

        private async void PaginationHistory_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isUpdatingPagination || _isBusy || e == null)
            {
                return;
            }

            _model.SetPage(e.Current, e.PageSize);
            await ReloadAsync();
        }

        private async Task ShowAllAsync()
        {
            _model.SetAllFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowCriticalAsync()
        {
            _model.SetCriticalFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowAlarmAsync()
        {
            _model.SetAlarmFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowWarningAsync()
        {
            _model.SetWarningFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowInfoAsync()
        {
            _model.SetInfoFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowUnclearedAsync()
        {
            _model.SetUnclearedFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task QueryAsync(bool resetPage)
        {
            _model.SearchText = inputSearch.Text;
            _model.StartDate = pickerStart.Value;
            _model.EndDate = pickerEnd.Value;

            if (resetPage)
            {
                _model.ResetToFirstPage();
            }

            UpdateFilterButtons();
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
                Result result = await _model.LoadAsync();

                RefreshStatCards();
                RebindTable();
                SyncPagination();

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

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;

            inputSearch.Enabled = !isBusy;
            pickerStart.Enabled = !isBusy;
            pickerEnd.Enabled = !isBusy;
            buttonQuery.Enabled = !isBusy;
            buttonAll.Enabled = !isBusy;
            buttonCritical.Enabled = !isBusy;
            buttonAlarm.Enabled = !isBusy;
            buttonWarning.Enabled = !isBusy;
            buttonUncleared.Enabled = !isBusy;
            paginationHistory.Enabled = !isBusy;
        }

        private void UpdateFilterButtons()
        {
            string filter = _model.SelectedFilter ?? "All";

            buttonAll.Type = string.Equals(filter, "All", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonCritical.Type = string.Equals(filter, "Critical", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonAlarm.Type = string.Equals(filter, "Alarm", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonWarning.Type = string.Equals(filter, "Warning", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonUncleared.Type = string.Equals(filter, "Uncleared", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
        }

        private void RefreshStatCards()
        {
            labelTotalCount.Text = _model.TotalAlarmCount.ToString();
            labelCriticalCount.Text = _model.CriticalCount.ToString();
            labelUnclearedCount.Text = _model.UnclearedCount.ToString();
            labelClearedCount.Text = _model.ClearedCount.ToString();
        }

        private void SyncPagination()
        {
            _isUpdatingPagination = true;
            try
            {
                paginationHistory.Current = _model.PageIndex;
                paginationHistory.Total = _model.TotalCount;
                paginationHistory.PageSize = _model.PageSize;
            }
            finally
            {
                _isUpdatingPagination = false;
            }
        }

        private void RebindTable()
        {
            _tableRows = new AntList<AlarmHistoryTableRow>();

            foreach (DevAlarmRecordEntity item in _model.Records)
            {
                _tableRows.Add(new AlarmHistoryTableRow
                {
                    AlarmCodeText = item.AlarmCode.ToString(),
                    LevelTag = BuildLevelTag(item.AlarmLevel),
                    RaisedTimeText = new CellText(item.RaisedTime.ToString("yyyy-MM-dd HH:mm:ss")),
                    SourceText = new CellText(item.Source ?? string.Empty),
                    ClearedTag = item.IsCleared
                        ? new CellTag("已清除", TTypeMini.Success)
                        : new CellTag("未清除", TTypeMini.Error),
                    ClearedTimeText = new CellText(
                        item.ClearedTime.HasValue
                            ? item.ClearedTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            : "-"),
                    MessageText = new CellText(item.Message ?? string.Empty)
                });
            }

            tableHistory.Binding(_tableRows);
        }

        private static CellTag BuildLevelTag(string levelText)
        {
            if (string.Equals(levelText, "Critical", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("Critical", TTypeMini.Error);
            }

            if (string.Equals(levelText, "Alarm", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("Alarm", TTypeMini.Error);
            }

            if (string.Equals(levelText, "Warning", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("Warning", TTypeMini.Warn);
            }

            return new CellTag(string.IsNullOrWhiteSpace(levelText) ? "Info" : levelText, TTypeMini.Default);
        }

        private sealed class AlarmHistoryTableRow
        {
            public string AlarmCodeText { get; set; }

            public CellTag LevelTag { get; set; }

            public CellText RaisedTimeText { get; set; }

            public CellText SourceText { get; set; }

            public CellTag ClearedTag { get; set; }

            public CellText ClearedTimeText { get; set; }

            public CellText MessageText { get; set; }
        }
    }
}