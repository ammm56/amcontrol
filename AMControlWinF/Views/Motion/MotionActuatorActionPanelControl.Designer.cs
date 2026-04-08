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
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new AntdUI.Label();
            this.labelSubTitle = new AntdUI.Label();
            this.flowOptions = new AntdUI.FlowPanel();
            this.checkWaitFeedback = new AntdUI.Checkbox();
            this.checkWaitWorkpiece = new AntdUI.Checkbox();
            this.checkStackLightWithBuzzer = new AntdUI.Checkbox();
            this.panelActionHost = new AntdUI.Panel();
            this.panelStackLightActions = new AntdUI.Panel();
            this.tableStackLightButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonStateOff = new AntdUI.Button();
            this.buttonStateIdle = new AntdUI.Button();
            this.buttonStateRunning = new AntdUI.Button();
            this.buttonStateWarning = new AntdUI.Button();
            this.buttonStateAlarm = new AntdUI.Button();
            this.panelNormalActions = new AntdUI.Panel();
            this.tableNormalButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPrimary = new AntdUI.Button();
            this.buttonSecondary = new AntdUI.Button();
            this.labelHint = new AntdUI.Label();
            this.panelRoot.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.flowOptions.SuspendLayout();
            this.panelActionHost.SuspendLayout();
            this.panelStackLightActions.SuspendLayout();
            this.tableStackLightButtons.SuspendLayout();
            this.panelNormalActions.SuspendLayout();
            this.tableNormalButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.tableMain);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(12);
            this.panelRoot.Radius = 0;
            this.panelRoot.Size = new System.Drawing.Size(320, 196);
            this.panelRoot.TabIndex = 0;
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.labelTitle, 0, 0);
            this.tableMain.Controls.Add(this.labelSubTitle, 0, 1);
            this.tableMain.Controls.Add(this.flowOptions, 0, 2);
            this.tableMain.Controls.Add(this.panelActionHost, 0, 3);
            this.tableMain.Controls.Add(this.labelHint, 0, 4);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(12, 12);
            this.tableMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 5;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Size = new System.Drawing.Size(296, 172);
            this.tableMain.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(296, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "当前对象：未选择";
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSubTitle.Location = new System.Drawing.Point(0, 24);
            this.labelSubTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(296, 22);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "—";
            // 
            // flowOptions
            // 
            this.flowOptions.Controls.Add(this.checkWaitFeedback);
            this.flowOptions.Controls.Add(this.checkWaitWorkpiece);
            this.flowOptions.Controls.Add(this.checkStackLightWithBuzzer);
            this.flowOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowOptions.Gap = 12;
            this.flowOptions.Location = new System.Drawing.Point(0, 46);
            this.flowOptions.Margin = new System.Windows.Forms.Padding(0);
            this.flowOptions.Name = "flowOptions";
            this.flowOptions.Size = new System.Drawing.Size(296, 28);
            this.flowOptions.TabIndex = 2;
            // 
            // checkWaitFeedback
            // 
            this.checkWaitFeedback.Location = new System.Drawing.Point(206, 0);
            this.checkWaitFeedback.Margin = new System.Windows.Forms.Padding(0);
            this.checkWaitFeedback.Name = "checkWaitFeedback";
            this.checkWaitFeedback.Size = new System.Drawing.Size(82, 24);
            this.checkWaitFeedback.TabIndex = 0;
            this.checkWaitFeedback.Text = "等待反馈";
            // 
            // checkWaitWorkpiece
            // 
            this.checkWaitWorkpiece.Location = new System.Drawing.Point(88, 0);
            this.checkWaitWorkpiece.Margin = new System.Windows.Forms.Padding(0);
            this.checkWaitWorkpiece.Name = "checkWaitWorkpiece";
            this.checkWaitWorkpiece.Size = new System.Drawing.Size(106, 24);
            this.checkWaitWorkpiece.TabIndex = 1;
            this.checkWaitWorkpiece.Text = "等待工件检测";
            this.checkWaitWorkpiece.Visible = false;
            // 
            // checkStackLightWithBuzzer
            // 
            this.checkStackLightWithBuzzer.Location = new System.Drawing.Point(0, 0);
            this.checkStackLightWithBuzzer.Margin = new System.Windows.Forms.Padding(0);
            this.checkStackLightWithBuzzer.Name = "checkStackLightWithBuzzer";
            this.checkStackLightWithBuzzer.Size = new System.Drawing.Size(76, 24);
            this.checkStackLightWithBuzzer.TabIndex = 2;
            this.checkStackLightWithBuzzer.Text = "附带蜂鸣";
            this.checkStackLightWithBuzzer.Visible = false;
            // 
            // panelActionHost
            // 
            this.panelActionHost.Controls.Add(this.panelStackLightActions);
            this.panelActionHost.Controls.Add(this.panelNormalActions);
            this.panelActionHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActionHost.Location = new System.Drawing.Point(0, 74);
            this.panelActionHost.Margin = new System.Windows.Forms.Padding(0);
            this.panelActionHost.Name = "panelActionHost";
            this.panelActionHost.Radius = 0;
            this.panelActionHost.Size = new System.Drawing.Size(296, 58);
            this.panelActionHost.TabIndex = 3;
            // 
            // panelStackLightActions
            // 
            this.panelStackLightActions.Controls.Add(this.tableStackLightButtons);
            this.panelStackLightActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStackLightActions.Location = new System.Drawing.Point(0, 0);
            this.panelStackLightActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelStackLightActions.Name = "panelStackLightActions";
            this.panelStackLightActions.Radius = 0;
            this.panelStackLightActions.Size = new System.Drawing.Size(296, 58);
            this.panelStackLightActions.TabIndex = 1;
            this.panelStackLightActions.Visible = false;
            // 
            // tableStackLightButtons
            // 
            this.tableStackLightButtons.ColumnCount = 5;
            this.tableStackLightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableStackLightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableStackLightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableStackLightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableStackLightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableStackLightButtons.Controls.Add(this.buttonStateOff, 0, 0);
            this.tableStackLightButtons.Controls.Add(this.buttonStateIdle, 1, 0);
            this.tableStackLightButtons.Controls.Add(this.buttonStateRunning, 2, 0);
            this.tableStackLightButtons.Controls.Add(this.buttonStateWarning, 3, 0);
            this.tableStackLightButtons.Controls.Add(this.buttonStateAlarm, 4, 0);
            this.tableStackLightButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableStackLightButtons.Location = new System.Drawing.Point(0, 0);
            this.tableStackLightButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tableStackLightButtons.Name = "tableStackLightButtons";
            this.tableStackLightButtons.RowCount = 1;
            this.tableStackLightButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableStackLightButtons.Size = new System.Drawing.Size(296, 58);
            this.tableStackLightButtons.TabIndex = 0;
            // 
            // buttonStateOff
            // 
            this.buttonStateOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStateOff.Location = new System.Drawing.Point(0, 8);
            this.buttonStateOff.Margin = new System.Windows.Forms.Padding(0, 8, 4, 8);
            this.buttonStateOff.Name = "buttonStateOff";
            this.buttonStateOff.Radius = 8;
            this.buttonStateOff.Size = new System.Drawing.Size(55, 42);
            this.buttonStateOff.TabIndex = 0;
            this.buttonStateOff.Text = "熄灭";
            this.buttonStateOff.WaveSize = 0;
            // 
            // buttonStateIdle
            // 
            this.buttonStateIdle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStateIdle.Location = new System.Drawing.Point(63, 8);
            this.buttonStateIdle.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.buttonStateIdle.Name = "buttonStateIdle";
            this.buttonStateIdle.Radius = 8;
            this.buttonStateIdle.Size = new System.Drawing.Size(51, 42);
            this.buttonStateIdle.TabIndex = 1;
            this.buttonStateIdle.Text = "空闲";
            this.buttonStateIdle.WaveSize = 0;
            // 
            // buttonStateRunning
            // 
            this.buttonStateRunning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStateRunning.Location = new System.Drawing.Point(122, 8);
            this.buttonStateRunning.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.buttonStateRunning.Name = "buttonStateRunning";
            this.buttonStateRunning.Radius = 8;
            this.buttonStateRunning.Size = new System.Drawing.Size(51, 42);
            this.buttonStateRunning.TabIndex = 2;
            this.buttonStateRunning.Text = "运行";
            this.buttonStateRunning.WaveSize = 0;
            // 
            // buttonStateWarning
            // 
            this.buttonStateWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStateWarning.Location = new System.Drawing.Point(181, 8);
            this.buttonStateWarning.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.buttonStateWarning.Name = "buttonStateWarning";
            this.buttonStateWarning.Radius = 8;
            this.buttonStateWarning.Size = new System.Drawing.Size(51, 42);
            this.buttonStateWarning.TabIndex = 3;
            this.buttonStateWarning.Text = "警告";
            this.buttonStateWarning.WaveSize = 0;
            // 
            // buttonStateAlarm
            // 
            this.buttonStateAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStateAlarm.Location = new System.Drawing.Point(240, 8);
            this.buttonStateAlarm.Margin = new System.Windows.Forms.Padding(4, 8, 0, 8);
            this.buttonStateAlarm.Name = "buttonStateAlarm";
            this.buttonStateAlarm.Radius = 8;
            this.buttonStateAlarm.Size = new System.Drawing.Size(56, 42);
            this.buttonStateAlarm.TabIndex = 4;
            this.buttonStateAlarm.Text = "报警";
            this.buttonStateAlarm.WaveSize = 0;
            // 
            // panelNormalActions
            // 
            this.panelNormalActions.Controls.Add(this.tableNormalButtons);
            this.panelNormalActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNormalActions.Location = new System.Drawing.Point(0, 0);
            this.panelNormalActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelNormalActions.Name = "panelNormalActions";
            this.panelNormalActions.Radius = 0;
            this.panelNormalActions.Size = new System.Drawing.Size(296, 58);
            this.panelNormalActions.TabIndex = 0;
            // 
            // tableNormalButtons
            // 
            this.tableNormalButtons.ColumnCount = 2;
            this.tableNormalButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableNormalButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableNormalButtons.Controls.Add(this.buttonPrimary, 0, 0);
            this.tableNormalButtons.Controls.Add(this.buttonSecondary, 1, 0);
            this.tableNormalButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableNormalButtons.Location = new System.Drawing.Point(0, 0);
            this.tableNormalButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tableNormalButtons.Name = "tableNormalButtons";
            this.tableNormalButtons.RowCount = 1;
            this.tableNormalButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableNormalButtons.Size = new System.Drawing.Size(296, 58);
            this.tableNormalButtons.TabIndex = 0;
            // 
            // buttonPrimary
            // 
            this.buttonPrimary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPrimary.Location = new System.Drawing.Point(0, 8);
            this.buttonPrimary.Margin = new System.Windows.Forms.Padding(0, 8, 6, 8);
            this.buttonPrimary.Name = "buttonPrimary";
            this.buttonPrimary.Radius = 8;
            this.buttonPrimary.Size = new System.Drawing.Size(142, 42);
            this.buttonPrimary.TabIndex = 0;
            this.buttonPrimary.Text = "主操作";
            this.buttonPrimary.Type = AntdUI.TTypeMini.Primary;
            this.buttonPrimary.WaveSize = 0;
            // 
            // buttonSecondary
            // 
            this.buttonSecondary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSecondary.Location = new System.Drawing.Point(154, 8);
            this.buttonSecondary.Margin = new System.Windows.Forms.Padding(6, 8, 0, 8);
            this.buttonSecondary.Name = "buttonSecondary";
            this.buttonSecondary.Radius = 8;
            this.buttonSecondary.Size = new System.Drawing.Size(142, 42);
            this.buttonSecondary.TabIndex = 1;
            this.buttonSecondary.Text = "副操作";
            this.buttonSecondary.WaveSize = 0;
            // 
            // labelHint
            // 
            this.labelHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHint.Location = new System.Drawing.Point(0, 132);
            this.labelHint.Margin = new System.Windows.Forms.Padding(0);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(296, 40);
            this.labelHint.TabIndex = 4;
            this.labelHint.Text = "请先在左侧选择一个执行器对象。";
            // 
            // MotionActuatorActionPanelControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelRoot);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MotionActuatorActionPanelControl";
            this.Size = new System.Drawing.Size(320, 196);
            this.panelRoot.ResumeLayout(false);
            this.tableMain.ResumeLayout(false);
            this.flowOptions.ResumeLayout(false);
            this.panelActionHost.ResumeLayout(false);
            this.panelStackLightActions.ResumeLayout(false);
            this.tableStackLightButtons.ResumeLayout(false);
            this.panelNormalActions.ResumeLayout(false);
            this.tableNormalButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private AntdUI.Panel panelRoot;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private AntdUI.Label labelTitle;
        private AntdUI.Label labelSubTitle;
        private AntdUI.FlowPanel flowOptions;
        private AntdUI.Checkbox checkWaitFeedback;
        private AntdUI.Checkbox checkWaitWorkpiece;
        private AntdUI.Checkbox checkStackLightWithBuzzer;
        private AntdUI.Panel panelActionHost;
        private AntdUI.Panel panelNormalActions;
        private System.Windows.Forms.TableLayoutPanel tableNormalButtons;
        private AntdUI.Button buttonPrimary;
        private AntdUI.Button buttonSecondary;
        private AntdUI.Panel panelStackLightActions;
        private System.Windows.Forms.TableLayoutPanel tableStackLightButtons;
        private AntdUI.Button buttonStateOff;
        private AntdUI.Button buttonStateIdle;
        private AntdUI.Button buttonStateRunning;
        private AntdUI.Button buttonStateWarning;
        private AntdUI.Button buttonStateAlarm;
        private AntdUI.Label labelHint;
    }
}