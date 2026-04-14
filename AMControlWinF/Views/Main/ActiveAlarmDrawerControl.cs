using AM.Core.Alarm;
using AM.Model.Alarm;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 活动报警抽屉面板。
    /// 职责：
    /// 1. 展示当前未清除报警列表；
    /// 2. 支持分页、刷新、清除当前、全部清除；
    /// 3. 展示选中报警详情；
    /// 4. 通过 AlarmManager 状态变化事件实时刷新。
    /// </summary>
    public partial class ActiveAlarmDrawerControl : UserControl
    {
        private const int PageSize = 20;

        private readonly List<AlarmDisplayItem> _allAlarmItems;

        private AlarmManager _alarmManager;
        private AlarmDisplayItem _selectedItem;
        private int _currentPageIndex;
        private bool _isRefreshing;

        public ActiveAlarmDrawerControl()
        {
            InitializeComponent();

            _allAlarmItems = new List<AlarmDisplayItem>();
            _currentPageIndex = 1;

            InitializeTableColumns();
            BindEvents();
            ShowAlarmDetail(null);

            Disposed += (s, e) => UnsubscribeAlarmManager();
        }

        /// <summary>
        /// 当前活动报警数量。
        /// </summary>
        public int ActiveAlarmCount
        {
            get { return _allAlarmItems.Count; }
        }

        /// <summary>
        /// 绑定报警管理器。
        /// </summary>
        /// <param name="alarmManager">报警管理器。</param>
        public void BindAlarmManager(AlarmManager alarmManager)
        {
            if (ReferenceEquals(_alarmManager, alarmManager))
            {
                return;
            }

            UnsubscribeAlarmManager();

            _alarmManager = alarmManager;
            SubscribeAlarmManager();
        }

        /// <summary>
        /// 刷新活动报警数据。
        /// </summary>
        public void RefreshAlarms()
        {
            if (_isRefreshing)
            {
                return;
            }

            _isRefreshing = true;
            try
            {
                string preferredKey = _selectedItem == null ? string.Empty : _selectedItem.UniqueKey;

                List<AlarmDisplayItem> items = _alarmManager == null
                    ? new List<AlarmDisplayItem>()
                    : _alarmManager.GetActiveAlarms()
                        .Where(x => x != null)
                        .OrderByDescending(x => x.Time)
                        .Select(x => new AlarmDisplayItem(x))
                        .ToList();

                _allAlarmItems.Clear();
                _allAlarmItems.AddRange(items);

                EnsureValidPageIndex();
                RebindTable(preferredKey);
                RefreshSummary();
                RefreshPagingState();
                RefreshActionButtons();
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        /// <summary>
        /// 初始化表格列。
        /// </summary>
        private void InitializeTableColumns()
        {
            tableAlarms.Columns = new ColumnCollection()
            {
                new Column("TimeText", "时间", ColumnAlign.Center)
                {
                    Width = "140",
                    Fixed = true
                },
                new Column("LevelTag", "等级", ColumnAlign.Center)
                {
                    Width = "88"
                },
                new Column("CodeText", "代码", ColumnAlign.Center)
                {
                    Width = "72"
                },
                new Column("SourceText", "来源", ColumnAlign.Left)
                {
                    Width = "132"
                },
                new Column("MessageText", "消息", ColumnAlign.Left)
                {
                    LineBreak = true
                }
            };
        }

        /// <summary>
        /// 绑定页面事件。
        /// </summary>
        private void BindEvents()
        {
            buttonRefresh.Click += (s, e) => RefreshAlarms();
            buttonClearCurrent.Click += ButtonClearCurrent_Click;
            buttonClearAll.Click += ButtonClearAll_Click;
            buttonPrevPage.Click += ButtonPrevPage_Click;
            buttonNextPage.Click += ButtonNextPage_Click;
            tableAlarms.CellClick += TableAlarms_CellClick;
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
                    BeginInvoke(new Action(() =>
                    {
                        if (!IsDisposed && IsHandleCreated && Visible)
                        {
                            RefreshAlarms();
                        }
                    }));
                }
                catch
                {
                }

                return;
            }

            RefreshAlarms();
        }

        /// <summary>
        /// 刷新顶部摘要区。
        /// </summary>
        private void RefreshSummary()
        {
            labelAlarmCount.Text = _allAlarmItems.Count.ToString();

            int criticalCount = _allAlarmItems.Count(x => x.Level == AlarmLevel.Critical);
            if (criticalCount > 0)
            {
                labelSummaryHint.Text = "存在 " + criticalCount + " 条严重报警，请立即处理。";
                labelSummaryHint.ForeColor = Color.FromArgb(220, 84, 84);
            }
            else
            {
                labelSummaryHint.Text = _allAlarmItems.Count <= 0
                    ? "当前没有活动报警。"
                    : "显示当前未清除报警。";
                labelSummaryHint.ForeColor = Color.FromArgb(90, 90, 90);
            }
        }

        /// <summary>
        /// 重新绑定当前页表格。
        /// </summary>
        /// <param name="preferredKey">优先保持选中的报警唯一键。</param>
        private void RebindTable(string preferredKey)
        {
            List<AlarmDisplayItem> pageItems = GetCurrentPageItems();

            var rows = new AntList<AlarmTableRow>();
            foreach (AlarmDisplayItem item in pageItems)
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

            _selectedItem = pageItems.FirstOrDefault(x =>
                string.Equals(x.UniqueKey, preferredKey, StringComparison.Ordinal))
                ?? pageItems.FirstOrDefault();

            ShowAlarmDetail(_selectedItem);
        }

        /// <summary>
        /// 获取当前页数据。
        /// </summary>
        private List<AlarmDisplayItem> GetCurrentPageItems()
        {
            if (_allAlarmItems.Count <= 0)
            {
                return new List<AlarmDisplayItem>();
            }

            int skip = (_currentPageIndex - 1) * PageSize;
            return _allAlarmItems
                .Skip(skip)
                .Take(PageSize)
                .ToList();
        }

        /// <summary>
        /// 确保页码处于合法区间。
        /// </summary>
        private void EnsureValidPageIndex()
        {
            int totalPages = GetTotalPages();

            if (totalPages <= 0)
            {
                _currentPageIndex = 1;
                return;
            }

            if (_currentPageIndex < 1)
            {
                _currentPageIndex = 1;
            }

            if (_currentPageIndex > totalPages)
            {
                _currentPageIndex = totalPages;
            }
        }

        private int GetTotalPages()
        {
            if (_allAlarmItems.Count <= 0)
            {
                return 0;
            }

            return (int)Math.Ceiling((double)_allAlarmItems.Count / PageSize);
        }

        /// <summary>
        /// 刷新底部分页状态。
        /// </summary>
        private void RefreshPagingState()
        {
            int totalCount = _allAlarmItems.Count;
            int totalPages = GetTotalPages();

            if (totalCount <= 0)
            {
                labelPageRange.Text = "0 / 0";
                labelPageInfo.Text = "第 0 / 0 页";
                buttonPrevPage.Enabled = false;
                buttonNextPage.Enabled = false;
                return;
            }

            int startIndex = ((_currentPageIndex - 1) * PageSize) + 1;
            int endIndex = Math.Min(_currentPageIndex * PageSize, totalCount);

            labelPageRange.Text = startIndex + "-" + endIndex + " / " + totalCount;
            labelPageInfo.Text = "第 " + _currentPageIndex + " / " + totalPages + " 页";
            buttonPrevPage.Enabled = _currentPageIndex > 1;
            buttonNextPage.Enabled = _currentPageIndex < totalPages;
        }

        /// <summary>
        /// 刷新按钮可用性。
        /// </summary>
        private void RefreshActionButtons()
        {
            bool hasItems = _allAlarmItems.Count > 0;
            bool hasSelected = _selectedItem != null && _selectedItem.Alarm != null;

            buttonClearCurrent.Enabled = hasSelected;
            buttonClearAll.Enabled = hasItems;
            buttonRefresh.Enabled = true;
        }

        private void TableAlarms_CellClick(object sender, TableClickEventArgs e)
        {
            AlarmTableRow row = e == null ? null : e.Record as AlarmTableRow;
            if (row == null || row.Item == null)
            {
                return;
            }

            _selectedItem = row.Item;
            ShowAlarmDetail(_selectedItem);
            RefreshActionButtons();
        }

        private void ButtonPrevPage_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex <= 1)
            {
                return;
            }

            _currentPageIndex--;
            RebindTable(string.Empty);
            RefreshPagingState();
            RefreshActionButtons();
        }

        private void ButtonNextPage_Click(object sender, EventArgs e)
        {
            int totalPages = GetTotalPages();
            if (_currentPageIndex >= totalPages)
            {
                return;
            }

            _currentPageIndex++;
            RebindTable(string.Empty);
            RefreshPagingState();
            RefreshActionButtons();
        }

        private void ButtonClearCurrent_Click(object sender, EventArgs e)
        {
            if (_alarmManager == null || _selectedItem == null || _selectedItem.Alarm == null)
            {
                return;
            }

            _alarmManager.ClearAlarm(_selectedItem.Alarm);
            RefreshAlarms();
        }

        private void ButtonClearAll_Click(object sender, EventArgs e)
        {
            if (_alarmManager == null || _allAlarmItems.Count <= 0)
            {
                return;
            }

            List<AlarmInfo> items = _allAlarmItems
                .Select(x => x.Alarm)
                .Where(x => x != null)
                .ToList();

            foreach (AlarmInfo item in items)
            {
                _alarmManager.ClearAlarm(item);
            }

            RefreshAlarms();
        }

        /// <summary>
        /// 绑定右侧报警详情区。
        /// </summary>
        /// <param name="item">报警显示项。</param>
        private void ShowAlarmDetail(AlarmDisplayItem item)
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

        /// <summary>
        /// 抽屉内部显示项。
        /// 保留 AlarmInfo 原始引用，便于直接执行清除。
        /// </summary>
        private sealed class AlarmDisplayItem
        {
            public AlarmDisplayItem(AlarmInfo alarm)
            {
                Alarm = alarm;
            }

            public AlarmInfo Alarm { get; private set; }

            public string UniqueKey
            {
                get
                {
                    return CodeNumberText + "|" + TimeText + "|" + (MessageText ?? string.Empty);
                }
            }

            public AlarmLevel Level
            {
                get { return Alarm == null ? AlarmLevel.Info : Alarm.Level; }
            }

            public string CodeNumberText
            {
                get { return Alarm == null ? "-" : ((int)Alarm.Code).ToString(); }
            }

            public string LevelText
            {
                get { return Alarm == null ? "-" : Alarm.Level.ToString(); }
            }

            public string MessageText
            {
                get { return Alarm == null ? "-" : (Alarm.Message ?? string.Empty); }
            }

            public string SourceText
            {
                get { return Alarm == null || string.IsNullOrWhiteSpace(Alarm.Source) ? "-" : Alarm.Source; }
            }

            public string CardIdText
            {
                get { return Alarm == null || !Alarm.CardId.HasValue ? "-" : Alarm.CardId.Value.ToString(); }
            }

            public string TimeText
            {
                get { return Alarm == null ? "-" : Alarm.Time.ToString("yyyy-MM-dd HH:mm:ss"); }
            }

            public string DescriptionText
            {
                get { return Alarm == null ? string.Empty : (Alarm.Description ?? string.Empty); }
            }

            public string SuggestionText
            {
                get { return Alarm == null ? string.Empty : (Alarm.Suggestion ?? string.Empty); }
            }
        }

        /// <summary>
        /// 表格行模型。
        /// </summary>
        private sealed class AlarmTableRow
        {
            public AlarmDisplayItem Item { get; set; }

            public string TimeText { get; set; }

            public CellTag LevelTag { get; set; }

            public string CodeText { get; set; }

            public string SourceText { get; set; }

            public CellText MessageText { get; set; }
        }
    }
}