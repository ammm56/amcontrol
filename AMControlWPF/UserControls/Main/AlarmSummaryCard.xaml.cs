using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Messaging;
using AM.Model.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.UserControls.Main
{
    /// <summary>
    /// AlarmSummaryCard.xaml 的交互逻辑
    /// 轻量摘要控件：首页/监视页复用
    /// </summary>
    public partial class AlarmSummaryCard : UserControl
    {
        private readonly List<RecentAlarmDisplayItem> _recentAlarmItems;
        private AlarmManager _alarmManager;

        public AlarmSummaryCard()
        {
            InitializeComponent();
            _recentAlarmItems = new List<RecentAlarmDisplayItem>();

            Loaded += AlarmSummaryCard_Loaded;
            Unloaded += AlarmSummaryCard_Unloaded;
        }

        public event EventHandler OpenRequested;

        public void BindAlarmManager(AlarmManager alarmManager)
        {
            _alarmManager = alarmManager;
            RefreshSummary();
        }

        public void RefreshSummary()
        {
            var alarms = _alarmManager == null
                ? new List<AlarmInfo>()
                : _alarmManager.GetActiveAlarms()
                    .OrderByDescending(x => x.Time)
                    .ToList();

            var criticalCount = alarms.Count(x => x.Level == AlarmLevel.Critical);

            TextBlockAlarmCount.Text = alarms.Count.ToString();
            TextBlockCriticalCount.Text = criticalCount.ToString();

            _recentAlarmItems.Clear();

            foreach (var alarm in alarms.Take(5))
            {
                _recentAlarmItems.Add(new RecentAlarmDisplayItem
                {
                    Message = alarm.Message,
                    SummaryText = alarm.Level + " · " + (string.IsNullOrWhiteSpace(alarm.Source) ? "-" : alarm.Source) + " · " + alarm.Time.ToString("HH:mm:ss")
                });
            }

            ItemsControlRecentAlarms.ItemsSource = null;
            ItemsControlRecentAlarms.ItemsSource = _recentAlarmItems;
            TextBlockEmptyState.Visibility = _recentAlarmItems.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AlarmSummaryCard_Loaded(object sender, RoutedEventArgs e)
        {
            SubscribeSystemMessages();

            if (_alarmManager == null)
            {
                _alarmManager = SystemContext.Instance.AlarmManager;
            }

            RefreshSummary();
        }

        private void AlarmSummaryCard_Unloaded(object sender, RoutedEventArgs e)
        {
            SystemContext.Instance.MessageBus?.Unsubscribe(this);
        }

        private void SubscribeSystemMessages()
        {
            SystemContext.Instance.MessageBus?.Unsubscribe(this);
            SystemContext.Instance.MessageBus?.Subscribe(this, OnSystemMessageReceived);
        }

        private void OnSystemMessageReceived(SystemMessage message)
        {
            if (message == null || message.Type != SystemMessageType.Alarm)
            {
                return;
            }

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() => OnSystemMessageReceived(message)));
                return;
            }

            RefreshSummary();
        }

        private void ButtonOpen_OnClick(object sender, RoutedEventArgs e)
        {
            OpenRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshSummary();
        }

        private sealed class RecentAlarmDisplayItem
        {
            public string Message { get; set; }

            public string SummaryText { get; set; }
        }
    }
}