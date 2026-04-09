using AM.Model.Auth;
using AM.PageModel.Am;
using AntdUI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 登录日志页面。
    /// 被 MainWindow 页面缓存复用：
    /// - 不在离开页面时释放 ViewModel；
    /// - 首次加载使用布尔标记控制，避免重复初始化。
    /// </summary>
    public partial class LoginLogPage : UserControl
    {
        private readonly LoginLogPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isUpdatingPagination;
        private bool _isTodayMode;
        private AntList<LoginLogTableRow> _tableRows;

        public LoginLogPage()
        {
            InitializeComponent();

            _model = new LoginLogPageModel();
            _tableRows = new AntList<LoginLogTableRow>();

            InitializeTableColumns();
            BindEvents();
            InitializePagination();
            UpdateFilterButtons();
        }

        private void BindEvents()
        {
            Load += LoginLogPage_Load;

            inputSearch.TextChanged += InputSearch_TextChanged;
            inputSearch.KeyDown += InputSearch_KeyDown;

            buttonAll.Click += async (s, e) => await ShowAllAsync();
            buttonSuccess.Click += async (s, e) => await ShowSuccessAsync();
            buttonFailed.Click += async (s, e) => await ShowFailedAsync();
            buttonToday.Click += async (s, e) => await ShowTodayAsync();
            buttonQuery.Click += async (s, e) => await QueryAsync(true);

            paginationLogs.ValueChanged += PaginationLogs_ValueChanged;
        }

        private void InitializePagination()
        {
            paginationLogs.Current = 1;
            paginationLogs.Total = 0;
            paginationLogs.PageSize = 50;
            paginationLogs.PageSizeOptions = new int[] { 10, 20, 50, 100, 200 };
            paginationLogs.ShowSizeChanger = true;
            paginationLogs.SizeChangerWidth = 72;
            paginationLogs.RightToLeft = RightToLeft.Yes;
            paginationLogs.Gap = 8;
            paginationLogs.Radius = 8;
        }

        private void InitializeTableColumns()
        {
            tableLogs.Columns = new ColumnCollection()
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
                new Column("LoginName", "登录名", ColumnAlign.Center)
                {
                    Width = "140",
                    Fixed = true
                },
                new Column("ResultTag", "结果", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("LoginTimeText", "登录时间", ColumnAlign.Center)
                {
                    Width = "180"
                },
                new Column("AppVersionText", "版本", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("MessageText", "消息", ColumnAlign.Left)
                {
                    LineBreak = true
                }
            };

            tableLogs.StackedHeaderRows = new StackedHeaderRow[]
            {
                new StackedHeaderRow(
                    new StackedColumn("LoginName,ResultTag,LoginTimeText", "登录信息"),
                    new StackedColumn("AppVersionText,MessageText", "附加信息"))
            };
        }

        private async void LoginLogPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            _model.SetAllFilter();
            _isTodayMode = false;
            ClearDatePickers();
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
                return;

            e.SuppressKeyPress = true;
            await QueryAsync(true);
        }

        private async void PaginationLogs_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isUpdatingPagination || _isBusy || e == null)
                return;

            _model.SetPage(e.Current, e.PageSize);
            await ReloadAsync();
        }

        private async Task ShowAllAsync()
        {
            _model.SetAllFilter();
            _isTodayMode = false;
            ClearDatePickers();
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowSuccessAsync()
        {
            _model.SetSuccessFilter();
            _isTodayMode = false;
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowFailedAsync()
        {
            _model.SetFailedFilter();
            _isTodayMode = false;
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ShowTodayAsync()
        {
            _model.SetTodayFilter();
            _isTodayMode = true;
            pickerStart.Value = _model.StartDate;
            pickerEnd.Value = _model.EndDate;
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task QueryAsync(bool resetPage)
        {
            _model.SearchText = inputSearch.Text;
            _model.StartDate = pickerStart.Value;
            _model.EndDate = pickerEnd.Value;

            if (resetPage)
                _model.ResetToFirstPage();

            _isTodayMode = false;
            UpdateFilterButtons();
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (_isBusy)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.LoadAsync();

                RefreshStatCards();
                RebindTable();
                SyncPagination();

                if (!result.Success)
                    return;
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
            buttonSuccess.Enabled = !isBusy;
            buttonFailed.Enabled = !isBusy;
            buttonToday.Enabled = !isBusy;
            paginationLogs.Enabled = !isBusy;
        }

        private void UpdateFilterButtons()
        {
            buttonAll.Type = !_isTodayMode && _model.IsSuccessFilter == null ? TTypeMini.Primary : TTypeMini.Default;
            buttonSuccess.Type = !_isTodayMode && _model.IsSuccessFilter == true ? TTypeMini.Primary : TTypeMini.Default;
            buttonFailed.Type = !_isTodayMode && _model.IsSuccessFilter == false ? TTypeMini.Primary : TTypeMini.Default;
            buttonToday.Type = _isTodayMode ? TTypeMini.Primary : TTypeMini.Default;
        }

        private void RefreshStatCards()
        {
            labelTotalCount.Text = _model.TotalCount.ToString();
            labelSuccessCount.Text = _model.SuccessCount.ToString();
            labelFailedCount.Text = _model.FailedCount.ToString();
        }

        private void SyncPagination()
        {
            _isUpdatingPagination = true;
            try
            {
                paginationLogs.Current = _model.PageIndex;
                paginationLogs.Total = _model.TotalCount;
                paginationLogs.PageSize = _model.PageSize;
            }
            finally
            {
                _isUpdatingPagination = false;
            }
        }

        private void ClearDatePickers()
        {
            pickerStart.Value = null;
            pickerEnd.Value = null;
        }

        private void RebindTable()
        {
            _tableRows = new AntList<LoginLogTableRow>();

            foreach (var item in _model.Logs)
            {
                _tableRows.Add(new LoginLogTableRow
                {
                    LoginName = item.LoginName ?? string.Empty,
                    ResultTag = new CellTag(
                        item.IsSuccess ? "登录成功" : "登录失败",
                        item.IsSuccess ? TTypeMini.Success : TTypeMini.Error),
                    LoginTimeText = new CellText(item.LoginTimeText),
                    AppVersionText = new CellText(item.AppVersionText),
                    MessageText = new CellText(item.MessageText)
                });
            }

            tableLogs.Binding(_tableRows);
        }

        private sealed class LoginLogTableRow
        {
            public string LoginName { get; set; }

            public CellTag ResultTag { get; set; }

            public CellText LoginTimeText { get; set; }

            public CellText AppVersionText { get; set; }

            public CellText MessageText { get; set; }
        }
    }
}