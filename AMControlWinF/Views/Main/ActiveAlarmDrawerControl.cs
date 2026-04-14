using AM.Core.Alarm;
using AM.PageModel.AlarmLog;
using AMControlWinF.Views.AlarmLog;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 活动报警抽屉面板。
    /// 仅负责：
    /// 1. 承载可复用的当前报警内容控件；
    /// 2. 为 Drawer 场景提供 Esc 关闭行为；
    /// 3. 对外转发 BindAlarmManager / RefreshAlarms。
    /// </summary>
    public partial class ActiveAlarmDrawerControl : UserControl
    {
        private readonly CurrentAlarmPageModel _model;

        public ActiveAlarmDrawerControl()
        {
            InitializeComponent();

            _model = new CurrentAlarmPageModel();
            activeAlarmContentControl.BindModel(_model);
        }

        /// <summary>
        /// 绑定报警管理器。
        /// </summary>
        public void BindAlarmManager(AlarmManager alarmManager)
        {
            _model.BindAlarmManager(alarmManager);
            activeAlarmContentControl.BindAlarmManager(alarmManager);
        }

        /// <summary>
        /// 刷新当前报警内容。
        /// </summary>
        public void RefreshAlarms()
        {
            activeAlarmContentControl.RefreshAlarms();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                CloseDrawer();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CloseDrawer()
        {
            var hostForm = FindForm();
            if (hostForm != null && !hostForm.IsDisposed)
            {
                hostForm.Close();
            }
        }
    }
}