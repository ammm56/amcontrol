using AM.Core.Alarm;
using AM.Model.Alarm;
using AM.PageModel.AlarmLog;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.AlarmLog
{
    /// <summary>
    /// 当前报警主体内容控件。
    /// 用途：
    /// 1. 供导航页 `CurrentAlarmPage` 直接承载；
    /// 2. 供 `ActiveAlarmDrawerControl` 复用；
    /// 3. 本控件不承担 Drawer 关闭行为。
    /// </summary>
    public partial class ActiveAlarmContentControl : UserControl
    {
        private CurrentAlarmPageModel _model;
        private AlarmManager _alarmManager;
        private bool _isRefreshing;

        public ActiveAlarmContentControl()
        {
            InitializeComponent();

            InitializeTableColumns();
            BindEvents();
            InitializePagination();
            ShowAlarmDetail(null);

            Disposed += (s, e) => UnsubscribeAlarmManager();
        }

        /// <summary>
        /// 绑定页面模型。
        /// </summary>
        public void BindModel(CurrentAlarmPageModel model)
        {
            _model = model;
        }

        /// <summary>
        /// 绑定报警管理器。
        /// </summary>
        public void BindAlarmManager(AlarmManager alarmManager)
        {
            _alarmManager = alarmManager;

            if (_model != null)
            {
                _model.BindAlarmManager(alarmManager);
            }

            SubscribeAlarmManager();
        }

        /// <summary>
        /// 主动刷新页面内容。
        /// </summary>
        public void RefreshAlarms()
        {
            if (_isRefreshing || _model == null)
            {
                return;
            }

            _isRefreshing = true;
            try
            {
                _model.Refresh();
                RebindTable();
                RefreshSummary();
                SyncPagination();
                RefreshActionButtons();
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void InitializeTableColumns()
        {
            tableAlarms.Columns = new ColumnCollection()
            {
                new Column("TimeText", "时间", ColumnAlign.Center) { Width = "140", Fixed = true },
                new Column("LevelTag", "等级", ColumnAlign.Center) { Width = "88" },
                new Column("CodeText", "代码", ColumnAlign.Center) { Width = "72" },
                new Column("SourceText", "来源", ColumnAlign.Left) { Width = "132" },
                new Column("MessageText", "消息", ColumnAlign.Left) { LineBreak = true }
            };
        }

        private void BindEvents()
        {
            buttonRefresh.Click += (s, e) => RefreshAlarms();
            buttonClearCurrent.Click += ButtonClearCurrent_Click;
            buttonClearAll.Click += ButtonClearAll_Click;
            tableAlarms.CellClick += TableAlarms_CellClick;
            paginationAlarms.ValueChanged += PaginationAlarms_ValueChanged;
        }

        /// <summary>
        /// 初始化分页栏。
        /// 风格与 LoginLogPage 保持一致。
        /// </summary>
        private void InitializePagination()
        {
            paginationAlarms.Current = 1;
            paginationAlarms.Total = 0;
            paginationAlarms.PageSize = 20;
            paginationAlarms.PageSizeOptions = new int[] { 10, 20, 50, 100 };
            paginationAlarms.ShowSizeChanger = true;
            paginationAlarms.SizeChangerWidth = 72;
            paginationAlarms.RightToLeft = RightToLeft.Yes;
            paginationAlarms.Gap = 8;
            paginationAlarms.Radius = 8;
        }

        private void SubscribeAlarmManager()
        {
            if (_alarmManager == null)
            {
                return;
            }

            _alarmManager.AlarmStateChanged -= AlarmManager_AlarmStateChanged;
            _alarmManager.AlarmStateChanged += AlarmManager_AlarmStateChanged;
        }

        private void UnsubscribeAlarmManager()
        {
            if (_alarmManager == null)
            {
                return;
            }

            _alarmManager.AlarmStateChanged -= AlarmManager_AlarmStateChanged;
            _alarmManager = null;
        }

        private void AlarmManager_AlarmStateChanged()
        {
            if (IsDisposed || !IsHandleCreated || !Visible)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(RefreshAlarms));
                }
                catch
                {
                }

                return;
            }

            RefreshAlarms();
        }

        private void PaginationAlarms_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_model == null || e == null)
            {
                return;
            }

            _model.SetPage(e.Current, e.PageSize);
            RebindTable();
            SyncPagination();
            RefreshActionButtons();
        }

        private void ButtonClearCurrent_Click(object sender, EventArgs e)
        {
            if (_model == null)
            {
                return;
            }

            _model.ClearSelected();
            RefreshAlarms();
        }

        private void ButtonClearAll_Click(object sender, EventArgs e)
        {
            if (_model == null)
            {
                return;
            }

            _model.ClearAll();
            RefreshAlarms();
        }

        private void TableAlarms_CellClick(object sender, TableClickEventArgs e)
        {
            if (_model == null)
            {
                return;
            }

            AlarmTableRow row = e == null ? null : e.Record as AlarmTableRow;
            if (row == null || row.Item == null)
            {
                return;
            }

            _model.SelectedItem = row.Item;
            ShowAlarmDetail(_model.SelectedItem);
            RefreshActionButtons();
        }

        private void RebindTable()
        {
            if (_model == null)
            {
                tableAlarms.Binding(new AntList<AlarmTableRow>());
                ShowAlarmDetail(null);
                return;
            }

            IList<CurrentAlarmPageModel.CurrentAlarmItem> pageItems = _model.GetCurrentPageItems();
            AntList<AlarmTableRow> rows = new AntList<AlarmTableRow>();

            foreach (CurrentAlarmPageModel.CurrentAlarmItem item in pageItems)
            {
                rows.Add(new AlarmTableRow
                {
                    Item = item,
                    TimeText = item.TimeText,
                    LevelTag = BuildLevelTag(item.Level),
                    CodeText = item.CodeNumberText,
                    SourceText = item.SourceText,
                    MessageText = new CellText(item.MessageText)
                });
            }

            tableAlarms.Binding(rows);

            if (_model.SelectedItem == null)
            {
                _model.SelectedItem = pageItems.FirstOrDefault();
            }

            ShowAlarmDetail(_model.SelectedItem);
        }

        private void RefreshSummary()
        {
            if (_model == null)
            {
                labelAlarmCount.Text = "0";
                labelSummaryHint.Text = "当前没有活动报警。";
                return;
            }

            labelAlarmCount.Text = _model.TotalCount.ToString();
            labelListSummary.Text = "当前活动报警列表";

            int criticalCount = _model.AllItems.Count(x => x.Level == AlarmLevel.Critical);
            if (criticalCount > 0)
            {
                labelSummaryHint.Text = "存在 " + criticalCount + " 条严重报警，请立即处理。";
                labelSummaryHint.ForeColor = Color.FromArgb(220, 84, 84);
            }
            else
            {
                labelSummaryHint.Text = _model.TotalCount <= 0
                    ? "当前没有活动报警。"
                    : "显示当前未清除报警。";
                labelSummaryHint.ForeColor = Color.FromArgb(90, 90, 90);
            }
        }

        private void SyncPagination()
        {
            if (_model == null)
            {
                return;
            }

            paginationAlarms.Current = _model.PageIndex;
            paginationAlarms.Total = _model.TotalCount;
            paginationAlarms.PageSize = _model.PageSize;
        }

        private void RefreshActionButtons()
        {
            bool hasItems = _model != null && _model.TotalCount > 0;
            bool hasSelected = _model != null && _model.SelectedItem != null && _model.SelectedItem.Alarm != null;

            buttonClearCurrent.Enabled = hasSelected;
            buttonClearAll.Enabled = hasItems;
            buttonRefresh.Enabled = true;
        }

        private void ShowAlarmDetail(CurrentAlarmPageModel.CurrentAlarmItem item)
        {
            if (item == null || item.Alarm == null)
            {
                labelDetailMessage.Text = "-";
                labelDetailMeta.Text = "报警代码：-\r\n报警等级：-\r\n报警来源：-\r\n控制卡号：-\r\n报警时间：-";
                labelDetailDescription.Text = "-";
                labelDetailSuggestion.Text = "-";
                return;
            }

            labelDetailMessage.Text = item.MessageText;
            labelDetailMeta.Text =
                "报警代码： " + item.CodeNumberText + "\r\n" +
                "报警等级： " + item.LevelText + "\r\n" +
                "报警来源： " + item.SourceText + "\r\n" +
                "控制卡号： " + item.CardIdText + "\r\n" +
                "报警时间： " + item.TimeText;
            labelDetailDescription.Text = string.IsNullOrWhiteSpace(item.DescriptionText) ? "-" : item.DescriptionText;
            labelDetailSuggestion.Text = string.IsNullOrWhiteSpace(item.SuggestionText) ? "-" : item.SuggestionText;
        }

        private static CellTag BuildLevelTag(AlarmLevel level)
        {
            switch (level)
            {
                case AlarmLevel.Critical:
                    return new CellTag("Critical", TTypeMini.Error);
                case AlarmLevel.Alarm:
                    return new CellTag("Alarm", TTypeMini.Error);
                case AlarmLevel.Warning:
                    return new CellTag("Warning", TTypeMini.Warn);
                default:
                    return new CellTag("Info", TTypeMini.Default);
            }
        }

        private sealed class AlarmTableRow
        {
            public CurrentAlarmPageModel.CurrentAlarmItem Item { get; set; }

            public string TimeText { get; set; }

            public CellTag LevelTag { get; set; }

            public string CodeText { get; set; }

            public string SourceText { get; set; }

            public CellText MessageText { get; set; }
        }
    }
}