namespace AMControlWinF.Views.AlarmLog
{
    partial class CurrentAlarmPage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.activeAlarmContentControl = new AMControlWinF.Views.AlarmLog.ActiveAlarmContentControl();
            this.panelRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.activeAlarmContentControl);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(1100, 680);
            this.panelRoot.TabIndex = 0;
            // 
            // activeAlarmContentControl
            // 
            this.activeAlarmContentControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activeAlarmContentControl.Location = new System.Drawing.Point(8, 8);
            this.activeAlarmContentControl.Margin = new System.Windows.Forms.Padding(0);
            this.activeAlarmContentControl.Name = "activeAlarmContentControl";
            this.activeAlarmContentControl.Size = new System.Drawing.Size(1084, 664);
            this.activeAlarmContentControl.TabIndex = 0;
            // 
            // CurrentAlarmPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CurrentAlarmPage";
            this.Size = new System.Drawing.Size(1100, 680);
            this.panelRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panelRoot;
        private ActiveAlarmContentControl activeAlarmContentControl;
    }
}