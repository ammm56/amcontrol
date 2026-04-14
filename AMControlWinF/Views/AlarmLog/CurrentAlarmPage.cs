using AM.Core.Context;
using AM.PageModel.AlarmLog;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.AlarmLog
{
    /// <summary>
    /// 当前报警页面。
    /// 导航页版本，不承担 Drawer 关闭行为。
    /// </summary>
    public partial class CurrentAlarmPage : UserControl
    {
        private readonly CurrentAlarmPageModel _model;
        private bool _isFirstLoad;

        public CurrentAlarmPage()
        {
            InitializeComponent();

            _model = new CurrentAlarmPageModel();

            BindEvents();
            BindModel();
        }

        private void BindEvents()
        {
            Load += CurrentAlarmPage_Load;
            VisibleChanged += CurrentAlarmPage_VisibleChanged;
        }

        private void BindModel()
        {
            activeAlarmContentControl.BindModel(_model);
        }

        private void CurrentAlarmPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;

            _model.BindAlarmManager(SystemContext.Instance.AlarmManager);
            activeAlarmContentControl.BindAlarmManager(SystemContext.Instance.AlarmManager);
            activeAlarmContentControl.RefreshAlarms();
        }

        private void CurrentAlarmPage_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible || !_isFirstLoad)
            {
                return;
            }

            activeAlarmContentControl.RefreshAlarms();
        }
    }
}