using AM.DBService.Services.Dev;
using AM.Model.Common;
using AM.Model.Model;
using AM.PageModel.AlarmLog;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.AlarmLog
{
    /// <summary>
    /// 运行日志页面。
    /// 布局与登录日志、报警历史保持一致：
    /// 1. 顶部工具栏；
    /// 2. 第二行统计卡片；
    /// 3. 第三行整页 Table；
    /// 4. 底部分页栏。
    /// </summary>
    public partial class RunLogPage : UserControl
    {
        private readonly RunLogPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isUpdatingPagination;
        private bool _isBindingView;
        private AntList<RunLogTableRow> _tableRows;

        public RunLogPage()
        {
            InitializeComponent();

            _model = new RunLogPageModel();
            _tableRows = new AntList<RunLogTableRow>();

            InitializeTableColumns();
            BindEvents();
            InitializePagination();
            UpdateFilterButtons();
        }

        private void BindEvents()
        {
            Load += RunLogPage_Load;
            VisibleChanged += RunLogPage_VisibleChanged;

            inputSearch.TextChanged += InputSearch_TextChanged;
            inputSearch.KeyDown += InputSearch_KeyDown;

            buttonRefresh.Click += async (s, e) => await RefreshFilesAndReloadAsync();
            buttonQuery.Click += async (s, e) => await QueryAsync(true);

            buttonAll.Click += async (s, e) => await ShowAllAsync();
            buttonError.Click += async (s, e) => await ShowErrorAsync();
            buttonWarn.Click += async (s, e) => await ShowWarnAsync();
            buttonInfo.Click += async (s, e) => await ShowInfoAsync();
            buttonDebug.Click += async (s, e) => await ShowDebugAsync();

            selectLogFile.SelectedIndexChanged += async (s, e) => await SelectLogFileAsync();
            paginationRunLogs.ValueChanged += PaginationRunLogs_ValueChanged;
        }

        private void InitializePagination()
        {
            paginationRunLogs.Current = 1;
            paginationRunLogs.Total = 0;
            paginationRunLogs.PageSize = 100;
            paginationRunLogs.PageSizeOptions = new int[] { 20, 50, 100, 200, 500 };
            paginationRunLogs.ShowSizeChanger = true;
            paginationRunLogs.SizeChangerWidth = 72;
            paginationRunLogs.RightToLeft = RightToLeft.Yes;
            paginationRunLogs.Gap = 8;
            paginationRunLogs.Radius = 8;
        }

        private void InitializeTableColumns()
        {
            tableRunLogs.Columns = new ColumnCollection()
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
                new Column("LineNumberText", "行号", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("TimeText", "时间", ColumnAlign.Center)
                {
                    Width = "130",
                    Fixed = true
                },
                new Column("LevelTag", "级别", ColumnAlign.Center)
                {
                    Width = "90"
                },
                new Column("LoggerText", "来源", ColumnAlign.Left)
                {
                    Width = "130"
                },
                new Column("MessageText", "日志消息", ColumnAlign.Left)
                {
                    LineBreak = true
                }
            };

            tableRunLogs.StackedHeaderRows = new StackedHeaderRow[]
            {
                new StackedHeaderRow(
                    new StackedColumn("LineNumberText,TimeText,LevelTag,LoggerText", "日志信息"),
                    new StackedColumn("MessageText", "日志内容"))
            };
        }

        private async void RunLogPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            BeginInvoke(new Action(async () =>
            {
                await InitializePageAsync();
            }));
        }

        private async Task InitializePageAsync()
        {
            _model.SetAllFilter();
            UpdateFilterButtons();
            await RefreshFilesAndReloadAsync();
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

        private async void RunLogPage_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible || !_isFirstLoad || _isBusy)
            {
                return;
            }

            await RefreshFilesAndReloadAsync();
        }

        private async Task RefreshFilesAndReloadAsync()
        {
            if (_isBusy)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                Result refreshResult = await _model.RefreshFilesAsync();
                BindLogFileSelect();
                RefreshStatCards();
                RebindTable();
                SyncPagination();

                if (!refreshResult.Success)
                {
                    return;
                }

                Result loadResult = await _model.LoadAsync();
                RefreshStatCards();
                RebindTable();
                SyncPagination();

                if (!loadResult.Success)
                {
                    return;
                }
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task SelectLogFileAsync()
        {
            if (_isBindingView || _isBusy)
            {
                return;
            }

            string selectedPath = selectLogFile.SelectedValue == null
                ? string.Empty
                : selectLogFile.SelectedValue.ToString();

            _model.SetSelectedLogFile(selectedPath);
            await ReloadAsync();
        }

        private async void PaginationRunLogs_ValueChanged(object sender, PagePageEventArgs e)
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

        private async Task ShowErrorAsync()
        {
            _model.SetErrorFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowWarnAsync()
        {
            _model.SetWarnFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowInfoAsync()
        {
            _model.SetInfoFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowDebugAsync()
        {
            _model.SetDebugFilter();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task QueryAsync(bool resetPage)
        {
            _model.SearchText = inputSearch.Text;

            if (resetPage)
            {
                _model.ResetToFirstPage();
            }

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
            selectLogFile.Enabled = !isBusy;
            buttonQuery.Enabled = !isBusy;
            buttonRefresh.Enabled = !isBusy;
            buttonAll.Enabled = !isBusy;
            buttonError.Enabled = !isBusy;
            buttonWarn.Enabled = !isBusy;
            buttonInfo.Enabled = !isBusy;
            buttonDebug.Enabled = !isBusy;
            paginationRunLogs.Enabled = !isBusy;
        }

        private void BindLogFileSelect()
        {
            _isBindingView = true;
            try
            {
                List<SelectItem> items = new List<SelectItem>();

                foreach (RunLogService.LogFileInfo item in _model.LogFiles)
                {
                    items.Add(new SelectItem(item.DisplayName, item.FilePath));
                }

                selectLogFile.Items.Clear();
                selectLogFile.Items.AddRange(items.ToArray());

                string selectedValue = _model.SelectedLogFilePath ?? string.Empty;
                if (!string.Equals(
                    selectLogFile.SelectedValue == null ? string.Empty : selectLogFile.SelectedValue.ToString(),
                    selectedValue,
                    StringComparison.OrdinalIgnoreCase))
                {
                    selectLogFile.SelectedValue = selectedValue;
                }
            }
            finally
            {
                _isBindingView = false;
            }
        }

        private void UpdateFilterButtons()
        {
            string level = _model.SelectedLevel ?? "All";

            buttonAll.Type = string.Equals(level, "All", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonError.Type = string.Equals(level, "ERROR", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonWarn.Type = string.Equals(level, "WARN", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonInfo.Type = string.Equals(level, "INFO", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
            buttonDebug.Type = string.Equals(level, "DEBUG", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
        }

        private void RefreshStatCards()
        {
            labelTotalCount.Text = _model.TotalLogCount.ToString();
            labelErrorCount.Text = _model.ErrorCount.ToString();
            labelWarnCount.Text = _model.WarnCount.ToString();
            labelInfoCount.Text = _model.InfoCount.ToString();
        }

        private void SyncPagination()
        {
            _isUpdatingPagination = true;
            try
            {
                paginationRunLogs.Current = _model.PageIndex;
                paginationRunLogs.Total = _model.TotalCount;
                paginationRunLogs.PageSize = _model.PageSize;
            }
            finally
            {
                _isUpdatingPagination = false;
            }
        }

        private void RebindTable()
        {
            _tableRows = new AntList<RunLogTableRow>();

            foreach (LogModel item in _model.Entries)
            {
                _tableRows.Add(new RunLogTableRow
                {
                    LineNumberText = item.LineNumber.ToString(),
                    TimeText = new CellText(item.Time.HasValue
                        ? item.Time.Value.ToString("HH:mm:ss.fff")
                        : "-"),
                    LevelTag = BuildLevelTag(item.Level),
                    LoggerText = new CellText(item.Logger ?? string.Empty),
                    MessageText = new CellText(item.Message ?? item.RawLine ?? string.Empty)
                });
            }

            tableRunLogs.Binding(_tableRows);
        }

        private static CellTag BuildLevelTag(string levelText)
        {
            if (string.Equals(levelText, "ERROR", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(levelText, "FATAL", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag(string.IsNullOrWhiteSpace(levelText) ? "ERROR" : levelText, TTypeMini.Error);
            }

            if (string.Equals(levelText, "WARN", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("WARN", TTypeMini.Warn);
            }

            if (string.Equals(levelText, "INFO", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag("INFO", TTypeMini.Success);
            }

            if (string.Equals(levelText, "DEBUG", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(levelText, "TRACE", StringComparison.OrdinalIgnoreCase))
            {
                return new CellTag(string.IsNullOrWhiteSpace(levelText) ? "DEBUG" : levelText, TTypeMini.Primary);
            }

            return new CellTag(string.IsNullOrWhiteSpace(levelText) ? "UNKNOWN" : levelText, TTypeMini.Default);
        }

        private sealed class RunLogTableRow
        {
            public string LineNumberText { get; set; }

            public CellText TimeText { get; set; }

            public CellTag LevelTag { get; set; }

            public CellText LoggerText { get; set; }

            public CellText MessageText { get; set; }
        }
    }
}