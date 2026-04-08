namespace AMControlWinF.Views.Motion
{
    partial class MotionActuatorActionPanelControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRoot = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelNormalActions = new AntdUI.Panel();
            this.buttonSecondary = new AntdUI.Button();
            this.buttonPrimary = new AntdUI.Button();
            this.panelStackLightActions = new AntdUI.Panel();
            this.buttonStateAlarm = new AntdUI.Button();
            this.buttonStateWarning = new AntdUI.Button();
            this.buttonStateRunning = new AntdUI.Button();
            this.buttonStateIdle = new AntdUI.Button();
            this.buttonStateOff = new AntdUI.Button();
            this.panelRoot.SuspendLayout();
            this.panelNormalActions.SuspendLayout();
            this.panelStackLightActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.panelStackLightActions);
            this.panelRoot.Controls.Add(this.panelNormalActions);
            this.panelRoot.Controls.Add(this.labelSubTitle);
            this.panelRoot.Controls.Add(this.labelTitle);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(12);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(320, 196);
            this.panelRoot.TabIndex = 0;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSubTitle.Location = new System.Drawing.Point(16, 40);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(288, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "—";
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(16, 16);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(288, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "当前对象：未选择";
            // 
            // panelNormalActions
            // 
            this.panelNormalActions.Controls.Add(this.buttonSecondary);
            this.panelNormalActions.Controls.Add(this.buttonPrimary);
            this.panelNormalActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNormalActions.Location = new System.Drawing.Point(16, 62);
            this.panelNormalActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelNormalActions.Name = "panelNormalActions";
            this.panelNormalActions.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.panelNormalActions.Radius = 0;
            this.panelNormalActions.Size = new System.Drawing.Size(288, 64);
            this.panelNormalActions.TabIndex = 2;
            // 
            // buttonSecondary
            // 
            this.buttonSecondary.Location = new System.Drawing.Point(148, 12);
            this.buttonSecondary.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSecondary.Name = "buttonSecondary";
            this.buttonSecondary.Radius = 8;
            this.buttonSecondary.Size = new System.Drawing.Size(132, 36);
            this.buttonSecondary.TabIndex = 1;
            this.buttonSecondary.Text = "副操作";
            this.buttonSecondary.Type = AntdUI.TTypeMini.Default;
            this.buttonSecondary.WaveSize = 0;
            // 
            // buttonPrimary
            // 
            this.buttonPrimary.Location = new System.Drawing.Point(0, 12);
            this.buttonPrimary.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPrimary.Name = "buttonPrimary";
            this.buttonPrimary.Radius = 8;
            this.buttonPrimary.Size = new System.Drawing.Size(132, 36);
            this.buttonPrimary.TabIndex = 0;
            this.buttonPrimary.Text = "主操作";
            this.buttonPrimary.Type = AntdUI.TTypeMini.Primary;
            this.buttonPrimary.WaveSize = 0;
            // 
            // panelStackLightActions
            // 
            this.panelStackLightActions.Controls.Add(this.buttonStateAlarm);
            this.panelStackLightActions.Controls.Add(this.buttonStateWarning);
            this.panelStackLightActions.Controls.Add(this.buttonStateRunning);
            this.panelStackLightActions.Controls.Add(this.buttonStateIdle);
            this.panelStackLightActions.Controls.Add(this.buttonStateOff);
            this.panelStackLightActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStackLightActions.Location = new System.Drawing.Point(16, 126);
            this.panelStackLightActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelStackLightActions.Name = "panelStackLightActions";
            this.panelStackLightActions.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelStackLightActions.Radius = 0;
            this.panelStackLightActions.Size = new System.Drawing.Size(288, 54);
            this.panelStackLightActions.TabIndex = 3;
            this.panelStackLightActions.Visible = false;
            // 
            // buttonStateAlarm
            // 
            this.buttonStateAlarm.Location = new System.Drawing.Point(232, 8);
            this.buttonStateAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateAlarm.Name = "buttonStateAlarm";
            this.buttonStateAlarm.Radius = 8;
            this.buttonStateAlarm.Size = new System.Drawing.Size(56, 32);
            this.buttonStateAlarm.TabIndex = 4;
            this.buttonStateAlarm.Text = "报警";
            this.buttonStateAlarm.Type = AntdUI.TTypeMini.Primary;
            this.buttonStateAlarm.WaveSize = 0;
            // 
            // buttonStateWarning
            // 
            this.buttonStateWarning.Location = new System.Drawing.Point(174, 8);
            this.buttonStateWarning.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateWarning.Name = "buttonStateWarning";
            this.buttonStateWarning.Radius = 8;
            this.buttonStateWarning.Size = new System.Drawing.Size(56, 32);
            this.buttonStateWarning.TabIndex = 3;
            this.buttonStateWarning.Text = "警告";
            this.buttonStateWarning.Type = AntdUI.TTypeMini.Default;
            this.buttonStateWarning.WaveSize = 0;
            // 
            // buttonStateRunning
            // 
            this.buttonStateRunning.Location = new System.Drawing.Point(116, 8);
            this.buttonStateRunning.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateRunning.Name = "buttonStateRunning";
            this.buttonStateRunning.Radius = 8;
            this.buttonStateRunning.Size = new System.Drawing.Size(56, 32);
            this.buttonStateRunning.TabIndex = 2;
            this.buttonStateRunning.Text = "运行";
            this.buttonStateRunning.Type = AntdUI.TTypeMini.Default;
            this.buttonStateRunning.WaveSize = 0;
            // 
            // buttonStateIdle
            // 
            this.buttonStateIdle.Location = new System.Drawing.Point(58, 8);
            this.buttonStateIdle.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateIdle.Name = "buttonStateIdle";
            this.buttonStateIdle.Radius = 8;
            this.buttonStateIdle.Size = new System.Drawing.Size(56, 32);
            this.buttonStateIdle.TabIndex = 1;
            this.buttonStateIdle.Text = "空闲";
            this.buttonStateIdle.Type = AntdUI.TTypeMini.Default;
            this.buttonStateIdle.WaveSize = 0;
            // 
            // buttonStateOff
            // 
            this.buttonStateOff.Location = new System.Drawing.Point(0, 8);
            this.buttonStateOff.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateOff.Name = "buttonStateOff";
            this.buttonStateOff.Radius = 8;
            this.buttonStateOff.Size = new System.Drawing.Size(56, 32);
            this.buttonStateOff.TabIndex = 0;
            this.buttonStateOff.Text = "熄灭";
            this.buttonStateOff.Type = AntdUI.TTypeMini.Default;
            this.buttonStateOff.WaveSize = 0;
            // 
            // MotionActuatorActionPanelControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionActuatorActionPanelControl";
            this.Size = new System.Drawing.Size(320, 196);
            this.panelRoot.ResumeLayout(false);
            this.panelNormalActions.ResumeLayout(false);
            this.panelStackLightActions.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel panelRoot;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Panel panelNormalActions;
        private AntdUI.Button buttonPrimary;
        private AntdUI.Button buttonSecondary;
        private AntdUI.Panel panelStackLightActions;
        private AntdUI.Button buttonStateOff;
        private AntdUI.Button buttonStateIdle;
        private AntdUI.Button buttonStateRunning;
        private AntdUI.Button buttonStateWarning;
        private AntdUI.Button buttonStateAlarm;
    }
}