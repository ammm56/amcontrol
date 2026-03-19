using AM.Core.Context;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Alarm
{
    /// <summary>
    /// CurrentAlarmView.xaml 的交互逻辑
    /// 复用 ActiveAlarmPanelControl 来显示当前活动的报警列表。
    /// </summary>
    public partial class CurrentAlarmView : UserControl
    {
        public CurrentAlarmView()
        {
            InitializeComponent();
            Loaded += CurrentAlarmView_Loaded;
        }

        private void CurrentAlarmView_Loaded(object sender, RoutedEventArgs e)
        {
            ActiveAlarmPanelControl.BindAlarmManager(SystemContext.Instance.AlarmManager);
            ActiveAlarmPanelControl.RefreshAlarms();
        }
    }
}