using AM.Core.Alarm;
using AM.Core.Context;
using AM.Model.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.UserControls.Main
{
    /// <summary>
    /// ActiveAlarmPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ActiveAlarmPanel : UserControl
    {
        private const int PageSize = 20;

        private readonly List<AlarmDisplayItem> _allAlarmItems;
        private AlarmManager _alarmManager;
        private int _currentPageIndex;

        public ActiveAlarmPanel()
        {
            InitializeComponent();
            _allAlarmItems = new List<AlarmDisplayItem>();
            _currentPageIndex = 1;

            Loaded += ActiveAlarmPanel_Loaded;
            Unloaded += ActiveAlarmPanel_Unloaded;
        }

        public event Action<int> AlarmCountChanged;

        public int ActiveAlarmCount
        {
            get { return _allAlarmItems.Count; }
        }

        public bool HasCriticalAlarm
        {
            get { return _allAlarmItems.Any(x => x.Level == AlarmLevel.Critical); }
        }

        public void BindAlarmManager(AlarmManager alarmManager)
        {
            UnsubscribeAlarmManager();

            _alarmManager = alarmManager;

            SubscribeAlarmManager();
            RefreshAlarms();
        }

        public void RefreshAlarms()
        {
            var selectedKey = GetSelectedAlarmKey();

            _allAlarmItems.Clear();

            var alarms = _alarmManager == null
                ? new List<AlarmInfo>()
                : _alarmManager.GetActiveAlarms()
                    .OrderByDescending(x => x.Time)
                    .ToList();

            foreach (var alarm in alarms)
            {
                _allAlarmItems.Add(new AlarmDisplayItem
                {
                    Alarm = alarm,
                    CodeText = alarm.Code.ToString(),
                    Level = alarm.Level,
                    LevelText = alarm.Level.ToString(),
                    Message = alarm.Message,
                    Time = alarm.Time,
                    TimeText = alarm.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    Source = alarm.Source,
                    SourceDisplay = string.IsNullOrWhiteSpace(alarm.Source) ? "-" : alarm.Source,
                    CardId = alarm.CardId,
                    CardIdText = alarm.CardId.HasValue ? alarm.CardId.Value.ToString() : "-",
                    Description = alarm.Description,
                    Suggestion = alarm.Suggestion
                });
            }

            TextBlockAlarmCount.Text = _allAlarmItems.Count.ToString();
            UpdateCriticalBanner();
            EnsureValidPageIndex();
            BindCurrentPage(selectedKey);

            AlarmCountChanged?.Invoke(_allAlarmItems.Count);
        }

        private void ActiveAlarmPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (_alarmManager == null)
            {
                _alarmManager = SystemContext.Instance.AlarmManager;
            }

            SubscribeAlarmManager();
            RefreshAlarms();
        }

        private void ActiveAlarmPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribeAlarmManager();
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
        }

        private void AlarmManager_AlarmStateChanged()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(RefreshAlarms));
                return;
            }

            RefreshAlarms();
        }

        private void UpdateCriticalBanner()
        {
            var criticalCount = _allAlarmItems.Count(x => x.Level == AlarmLevel.Critical);
            if (criticalCount <= 0)
            {
                BorderCriticalBanner.Visibility = Visibility.Collapsed;
                TextBlockCriticalBanner.Text = "存在高等级报警，请立即处理。";
                return;
            }

            BorderCriticalBanner.Visibility = Visibility.Visible;
            TextBlockCriticalBanner.Text = "存在 " + criticalCount + " 条严重报警，请立即处理。";
        }

        private void BindCurrentPage(string preferredSelectedKey)
        {
            if (_allAlarmItems.Count == 0)
            {
                DataGridActiveAlarms.ItemsSource = null;
                UpdatePagingState(0, 0, 0);
                ShowAlarmDetail(null);
                return;
            }

            var skip = (_currentPageIndex - 1) * PageSize;
            var pageItems = _allAlarmItems
                .Skip(skip)
                .Take(PageSize)
                .ToList();

            DataGridActiveAlarms.ItemsSource = null;
            DataGridActiveAlarms.ItemsSource = pageItems;

            var selectedItem = pageItems.FirstOrDefault(x => x.UniqueKey == preferredSelectedKey)
                               ?? pageItems.FirstOrDefault();

            DataGridActiveAlarms.SelectedItem = selectedItem;

            if (selectedItem != null)
            {
                DataGridActiveAlarms.ScrollIntoView(selectedItem);
            }

            UpdatePagingState(skip + 1, skip + pageItems.Count, _allAlarmItems.Count);
            ShowAlarmDetail(selectedItem);
        }

        private void EnsureValidPageIndex()
        {
            var totalPages = GetTotalPages();

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
            if (_allAlarmItems.Count == 0)
            {
                return 0;
            }

            return (int)Math.Ceiling((double)_allAlarmItems.Count / PageSize);
        }

        private void UpdatePagingState(int startIndex, int endIndex, int totalCount)
        {
            var totalPages = GetTotalPages();

            TextBlockPageInfo.Text = totalPages <= 0
                ? "第 0 / 0 页"
                : "第 " + _currentPageIndex + " / " + totalPages + " 页";

            TextBlockPageRange.Text = totalCount <= 0
                ? "0 / 0"
                : startIndex + "-" + endIndex + " / " + totalCount;

            ButtonPrevPage.IsEnabled = _currentPageIndex > 1;
            ButtonNextPage.IsEnabled = totalPages > 0 && _currentPageIndex < totalPages;
        }

        private string GetSelectedAlarmKey()
        {
            var item = DataGridActiveAlarms.SelectedItem as AlarmDisplayItem;
            return item == null ? string.Empty : item.UniqueKey;
        }

        private void DataGridActiveAlarms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAlarmDetail(DataGridActiveAlarms.SelectedItem as AlarmDisplayItem);
        }

        private void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshAlarms();
        }

        private void ButtonPrevPage_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentPageIndex <= 1)
            {
                return;
            }

            _currentPageIndex--;
            BindCurrentPage(string.Empty);
        }

        private void ButtonNextPage_OnClick(object sender, RoutedEventArgs e)
        {
            if (_currentPageIndex >= GetTotalPages())
            {
                return;
            }

            _currentPageIndex++;
            BindCurrentPage(string.Empty);
        }

        private void ButtonClearCurrent_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = DataGridActiveAlarms.SelectedItem as AlarmDisplayItem;
            if (selectedItem == null || selectedItem.Alarm == null || _alarmManager == null)
            {
                return;
            }

            _alarmManager.ClearAlarm(selectedItem.Alarm);
            RefreshAlarms();
        }

        private void ButtonClearAll_OnClick(object sender, RoutedEventArgs e)
        {
            if (_alarmManager == null || _allAlarmItems.Count == 0)
            {
                return;
            }

            var items = _allAlarmItems
                .Select(x => x.Alarm)
                .Where(x => x != null)
                .ToList();

            foreach (var item in items)
            {
                _alarmManager.ClearAlarm(item);
            }

            RefreshAlarms();
        }

        private void ShowAlarmDetail(AlarmDisplayItem item)
        {
            if (item == null)
            {
                TextBlockAlarmDetailMessage.Text = "-";
                TextBlockAlarmDetailCode.Text = "报警代码：-";
                TextBlockAlarmDetailLevel.Text = "报警等级：-";
                TextBlockAlarmDetailSource.Text = "报警来源：-";
                TextBlockAlarmDetailCardId.Text = "控制卡号：-";
                TextBlockAlarmDetailTime.Text = "报警时间：-";
                TextBlockAlarmDetailDescription.Text = "-";
                TextBlockAlarmDetailSuggestion.Text = "-";
                return;
            }

            TextBlockAlarmDetailMessage.Text = string.IsNullOrWhiteSpace(item.Message) ? "-" : item.Message;
            TextBlockAlarmDetailCode.Text = "报警代码：" + item.CodeText;
            TextBlockAlarmDetailLevel.Text = "报警等级：" + item.LevelText;
            TextBlockAlarmDetailSource.Text = "报警来源：" + item.SourceDisplay;
            TextBlockAlarmDetailCardId.Text = "控制卡号：" + item.CardIdText;
            TextBlockAlarmDetailTime.Text = "报警时间：" + item.TimeText;
            TextBlockAlarmDetailDescription.Text = string.IsNullOrWhiteSpace(item.Description) ? "-" : item.Description;
            TextBlockAlarmDetailSuggestion.Text = string.IsNullOrWhiteSpace(item.Suggestion) ? "-" : item.Suggestion;
        }

        private sealed class AlarmDisplayItem
        {
            public AlarmInfo Alarm { get; set; }

            public string CodeText { get; set; }

            public AlarmLevel Level { get; set; }

            public string LevelText { get; set; }

            public string Message { get; set; }

            public DateTime Time { get; set; }

            public string TimeText { get; set; }

            public string Source { get; set; }

            public string SourceDisplay { get; set; }

            public short? CardId { get; set; }

            public string CardIdText { get; set; }

            public string Description { get; set; }

            public string Suggestion { get; set; }

            public string UniqueKey
            {
                get
                {
                    return CodeText + "|" + Message + "|" + Time.Ticks;
                }
            }
        }
    }
}