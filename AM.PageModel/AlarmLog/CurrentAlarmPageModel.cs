using AM.Core.Alarm;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.PageModel.AlarmLog
{
    /// <summary>
    /// 当前报警页面模型。
    /// 负责：
    /// 1. 绑定 AlarmManager；
    /// 2. 维护当前活动报警集合；
    /// 3. 提供分页、选中、清除当前、全部清除能力；
    /// 4. 为 WinForms 页面 / Drawer 共用内容控件提供统一数据入口。
    /// </summary>
    public class CurrentAlarmPageModel : BindableBase
    {
        private const int DefaultPageSize = 20;

        private AlarmManager _alarmManager;
        private List<CurrentAlarmItem> _allItems;
        private CurrentAlarmItem _selectedItem;
        private int _pageIndex;
        private int _pageSize;
        private int _totalCount;

        public CurrentAlarmPageModel()
        {
            _allItems = new List<CurrentAlarmItem>();
            _pageIndex = 1;
            _pageSize = DefaultPageSize;
        }

        /// <summary>
        /// 当前全部活动报警。
        /// </summary>
        public IList<CurrentAlarmItem> AllItems
        {
            get { return _allItems; }
        }

        /// <summary>
        /// 当前选中报警。
        /// </summary>
        public CurrentAlarmItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        /// <summary>
        /// 当前页码。
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            private set { SetProperty(ref _pageIndex, value <= 0 ? 1 : value); }
        }

        /// <summary>
        /// 当前分页大小。
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            private set { SetProperty(ref _pageSize, value <= 0 ? DefaultPageSize : value); }
        }

        /// <summary>
        /// 当前活动报警总数。
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 总页数。
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (PageSize <= 0)
                {
                    return 1;
                }

                int pages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                return pages <= 0 ? 1 : pages;
            }
        }

        /// <summary>
        /// 绑定报警管理器。
        /// </summary>
        public void BindAlarmManager(AlarmManager alarmManager)
        {
            _alarmManager = alarmManager;
        }

        /// <summary>
        /// 刷新当前活动报警。
        /// </summary>
        public Result Refresh()
        {
            try
            {
                string preferredKey = SelectedItem == null ? string.Empty : SelectedItem.UniqueKey;

                List<CurrentAlarmItem> items = _alarmManager == null
                    ? new List<CurrentAlarmItem>()
                    : _alarmManager.GetActiveAlarms()
                        .Where(x => x != null)
                        .OrderByDescending(x => x.Time)
                        .Select(x => new CurrentAlarmItem(x))
                        .ToList();

                _allItems = items;
                TotalCount = _allItems.Count;

                EnsureValidPageIndex();

                SelectedItem = _allItems.FirstOrDefault(x =>
                    string.Equals(x.UniqueKey, preferredKey, StringComparison.Ordinal))
                    ?? GetCurrentPageItems().FirstOrDefault();

                OnPropertyChanged(nameof(AllItems));
                OnPropertyChanged(nameof(TotalPages));

                return Result.Ok("当前报警刷新成功", ResultSource.UI);
            }
            catch (Exception ex)
            {
                return Result.Fail(-1, "当前报警刷新失败: " + ex.Message, ResultSource.UI);
            }
        }

        /// <summary>
        /// 设置分页。
        /// </summary>
        public void SetPage(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex <= 0 ? 1 : pageIndex;
            PageSize = pageSize <= 0 ? PageSize : pageSize;

            EnsureValidPageIndex();
            OnPropertyChanged(nameof(TotalPages));
        }

        /// <summary>
        /// 获取当前页数据。
        /// </summary>
        public IList<CurrentAlarmItem> GetCurrentPageItems()
        {
            if (_allItems.Count <= 0)
            {
                return new List<CurrentAlarmItem>();
            }

            int skip = (PageIndex - 1) * PageSize;
            return _allItems
                .Skip(skip)
                .Take(PageSize)
                .ToList();
        }

        /// <summary>
        /// 清除当前选中报警。
        /// </summary>
        public Result ClearSelected()
        {
            if (_alarmManager == null || SelectedItem == null || SelectedItem.Alarm == null)
            {
                return Result.Fail(-1, "当前未选择可清除的报警", ResultSource.UI);
            }

            _alarmManager.ClearAlarm(SelectedItem.Alarm);
            return Refresh();
        }

        /// <summary>
        /// 清除全部活动报警。
        /// </summary>
        public Result ClearAll()
        {
            if (_alarmManager == null || _allItems.Count <= 0)
            {
                return Result.Ok("当前没有可清除的活动报警", ResultSource.UI);
            }

            List<AlarmInfo> alarms = _allItems
                .Select(x => x.Alarm)
                .Where(x => x != null)
                .ToList();

            foreach (AlarmInfo alarm in alarms)
            {
                _alarmManager.ClearAlarm(alarm);
            }

            return Refresh();
        }

        private void EnsureValidPageIndex()
        {
            int totalPages = TotalPages;

            if (totalPages <= 0)
            {
                PageIndex = 1;
                return;
            }

            if (PageIndex < 1)
            {
                PageIndex = 1;
            }

            if (PageIndex > totalPages)
            {
                PageIndex = totalPages;
            }
        }

        /// <summary>
        /// 当前报警显示项。
        /// </summary>
        public sealed class CurrentAlarmItem
        {
            public CurrentAlarmItem(AlarmInfo alarm)
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
    }
}