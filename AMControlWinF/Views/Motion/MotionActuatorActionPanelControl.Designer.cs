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
            this.gridRoot = new AntdUI.GridPanel();
            this.panelHeader = new AntdUI.Panel();
            this.flowActionButtons = new AntdUI.FlowPanel();
            this.buttonPrimary = new AntdUI.Button();
            this.buttonSecondary = new AntdUI.Button();
            this.buttonStateOff = new AntdUI.Button();
            this.buttonStateIdle = new AntdUI.Button();
            this.buttonStateRunning = new AntdUI.Button();
            this.buttonStateWarning = new AntdUI.Button();
            this.buttonStateAlarm = new AntdUI.Button();
            this.panelOptions = new AntdUI.Panel();
            this.flowOptions = new AntdUI.FlowPanel();
            this.checkStackLightWithBuzzer = new AntdUI.Checkbox();
            this.checkWaitWorkpiece = new AntdUI.Checkbox();
            this.checkWaitFeedback = new AntdUI.Checkbox();
            this.panelButtons = new AntdUI.Panel();
            this.labelSubTitle = new AntdUI.Label();
            this.labelTitle = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.gridRoot.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.flowActionButtons.SuspendLayout();
            this.panelOptions.SuspendLayout();
            this.flowOptions.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.gridRoot);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(12);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(320, 196);
            this.panelRoot.TabIndex = 0;
            // 
            // gridRoot
            // 
            this.gridRoot.Controls.Add(this.panelHeader);
            this.gridRoot.Controls.Add(this.panelOptions);
            this.gridRoot.Controls.Add(this.panelButtons);
            this.gridRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRoot.Location = new System.Drawing.Point(12, 12);
            this.gridRoot.Margin = new System.Windows.Forms.Padding(0);
            this.gridRoot.Name = "gridRoot";
            this.gridRoot.Size = new System.Drawing.Size(296, 172);
            this.gridRoot.Span = "56:100%;28:100%;fill:100%";
            this.gridRoot.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.flowActionButtons);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(0, 84);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Radius = 0;
            this.panelHeader.Size = new System.Drawing.Size(296, 88);
            this.panelHeader.TabIndex = 0;
            // 
            // flowActionButtons
            // 
            this.flowActionButtons.Align = AntdUI.TAlignFlow.Left;
            this.flowActionButtons.Controls.Add(this.buttonStateAlarm);
            this.flowActionButtons.Controls.Add(this.buttonStateWarning);
            this.flowActionButtons.Controls.Add(this.buttonStateRunning);
            this.flowActionButtons.Controls.Add(this.buttonStateIdle);
            this.flowActionButtons.Controls.Add(this.buttonStateOff);
            this.flowActionButtons.Controls.Add(this.buttonSecondary);
            this.flowActionButtons.Controls.Add(this.buttonPrimary);
            this.flowActionButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowActionButtons.Gap = 8;
            this.flowActionButtons.Location = new System.Drawing.Point(0, 0);
            this.flowActionButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowActionButtons.Name = "flowActionButtons";
            this.flowActionButtons.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.flowActionButtons.Size = new System.Drawing.Size(296, 88);
            this.flowActionButtons.TabIndex = 0;
            // 
            // buttonPrimary
            // 
            this.buttonPrimary.Location = new System.Drawing.Point(168, 48);
            this.buttonPrimary.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPrimary.Name = "buttonPrimary";
            this.buttonPrimary.Radius = 8;
            this.buttonPrimary.Size = new System.Drawing.Size(92, 36);
            this.buttonPrimary.TabIndex = 0;
            this.buttonPrimary.Text = "主操作";
            this.buttonPrimary.Type = AntdUI.TTypeMini.Primary;
            this.buttonPrimary.WaveSize = 0;
            // 
            // buttonSecondary
            // 
            this.buttonSecondary.Location = new System.Drawing.Point(68, 48);
            this.buttonSecondary.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSecondary.Name = "buttonSecondary";
            this.buttonSecondary.Radius = 8;
            this.buttonSecondary.Size = new System.Drawing.Size(92, 36);
            this.buttonSecondary.TabIndex = 1;
            this.buttonSecondary.Text = "副操作";
            this.buttonSecondary.WaveSize = 0;
            // 
            // buttonStateOff
            // 
            this.buttonStateOff.Location = new System.Drawing.Point(0, 48);
            this.buttonStateOff.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateOff.Name = "buttonStateOff";
            this.buttonStateOff.Radius = 8;
            this.buttonStateOff.Size = new System.Drawing.Size(60, 36);
            this.buttonStateOff.TabIndex = 2;
            this.buttonStateOff.Text = "熄灭";
            this.buttonStateOff.Visible = false;
            this.buttonStateOff.WaveSize = 0;
            // 
            // buttonStateIdle
            // 
            this.buttonStateIdle.Location = new System.Drawing.Point(204, 4);
            this.buttonStateIdle.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateIdle.Name = "buttonStateIdle";
            this.buttonStateIdle.Radius = 8;
            this.buttonStateIdle.Size = new System.Drawing.Size(60, 36);
            this.buttonStateIdle.TabIndex = 3;
            this.buttonStateIdle.Text = "空闲";
            this.buttonStateIdle.Visible = false;
            this.buttonStateIdle.WaveSize = 0;
            // 
            // buttonStateRunning
            // 
            this.buttonStateRunning.Location = new System.Drawing.Point(136, 4);
            this.buttonStateRunning.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateRunning.Name = "buttonStateRunning";
            this.buttonStateRunning.Radius = 8;
            this.buttonStateRunning.Size = new System.Drawing.Size(60, 36);
            this.buttonStateRunning.TabIndex = 4;
            this.buttonStateRunning.Text = "运行";
            this.buttonStateRunning.Visible = false;
            this.buttonStateRunning.WaveSize = 0;
            // 
            // buttonStateWarning
            // 
            this.buttonStateWarning.Location = new System.Drawing.Point(68, 4);
            this.buttonStateWarning.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateWarning.Name = "buttonStateWarning";
            this.buttonStateWarning.Radius = 8;
            this.buttonStateWarning.Size = new System.Drawing.Size(60, 36);
            this.buttonStateWarning.TabIndex = 5;
            this.buttonStateWarning.Text = "警告";
            this.buttonStateWarning.Type = AntdUI.TTypeMini.Warn;
            this.buttonStateWarning.Visible = false;
            this.buttonStateWarning.WaveSize = 0;
            // 
            // buttonStateAlarm
            // 
            this.buttonStateAlarm.Location = new System.Drawing.Point(0, 4);
            this.buttonStateAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateAlarm.Name = "buttonStateAlarm";
            this.buttonStateAlarm.Radius = 8;
            this.buttonStateAlarm.Size = new System.Drawing.Size(60, 36);
            this.buttonStateAlarm.TabIndex = 6;
            this.buttonStateAlarm.Text = "报警";
            this.buttonStateAlarm.Type = AntdUI.TTypeMini.Error;
            this.buttonStateAlarm.Visible = false;
            this.buttonStateAlarm.WaveSize = 0;
            // 
            // panelOptions
            // 
            this.panelOptions.Controls.Add(this.flowOptions);
            this.panelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptions.Location = new System.Drawing.Point(0, 56);
            this.panelOptions.Margin = new System.Windows.Forms.Padding(0);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Radius = 0;
            this.panelOptions.Size = new System.Drawing.Size(296, 28);
            this.panelOptions.TabIndex = 1;
            // 
            // flowOptions
            // 
            this.flowOptions.Controls.Add(this.checkStackLightWithBuzzer);
            this.flowOptions.Controls.Add(this.checkWaitWorkpiece);
            this.flowOptions.Controls.Add(this.checkWaitFeedback);
            this.flowOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowOptions.Gap = 12;
            this.flowOptions.Location = new System.Drawing.Point(0, 0);
            this.flowOptions.Margin = new System.Windows.Forms.Padding(0);
            this.flowOptions.Name = "flowOptions";
            this.flowOptions.Size = new System.Drawing.Size(296, 28);
            this.flowOptions.TabIndex = 0;
            // 
            // checkStackLightWithBuzzer
            // 
            this.checkStackLightWithBuzzer.Location = new System.Drawing.Point(212, 0);
            this.checkStackLightWithBuzzer.Margin = new System.Windows.Forms.Padding(0);
            this.checkStackLightWithBuzzer.Name = "checkStackLightWithBuzzer";
            this.checkStackLightWithBuzzer.Size = new System.Drawing.Size(76, 24);
            this.checkStackLightWithBuzzer.TabIndex = 0;
            this.checkStackLightWithBuzzer.Text = "附带蜂鸣";
            this.checkStackLightWithBuzzer.Visible = false;
            // 
            // checkWaitWorkpiece
            // 
            this.checkWaitWorkpiece.Location = new System.Drawing.Point(94, 0);
            this.checkWaitWorkpiece.Margin = new System.Windows.Forms.Padding(0);
            this.checkWaitWorkpiece.Name = "checkWaitWorkpiece";
            this.checkWaitWorkpiece.Size = new System.Drawing.Size(106, 24);
            this.checkWaitWorkpiece.TabIndex = 1;
            this.checkWaitWorkpiece.Text = "等待工件检测";
            this.checkWaitWorkpiece.Visible = false;
            // 
            // checkWaitFeedback
            // 
            this.checkWaitFeedback.Checked = true;
            this.checkWaitFeedback.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkWaitFeedback.Location = new System.Drawing.Point(0, 0);
            this.checkWaitFeedback.Margin = new System.Windows.Forms.Padding(0);
            this.checkWaitFeedback.Name = "checkWaitFeedback";
            this.checkWaitFeedback.Size = new System.Drawing.Size(82, 24);
            this.checkWaitFeedback.TabIndex = 2;
            this.checkWaitFeedback.Text = "等待反馈";
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.labelSubTitle);
            this.panelButtons.Controls.Add(this.labelTitle);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Radius = 0;
            this.panelButtons.Size = new System.Drawing.Size(296, 56);
            this.panelButtons.TabIndex = 2;
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelSubTitle.Location = new System.Drawing.Point(0, 34);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(296, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "内部名称：—";
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(296, 32);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "当前对象：未选择";
            // 
            // MotionActuatorActionPanelControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionActuatorActionPanelControl";
            this.Size = new System.Drawing.Size(320, 196);
            this.panelRoot.ResumeLayout(false);
            this.gridRoot.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.flowActionButtons.ResumeLayout(false);
            this.panelOptions.ResumeLayout(false);
            this.flowOptions.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private AntdUI.GridPanel gridRoot;
        private AntdUI.Panel panelHeader;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private AntdUI.Panel panelOptions;
        private AntdUI.FlowPanel flowOptions;
        private AntdUI.Checkbox checkStackLightWithBuzzer;
        private AntdUI.Checkbox checkWaitWorkpiece;
        private AntdUI.Checkbox checkWaitFeedback;
        private AntdUI.Panel panelButtons;
        private AntdUI.FlowPanel flowActionButtons;
        private AntdUI.Button buttonPrimary;
        private AntdUI.Button buttonSecondary;
        private AntdUI.Button buttonStateOff;
        private AntdUI.Button buttonStateIdle;
        private AntdUI.Button buttonStateRunning;
        private AntdUI.Button buttonStateWarning;
        private AntdUI.Button buttonStateAlarm;
    }
}